import React, { useState, useEffect } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table, Input } from 'antd';
import getTableCols from './getTableCols';
import getTableData from './getTableData';
import NestedTable from './NestedTable';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

const { Search } = Input;

function AdminTable() {
    const [table, setTable] = useState({});
    const [loading, setLoading] = useState(true);
    const [paginationState, setPaginationState] = useState({
        position: ['none', 'bottomCenter'],
        current: 1,
        pageSize: 3,
        total: 5
    });

    async function getAmount() {
        const signal = axios.CancelToken.source();
        try {
            const response = await MakeRequestAsync("Students/get/amount", { msg: "Hello" }, "get", signal.token);
            if (response.status === 200) {
                const total = response.data;
                setPaginationState(oldPagination => ({ ...oldPagination, ...{ total: total } }));
            }
        } catch (error) {
            console.log(error);
        }
    }

    async function getUsers() {
        const signal = axios.CancelToken.source();
        try {
            const sorting = {
                sortField: "id",
                sortOrder: "descend",
                pagination: paginationState
            };
            const response = await MakeRequestAsync(`Students/post/sort`, sorting, "post", signal.token);

            if (response.status === 200) {
                const respData = response.data;
                setTable({ columns: getTableCols(), data: getTableData(respData) });
            }
        } catch (error) {
            console.log(error);
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        const signal = axios.CancelToken.source();
        async function fetchData() {
            await getAmount();
        }

        fetchData();

        return function cleanup() {
            signal.cancel("CANCEL IN GET AMOUNT");
        }
    }, []);

    useEffect(() => {
        const signal = axios.CancelToken.source();
        async function fetchData() {
            await getUsers();
        }

        fetchData();

        return function cleanup() {
            signal.cancel("CANCEL IN GET USERS");
        }
    }, []);

    const handleChange = async (pagination, filters, sorter) => {
        setLoading(true);
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

        try {
            const response = await MakeRequestAsync(url, sorting, "post", signal.token);

            if (response.status === 200) {
                const respUsers = response.data;
                setTable({ columns: getTableCols(), data: getTableData(respUsers) });
            }
        } catch (error) {
            console.log(error);
        } finally {
            setLoading(false);
        }

    };

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
    };

    const onSearchHandler = async (value, event) => {
        const isEmpty = value === "";
        if (!isEmpty) {
            const search = value;
            const signal = axios.CancelToken.source();

            const response = await MakeRequestAsync(`Students/get/${search}`, { msg: "hello" }, "get", signal.token);
            if (response.status === 200) {
                const data = response.data;
                const total = data.length;
                setPaginationState(oldPagination => ({ ...oldPagination, ...{ total: total } }));
                setTable({ columns: getTableCols(), data: getTableData(data) });
            }
        }
        else {
            // if search criteria is empty - get all users
            setLoading(true);
            await getAmount();
            await getUsers();
        }
    };

    const outerContainerClasses = ["width-100", "display-flex", "col-flex"];
    const innerContainerClasses = ["width-30", "display-flex", "mb-25px"];

    return (
        <Container classes={outerContainerClasses}>
            <H level={2} myText="Admin table" />
            <Container classes={innerContainerClasses}>
                <Search
                    placeholder="Enter search criteria"
                    onSearch={onSearchHandler}
                    allowClear={true}
                    enterButton />
            </Container>
            <Table
                className="components-table-demo-nested"
                columns={table.columns}
                expandedRowRender={expandedRowRender}
                dataSource={table.data}
                loading={loading}
                pagination={paginationState}
                onChange={handleChange}
            />
        </Container>

    )
};

export default AdminTable;