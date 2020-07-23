import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Button, Drawer } from 'antd';
import UserInfoContainer from './UserInfoContainer';
import MainContainer from '../common/ContainerComponent';
import UserCoursesContainer from './UserCoursesContainer';
import SetLocalStorage from '../../helpers/setDataToLocalStorage';
import ModalWithMessage from '../common/ModalWithMessage';

function UserProfileComponent(props) {
    const id = +localStorage.getItem("current_user_id");
    const [isLoading, setIsloading] = useState(true);
    const [user, setUser] = useState({});
    const [showDrawer, setShowDrawer] = useState(false);

    const [emailState, setEmailState] = useState({
        valid: false,
        changed: false
    });

    const [modalState, setModalState] = useState({
        title: "",
        message: "",
        show: false
    });

    useEffect(() => {
        const signal = axios.CancelToken.source();

        async function fetchUser() {
            try {
                const response = await MakeRequestAsync(`Students/get/${id}`, { msg: "hello" }, "get", signal.token);
                const data = response.data;
                setUser(data);
            } catch (error) {
                const data = error.response.data;
                const message = data.message;

                setModalState({
                    title: "Change email has failed",
                    message: message,
                    show: true
                });
                console.log(error.response);
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
            setEmailState(oldState => ({ ...oldState, ...{ changed: true } }));
        }
    }

    const submitClick = async () => {
        const signal = axios.CancelToken.source();
        setIsloading(true);
        const data = {
            user: user,
            isEmailChanged: emailState.changed
        };
        try {

            const email = localStorage.getItem("new_email");
            const verifyResponse = await MakeRequestAsync(`account/verifyEmail/${email}`, { msg: "Hello" }, "get", signal.token);

            setEmailState(old => ({ ...old, ...{ valid: true } }));

            setModalState({
                title: "Confirm your new email, please",
                message: "A confirm message was sent to your email. Follow the instructions",
                show: true
            });

            const response = await MakeRequestAsync("account/update", data, "post", signal.token);
            const userData = response.data;
            setUser(userData);
        } catch (error) {
            const data = error.response.data;
            const message = data.message;

            setModalState({
                title: "Change email has failed",
                message: message,
                show: true
            });
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
    const modalOk = (e) => {
        setModalState(old => ({ show: false }));
    }
    const modalCancel = (e) => {
        setModalState(old => ({ show: false }));
    }
    const modal =
        <ModalWithMessage
            modalTitle={modalState.title}
            modalVisible={modalState.show}
            modalOk={modalOk}
            modalCancel={modalCancel}
            modalMessage={modalState.message} />;

    const mainContainerClasses = ["display-flex", "width-100", "col-flex"];
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
    return (
        <MainContainer classes={mainContainerClasses}>
            {content}
            {emailState.valid === true && emailState.changed === true && modalState.show === true && modal}
            {emailState.valid === false && modalState.show === true && modal}
        </MainContainer>
    );
};

export default UserProfileComponent;