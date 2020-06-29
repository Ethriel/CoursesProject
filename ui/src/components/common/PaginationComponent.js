import React from 'react';
import { Pagination } from 'antd';
import '../../css/styles.css';

const PaginationComponent = props => {
    return (
        <Pagination 
        defaultCurrent={props.defaultCurrent} 
        pageSize={props.pageSize} 
        total={props.total} 
        onChange={props.onChange} />
    );
};

export default PaginationComponent;