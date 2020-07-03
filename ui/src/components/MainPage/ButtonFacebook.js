import React from 'react';
import ButtonComponent from '../common/ButtonComponent';

const ButtonFaceBook = props => {
    const btnSize = "large";
    const btnText = "Facebook";
    const btnBgColor = "#3c5a99";
    const handler = props.clickHandler;

    return(
        <ButtonComponent
                        mySize={btnSize}
                        myBgColor={btnBgColor}
                        myText={btnText}
                        myHandler={handler} />
    );
};

export default ButtonFaceBook;