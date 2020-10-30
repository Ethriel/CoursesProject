import React, { useState } from 'react';
import AddCourseForm from './add-course-form';
import getFormattedDate from '../../helpers/get-formatted-date';
import Notification from '../common/Notification';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

const AddCourseComponent = ({...props}) => {
    const [loading, setLoading] = useState(false);
    const [reset, setReset] = useState(false);
    const submit = async values => {
        const { title, description, cover } = values;
        const course = {
            title: title,
            description: description,
            cover: cover
        };

        try {
            setLoading(true);

            const signal = axios.CancelToken.source();
            const response = await MakeRequestAsync("Courses/add", course, "post", signal.token);

            setReset(true);

            Notification(undefined, undefined, "Course was added!", true);
        } catch (error) {
            Notification(error);
        }
        finally{
            setLoading(false);
        }
    };

    return (
        <AddCourseForm onFinish={submit} loading={loading} reset={reset} />
    )
}

export default AddCourseComponent;