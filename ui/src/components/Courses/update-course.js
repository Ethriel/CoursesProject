import React, { useEffect, useState } from 'react';
import getFormattedDate from '../../helpers/get-formatted-date';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import AddCourseForm from './add-course-form';
import axios from 'axios';
import Notification from '../common/Notification';

const UpdateCourse = (props) => {
    const [loading, setLoading] = useState(false);
    const [reset, setReset] = useState(false);
    const [course, setCourse] = useState({});
    const courseId = props.match.params.id;

    useEffect(() => {
        const signal = axios.CancelToken.source();
        const fetchCourse = async () => {
            try {
                setLoading(true);

                const response = await MakeRequestAsync(`courses/get/${courseId}`, { msg: "hello" }, "get", signal.token);
                const courseData = response.data.data;
                
                setCourse(courseData);
            } catch (error) {
                Notification(error);
            } finally {
                setLoading(false);
            }
        }

        fetchCourse();

        return function cleanUp() {
            signal.cancel("UPDATE COURSE: CANCEL IN FETCH COURSE");
        }
    }, [courseId])
    const submit = async values => {
        const { title, description, cover } = values;
        const startDate = getFormattedDate(values.startDate._d);
        const course = {
            title: title,
            description: description,
            cover: cover,
            startDate: startDate
        };

        try {
            setLoading(true);

            const signal = axios.CancelToken.source();
            const response = await MakeRequestAsync("Courses/update", course, "post", signal.token);

            setReset(true);

            Notification(undefined, undefined, "Course was updated!", true);
        } catch (error) {
            Notification(error);
        }
        finally {
            setLoading(false);
        }
    }
    return (
        <AddCourseForm onFinish={submit} loading={loading} reset={reset} course={course} />
    )
};

export default UpdateCourse;