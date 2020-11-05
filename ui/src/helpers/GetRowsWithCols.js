import React from 'react';
import { Row, Col } from 'antd';

const GetRowsWithCols = (elements) => {
    const row = getRow(elements);
    return row;
};

const getRow = (items) => {
    let columns = items.map((item) => {
        return <Col span={6} key={item.key}>{item}</Col>;
    });
    return <Row gutter={[24, 24]} key={updateIndex(1)}>{columns}</Row>;
};

let index = 0;
const updateIndex = (key) => {
    return (key + index++);
};


export default GetRowsWithCols;