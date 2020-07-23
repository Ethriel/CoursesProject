import React from 'react';
import { Modal } from 'antd';
import H from './HAntD';

const ModalWithMessage = (props) => {
    const modalTitle = props.modalTitle;
    const modalVisible = props.modalVisible;
    const modalOk = props.modalOk;
    const modalCancel = props.modalCancel;
    const modalMessage = props.modalMessage;
    return (
        <Modal
            title={modalTitle}
            visible={modalVisible}
            onOk={modalOk}
            onCancel={modalCancel}>
            <H
                level={4}
                myText={modalMessage} />
        </Modal>
    )
}

export default ModalWithMessage;