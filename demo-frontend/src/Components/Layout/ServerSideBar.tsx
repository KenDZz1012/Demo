import { Menu } from 'antd';
import { PlusOutlined, PlusCircleFilled } from '@ant-design/icons';
import { Server } from '../../Connections/Types/Channel';

interface ServerSidebarProps {
    servers: Server[];
    onSelectServer: (serverId: string) => void;
    setOpenCreateServerModal: (open: boolean) => void;
    selectedServerId?: string;
}

export default function ServerSidebar({ servers, onSelectServer, setOpenCreateServerModal, selectedServerId }: ServerSidebarProps) {
    return (
        <Menu
            mode="inline"
            theme="light"
            inlineCollapsed
            style={{
                paddingLeft: 4,
                paddingRight: 4,
                paddingTop: 10,
                backgroundColor: '#2a2c35',
                color: 'white',
                borderRadius: 20
            }}
            selectedKeys={[selectedServerId ?? '']}
        >

            <Menu.Item
                title="Direct Messages"
                key="logo"
                onClick={() => onSelectServer("@me")}
                style={{
                    backgroundColor: "rgb(0, 21, 41)",
                    borderRadius: 16,
                    userSelect: 'none',
                    outline: 'none',
                    width: 46,
                    height: 46,
                    padding: 0,
                    overflow: 'visible',
                    position: "relative",
                    marginLeft: 14,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                {selectedServerId === "@me" && (
                    <div
                        style={{
                            position: 'absolute',
                            left: -12,
                            top: '50%',
                            transform: 'translateY(-50%)',
                            width: 4,
                            height: 40, // sẽ animate từ 0 đến 40
                            borderRadius: 4,
                            backgroundColor: '#fff',
                            zIndex: 2,
                            transition: 'height 0.3s ease, opacity 0.3s ease',
                            opacity: 1,
                        }}
                        className="grow-indicator"
                    />
                )}
                <img
                    src="/logo.png"
                    alt="logo"
                    style={{ width: 32, height: 40, objectFit: 'contain', marginTop: 20 }}
                />
            </Menu.Item>

            {servers.map((server) => (
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
                        overflow: 'visible',
                        position: "relative",
                        marginLeft: 14
                    }}
                >
                    {selectedServerId === server.id && (
                        <div
                            style={{
                                position: 'absolute',
                                left: -12,
                                top: '50%',
                                transform: 'translateY(-50%)',
                                width: 4,
                                height: 40, // sẽ animate từ 0 đến 40
                                borderRadius: 4,
                                backgroundColor: '#fff',
                                zIndex: 2,
                                transition: 'height 0.3s ease, opacity 0.3s ease',
                                opacity: 1,
                            }}
                            className="grow-indicator"
                        />
                    )}

                    <div style={{
                        width: "100%",
                        height: "100%",
                        zIndex: 1,
                        display: 'flex',
                        justifyContent: 'center',
                        alignItems: 'center'
                    }}>
                        {server.iconUrl ? (
                            <img
                                src={server.iconUrl}
                                alt={server.name}
                                style={{
                                    width: "100%",
                                    height: "100%",
                                    objectFit: "cover",
                                    borderRadius: 16
                                }}
                            />
                        ) : (
                            <p style={{ fontSize: 18, color: '#fff' }}>{server.name[0]}</p>
                        )}
                    </div>
                </Menu.Item>
            ))}

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
