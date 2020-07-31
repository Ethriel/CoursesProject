import { notification } from 'antd';
import SetErorrData from '../../helpers/SetErrorData';
import GetNotificationDescription from '../../helpers/GetNotificationDescription';

const Notification = (error = undefined, warning = undefined, message = undefined, isInfo = false) => {
    if (error) {
        const errorData = SetErorrData(error);
        notification.error({
            message: errorData.message,
            description: GetNotificationDescription(errorData.errors)
        });
    }
    if (warning && message) {
        notification.warning({
            message: message,
            description: warning
        });
    }
    if (isInfo) {
        notification.info({
            message: "Information",
            description: message
        });
    }
};

export default Notification;