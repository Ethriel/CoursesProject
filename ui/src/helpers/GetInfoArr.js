function GetInfoArr(quantity) {
    let info = [];

    for (let i = 0; i < quantity; i++) {
        info.push(
            { title: `Title ${i + 1}`, description: `Description ${i + 1}`, key: i }
        );
    }

    return info;
}

export default GetInfoArr;