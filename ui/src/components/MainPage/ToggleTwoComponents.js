import React, { Component } from 'react';
import '../../index.css';
import ContainerComponent from '../common/ContainerComponent';
import SwitchAntD from './SwitchAntD';
import H from '../common/HAntD';

class ToggleTwoComponents extends Component {
    constructor(props) {
        super(props);
        this.state = {
            toSwitch: true,
        };
        this.handleSwitch = this.handleSwitch.bind(this);
    };

    handleSwitch(checked, event) {
        this.setState({
            toSwitch: !this.state.toSwitch,
        });
    };

    setText(text) {
        return text === "Sign up" ? "Sign in" : "Sign up";;
    };

    render() {
        const { classes, firstComponent, secondComponent } = this.props;
        const { toSwitch } = this.state;
        const className = "ant-switch-my";
        const tip = toSwitch === true ? "Switch to sign up" : "Switch to sign in";

        // const title = toSwitch === true ? "Sign in" : "Sign up";

        return (
            <ContainerComponent classes={classes}>
                {/* <H level={4} myText={title} /> */}
                <SwitchAntD
                    myCheckedText="Sign up"
                    myUnCheckedText="Sign in"
                    myOnChange={this.handleSwitch}
                    className={className}
                    myTitle={tip} />
                {toSwitch === true ? firstComponent : secondComponent}
            </ContainerComponent>
        );
    };
};

export default ToggleTwoComponents;