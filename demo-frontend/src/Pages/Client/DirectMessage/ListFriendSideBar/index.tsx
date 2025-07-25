import { Menu, Avatar, Badge } from "antd";
import { UserOutlined } from "@ant-design/icons";
import CustomInput from "Components/CustomInput";
import { PlusOutlined, } from '@ant-design/icons';
import { Friend } from "types/user";



export default function ListFriendSideBar({ friends }: { friends: Friend[] }) {
    return (
        <Menu
            className="channel-menu"
            theme="dark"
            mode="inline"
            style={{
                backgroundColor: '#2a2c35',
                color: 'white',
                border: 'none',
                borderTopLeftRadius: 20,
                borderBottomLeftRadius: 20,
                paddingTop: 8,
                borderRight: '1px solid #555',
            }}
        >
            <div style={{ padding: "0px 10px 10px 10px", borderBottom: "1px solid #555" }}>
                <CustomInput
                    placeholder='Find friends'
                    size="large"
                    style={{
                        backgroundColor: "#212126",
                        color: "#fff",
                        borderColor: "#212126"
                    }}
                    className='input-create-server'
                />
            </div>
            <Menu.ItemGroup key="friends" title={<div style={{ display: "flex", justifyContent: "space-between" }}><span style={{ float: "left" }}>Direct Messages</span><PlusOutlined style={{ cursor: "pointer" }} /></div>}>
                {friends.map(friend => (
                    <Menu.Item key={friend.id} style={{ paddingLeft: 10 }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                            <Badge
                                dot
                                color={friend.isOnline ? "green" : "gray"}
                                offset={[-5, 32]}
                            >
                                <Avatar
                                    size="default"
                                    icon={<UserOutlined style={{ fontSize: 16 }} />}
                                    style={{ backgroundColor: '#5c5f6e' }}
                                />
                            </Badge>
                            <span style={{ color: 'white', fontSize: 16 }}>{friend.displayName}</span>
                        </div>
                    </Menu.Item>
                ))}
            </Menu.ItemGroup>
        </Menu>
    );
}
