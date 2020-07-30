import axios from 'axios';

const MakeRequest = (urlTail, info, method, cancelToken, parameters = null) => {
    const requestUrl = "https://localhost:44382/api/".concat(urlTail);
    axios.defaults.withCredentials = true;
    let axiosConfig = {};
    if (info === null) {
        axiosConfig = {
            method: method,
            params: parameters,
            cancelToken: cancelToken,
            url: requestUrl,
            headers: {
                "Authorization": localStorage.getItem("bearer_header"),
                "Content-Type": "application/json",
                "Access-Control-Allow-Origin": "*"
            }
        };
    }
    else {
        axiosConfig = {
            method: method,
            data: info,
            cancelToken: cancelToken,
            url: requestUrl,
            headers: {
                "Authorization": localStorage.getItem("bearer_header"),
                "Content-Type": "application/json",
                "Access-Control-Allow-Origin": "*"
            }
        };
    }
    try {
        axios(axiosConfig)
            .then((response) => {
                const data = response.data;
                return data;
            })
            .catch((reason) => {
                return reason;
            })
    } catch (error) {
        throw error;
    }
};

export default MakeRequest;