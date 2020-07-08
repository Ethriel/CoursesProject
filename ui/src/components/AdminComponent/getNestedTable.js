import React from 'react';
import { Table } from 'antd';
import getNestedCols from './getNestedCols';
import getNestedData from './getNestedData';
function getNestedTable(users) {
    const columns = getNestedCols();
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
};

export default getNestedTable;