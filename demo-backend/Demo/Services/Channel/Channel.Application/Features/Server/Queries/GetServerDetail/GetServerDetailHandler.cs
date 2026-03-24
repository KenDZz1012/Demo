using AutoMapper;
using Channel.Application.Contracts.Persistence;
using Channel.Application.Features.Server.Queries.GetServers;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Channel.Application.Features.Server.Queries.GetServerDetail
{
    public class GetServerDetailHandler : IRequestHandler<GetServerDetail, ApiResponse<GetServerDetailVm>>
    {
        public readonly IServerRepository _serverRepository;
        public readonly IMapper _mapper;

        public GetServerDetailHandler(IServerRepository serverRepository, IMapper mapper)
        {
            _serverRepository = serverRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<GetServerDetailVm>> Handle(GetServerDetail request, CancellationToken cancellationToken)
        {
            try
            {
                var server = await _serverRepository.GetServer(request.Id);
                if (server == null) return ApiResponse<GetServerDetailVm>.Failure("404", "Server not found");
                var serverDto = _mapper.Map<GetServerDetailVm>(server);
                var userIds = serverDto.ServerMembers
                    .Select(m => m.UserId.ToString())
                    .Distinct()
                    .ToList();

                return ApiResponse<GetServerDetailVm>.Success(serverDto, "Get server successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<GetServerDetailVm>.Failure("500", ex.Message);
            }
        }
    }
}
