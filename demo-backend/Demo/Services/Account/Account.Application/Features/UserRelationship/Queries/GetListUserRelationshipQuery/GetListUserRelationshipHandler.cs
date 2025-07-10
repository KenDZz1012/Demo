using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;

namespace Account.Application.Features.UserRelationship.Queries.GetListUserRelationshipQuery
{
    public class GetListUserRelationshipHandler : IRequestHandler<GetListUserRelationship, ApiResponse<List<GetListUserRelationshipVm>>>
    {
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;

        public GetListUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<GetListUserRelationshipVm>>> Handle(GetListUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRelationshipRepository.GetAllAsync(request);
                return ApiResponse<List<GetListUserRelationshipVm>>.Success(_mapper.Map<List<GetListUserRelationshipVm>>(users), "Lấy danh sách thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<GetListUserRelationshipVm>>.Failure("500", ex.Message);
            }
        }
    }
}
