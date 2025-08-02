import { Tabs, Tooltip } from 'antd';
import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { TeamOutlined, CloseOutlined, CheckOutlined, StopOutlined } from '@ant-design/icons';
import CustomInput from 'Components/CustomInput';
import CustomButton from 'Components/CustomButton';
import {
    useAddFriend,
    useCancelFriendRequest,
    useUpdateUserRelationship,
} from 'Connections/AppBackend/UserRelationship';
import { FriendPending } from 'types/user';
import { useSelector } from 'react-redux';
import { selectAuthUser } from 'store/selectors/authSelectors';
import { userRelationshipStatus } from 'shared';
import { usePresenceSocket } from 'signalr/usePresenceSocket';

const { TabPane } = Tabs;

export default function AddFriendSideBar({ friendPending }: { friendPending: FriendPending[] }) {
    const [activeTab, setActiveTab] = useState('add');
    const [addresseeName, setAddresseeName] = useState('');
    const [messageSubmit, setMessageSubmit] = useState('');
    const [pendingList, setPendingList] = useState(friendPending);
    const sentRequests = pendingList.filter(friend => !friend.isSender);
    const receivedRequests = pendingList.filter(friend => friend.isSender);
    const { id: ownerId } = useSelector(selectAuthUser) || {};
    const { mutate: sendRequest, isSuccess, isError } = useAddFriend();
    const { mutate: acceptRequest } = useUpdateUserRelationship();
    const { mutate: cancelRequest } = useCancelFriendRequest();
    usePresenceSocket(ownerId || '', (fromUserId, fromUserName, fromUserDisplayName, fromUserAvatarUrl) => {
        setPendingList(prev => {
            const exists = prev.some(friend => friend.id === fromUserId && !friend.isSender);
            if (exists) return prev;

            return [
                ...prev,
                {
                    id: fromUserId,
                    userName: fromUserName,
                    displayName: fromUserDisplayName,
                    isSender: false,
                    avatarUrl: fromUserAvatarUrl || '',
                },
            ];
        });
    });


    const onSubmit = () => {
        if (!addresseeName.trim()) return;
        const input = {
            requesterId: ownerId || '',
            addresseeName
        };

        sendRequest(input, {
            onSuccess: () => {
                setMessageSubmit(`Friend request sent successfully to ${addresseeName}`);
            },
            onError: (err) => {
                setMessageSubmit(err.response?.data.message || 'Add friend failed');
            }
        });
    };

    useEffect(() => {
        setAddresseeName('');
    }, [activeTab]);

    const handleAccept = (friendId: string) => {
        acceptRequest({
            userID: ownerId || '',
            friendID: friendId,
            status: userRelationshipStatus.Accepted
        });
    };

    const handleCancel = (friendId: string) => {
        cancelRequest({
            userID: ownerId || '',
            friendID: friendId,
        });
    };
    useEffect(() => {
        setPendingList(friendPending);
    }, [friendPending]);

    return (
        <div style={{ backgroundColor: 'rgb(48 48 49)', display: 'flex', flexDirection: 'column', height: '100%', borderTopRightRadius: 20, borderBottomRightRadius: 20 }}>
            <div style={{ borderBottom: '1px solid #555', height: 59, alignItems: 'center', paddingLeft: 16, display: 'flex' }}>
                <div>
                    <TeamOutlined style={{ fontSize: 20, color: '#fff' }} />
                    <span style={{ marginLeft: 8, color: '#fff', fontSize: 16, fontWeight: 600 }}>Friends</span>
                </div>
                <Tabs activeKey={activeTab} onChange={setActiveTab} tabBarStyle={{ marginBottom: 0, borderBottom: 'none', marginLeft: 32 }}>
                    <TabPane tab={<div className={`custom-tab ${activeTab === 'pending' ? 'active' : ''}`}>Pending</div>} key="pending" />
                    <TabPane tab={<div className={`custom-tab ${activeTab === 'add' ? 'active' : ''}`}>Add Friend</div>} key="add" />
                </Tabs>
            </div>

            <div style={{ padding: 16, color: '#fff', flex: 1 }}>
                {activeTab === 'add' && (
                    <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.3 }}>
                        <div style={{ textAlign: 'left', fontSize: 18, fontWeight: 500, marginBottom: 16 }}>
                            <h3>Add Friend</h3>
                            <h5 style={{ fontWeight: 400 }}>You can add friends with their KenVerse username</h5>
                        </div>
                        <div style={{ position: 'relative', width: '100%' }}>
                            <CustomInput
                                placeholder='You can add friends with their KenVerse username'
                                size="large"
                                style={{ backgroundColor: '#212126', color: '#fff', borderColor: isSuccess ? '#28a745' : isError ? '#dc3545' : '#212126', paddingRight: 120, height: 50 }}
                                className="input-create-server"
                                onPressEnter={onSubmit}
                                onChange={(e) => setAddresseeName(e.target.value)}
                            />
                            <CustomButton
                                style={{ position: 'absolute', top: 4, right: 4, height: 'calc(100% - 8px)', backgroundColor: '#5865F2', color: '#fff', border: 'none', padding: '0 16px' }}
                                disabled={!addresseeName.trim()}
                                onClick={onSubmit}
                            >
                                Send Friend Request
                            </CustomButton>
                        </div>
                        <span style={{ color: isSuccess ? '#28a745' : isError ? '#dc3545' : '#212126', float: 'left', marginTop: 8 }}>{messageSubmit}</span>
                    </motion.div>
                )}

                {activeTab === 'pending' && (
                    <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ duration: 0.3 }}>
                        {pendingList.length === 0 ? (
                            <p style={{ color: '#aaa' }}>No pending friend requests.</p>
                        ) : (
                            <>
                                {receivedRequests.length > 0 && (
                                    <div style={{ marginBottom: 24 }}>
                                        <h4 style={{ color: '#fff', marginBottom: 12, textAlign: 'left', paddingLeft: 10, fontWeight: 500 }}>Receive ({receivedRequests.length})</h4>
                                        {receivedRequests.map(friend => (
                                            <RequestItem key={friend.id} friend={friend} onAccept={handleAccept} onCancel={handleCancel} />
                                        ))}
                                    </div>
                                )}
                                {sentRequests.length > 0 && (
                                    <div>
                                        <h4 style={{ color: '#fff', marginBottom: 12, textAlign: 'left', paddingLeft: 10, fontWeight: 500 }}>Sent ({sentRequests.length})</h4>
                                        {sentRequests.map(friend => (
                                            <RequestItem key={friend.id} friend={friend} onCancel={handleCancel} />
                                        ))}
                                    </div>
                                )}
                            </>
                        )}
                    </motion.div>
                )}
            </div>
        </div>
    );
}

