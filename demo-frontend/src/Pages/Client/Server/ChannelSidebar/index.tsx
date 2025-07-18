import { Dropdown, Menu } from 'antd';
import { PlusOutlined, MessageOutlined, AudioOutlined, DownOutlined, UsergroupAddOutlined, SettingOutlined, ExportOutlined, PlusCircleFilled } from '@ant-design/icons';
import { Channel } from '../../../../Connections/Types/Channel';

interface ChannelMenuProps {
    channels: Channel[];
    onSelectChannel: (channelId: string) => void;
    onAddTextChannel: () => void;
    onAddVoiceChannel: () => void;
    serverName: string,
    setModalCreateChannelVisible: (visible: boolean) => void;
}

export default function ChannelMenu({
    channels,
    onSelectChannel,
    onAddTextChannel,
    onAddVoiceChannel,
    serverName,
    setModalCreateChannelVisible
}: ChannelMenuProps) {

    const serverMenu = (
        <Menu
            className="menu-server-setting"
            theme="dark"
            style={{
                backgroundColor: "#001529",
                width: "80%",
                placeSelf: "center"
            }}
            items={[
                {
                    key: 'createchannel',
                    label: (
                        <div style={{ display: "flex", justifyContent: "space-between" }}>
                            <span style={{ color: '#fff', fontSize: 14 }}>Create Channel</span>
                            <PlusCircleFilled style={{ color: '#fff', fontSize: 14 }} />
                        </div>
                    ),
                    onClick: () => setModalCreateChannelVisible(true),
                },
                {
                    key: 'invite',
                    label: (
                        <div style={{ display: "flex", justifyContent: "space-between" }}>
                            <span style={{ color: '#fff', fontSize: 14 }}>Invite People</span>
                            <UsergroupAddOutlined style={{ color: '#fff', fontSize: 14 }} />
                        </div>
                    ),
                    onClick: () => console.log('Invite')
                },
                {
                    key: 'settings',
                    label: (
                        <div style={{ display: "flex", justifyContent: "space-between" }}>
                            <span style={{ color: '#fff', fontSize: 14 }}>Server Settings</span>
                            <SettingOutlined style={{ color: '#fff', fontSize: 14 }} />
                        </div>
                    ),
                    onClick: () => console.log('Settings')
                },
                { type: 'divider' },
                {
                    key: 'leave',
                    label: (
                        <div style={{ display: "flex", justifyContent: "space-between" }}>
                            <span style={{ color: '#f17875' }}>Leave Server</span>
                            <ExportOutlined style={{ color: '#f17875', fontSize: 14 }} />
                        </div>
                    ),
                    onClick: () => console.log('Leave Server'),
                },
            ]}
        />
    );

    return (
        <Menu
            className="channel-menu"
            theme="dark"
            mode="inline"
            style={{ backgroundColor: '#2a2c35', color: 'white', border: 'none', borderTopLeftRadius: 20, borderBottomLeftRadius: 20 }}
        >
            <Dropdown overlay={serverMenu} trigger={['click']} placement="bottomLeft">
                <div
                    style={{
                        backgroundColor: "#2a2c35",
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'center',
                        padding: 18,
                        borderBottom: '1px solid #555',
                        color: '#fff',
                        cursor: 'pointer',
                        userSelect: 'none',
                        borderTopLeftRadius: 20
                    }}
                >
                    <span style={{ fontWeight: 'bold' }}>{serverName}</span>
                    <DownOutlined />
                </div>
            </Dropdown>

            {/* TEXT CHANNELS */}
            <Menu.Item key="text-title" disabled style={sectionTitleStyle}>
                <div style={titleRowStyle}>
                    <span>Text Channels</span>
                    <PlusOutlined onClick={onAddTextChannel} style={plusStyle} />
                </div>
            </Menu.Item>
            {channels.filter((ch) => ch.type === 'Text').map((channel) => (
                <Menu.Item
                    key={channel.id}
                    onClick={() => onSelectChannel(channel.id)}
                    style={{ textAlign: "left" }}
                >
                    <div style={{ display: "flex", justifyContent: 'space-between' }}>
                        # {channel.name}
                        <MessageOutlined />
                    </div>
                </Menu.Item>
            ))}

            {/* VOICE CHANNELS */}
            <Menu.Item key="voice-title" disabled style={sectionTitleStyle}>
                <div style={titleRowStyle}>
                    <span>Voice Channels</span>
                    <PlusOutlined onClick={onAddVoiceChannel} style={plusStyle} />
                </div>
            </Menu.Item>
            {channels.filter((ch) => ch.type === 'Voice').map((channel) => (
                <Menu.Item
                    key={channel.id}
                    icon={<AudioOutlined />}
                    onClick={() => onSelectChannel(channel.id)}
                >
                    {channel.name}
                </Menu.Item>
            ))}
        </Menu>
    );
}


// Styles
const sectionTitleStyle = {
    cursor: 'default',
    color: '#888',
    paddingLeft: 16,
    paddingRight: 12,
    fontSize: 14,
    fontWeight: 'bold' as const,
    backgroundColor: '#2a2c35',
};

const titleRowStyle = {
    display: 'flex',
    justifyContent: 'space-between',
    alignItems: 'center',
};

const plusStyle = {
    color: '#888',
    fontSize: 12,
    cursor: 'pointer',
};
