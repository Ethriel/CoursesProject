import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import getNestedCols from './getNestedCols';
import getNestedData from './getNestedData';
import axios from 'axios';

const NestedTable = (props) => {
    const [state, setState] = useState({ data: [] });
    const [loading, setLoading] = useState(true);
    const userId = props.userId;
    const signal = axios.CancelToken.source();
    useEffect(() => {
        async function fetchData() {
            // config to get user's courses
            const cfg = {
                method: "get",
                data: { msg: "Hello" },
                url: `https://localhost:44382/UserCourses/get/${userId}`,
                cancelToken: signal.token,
                headers: {
                    "Authorization": localStorage.getItem("bearer_header"),
                    "Content-Type": "application/json",
                    "Access-Control-Allow-Origin": "*"
                }
            };
            try {
                // getting courses
                const response = await axios(cfg);
                if (response.status === 200) {
                    const coursesData = response.data.data;
                    const data = [];
                    let obj = {};
                    for (let c of coursesData) {
                        obj = getNestedData(c);
                        data.push(obj);
                    };
                    setState(oldState => ({ ...oldState, ...{ data: data } }));
                }
            } catch (error) {
                console.log(error)
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