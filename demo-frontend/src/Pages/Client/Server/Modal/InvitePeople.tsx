import { Modal } from "antd";
import React from "react";

interface InvitePeopleModalProps {
    visible: boolean;
}

const InvitePeopleModal: React.FC<InvitePeopleModalProps> = ({ visible }) => {
    return (
        <Modal open={visible}>

        </Modal>
    )
}

export default InvitePeopleModal;
