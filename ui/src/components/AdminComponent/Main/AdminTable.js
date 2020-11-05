import React, { useState, useEffect, useCallback } from 'react';
import { Table, Input } from 'antd';
import { withRouter } from "react-router";
import { connect } from 'react-redux';
import 'antd/dist/antd.css';
import getTableCols from './getTableCols';
import getTableData from './getTableData';
import NestedTable from '../Nested/NestedTable';
import Container from '../../common/ContainerComponent';
import H from '../../common/HAntD';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from 'axios';
import NotificationError from '../../common/notifications/notification-error';
import { ADMIN } from '../../common/roles';
import { forbidden } from '../../../Routes/RoutersDirections';

const { Search } = Input;
const url = "Students/post/searchAndSort";

const AdminTable = ({ userRole, history, ...props }) => {

    const setCatch = (error) => {
        NotificationError(error);
    };

    const setFinally = () => {
        setLoading(false);
    }

    const [table, setTable] = useState({});
    const [loading, setLoading] = useState(true);
    const [paginationState, setPaginationState] = useState();

    const getCols = useCallback(() => {
        const editClick = id => {
            history.push(`/admin/editStudent/${id}`);
        }
        return getTableCols(editClick);
    }, [history]);

    const getUsers = useCallback(async (token) => {
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
                const respData = response.data.data;
                const pagination = respData.pagination;
                const users = respData.users;
                setPaginationState(pagination);
                setTable({ columns: getCols(), data: getTableData(users) });
            }

        } catch (error) {
            setCatch(error);
        } finally {
            setFinally();
        }
    }, [getCols]);

    useEffect(() => {
        const signal = axios.CancelToken.source();
        if (userRole !== ADMIN) {
            history.push(forbidden);
        }
        else {
            getUsers(signal.token);
        }

        return function cleanup() {
            signal.cancel("CANCEL IN GET USERS");
        };
    }, [userRole, history, getUsers]);


    // handles changes in sorting and pagination
    const handleChange = async (pagination, filters, sorter) => {
        setLoading(true);
        const signal = axios.CancelToken.source();

        const newPagination = {
            page: pagination.current,
            pageSize: pagination.pageSize,
            position: pagination.position,
            total: pagination.total
        }

        const searchValue = localStorage.getItem("search_value");
        const isSearch = searchValue !== "" && searchValue !== null && searchValue !== undefined;

        const sorting = {
            sortField: sorter.field,
            sortOrder: sorter.order
        };

        const searchAndSort = {
            searchField: searchValue,
            sort: sorting,
            pagination: newPagination,
            isSearch: isSearch
        };

        try {
            const response = await MakeRequestAsync(url, searchAndSort, "post", signal.token);

            const data = response.data.data;
            const responsePagination = data.pagination;
            const users = data.users;

            setPaginationState(responsePagination);
            setTable({ columns: getCols(), data: getTableData(users) });

        } catch (error) {
            setCatch(error);
        } finally {
            setFinally();
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
            setCatch(error);
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

            try {
                const response = await MakeRequestAsync(url, searchAndSort, "post", signal.token);
                const data = response.data.data;
                const users = data.users;
                const pagination = data.pagination;

                setPaginationState(pagination);
                setTable({ columns: getCols(), data: getTableData(users) });
            } catch (error) {
                setCatch(error);
            } finally {
                setFinally();
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
        <>
            <Container classes={outerContainerClasses}>
                <H level={4} myText="Students table" />
                <Container classes={innerContainerClasses}>
                    <Search
                        className="ant-input-search-my"
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
        </>
    )
};

export default withRouter(connect(
    state => ({
        userRole: state.userReducer.role
    })
)(AdminTable));