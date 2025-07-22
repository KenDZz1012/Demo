import { RootState } from 'app/store'; // chỉnh path nếu cần

export const selectAuthUser = (state: RootState) => state.auth.user;
