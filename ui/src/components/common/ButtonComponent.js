import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Button } from 'antd';

const ButtonComponent = props => {
    return (
        <Button
            type="primary"
            size={props.mySize}
            // style={{backgroundColor: props.myBgColor}}
            onClick={props.myHandler}
            disabled={props.disabled}
            style={{ maxWidth: 150 }}>
            {props.myText}
        </Button>
    );
};

export default ButtonComponent;