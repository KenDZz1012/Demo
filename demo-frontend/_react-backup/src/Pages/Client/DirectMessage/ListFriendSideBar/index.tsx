import { Menu, Avatar, Badge } from "antd";
import { TeamOutlined, UserOutlined } from "@ant-design/icons";
import CustomInput from "Components/CustomInput";
import { PlusOutlined, } from '@ant-design/icons';
import { Friend } from "types/user";



export default function ListFriendSideBar({ friends, onSelectedFriend, friendId }: { friends: Friend[], onSelectedFriend: (friend: Friend | null) => void, friendId: string | null | undefined }) {
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
            defaultSelectedKeys={friendId ? [friendId] : ["addFriend"]}
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
            <Menu.ItemGroup key="other" >
                <Menu.Item key={"addFriend"} style={{ paddingLeft: 10 }} onClick={() => {
                    onSelectedFriend(null)
                }}>
                    <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                        <div style={{ position: 'relative', width: 36, height: 36 }}>
                            <TeamOutlined style={{ fontSize: 20, color: '#fff' }} />
                        </div>
                        <span style={{ color: 'white', fontSize: 16 }}>Friends</span>
                    </div>
                </Menu.Item>
            </Menu.ItemGroup>
            <Menu.ItemGroup key="friends" title={<div style={{ display: "flex", justifyContent: "space-between" }}><span style={{ float: "left" }}>Direct Messages</span><PlusOutlined style={{ cursor: "pointer" }} /></div>}>
                {friends.map(friend => (
                    <Menu.Item key={friend.id} style={{ paddingLeft: 10 }} onClick={() => {
                        onSelectedFriend(friend)
                    }}>
                        <div style={{ display: 'flex', alignItems: 'center', gap: 10 }}>
                            <div style={{ position: 'relative', width: 36, height: 36 }}>
                                <img
                                    src={friend.avatarUrl || '/logo.png'}
                                    alt={friend.displayName}
                                    style={{
                                        width: '100%',
                                        height: '100%',
                                        borderRadius: '50%',
                                        objectFit: 'cover',
                                        display: 'block',
                                        backgroundColor: "#6b6967"
                                    }}
                                />
                                <span
                                    style={{
                                        position: 'absolute',
                                        bottom: 0,
                                        right: 0,
                                        width: 10,
                                        height: 10,
                                        backgroundColor: friend.isOnline ? 'green' : 'gray',
                                        borderRadius: '50%',
                                        border: '2px solid white',
                                    }}
                                />
                            </div>
                            <span style={{ color: 'white', fontSize: 16 }}>{friend.displayName}</span>
                        </div>
                    </Menu.Item>
                ))}
            </Menu.ItemGroup>
        </Menu>
    );
}
