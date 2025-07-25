import { useEffect, useState } from 'react';
import { Layout } from 'antd';
import { useServers } from 'Connections/AppBackend/Channel';
import CreateServerModal from './Modal/CreateServer';
import ServerSidebar from 'layouts/ServerSideBar';
import { Outlet, useNavigate, useParams } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { selectAuthUser, selectServerId, selectServers } from 'store/selectors/authSelectors';
import { setSelectedServerId, setServers } from 'features/server/serverSlice';
import LoadingScreen from 'Components/LoadingScreen';

const { Sider, Content } = Layout;

export default function DiscordClone() {
    const { id: ownerId } = useSelector(selectAuthUser) || {};
    const { id: urlServerId } = useParams();
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const { data, isLoading } = useServers({ ownerId });
    const selectedServerId = useSelector(selectServerId);
    const servers = useSelector(selectServers);

    const [openCreateServerModal, setOpenCreateServerModal] = useState(false);
    const [showInitialLoading, setShowInitialLoading] = useState(true);

    useEffect(() => {
        const timeout = setTimeout(() => {
            setShowInitialLoading(false);
        }, 2000);
        return () => clearTimeout(timeout);
    }, []);

    useEffect(() => {
        if (data?.data) {
            dispatch(setServers(data.data));
        }
    }, [data, dispatch]);

    useEffect(() => {
        const nextId = urlServerId ?? '@me';
        if (selectedServerId !== nextId) {
            dispatch(setSelectedServerId(nextId));
        }
    }, [urlServerId, selectedServerId, dispatch]);

    useEffect(() => {
        const serverId = urlServerId ?? '@me';

        if (!isLoading && !showInitialLoading) {
            const isValid = serverId === '@me' || servers.some(server => server.id === serverId);
            if (!isValid) {
                navigate('/server/@me', { replace: true });
            }
        }
    }, [isLoading, showInitialLoading, servers, urlServerId, navigate]);

    if (isLoading || showInitialLoading) {
        return <LoadingScreen />;
    }

    return (
        <Layout style={{ height: '100vh', backgroundColor: '#21212a' }}>
            <CreateServerModal
                open={openCreateServerModal}
                onClose={() => setOpenCreateServerModal(false)}
                ownerId={ownerId}
            />

            <Sider width={100} style={{ padding: 10, backgroundColor: '#21212a' }}>
                <ServerSidebar
                    servers={servers}
                    onSelectServer={serverId => {
                        dispatch(setSelectedServerId(serverId));
                        navigate(`/server/${serverId}`, { replace: true });
                    }}
                    setOpenCreateServerModal={setOpenCreateServerModal}
                    selectedServerId={selectedServerId}
                />
            </Sider>

            <Layout>
                <Content style={{ backgroundColor: '#21212a', overflow: 'auto' }}>
                    <Outlet />
                </Content>
            </Layout>
        </Layout>
    );
}
