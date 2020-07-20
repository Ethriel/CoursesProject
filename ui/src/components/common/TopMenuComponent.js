import React from 'react'
import 'antd/dist/antd.css';
import '../../index.css';
import { Menu } from 'antd';
import { NavLink } from 'react-router-dom';

function TopMenuComponent (props){
    const items = props.myMenuItems;
    return(
        <Menu mode="horizontal">
                {
                    items.map((item) => {
                        return <Menu.Item
                            key={item.key}>
                            <NavLink to={item.to} />{item.text}
                        </Menu.Item>;
                    })
                }
            </Menu>
    );
};

export default TopMenuComponent;