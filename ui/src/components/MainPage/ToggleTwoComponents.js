import React, { Component } from 'react';
import '../../index.css';
import ContainerComponent from '../common/ContainerComponent';
import SwitchAntD from './SwitchAntD';
import H from '../common/HAntD';
import ButtonComponent from '../common/ButtonComponent';

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
        return text === "Register" ? "Log in" : "Register";;
    };

    handleFaceBookClick(event) {

    };

    render() {
        const { classes, firstComponent, secondComponent } = this.props;
        const { toSwitch } = this.state;
        const styles = { margin: '2%', maxWidth: 75 };
        const tip = toSwitch === true ? "Click to switch to register" : "Click to switch to login";
        const title = toSwitch === true ? "Log in now!" : "Register now!";
        const btnSize = "large";
        const btnText = "Facebook";
        const btnBgColor = "#3c5a99";

        return (
            <ContainerComponent classes={classes}>
                <H level={4} myText={title} />
                <SwitchAntD
                    myCheckedText="Register"
                    myUnCheckedText="Log in"
                    myOnChange={this.handleSwitch}
                    myStyle={styles}
                    myTitle={tip} />
                {toSwitch === true ? firstComponent : secondComponent}
                {
                    toSwitch && <ButtonComponent
                        mySize={btnSize}
                        myBgColor={btnBgColor}
                        myText={btnText}
                        myHandler={this.handleFaceBookClick} />
                }
            </ContainerComponent>
        );
    };
};

export default ToggleTwoComponents;