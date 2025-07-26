import { Input, Tabs, Tooltip } from 'antd';
import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import Icon, { TeamOutlined, CloseCircleOutlined, CloseOutlined } from '@ant-design/icons';
import CustomInput from 'Components/CustomInput';
import CustomButton from 'Components/CustomButton';
import { useAddFriend } from 'Connections/AppBackend/UserRelationship';
import { Friend, FriendPending } from 'types/user';

const { TabPane } = Tabs;

export default function AddFriendSideBar({ friendPending }: { friendPending: FriendPending[] }) {
    const [activeTab, setActiveTab] = useState('add');
    const [addresseeName, setAddresseeName] = useState('');
    const { mutate, isSuccess, isError } = useAddFriend();
    const [messageSubmit, setMessageSubmit] = useState('');
    const sentRequests = friendPending.filter(friend => friend.isSender);
    const receivedRequests = friendPending.filter(friend => !friend.isSender);

    const onSubmit = () => {
        if (!addresseeName.trim()) return;
        const input = {
            requesterId: localStorage.getItem('userID') || '',
            addresseeName
        };
        mutate(input, {
            onSuccess: async () => {
                setMessageSubmit('Friend request sent successfully to ' + addresseeName);
            },
            onError: (err) => {
                setMessageSubmit(err.response?.data.message || 'Add friend failed');
            },
        });

    };

    useEffect(() => {
        setAddresseeName('');
    }, [activeTab])


    const handleAccept = (id: string) => {
        console.log('Accept friend with id:', id);
        // TODO: call API accept friend
    };

    const handleReject = (id: string) => {
        console.log('Reject friend with id:', id);
        // TODO: call API reject friend
    };
    const handleCancel = (id: string) => { }

    return (
        <div style={{
            backgroundColor: 'rgb(48 48 49)',
            display: 'flex',
            flexDirection: 'column',
            height: '100%',
            borderTopRightRadius: 20,
            borderBottomRightRadius: 20
        }}>
            <div
                style={{
                    borderBottom: '1px solid #555',
                    height: 59,
                    alignItems: 'center',
                    paddingLeft: 16,
                    display: 'flex',
                }}
            >
                <div>
                    <TeamOutlined style={{ fontSize: 20, color: "#fff" }} />
                    <span style={{ marginLeft: 8, color: "#fff", fontSize: 16, fontWeight: 600 }}>
                        Friends
                    </span>
                </div>

                <Tabs
                    activeKey={activeTab}
                    onChange={setActiveTab}
                    tabBarStyle={{
                        marginBottom: 0,
                        borderBottom: 'none',
                        marginLeft: 32,
                    }}
                >
                    <TabPane
                        tab={
                            <div className={`custom-tab ${activeTab === 'pending' ? 'active' : ''}`}>
                                Pending
                            </div>
                        }
                        key="pending"
                    />
                    <TabPane
                        tab={
                            <div className={`custom-tab ${activeTab === 'add' ? 'active' : ''}`}>
                                Add Friend
                            </div>
                        }
                        key="add"
                    />
                </Tabs>
            </div>

            {/* Content */}
            <div style={{ padding: 16, color: '#fff', flex: 1 }}>
                {activeTab === 'add' && (
                    <motion.div
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.3 }}
                    >
                        <div style={{ textAlign: "left", fontSize: 18, fontWeight: 500, marginBottom: 16 }}>
                            <h3 style={{ marginTop: 0, marginBottom: 0 }}>Add Friend</h3>
                            <h5 style={{ marginTop: 10, marginBottom: 0, fontWeight: 400 }}>You can add friends with their KenVerse username</h5>
                        </div>
                        <div style={{ position: 'relative', width: '100%' }}>
                            <CustomInput
                                placeholder='You can add friends with their KenVerse username'
                                size="large"
                                style={{
                                    backgroundColor: "#212126",
                                    color: "#fff",
                                    borderColor: isSuccess ? "#28a745" : isError ? "#dc3545" : "#212126",
                                    paddingRight: 120, // chừa khoảng cho button
                                    height: 50,

                                }}
                                className="input-create-server"
                                onPressEnter={onSubmit}
                                onChange={(e) => setAddresseeName(e.target.value)}
                            />
                            <CustomButton
                                style={{
                                    position: 'absolute',
                                    top: 4,
                                    right: 4,
                                    height: 'calc(100% - 8px)',
                                    backgroundColor: "#5865F2",
                                    color: "#fff",
                                    border: "none",
                                    padding: '0 16px',
                                }}
                                disabled={!addresseeName.trim()}
                                onClick={onSubmit}
                            >
                                Send Friend Request
                            </CustomButton>
                        </div>
                        <span style={{ color: isSuccess ? "#28a745" : isError ? "#dc3545" : "#212126", float: "left", marginTop: 8 }}>{messageSubmit}</span>

                    </motion.div>
                )}

                {activeTab === 'pending' && (
                    <motion.div
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        transition={{ duration: 0.3 }}
                    >
                        {friendPending.length === 0 ? (
                            <p style={{ color: '#aaa' }}>No pending friend requests.</p>
                        ) : (
                            <>
                                {receivedRequests.length > 0 && (
                                    <div style={{ marginBottom: 24 }}>
                                        <h4 style={{ color: '#fff', marginBottom: 12 }}>Received</h4>
                                        {receivedRequests.map(friend => (
                                            <div
                                                key={friend.id}
                                                style={{
                                                    display: 'flex',
                                                    alignItems: 'center',
                                                    justifyContent: 'space-between',
                                                    backgroundColor: '#2c2c2f',
                                                    padding: '12px 16px',
                                                    borderRadius: 8,
                                                    marginBottom: 12
                                                }}
                                            >
                                                <div style={{ display: 'flex', alignItems: 'center' }}>
                                                    <img
                                                        src={friend.avatarUrl || '/logo.png'}
                                                        alt={friend.displayName}
                                                        style={{
                                                            width: 40,
                                                            height: 40,
                                                            borderRadius: '50%',
                                                            objectFit: 'cover',
                                                            marginRight: 12
                                                        }}
                                                    />
                                                    <div>
                                                        <div style={{ fontWeight: 600, color: '#fff' }}>{friend.displayName}</div>
                                                        <div style={{ fontSize: 13, color: '#aaa', textAlign: "left" }}>{friend.userName}</div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <CustomButton
                                                        style={{
                                                            backgroundColor: "#28a745",
                                                            color: "#fff",
                                                            border: "none",
                                                            padding: '4px 12px',
                                                            marginRight: 8
                                                        }}
                                                        onClick={() => handleAccept(friend.id)}
                                                    >
                                                        Accept
                                                    </CustomButton>
                                                    <CustomButton
                                                        style={{
                                                            backgroundColor: "#dc3545",
                                                            color: "#fff",
                                                            border: "none",
                                                            padding: '4px 12px'
                                                        }}
                                                        onClick={() => handleReject(friend.id)}
                                                    >
                                                        Reject
                                                    </CustomButton>
                                                </div>
                                            </div>
                                        ))}
                                    </div>
                                )}

                                {sentRequests.length > 0 && (
                                    <div>
                                        <h4 style={{ color: '#fff', marginBottom: 12, textAlign: "left", paddingLeft: 10, fontWeight: 500 }}>Sent ({sentRequests.length})</h4>
                                        {sentRequests.map(friend => (
                                            <div
                                                key={friend.id}
                                                style={{
                                                    display: 'flex',
                                                    alignItems: 'center',
                                                    justifyContent: 'space-between',
                                                    backgroundColor: '#2c2c2f',
                                                    padding: '12px 16px',
                                                    borderRadius: 8,
                                                    marginBottom: 12
                                                }}
                                            >
                                                <div style={{ display: 'flex', alignItems: 'center' }}>
                                                    <img
                                                        src={friend.avatarUrl || '/logo.png'}
                                                        alt={friend.displayName}
                                                        style={{
                                                            width: 40,
                                                            height: 40,
                                                            borderRadius: '50%',
                                                            objectFit: 'cover',
                                                            marginRight: 12
                                                        }}
                                                    />
                                                    <div>
                                                        <div style={{ fontWeight: 600, color: '#fff' }}>{friend.displayName}</div>
                                                        <div style={{ fontSize: 13, color: '#aaa', textAlign: "left" }}>{friend.userName}</div>
                                                    </div>
                                                </div>
                                                <Tooltip title="Cancel">
                                                    <CustomButton
                                                        style={{
                                                            width: 40,
                                                            height: 40,
                                                            color: "#fff",
                                                            border: "1px solid #393b47",
                                                            backgroundColor: "#393b47",
                                                            borderRadius: "100%"
                                                        }}
                                                        onClick={() => handleCancel(friend.id)}
                                                        hoverColor="rgb(70 72 87)"
                                                        bgColor="#393b47"
                                                    >
                                                        <CloseOutlined />
                                                    </CustomButton>
                                                </Tooltip>
                                            </div>
                                        ))}
                                    </div>
                                )}
                            </>
                        )}
                    </motion.div>
                )}
            </div>
        </div >
    );
}
