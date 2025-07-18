import { useNavigate, useParams } from 'react-router-dom';
import { useServer } from '../../../Connections/AppBackend/Channel';
import ChannelSidebar from './ChannelSidebar';
import ChatArea from './ChatArea';
import { useEffect, useState } from 'react';
import { Channel, ServerDetail } from '../../../Connections/Types/Channel';
import { Layout } from 'antd';
import CreateChannelModal from './Modal/CreatChannel';

const { Sider, Content } = Layout;

export default function ServerDetailPage() {
    const navigate = useNavigate();
    const { id } = useParams();
    const [selectedServer, setSelectedServer] = useState<ServerDetail | null>(null);
    const [selectedChannel, setSelectedChannel] = useState<Channel | null>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState('');
    const { data, isLoading, isError } = useServer(id || '');
    const [modalCreateChannelVisible, setModalCreateChannelVisible] = useState(false);

    useEffect(() => {
        if (data?.data) {
            setSelectedServer(data.data);
            setSelectedChannel(data.data.channels?.[0] || null);
        }
    }, [data]);

    const handleChannelSelect = (channelId: string) => {
        const channel = selectedServer?.channels.find(c => c.id === channelId);
        if (channel) {
            setSelectedChannel(channel);
            setMessages([]);
        }
    };

    const sendMessage = () => {
        if (input.trim()) {
            setMessages([...messages, input]);
            setInput('');
        }
    };

    if (isError) navigate("/server/@me", { replace: true });

    return (
        <Layout style={{ height: '100%' }}>
            <CreateChannelModal
                visible={modalCreateChannelVisible}
                onCancel={() => setModalCreateChannelVisible(false)}
                onCreate={(values) => {
                    setModalCreateChannelVisible(false);
                }}
            />
            <Sider width={300} style={{ backgroundColor: "#21212a", padding: "10px 0px 10px 10px" }}>
                <ChannelSidebar
                    channels={selectedServer?.channels || []}
                    onSelectChannel={handleChannelSelect}
                    onAddTextChannel={() => console.log('Add text')}
                    onAddVoiceChannel={() => console.log('Add voice')}
                    serverName={selectedServer?.name || ""}
                    setModalCreateChannelVisible={setModalCreateChannelVisible}
                />
            </Sider>
            <Content style={{ backgroundColor: "#21212a", padding: "10px 10px 10px 0px" }}>
                <ChatArea
                    channelName={selectedChannel?.name}
                    messages={messages}
                    input={input}
                    setInput={setInput}
                    sendMessage={sendMessage}
                />
            </Content>
        </Layout>
    );
}
