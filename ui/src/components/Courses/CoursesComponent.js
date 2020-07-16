import React, { Component } from 'react';
import GridComponent from '../common/GridComponent';
import '../../css/styles.css';
import MapCards from '../../helpers/MapCards';
import PaginationComponent from '../common/PaginationComponent';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import H from '../common/HAntD';
import ContainerComponent from '../common/ContainerComponent';
import LayoutAntD from '../common/LayoutAntD';
import { Redirect } from 'react-router-dom';
import axios from 'axios';

class CoursesComponent extends Component {

    constructor(props) {
        super(props);
        this.state = {
            items: [],
            amount: 0,
            skip: 0,
            page: 1,
            pageSize: 3,
            redirect: false,
            course: 0,
            signal: axios.CancelToken.source()
        };
    }

    async componentDidMount() {
        const cancelToken = this.state.signal.token;
        const responseA = await MakeRequestAsync("courses/get/amount", { msg: "hello" }, "get", cancelToken);
        const amount = responseA.data.amount;
        const skip = this.state.skip;
        const take = this.state.pageSize;
        const responseTake = await MakeRequestAsync(`courses/get/forpage/${skip}/${take}`, { msg: "hello" }, "get", cancelToken);
        const info = responseTake.data.courses;
        this.setState({
            amount: amount.amount,
            items: MapCards(info, this.handleCourseClick)
        });
    }

    handleChange = async (page, pageSize) => {
        const cancelToken = this.state.signal.token;
        const skip = (page * pageSize) - pageSize;
        const take = pageSize === 1 ? pageSize : page * pageSize;
        const response = await MakeRequestAsync(`courses/get/forpage/${skip}/${take}`, { msg: "hello" }, "get", cancelToken);
        const info = response.data;
        this.setState({
            skip: skip,
            page: page,
            items: MapCards(info, this.handleCourseClick)
        });
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

    render() {
        const classes = ["display-flex", "col-flex", "center-flex", "width-100", "height-100"];
        const header = <H myText="Select a training course" level={3} />;
        const grid = <GridComponent colNum={12} elementsToDisplay={this.state.items} />;
        const pagination = <PaginationComponent defaultCurrent={1} pageSize={this.state.pageSize} total={this.state.amount} onChange={this.handleChange} />
        const content = <ContainerComponent classes={classes}>{grid}{pagination}</ContainerComponent>
        const footer = <H myText="Footer" level={3} />;
        return (
            <>
                {this.state.redirect && this.handleRedirect()}
                {this.state.redirect === false && <LayoutAntD myHeader={header} myContent={content} myFooter={footer} />}
            </>
        );
    };
}
export default CoursesComponent;