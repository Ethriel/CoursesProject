import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Button, Drawer, Spin, Space } from 'antd';
import UserInfoContainer from './UserInfoContainer';
import MainContainer from '../common/ContainerComponent';
import UserCoursesContainer from './UserCoursesContainer';
import SetLocalStorage from '../../helpers/setDataToLocalStorage';
import ModalWithMessage from '../common/ModalWithMessage';
import SetModalData from '../../helpers/SetModalData';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import ValidateEmail from '../../helpers/ValidateEmail';
import { ProfileOutlined } from '@ant-design/icons';

const UserProfileComponent = ({userId, ...props}) => {
    const id = (userId === undefined || userId < 0) ? +localStorage.getItem("current_user_id") : userId;

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

    }, [id]);

    const changeField = (field, value) => {
        setUser(old => ({ ...old, [field]: value }));

        if (field === "email") {
            try {
                if (ValidateEmail(value)) {
                    localStorage.setItem("new_email", value);
                    setEmailState(old => ({ ...old, ...{ changed: true } }));
                }
                else {
                    throw new Error("Invalid email input");
                }
            } catch (error) {
                setCatch(error)
            }
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
            // if email was changed - make a request to verify new email
            if (emailState.changed === true) {
                const email = localStorage.getItem("new_email");
                await MakeRequestAsync(`account/verifyEmail/${email}`, { msg: "Hello" }, "get", signal.token);

                setEmailState(old => ({ ...old, ...{ valid: true } }));

                setModalState(old => ({
                    ...old, ...{
                        message: "Confirm your new email, please",
                        errors: "A confirm message was sent to your email. Follow the instructions",
                        visible: true
                    }
                }));
            }

            // if user data was changed - send an update request
            if (fieldChanged === true || emailState.changed === true) {
                const response = await MakeRequestAsync("account/update", data, "post", signal.token);
                const userData = response.data.data;
                setUser(userData);
            }

        } catch (error) {
            setCatch(error);
        } finally {
            setIsloading(false);
        }
    }

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

    const closeDrawer = () => {
        setShowDrawer(false);
    };

    const openDrawer = (e) => {
        setShowDrawer(true);
    }

    const openProfile =
        <Button
            icon={<ProfileOutlined />}
            title="Open profile"
            style={{ alignSelf: "flex-end" }}
            onClick={openDrawer}
            type="primary"
        />

    const modalWindow = ModalWithMessage(modalState);

    const content =
        <>
            {openProfile}
            <Drawer title="Profile info"
                width={400}
                placement="right"
                closable={true}
                onClose={closeDrawer}
                visible={showDrawer}>
                <UserInfoContainer user={user} onValueChange={changeField} submitClick={submitClick} />
            </Drawer>
            <UserCoursesContainer userId={id} />
        </>;

    const spinner = <Space size="middle"> <Spin tip="Getting your data..." size="large" /></Space>;

    const mainContainerClasses = ["display-flex", "width-100", "col-flex", "center-flex"];

    return (
        <MainContainer classes={mainContainerClasses}>
            {isLoading === true && spinner}
            {isLoading === false && content}
            {emailState.valid === true && emailState.changed === true && modalState.visible === true && modalWindow}
            {emailState.valid === false && modalState.visible === true && modalWindow}
        </MainContainer>
    );
};

export default UserProfileComponent;