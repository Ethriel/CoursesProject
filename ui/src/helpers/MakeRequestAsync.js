import axios from 'axios';

async function MakeRequestAsync(urlTail, info, method, cancelToken, params = null) {
    const ulr = "https://localhost:44382/api/".concat(urlTail);
    axios.defaults.withCredentials = true;
    let config = {};
    if (info === null && params !== null) {
        config = {
            method: method,
            params: params,
            cancelToken: cancelToken,
            url: ulr,
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
            url: ulr,
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
        console.log("CATCH in MAKE REQUEST");
        console.log(error.response);
        return error.response;
    }
};

export default MakeRequestAsync;