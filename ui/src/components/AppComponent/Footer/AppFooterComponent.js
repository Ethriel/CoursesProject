import React from 'react';
import FooterComponent from './FooterComponent';
import { FacebookFilled, GithubFilled } from '@ant-design/icons';

const AppFooterComponent = () => {

    const footerClasses = ["display-flex", "width-30", "space-around-flex", "center-a-div-no-margin"];

    const redirectTo = url => {
        window.open(url, "_blank");
    };

    const faceBookClick = (event) => {
        redirectTo("https://www.facebook.com/profile.php?id=100006888912115");
    };

    const gitHubClick = (event) => {
        redirectTo("https://github.com/Ethriel");
    };

    const className = "social-link";
    const facebookClassName = className.concat(" ", "social-link-facebook");

    return (
        <FooterComponent myClasses={footerClasses}>
            <FacebookFilled onClick={faceBookClick} className={facebookClassName} title="Me on Facebook"/>
            <GithubFilled onClick={gitHubClick} className={className} title="Me on Github"/>
        </FooterComponent>
    )
};

export default AppFooterComponent;