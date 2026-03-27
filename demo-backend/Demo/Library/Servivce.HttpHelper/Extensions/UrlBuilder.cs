using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Servivce.HttpHelper.Extensions;

public static class UrlBuilder
{
    private static readonly Regex TokenRegex = new(@"\{(\w+)\}", RegexOptions.Compiled);

    private static readonly Regex AbsolutePrefixRegex =
        new(@"^([a-zA-Z][a-zA-Z0-9+\-.]*://[^/]+)(/.*)?$", RegexOptions.Compiled);

    /// <summary>
    /// API hợp nhất. Hỗ trợ:
    /// - routeTemplate: "{id}/print-infomation", "{slug}/{id}/x", "http://host/api/{id}/x", "print?copy=true"
    /// - param: scalar/list/object
    /// </summary>
    public static string Build<TParam>(string? routeTemplate = null, TParam? param = default,
        bool preferPathForScalars = true, string queryKeyForScalar = "value", bool strict = false,
        string? scalarTokenName = null)
    {
        // Không có param: giữ nguyên template (kể cả absolute) + query sẵn có
        if (param is null)
        {
            SplitTemplate(routeTemplate ?? string.Empty, out var basePrefix, out var path, out var existing);
            var qs = ToQueryStringFromNameValueCollection(existing);
            return CombineUrl(basePrefix, path, qs);
        }

        // Không có template: dùng ToUrlSuffix cũ
        if (string.IsNullOrWhiteSpace(routeTemplate))
            return ToUrlSuffix(param!, preferPathForScalars, queryKeyForScalar);

        // Có template
        var type = param!.GetType();

        if (IsScalar(type))
        {
            if (HasAnyToken(routeTemplate))
            {
                // Nếu chỉ định token, dùng token đó; nếu không, nhét vào token đầu tiên
                var token = !string.IsNullOrWhiteSpace(scalarTokenName) && HasToken(routeTemplate, scalarTokenName!)
                    ? scalarTokenName!
                    : FirstToken(routeTemplate);

                if (!string.IsNullOrEmpty(token))
                    return BuildFromTemplate(routeTemplate, new Dictionary<string, object?> { [token] = param }, null,
                        strict);
            }

            // Không có token → nối theo scalar mode
            var suffix = ToUrlSuffix(param!, preferPathForScalars, queryKeyForScalar);
            SplitTemplate(routeTemplate, out var basePrefix, out var path, out var existing);
            var baseQs = ToQueryStringFromNameValueCollection(existing);
            var joined = JoinQueryIntoBase(suffix, hasBaseQuery: baseQs.Length > 0);
            return CombineUrl(basePrefix, path, baseQs) + joined;
        }

        if (param is IEnumerable && param is not string)
        {
            var suffix = ToUrlSuffix(param!, preferPathForScalars, queryKeyForScalar);
            SplitTemplate(routeTemplate, out var basePrefix, out var path, out var existing);
            var baseQs = ToQueryStringFromNameValueCollection(existing);
            var joined = JoinQueryIntoBase(suffix, hasBaseQuery: baseQs.Length > 0);
            return CombineUrl(basePrefix, path, baseQs) + joined;
        }

        // Object → SmartFromTemplate
        return SmartFromTemplate(routeTemplate, param!, strict);
    }

    public static string BuildFromTemplate(string routeTemplate, object? routeValues = null, object? query = null,
        bool strict = false)
    {
        if (string.IsNullOrWhiteSpace(routeTemplate))
            return string.Empty;

        SplitTemplate(routeTemplate, out var basePrefix, out var templatePath, out var existingQueryNvc);

        var map = ToDictionary(routeValues);
        string replaced = TokenRegex.Replace(templatePath, m =>
        {
            var key = m.Groups[1].Value;
            if (!map.TryGetValue(key, out var raw) || raw is null)
            {
                if (strict)
                    throw new ArgumentException($"Missing route value for '{key}'.");
                return string.Empty;
            }

            var segment = IsScalar(raw.GetType()) ? FormatScalar(raw) : raw.ToString() ?? string.Empty;
            return HttpUtility.UrlEncode(segment);
        });

        var path = NormalizeSlashes(replaced);

        var merged = MergeQuery(existingQueryNvc, query);
        var qs = ToQueryStringFromNameValueCollection(merged);

        return CombineUrl(basePrefix, path, qs);
    }

