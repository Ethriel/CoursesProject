import React, { useState } from 'react';
import AddCourseForm from './add-course-form';
import NotificationError from '../common/notifications/notification-error';
import NotificationOk from '../common/notifications/notification-ok';
import MakeRequestAsync from '../../helpers/MakeRequestAsync';
import axios from 'axios';

const AddCourseComponent = ({ ...props }) => {
    const [loading, setLoading] = useState(false);
    const [reset, setReset] = useState(false);

    const [selectedFile, setSelectedFile] = useState({});
    const [fileSelected, setFileSelected] = useState(false);
    const [imagePath, setImagePath] = useState(undefined);

    const onFileChange = event => {
        const file = event.target.files[0];
        setSelectedFile(file);
        setFileSelected(true);
    }

    const onFileUpload = async () => {
        const signal = axios.CancelToken.source();
        try {
            setLoading(true);

            const formData = new FormData();

            formData.set(
                "image",
                selectedFile,
                selectedFile.name
            );

            const response = await MakeRequestAsync(`courses/saveImage`, formData, "post", signal.token);
            const imagePath = response.data.data;
            console.log(imagePath);

            setImagePath(imagePath);
        } catch (error) {
            NotificationError(error);
        } finally {
            setLoading(false);
        }
    }
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
            await MakeRequestAsync("Courses/add", course, "post", signal.token);

            setReset(true);

            NotificationOk("Course was added!");
        } catch (error) {
            NotificationError(error);
        }
        finally {
            setLoading(false);
        }
    };

    return (
        <AddCourseForm
            onFinish={submit}
            loading={loading}
            reset={reset}
            onFileChange={onFileChange}
            onFileUpload={onFileUpload}
            imagePath={imagePath}
            fileSelected={fileSelected} />
    )
}

export default AddCourseComponent;