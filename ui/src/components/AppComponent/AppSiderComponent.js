import React from 'react';
import '../../index.css';
import SideMenuAntD from '../common/SideMenuAntD';

function AppSiderComponent() {

    const subItems = [
        { key: 1, text: "Home", to: "/" },
        { key: 2, text: "Courses", to: "/courses" },
        { key: 3, text: "About us", to: "/aboutus" },
        { key: 4, text: "Admin", to: "/admin" }
    ];
    return (
        <SideMenuAntD myMenuItems={subItems} />
    );
};

export default AppSiderComponent;