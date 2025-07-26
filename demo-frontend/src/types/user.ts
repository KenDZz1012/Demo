export interface AuthUser {
    id: string;
    username: string;
    email: string;
    avatarUrl?: string;
}

export interface Friend {
    id: string;
    userName: string;
    displayName: string;
    avatarUrl?: string;
    isOnline: boolean
}

export interface FriendPending {
    id: string;
    userName: string;
    displayName: string;
    avatarUrl?: string;
    isSender: boolean
}

export interface AddFriendRequest {
    requesterId: string;
    addresseeName: string;
}

export interface CreateUserInput {
    userName: string;
    email: string;
    passwordHash: string;
    displayName?: string;
    dateOfBirth?: string;
    isAdmin?: boolean;
}

