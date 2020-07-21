import React from 'react';
import { Typography } from 'antd';
import Container from '../common/ContainerComponent';

const { Text, Paragraph } = Typography;

const StaticUserInfoComponent = props => {
    const containerClasses = ["display-flex", "width-100", "space-between-flex"];
    const label = props.label;
    const text = props.text;

    return(
        <Container classes={containerClasses}>
            <Paragraph>{label}</Paragraph>
            <Text>{text}</Text>
        </Container>
    )
}

export default StaticUserInfoComponent;