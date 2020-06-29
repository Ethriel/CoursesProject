import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table } from 'antd';


const TableComponent = props => {
    console.log(props.expandable);
    const ext = props.expandable;
    return (
        <Table
            className={props.className}
            expandable={{ext}}
            columns={props.columns}
            dataSource={props.data}
            pagination={
                {
                    position: ['none', 'bottomCenter'],
                    pageSize: props.pageSize,
                    total: props.total
                }
            } />
    );
};

export default TableComponent;