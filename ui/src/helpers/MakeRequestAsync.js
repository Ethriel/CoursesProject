import axios from 'axios';

async function MakeRequestAsync(url, info, method, cancelToken, params = null) {
    axios.defaults.withCredentials = true;
    let config = {};
    if (info === null && params !== null) {
        config = {
            method: method,
            params: params,
            cancelToken: cancelToken,
            url: url,
            headers: {
                "Authorization": localStorage.getItem("bearer_header"),
                "Content-Type": "application/json",
                "Access-Control-Allow-Origin": "*"
            }
        };
    }
    else {
        config = {
            method: method,
            data: info,
            cancelToken: cancelToken,
            url: url,
            headers: {
                "Authorization": localStorage.getItem("bearer_header"),
                "Content-Type": "application/json",
                "Access-Control-Allow-Origin": "*"
            }
        };
    }
    try {
        const response = await axios(config);
        return response;
    } catch (error) {
        throw error;
    }
};

export default MakeRequestAsync;