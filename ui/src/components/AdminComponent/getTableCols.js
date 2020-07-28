import React from 'react';

const names = ["Id", "First Name", "Last Name", "Age", "Email", "Registered Date"];
const indexes = names.map((value) => {
    let index = value.replace(" ", "");
    index = index.toLowerCase();
    return index;
});

const getCol = (name, index, sorter, editable) => {
    return {
        title: name,
        dataIndex: index,
        key: index,
        sorter: sorter,
        align: "center",
        editable: editable
    }
};

const getTableCols = (editClick) => {
    const cols = [];
    let sorter = false;
    let editable = false;
    for (let i = 0; i < names.length; i++) {
        sorter = i < 4 ? true : false;
        editable = (i === 1 || i === 2 || i === 4) ? true : false;
        const col = getCol(names[i], indexes[i], sorter, editable);
        cols.push(col);
    };
    cols.push({
        title: "Action",
        dataIndex: "action",
        key: "action",
        align: "center",
        render: 
        (_,record) => {
            return(
                <a onClick={() => editClick(record.id)}>Edit</a>
            )
        }
    })
    return cols;
};

export default getTableCols;