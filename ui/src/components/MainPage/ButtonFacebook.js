import React from 'react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';
import '../../css/styles.css';

const ButtonFaceBook = props => {
    const responseHandler = props.facebookResponse;
    return (
        <FacebookLogin
            appId="327773058385961"
            htmlType="button"
            fields="first_name,last_name,email,picture"
            callback={responseHandler}
            render={
                renderProps => (
                    <>
                        <div
                            onClick={renderProps.onClick}
                            className="my-facebook">
                            Continue with Facebook
                        </div>
                    </>
                )
            } />
    );
};

export default ButtonFaceBook;