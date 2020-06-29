import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Switch } from 'antd';

const SwitchAntD = (props) => {
    const checkedText = props.myCheckedText;
    const unCheckedText = props.myUnCheckedText;
    const onChange = props.myOnChange;
    const style = props.myStyle;
    const title = props.myTitle;

    return (
        <Switch
            checkedChildren={checkedText}
            unCheckedChildren={unCheckedText}
            defaultChecked
            onChange={onChange}
            style={style}
            title={title} />
    );
};

export default SwitchAntD;