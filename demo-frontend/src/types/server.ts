import { Channel } from "./channel";

export interface Server {
    id: string;
    name: string;
    ownerId: string;
    iconUrl?: string;
}


export interface ServerMember {
    id: string;
    userId: string;
    userName: string;
    avatarUrl?: string;
    role: string;
    displayName?: string;
    email?: string;
}

export interface ServerDetail {
    id: string;
    name: string;
    ownerId: string;
    iconUrl?: string;
    channels: Channel[];
    serverMembers: ServerMember[];
}

export interface CreateServer {
    name: string;
    iconUrl?: string;
    ownerId?: string;
}

export interface JoinServerByInviteLinkRequest {
    Code: string;
    UserId: string;
}

