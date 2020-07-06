import React from 'react';
import '../../index.css';
import AppSiderComponent from './AppSiderComponent';
import Routes from '../../Routes';

import { Layout } from 'antd';

const { Sider, Content } = Layout;

function AppComponent() {
    const color = "rgb(240, 242, 245)";
    return (
        <Layout>
            <Sider style={{ backgroundColor: color }}>
                <AppSiderComponent />
            </Sider>
            <Content>
                {<Routes />}
            </Content>
        </Layout>
    )
};

export default AppComponent;