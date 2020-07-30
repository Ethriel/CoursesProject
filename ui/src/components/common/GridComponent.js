import React from 'react';
import 'antd/dist/antd.css';
import GetRowsAndCols from '../../helpers/GetRowsWithCols';

const GridComponent = props => {
    const elements = props.elementsToDisplay;
    const rows = GetRowsAndCols(elements);
    return (
        <>
            {rows}
        </>
    );
};

export default GridComponent;