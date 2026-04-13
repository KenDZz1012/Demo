using MediatR;
using Service.Lib.BaseResponse;

namespace Authorize.Application.Features.User.Commands.CreateIdentityUserCommand;

/// <summary>
/// Tạo user trong ASP.NET Identity (DB Authorize) — dùng cho đăng ký / đồng bộ với Account (cùng UserId nếu truyền).
/// </summary>
public sealed class CreateIdentityUser : IRequest<ApiResponse<Guid>>
{
    /// <summary>Nếu có, gán trùng <see cref="Account"/> user Id để ProfileService / token <c>sub</c> khớp.</summary>
    public Guid? UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? DisplayName { get; set; }

    public string? AvatarUrl { get; set; }

    public string AccountStatus { get; set; } = "active";
}
