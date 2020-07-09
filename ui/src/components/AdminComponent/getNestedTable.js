import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import getNestedCols from './getNestedCols';
import getNestedData from './getNestedData';
import axios from 'axios';

const GetNestedTable = (props) => {
    const [state, setState] = useState({data: []});
    const [loading, setLoading] = useState(true);
    const userId = props.userId;
    const signal = axios.CancelToken.source();
    const abortContoller = new AbortController();
    const abortSignal = abortContoller.signal;
    useEffect(() => {
        async function fetchData() {
            const cfg = {
                method: "get",
                data: { msg: "Hello" },
                url: `https://localhost:44382/UserCourses/get/${userId}`,
                signal: abortSignal,
                headers: {
                    "Authorization": localStorage.getItem("bearer_header"),
                    "Content-Type": "application/json",
                    "Access-Control-Allow-Origin": "*"
                }
            };
            const response = await axios(cfg);
            console.log("RESPONSE: ", response);
            if (response.status === 200) {
                const coursesData = response.data.data;
                console.log("DATA IN NESTED", coursesData);
                const data = [];
                let obj = {};
                for (let c of coursesData) {
                    obj = getNestedData(c);
                    data.push(obj);
                };
                setState(oldState => ({ ...oldState, ...{ data: data} }));
                setLoading(false);
                return function cleanup(){
                    abortContoller.abort();
                }
            }
        }
        fetchData();
    });
    const columns = getNestedCols();
    return <Table columns={columns} dataSource={state.data} pagination={false} loading={loading} />;
};

export default GetNestedTable;