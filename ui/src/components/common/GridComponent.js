import React from 'react';
import '../../index.css';
import 'antd/dist/antd.css';
import GetRowsAndCols from '../../helpers/GetRowsWithCols';

const GridComponent = props => {
    const elements = props.elementsToDisplay;
    const rows = GetRowsAndCols(elements);
    console.log("ROWS", rows);
    return (
        <>
            {rows}
        </>
    );
};

export default GridComponent;