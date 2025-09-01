import { Modal } from "antd";
import React from "react";
import { SoundFilled, CloseOutlined, SearchOutlined } from '@ant-design/icons';
import { ServerDetail } from "types";
import CustomInput from "Components/CustomInput";
import { useSelector } from "react-redux";
import { selectFriends } from "store/selectors/authSelectors";

interface InvitePeopleModalProps {
    visible: boolean;
    onCancel: () => void;
    server: ServerDetail | null;
}

const InvitePeopleModal: React.FC<InvitePeopleModalProps> = ({ visible, onCancel, server }) => {

    return (
        <Modal open={visible} className="dark-modal"
            closeIcon={
                <CloseOutlined style={{ color: 'white', fontSize: 20 }} />
            }
            footer={null}
            onCancel={() => {
                onCancel()
            }}
            title={<p style={{ fontWeight: 'bold', fontSize: 15, textAlign: "left", marginTop: 0 }}>Invite people to {server?.name}</p>}
        >
            <CustomInput placeholder="Seach for friends" className='input-create-server' style={{ backgroundColor: "#212126", color: "#fff", borderColor: "#212126" }} icon={<SearchOutlined />} iconPosition="right" />

        </Modal>
    )
}

export default InvitePeopleModal;
