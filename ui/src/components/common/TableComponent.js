import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table } from 'antd';


function TableComponent(props) {
    const exp = props.expandable;
    return (
        <Table
            className={props.className}
            expandable={exp}
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