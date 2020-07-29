import React from 'react'
import 'antd/dist/antd.css';
import '../../index.css';
import { Menu, Avatar } from 'antd';
import { NavLink } from 'react-router-dom';
import { UserOutlined, LogoutOutlined, ProfileOutlined } from '@ant-design/icons';
const { SubMenu } = Menu;

const TopMenuComponent = (props) => {
    const items = props.myMenuItems;
    const isUser = props.isUser;
    const src = localStorage.getItem("user_picture");
    const showIcon = (src === "" || src === null) ? true : false;
    const icon = <UserOutlined />;
    const withPicture = <Avatar src={src} />;
    const withIcon = <Avatar icon={icon} />;
    const className = props.className;
    const avatar = showIcon ? withIcon : withPicture;

    const menuItems = items.map((item) => {
        return <Menu.Item
            key={item.key}>
            <NavLink to={item.to} />{item.text}
        </Menu.Item>;
    });

    const avatarMenu =
        <SubMenu key={5} icon={<UserOutlined />}>
            <Menu.Item key={"profile"} icon={<ProfileOutlined />}
                className="header-sub-my">
                Profile
            </Menu.Item>
            <Menu.Item key={"signout"} icon={<LogoutOutlined />}
                className="header-sub-my">
                Sign out
            </Menu.Item>
        </SubMenu>;

    return (
        <Menu mode="horizontal"
            onClick={props.menuClick}
            className={className}>
            {menuItems}
            {
                isUser === true && avatarMenu
            }
        </Menu>
    );
};

export default TopMenuComponent;