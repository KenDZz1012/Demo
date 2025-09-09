using DirectMessage.Application.Contracts.Persistence;
using DirectMessage.Domain.Entities;
using MediatR;
using Service.Lib.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectMessage.Application.Features.DirectMessage.Commands.SendMessage
{
    public class SendMessageHandler : IRequestHandler<SendMessage, ApiResponse<string>>
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IDirectMessageRepository _directMessageRepository;
        public SendMessageHandler(IParticipantRepository participantRepository, IConversationRepository conversationRepository)
        {
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
        }
        public async Task<ApiResponse<string>> Handle(SendMessage request, CancellationToken cancellationToken)
        {
            try
            {
                Guid conversationId = Guid.Empty;

                if (request.recipientIds.Count == 1)
                {
                    var recipientId = request.recipientIds[0];
                    var senderConversations = await _participantRepository.CheckUserConversation(request.senderId);
                    DirectMessageConversation? existingConversation = null;
                    foreach (var conv in senderConversations)
                    {
                        var recipent = await _participantRepository.CheckUserConversation(recipientId, conv.ConversationId);
                        if (recipent != null)
                        {
                            var conversation = await _conversationRepository.CheckExistConversation(conv.ConversationId);
                            if (conversation != null && conversation.Type == "direct")
                            {
                                existingConversation = conversation;
                                break;
                            }
                        }
                    }

                    if (existingConversation != null)
                    {
                        conversationId = existingConversation.ConversationId;
                    }
                    else
                    {
                        conversationId = Guid.NewGuid();
                        await _conversationRepository.AddAsync(new DirectMessageConversation()
                        {
                            ConversationId = conversationId,
                            Type = "direct",
                            Name = null,
                            CreatedAt = DateTimeOffset.UtcNow,
                        });

                        await _participantRepository.AddAsync(new DirectMessageParticipant()
                        {
                            ConversationId = conversationId,
                            UserId = request.senderId,
                            JoinedAt = DateTimeOffset.UtcNow,
                            Role = "member"
                        });

                        await _participantRepository.AddAsync(new DirectMessageParticipant
                        {
                            ConversationId = conversationId,
                            UserId = recipientId,
                            JoinedAt = DateTimeOffset.UtcNow,
                            Role = "member"
                        });

                    }
                }
                else { }

                var messageId = Guid.NewGuid().ToString();
                await _directMessageRepository.AddAsync(new Domain.Entities.DirectMessage()
                {
                    ConversationId = conversationId,
                    MessageId = messageId,
                    SenderId = request.senderId,
                    Content = request.content,
                    CreatedAt = DateTimeOffset.UtcNow
                });
                return ApiResponse<string>.Success(messageId);

            }
            catch (Exception ex)
            {
                return await Task.FromResult(ApiResponse<string>.Failure("500", ex.Message));
            }
        }
    }
}
