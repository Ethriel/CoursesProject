import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import RegistrationForm from './RegistrationFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Redirect } from 'react-router-dom';
import axios from 'axios';
import GetUserData from '../../helpers/GetUserData';

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            signal: axios.CancelToken.source()
        };
    }
    confirmHandler = async values => {
        console.log("VALUES", values);
        const userData = {
            firstName: values.user.name,
            lastName: values.user.lastname,
            birthDate: values.birthdate._i,
            email: values.email,
            password: values.password
        };
        console.log("USER DATA", userData);
        try {
            const cancelToken = this.state.signal.token;
            const response = await MakeRequestAsync("https://localhost:44382/account/signup", userData, "post", cancelToken);
            const data = response.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);
            localStorage.setItem("bearer_header", `Bearer ${token}`);
            localStorage.setItem("access_token", token);
            localStorage.setItem("current_user_role", role);
            localStorage.setItem("current_user_id", user.id);
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

    facebookClick = () => {
        //const cancelToken = this.state.signal.token;
        // const response = await MakeRequestAsync("https://localhost:44382/courses/get/all", { msg: "hello" }, "get", cancelToken);
        // const data = response.data;
        // console.log("DATA", data);
    }
    facebookResponseHandler = (response) => {
        console.log(response);
    }
    render() {
        return (
            <RegistrationForm onFinish={this.confirmHandler}
                facebookClick={this.facebookClick} facebookResponse={this.facebookResponseHandler} />
        )
    }
}

export default RegistrationComponent;