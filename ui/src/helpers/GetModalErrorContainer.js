import React from 'react';
import { Typography } from 'antd';
import Container from '../components/common/ContainerComponent';

const { Paragraph } = Typography;

const GetModalErrorContainer = (errors) => {
    const classes = ["display-flex", "center-a-div", "col-flex", "center-flex"];
    const paragraphs = [];
    if (errors) {
        if (errors.length > 0) {
            paragraphs.push(
                <Paragraph key={getKey()}>Errors:</Paragraph>
            );

            const errosParagraphs = errors.map((error) => {
                return <Paragraph key={getKey()}>{error}</Paragraph>
            });

            for (let paragraph of errosParagraphs) {
                paragraphs.push(paragraph);
            };
        }
    } 

    return (
        <Container classes={classes}>
            {paragraphs}
        </Container>
    );
};

let i = 0;
const getKey = () => {
    let key = i.toString();
    i++;
    return key;
}

export default GetModalErrorContainer;