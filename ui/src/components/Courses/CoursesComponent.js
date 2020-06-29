import React, { Component } from 'react';
import GridComponent from '../common/GridComponent';
import '../../css/styles.css';
import MapCards from '../../helpers/MapCards';
import PaginationComponent from '../common/PaginationComponent';

class CoursesComponent extends Component {

    constructor(props) {
        super(props);
        const cards = MapCards(8);
        this.state = {
            items: cards.slice(0, 2),
            length: cards.length,
            quantity: 8
        };
    }

    handleChange = (page, pageSize) => {
        let start = (page * pageSize) - pageSize;
        let end = page * pageSize;
        let cards = MapCards(this.state.quantity);
        this.setState({
            items: cards.slice(start, end)
        });
    };

    render() {
        return (
            <>
                <GridComponent colNum={12} elementsToDisplay={this.state.items} />
                <PaginationComponent defaultCurrent={1} pageSize={2} total={this.state.length} onChange={this.handleChange} />
            </>
        );
    };
}
export default CoursesComponent;