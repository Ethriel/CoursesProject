const IsAxiosError = (error) => {
    if (error.isAxiosError) {
        return true;
    }
    return false;
}

export default IsAxiosError;