import React from 'react';
import { Avatar, Menu } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import ProfileSubItem from './ProfileSubComponent';
const { SubMenu } = Menu;

function UserAvatar(props) {
    const src = localStorage.getItem("user_picture");
    const showIcon = (src === "" || src === null) ? true : false;
    const icon = <UserOutlined />;
    const withPicture = <Avatar src={src} />;
    const withIcon = <Avatar icon={icon} />;
    const avatar = showIcon ? withIcon : withPicture;

    const avatarMenu =
        // <Menu onClick={props.menuClick} mode="horizontal">
        <SubMenu key={5} icon={icon}>
            <Menu.Item key={"profile"}>
                <ProfileSubItem text="Profile" />
            </Menu.Item>
            <Menu.Item key={"signout"}>
                <ProfileSubItem text="Sign out" />
            </Menu.Item>
        </SubMenu>
    // </Menu>

    return avatarMenu;
};

export default UserAvatar;