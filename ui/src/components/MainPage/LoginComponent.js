import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import GetUserData from '../../helpers/GetUserData';
import { Redirect } from 'react-router-dom';
import axios from 'axios';

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

    facebookHandler = async () => {
        const cancelToken = this.state.signal.token;
        const response = await MakeRequestAsync("https://localhost:44382/courses/get/all", { msg: "hello" }, "get", cancelToken);
        const data = response.data;
        console.log("DATA", data);
    }
    render() {
        return (
            <>
                {this.state.redirect && this.renderRedirect()}
                {this.state.redirect === false && <NormalLoginFormAntD myConfirHandler={this.confirmHandler} facebookHandler={this.facebookHandler} />}
            </>
        )
    }
}

export default LoginComponent;