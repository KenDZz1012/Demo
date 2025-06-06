using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.HttpRequest
{
    public interface IHttpRequestService
    {
        Task<TResponse> GetAsync<TResponse>(string endpoint, string token)
            where TResponse : new();
    }
}
