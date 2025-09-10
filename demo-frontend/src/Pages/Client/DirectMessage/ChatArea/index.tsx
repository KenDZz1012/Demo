import { Input } from 'antd';
import { motion } from 'framer-motion';
import { Friend } from 'types/user';

interface ChatAreaProps {
    friend: Friend | null | undefined;
    input: string;
    setInput: (value: string) => void;
    sendMessage: () => void;
}

export default function ChatArea({ friend, input, setInput, sendMessage }: ChatAreaProps) {
    return (
        <div style={{ backgroundColor: '#31323dff', display: 'flex', flexDirection: 'column', height: '100%', borderTopRightRadius: 20, borderBottomRightRadius: 20 }}>
            <div style={{ borderBottom: '1px solid #555', height: 59, display: 'flex', alignItems: 'center', paddingLeft: 16 }}>
                <div>
                    <div style={{ position: 'relative', width: 30, height: 30 }}>
                        <img
                            src={friend?.avatarUrl || '/logo.png'}
                            alt={friend?.displayName}
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
                                backgroundColor: friend?.isOnline ? 'green' : 'gray',
                                borderRadius: '50%',
                                border: '2px solid white',
                            }}
                        />
                    </div>
                </div>
                <h3 style={{ color: 'white', marginLeft: 10 }}>{friend?.displayName}</h3>
            </div>
            <div style={{ flex: 1, overflowY: 'auto', marginBottom: 12, padding: 20 }}>
                {/* {messages.map((msg, i) => (
                    <motion.div
                        key={i}
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        style={{ background: '#4f545c', padding: 8, marginBottom: 4, borderRadius: 4, color: 'white' }}
                    >
                        {msg}
                    </motion.div>
                ))} */}
            </div>
            <div style={{ padding: 20 }}>
                <Input.Search
                    placeholder="Type your message..."
                    value={input}
                    onChange={(e) => setInput(e.target.value)}
                    onSearch={sendMessage}
                    enterButton="Send"
                    size="large"
                />
            </div>
        </div>
    );
}
