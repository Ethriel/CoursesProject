import React, { useState, useEffect } from 'react';
import H from '../common/HAntD';
import { withRouter, NavLink } from 'react-router-dom';
import { useStore } from 'react-redux';
import { Typography } from 'antd';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { ADMIN } from './roles';
import SetModalData from '../../helpers/SetModalData';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import ModalWithMessage from '../common/ModalWithMessage';
import { main, confirmEmailForm } from '../../Routes/RoutersDirections';

const { Paragraph } = Typography;
const NoAccessPage = () => {
    const modalOk = (e) => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
    };

    const modalCancel = (e) => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
    };

    const setCatch = (error) => {
        const modalData = SetModalData(error);
        setModal(oldModal => ({
            ...oldModal, ...{
                message: modalData.message,
                errors: modalData.errors,
                visible: true
            }
        }));
    };

    const [notUser, setNotUser] = useState(false);
    const [emailConfirmed, setEmailConfirmed] = useState(true);
    const [noAccess, setNoAccess] = useState(false);
    const [modal, setModal] = useState(GetModalPresentation(modalOk, modalCancel));

    const store = useStore().getState();
    const role = store.userRoleReducer.role;

    useEffect(() => {
        const signal = axios.CancelToken.source();
        const checkEmail = async (email) => {
            try {
                await MakeRequestAsync("account/checkEmailConfirmed", {email: email}, "post", signal.token);
            } catch (error) {
                setEmailConfirmed(false);
                //setCatch(error);
            }
        }
        const email = localStorage.getItem("current_user_email");

        if (email) {
            checkEmail(email);
            if (role !== ADMIN) {
                setNoAccess(true);
            }
        }
        else {
            setNotUser(true);
        }

        return function cleanup() {
            signal.cancel();
        }
    }, []);

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


    const noAccessMessage = <Paragraph>
        You don't have access to this page
    </Paragraph >;

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
            <H level={3} myText="Access denied" />
            {noAccess === true && noAccessMessage}
            {emailConfirmed === false && emailNotConfirmed}
            {notUser === true && notAUser}
        </>

    );
}

export default withRouter(NoAccessPage);