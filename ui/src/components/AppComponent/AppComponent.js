import React from 'react';
import '../../index.css';
import AppHeaderComponent from './AppHeaderComponent';
import Routes from '../../Routes/Routes';
import AppFooterComponent from './AppFooterComponent';
import { Layout } from 'antd';

const { Content, Header, Footer } = Layout;

function AppComponent() {
    const color = "rgb(240, 242, 245)";
    return (
        <Layout>
            <Header>
                <AppHeaderComponent />
            </Header>
            <Content className="overflow-auto">
                {<Routes />}
            </Content>
            <Footer>
                <AppFooterComponent />
            </Footer>
        </Layout>
    )
};

export default AppComponent;