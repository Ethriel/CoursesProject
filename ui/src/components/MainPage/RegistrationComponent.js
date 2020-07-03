import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import RegistrationForm from './RegistrationFormAntD';
import ButtonFaceBook from './ButtonFacebook';

class RegistrationComponent extends Component {
    facebookHandler = () => {

    }
    render() {
        return (
            <>
                <RegistrationForm />
                <ButtonFaceBook clickHandler={this.facebookHandler} />
            </>
        )
    }
}

export default RegistrationComponent;