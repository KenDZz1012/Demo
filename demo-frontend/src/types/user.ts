export interface AuthUser {
    id: string;
    username: string;
    email: string;
    avatarUrl?: string;
    displayName?: string;
    status?: string;
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

export interface CancelFriendRequest {
    userID: string;
    friendID: string;
}

export interface UpdateUserRelationship {
    userID: string;
    friendID: string;
    status: string
}

export interface CreateUserInput {
    userName: string;
    email: string;
    passwordHash: string;
    displayName?: string;
    dateOfBirth?: string;
    isAdmin?: boolean;
}

export interface CreateUserRelationshipResponse {
    id: string;
    userName: string;
    displayName: string;
    avatarUrl: string;
}
