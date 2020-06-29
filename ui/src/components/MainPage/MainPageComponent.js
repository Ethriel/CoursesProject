import React from 'react';
import Toggle from './ToggleTwoComponents';
import Login from './LoginComponent';
import Reg from './RegistrationComponent';
import LayoutAntD from './LayoutAntD';
import '../../index.css';


function MainPageComponent() {
  const toggleClasses = ["display-flex", "col-flex", "center-flex", "width-100", "height-100"];
  const content = <Toggle classes={toggleClasses} firstComponent={<Login />} secondComponent={<Reg />} />;

  return (
    <LayoutAntD
      myContent={content}
    />
  );
};

export default MainPageComponent;
