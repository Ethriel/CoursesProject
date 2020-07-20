import React from 'react';
import '../../index.css';
import TopMenu from '../common/TopMenuComponent';
import Container from '../common/ContainerComponent';
import Avatar from './UserAvatar';
import headerLogo from '../../img/logo.png';
import H from '../common/HAntD';


function AppHeaderComponent() {
    const role = localStorage.getItem("current_user_role");
    const subItems = [
        { key: 1, text: "Home", to: "/" },
        { key: 2, text: "Courses", to: "/courses" },
        { key: 3, text: "About us", to: "/aboutus" },
    ];
    if (role === "ADMIN") {
        subItems.push({ key: 4, text: "Admin", to: "/admin" });
    }

    const containerClasses = ["display-flex", "space-around-flex", "align-center"];
    const avatarClasses = ["display-flex", "col-flex"];
    const logo = <img src={headerLogo} width={'50px'} height={'50px'} alt="Logo" />
    const headerText = <H level={4} myText="Forge your future with us!" />;

    const avatarClick = event => {
        console.log("Clicked", event);
    }

    return (
        <Container classes={containerClasses}>
            {logo}
            {headerText}
            <TopMenu myMenuItems={subItems} />
            <Container classes={avatarClasses}>
                <Avatar menuClick={avatarClick} />

            </Container>
        </Container>
    );
}

export default AppHeaderComponent;