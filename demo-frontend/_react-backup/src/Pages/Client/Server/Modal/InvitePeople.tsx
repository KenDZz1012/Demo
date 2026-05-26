import { Modal } from "antd";
import React from "react";
import { CloseOutlined, SearchOutlined, CopyOutlined } from '@ant-design/icons';
import { ServerDetail } from "types";
import CustomInput from "Components/CustomInput";
import CustomButton from "Components/CustomButton";

interface InvitePeopleModalProps {
    visible: boolean;
    onCancel: () => void;
    server: ServerDetail | null;
}

const InvitePeopleModal: React.FC<InvitePeopleModalProps> = ({ visible, onCancel, server }) => {

    const handleCopy = () => {
        if (server?.code) {
            navigator.clipboard.writeText(server.code);
        }
    };

    return (
        <Modal open={visible} className="dark-modal"
            closeIcon={
                <CloseOutlined style={{ color: 'white', fontSize: 20 }} />
            }
            footer={null}
            onCancel={() => {
                onCancel()
            }}
            title={<p style={{ fontWeight: 500, fontSize: 15, textAlign: "left", marginTop: 0 }}>Invite people to {server?.name}</p>}
        >
            <CustomInput placeholder="Seach for friends" className='input-create-server' style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} icon={<SearchOutlined />} iconPosition="right" />
            <p style={{ fontWeight: 500, fontSize: 15, textAlign: "left", marginTop: 16, marginBottom: 0 }}>Send a server invite link to a friend</p>
            <div style={{ backgroundColor: "#2f3136", padding: 10, borderRadius: 5, marginTop: 8, color: "#ebebebff", fontSize: 16, display: "flex", justifyContent: "space-between", alignItems: "center" }}>
                {server?.code}
                <CustomButton bgColor="#5865f2" hoverColor="#4957f1ff" style={{ border: "none" }} title="Copy" onClick={handleCopy}>
                    <CopyOutlined style={{ color: "#fff", fontSize: 16 }} />
                </CustomButton>
            </div>
        </Modal>
    )
}

export default InvitePeopleModal;
