import { combineReducers } from '@reduxjs/toolkit';
import authReducer from 'features/auth/authSlice';
import serverReducer from 'features/server/serverSlice';
import userRelationshipReducer from 'features/user-relationship/userRelationshipSlice';

const rootReducer = combineReducers({
    auth: authReducer,
    server: serverReducer,
    userRelationship: userRelationshipReducer
});
export default rootReducer;
