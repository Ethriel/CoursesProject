import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import ButtonFaceBook from './ButtonFacebook';
import MakeRequest from '../../helpers/MakeRequest';

class LoginComponent extends Component {
    confirmHandler = async values => {
        let userData = {
            email: values.username,
            password: values.password
        };
        try {
            const data = await MakeRequest("https://localhost:44382/account/signin", userData, "post");
            const token = data.token.key;
            const role = data.user.roleName;
            localStorage.setItem("bearer_header", `Bearer ${token}`);
            localStorage.setItem("access_token", token);
            localStorage.setItem("current_user_role", role);
            console.log("All good");
        } catch (error) {
            console.log(error);
        }

    }

    facebookHandler = async () => {
        const data = await MakeRequest("https://localhost:44382/courses/get/all", { msg: "hello" }, "get");
        console.log("DATA", data);
    }
    render() {
        return (
            <>
                <NormalLoginFormAntD myConfirHandler={this.confirmHandler} />
                <ButtonFaceBook clickHandler={this.facebookHandler} />
            </>
        )
    }
}

export default LoginComponent;