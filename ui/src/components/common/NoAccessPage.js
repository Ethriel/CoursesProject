import React, { useState, useEffect } from 'react';
import H from '../common/HAntD';
import { withRouter, NavLink } from 'react-router-dom';
import { connect } from 'react-redux';
import { Typography } from 'antd';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { ADMIN } from './roles';
import { main, confirmEmailForm } from '../../Routes/RoutersDirections';
import Notification from './Notification';

const { Paragraph } = Typography;

const NoAccessPage = ({ currentUser, histroy, ...props }) => {
    const setCatch = error => {
        Notification(error);
    };
    const [notUser, setNotUser] = useState(false);
    const [emailConfirmed, setEmailConfirmed] = useState(true);
    const [noAccess, setNoAccess] = useState(false);

    useEffect(() => {
        const signal = axios.CancelToken.source();
        const checkEmail = async (email) => {
            try {
                await MakeRequestAsync("account/checkEmailConfirmed", { email: email }, "post", signal.token);
            } catch (error) {
                const response = error.response;
                if (response) {
                    if (response.status === 404) {
                        setCatch(error);
                    }
                }
                setEmailConfirmed(false);
            }
        };

        if (currentUser.email) {
            checkEmail(currentUser.email);
            if (currentUser.role !== ADMIN) {
                setNoAccess(true);
            }
            else {
                return function cleanup() {
                    signal.cancel();
                }
            }
        }
        else {
            setNotUser(true);
        }

        return function cleanup() {
            signal.cancel();
        }
    }, [currentUser.role, currentUser.email]);

    const confirmEmail = <NavLink to={confirmEmailForm}>Confirm</NavLink>;
    const emailNotConfirmed =
        <Paragraph>
            Your email is not confirmed. {confirmEmail} it now
        </Paragraph>;

    const signIn = <NavLink to={main}>Sign in</NavLink>;
    const signUp = <NavLink to={main}>Sign up</NavLink>;
    const notAUser =
        <Paragraph>
            You are not signed in. {signIn} now. Not a member? {signUp} now
        </Paragraph>;

    const noAccessMessage =
        <Paragraph>
            You don't have access to this page
    </Paragraph >;

    return (
        <>
            <H level={3} myText="Access denied" />
            {noAccess === true && noAccessMessage}
            {emailConfirmed === false && emailNotConfirmed}
            {notUser === true && notAUser}
        </>
    );
}

export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    })
)(NoAccessPage));