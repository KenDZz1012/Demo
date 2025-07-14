export interface Server {
    id: string;
    name: string;
    ownerId: string;
    iconUrl?: string;
    channels: Channel[];
    serverMembers: ServerMember[];
}

export interface Channel {
    id: string;
    name: string;
    type: string
}

export interface ServerMember {
    id: string;
    userId: string;
    userName: string;
    avatarUrl?: string;
    role: string; // e.g., 'admin', 'member'
    displayName?: string;
    email?: string;
}

export interface CreateServer {
    name: string;
    iconUrl?: string;
    ownerId?: string;
}