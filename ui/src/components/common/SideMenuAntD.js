import React from 'react';
import 'antd/dist/antd.css';
import '../../index.css';
import { Menu } from 'antd';
import { NavLink } from 'react-router-dom';


class SideMenuAntD extends React.Component {

    render() {
        const items = this.props.myMenuItems;

        return (
            <Menu style={{width: "150px", maxWidth: "100px"}}
                mode="inline">
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
};
export default SideMenuAntD;