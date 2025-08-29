import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useCreateChannel, useDeleteServer, useLeaveServer, useServer } from 'Connections/AppBackend/Channel';
import { Channel, CreateChannel, ServerDetail } from 'types';
import { Layout, Spin } from 'antd';

import ChannelSidebar from './ChannelSidebar';
import ChatArea from './ChatArea';
import CreateChannelModal from './Modal/CreatChannel';
import { useDispatch, useSelector } from 'react-redux';
import { setSelectedServer } from 'features/server/serverSlice';
import { selectAuthUser, selectServer, selectServerId } from 'store/selectors/authSelectors';
import InvitePeopleModal from './Modal/InvitePeople';

const { Sider, Content } = Layout;

export default function ServerDetailPage() {
    const { id } = useParams();
    const { data, isError } = useServer(id || '');
    const server = useSelector(selectServer);
    const serverId = useSelector(selectServerId);
    const { id: userId } = useSelector(selectAuthUser) || {};
    const [selectedChannel, setSelectedChannel] = useState<Channel | null>(null);
    const [messages, setMessages] = useState<string[]>([]);
    const [input, setInput] = useState('');
    const [modalVisible, setModalVisible] = useState(false);
    const [invitePeopleModalVisible, setInvitePeopleModal] = useState(false);

    const { mutate: mutateDelete } = useDeleteServer();
    const { mutate: mutateCreatChannel } = useCreateChannel();
    const { mutate: mutateLeaveServer } = useLeaveServer();
    const dispatch = useDispatch();
    const navigate = useNavigate();

    useEffect(() => {
        if (data?.data) {
            dispatch(setSelectedServer(data.data));
        }
    }, [data, dispatch]);


    useEffect(() => {
        if (data?.data) {
            setSelectedChannel(data.data.channels?.[0] || null);
        }
    }, [data?.data]);

    useEffect(() => {
        console.log(isError)
        if (isError) {
            navigate('/server/@me', { replace: true });
        }
    }, [serverId]);

    const handleChannelSelect = (channelId: string) => {
        const channel = server?.channels.find(c => c.id === channelId);
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

    const deleteServer = () => {
        if (!serverId) return;
        mutateDelete(serverId, {
            onSuccess: () => {
            },
        });
    };

    const leaveServer = () => {
        if (!serverId) return;
        if (!userId) return;
        mutateLeaveServer({ ServerId: serverId, UserId: userId }, {
            onSuccess: () => {
            },
        });
    }

    const onCreatChannel = (input: CreateChannel) => {
        console.log(input)
        // mutateCreatChannel(input, {
        //     onSuccess: () => {
        //         setModalVisible(false)
        //     },
        //     onError: (err) => {
        //         setModalVisible(false)
        //     },
        // });
    }


    return (
        <Layout style={{ height: '100%' }}>
            <CreateChannelModal
                visible={modalVisible}
                onCancel={() => setModalVisible(false)}
                onCreate={onCreatChannel}
            />
            <InvitePeopleModal visible={invitePeopleModalVisible} />

            <Sider
                width={300}
                style={{ backgroundColor: "#21212a", padding: "10px 0 10px 10px" }}
            >
                <ChannelSidebar
                    channels={server?.channels || []}
                    onSelectChannel={handleChannelSelect}
                    onAddTextChannel={() => setModalVisible(true)}
                    onAddVoiceChannel={() => setModalVisible(true)}
                    serverName={server?.name || ""}
                    setModalCreateChannelVisible={setModalVisible}
                    isOwner={server?.ownerId === userId}
                    deleteServer={deleteServer}
                    leaveServer={leaveServer}
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
