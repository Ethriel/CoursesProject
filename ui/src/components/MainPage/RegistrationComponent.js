import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import RegistrationForm from './RegistrationFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Redirect } from 'react-router-dom';
import axios from 'axios';

class RegistrationComponent extends Component {
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            signal: axios.CancelToken.source()
        };
    }
    confirmHandler = async values => {
        const userData = {
            email: values.email,
            password: values.password
        };
        console.log(values);
        try {
            const cancelToken = this.state.signal.token;
            //const data = await MakeRequestAsync("https://localhost:44382/account/signup", userData, "post", cancelToken);
            //console.log(data);
            //this.setState({ redirect: true });
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