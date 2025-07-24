// features/server/serverSlice.ts

import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Server } from 'types';

interface ServerState {
    servers: Server[];
    selectedServerId: string | null;
}

const initialState: ServerState = {
    servers: [],
    selectedServerId: null,
};

const serverSlice = createSlice({
    name: 'server',
    initialState,
    reducers: {
        setServers: (state, action: PayloadAction<Server[]>) => {
            state.servers = action.payload;
        },
        setSelectedServerId: (state, action: PayloadAction<string | null>) => {
            state.selectedServerId = action.payload;
        },
    },
});

export const { setSelectedServerId, setServers } = serverSlice.actions;
export default serverSlice.reducer;
