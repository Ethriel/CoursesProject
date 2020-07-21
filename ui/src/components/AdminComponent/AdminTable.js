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
const url = "Students/post/searchAndSort";

function AdminTable() {
    const [table, setTable] = useState({});
    const [loading, setLoading] = useState(true);
    const [paginationState, setPaginationState] = useState();

    async function getUsers(token) {
        try {
            const sorting = {
                sortField: "id",
                sortOrder: "descend"
            };

            const searchAndSort = {
                searchField: "",
                sort: sorting,
                pagination: null,
                isSearch: false
            };

            const response = await MakeRequestAsync(url, searchAndSort, "post", token);

            if (response.status === 200) {
                const respData = response.data;
                const pagination = respData.pagination;
                const users = respData.users;
                setPaginationState(pagination);
                setTable({ columns: getTableCols(), data: getTableData(users) });
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
            await getUsers(signal.token);
        }

        fetchData();

        return function cleanup() {
            signal.cancel("CANCEL IN GET USERS");
        }
    }, []);

    // handles changes in sorting and pagination
    const handleChange = async (pagination, filters, sorter) => {
        setLoading(true);
        const signal = axios.CancelToken.source();

        const searchValue = localStorage.getItem("search_value");
        const isSearch = searchValue !== "" && searchValue !== null && searchValue !== undefined;

        const sorting = {
            sortField: sorter.field,
            sortOrder: sorter.order
        };

        const searchAndSort = {
            searchField: searchValue,
            sort: sorting,
            pagination: pagination,
            isSearch: isSearch
        };

        try {
            const response = await MakeRequestAsync(url, searchAndSort, "post", signal.token);

            if (response.status === 200) {
                const data = response.data;
                const pagination = data.pagination;
                const users = data.users;

                setPaginationState(pagination);
                setTable({ columns: getTableCols(), data: getTableData(users) });
            }

        } catch (error) {
            console.log(error);
        } finally {
            setLoading(false);
        }
    };

    // renders inner table
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

    // handles search
    const onSearchHandler = async (value, event) => {
        const signal = axios.CancelToken.source();
        const isEmpty = value === "";
        localStorage.setItem("search_value", value);

        if (!isEmpty) {

            const sorting = {
                sortField: "id",
                sortOrder: "descend"
            };

            const searchAndSort = {
                searchField: value,
                sort: sorting,
                isSearch: true,
                pagination: paginationState
            };

            const response = await MakeRequestAsync(url, searchAndSort, "post", signal.token);
            if (response.status === 200) {
                const data = response.data;
                const users = data.users;
                const pagination = data.pagination;

                setPaginationState(pagination);
                setTable({ columns: getTableCols(), data: getTableData(users) });
            }
        }
        else {
            // if search criteria is empty - get all users
            setLoading(true);
            await getUsers(signal.token);
        }
    };

    const outerContainerClasses = ["display-flex", "col-flex"];
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