function RequestItem({ friend, onAccept, onCancel }: {
    friend: FriendPending,
    onAccept?: (id: string) => void,
    onCancel: (id: string) => void
}) {
    return (
        <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', backgroundColor: '#2c2c2f', padding: '12px 16px', borderRadius: 8, marginBottom: 12 }}>
            <div style={{ display: 'flex', alignItems: 'center' }}>
                <img
                    src={friend.avatarUrl || '/logo.png'}
                    alt={friend.displayName}
                    style={{ width: 40, height: 40, borderRadius: '50%', objectFit: 'cover', marginRight: 12 }}
                />
                <div>
                    <div style={{ fontWeight: 600, color: '#fff', textAlign: 'left' }}>{friend.displayName}</div>
                    <div style={{ fontSize: 13, color: '#aaa', textAlign: 'left' }}>{friend.userName}</div>
                </div>
            </div>
            <div>
                {onAccept && (
                    <Tooltip title="Accept">
                        <CustomButton
                            style={{ width: 40, height: 40, color: '#fff', border: '1px solid #393b47', backgroundColor: '#393b47', borderRadius: '100%', marginRight: 12 }}
                            onClick={() => onAccept(friend.id)}
                            hoverColor="#28a745"
                            bgColor="#393b47"
                        >
                            <CheckOutlined />
                        </CustomButton>
                    </Tooltip>
                )}
                <Tooltip title={onAccept ? 'Reject' : 'Cancel'}>
                    <CustomButton
                        style={{ width: 40, height: 40, color: '#fff', border: '1px solid #393b47', backgroundColor: '#393b47', borderRadius: '100%' }}
                        onClick={() => onCancel(friend.id)}
                        hoverColor={onAccept ? '#dc3545' : 'rgb(70 72 87)'}
                        bgColor="#393b47"
                    >
                        {onAccept ? <StopOutlined /> : <CloseOutlined />}
                    </CustomButton>
                </Tooltip>
            </div>
        </div>
    );
}
