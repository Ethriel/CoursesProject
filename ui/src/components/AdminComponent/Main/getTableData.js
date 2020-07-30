function getTableData(users){
    const data = [];
    let obj = {};

    for(let user of users){
        obj = {
            key: user.id,
            id: user.id,
            firstname: user.firstName,
            lastname: user.lastName,
            age: user.age,
            email: user.email,
            registereddate: user.registeredDate
        }
        data.push(obj);
    }
    return data;
}

export default getTableData;