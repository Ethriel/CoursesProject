import React from 'react';
import { Table } from 'antd';

function GetNestedTable(records) {
    const columnsNames = ["Id", "Name", "Data"];
    let columns = [];

    for (let colName of columnsNames) {
        let indexAndKey = colName.replace(/\s/g, '').toLowerCase();
        let col = {
            title: colName,
            dataIndex: indexAndKey,
            key: indexAndKey,
            align: 'center'
        };
        columns.push(col);
    };
    let data = [];
    for (let i = 0; i < records; i++) {
        let obj = {
            id: i,
            name: `Name ${i}`,
            data: `Data ${i}`
        };
        data.push(obj);
    };

    return (
        <Table columns={columns} dataSource={data} pagination={false} />
    );
};

export default GetNestedTable;