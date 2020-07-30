import React from 'react';
import ContainerComponent from '../../common/ContainerComponent';

const FooterComponent = props => {

    const classes = props.myClasses;
    return(
        <ContainerComponent classes={classes}>
            {props.children}
        </ContainerComponent>
    );
};

export default FooterComponent;