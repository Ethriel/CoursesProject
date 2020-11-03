import React from 'react'
import 'antd/dist/antd.css';
import { Menu } from 'antd';
import { connect } from 'react-redux';
import { NavLink, withRouter } from 'react-router-dom';
import { UserOutlined, LogoutOutlined, ProfileOutlined } from '@ant-design/icons';
import './header-styles.css';
import { SET_AVATAR, SET_EMAIL_CONFIRMED, SET_ROLE } from '../../../reducers/reducersActions';

const { SubMenu } = Menu;

const TopMenuComponent = ({ userAvatar, ...props }) => {
    const mode = isSmallScreen() ? "inline" : "horizontal";

    const menuItems = props.myMenuItems.map((item) => {
        return <Menu.Item
            key={item.key}>
            <NavLink to={item.to} />{item.text}
        </Menu.Item>;
    });

    const avatar = (userAvatar === undefined || userAvatar === null) ? <UserOutlined /> : <img src={userAvatar} alt="No" className="header-user-img" />;

    const userMenu =
        <SubMenu key={5} icon={avatar} >
            <Menu.Item key={"profile"}
                icon={<ProfileOutlined />}
                className="header-sub-my"
            >
                Profile
            </Menu.Item>
            <Menu.Item key={"signout"}
                icon={<LogoutOutlined />}
                className="header-sub-my">
                Sign out
            </Menu.Item>
        </SubMenu>;

    return (
        <Menu
            mode={mode}
            selectedKeys={[props.location.pathname]}
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
)(TopMenuComponent));