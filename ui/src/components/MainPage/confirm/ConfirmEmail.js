import React from 'react';
import H from '../../common/HAntD';
import { Spin, Space } from 'antd';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from "axios";
const queryString = require('query-string');

class ConfirmEmail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirmed: false,
            id: localStorage.getItem("current_user_id")
        }
    }

    componentDidMount = async () => {
        const location = this.props.location;
        const search = location.search;
        const parsed = queryString.parse(search);
        const token = parsed.token;
        console.log(parsed);
        try {
            const signal = axios.CancelToken.source();
            const requestData = {
                id: this.state.id,
                token: token
            };
            console.log(requestData);
            const response = await MakeRequestAsync("account/confirmEmail", requestData, "post", signal.token);
            console.log(response);
            if (response.status === 204) {
                this.setState({
                    confirmed: true
                });
            }
        } catch (error) {
            console.log(error)
        }
    }

    render() {
        const { confirmed } = this.state;
        const info = <H level={4} myText="Email confirmed" />;
        const spinner = <Space size="middle"> <Spin tip="Confirming your email..." size="large" /></Space>;
        return (
            <>
                {confirmed === false && spinner}
                {confirmed === true && info}
            </>
        );
    }
}

export default ConfirmEmail;