using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Account.Application.Contracts.Persistence;
using AutoMapper;
using MediatR;
using Service.Lib.BaseResponse;
using Service.Lib.Password;

namespace Account.Application.Features.UserRelationship.Commands.CreateUserRelationshipCommand
{
    public class CreateUserRelationshipHandler : IRequestHandler<CreateUserRelationship, ApiResponse<CreateUserRelationshipResponse>>
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
        public async Task<ApiResponse<CreateUserRelationshipResponse>> Handle(CreateUserRelationship request, CancellationToken cancellationToken)
        {
            try
            {
                var addressee = await _userRepository.CheckExistUserName(request.AddresseeName);
                if (addressee == null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("404", "User does not exist");

                var requester = await _userRepository.GetByIdAsync(request.RequesterId);
                if (requester == null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("404", "User does not exist");

                if (requester.UserName == request.AddresseeName)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("400", "Cannot send friend request to yourself");

                var existing = await _userRelationshipRepository.CheckExistRelationship(request.RequesterId, addressee.Id);
                if (existing != null)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("409", "Relationship already exists");

                var userRelationship = _mapper.Map<Account.Domain.Entities.UserRelationship>(request);
                userRelationship.AddresseeId = addressee.Id;

                var isCreated = await _userRelationshipRepository.AddAsync(userRelationship);
                if (!isCreated)
                    return ApiResponse<CreateUserRelationshipResponse>.Failure("500", "Failed to create relationship");

                var userReceived = _mapper.Map<CreateUserRelationshipResponse>(addressee);
                return ApiResponse<CreateUserRelationshipResponse>.Success(userReceived, "Friend request sent");
            }
            catch (Exception ex)
            {
                return ApiResponse<CreateUserRelationshipResponse>.Failure("500", ex.Message);
            }
        }
    }
}


