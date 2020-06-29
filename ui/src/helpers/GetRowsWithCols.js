import React from 'react';
import { Row, Col } from 'antd';

const GetRowsWithCols = (elements, toTake) => {
    let rows = [];
    let slice = toTake;
    for (let i = 0; i < elements.length;) {
        let items = elements.slice(i, slice);
        let row = getRow(items);
        rows.push(row);
        i += toTake;
        slice += toTake;
    };

    return rows;
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