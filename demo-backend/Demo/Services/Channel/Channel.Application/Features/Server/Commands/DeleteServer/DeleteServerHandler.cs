using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel.Application.Contracts.Persistence;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.Server.Commands.DeleteServer
{
    public class DeleteServerHandler : IRequestHandler<DeleteServer, ApiResponse<bool>>
    {
        public IServerRepository _serverRepository { get; set; }
        public DeleteServerHandler(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteServer request, CancellationToken cancellationToken)
        {
            try
            {
                var existingServer = await _serverRepository.CheckExistServer(request.Id);
                if(existingServer == null) return ApiResponse<bool>.Failure("404", "Server not found");
                var isDeleted = await _serverRepository.DeleteAsync(existingServer);
                return isDeleted 
                    ? ApiResponse<bool>.Success(isDeleted, "Delete server successfully") 
                    : ApiResponse<bool>.Failure("500", "Delete server failed");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Failure("500", ex.Message);
            }
        }
    }
}
