function getTableCols(){
    return [
        {title: "Id", dataIndex: "id", key: "id"},
        {title: "First Name", dataIndex: "firstname", key: "firstname"},
        {title: "Last Name", dataIndex: "lastname", key: "lastname"},
        {title: "Age", dataIndex: "age", key: "age"},
        {title: "Email", dataIndex: "email", key: "email"},
        {title: "Registered Date", dataIndex: "registereddate", key: "registereddate"}
    ];
};

export default getTableCols;