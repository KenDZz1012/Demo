import { Menu } from 'antd';
import { PlusOutlined, PlusCircleFilled } from '@ant-design/icons';
import { Server } from '../../../../Connections/Types/Channel';

interface ServerSidebarProps {
    servers: Server[];
    onSelectServer: (serverId: string) => void;
    setOpenCreateServerModal: (open: boolean) => void; // ✅ Sửa đúng kiểu hàm
    selectedServerId?: string; // <-- thêm dòng này
}

export default function ServerSidebar({ servers, onSelectServer, setOpenCreateServerModal, selectedServerId }: ServerSidebarProps) {
    return (
        <Menu
            mode="inline"
            theme="light"
            inlineCollapsed
            style={{ paddingLeft: 4, paddingRight: 4, paddingTop: 10, backgroundColor: '#2a2c35', color: 'white', borderRadius: 20 }}
            className="no-indicator"
            selectedKeys={[selectedServerId ?? '']}
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
                    height: 46,
                    marginLeft: 14

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
                    <div key={server.id} style={{ position: 'relative', height: 56, marginBottom: 8 }}>
                        {selectedServerId === server.id && (
                            <div style={{
                                position: 'absolute',
                                left: 0,
                                top: '42%',
                                transform: 'translateY(-50%)',
                                width: 4,
                                height: 40,
                                borderRadius: 4,
                                backgroundColor: '#fff',
                                zIndex: 1,
                                animation: 'growLine 0.2s ease-out'
                            }} />
                        )}
                        <Menu.Item
                            title={server.name}
                            key={server.id}
                            onClick={() => onSelectServer(server.id)}
                            style={{
                                backgroundColor: "rgb(0, 21, 41)",
                                borderRadius: 16,
                                userSelect: 'none',
                                outline: 'none',
                                width: 46,
                                height: 46,
                                padding: 0,
                                overflow: 'hidden', // 🧽 Giúp cắt phần thừa nếu ảnh vượt quá
                                position: "relative",
                                marginLeft: 14
                            }}>

                            {server.iconUrl ?
                                <img
                                    src={server.iconUrl}
                                    alt={server.name}
                                    style={{
                                        width: "100%",
                                        height: "100%",
                                        objectFit: "cover",      // <-- giúp ảnh fill mà không méo
                                        borderRadius: 16
                                    }}
                                /> : <p style={{ fontSize: 18, marginTop: 2 }}>{server.name[0]}</p>
                            }
                        </Menu.Item>
                    </div>
                ))
            }
            <Menu.Item
                style={{
                    backgroundColor: 'rgb(0, 21, 41)',
                    borderRadius: 16,
                    width: 46,
                    height: 46,
                    paddingTop: 4,
                    paddingLeft: 14,
                    marginLeft: 14
                }}
                icon={<PlusCircleFilled style={{ fontSize: 20 }} />}
                onClick={() => { setOpenCreateServerModal(true) }}
            />
        </Menu>
    );
}
