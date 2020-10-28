import React from 'react';
import AddCourseForm from './add-course-form';
import getFormattedDate from '../../helpers/get-formatted-date';


const AddCourseComponent = (props) => {

    const submit = values => {
        const { title, description, cover } = values;
        const startDate = getFormattedDate(values.startDate._d);
        const obj = {
            title: title,
            description: description,
            cover: cover,
            startDate: startDate
        };

        console.log(obj);
    };

    return (
        <AddCourseForm onFinish={submit} />
    )
}

export default AddCourseComponent;