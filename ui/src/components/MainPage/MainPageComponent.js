import React from 'react';
import Toggle from './ToggleTwoComponents';
import Login from './LoginComponent';
import Reg from './RegistrationComponent';
import SwitchUserActions from './SwitchUserActions';
import '../../index.css';

const MainPageComponent = () => {
  const toggleClasses = ["display-flex", "col-flex", "align-center", "width-100", "height-100", "justify-center"];

  return (
    // <Toggle classes={toggleClasses} firstComponent={<Login />} secondComponent={<Reg />} />
    <SwitchUserActions />
  );
};

export default MainPageComponent;
