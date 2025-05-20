using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Password;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationshipHandler : IRequestHandler<CreateUserRelationship, ApiResponse<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRelationshipRepository _userRelationshipRepository;
        private readonly IMapper _mapper;

        public CreateUserRelationshipHandler(IUserRelationshipRepository userRelationshipRepository, IMapper mapper, IUserRepository userRepository)
        {
            _userRelationshipRepository = userRelationshipRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<Guid>> Handle(CreateUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var Addressee = await _userRepository.CheckExistUserName(request.AddresseeName);
                if(Addressee == null)
                {
                    return ApiResponse<Guid>.Failure("500", "Không tồn tại người dùng này");
                }
                var exitRelationship = await _userRelationshipRepository.CheckExistRelationship(request.RequesterId, Addressee.Id);
                if (exitRelationship == null)
                {
                    var userRelationship = _mapper.Map<Account.Domain.Entities.UserRelationship>(request);
                    userRelationship.AddresseeId = Addressee.Id;
                    var isCreatedSuccess = await _userRelationshipRepository.AddAsync(userRelationship);
                    return isCreatedSuccess ? ApiResponse<Guid>.Success(userRelationship.Id, "Thêm thành công") : ApiResponse<Guid>.Failure("500", "Không thêm được");
                }
                else
                {
                    return ApiResponse<Guid>.Failure("500", "Đã tồn tại mối quan hệ này");
                }      
            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<Guid>.Failure("500", ex.Message));
            }
        }
    }
}
