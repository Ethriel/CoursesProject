import React from 'react';
import '../../index.css';
import ContainerComponent from './ContainerComponent';

const HeaderComponent = props => {

    const classes = props.myClasses;
    return(
        <ContainerComponent classes={classes}>
            {props.children}
        </ContainerComponent>
    );
};

export default HeaderComponent;