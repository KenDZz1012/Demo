import { useEffect, useState } from 'react';
import { Layout } from 'antd';
import ServerSidebar from './ServerSidebar';
import ChannelSidebar from './ChannelSidebar';
import ChatArea from './ChatArea';
import { useChannels } from '../../../Connections/AppBackend/Channel';
import { Channel, Server } from '../../../Connections/Types/Channel';
import CreateServerModal from './Modal/CreateServer';

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
    const { data, isLoading, isError } = useChannels();
    const [servers, setServers] = useState<Server[]>([]);
    const [selectedServer, setSelectedServer] = useState<Server | null>(null);
    const [selectedChannel, setSelectedChannel] = useState<Channel | any>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState('');
    const [openCreateServerModal, setOpenCreateServerModal] = useState(false);

    const handleServerSelect = (serverId: string) => {
        const server = servers.find((s) => s.id === serverId);
        if (server) {
            setSelectedServer(server);
            setSelectedChannel(server.channels[0]);
            setMessages([]); // Clear chat on server change (optional)
        }
    };

    const handleChannelSelect = (channelId: string) => {
        const channel = selectedServer?.channels.find((c: Channel) => c.id === channelId);
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

    useEffect(() => {
        if (data?.data?.length) {
            setServers(data.data);
            setSelectedServer(data.data[0]);
            setSelectedChannel(data.data[0].channels?.[0]);
        }
    }, [data]);


    return (
        <Layout style={{ height: '100vh' }}>
            <CreateServerModal open={openCreateServerModal} onClose={() => setOpenCreateServerModal(false)} />
            <Sider width={85} style={{ padding: 10, backgroundColor: "#21212a" }}>
                <ServerSidebar servers={servers} onSelectServer={handleServerSelect} setOpenCreateServerModal={setOpenCreateServerModal} />
            </Sider>
            <Sider width={300} style={{ borderRight: 'none', backgroundColor: "#21212a", paddingLeft: 10, paddingTop: 10, paddingBottom: 10 }}>
                <ChannelSidebar
                    channels={selectedServer?.channels || []}
                    onSelectChannel={handleChannelSelect}
                    onAddTextChannel={() => console.log('Add text channel')}
                    onAddVoiceChannel={() => console.log('Add voice channel')}
                    serverName={selectedServer?.name ?? ""}
                />
            </Sider>
            <Layout>
                <Content style={{ backgroundColor: "#21212a", paddingRight: 10, paddingTop: 10, paddingBottom: 10 }}>
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
