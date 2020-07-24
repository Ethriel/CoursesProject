import React from 'react';
import { Modal } from 'antd';
import GetModalErorrContainer from '../../helpers/GetModalErrorContainer';

const ModalWithMessage = (modalData) => {
    const errorContainer = GetModalErorrContainer(modalData.errors);
    const modalTitle = modalData.message;
    const modalVisible = modalData.visible;
    const modalOk = modalData.modalOk;
    const modalCancel = modalData.modalCancel;
    return (
        <Modal
            title={modalTitle}
            visible={modalVisible}
            onOk={modalOk}
            onCancel={modalCancel}
            closable={true}>
            {errorContainer}
        </Modal>
    )
}

export default ModalWithMessage;