import React from 'react';
import '../../index.css';
import AppSiderComponent from './AppSiderComponent';
import AppHeaderComponent from './AppHeaderComponent';
import Routes from '../../Routes';
import AppFooterComponent from './AppFooterComponent';
import { Layout } from 'antd';

const { Sider, Content, Header, Footer } = Layout;

function AppComponent() {
    const color = "rgb(240, 242, 245)";
    return (
        <Layout style={{ overflow: "initial" }}>
            {/* <Sider style={{ backgroundColor: color, maxWidth: "150px" }}>
                <AppSiderComponent />
            </Sider>
            <Content>
                {<Routes />}
            </Content> */}

            <Header>
                <AppHeaderComponent />
            </Header>
            <Content>
                {<Routes />}
            </Content>
            <Footer>
                <AppFooterComponent />
            </Footer>
        </Layout>
    )
};

export default AppComponent;