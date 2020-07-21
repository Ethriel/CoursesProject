import React, { useState, useEffect } from 'react';
import { Table } from 'antd';
import axios from 'axios';
import Container from '../common/ContainerComponent';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import getNestedCols from '../AdminComponent/getNestedCols';
import getNestedData from '../AdminComponent/getNestedData';
import H from '../common/HAntD';

const UserCoursesContainer = props => {
    const id = props.userId;
    const [isLoading, setLoading] = useState(true);
    const [courses, setCourses] = useState([]);
    useEffect(() => {
        const signal = axios.CancelToken.source();
        async function fetchCourses() {
            try {
                const response = await MakeRequestAsync(`UserCourses/get/${id}`, { msg: "hello" }, "get", signal.token);
                if (response.status === 200) {
                    const coursesData = response.data;
                    const data = [];
                    let obj = {};
                    for (let c of coursesData) {
                        obj = getNestedData(c);
                        data.push(obj);
                    };
                    setCourses(data);
                }
            } catch (error) {
                console.log(error);
            } finally {
                setLoading(false);
            }
        }

        fetchCourses();

        return function cleanup() {
            signal.cancel("CANCEL IN GET USER COURSES");
        }
    }, []);

    const containerClasses = ["display-flex", "width-90", "center-a-div", "col-flex"];
    const columns = getNestedCols();
    return (
        <Container classes={containerClasses}>
            <H level={4} myText="Your courses"/>
            <Table columns={columns} dataSource={courses} pagination={false} loading={isLoading} />
        </Container>
    );
}

export default UserCoursesContainer;