using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Channel.Application.Contracts.Persistence;
using MediatR;
using Service.Lib.BaseResponse;

namespace Channel.Application.Features.ServerMember.Commands.CreateServerMember
{
    public class CreateServerMemberHandler : IRequestHandler<CreateServerMember, ApiResponse<Guid>>
    {
        private readonly IServerMemberRepository _serverMemberRepository;
        private readonly IMapper _mapper;
        public CreateServerMemberHandler(IServerMemberRepository serverMemberRepository, IMapper mapper)
        {
            _serverMemberRepository = serverMemberRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(CreateServerMember request, CancellationToken cancellationToken)
        {
            try
            {
                var serverMember = _mapper.Map<Domain.Entities.ServerMember>(request);

                var isCreatedSuccess = await _serverMemberRepository.AddAsync(serverMember);

                return isCreatedSuccess
                    ? ApiResponse<Guid>.Success(serverMember.Id, "Create servermember successfully")
                    : ApiResponse<Guid>.Failure("500", "Create servermember failed");
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
