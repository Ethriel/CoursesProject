import axios from 'axios';

function MakeRequest(url, info, method, params = null) {
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
        axios(config)
            .then((response) => {
                console.log("RESPONSE", response);
                const data = response.data;
                console.log("DATA", data);
                return data;
            })
            .catch((reason) => {
                return reason;
            })
    } catch (error) {
        return error;
    }
};

export default MakeRequest;