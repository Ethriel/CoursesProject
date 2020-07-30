import IsAxiosError from './IsAxiosError';

const SetModalData = (error) => {
    let modalData = {
        message: "",
        errors: []
    };
    if (IsAxiosError(error)) {
        if (error.response) {
            if (error.response.status === 500) {
                modalData.message = "Server error occured";
                modalData.errors.push("Internal server error");
            }
            else if(error.response.status === 401){
                modalData.message = "No access";
                modalData.errors.push("Unauthorised");
            }
            else {
                modalData.message = error.response.data.message;
                modalData.errors = error.response.data.errors;
            }

        }
        else {
            modalData.message = "A network error occured";
            modalData.errors.push("Connection refused. API server is offline");
            modalData.errors.push("Or check your internet connection");
        }
    }
    else {
        modalData.message = "Script error";
        modalData.errors.push(`${error.message}`);
    }
    return modalData;
};

export default SetModalData;