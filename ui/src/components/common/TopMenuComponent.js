import React from 'react'
import 'antd/dist/antd.css';
import '../../index.css';
import { Menu, Avatar } from 'antd';
import { NavLink } from 'react-router-dom';
import { UserOutlined } from '@ant-design/icons';
import ProfileSubItem from '../AppComponent/ProfileSubComponent';
const { SubMenu } = Menu;

const TopMenuComponent = (props) => {
    const items = props.myMenuItems;
    const isUser = props.isUser;
    const src = localStorage.getItem("user_picture");
    const showIcon = (src === "" || src === null) ? true : false;
    const icon = <UserOutlined />;
    const withPicture = <Avatar src={src} />;
    const withIcon = <Avatar icon={icon} />;
    const avatar = showIcon ? withIcon : withPicture;

    const menuItems = items.map((item) => {
        return <Menu.Item
            key={item.key}>
            <NavLink to={item.to} />{item.text}
        </Menu.Item>;
    });

    const avatarMenu =
    <SubMenu key={5} icon={icon}>
        <Menu.Item key={"profile"}>
            <ProfileSubItem text="Profile" />
        </Menu.Item>
        <Menu.Item key={"signout"}>
            <ProfileSubItem text="Sign out" />
        </Menu.Item>
    </SubMenu>;

    return (
        <Menu mode="horizontal" onClick={props.menuClick}>
            {menuItems}
            {
                isUser === true && avatarMenu
            }
        </Menu>
    );
};

export default TopMenuComponent;