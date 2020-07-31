const GetNotificationDescription = (errors) => {
    const description =
        errors.toString()
            .split(",")
            .join("\n");
    return description;
};

export default GetNotificationDescription;