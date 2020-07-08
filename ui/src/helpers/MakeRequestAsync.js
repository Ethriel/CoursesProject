import axios from 'axios';

async function MakeRequestAsync(url, info, method, params = null) {
    axios.defaults.withCredentials = true;
    let config = {};
    if (info === null) {
        config = {
            method: method,
            params: params,
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
        const data = response.data;
        return data;
    } catch (error) {
        return error;
    }
};

export default MakeRequestAsync;