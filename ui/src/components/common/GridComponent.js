import React from 'react';
import '../../index.css';
import 'antd/dist/antd.css';
import GetRowsAndCols from '../../helpers/GetRowsWithCols';

const GridComponent = props => {
    const cols = props.colNum;
    const elements = props.elementsToDisplay;
    const toTake = Math.trunc(24 / cols);
    const rows = GetRowsAndCols(elements, toTake);

    return (
        <>
        {rows.map((item) => {
                return item;
            })}
        </>
    );
};

export default GridComponent;