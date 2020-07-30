import axios from 'axios';

const MakeRequestAsync = async (urlTail, info, method, cancelToken, parameters = null) => {
    const requestUrl = "https://localhost:44382/api/".concat(urlTail);
    axios.defaults.withCredentials = true;
    let axiosConfig = {};
    if (info === null && parameters !== null) {
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
        const response = await axios(axiosConfig);
        return response;
    } catch (error) {
        throw error;
    }
};

export default MakeRequestAsync;