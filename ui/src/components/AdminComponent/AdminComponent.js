import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import AdminTable from "./AdminTable";
import Container from '../common/ContainerComponent';
import { Input } from 'antd';

const { Search } = Input;

class AdminComponent extends Component {

    render() {
        const outerContainerClasses = ["width-100", "display-flex", "col-flex"];
        const innerContainerClasses = ["width-30", "display-flex", "space-around-flex"];
        return (
            <Container classes={outerContainerClasses}>
                <Container classes={innerContainerClasses}>
                <Search placeholder="input search text" onSearch={value => console.log(value)} enterButton />
                </Container>
                <AdminTable />
            </Container>
        )
    }
}
export default AdminComponent;