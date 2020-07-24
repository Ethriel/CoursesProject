import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Button, Drawer } from 'antd';
import UserInfoContainer from './UserInfoContainer';
import MainContainer from '../common/ContainerComponent';
import UserCoursesContainer from './UserCoursesContainer';
import SetLocalStorage from '../../helpers/setDataToLocalStorage';
import ModalWithMessage from '../common/ModalWithMessage';
import SetModalData from '../../helpers/SetModalData';
import GetUserData from '../../helpers/GetUserData';
import GetModalPresentation from '../../helpers/GetModalPresentation';

function UserProfileComponent(props) {
    const id = +localStorage.getItem("current_user_id");

    const modalOk = (e) => {
        setModalState(old => ({ ...old, ...{ visible: false } }));
    };

    const modalCancel = (e) => {
        setModalState(old => ({ ...old, ...{ visible: false } }));
    };

    const [modalState, setModalState] = useState(GetModalPresentation(modalOk, modalCancel));

    const [isLoading, setIsloading] = useState(true);
    const [user, setUser] = useState({});
    const [showDrawer, setShowDrawer] = useState(false);
    const [fieldChanged, setFieldChanged] = useState(false);

    const [emailState, setEmailState] = useState({
        valid: false,
        changed: false
    });

    const setCatch = error => {
        const modalData = SetModalData(error);
        setModalState(old => ({
            ...old, ...{
                message: modalData.message,
                errors: modalData.errors,
                visible: true
            }
        }));
    };

    useEffect(() => {
        const signal = axios.CancelToken.source();

        async function fetchUser() {
            try {
                const response = await MakeRequestAsync(`Students/get/${id}`, { msg: "hello" }, "get", signal.token);
                const userData = response.data.data;
                setUser(userData);
            } catch (error) {
                setCatch(error);
            } finally {
                setIsloading(false);
            }
        };

        fetchUser();

        return function cleanup() {
            signal.cancel("CANCEL IN GET USER");
        };

    }, []);

    const changeField = (field, value) => {
        setUser(old => ({ ...old, [field]: value }));
        if (field === "email") {
            localStorage.setItem("new_email", value);
            setEmailState(old => ({ ...old, ...{ changed: true } }));
        }
        else {
            setFieldChanged(true);
        }
    }

    const submitClick = async () => {
        const signal = axios.CancelToken.source();
        setIsloading(true);
        const data = {
            user: user,
            isEmailChanged: emailState.changed,
            anyFieldChanged: fieldChanged
        };
        try {

            const email = localStorage.getItem("new_email");
            const verifyResponse = await MakeRequestAsync(`account/verifyEmail/${email}`, { msg: "Hello" }, "get", signal.token);

            setEmailState(old => ({ ...old, ...{ valid: true } }));

            setModalState(old => ({
                ...old, ...{
                    message: "Confirm your new email, please",
                    errors: "A confirm message was sent to your email. Follow the instructions",
                    visible: true
                }
            }));

            const response = await MakeRequestAsync("account/update", data, "post", signal.token);
            const userData = response.data.data;
            setUser(userData);
        } catch (error) {
            setCatch(error);
        } finally {
            setIsloading(false);
        }
    }

    const closeDrawer = () => {
        setShowDrawer(false);
    };

    const openDrawer = () => {
        setShowDrawer(true);
    }

    const modalWindow = ModalWithMessage(modalState);

    const content =
        <>
            <Button type="primary" onClick={openDrawer}
                style={{ width: 150, alignSelf: "flex-end" }}>
                Open user info
          </Button>
            <Drawer title="User info"
                width={400}
                placement="left"
                closable={true}
                onClose={closeDrawer}
                visible={showDrawer}>
                <UserInfoContainer user={user} onValueChange={changeField} submitClick={submitClick} />
            </Drawer>
            <UserCoursesContainer userId={id} />
        </>;

    const mainContainerClasses = ["display-flex", "width-100", "col-flex"];

    return (
        <MainContainer classes={mainContainerClasses}>
            {content}
            {emailState.valid === true && emailState.changed === true && modalState.visible === true && modalWindow}
            {emailState.valid === false && modalState.visible === true && modalWindow}
        </MainContainer>
    );
};

export default UserProfileComponent;