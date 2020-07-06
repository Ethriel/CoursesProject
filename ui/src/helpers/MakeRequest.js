import axios from 'axios';

async function MakeRequest(url, info, method) {
    axios.defaults.withCredentials = true;
    const config = {
        method: method,
        data: info,
        url: url,
        headers: {
            "Authorization": localStorage.getItem("bearer_header"),
            "Content-Type": "application/json",
            "Access-Control-Allow-Origin": "*"
        }
    };
    try {
        const response = await axios(config);
        const data = response.data;
        return data;
    } catch (error) {
        return error;
    }
};

export default MakeRequest;