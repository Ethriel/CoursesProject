import React, { useState, useEffect } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table } from 'antd';
import getTableCols from './getTableCols';
import getTableData from './getTableData';
import NestedTable from './NestedTable';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

function AdminTable() {
    const [table, setTable] = useState({});
    const [loading, setLoading] = useState(true);
    const [paginationState, setPaginationState] = useState({
        position: ['none', 'bottomCenter'],
        current: 1,
        pageSize: 3,
        total: 5
    });
    
    useEffect(() => {
        const signal = axios.CancelToken.source();
        async function fetchData() {
            // getting amount
            try {
                const respAmount = await MakeRequestAsync("Students/get/amount", { msg: "Hello" }, "get", signal.token);
                if (respAmount.status === 200) {
                    const total = respAmount.data.amount;
                    setPaginationState(oldPagination => ({ ...oldPagination, ...{ total: total } }));
                }
            } catch (error) {
                console.log(error)
            }
        }

        fetchData();
        
        return function cleanup() {
            signal.cancel("CANCEL IN GET AMOUNT");
        }
    }, []);

    useEffect(() => {
        const signal = axios.CancelToken.source();
        async function fetchData() {
            // getting users
            try {
                const sorting = {
                    sortField: "id",
                    sortOrder: "descend",
                    pagination: paginationState
                };
                const respUsersCourses = await MakeRequestAsync(`Students/post/sort`, sorting, "post", signal.token);

                if (respUsersCourses.status === 200) {
                    const respData = respUsersCourses.data.data;
                    setTable({ columns: getTableCols(), data: getTableData(respData) });
                }
            } catch (error) {
                console.log(error);
            }
            finally {
                setLoading(false);
            }
        }

        fetchData();

        return function cleanup() {
            signal.cancel("CANCEL IN GET USERS");
        }
    },[paginationState]);

    const handleChange = async (pagination, filters, sorter) => {
        const signal = axios.CancelToken.source();
        const url = `Students/post/sort`;
        const current = pagination.current;
        const pag = pagination;

        setPaginationState(oldPagination => ({ ...oldPagination, ...{ current: current } }));

        const sorting = {
            sortField: sorter.field,
            sortOrder: sorter.order,
            pagination: pag
        };

        const response = await MakeRequestAsync(url, sorting, "post", signal.token);
        
        if (response.status === 200) {
            const respUsers = response.data.data;
            setTable({ columns: getTableCols(), data: getTableData(respUsers) });
        }
    }
    const expandedRowRender = (record, index, indent, expanded) => {
        try {
            if (expanded === true) {
                return <NestedTable userId={record.id} />;
            }
            else {
                return <Table />
            }
        } catch (error) {
            console.log(error);
        }
    }
    return (
        <Table
            className="components-table-demo-nested"
            columns={table.columns}
            expandedRowRender={expandedRowRender}
            dataSource={table.data}
            loading={loading}
            pagination={paginationState}
            onChange={handleChange}
        />
    )
};

export default AdminTable;