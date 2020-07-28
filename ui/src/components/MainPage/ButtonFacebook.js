import React from 'react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';
import '../../css/styles.css';

const ButtonFaceBook = props => {
    const responseHandler = props.facebookResponse;
    return(
        <FacebookLogin
                    appId="327773058385961"
                    fields="first_name,last_name,email,picture"
                    callback={responseHandler}
                    render={
                        renderProps => (
                            <button
                                onClick={renderProps.onClick}
                                className="my-facebook">
                                Continue with Facebook
                            </button>
                        )
                    }/>
    );
};

export default ButtonFaceBook;