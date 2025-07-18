import { Input } from 'antd';
import { motion } from 'framer-motion';

interface ChatAreaProps {
    channelName?: string;
    messages: string[];
    input: string;
    setInput: (value: string) => void;
    sendMessage: () => void;
}

export default function ChatArea({ channelName, messages, input, setInput, sendMessage }: ChatAreaProps) {
    return (
        <div style={{ backgroundColor: '#363851', display: 'flex', flexDirection: 'column', height: '100%', borderTopRightRadius: 20, borderBottomRightRadius: 20 }}>
            <div style={{ borderBottom: '1px solid #555', height: 56, display: 'flex', alignItems: 'center', paddingLeft: 16 }}>
                <h2 style={{ color: 'white' }}>#{channelName}</h2>
            </div>
            <div style={{ flex: 1, overflowY: 'auto', marginBottom: 12, padding: 20 }}>
                {messages.map((msg, i) => (
                    <motion.div
                        key={i}
                        initial={{ opacity: 0, y: 10 }}
                        animate={{ opacity: 1, y: 0 }}
                        style={{ background: '#4f545c', padding: 8, marginBottom: 4, borderRadius: 4, color: 'white' }}
                    >
                        {msg}
                    </motion.div>
                ))}
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
