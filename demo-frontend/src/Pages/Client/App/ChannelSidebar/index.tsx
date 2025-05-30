import { Menu, Button } from 'antd';
import { DownOutlined } from '@ant-design/icons';

interface ChannelSidebarProps {
    serverName?: string;
    channels: { id: string; name: string }[];
    onSelectChannel: (channelId: string) => void;
}

export default function ChannelSidebar({ serverName, channels, onSelectChannel }: ChannelSidebarProps) {
    return (
        <div style={{ height: '100%', background: '#001529', borderRight: '1px solid #555', overflow: "hidden" }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', paddingLeft: 16, paddingRight: 16, borderBottom: '1px solid #555' }}>
                <h3 style={{ color: '#fff' }}>{serverName}</h3>
                <Button type="text" icon={<DownOutlined />} style={{ color: '#fff' }} />
            </div>
            <Menu theme="dark" mode="inline">
                {channels.map((channel) => (
                    <Menu.Item key={channel.id} onClick={() => onSelectChannel(channel.id)}>
                        {channel.name}
                    </Menu.Item>
                ))}
            </Menu>
        </div>
    );
}
