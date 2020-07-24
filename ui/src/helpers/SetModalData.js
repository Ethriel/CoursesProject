import IsAxiosError from './IsAxiosError';

const SetModalData = (error) => {
    let modalData = {
        message: "",
        errors: []
    };
    if (IsAxiosError(error)) {
        modalData.message = error.response.data.message;
        modalData.errors = error.response.data.errors;
    }
    else {
        modalData.message = `${error.message}`
        modalData.errors.push(error.stack)
    }
    return modalData;
};

export default SetModalData;