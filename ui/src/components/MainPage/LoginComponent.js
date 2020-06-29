import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';

class LoginComponent extends Component {
    confirmHandler = values => {
        console.log('Received values of form: ', values);
    }

    render() {
        return (
            <NormalLoginFormAntD myConfirHandler={this.confirmHandler} />
        )
    }
}

export default LoginComponent;