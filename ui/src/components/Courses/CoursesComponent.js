import React, { Component } from 'react';
import '../../css/styles.css';
import MapCards from '../../helpers/MapCards';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import H from '../common/HAntD';
import LayoutAntD from '../common/LayoutAntD';
import { Redirect } from 'react-router-dom';
import axios from 'axios';
import GetModalPresentation from '../../helpers/GetModalPresentation';
import ModalWithMessage from '../common/ModalWithMessage';
import SetModalData from '../../helpers/SetModalData';
import CoursesContent from './CoursesContent';
import { Spin, Space } from 'antd';
import { withRouter } from "react-router";
import { connect } from 'react-redux';
import { GET_ROLE } from '../../reducers/reducersActions';
import { ADMIN, USER } from '../common/roles';
import { forbidden } from '../../Routes/RoutersDirections';

class CoursesComponent extends Component {

    constructor(props) {
        super(props);
        this.state = {
            items: [],
            pagination: null,
            redirect: false,
            loading: true,
            course: 0,
            modal: GetModalPresentation(this.modalOk, this.modalCancel)
        };
    }

    async componentDidMount() {
        const role = this.props.store.userRoleReducer.role;
        if (role !== USER && role !== ADMIN) {
            this.props.history.push(forbidden);
        }
        else {
            const signal = axios.CancelToken.source();
            const { pagination } = this.state;
            const coursesPagination = {
                pagination: pagination
            };
            try {
                const response = await MakeRequestAsync("courses/get/paged", coursesPagination, "post", signal.token);
                const data = response.data.data;
                const courses = data.courses;
                const newPagination = data.pagination;
                this.setState({ pagination: newPagination });
                this.setState({ items: MapCards(courses, this.handleCourseClick) });
                this.setState({ loading: false });
            } catch (error) {
                this.setCatch(error);
            }
        }
    }

    handleChange = async (page, pageSize) => {

        const signal = axios.CancelToken.source()
        this.setState({ loading: true });
        const pagination = {
            position: this.state.position,
            page: page,
            pageSize: pageSize,
            total: this.state.total
        };

        const coursesPagination = {
            pagination: pagination
        };

        this.setState(old => ({
            pagination: {
                ...old.pagination,
                page: page,
                pageSize: pageSize
            }
        }));

        try {
            const response = await MakeRequestAsync("courses/get/paged", coursesPagination, "post", signal.token);
            const data = response.data.data;
            const courses = data.courses;
            this.setState({ items: MapCards(courses, this.handleCourseClick) });
            this.setState({ loading: false });
        } catch (error) {
            this.setCatch(error);
        }
    };

    handleCourseClick = event => {
        const id = +event.currentTarget.getAttribute("cardid");
        this.setState({
            redirect: true,
            course: id
        });
    };

    handleRedirect = () => {
        if (this.state.redirect) {
            const id = this.state.course;
            return <Redirect to={`/coursedetails/${id}`} push={true} />
        }
    }

    setCatch(error) {
        const modalData = SetModalData(error);
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                message: modalData.message,
                errors: modalData.errors,
                visible: true
            }
        }));
    }

    modalOk = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    }

    modalCancel = (e) => {
        this.setState(oldState => ({
            modal: {
                ...oldState.modal,
                visible: false
            }
        }));
    }
    render() {
        const header = <H myText="Select a training course" level={4} />;
        const { pagination, loading, items, modal } = this.state;
        const modalWindow = ModalWithMessage(modal);
        const spinner = <Space size="middle"> <Spin tip="Loading courses..." size="large" /></Space>;

        const content = <CoursesContent
            pagination={pagination}
            handleChange={this.handleChange}
            items={items} />

        const toRender =
            <>
                {this.state.redirect && this.handleRedirect()}
                {this.state.redirect === false && <LayoutAntD myHeader={header} myContent={content} />}
            </>;

        return (
            <>
                {loading === true && spinner}
                {loading === false && toRender}
                {this.state.modal.visible === true && modalWindow}
            </>
        );
    };
}
export default withRouter(connect(
    state => ({
        store: state
    }),
    dispatch => ({
        onGetRole: () => {
            dispatch({
                type: GET_ROLE
            })
        }
    })
)(CoursesComponent));