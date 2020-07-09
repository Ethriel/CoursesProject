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
import GetNestedTable from './GetNestedTable';
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
                    setState(oldState => ({ ...oldState, users: respData, columns: getTableCols(), data: getTableData(respData) }));
                }
            }
            setLoading(false);

        }
        fetchData();
    }, [skip, pageSize])


    const expandedRowRender = (record, index, indent, expanded) => {
        if(expanded === true){
            return <GetNestedTable userId={record.id} />;
        }
        else{
            return <Table />
        }
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