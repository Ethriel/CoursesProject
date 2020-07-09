function getTableCols(){
    return [
        {title: "Id", dataIndex: "id", key: "id", sorter: true, align: "center"},
        {title: "First Name", dataIndex: "firstname", key: "firstname", sorter: true, align: "center"},
        {title: "Last Name", dataIndex: "lastname", key: "lastname", sorter: true, align: "center"},
        {title: "Age", dataIndex: "age", key: "age", sorter: true, align: "center"},
        {title: "Email", dataIndex: "email", key: "email", align: "center"},
        {title: "Registered Date", dataIndex: "registereddate", key: "registereddate", align: "center"}
    ];
};

export default getTableCols;