import React from 'react';
import { withRouter } from 'react-router-dom';
import H from '../../common/HAntD';
import { Spin, Space } from 'antd';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from "axios";
import { connect } from 'react-redux';
import { USER, ADMIN } from '../../common/roles';
import { forbidden } from '../../../Routes/RoutersDirections';
import GetModalPresentation from '../../../helpers/GetModalPresentation';
import ModalWithMessage from '../../common/ModalWithMessage';
import SetModalData from '../../../helpers/SetModalData';
const queryString = require('query-string');

class ConfirmChangeEmail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirmed: false,
            id: this.props.currentUser.id,
            email: localStorage.getItem("new_email"),
            modal: GetModalPresentation(this.modalOk, this.modalCancel)
        }
    }

    componentDidMount = async () => {
        const role = this.props.currentUser.role;
        if (role !== USER && role !== ADMIN) {
            this.props.history.push(forbidden);
        }
        else {
            const location = this.props.location;
            const search = location.search;
            const parsed = queryString.parse(search);
            const token = parsed.token;
            try {
                const signal = axios.CancelToken.source();
                const requestData = {
                    id: this.state.id,
                    token: token,
                    email: this.state.email
                };
                const response = await MakeRequestAsync("account/confirmChangeEmail", requestData, "post", signal.token);
                if (response.status === 204) {
                    this.setState({
                        confirmed: true
                    });
                }
            } catch (error) {
                this.setCatch(error);
            }
        }
    };

    setModal = () => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    };

    modalOk = (e) => {
        this.setModal();
    };

    modalCancel = (e) => {
        this.setModal();
    };

    setCatch = (error) => {
        const modalData = SetModalData(error);
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                message: modalData.message,
                errors: modalData.errors,
                visible: true
            }
        }));
    };

    render() {
        const { confirmed, modal } = this.state;
        const info = <H level={4} myText="Email confirmed" />;
        const modalWindow = ModalWithMessage(modal);
        const spinner = <Space size="middle"> <Spin tip="Confirming your email..." size="large" /></Space>;
        return (
            <>
                {modal.visible === true && modalWindow}
                {confirmed === false && spinner}
                {confirmed === true && info}
            </>
        );
    }
}

export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    })
)(ConfirmChangeEmail));