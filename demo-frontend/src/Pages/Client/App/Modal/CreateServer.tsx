import { Modal, Card, Space, Typography, Button, Form, Input, message } from 'antd';
import { PlusCircleOutlined, LinkOutlined, ArrowLeftOutlined, CloseOutlined } from '@ant-design/icons';
import { useState } from 'react';

const { Title, Text } = Typography;

type Step = 'select' | 'create' | 'join';

export default function CreateServerModal({ open, onClose }: { open: boolean; onClose: () => void }) {
    const [step, setStep] = useState<Step>('select');
    const [loading, setLoading] = useState(false);

    const [form] = Form.useForm();



    const renderContent = () => {
        if (step === 'select') {
            return (
                <Space direction="vertical" size="large" style={{ width: '100%' }}>
                    <Card
                        hoverable
                        onClick={() => setStep('create')}
                        style={{ textAlign: 'center', borderRadius: 12, backgroundColor: "#001529", borderColor: "#001529" }}
                    >
                        <Space direction="vertical">
                            <PlusCircleOutlined style={{ fontSize: 40, color: '#1677ff' }} />
                            <Title level={4} style={{ color: "#fff" }}>Create My Own</Title>
                            <Text type="secondary" style={{ color: "#fff" }}>Start a new server and invite friends</Text>
                        </Space>
                    </Card>
                    <Card
                        hoverable
                        onClick={() => setStep('join')}
                        style={{ textAlign: 'center', borderRadius: 12, backgroundColor: "#001529", borderColor: "#001529" }}
                    >
                        <Space direction="vertical">
                            <LinkOutlined style={{ fontSize: 40, color: '#52c41a' }} />
                            <Title level={4} style={{ color: "#fff" }}>Join a Server</Title>
                            <Text type="secondary" style={{ color: "#fff" }}>Enter an invite link to join existing</Text>
                        </Space>
                    </Card>
                </Space>
            );
        }

        const isCreate = step === 'create';
        return (
            <Form
                layout="vertical"
                form={form}
                style={{ marginTop: 12 }}
            >
                {isCreate ? (
                    <Form.Item
                        label="Server Name"
                        name="name"
                    >
                        <Input style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} />
                    </Form.Item>
                ) : (
                    <Form.Item
                        label="Invite Link"
                        name="invite"
                    >
                        <Input placeholder="https://discord.gg/xyz" style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} />
                    </Form.Item>
                )}
                <div style={{ display: "flex", justifyContent: "space-between" }}>
                    <Button
                        icon={<ArrowLeftOutlined />}
                        type="link"
                        onClick={() => {
                            form.resetFields();
                            setStep('select');
                        }}
                        style={{ paddingLeft: 0, color: "#fff" }}
                    >
                        Back
                    </Button>
                    <Button
                        htmlType="submit"
                        type="primary"
                        loading={loading}
                        block
                        style={{ width: 100, backgroundColor: "#5865f2" }}
                    >
                        {isCreate ? 'Create' : 'Join'}
                    </Button>
                </div>

            </Form>
        );
    };

    return (
        <Modal
            open={open}
            onCancel={() => {
                form.resetFields();
                setStep('select');
                onClose();
            }}
            footer={null}
            title={
                step == "select" ?
                    <div style={{ textAlign: "center" }}>
                        <p style={{ fontWeight: 'bold', fontSize: 22 }}>Create Your Server</p>
                        <p>Your server is where you and your friends hang out. Make yours and start talking</p>
                    </div>
                    : step == "create" ?
                        <div style={{ textAlign: "center" }}>
                            <p style={{ fontWeight: 'bold', fontSize: 22 }}>Customize Your Server</p>
                            <p>Give your new server a  personality with a name and an icon. You can always change it later.</p>
                        </div>
                        : <div style={{ textAlign: "center" }}>
                            <p style={{ fontWeight: 'bold', fontSize: 22 }}>Join a Server</p>
                            <p>Enter an invite below to join an existing server.</p>
                        </div>
            }
            centered
            width={400}
            destroyOnClose
            className="dark-modal"
            closeIcon={
                <CloseOutlined style={{ color: 'white', fontSize: 20 }} />  // ⬅️ tô màu & cỡ theo ý
            }
        >
            {renderContent()}
        </Modal>
    );
}
