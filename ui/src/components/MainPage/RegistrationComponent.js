import React, { Component } from 'react';
import RegistrationForm from './RegistrationFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import GetUserData from '../../helpers/GetUserData';
import ButtonFaceBook from '../MainPage/ButtonFacebook';
import axios from 'axios';
import setDataToLocalStorage from '../../helpers/setDataToLocalStorage';
import SetModalData from '../../helpers/SetModalData';
import 'antd/dist/antd.css';
import { Spin, Space } from 'antd';
import '../../index.css';
import '../../css/styles.css';
import ModalWithMessage from '../common/ModalWithMessage';
import GetFacebookData from './GetFacebookData';
import GetModalPresentation from '../../helpers/GetModalPresentation';

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            spin: false,
            allGood: false,
            modal: GetModalPresentation(this.modalOk, this.modalCancel)
        };
    }

    setCatch = error => {
        const modalData = SetModalData(error);
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                message: modalData.message,
                errors: modalData.errors
            }
        }));
    };
    
    setFinally = () => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: true
            },
            spin: false
        }));
    }

    confirmHandler = async values => {
        this.setState({ spin: true });

        const userData = {
            firstName: values.user.name,
            lastName: values.user.lastname,
            birthDate: values.birthdate._i,
            email: values.email,
            password: values.password
        };
        try {
            const cancelToken = axios.CancelToken.source().token;
            const response = await MakeRequestAsync("account/signup", userData, "post", cancelToken);
            const data = response.data.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);
            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);
            console.log("All good");
            this.setState(oldState => ({
                modal: {
                    ...oldState.modal,
                    message: "A confirm message was sent to your email. Follow the instructions",
                },
                allGood: true
            }));
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    }

    modalOk = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    }

    modalCancel = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    }

    facebookClick = () => { };

    facebookResponseHandler = async (response) => {
        this.setState({ spin: true });
        const cancelToken = axios.CancelToken.source().token;
        const userData = GetFacebookData(response);

        try {
            const reqResponse = await MakeRequestAsync("https://localhost:44382/account/signin-facebook", userData, "post", cancelToken);
            const data = reqResponse.data.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);
            setDataToLocalStorage(user.id, token, role, user.email);
            this.setState(oldState => ({
                modal: {
                    ...oldState.modal,
                    message: "You can now use your Facebook account to enter the system",
                },
                allGood: true
            }));
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    render() {
        const { spin, allGood, modal } = this.state;
        const spinner = <Space size="middle"> <Spin tip="Signing you up..." size="large" /></Space>;
        const modalWindow = ModalWithMessage(modal);
        const signUp =
            <>
                <RegistrationForm onFinish={this.confirmHandler} />
                <ButtonFaceBook facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />
            </>
        return (
            <>
                {spin === true && spinner}
                {spin === false && signUp}
                {allGood === false && modal.visible === true && modalWindow}
                {allGood === true && modal.visible === true && modalWindow}
            </>
        )
    }
}

export default RegistrationComponent;