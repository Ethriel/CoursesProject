import React, { useState } from 'react';
import { Input } from 'antd';
import H from '../../common/HAntD';
import Container from '../../common/ContainerComponent';
import ButtonComponent from '../../common/ButtonComponent';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from "axios";

const ConfirmEmail = () => {
    const [showInfo, setShowInfo] = useState(false);
    const [showError, setShowError] = useState(false);
    const [buttonActive, setButtonActive] = useState(false);
    const [token, setToken] = useState("");
    const id = localStorage.getItem("current_user_id");
    const fetchToken = async (event) => {
        const signal = axios.CancelToken.source();
        try {
            const requestObj = {
                id: id,
                token: token
            };
            const response = await MakeRequestAsync("account/confirmEmail", requestObj, "post", signal.token);
            setShowInfo(true);
        } catch (error) {
            setShowError(true);
        }
    }
    const handleInput = event => {
        const input = event.currentTarget;
        const token = input.value;
        setToken(token);
        setButtonActive(true);
    }
    const about = <H level={4} myText="Confirm token was sent to your email. Copy and paste it below. Then press enter" />
    const info = <H level={4} myText="Email confirmed" />;
    const error = <H level={4} myText="Email was not confirmed" />;
    const button = <ButtonComponent
    mySize="small"
    myHandler={fetchToken}
    myText="Confirm"/>;
    const input = <Input
                    size="middle" 
                    type="text" 
                    onPressEnter={handleInput} 
                    placeholder="Paste token here"
                    style={{width: 300}}/>;
    const classes = ["display-flex", "width-80", "center-a-div", "col-flex", "align-center"];
    return (
        <Container classes={classes}>
            {about}
            {input}
            {buttonActive && button}
            {showInfo && info}
            {showError && error}
        </Container>
    )
}

export default ConfirmEmail;