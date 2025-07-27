using Authorize.Application.Models;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Keycloak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorize.Application.Features.Logout.Commands.LogoutCommand
{
    public class LogoutHandler : IRequestHandler<Logout, ApiResponse<bool>>
    {
        private readonly IKeycloakService _keycloakService;

        public LogoutHandler(IKeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        public async Task<ApiResponse<bool>> Handle(Logout request, CancellationToken cancellationToken)
        {
            try
            {
                bool isLogoutSuccess = await _keycloakService.LogoutAsync(request.RefreshToken);
                return isLogoutSuccess
                    ? ApiResponse<bool>.Success(true, "Logout successful")
                    : ApiResponse<bool>.Failure("500", "Logout failed");
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ApiResponse<bool>.Failure("500", ex.Message));
            }
        }
    }
}
