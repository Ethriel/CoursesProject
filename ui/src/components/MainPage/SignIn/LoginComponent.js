import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import { Spin, Space } from 'antd';
import { connect } from 'react-redux';
import axios from 'axios';
import 'antd/dist/antd.css';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import GetUserData from '../../../helpers/GetUserData';
import setDataToLocalStorage from '../../../helpers/setDataToLocalStorage';
import GetFacebookData from '../Facebook/GetFacebookData';
import Notification from '../../common/Notification';
import { SET_ROLE, SET_EMAIL_CONFIRMED, SET_EMAIL, SET_ID } from '../../../reducers/reducersActions';
import { ADMIN } from '../../common/roles';
import { courses, admin } from '../../../Routes/RoutersDirections';

class LoginComponent extends Component {
    signal = axios.CancelToken.source();
    constructor(props) {
        super(props);
        this.state = {
            redirect: false,
            spin: false,
            modal: {
                message: "",
                errors: [],
                visible: false,
            }
        };
    };

    componentWillUnmount() {
        this.signal.cancel();
    };

    setCatch = error => {
        Notification(error);
    };

    setFinally = () => {
        this.setState({ spin: false });
    };

    setEmailConfirmed = confirmed => {
        localStorage.setItem("current_user_email_confirmed", confirmed);
        this.props.onEmailConfirmedChanged(confirmed);
    }

    confirmHandler = async values => {
        this.setState({ spin: true });
        const userData = {
            email: values.username,
            password: values.password
        };

        try {
            const cancelToken = this.signal.token;

            const response = await MakeRequestAsync("account/signin", userData, "post", cancelToken);
            const data = response.data.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);

            this.props.onsetId(user.id);

            this.props.onRoleChange(role);

            this.props.onSetEmail(user.email);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email, false);

            await MakeRequestAsync("account/checkEmailConfirmed", { email: user.email }, "post", cancelToken);

            this.setEmailConfirmed(true);

            this.setState({ redirect: true });
        } catch (error) {
            const errorResponse = error.response;
            if (errorResponse) {
                const message = errorResponse.data.message;
                if (errorResponse.status === 400 && message.includes("Email is not confirmed")) {
                    this.setEmailConfirmed(false);
                }
            }
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    renderRedirect = () => {
        const role = this.props.currentUser.role;
        if (this.state.redirect) {
            const redirectDirection = role === ADMIN ? admin : courses;
            return <Redirect to={redirectDirection} push={true} />
        }
    };

    facebookCallback = async (response) => {
        this.setState({ spin: true });
        const cancelToken = this.signal.token;
        const userData = GetFacebookData(response);

        try {
            const reqResponse = await MakeRequestAsync("account/signin-facebook", userData, "post", cancelToken);

            const data = reqResponse.data.data;
            const token = data.token.key;
            const role = data.user.roleName;

            const user = GetUserData(data.user);

            this.props.onsetId(user.id);

            this.props.onRoleChange(role);

            this.props.onSetEmail(user.email);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email, true);

            this.setState({ redirect: true });
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    render() {
        const { spin } = this.state;
        const spinner = <Space size="middle"> <Spin tip="Signing you in..." size="large" /></Space>;
        const login =
            <>
                {this.state.redirect && this.renderRedirect()}
                {this.state.redirect === false && <NormalLoginFormAntD
                    myConfirHandler={this.confirmHandler}
                    facebookClick={this.facebookClick}
                    facebookResponse={this.facebookCallback} />}
            </>;

        return (
            <>
                {spin === true && spinner}
                {spin === false && login}
            </>
        )
    }
}

export default connect(
    state => ({
        currentUser: state.userReducer
    }),
    dispatch => ({
        onRoleChange: (role) => {
            dispatch({ type: SET_ROLE, payload: role })
        },
        onEmailConfirmedChanged: (emailConfirmed) => {
            dispatch({ type: SET_EMAIL_CONFIRMED, payload: emailConfirmed })
        },
        onSetEmail: (email) => {
            dispatch({ type: SET_EMAIL, payload: email })
        },
        onsetId: (id) => {
            dispatch({ type: SET_ID, payload: id })
        }
    })
)(LoginComponent);