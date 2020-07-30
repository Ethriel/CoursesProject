import React from 'react';
import AppHeaderComponent from './Header/AppHeaderComponent';
import Routes from '../../Routes/Routes';
import AppFooterComponent from './Footer/AppFooterComponent';
import { Layout } from 'antd';

const { Content, Header, Footer } = Layout;

const AppComponent = () => {
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