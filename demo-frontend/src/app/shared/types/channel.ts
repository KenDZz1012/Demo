export interface Channel {
  id: string;
  name: string;
  type: string;
}

export interface CreateChannel {
  name: string;
  serverId: string;
  type: string;
}
