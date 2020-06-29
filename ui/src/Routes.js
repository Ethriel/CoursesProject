import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainPage from './components/MainPage/MainPageComponent';
import CoursesComponent from './components/Courses/CoursesComponent';
import AboutUsComponent from './components/AboutUs/AboutUsComponent';
import AdminComponent from './components/AdminComponent/AdminComponent';

const Routes = () => (
    <Switch>
        <Route exact path="/" component={MainPage} />
        <Route exact path="/courses" component={CoursesComponent} />
        <Route exact path="/aboutus" component={AboutUsComponent} />
        <Route exact path="/admin" component={AdminComponent} />
    </Switch>
);
export default Routes;