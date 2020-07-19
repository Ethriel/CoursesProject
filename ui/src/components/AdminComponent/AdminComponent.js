import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import AdminTable from "./AdminTable";
import Container from '../common/ContainerComponent';
import { Input } from 'antd';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

const { Search } = Input;

class AdminComponent extends Component {

    render() {
        return (
            <AdminTable />
        )
    }
}
export default AdminComponent;