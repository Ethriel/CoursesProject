const ConvertArray = arr => {
    let str = arr.toString();
    str = str.split(",").join(" ");
    return str;
}

export default ConvertArray;