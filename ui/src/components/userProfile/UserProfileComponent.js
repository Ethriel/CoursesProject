import React, { useEffect, useState } from 'react';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Button, Drawer, Spin, Space } from 'antd';
import { ProfileOutlined } from '@ant-design/icons';
import UserInfoContainer from './UserInfoContainer';
import MainContainer from '../common/ContainerComponent';
import UserCoursesContainer from './UserCoursesContainer';
import ValidateEmail from '../../helpers/ValidateEmail';
import { USER, ADMIN } from '../common/roles';
import { forbidden } from '../../Routes/RoutersDirections';
import NotificationError from '../common/notifications/notification-error';
import NotificationOk from '../common/notifications/notification-ok';
import GetUserData from '../../helpers/GetUserData';
import SetLocalStorage from '../../helpers/setDataToLocalStorage';
import { SET_ROLE, SET_EMAIL_CONFIRMED, SET_AVATAR } from '../../reducers/reducersActions';
import ImageUploader from '../common/image-uploader';

const UserProfileComponent = ({ userId, history, currentUser, ...props }) => {
    const id = (userId === undefined || userId < 0) ? currentUser.id : userId;

    const [isLoading, setIsloading] = useState(true);
    const [user, setUser] = useState({});
    const [showDrawer, setShowDrawer] = useState(false);
    const [fieldChanged, setFieldChanged] = useState(false);
    const [selectedFile, setSelectedFile] = useState({});

    const [emailState, setEmailState] = useState({
        valid: false,
        changed: false
    });

    useEffect(() => {
        const signal = axios.CancelToken.source();

        async function fetchUser() {

            try {
                const response = await MakeRequestAsync(`students/get/user/${id}`, { msg: "hello" }, "get", signal.token);
                const userData = response.data.data;

                setUser(userData);
            } catch (error) {
                setCatch(error);
            } finally {
                setIsloading(false);
            }
        };
        const role = currentUser.role;
        if (role !== USER && role !== ADMIN) {
            history.push(forbidden);
        }
        else {
            fetchUser();
        }

        return function cleanup() {
            signal.cancel("CANCEL IN GET USER");
        };

    }, [id, currentUser.role, history]);

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
        setIsloading(true);

        const signal = axios.CancelToken.source();
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

                NotificationOk("A confirm message was sent to your email. Follow the instructions");

                setEmailState(old => ({ ...old, ...{ valid: true } }));

                props.onEmailConfirmedChanged(false);
            }

            // if user data was changed - send an update request
            if (fieldChanged === true || emailState.changed === true) {
                const response = await MakeRequestAsync("account/update", data, "post", signal.token);
                const respData = response.data.data;
                const userData = respData.user;
                const token = respData.token.key;
                const role = data.user.roleName;

                setUser(userData);

                const newUser = GetUserData(userData);

                const emailConfirmed = !emailState.changed;
                props.onEmailConfirmedChanged(emailConfirmed);
                props.onRoleChange(role);

                SetLocalStorage(newUser.id, token, role, newUser.avatarPath, newUser.email, emailConfirmed);
            }

        } catch (error) {
            setCatch(error);
        } finally {
            setIsloading(false);
        }
    };

    const setCatch = error => {
        NotificationError(error);
    };

    const closeDrawer = () => {
        setShowDrawer(false);
    };

    const openDrawer = (e) => {
        setShowDrawer(true);
    }

    const onFileChange = event => {
        const file = event.target.files[0];
        setSelectedFile(file);
        console.log(file);
    }

    const onFileUpload = async () => {

        const formData = new FormData();

        formData.set(
            "image",
            selectedFile,
            selectedFile.name
        );

        var cancelToken = axios.CancelToken.source().token;
        const response = await MakeRequestAsync(`students/user/uploadImage/${id}`, formData, "post", cancelToken);
        const responseData = response.data.data;
        const userAvatar = responseData.avatarPath;
        localStorage.setItem("user_avatar", userAvatar);
        props.onAvatarChange(userAvatar);
    };

    const openProfile =
        <Button
            icon={<ProfileOutlined />}
            title="Open profile"
            style={{ alignSelf: "flex-end" }}
            onClick={openDrawer}
            type="primary"
        />

    const imageUploader = <ImageUploader onFileChange={onFileChange} onFileUpload={onFileUpload} />;
    const content =
        <>
            {openProfile}
            <Drawer title="Profile info"
                width={400}
                placement="right"
                closable={true}
                onClose={closeDrawer}
                visible={showDrawer}>
                <UserInfoContainer user={user} onValueChange={changeField} submitClick={submitClick} imageUploader={imageUploader} />

            </Drawer>
            <UserCoursesContainer userId={id} />
        </>;

    const spinner = <Space size="middle"> <Spin tip="Getting your data..." size="large" /></Space>;

    const mainContainerClasses = ["display-flex", "width-100", "col-flex", "center-flex"];

    return (
        <MainContainer classes={mainContainerClasses}>
            {isLoading === true && spinner}
            {isLoading === false && content}
        </MainContainer>
    );
};

export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    }),
    dispatch => ({
        onRoleChange: (role) => {
            dispatch({ type: SET_ROLE, payload: role })
        },
        onEmailConfirmedChanged: (emailConfirmed) => {
            dispatch({ type: SET_EMAIL_CONFIRMED, payload: emailConfirmed })
        },
        onAvatarChange: (avatar) => {
            dispatch({ type: SET_AVATAR, payload: avatar })
        }
    })
)(UserProfileComponent));