    public static string SmartFromTemplate(string routeTemplate, object? values, bool strict = false)
    {
        SplitTemplate(routeTemplate, out var basePrefix, out var templatePath, out var existingQueryNvc);

        var all = ToDictionary(values);
        var tokenNames = TokenRegex.Matches(templatePath ?? string.Empty).Cast<Match>().Select(m => m.Groups[1].Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var routeDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        var queryDict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        foreach (var kv in all)
        {
            if (tokenNames.Contains(kv.Key))
                routeDict[kv.Key] = kv.Value;
            else
                queryDict[kv.Key] = kv.Value;
        }

        // Build path (không merge query ở đây)
        var basePath = BuildFromTemplate(templatePath, routeDict, null, strict);

        // Merge query: existing trong template + queryDict
        var merged = MergeQuery(existingQueryNvc, queryDict.Count == 0 ? null : queryDict);
        var qs = ToQueryStringFromNameValueCollection(merged);

        // basePath hiện đang là "/path..." – nhưng cần trả về kèm basePrefix
        // Extract again from basePath to get its path part:
        var finalPath = basePath.TrimStart('/'); // vì basePrefix sẽ thêm '/'
        return CombineUrl(basePrefix, finalPath, qs);
    }

    public static string ToUrlSuffix(object? param, bool preferPathForScalars = true,
        string queryKeyForScalar = "value")
    {
        if (param is null) return string.Empty;

        if (param is string || param.GetType().IsPrimitive || param is Guid)
        {
            var value = HttpUtility.UrlEncode(param.ToString());
            return $"{value}";
        }

        if (IsScalar(param.GetType()))
        {
            var s = FormatScalar(param);
            var encoded = HttpUtility.UrlEncode(s);

            return preferPathForScalars ? $"/{encoded}" : $"?{HttpUtility.UrlEncode(queryKeyForScalar)}={encoded}";
        }

        if (param is IEnumerable en && param is not string)
        {
            if (preferPathForScalars)
            {
                var segments = new List<string>();
                foreach (var item in en)
                {
                    if (item is null) continue;
                    var s = FormatScalar(item);
                    segments.Add(HttpUtility.UrlEncode(s));
                }

                return segments.Count > 0 ? "/" + string.Join("/", segments) : string.Empty;
            }
            else
            {
                var pairs = new List<string>();
                var key = HttpUtility.UrlEncode(queryKeyForScalar);
                foreach (var item in en)
                {
                    if (item is null) continue;
                    var s = HttpUtility.UrlEncode(FormatScalar(item));
                    pairs.Add($"{key}={s}");
                }

                return pairs.Count > 0 ? "?" + string.Join("&", pairs) : string.Empty;
            }
        }

        return ToQueryStringFromObject(param);
    }

    // ===== Helpers =====

    private static bool IsScalar(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(Guid) ||
               t == typeof(DateTime) || t == typeof(DateTimeOffset) || t == typeof(TimeSpan);
    }

    private static string FormatScalar(object value)
    {
        switch (value)
        {
            case null:
                return string.Empty;
            case bool b:
                return b ? "true" : "false";
            case decimal m:
                return m.ToString(CultureInfo.InvariantCulture);
            case float f:
                return f.ToString(CultureInfo.InvariantCulture);
            case double d:
                return d.ToString(CultureInfo.InvariantCulture);
            case DateTime dt:
                return dt.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            case DateTimeOffset dto:
                return dto.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            case TimeSpan ts:
                return ts.ToString("c", CultureInfo.InvariantCulture);
            case Enum e:
                return Convert.ToInt64(e, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
            default:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        }
    }

    private static string ToQueryStringFromObject(object obj)
    {
        var pairs = new List<string>();

        foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value = prop.GetValue(obj);
            if (value is null) continue;

            var camel = ToCamelCase(prop.Name);
            var name = HttpUtility.UrlEncode(camel);

            if (value is IEnumerable en && value is not string)
            {
                foreach (var item in en)
                {
                    if (item is null) continue;
                    var v = HttpUtility.UrlEncode(IsScalar(item.GetType()) ? FormatScalar(item) : item.ToString());
                    pairs.Add($"{name}={v}");
                }
            }
            else
            {
                var v = HttpUtility.UrlEncode(IsScalar(value.GetType()) ? FormatScalar(value) : value.ToString());
                pairs.Add($"{name}={v}");
            }
        }

        return pairs.Count > 0 ? "?" + string.Join("&", pairs) : string.Empty;
    }

    private static Dictionary<string, object?> ToDictionary(object? obj)
    {
        var dict = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

        if (obj is null) return dict;

        if (obj is IDictionary<string, object?> dso)
        {
            foreach (var kv in dso) dict[kv.Key] = kv.Value;
            return dict;
        }

        foreach (var prop in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            dict[prop.Name] = prop.GetValue(obj);

        return dict;
    }

    private static string NormalizeSlashes(string path)
    {
        if (string.IsNullOrEmpty(path)) return path;
        while (path.Contains("//"))
            path = path.Replace("//", "/");
        return path.Trim('/');
    }

    private static bool HasAnyToken(string? tpl) => !string.IsNullOrWhiteSpace(tpl) && TokenRegex.IsMatch(tpl!);

    private static bool HasToken(string tpl, string tokenName) =>
        TokenRegex.Matches(tpl ?? string.Empty).Cast<Match>().Any(m =>
            string.Equals(m.Groups[1].Value, tokenName, StringComparison.OrdinalIgnoreCase));

    private static string? FirstToken(string tpl) =>
        TokenRegex.Matches(tpl ?? string.Empty).Cast<Match>().Select(m => m.Groups[1].Value).FirstOrDefault();

    /// <summary>
    /// Phân tách template thành: basePrefix (absolute prefix), path template, và existing query.
    /// Ví dụ:
    /// - "http://h/api/{id}/x?copy=true" => basePrefix="http://h", path="api/{id}/x", query="copy=true"
    /// - "{id}/x?y=1" => basePrefix="", path="{id}/x", query="y=1"
    /// - "print" => basePrefix="", path="print", query=""
    /// </summary>
    private static void SplitTemplate(string routeTemplate, out string basePrefix, out string templatePath,
        out NameValueCollection existingQuery)
    {
        basePrefix = string.Empty;
        existingQuery = HttpUtility.ParseQueryString(string.Empty);

        var input = routeTemplate?.Trim() ?? string.Empty;

        // Match absolute prefix
        var m = AbsolutePrefixRegex.Match(input);
        string rest;
        if (m.Success)
        {
            basePrefix = m.Groups[1].Value; // e.g., http://host:port
            rest = m.Groups[2].Success ? m.Groups[2].Value : string.Empty; // includes leading '/'
        }
        else
        {
            rest = input;
        }

        // Split query
        var idxQ = rest.IndexOf('?', StringComparison.Ordinal);
        string pathPart = idxQ >= 0 ? rest[..idxQ] : rest;
        var queryPart = idxQ >= 0 ? rest[(idxQ + 1)..] : string.Empty;

        // normalize pathPart (remove leading '/')
        pathPart = pathPart.Trim();
        if (pathPart.StartsWith("/")) pathPart = pathPart[1..];
        templatePath = pathPart.Trim('/');

        if (!string.IsNullOrEmpty(queryPart))
            existingQuery = HttpUtility.ParseQueryString(queryPart);
    }

    private static string CombineUrl(string basePrefix, string path, string query)
    {
        var sb = new StringBuilder();

        if (!string.IsNullOrEmpty(basePrefix))
        {
            sb.Append(basePrefix);
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.StartsWith("/")) sb.Append('/');
                sb.Append(path);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (!path.StartsWith("/")) sb.Append('/');
                sb.Append(path);
            }
        }

        if (!string.IsNullOrEmpty(query))
        {
            if (sb.Length == 0) sb.Append('/');
            sb.Append(query);
        }

        return sb.ToString();
    }

