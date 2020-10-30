import React, { useEffect, useState } from 'react';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import UpdateCourseForm from './update-course-form';
import axios from 'axios';
import NotificationError from '../common/notifications/notification-error';
import NotificationOk from '../common/notifications/notification-ok';

const UpdateCourse = (props) => {
    const [loading, setLoading] = useState(false);
    const [loaded, setLoaded] = useState(false);
    const [course, setCourse] = useState({});
    const courseId = props.match.params.id;

    const setCatch = error => {
        NotificationError(error);
    }

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
                setCatch(error);
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
            await MakeRequestAsync("Courses/update", courseData, "post", signal.token);

            setCourse(courseData);
            setLoaded(true);
            
            NotificationOk("Course was updated!");
        } catch (error) {
            setCatch(error);
        }
        finally {
            setLoading(false);
        }
    }
    return (
        <>
            { loaded === true && <UpdateCourseForm onFinish={submit} loading={loading} course={course} />}
        </>

    )
};

export default UpdateCourse;