import React, { useEffect, useState } from 'react';
import getFormattedDate from '../../helpers/get-formatted-date';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import UpdateCourseForm from './update-course-form';
import axios from 'axios';
import Notification from '../common/Notification';

const UpdateCourse = (props) => {
    const [loading, setLoading] = useState(false);
    const [loaded, setLoaded] = useState(false);
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
                setLoaded(true);
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
        setLoaded(false);
        const { title, description, cover } = values;
        const courseData = {
            id: course.id,
            title: title,
            description: description,
            cover: cover
        };

        try {
            setLoading(true);
            
            const signal = axios.CancelToken.source();
            const response = await MakeRequestAsync("Courses/update", courseData, "post", signal.token);
            setCourse(courseData);
            setLoaded(true);

            Notification(undefined, undefined, "Course was updated!", true);
        } catch (error) {
            Notification(error);
        }
        finally {
            setLoading(false);
        }
    }
    return (
        <>
            { loaded === true && <UpdateCourseForm onFinish={submit} loading={loading} reset={reset} course={course} />}
        </>

    )
};

export default UpdateCourse;