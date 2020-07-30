import React from 'react';
import 'antd/dist/antd.css';
import { Switch } from 'antd';

const SwitchAntD = (props) => {
    const checkedText = props.myCheckedText;
    const unCheckedText = props.myUnCheckedText;
    const onChange = props.myOnChange;
    const title = props.myTitle;
    const className = props.className;

    return (
        <Switch
            checkedChildren={checkedText}
            unCheckedChildren={unCheckedText}
            defaultChecked
            onChange={onChange}
            className={className}
            title={title} />
    );
};

export default SwitchAntD;