import { notification } from 'antd';
import SetErorrData from '../../../helpers/SetErrorData';
import GetNotificationDescription from '../../../helpers/GetNotificationDescription';

const NotificationError = (error) => {
    const errorData = SetErorrData(error);
        const description = GetNotificationDescription(errorData.errors);
        notification.error({
            message: errorData.message,
            description: description
        });
};

export default NotificationError;