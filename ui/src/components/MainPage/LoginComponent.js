import React, { Component } from 'react';
import { Redirect } from 'react-router-dom';
import NormalLoginFormAntD from './NormalLoginFormAntD';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import GetUserData from '../../helpers/GetUserData';
import axios from 'axios';
import setDataToLocalStorage from '../../helpers/setDataToLocalStorage';
import 'antd/dist/antd.css';
import { Spin, Space } from 'antd';
import '../../index.css';
import '../../css/styles.css';
import ModalWithMessage from '../common/ModalWithMessage';
import SetModalData from '../../helpers/SetModalData';
import GetFacebookData from './GetFacebookData';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import { connect } from 'react-redux';
import { SET_ROLE } from '../../reducers/reducersActions';
import { ADMIN } from '../common/roles';
import { courses, admin } from '../../Routes/RoutersDirections';

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
                modalOk: this.modalOk,
                modal: GetModalPresentation(this.modalOk, this.modalCancel)
            }
        };
    };

    componentWillUnmount() {
        this.signal.cancel();
    };
    
    setCatch = error => {
        const modalData = SetModalData(error);
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                message: modalData.message,
                errors: modalData.errors
            }
        }));
    };

    setFinally = () => {
        this.setState({ spin: false });
    };

    changeRole = role => {
        this.props.onRoleChange(role);
    };

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

            this.changeRole(role);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);

            this.setState({ redirect: true });
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    renderRedirect = () => {
        const role = localStorage.getItem("current_user_role");
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

            this.changeRole(role);

            setDataToLocalStorage(user.id, token, role, user.avatarPath, user.email);

            this.setState({ redirect: true });
        } catch (error) {
            this.setCatch(error);
        } finally {
            this.setFinally();
        }
    };

    modalOk = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    };

    modalCancel = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    };

    render() {
        const { spin, modal } = this.state;

        const modalWindow = ModalWithMessage(modal);

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
                {modal.visible === true && modalWindow}
            </>
        )
    }
}

export default connect(
    state => ({
        store: state
    }),
    dispatch => ({
        onRoleChange: (role) => {
            dispatch({
                type: SET_ROLE,
                payload: {
                    role: role
                }
            })
        }
    })
)(LoginComponent);