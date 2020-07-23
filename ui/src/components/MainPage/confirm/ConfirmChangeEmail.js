import React from 'react';
import H from '../../common/HAntD';
import { Spin, Space } from 'antd';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from "axios";
const queryString = require('query-string');

class ConfirmChangeEmail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirmed: false,
            id: localStorage.getItem("current_user_id"),
            email: localStorage.getItem("new_email")
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
                token: token,
                email: this.state.email
            };
            console.log(requestData);
            const response = await MakeRequestAsync("account/confirmChangeEmail", requestData, "post", signal.token);
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

export default ConfirmChangeEmail;