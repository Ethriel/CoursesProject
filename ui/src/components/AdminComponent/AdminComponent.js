import React, { Component } from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import TableComponent from '../common/TableComponent';
import GetTableData from '../../helpers/GetTableData';

class AdminComponent extends Component {

    render() {

        const info = GetTableData(10);
        const columns = info.columns;
        const data = info.data;

        return (
            <TableComponent columns={columns} data={data} pageSize={5} total={info.length} />
        )
    }
}
export default AdminComponent;