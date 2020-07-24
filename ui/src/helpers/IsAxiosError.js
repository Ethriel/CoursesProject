const IsAxiosError = (error) => {
    if (error.response || error.request) {
        return true;
    }
    return false;
}

export default IsAxiosError;