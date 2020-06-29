import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Table } from 'antd';


const TableComponent = props => {

    return (
        <Table
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