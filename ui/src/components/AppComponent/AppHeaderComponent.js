import React, { useState } from 'react';
import '../../index.css';
import TopMenu from '../common/TopMenuComponent';
import Container from '../common/ContainerComponent';
import Avatar from './UserAvatar';
import headerLogo from '../../img/logo.png';
import H from '../common/HAntD';
import ClearLocalStorage from '../../helpers/ClearLocalStorage';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { withRouter } from "react-router";
import axios from 'axios';
import SetModalData from '../../helpers/SetModalData';
import ModalWithMessage from '../common/ModalWithMessage';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import { useStore, useDispatch } from 'react-redux';

const AppHeaderComponent = (props) => {
    const store = useStore();
    const dispatch = useDispatch();
    const getRole = () => {
        const state = store.getState();
        const role = state.userRoleReducer.role;
        return role;
    }
    getRole();
    const role = getRole();
    const subItems = [];
    subItems.push({ key: 1, text: "Home", to: "/" });
    if (role !== undefined) {
        subItems.push({ key: 2, text: "Courses", to: "/courses" });
        subItems.push({ key: 3, text: "About us", to: "/aboutus" });
        if (role === "ADMIN") {
            subItems.push({ key: 4, text: "Admin", to: "/admin" });
        }
    }
    const closeModal = () => {
        setModal(oldModal => ({ ...oldModal, ...{ visible: false } }));
    };

    const modalOk = () => {
        closeModal();
    };

    const modalCancel = () => {
        closeModal();
    };

    const [modal, setModal] = useState(GetModalPresentation(modalOk, modalCancel));
    const containerClasses = ["display-flex", "space-around-flex", "align-center"];
    const avatarClasses = ["display-flex", "col-flex"];
    const logo = <img src={headerLogo} width={'50px'} height={'50px'} alt="Logo" />
    const headerText = <H level={2} myText="Forge your future with us!" />;

    const profileClick = async (event) => {
        const text = event.item.props.children[1].props.text;
        const signal = axios.CancelToken.source();
        if (text === "Sign out") {
            try {
                const email = localStorage.getItem("current_user_email");
                const requestData = {
                    email: email
                };
                const response = await MakeRequestAsync("account/signout", requestData, "post", signal.token);
                dispatch({
                    type: "SET_ROLE",
                    payload: { role: undefined }
                });
                ClearLocalStorage();
                props.history.push("/");
            } catch (error) {
                const modalData = SetModalData(error);
                setModal(oldModal => ({
                    ...oldModal,
                    ...{
                        message: modalData.message,
                        errors: modalData.errors,
                        visible: true
                    }
                }));
            }
        }
        else if (text === "Profile") {
            props.history.push("/profile");
        }
    }

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
            <Container classes={containerClasses}>
                {logo}
                {headerText}
                <TopMenu myMenuItems={subItems} />
                <Container classes={avatarClasses}>
                    {role !== undefined && <Avatar menuClick={profileClick} />}
                </Container>
            </Container>
        </>
    );
}
export default withRouter(AppHeaderComponent);