import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { useServer } from 'Connections/AppBackend/Channel';
import { Channel, ServerDetail } from 'types';
import { Layout, Spin } from 'antd';

import ChannelSidebar from './ChannelSidebar';
import ChatArea from './ChatArea';
import CreateChannelModal from './Modal/CreatChannel';

const { Sider, Content } = Layout;

export default function ServerDetailPage() {
    const { id } = useParams();
    const [selectedServer, setSelectedServer] = useState<ServerDetail | null>(null);
    const [selectedChannel, setSelectedChannel] = useState<Channel | null>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState('');
    const [modalVisible, setModalVisible] = useState(false);

    const { data } = useServer(id || '');

    useEffect(() => {
        if (data?.data) {
            setSelectedServer(data.data);
            setSelectedChannel(data.data.channels?.[0] || null);
        }
    }, [data?.data]);

    const handleChannelSelect = (channelId: string) => {
        const channel = selectedServer?.channels.find(c => c.id === channelId);
        if (channel) {
            setSelectedChannel(channel);
            setMessages([]);
        }
    };

    const sendMessage = () => {
        if (input.trim()) {
            setMessages(prev => [...prev, input]);
            setInput('');
        }
    };


    return (
        <Layout style={{ height: '100%' }}>
            <CreateChannelModal
                visible={modalVisible}
                onCancel={() => setModalVisible(false)}
                onCreate={() => setModalVisible(false)}
            />

            <Sider
                width={300}
                style={{ backgroundColor: "#21212a", padding: "10px 0 10px 10px" }}
            >
                <ChannelSidebar
                    channels={selectedServer?.channels || []}
                    onSelectChannel={handleChannelSelect}
                    onAddTextChannel={() => setModalVisible(true)}
                    onAddVoiceChannel={() => setModalVisible(true)}
                    serverName={selectedServer?.name || ""}
                    setModalCreateChannelVisible={setModalVisible}
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
