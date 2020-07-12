import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import GetUserData from '../../helpers/GetUserData';
import { Redirect } from 'react-router-dom';
import axios from 'axios';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props'
import ButtonFaceBook from '../MainPage/ButtonFacebook';
import '../../css/styles.css';

class LoginComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            signal: axios.CancelToken.source()
        };
    }
    confirmHandler = async values => {
        const userData = {
            email: values.username,
            password: values.password
        };
        try {
            const cancelToken = this.state.signal.token;
            const response = await MakeRequestAsync("https://localhost:44382/account/signin", userData, "post", cancelToken);
            const data = response.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);
            localStorage.setItem("bearer_header", `Bearer ${token}`);
            localStorage.setItem("access_token", token);
            localStorage.setItem("current_user_role", role);
            localStorage.setItem("current_user_id", user.id);
            console.log("All good");
            this.setState({ redirect: true });

        } catch (error) {
            console.log(error);
        }

    }

    renderRedirect = () => {
        if (this.state.redirect) {
            return <Redirect to="/courses" push={true} />
        }
    }

    facebookClick = () => {
        console.log("FB clicked");
    }
    facebookResponseHandler = async (response) => {
        console.log(response);
        const cancelToken = this.state.signal.token;
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
        localStorage.setItem("bearer_header", `Bearer ${token}`);
        localStorage.setItem("access_token", token);
        localStorage.setItem("current_user_role", role);
        localStorage.setItem("current_user_id", user.id);
    }

    render() {
        return (
            <>
                {this.state.redirect && this.renderRedirect()}
                {this.state.redirect === false && <NormalLoginFormAntD myConfirHandler={this.confirmHandler}
                    facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />}
                {this.state.redirect === false && <ButtonFaceBook facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />}

            </>
        )
    }
}

export default LoginComponent;