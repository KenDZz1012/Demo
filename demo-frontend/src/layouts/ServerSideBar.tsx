import { Menu } from 'antd';
import { PlusCircleFilled } from '@ant-design/icons';
import { Server } from 'types';

interface ServerSidebarProps {
    servers: Server[];
    onSelectServer: (serverId: string) => void;
    setOpenCreateServerModal: (open: boolean) => void;
    selectedServerId: string | null;
}

export default function ServerSidebar({ servers, onSelectServer, setOpenCreateServerModal, selectedServerId }: ServerSidebarProps) {
    console.log(servers)
    return (
        <Menu
            mode="inline"
            theme="light"
            inlineCollapsed
            className="menu-hide-scroll"
            style={{
                paddingLeft: 4,
                paddingRight: 4,
                paddingTop: 4,
                backgroundColor: '#2a2c35',
                color: 'white',
                borderRadius: 20,
                overflowY: "auto",
                overflowX: "hidden",
                scrollbarWidth: 'none',
                msOverflowStyle: 'none',
                paddingBottom: 200
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
                            height: 40,
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
            <div
                style={{
                    marginTop: 6,
                    borderBottom: "1px solid #555",
                    width: "50%",
                    placeSelf: "center"
                }}>
            </div>
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
                        marginLeft: 14,
                        marginTop: 10
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
                                height: 40,
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
                title="Add a Server"
                style={{
                    backgroundColor: 'rgb(0, 21, 41)',
                    borderRadius: 16,
                    width: 46,
                    height: 46,
                    paddingLeft: 14,
                    marginLeft: 14,
                    marginTop: 10,
                    alignContent: "center"
                }}
                icon={<PlusCircleFilled style={{ fontSize: 20, marginTop: 10 }} />}
                onClick={() => { setOpenCreateServerModal(true) }}
            />
        </Menu>
    );
}
