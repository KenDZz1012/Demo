import { useEffect, useState } from 'react';
import { Layout } from 'antd';
import { useServers } from 'Connections/AppBackend/Channel';
import { Server } from 'types';
import CreateServerModal from './Modal/CreateServer';
import ServerSidebar from 'layouts/ServerSideBar';
import { Outlet, useLocation, useNavigate, useParams } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { selectAuthUser } from 'store/selectors/authSelectors';

const { Sider, Content } = Layout;

export default function DiscordClone() {
    const { id: ownerId } = useSelector(selectAuthUser) || {};
    const { id } = useParams();
    const navigate = useNavigate();
    const location = useLocation();
    const { data, isLoading, isError } = useServers({ ownerId });
    const [servers, setServers] = useState<Server[]>([]);
    const [selectedServer, setSelectedServer] = useState<Server | null>(null);
    const [openCreateServerModal, setOpenCreateServerModal] = useState(false);

    useEffect(() => {
        if (data?.data?.length) {
            setServers(data.data);
        }
    }, [data]);

    useEffect(() => {
        if ((location.pathname.startsWith("/server/") && id) || location.pathname.startsWith("/server/@me")) {
            const found = data?.data.find(server => server.id === id) || { id: "@me", name: "", ownerId: "" };
            setSelectedServer(found);
        } else {
            setSelectedServer(null);
        }
    }, [location, id, data]);

    return (
        <Layout style={{ height: '100vh', backgroundColor: '#21212a' }}>
            <CreateServerModal open={openCreateServerModal} onClose={() => setOpenCreateServerModal(false)} ownerId={ownerId} />

            {isLoading ? (
                <div style={{ color: 'white', padding: 24, fontSize: 18 }}>
                    Đang tải dữ liệu server...
                </div>
            ) : (
                <>
                    <Sider width={100} style={{ padding: 10, backgroundColor: '#21212a' }}>
                        <ServerSidebar
                            servers={servers}
                            onSelectServer={(serverId) => navigate(`/server/${serverId}`)}
                            setOpenCreateServerModal={setOpenCreateServerModal}
                            selectedServerId={selectedServer?.id}
                        />
                    </Sider>

                    <Layout>
                        <Content style={{ backgroundColor: '#21212a', overflow: 'auto' }}>
                            <Outlet context={{ selectedServer }} />
                        </Content>
                    </Layout>
                </>
            )}
        </Layout>
    );
}
