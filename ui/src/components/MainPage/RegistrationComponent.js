import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
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

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            spin: false
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
            setDataToLocalStorage(user.id, token, role);
            console.log("All good");

            this.setState({ spin: false });

            this.setState({ redirect: true });
        } catch (error) {
            console.log(error);
        }
    }

    renderRedirect = () => {
        if (this.state.redirect) {
            return <Redirect to="/courses" />
        }
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
        setDataToLocalStorage(user.id, token, role);

        this.setState({ spin: false });

        this.setState({ redirect: true });
    }
    render() {
        const { spin } = this.state;
        const spinner = <Space size="middle"> <Spin tip="Signing you up..." size="large" /></Space>;
        const signUp =
            <>
                {this.state.redirect && this.renderRedirect()}
                {this.state.redirect === false && <RegistrationForm onFinish={this.confirmHandler} />}
                {this.state.redirect === false && <ButtonFaceBook facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />}
            </>
        return (
            <>
                {spin === true && spinner}
                {spin === false && signUp}
            </>
        )
    }
}

export default RegistrationComponent;