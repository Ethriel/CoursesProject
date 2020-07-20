import React from 'react';
import { Avatar, Menu } from 'antd';
import { UserOutlined } from '@ant-design/icons';
import H from '../common/HAntD';
const { SubMenu } = Menu;

function UserAvatar(props) {
    const src = localStorage.getItem("user_picture");
    const showIcon = (src === "" || src === null) ? true : false;
    const icon = <UserOutlined />;
    const withPicture = <Avatar src={src} />;
    const withIcon = <Avatar icon={icon} />;
    const avatar = showIcon ? withIcon : withPicture;
    const avatarMenu =
        <Menu onClick={props.menuClick} mode="horizontal">
            <SubMenu icon={icon}>
                <Menu.Item style={{width: "auto", height: "auto"}}>
                    <H level={4} myText="Sign out"/>
                </Menu.Item>
            </SubMenu>
        </Menu>

    return avatarMenu;
};

export default UserAvatar;