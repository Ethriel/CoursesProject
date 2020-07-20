import React from 'react';
import HeaderComponent from '../common/HeaderComponent';
import headerLogo from '../../img/logo.png';
import '../../index.css';
import H from '../common/HAntD';

function AppHeaderComponent() {

    const logo = <img src={headerLogo} width={'50px'} height={'50px'} alt="Logo" />
    const headerClasses = ["display-flex", "width-100", "center-flex", "header-flex"];
    const headerText = <H level={4} myText="Forge your future with us!" />;

    return (
        <HeaderComponent myClasses={headerClasses}>
            {logo}
            {headerText}
        </HeaderComponent>
    );
};

export default AppHeaderComponent;