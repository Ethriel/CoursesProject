import React, { Component } from 'react';
import RegistrationForm from './RegistrationFormAntD';
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

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            spin: false,
            allGood: false,
            modalVisible: false,
            modalMessage: "",
            modalTitle: ""
        };
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
            const data = response.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);
            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);
            console.log("All good");
            this.setState({
                allGood: true,
                modalMessage: "A confirm message was sent to your email. Follow the instructions",
                modalTitle: "Sign up successful",
            });
        } catch (error) {
            console.log(error.response.data);
            const data = error.response.data;
            this.setState({
                modalMessage: `${data.message}`,
                modalTitle: "Sign up has failed"
            });
        } finally {
            this.setState({
                spin: false,
                modalVisible: true
            });
        }
    }
    modalOk = (e) => {
        this.setState({ modalVisible: false });
    }
    facebookClick = () => { }
    facebookResponseHandler = async (response) => {
        this.setState({ spin: true });

        console.log(response);

        const cancelToken = axios.CancelToken.source().token;

        const userData = {
            firstName: response.first_name,
            lastName: response.last_name,
            email: response.email,
            accessToken: response.accessToken,
            pictureUrl: response.picture.data.url,
            userId: response.userID
        };

        const reqResponse = await MakeRequestAsync("https://localhost:44382/account/signin-facebook", userData, "post", cancelToken);
        console.log(reqResponse);
        const data = reqResponse.data;
        const token = data.token.key;
        const role = data.user.roleName;
        const user = GetUserData(data.user);
        setDataToLocalStorage(user.id, token, role, user.email);
        this.setState({
            spin: false,
            allGood: true,
            modalMessage: "You can now use your Facebook account to enter the system",
            modalTitle: "Sign up successful"
        });
    }
    render() {
        const { spin, modalVisible, allGood, modalMessage, modalTitle } = this.state;
        const spinner = <Space size="middle"> <Spin tip="Signing you up..." size="large" /></Space>;
        const modal =
            <ModalWithMessage
                modalTitle={modalTitle}
                modalVisible={modalVisible}
                modalOk={this.modalOk}
                modalCancel={this.modalCancel}
                modalMessage={modalMessage} />;
        const signUp =
            <>
                <RegistrationForm onFinish={this.confirmHandler} />
                <ButtonFaceBook facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />
            </>
        return (
            <>
                {spin === true && spinner}
                {spin === false && signUp}
                {allGood === false && modalVisible === true && modal}
                {allGood === true && modalVisible === true && modal}
            </>
        )
    }
}

export default RegistrationComponent;