using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Commands.DeleteUserRelationshipCommand
{
    public class DeleteUserRelationshipHandler : IRequestHandler<DeleteUserRelationship, ApiResponse<Guid>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;

        public DeleteUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<Guid>> Handle(DeleteUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var exitRelationship = await _userRelationshipRepository.GetByIdAsync(request.ID);
                if (exitRelationship != null)
                {
                    var isDeletedSuccess = await _userRelationshipRepository.DeleteAsync(exitRelationship);
                    return isDeletedSuccess ? ApiResponse<Guid>.Success(exitRelationship.ID, "Xóa thành công") : ApiResponse<Guid>.Failure("500", "Không xóa được");
                }
                else
                {
                    return ApiResponse<Guid>.Failure("500", "Không tồn tại mối quan hệ này");
                }
            }
            catch(Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));

            }
        }
    }
}
