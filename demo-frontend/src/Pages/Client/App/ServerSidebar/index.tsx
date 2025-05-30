import { Menu } from 'antd';
import { PlusOutlined } from '@ant-design/icons';

interface ServerSidebarProps {
    servers: { id: string; name: string; channels: { id: string; name: string }[] }[];
    onSelectServer: (serverId: string) => void;
}

export default function ServerSidebar({ servers, onSelectServer }: ServerSidebarProps) {
    return (
        <Menu mode="inline" theme="dark" inlineCollapsed style={{ paddingLeft: 20, paddingRight: 20 }}>
            <Menu.Item icon={<img src="/logo.png" alt="logo" style={{ width: 20, height: 40 }} />} />
            {servers.map((server) => (
                <Menu.Item key={server.id} onClick={() => onSelectServer(server.id)}>
                    {server.name[0]}
                </Menu.Item>
            ))}
            <Menu.Item icon={<PlusOutlined />} />
        </Menu>
    );
}
