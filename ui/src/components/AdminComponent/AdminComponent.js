import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import TableComponent from '../common/TableComponent';
import GetTableData from '../../helpers/GetTableData';
import GetNestedTable from '../../helpers/GetNestedTable';
import AdminNestedTable from './AdminNestedTable';
import NestedTableUseState from "./NestedTableUseState";

class AdminComponent extends Component {

    render() {

        const info = GetTableData(10);
        const columns = info.columns;
        const data = info.data;
        const nestedTable = GetNestedTable(5);

        return (
            // <TableComponent
            // className="components-table-demo-nested"
            // columns={columns} 
            // data={data}
            // expandable={{nestedTable}}
            // pageSize={5} 
            // total={info.length} />
            <NestedTableUseState />
        )
    }
}
export default AdminComponent;