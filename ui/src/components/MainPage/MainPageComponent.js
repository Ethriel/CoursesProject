import React from 'react';
import Toggle from './ToggleTwoComponents';
import Login from './LoginComponent';
import Reg from './RegistrationComponent';
import LayoutAntD from '../common/LayoutAntD';
import '../../index.css';
import AppHeader from './AppHeaderComponent';
import AppFooter from './AppFooterComponent';


function MainPageComponent() {
  const toggleClasses = ["display-flex", "col-flex", "center-flex", "width-100", "height-100"];
  const content = <Toggle classes={toggleClasses} firstComponent={<Login />} secondComponent={<Reg />} />;
  return (
    <LayoutAntD
      myHeader={<AppHeader />}
      myContent={content}
      myFooter={<AppFooter />}
    />
  );
};

export default MainPageComponent;
