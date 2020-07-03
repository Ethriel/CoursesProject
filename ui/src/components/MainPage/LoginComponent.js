import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import axios from "axios";
import Cookies from "js-cookie";
import ButtonFaceBook from './ButtonFacebook';

class LoginComponent extends Component {
    confirmHandler = async values => {
        let userData = {
            email: values.username,
            password: values.password
        };
        try {
            axios.defaults.withCredentials = true;
            const response = await axios.post("https://localhost:44382/account/signin",
                userData,
                {
                    headers: {
                        "Content-Type": "application/json",
                        "Access-Control-Allow-Origin": "*"
                    }
                }).catch((reason) => {
                    console.log(reason);
                })

            if (response.status === 200) {
                console.log(response.data);
                const token = response.data.access_token;
                const role = response.data.userRole;
                Cookies.set("access_token", response.data.access_token, { expires: response.data.expires });
                localStorage.setItem("bearer_header", `Bearer ${token}`);
                localStorage.setItem("access_token", token);
                localStorage.setItem("current_user_role", role);
            }
        } catch (error) {
            console.log(error);
        }

    }

    facebookHandler = async () => {
        const url = "https://localhost:44382/account/protected";
        //axios.defaults.withCredentials = true;
        axios.defaults.headers.post["Authorization"] = localStorage.getItem("bearer_header");
        const response = await axios.post(
            url,
            "hello",
            {
                headers: {
                    "Content-Type": "application/json",
                    "Access-Control-Allow-Origin": "*"
                }
            }).catch((reason) => {
                console.log(reason);
            })
        console.log(response);
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