    private static NameValueCollection MergeQuery(NameValueCollection existing, object? extra)
    {
        var merged = HttpUtility.ParseQueryString(string.Empty);
        foreach (string? key in existing)
        {
            if (key == null) continue;
            foreach (var val in existing.GetValues(key) ?? Array.Empty<string>())
                merged.Add(ToCamelCase(key), val);
        }

        if (extra is null) return merged;

        if (extra is IDictionary<string, object?> dict)
        {
            foreach (var kv in dict)
                AddToNvc(merged, ToCamelCase(kv.Key), kv.Value);
        }
        else
        {
            foreach (var prop in extra.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var camel = ToCamelCase(prop.Name);
                AddToNvc(merged, camel, prop.GetValue(extra));
            }
        }

        return merged;
    }


    private static void AddToNvc(NameValueCollection nvc, string key, object? value)
    {
        if (value is null) return;
        key = ToCamelCase(key);

        if (value is IEnumerable en && value is not string)
        {
            foreach (var item in en)
            {
                if (item is null) continue;
                nvc.Add(key, IsScalar(item.GetType()) ? FormatScalar(item) : item.ToString());
            }
        }
        else
        {
            nvc.Add(key, IsScalar(value.GetType()) ? FormatScalar(value) : value.ToString());
        }
    }


    private static string ToQueryStringFromNameValueCollection(NameValueCollection nvc)
    {
        if (nvc.Count == 0) return string.Empty;

        var sb = new StringBuilder("?");
        bool first = true;
        foreach (string? key in nvc)
        {
            if (key == null) continue;
            var values = nvc.GetValues(key) ?? Array.Empty<string>();
            foreach (var v in values)
            {
                if (!first) sb.Append('&');
                sb.Append(HttpUtility.UrlEncode(key));
                sb.Append('=');
                sb.Append(HttpUtility.UrlEncode(v ?? string.Empty));
                first = false;
            }
        }

        return sb.ToString();
    }

    private static string JoinQueryIntoBase(string suffix, bool hasBaseQuery)
    {
        if (string.IsNullOrEmpty(suffix)) return string.Empty;
        if (!suffix.StartsWith("?")) return suffix;
        return hasBaseQuery ? "&" + suffix[1..] : suffix;
    }

    private static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            return input;
        if (input.Length == 1)
            return input.ToLowerInvariant();
        return char.ToLowerInvariant(input[0]) + input.Substring(1);
    }
}