import React, { useState } from 'react';
import { connect } from 'react-redux';
import { withRouter } from "react-router";
import axios from 'axios';
import TopMenu from './TopMenuComponent';
import Container from '../../common/ContainerComponent';
import ClearLocalStorage from '../../../helpers/ClearLocalStorage';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import SetModalData from '../../../helpers/SetModalData';
import GetModalPresentation from '../../../helpers/GetModalPresentation';
import ModalWithMessage from '../../common/ModalWithMessage';
import { main, courses, aboutUs, admin, userProfile } from '../../../Routes/RoutersDirections';
import { SET_ROLE } from '../../../reducers/reducersActions';
import { ADMIN, UNDEFINED, NULL } from '../../common/roles';

const AppHeaderComponent = ({ currentUser, history, ...props }) => {
    const isUser = currentUser.role !== UNDEFINED && currentUser.role !== NULL;

    const subItems = [];

    subItems.push({ key: main, text: "Home", to: main });

    subItems.push({ key: aboutUs, text: "About us", to: aboutUs });

    if (isUser) {
        subItems.push({ key: courses, text: "Courses", to: courses });
        if (currentUser.role === ADMIN) {
            subItems.push({ key: admin, text: "Admin", to: admin });
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

    const profileClick = async (event) => {
        const key = event.key;
        const signal = axios.CancelToken.source();
        if (key !== "profile" && key !== "signout") {
            return;
        }
        else if (key === "signout") {
            try {
                const email = currentUser.email;
                const requestData = {
                    email: email
                };

                // sign out user on server
                await MakeRequestAsync("account/signout", requestData, "post", signal.token);
                // set user role to undefined in redux store
                props.onRoleChange(UNDEFINED);

                // clear local storage
                ClearLocalStorage();

                // redirect to main page
                history.push(main);
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
            history.push(userProfile);
        }
    }

    const modalWindow = ModalWithMessage(modal);

    return (
        <>
            {modal.visible === true && modalWindow}
            <Container classes={headerContainer}>
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
export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    }),
    dispatch => ({
        onRoleChange: (role) => {
            dispatch({ type: SET_ROLE, payload: role })
        }
    })
)(AppHeaderComponent));