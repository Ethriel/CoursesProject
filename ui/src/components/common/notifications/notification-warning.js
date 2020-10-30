import { notification } from 'antd';

const NotificationWarning = (warning, message) => {
    notification.warning({
        message: warning,
        description: message
    });
};

export default NotificationWarning;