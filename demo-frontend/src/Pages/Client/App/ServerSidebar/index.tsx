import { Menu } from 'antd';
import { PlusOutlined, PlusCircleFilled } from '@ant-design/icons';

interface ServerSidebarProps {
    servers: { id: string; name: string; channels: { id: string; name: string }[] }[];
    onSelectServer: (serverId: string) => void;
    setOpenCreateServerModal: (open: boolean) => void; // ✅ Sửa đúng kiểu hàm
}

export default function ServerSidebar({ servers, onSelectServer, setOpenCreateServerModal }: ServerSidebarProps) {
    return (
        <Menu
            mode="inline"
            theme="light"
            inlineCollapsed
            style={{ paddingLeft: 4, paddingRight: 4, paddingTop: 10, backgroundColor: '#2a2c35', color: 'white', borderRadius: 20 }}
            className="no-indicator"
        >
            <Menu.Item
                title="Direct Messages"
                key="logo"
                style={{
                    backgroundColor: 'rgb(0, 21, 41)',
                    borderRadius: 16,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center', // căn giữa
                    padding: 0,
                    width: 46,
                    height: 46
                }}
            >
                <img
                    src="/logo.png"
                    alt="logo"
                    style={{ width: 32, height: 40, objectFit: 'contain', marginTop: 20 }}
                />
            </Menu.Item>
            {
                servers.map((server) => (
                    <Menu.Item
                        key={server.id}
                        onClick={() => onSelectServer(server.id)}
                        style={{
                            backgroundColor: "rgb(0, 21, 41)",
                            borderRadius: 16,
                            userSelect: 'none',
                            outline: 'none',
                            width: 46,
                            height: 46,
                        }}>
                        <p style={{ fontSize: 18, marginTop: 2 }}>
                            {server.name[0]}
                        </p>
                    </Menu.Item>
                ))
            }
            <Menu.Item
                style={{
                    backgroundColor: 'rgb(0, 21, 41)',
                    borderRadius: 16,
                    width: 46,
                    height: 46,
                    paddingTop: 4,
                    paddingLeft: 14
                }}
                icon={<PlusCircleFilled style={{ fontSize: 20 }} />}
                onClick={() => { setOpenCreateServerModal(true) }}
            />
        </Menu>
    );
}
