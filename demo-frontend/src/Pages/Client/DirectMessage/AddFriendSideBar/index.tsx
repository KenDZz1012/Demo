import { Input, Tabs } from 'antd';
import { useState } from 'react';
import { motion } from 'framer-motion';
import { TeamOutlined, } from '@ant-design/icons';
import CustomInput from 'Components/CustomInput';
import CustomButton from 'Components/CustomButton';
import { useAddFriend } from 'Connections/AppBackend/UserRelationship';

const { TabPane } = Tabs;

export default function AddFriendSideBar() {
    const [activeTab, setActiveTab] = useState('add');
    const [addresseeName, setAddresseeName] = useState('');
    const { mutate, isSuccess, isError } = useAddFriend();
    const [messageSubmit, setMessageSubmit] = useState('');

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
                        <h3>Pending Requests</h3>
                        <p>No pending friend requests.</p>
                    </motion.div>
                )}
            </div>
        </div >
    );
}
