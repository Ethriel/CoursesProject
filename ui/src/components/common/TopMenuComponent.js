import React from 'react'
import 'antd/dist/antd.css';
import '../../index.css';
import { Menu } from 'antd';
import { NavLink } from 'react-router-dom';
import { UserOutlined, LogoutOutlined, ProfileOutlined } from '@ant-design/icons';
const { SubMenu } = Menu;

const TopMenuComponent = (props) => {
    const mode = isSmallScreen() ? "inline" : "horizontal";

    const menuItems = props.myMenuItems.map((item) => {
        return <Menu.Item
            key={item.key}>
            <NavLink to={item.to} />{item.text}
        </Menu.Item>;
    });

    const userMenu =
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
        <Menu mode={mode}
            onClick={props.menuClick}
            className={props.className}>
            {menuItems}
            {
                props.isUser === true && userMenu
            }
        </Menu>
    );
};

const isSmallScreen = () => {
    const width = window.innerWidth;
    return width <= 600;
};

export default TopMenuComponent;