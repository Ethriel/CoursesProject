import React, { Component } from 'react';
import ConvertArray from '../../helpers/ConvertArray';

class ContainerComponent extends Component {
    render(){
        const classes = ConvertArray(this.props.classes);
        return (
            <div className={classes}>
                {this.props.children}
            </div>
        )
    }
}

export default ContainerComponent;