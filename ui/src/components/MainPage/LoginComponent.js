import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import GetUserData from '../../helpers/GetUserData';
import ButtonFaceBook from '../MainPage/ButtonFacebook';
import axios from 'axios';
import setDataToLocalStorage from '../../helpers/setDataToLocalStorage';
import 'antd/dist/antd.css';
import { Spin, Space } from 'antd';
import '../../index.css';
import '../../css/styles.css';
import ModalWithMessage from '../common/ModalWithMessage';

class LoginComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            spin: false,
            modalVisible: false,
            modalMessage: "",
            modalTitle: ""
        };
    }
    confirmHandler = async values => {
        this.setState({ spin: true });

        const userData = {
            email: values.username,
            password: values.password
        };
        try {
            const cancelToken = axios.CancelToken.source().token;

            const response = await MakeRequestAsync("account/signin", userData, "post", cancelToken);
            const data = response.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);

            console.log("All good");

            this.setState({ redirect: true });
        } catch (error) {
            const data = error.response.data;
            const message = data.message;
            this.setState({
                modalMessage: message,
                modalTitle: "Sign in has failed",
                modalVisible: true
            });
            console.log(error);
        } finally {
            this.setState({
                spin: false,
            });
        }
    }

    renderRedirect = () => {
        const role = localStorage.getItem("current_user_role");
        if (this.state.redirect) {
            const redirectDirection = role === "ADMIN" ? "/admin" : "/courses";
            return <Redirect to={redirectDirection} push={true} />
        }
    }

    facebookClick = () => { }
    facebookResponseHandler = async (response) => {
        const cancelToken = axios.CancelToken.source().token;
        const userData = {
            firstName: response.first_name,
            lastName: response.last_name,
            email: response.email,
            accessToken: response.accessToken,
            pictureUrl: response.picture.data.url,
            userId: response.userID
        };

        try {
            const reqResponse = await MakeRequestAsync("account/signin-facebook", userData, "post", cancelToken);

            const data = reqResponse.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);

            this.setState({ redirect: true });
        } catch (error) {
            const data = error.response.data;
            const message = data.message;
            this.setState({
                modalMessage: message,
                modalTitle: "Sign in has failed",
                modalVisible: true
            });
        } finally {
            this.setState({
                spin: false,
            });
        }

    }

    modalOk = (e) => {
        this.setState({ modalVisible: false });
    }
    modalCancel = (e) => {
        this.setState({ modalVisible: false });
    }
    render() {
        const { spin, modalVisible, modalMessage, modalTitle } = this.state;
        const modal =
            <ModalWithMessage
                modalTitle={modalTitle}
                modalVisible={modalVisible}
                modalOk={this.modalOk}
                modalCancel={this.modalCancel}
                modalMessage={modalMessage} />;

        const spinner = <Space size="middle"> <Spin tip="Signing you in..." size="large" /></Space>;

        const login =
            <>
                {this.state.redirect && this.renderRedirect()}
                {this.state.redirect === false && <NormalLoginFormAntD myConfirHandler={this.confirmHandler} />}
                {this.state.redirect === false && <ButtonFaceBook facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />}
            </>;

        return (
            <>
                {spin === true && spinner}
                {spin === false && login}
                {modalVisible === true && modal}
            </>
        )
    }
}

export default LoginComponent;