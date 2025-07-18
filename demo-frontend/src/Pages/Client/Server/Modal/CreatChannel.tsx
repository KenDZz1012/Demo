import React, { useState } from 'react';
import { Modal, Form, Input, Radio, Button } from 'antd';
import { SoundFilled, CloseOutlined } from '@ant-design/icons';
import CustomInput from '../../../../Components/CustomInput';
import CustomButton from '../../../../Components/CustomButton';

interface CreateChannelModalProps {
    visible: boolean;
    onCreate: (values: { name: string; type: 'text' | 'voice' }) => void;
    onCancel: () => void;
}

const CreateChannelModal: React.FC<CreateChannelModalProps> = ({
    visible,
    onCreate,
    onCancel,
}) => {
    const [form] = Form.useForm();
    const [channelType, setChannelType] = useState<'text' | 'voice'>('text');


    return (
        <Modal
            open={visible}
            title={<p style={{ fontWeight: 'bold', fontSize: 22 }}>Create Channel</p>}
            className="dark-modal"
            closeIcon={
                <CloseOutlined style={{ color: 'white', fontSize: 20 }} />
            }
            footer={null}
            onCancel={() => {
                form.resetFields();
                onCancel()
            }}
        >
            <Form form={form} layout="vertical">
                <Form.Item>
                    <Radio.Group
                        value={channelType}
                        onChange={(e) => setChannelType(e.target.value)}
                        style={{ width: '100%' }}
                        size="large"
                    >
                        <div
                            onClick={() => setChannelType("text")}
                            style={{
                                backgroundColor: channelType === "text" ? "#393b47" : "#2a2c35",
                                padding: 10,
                                borderRadius: 10,
                                cursor: 'pointer',
                            }}
                        >
                            <Radio value="text" style={{ color: "#fff" }} >
                                <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                                    <span style={{ fontSize: 32 }}>#</span>
                                    <div style={{ display: 'flex', flexDirection: "column", marginLeft: 26 }}>
                                        <span style={{ fontSize: 16, fontWeight: 500 }}>Text</span>
                                        <span style={{ fontSize: 14 }}>Send Messages, images, GIFs, emoji, opinions, and puns</span>
                                    </div>
                                </div>
                            </Radio>
                        </div>

                        <div
                            onClick={() => setChannelType("voice")}
                            style={{
                                backgroundColor: channelType === "voice" ? "#393b47" : "#2a2c35",
                                padding: 10,
                                borderRadius: 10,
                                cursor: 'pointer',
                            }}
                        >
                            <Radio value="voice" style={{ color: "#fff" }}>
                                <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center' }}>
                                    <span style={{ fontSize: 28 }}><SoundFilled /></span>
                                    <div style={{ display: 'flex', flexDirection: "column", marginLeft: 18 }}>
                                        <span style={{ fontSize: 16, fontWeight: 500 }}>Voice</span>
                                        <span style={{ fontSize: 14 }}>Hang out together with voice, video, and screen share</span>
                                    </div>
                                </div>
                            </Radio>
                        </div>
                    </Radio.Group>

                </Form.Item>

                <Form.Item
                    label="Channel Name"
                    name="name"
                >
                    <CustomInput prefix={channelType == "text" ? "#" : <SoundFilled />} placeholder='new-channel' size="large" style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} className='input-create-server' />
                </Form.Item>
                <div style={{ display: "flex", justifyContent: "flex-end" }}>
                    <CustomButton
                        onClick={() => {
                            form.resetFields();
                            onCancel()
                        }}
                        style={{ color: "#fff", border: "1px solid #393b47", marginRight: 10, width: 100, backgroundColor: "#393b47" }}
                        size="large"
                        hoverColor="rgb(70 72 87)"
                        bgColor="#393b47"
                    >
                        Cancel
                    </CustomButton>
                    <CustomButton
                        htmlType="submit"
                        type="primary"
                        // loading={isPending}
                        // disabled={isDisabled}
                        block
                        style={{ width: 140, backgroundColor: "#5865f2" }}
                        size="large"
                    >
                        Create channel
                    </CustomButton>
                </div>
            </Form>
        </Modal >
    );
};

export default CreateChannelModal;
