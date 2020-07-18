import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import AdminTable from "./AdminTable";
import Container from '../common/ContainerComponent';
import { Input } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

const { Search } = Input;

const onSearchHandler = async value => {
    const search = value;
    const signal = axios.CancelToken.source();

    const response = await MakeRequestAsync(`Students/get/${search}`, {msg: "hello"}, "get", signal.token);
    const data = response.data;
}

class AdminComponent extends Component {

    render() {
        const outerContainerClasses = ["width-100", "display-flex", "col-flex"];
        const innerContainerClasses = ["width-30", "display-flex", "space-around-flex"];
        return (
            <Container classes={outerContainerClasses}>
                <Container classes={innerContainerClasses}>
                    <Search placeholder="input search text" onSearch={onSearchHandler} enterButton />
                </Container>
                <AdminTable />
            </Container>
        )
    }
}
export default AdminComponent;