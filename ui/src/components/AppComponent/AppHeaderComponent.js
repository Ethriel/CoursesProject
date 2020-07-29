import React, { useState } from 'react';
import { useStore, useDispatch } from 'react-redux';
import { withRouter } from "react-router";
import axios from 'axios';
import TopMenu from '../common/TopMenuComponent';
import Container from '../common/ContainerComponent';
import headerLogo from '../../img/logo.png';
import H from '../common/HAntD';
import ClearLocalStorage from '../../helpers/ClearLocalStorage';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import SetModalData from '../../helpers/SetModalData';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import ModalWithMessage from '../common/ModalWithMessage';
import { main, courses, aboutUs, admin, userProfile } from '../../Routes/RoutersDirections';
import { SET_ROLE } from '../../reducers/reducersActions';
import { ADMIN, UNDEFINED, NULL } from '../common/roles';
import '../../index.css';

const AppHeaderComponent = (props) => {
    const store = useStore();
    const dispatch = useDispatch();

    // get user's role from redux store
    const getRole = () => {
        const state = store.getState();
        const role = state.userRoleReducer.role;
        return role;
    }

    const role = getRole();

    const isUser = role !== UNDEFINED && role !== NULL;

    const subItems = [];

    subItems.push({ key: 1, text: "Home", to: main });
    
    subItems.push({ key: 3, text: "About us", to: aboutUs });
    
    if (isUser) {
        subItems.push({ key: 2, text: "Courses", to: courses });
        if (role === ADMIN) {
            subItems.push({ key: 4, text: "Admin", to: admin });
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
    const headerContainer = ["display-flex", "align-center", "col-flex", "width-95", "center-a-div"];
    const menuContainer = ["display-flex", "justify-center", "align-center", "width-90"];
    // const logoTextContainer = ["display-flex", "align-center", "justify-center", "width-90", "max-width-350"];
    // const logo = <img src={headerLogo} className="logo-img" alt="Logo" />
    // const headerText = <H level={2} myText="Forge your future with us!" />;

    const profileClick = async (event) => {
        const key = event.key;
        const signal = axios.CancelToken.source();
        if (key !== "profile" && key !== "signout") {
            return;
        }
        else if (key === "signout") {
            try {
                const email = localStorage.getItem("current_user_email");
                const requestData = {
                    email: email
                };

                // sign out user on server
                await MakeRequestAsync("account/signout", requestData, "post", signal.token);

                // set user role to undefined in redux store
                dispatch({
                    type: SET_ROLE,
                    payload: { role: UNDEFINED }
                });

                // clear local storage
                ClearLocalStorage();

                // redirect to main page
                props.history.push(main);
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
        else if (key === "profile") {
            // redirect user's profile
            props.history.push(userProfile);
        }
    }

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
            <Container classes={headerContainer}>
                {/* <Container classes={logoTextContainer}>
                    {logo}
                    {headerText}
                </Container> */}
                <Container classes={menuContainer}>
                    <TopMenu 
                    myMenuItems={subItems}
                    isUser={isUser}
                    menuClick={profileClick}
                    className="header-menu-width" />
                </Container>
            </Container>

        </>
    );
}
export default withRouter(AppHeaderComponent);