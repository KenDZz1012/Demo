import { RootState } from 'app/store'; // chỉnh path nếu cần

export const selectAuthUser = (state: RootState) => state.auth.user;
export const selectServerId = (state: RootState) => state.server.selectedServerId;
export const selectServers = (state: RootState) => state.server.servers;
export const selectFriends = (state: RootState) => state.userRelationship.friends;