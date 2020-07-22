import React, { useEffect, useState } from 'react';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import { Button, Drawer } from 'antd';
import UserInfoContainer from './UserInfoContainer';
import MainContainer from '../common/ContainerComponent';
import UserCoursesContainer from './UserCoursesContainer';
import SetLocalStorage from '../../helpers/setDataToLocalStorage';

function UserProfileComponent(props) {
    const [user, setUser] = useState({});
    const [isLoading, setIsloading] = useState(true);
    const [isFirstNameChanged, setIsFirstNameChanged] = useState(false);
    const [isLastNameChanged, setIsLastNameChanged] = useState(false);
    const [isEmailChanged, setIsEmailChanged] = useState(false);
    const [showDrawer, setShowDrawer] = useState(false);
    const id = +localStorage.getItem("current_user_id");

    useEffect(() => {
        const signal = axios.CancelToken.source();

        async function fetchUser() {
            try {
                const response = await MakeRequestAsync(`Students/get/${id}`, { msg: "hello" }, "get", signal.token);
                const data = response.data;
                setUser(data);
            } catch (error) {
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
        setUser(oldState => ({ ...oldState, [field]: value }));
        switch (field) {
            case "firstName":
                setIsFirstNameChanged(true);
                break;
            case "lastName":
                setIsLastNameChanged(true);
                break;
            case "email":
                setIsEmailChanged(true);
                break;
            default:
                break;
        }
    }

    const submitClick = async () => {
        const signal = axios.CancelToken.source();
        setIsloading(true);
        const data = {
            user: user,
            isFirstNameChanged: isFirstNameChanged,
            isLastNameChanged: isLastNameChanged,
            isEmailChanged: isEmailChanged
        };

        try {
            const response = await MakeRequestAsync("account/update", data, "post", signal.token);
            const userData = response.data;
            setUser(userData);
        } catch (error) {
            console.log(error);
        } finally{
            setIsloading(false);
        }
    }

    const closeDrawer = () => {
        setShowDrawer(false);
    };

    const openDrawer = () => {
        setShowDrawer(true);
    }
    const mainContainerClasses = ["display-flex", "width-100", "col-flex"];
    return (

        <MainContainer classes={mainContainerClasses}>
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
        </MainContainer>
    );
};

export default UserProfileComponent;