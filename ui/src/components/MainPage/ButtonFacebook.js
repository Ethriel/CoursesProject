import React from 'react';
import FacebookLogin from 'react-facebook-login/dist/facebook-login-render-props';
import '../../css/styles.css';

const ButtonFaceBook = props => {
    const responseHandler = props.facebookResponse;
    const onClick = props.facebookClick;

    return(
        <FacebookLogin
                    appId="327773058385961"
                    fields="name,email,picture"
                    callback={responseHandler}
                    render={
                        renderProps => (
                            <button
                                onClick={onClick}
                                className="my-facebook">
                                Continue with facebook
                            </button>
                        )
                    }
                    size="small"/>
    );
};

export default ButtonFaceBook;