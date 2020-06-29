import React from 'react';
import '../../index.css';
import AppHeaderComponent from './AppHeaderComponent';
import AppFooterComponent from './AppFooterComponent';
import AppSiderComponent from './AppSiderComponent';
import Routes from '../../Routes';

import { Layout } from 'antd';

const { Header, Footer, Sider, Content } = Layout;

function AppComponent() {
    const color = "rgb(240, 242, 245)";
    return (
        <Layout>
            <Sider style={{backgroundColor: color}}>
            <AppSiderComponent />
            </Sider>
            <Layout>
                <Header style={{backgroundColor: color}}>
                <AppHeaderComponent />
                </Header>
                <Content>
                    {<Routes />}
                </Content>
                <Footer style={{backgroundColor: color}}>
                <AppFooterComponent />
                    </Footer>
            </Layout>
        </Layout>
    )
};

export default AppComponent;