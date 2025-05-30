import { useState } from 'react';
import { Layout } from 'antd';
import ServerSidebar from './ServerSidebar';
import ChannelSidebar from './ChannelSidebar';
import ChatArea from './ChatArea';

const { Sider, Content } = Layout;

const initialServers = [
    {
        id: '1',
        name: 'Server 1',
        channels: [
            { id: 'c1', name: 'general' },
            { id: 'c2', name: 'random' },
        ],
    },
    {
        id: '2',
        name: 'Server 2',
        channels: [
            { id: 'c3', name: 'chat' },
        ],
    },
];

export default function DiscordClone() {
    const [servers] = useState(initialServers);
    const [selectedServer, setSelectedServer] = useState(servers[0]);
    const [selectedChannel, setSelectedChannel] = useState(servers[0].channels[0]);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState('');

    const handleServerSelect = (serverId: string) => {
        const server = servers.find((s) => s.id === serverId);
        if (server) {
            setSelectedServer(server);
            setSelectedChannel(server.channels[0]);
            setMessages([]); // Clear chat on server change (optional)
        }
    };

    const handleChannelSelect = (channelId: string) => {
        const channel = selectedServer.channels.find((c) => c.id === channelId);
        if (channel) {
            setSelectedChannel(channel);
            setMessages([]); // Clear chat on channel change (optional)
        }
    };

    const sendMessage = () => {
        if (input.trim()) {
            setMessages([...messages, input]);
            setInput('');
        }
    };

    return (
        <Layout style={{ height: '100vh' }}>
            <Sider width={100} style={{ background: '#1f1f1f', borderRight: '1px solid #555' }}>
                <ServerSidebar servers={servers} onSelectServer={handleServerSelect} />
            </Sider>
            <Sider width={300} style={{ background: '#001529', borderRight: '1px solid #555' }}>
                <ChannelSidebar
                    serverName={selectedServer?.name}
                    channels={selectedServer?.channels || []}
                    onSelectChannel={handleChannelSelect}
                />
            </Sider>
            <Layout>
                <Content>
                    <ChatArea
                        channelName={selectedChannel?.name}
                        messages={messages}
                        input={input}
                        setInput={setInput}
                        sendMessage={sendMessage}
                    />
                </Content>
            </Layout>
        </Layout>
    );
}
