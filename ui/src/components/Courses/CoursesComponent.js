import React, { Component } from 'react';
import GridComponent from '../common/GridComponent';
import '../../css/styles.css';
import MapCards from '../../helpers/MapCards';
import PaginationComponent from '../common/PaginationComponent';
import MakeRequest from '../../helpers/MakeRequest';
import H from '../common/HAntD';
import ContainerComponent from '../common/ContainerComponent';
import LayoutAntD from '../common/LayoutAntD';

class CoursesComponent extends Component {

    constructor(props) {
        super(props);
        this.state = {
            items: [],
            amount: 0,
            skip: 0,
            page: 1,
            pageSize: 3
        };
    }

    async componentDidMount() {
        const amount = await MakeRequest("https://localhost:44382/courses/get/amount", { msg: "hello" }, "get");
        const skip = this.state.skip;
        const take = this.state.pageSize;
        const info = await MakeRequest(`https://localhost:44382/courses/get/forpage/${skip}/${take}`, { msg: "hello" }, "get");
        console.log(info);
        this.setState({
            amount: amount.amount,
            items: MapCards(info)
        });
    }

    handleChange = async (page, pageSize) => {
        const skip = (page * pageSize) - pageSize;
        const take = pageSize === 1 ? pageSize : page * pageSize;
        const info = await MakeRequest(`https://localhost:44382/courses/get/forpage/${skip}/${take}`, { msg: "hello" }, "get");
        this.setState({
            skip: skip,
            page: page,
            items: MapCards(info)
        });
    };

    render() {
        const classes = ["display-flex", "col-flex", "center-flex", "width-100", "height-100"];
        const header = <H myText="Select a training course" level={3} />;
        const grid = <GridComponent colNum={12} elementsToDisplay={this.state.items} />;
        const pagination = <PaginationComponent defaultCurrent={1} pageSize={this.state.pageSize} total={this.state.amount} onChange={this.handleChange} />
        const content = <ContainerComponent classes={classes}>{grid}{pagination}</ContainerComponent>
        const footer = <H myText="Footer" level={3} />;
        return (
            <LayoutAntD myHeader={header} myContent={content} myFooter={footer} />
        );
    };
}
export default CoursesComponent;