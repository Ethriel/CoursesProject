import React from 'react';
import { Typography } from 'antd';
import Container from '../common/ContainerComponent';

const { Text, Paragraph } = Typography;

function EditableUserInfoComponent(props) {
    const containerClasses = ["display-flex", "width-100", "space-between-flex"];
    const label = props.label;
    const text = props.text;
    const onChange = props.onChange;
    return (
        <Container classes={containerClasses}>
            <Paragraph>{label}</Paragraph>
            <Text editable={{ onChange: onChange }}>{text}</Text>
        </Container>
    )
}

export default EditableUserInfoComponent;