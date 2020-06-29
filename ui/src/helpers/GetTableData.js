import React from 'react';

function GetTableData(records) {
    const columnsNames = ["Id", "Name", "Last Name", "Age", "Email", "Registered Date", "Study Date"];
    let columns = [];
    let i = columnsNames.length;
    for (let colName of columnsNames) {
        let indexAndKey = colName.replace(/\s/g, '').toLowerCase();
        let col = {
            title: colName,
            dataIndex: indexAndKey,
            key: indexAndKey,
            sorter: {
                compare: (a, b) => a.id - b.id,
                multiple: i
            },
            align: 'center'
        };
        columns.push(col);
        i--;
    };
    let data = [];
    for (let i = 0; i < records; i++) {
        let obj = {
            id: i,
            key: `${i}`,
            name: `Name ${i}`,
            lastname: `Last name ${i}`,
            age: `Age ${i}`,
            email: `Email ${i}`,
            registereddate: `Date ${i}`,
            studydate: `Date ${i}`
        };
        data.push(obj);
    };

    return (
        {
            columns: columns,
            data: data
        }
    );
};

export default GetTableData;