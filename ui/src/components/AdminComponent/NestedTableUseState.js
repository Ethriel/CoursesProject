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
import axios from 'axios';

function NestedTableUseState() {
    const [state, setState] = useState({
        total: 0,
        skip: 0,
        pageSize: 5,
    });

    const [loading, setLoading] = useState(true);
    const { skip, pageSize } = state;
    useEffect(() => {
        async function fetchData() {
            const cfgAmount = {
                method: "get",
                data: { msg: "Hello" },
                url: "https://localhost:44382/Students/get/amount",
                headers: {
                    "Authorization": localStorage.getItem("bearer_header"),
                    "Content-Type": "application/json",
                    "Access-Control-Allow-Origin": "*"
                }
            };

            const respAmount = (await axios(cfgAmount));
            if (respAmount.status === 200) {
                const total = respAmount.data.amount;
                setState(oldState => ({ ...oldState, ...{ total: total } }))
                console.log("RESP AMOUNT = ", total);
                const cfgUsers = {
                    method: "get",
                    data: { msg: "Hello" },
                    url: `https://localhost:44382/Students/get/forpage/${skip}/${pageSize}`,
                    headers: {
                        "Authorization": localStorage.getItem("bearer_header"),
                        "Content-Type": "application/json",
                        "Access-Control-Allow-Origin": "*"
                    }
                };
                const respUsersCourses = (await axios(cfgUsers));
                if (respUsersCourses.status === 200) {
                    const respData = respUsersCourses.data.data;
                    console.log(respData);
                    setState(oldState => ({ ...oldState, users: respData, columns: getTableCols(), data: getTableData(respData) }));
                }
            }
            setLoading(false);

        }
        fetchData();
    }, [skip, pageSize])


    const expandedRowRender = (record, index, indent, expanded) => {
        console.log(`record, index, indent, expanded`, record, index, indent, expanded);
        const columns = getNestedCols();
        const users = state.users;
        console.log(users);
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
            columns={state.columns}
            expandedRowRender={expandedRowRender}
            dataSource={state.data}
            loading={loading}
        />
    )
};

export default NestedTableUseState;