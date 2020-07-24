import React from 'react';
import '../../index.css';
import TopMenu from '../common/TopMenuComponent';
import Container from '../common/ContainerComponent';
import Avatar from './UserAvatar';
import headerLogo from '../../img/logo.png';
import H from '../common/HAntD';
import ClearLocalStorage from '../../helpers/ClearLocalStorage';
import { withRouter } from "react-router";

function AppHeaderComponent(props) {
    const role = localStorage.getItem("current_user_role");
    const subItems = [];
    if (role !== undefined) {
        subItems.push({ key: 1, text: "Home", to: "/" });
        subItems.push({ key: 2, text: "Courses", to: "/courses" });
        subItems.push({ key: 3, text: "About us", to: "/aboutus" });
        if (role === "ADMIN") {
            subItems.push({ key: 4, text: "Admin", to: "/admin" });
        }
    }

    const containerClasses = ["display-flex", "space-around-flex", "align-center"];
    const avatarClasses = ["display-flex", "col-flex"];
    const logo = <img src={headerLogo} width={'50px'} height={'50px'} alt="Logo" />
    const headerText = <H level={2} myText="Forge your future with us!" />;

    const profileClick = event => {
        const text = event.item.props.children[1].props.text;
        console.log(props);
        if (text === "Sign out") {
            //ClearLocalStorage();
            props.history.push("/");
        }
        else if(text === "Profile"){
            props.history.push("/profile");
        }
    }
    return (
        <Container classes={containerClasses}>
            {logo}
            {headerText}
            <TopMenu myMenuItems={subItems} />
            <Container classes={avatarClasses}>
                <Avatar menuClick={profileClick} />
            </Container>
        </Container>
    );
}

export default withRouter(AppHeaderComponent);