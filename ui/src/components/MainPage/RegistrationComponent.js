import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import RegistrationForm from './RegistrationFormAntD';
import MakeRequest from '../../helpers/MakeRequest';
import { Redirect } from 'react-router-dom';

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false
        };
    }
    confirmHandler = async values => {
        const userData = {
            email: values.email,
            password: values.password
        };
        try {
            const data = await MakeRequest("https://localhost:44382/account/signup", userData, "post");
            console.log(data);
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

    facebookHandler = () => {

    }
    render() {
        return (
            <RegistrationForm onFinish={this.confirmHandler} facebook={this.facebookHandler} />
        )
    }
}

export default RegistrationComponent;