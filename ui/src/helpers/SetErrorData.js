import MakeRequestAsync from './MakeRequestAsync';
import axios from 'axios';

const logUrl = "Error/logJavascriptError";

const SetErrorData = (error) => {
    let errorData = {
        message: "",
        errors: []
    };
    if (error.isAxiosError) {
        if (error.response) {
            const status = error.response.status;
            if (status === 500) {
                errorData.message = "Server error occured";
                errorData.errors.push("Internal server error");
            }
            else if (status === 401) {
                errorData.message = "No access";
                errorData.errors.push("Unauthorised");
            }
            else if (status === 403) {
                errorData.message = "No access";
                errorData.errors.push("Forbidden")
            }
            else if (status === 404) {
                errorData.message = "Not found";
                errorData.errors.push("Resourse not found")
            }
            else {
                errorData.message = error.message;
                errorData.errors = error.response.data.errors;
            }

        }
        else {
            errorData.message = "A network error occured";
            errorData.errors.push("Connection refused. API server is offline");
            errorData.errors.push("Or check your internet connection");
        }
    }
    else {
        errorData.message = "Script error";
        errorData.errors.push("Contact the administrator if needed");

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
    return errorData;
};

export default SetErrorData;