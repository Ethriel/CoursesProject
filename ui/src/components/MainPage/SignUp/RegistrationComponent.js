import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import { Spin, Space } from 'antd';
import { connect } from 'react-redux';
import axios from 'axios';
import 'antd/dist/antd.css';
import RegistrationForm from './RegistrationFormAntD';
import MakeRequestAsync from '../../../helpers/MakeRequestAsync';
import GetUserData from '../../../helpers/GetUserData';
import setDataToLocalStorage from '../../../helpers/setDataToLocalStorage';
import GetFacebookData from '../Facebook/GetFacebookData';
import Notification from '../../common/Notification';
import { SET_ROLE, SET_EMAIL_CONFIRMED, SET_EMAIL, SET_ID } from '../../../reducers/reducersActions';
import { courses } from '../../../Routes/RoutersDirections';

class RegistrationComponent extends Component {
    signal = axios.CancelToken.source();
    constructor(props) {
        super(props);
        this.state = {
            spin: false,
            redirect: false
        };
    }
    
    componentWillUnmount() {
        this.signal.cancel();
    };

    setCatch = error => {
        Notification(error);
    };

    setFinally = () => {
        this.setState({ spin: false });
    }

    renderRedirect = () => {
        return (
            <Redirect to={courses} push={true} />
        )
    };

    confirmHandler = async values => {
        this.setState({ spin: true });

        const userData = {
            firstName: values.user.name,
            lastName: values.user.lastname,
            birthDate: values.birthdate._i,
            email: values.email,
            password: values.password
        };

        try {
            const cancelToken = this.signal.token;

            const response = await MakeRequestAsync("account/signup", userData, "post", cancelToken);

            const data = response.data.data;
            const token = data.token.key;
            const role = data.user.roleName;
            const user = GetUserData(data.user);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email, false);

            this.props.onsetId(user.id);

            this.props.onRoleChange(role);

            this.props.onSetEmail(user.email);


            this.props.onEmailConfirmedChanged(false);

            Notification(undefined, undefined, "A confirm message was sent to your email. Follow the instructions", true);

            this.setState({ redirect: true });
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    }

    facebookCallback = async (response) => {
        this.setState({ spin: true });

        const cancelToken = this.signal.token

        const facebookUserData = GetFacebookData(response);

        try {
            const reqResponse = await MakeRequestAsync("account/signin-facebook", facebookUserData, "post", cancelToken);

            const data = reqResponse.data.data;
            const token = data.token.key;
            const role = data.user.roleName;

            const user = GetUserData(data.user);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email, true);

            this.props.onRoleChange(role);

            this.props.onEmailConfirmedChanged(true);

            Notification(undefined, undefined, "You can now use your Facebook account to enter the system", true);

            this.setState({ redirect: true });
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    render() {
        const { spin, redirect } = this.state;
        const spinner = <Space size="middle"> <Spin tip="Signing you up..." size="large" /></Space>;
        const signUp =
            <RegistrationForm onFinish={this.confirmHandler}
                facebookResponse={this.facebookCallback} />;

        return (
            <>
                {spin === true && spinner}
                {spin === false && signUp}
                {redirect && this.renderRedirect()}
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
)(RegistrationComponent);