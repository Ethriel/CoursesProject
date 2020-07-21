import React from 'react';
import FooterComponent from '../common/FooterComponent';
import '../../index.css';
import H from '../common/HAntD';

function AppFooterComponent() {

    const footerClasses = ["display-flex", "width-100", "center-flex"];

    return (
        <FooterComponent myClasses={footerClasses}>
            <H level={4} myText="THIS IS FOOTER"></H>
        </FooterComponent>
    )
};

export default AppFooterComponent;