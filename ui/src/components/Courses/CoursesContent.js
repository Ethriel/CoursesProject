import React from 'react';
import ContainerComponent from '../common/ContainerComponent';
import GridComponent from '../common/GridComponent';
import PaginationComponent from '../common/PaginationComponent';

const CoursesContent = props => {
    const pagination = props.pagination;
    const handleChange = props.handleChange;
    const items = props.items;

    const classes = ["display-flex", "col-flex", "center-flex", "width-100", "height-100"];

    const grid = <GridComponent colNum={12} elementsToDisplay={items} />;
    const paginationComponent = <PaginationComponent
        pagination={pagination}
        onChange={handleChange} />

    return (
        <ContainerComponent classes={classes}>
            {grid}
            {paginationComponent}
        </ContainerComponent>
    );
};

export default CoursesContent;