// features/server/serverSlice.ts

import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Channel, Server, ServerDetail } from 'types';

interface ServerState {
    servers: Server[];
    selectedServer: ServerDetail | null;
    selectedServerId: string | null;
}

const initialState: ServerState = {
    servers: [],
    selectedServer: null,
    selectedServerId: null,
};

const serverSlice = createSlice({
    name: 'server',
    initialState,
    reducers: {
        setServers: (state, action: PayloadAction<Server[]>) => {
            state.servers = action.payload;
        },
        setSelectedServer: (state, action: PayloadAction<ServerDetail | null>) => {
            state.selectedServer = action.payload;
        },
        setSelectedServerId: (state, action: PayloadAction<string | null>) => {
            state.selectedServerId = action.payload;
        },
        addServer: (state, action: PayloadAction<Server>) => {
            const exists = state.servers.some(server => server.id === action.payload.id);
            if (!exists) {
                state.servers.push(action.payload);
            }
        },
        removeServer: (state, action: PayloadAction<string>) => {
            state.servers = state.servers.filter(server => server.id !== action.payload);
        },
        updateServer: (state, action: PayloadAction<Server>) => {
            const index = state.servers.findIndex(server => server.id === action.payload.id);
            if (index !== -1) {
                state.servers[index] = action.payload;
            }
        },
        addChannel: (state, action: PayloadAction<Channel>) => {
            const exists = state.selectedServer?.channels.some(channel => channel.id === action.payload.id);
            if (!exists) {
                state.selectedServer?.channels.push(action.payload);
            }
        },
        removeChannel: (state, action: PayloadAction<string>) => {
            if (state.selectedServer) {
                state.selectedServer.channels = state.selectedServer?.channels.filter(channel => channel.id !== action.payload);
            }
        }
    },
});

export const { setSelectedServerId, setSelectedServer, setServers, addServer, removeServer, updateServer, addChannel, removeChannel } = serverSlice.actions;
export default serverSlice.reducer;
