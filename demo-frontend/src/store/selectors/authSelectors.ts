import { RootState } from 'app/store';

//Authentication Selectors
export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated;
export const selectAuthUser = (state: RootState) => state.auth.user;

//Server Selectors
export const selectServerId = (state: RootState) => state.server.selectedServerId;
export const selectServers = (state: RootState) => state.server.servers;
export const selectServer = (state: RootState) => state.server.selectedServer;

//User Relationship Selectors
export const selectFriends = (state: RootState) => state.userRelationship.friends;
export const selectFriendsPending = (state: RootState) => state.userRelationship.friensPending;
export const selectedFriendId = (state: RootState) => state.userRelationship.selectedFriendId;
export const selectedFriend = (state: RootState) => state.userRelationship.selectedFriend;