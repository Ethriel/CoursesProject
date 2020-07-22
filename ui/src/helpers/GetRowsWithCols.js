import React from 'react';
import { Row, Col } from 'antd';

const GetRowsWithCols = (elements) => {
    const row = getRow(elements);
    return row;
};

function getRow(items) {
    let columns = items.map((item) => {
        return <Col key={item.key}>{item}</Col>;
    });
    return <Row key={updateIndex (1)}>{columns}</Row>;
};

let index = 0;
function updateIndex(key) {
    return ( key + index++);
};


export default GetRowsWithCols;