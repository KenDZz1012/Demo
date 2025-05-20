using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.UpdateStatusCommand
{
    public class UpdateStatusHandler : IRequestHandler<UpdateStatus, ApiResponse<Guid>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;

        public UpdateStatusHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(UpdateStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var userRelationship = await _userRelationshipRepository.GetByIdAsync(request.ID);
                if (userRelationship != null)
                {
                    userRelationship.Status = request.Status;
                    var isUpdatedSuccess = await _userRelationshipRepository.UpdateAsync(userRelationship);
                    return isUpdatedSuccess ? ApiResponse<Guid>.Success(userRelationship.Id, "Update thành công") : ApiResponse<Guid>.Failure("500", "Không Update được");
                }
                else
                {
                    return ApiResponse<Guid>.Failure("500", "Không tồn tại");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
