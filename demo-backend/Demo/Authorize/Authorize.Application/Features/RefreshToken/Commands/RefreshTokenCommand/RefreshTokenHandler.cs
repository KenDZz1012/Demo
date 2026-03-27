using Authorize.Application.Contracts.Persistence;
using Authorize.Application.Models;
using AutoMapper;
using MediatR;
using Newtonsoft.Json;
using Service.Lib.BaseResponse;
using Service.Lib.Keycloak;


namespace Authorize.Application.Features.RefreshToken.Commands.RefreshTokenCommand
{
    public class RefreshTokenHandler : IRequestHandler<RefreshToken, ApiResponse<RefreshTokenResponse>>
    {
        private readonly IRefreshTokenRepository _repository;
        private readonly IMapper _mapper;
        private readonly IKeycloakService _keycloakService;

        public RefreshTokenHandler(IRefreshTokenRepository repository, IMapper mapper, IKeycloakService keycloakService)
        {
            _repository = repository;
            _mapper = mapper;
            _keycloakService = keycloakService;
        }

        public async Task<ApiResponse<RefreshTokenResponse>> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
