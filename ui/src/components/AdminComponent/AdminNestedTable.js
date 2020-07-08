import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table, Badge, Menu, Dropdown } from 'antd';
import { DownOutlined } from '@ant-design/icons';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import MakeRequest from '../../helpers/MakeRequest';
import getTableCols from './getTableCols';
import getNestedCols from './getNestedCols';
import getNestedData from './getNestedData';
import getTableData from './getTableData';
import getNestedTable from './getNestedTable';

class AdminNestedTable extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isLoading: true,
            total: 0,
            skip: 0,
            pageSize: 5,
            users: null,
            nestedData: null
        };
        //this.expandedRowRender = this.expandedRowRender.bind(this);
    }
    async componentDidMount() {
        const respAmount = await MakeRequestAsync("https://localhost:44382/Students/get/amount", { msg: "Hello" }, "get");
        const total = respAmount.amount;
        this.setState({
            total: total
        });

        const skip = this.state.skip;
        const take = this.state.pageSize;
        const respUsersCourses = await MakeRequestAsync(`https://localhost:44382/Students/get/forpage/${skip}/${take}`, { msg: "Hello" }, "get");
        const data = respUsersCourses.data;
        this.setState({
            users: data,
            isLoading: false
        });
        //this.expandedRowRender();
    }

    expandedRowRender = () => {
        const users = this.state.users;


    }
    render() {
        let columns = [];
        let data = [];
        let rowRender = () => { return <Table />; };
        if (!this.state.isLoading) {
            const users = this.state.users;
            columns = getTableCols();
            data = getTableData(users);
            rowRender = () => { return getNestedTable(users); };
        }

        return (
            <Table
                className="components-table-demo-nested"
                columns={columns}
                expandable={{ rowRender }}
                expandedRowRender={{ rowRender }}
                dataSource={data}
            />
        )
    }
}

export default AdminNestedTable;