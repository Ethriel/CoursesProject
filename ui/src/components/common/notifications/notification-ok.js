import { notification } from 'antd';

const NotificationOk = message => {
    notification.info({
        message: "Information",
        description: message
    });
};

export default NotificationOk;