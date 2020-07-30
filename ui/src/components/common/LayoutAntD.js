import React from 'react';
import 'antd/dist/antd.css';
import { Layout } from 'antd';

const { Header, Footer, Content } = Layout;

const LayoutAntD = props => {

    const header = props.myHeader;
    const content = props.myContent;
    const footer = props.myFooter;
    const color = "transparent";

    return (
        <Layout >
            <Header style={{ backgroundColor: color }}>{header}</Header>
            <Layout>
                <Content >{content}</Content>
            </Layout>
            <Footer >{footer}</Footer>
        </Layout>
    );
};

export default LayoutAntD;