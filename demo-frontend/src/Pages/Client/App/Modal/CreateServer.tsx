import { Modal, Card, Space, Typography, Button, Form, Input, message, Upload, Image } from 'antd';
import { PlusCircleOutlined, LinkOutlined, ArrowLeftOutlined, CloseOutlined, LoadingOutlined, PlusOutlined } from '@ant-design/icons';
import { useState } from 'react';
import CustomInput from '../../../../Components/CustomInput';
import type { GetProp, UploadProps } from 'antd';
import { useCreateServer } from '../../../../Connections/AppBackend/Channel';
import { CreateServer } from '../../../../Connections/Types/Channel';
import { on } from 'events';

type FileType = Parameters<GetProp<UploadProps, 'beforeUpload'>>[0];
const { Title, Text } = Typography;
type Step = 'select' | 'create' | 'join';

export default function CreateServerModal({ open, onClose }: { open: boolean; onClose: () => void }) {
    const [step, setStep] = useState<Step>('select');
    const [loading, setLoading] = useState(false);
    const [form] = Form.useForm();
    const [loadingImg, setLoadingImg] = useState(false);
    const [imageUrl, setImageUrl] = useState<string>();
    const serverName = Form.useWatch('name', form);      // string | undefined
    const inviteLink = Form.useWatch('invite', form);
    const isDisabled = step === 'create' ? !serverName : !inviteLink;
    const { mutate, isPending } = useCreateServer();
    const [messageApi, contextHolder] = message.useMessage();

    const onSubmit = async (values: any) => {
        if (step === 'create') {
            const input: CreateServer = {
                name: values.name,
                iconUrl: imageUrl,
                ownerId: localStorage.getItem("userID")?.toString(),
            }
            mutate(input, {
                onSuccess: (response) => {
                    messageApi
                        .open({
                            type: 'loading',
                            content: 'Action in progress..',
                            duration: 0.5,
                        })
                        .then(() => {
                            form.resetFields();
                            setImageUrl("");
                            setLoadingImg(false)
                            setStep('select');
                            onClose();
                        })
                },
                onError: (err) => {
                    console.log(err);
                    messageApi
                        .open({
                            type: 'loading',
                            content: 'Action in progress..',
                            duration: 0.5,
                        })
                        .then(() => message.error(err.response?.data.message, 2.5))
                },
            });
        }
    }

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
                onFinish={onSubmit}
            >
                {isCreate ? (
                    <div>
                        <Upload
                            name="IconUrl"
                            listType="picture-circle"
                            className="server-uploader"
                            showUploadList={false}
                            action={`${process.env.REACT_APP_URL_CHANNEL}/server/UploadIcon`}
                            headers={{
                                Authorization: `Bearer ${localStorage.getItem("token")}`,        // ⬅️ thêm header
                            }}
                            beforeUpload={beforeUpload}
                            onChange={handleChange}
                        >
                            {imageUrl ?
                                <Image
                                    src={imageUrl}
                                    width={100}
                                    height={100}
                                    style={{ borderRadius: '50%', objectFit: 'cover' }}
                                    preview={false}
                                />
                                : uploadButton}
                        </Upload>
                        <Form.Item
                            label="Server Name"
                            name="name"
                        >
                            <CustomInput style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} className='input-create-server' />
                        </Form.Item>
                    </div>

                ) : (
                    <Form.Item
                        label="Invite Link"
                        name="invite"
                    >
                        <CustomInput placeholder="https://kendz.site/xyz" style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} className='input-create-server' />
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
                        disabled={isDisabled}
                        block
                        style={{ width: 100, backgroundColor: "#5865f2" }}
                    >
                        {isCreate ? 'Create' : 'Join'}
                    </Button>
                </div>

            </Form>
        );
    };

    const getBase64 = (img: FileType, callback: (url: string) => void) => {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result as string));
        reader.readAsDataURL(img);
    };

    const beforeUpload = (file: FileType) => {
        const isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
        if (!isJpgOrPng) {
            message.error('You can only upload JPG/PNG file!');
        }
        const isLt2M = file.size / 1024 / 1024 < 2;
        if (!isLt2M) {
            message.error('Image must smaller than 2MB!');
        }
        return isJpgOrPng && isLt2M;
    };

    const handleChange: UploadProps['onChange'] = (info) => {
        if (info.file.status === 'uploading') {
            setLoadingImg(true);
            return;
        }
        if (info.file.status === 'done') {
            // Get this url from response in real world.
            console.log(info)
            getBase64(info.file.originFileObj as FileType, (url) => {
                setLoadingImg(false);
                setImageUrl(info.file.response.data);
            });
        }
    };

    const uploadButton = (
        <button style={{ border: 0, background: 'none', cursor: "pointer" }} type="button">
            {loadingImg ? <LoadingOutlined style={{ color: "#fff" }} /> : <PlusOutlined style={{ color: "#fff" }} />}
            <div style={{ marginTop: 8, color: "#fff" }}>Upload</div>
        </button>
    );

    return (
        <Modal
            open={open}
            onCancel={() => {
                form.resetFields();
                setImageUrl("");
                setLoadingImg(false)
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
            {contextHolder}
            {renderContent()}
        </Modal>
    );
}
