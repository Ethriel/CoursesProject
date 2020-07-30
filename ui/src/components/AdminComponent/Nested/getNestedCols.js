import React from 'react';
import { Tooltip } from 'antd';

const getNestedCols = () => {
    return [
        { title: "Id", dataIndex: "id", key: "id" , align: "center"},
        {
            title: "Title", dataIndex: "title", key: "title", ellipsis: {
                showTitle: false,
            },
            render: title => (
                <Tooltip placement="topLeft" title={title}>
                    {title}
                </Tooltip>
            ),
            align: "center"
        },
        { title: "Study Date", dataIndex: "studydate", key: "studydate", align: "center" }
    ];
};

export default getNestedCols;