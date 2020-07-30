import React from 'react';
import { Pagination } from 'antd';

const PaginationComponent = props => {
    const pagination = props.pagination;
    return (
        <Pagination 
        defaultCurrent={1} 
        pageSize={pagination.pageSize} 
        total={pagination.total}
        current={pagination.page}
        onChange={props.onChange} />
    );
};

export default PaginationComponent;