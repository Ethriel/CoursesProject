import React, { useState, useEffect } from 'react';
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

function NestedTableUseState() {
    const { state, setState } = useState({
        isLoading: true,
        total: 0,
        skip: 0,
        pageSize: 5,
        users: null,
        nestedData: null
    });
    const respAmount = MakeRequest("https://localhost:44382/Students/get/amount", { msg: "Hello" }, "get");
    console.log("RESP AMOUNT = ", respAmount);
    const total = respAmount.amount;
    setState({
        total: total
    });

    const skip = state.skip;
    const take = state.pageSize;
    const respUsersCourses = MakeRequest(`https://localhost:44382/Students/get/forpage/${skip}/${take}`, { msg: "Hello" }, "get");
    const respData = respUsersCourses.data;
    setState({
        users: respData,
        isLoading: false
    });
    const columns = getTableCols();
    const users = state.users;
    const data = getTableData(users);

    const expandedRowRender = () => {
        const columns = getNestedCols();
        const users = state.users;
        const data = [];
        let courses = [];
        let obj = {};
        for (let user of users) {
            courses = user.systemUsersTrainingCourses;
            for (let c of courses) {
                obj = getNestedData(c);
                data.push(obj);
            }
        }
        return <Table columns={columns} dataSource={data} pagination={false} />;
    }
    return (
        <Table
            className="components-table-demo-nested"
            columns={columns}
            expandedRowRender={{ expandedRowRender }}
            dataSource={data}
        />
    );
};

export default NestedTableUseState;