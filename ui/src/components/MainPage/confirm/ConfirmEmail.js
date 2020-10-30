import React from 'react';
import H from '../../common/HAntD';
import { withRouter } from 'react-router-dom';
import { Spin, Space } from 'antd';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import axios from "axios";
import { connect } from 'react-redux';
import { USER, ADMIN } from '../../common/roles';
import { forbidden } from '../../../Routes/RoutersDirections';
import NotificationError from '../../common/notifications/notification-error';

const queryString = require('query-string');

class ConfirmEmail extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            confirmed: false,
            id: this.props.currentUser.id,
        }
    }

    componentDidMount = async () => {
        const role = this.props.currentUser.role;
        console.log(role);
        if (role !== USER && role !== ADMIN) {
            this.props.history.push(forbidden);
        }
        else {
            const location = this.props.location;
            const search = location.search;
            const parsed = queryString.parse(search);
            const token = parsed.token;
            console.log(token);
            try {
                const signal = axios.CancelToken.source();
                const requestData = {
                    id: this.state.id,
                    token: token
                };
                const response = await MakeRequestAsync("account/confirmEmail", requestData, "post", signal.token);
                console.log(response);
                if (response.status === 200) {
                    this.setState({
                        confirmed: true
                    });
                }
            } catch (error) {
                this.setCatch(error);
            }
        }
    };

    setCatch = (error) => {
        NotificationError(error);
    };
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

export default withRouter(connect(
    state => ({
        currentUser: state.userReducer
    })
)(ConfirmEmail));