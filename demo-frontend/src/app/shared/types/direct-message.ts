export interface SendMessageRequest {
  senderId: string;
  recipientIds: [string];
  content: string;
}
