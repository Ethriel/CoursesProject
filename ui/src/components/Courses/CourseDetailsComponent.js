import React, { Component } from 'react';
import MakeRequest from '../../helpers/MakeRequest';
import Container from '../common/ContainerComponent';
import H from '../common/HAntD';
import { Typography } from 'antd';

const { Paragraph } = Typography;

class CourseDetailsComponent extends Component {
    constructor(props) {
        super(props)
        this.state = {
            id: props.match.params.id,
            course: null,
            isLoading: true,
            plug: { a: "", b: "", c: "" }
        }
    }

    async componentDidMount() {
        const gottedCourse = await MakeRequest(`https://localhost:44382/courses/get/${this.state.id}`, { msg: "hello" }, "get");
        this.setState({
            course: gottedCourse,
            isLoading: false
        });
    }

    render() {
        const classes = ["display-flex", "col-flex", "center-flex", "width-75", "height-100", "center-a-div"];
        const { title, cover, description } = this.state.isLoading ? this.state.plug : this.state.course;
        return (
            <>
                {
                    !this.state.isLoading &&
                    <Container classes={classes}>
                        <H myText={title} level={2} />
                        <img 
                            style={{ margin: "0 auto" }}
                            alt="No"
                            src={`https://localhost:44382/${cover}`} />
                        <Paragraph>
                            {description}
                        </Paragraph>
                    </Container>
                }
            </>

        );
    };
};

export default CourseDetailsComponent;