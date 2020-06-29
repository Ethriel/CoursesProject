import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Typography } from 'antd';

const { Title } = Typography;

const HAntD = props => {
    const text = props.myText;
    const level = props.level;

    return (
        <Title level={level}>{text}</Title>
    );
};

export default HAntD;