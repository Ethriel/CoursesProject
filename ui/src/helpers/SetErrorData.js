import IsAxiosError from './IsAxiosError';
import MakeRequestAsync from './MakeRequestAsync';
import axios from 'axios';

const logUrl = "Error/logJavascriptError";

const SetErrorData = (error) => {
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
            else if (error.response.status === 401) {
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
        modalData.errors.push("Contact the administrator if needed");
        
        // send an error to server to log it
        const sendError = async () => {
            const signal = axios.CancelToken.source();
            const toLogError = {
                message: error.message,
                stack: error.stack
            }
            await MakeRequestAsync(logUrl, toLogError, "post", signal.token);
        };
        sendError();
    }
    return modalData;
};

export default SetErrorData;