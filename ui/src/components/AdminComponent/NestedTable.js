import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import getNestedCols from './getNestedCols';
import getNestedData from './getNestedData';
import axios from 'axios';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';

const NestedTable = (props) => {
    const [state, setState] = useState({ data: [] });
    const [loading, setLoading] = useState(true);
    const userId = props.userId;
    const signal = axios.CancelToken.source();
    useEffect(() => {
        async function fetchData() {
            try {
                // getting courses
                const data = { msg: "Hello" };
                const response = await MakeRequestAsync(`UserCourses/get/${userId}`, data, "get", signal.token);
                const coursesData = response.data.data;
                    const info = [];
                    let obj = {};
                    for (let c of coursesData) {
                        obj = getNestedData(c);
                        info.push(obj);
                    };
                    setState(oldState => ({ ...oldState, ...{ data: info } }));
            } catch (error) {
                throw error;
            }
            finally {
                setLoading(false);
            }
        }
        fetchData();
        return function cleanup() {
            signal.cancel("Cancel in nested table");
        }
    }, []);
    const columns = getNestedCols();
    return <Table columns={columns} dataSource={state.data} pagination={false} loading={loading} />;
};

export default NestedTable;