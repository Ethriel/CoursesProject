import React from 'react';
import Toggle from './ToggleTwoComponents';
import Login from './LoginComponent';
import Reg from './RegistrationComponent';
import '../../index.css';

function MainPageComponent() {
  const toggleClasses = ["display-flex", "col-flex", "align-center", "width-100", "height-100"];
  
  return (
    <Toggle classes={toggleClasses} firstComponent={<Login />} secondComponent={<Reg />} />
  );
};

export default MainPageComponent;
