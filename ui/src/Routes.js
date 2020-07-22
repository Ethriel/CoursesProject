import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainPage from './components/MainPage/MainPageComponent';
import CoursesComponent from './components/Courses/CoursesComponent';
import AboutUsComponent from './components/AboutUs/AboutUsComponent';
import AdminComponent from './components/AdminComponent/AdminComponent';
import CourseDetails from './components/Courses/CourseDetailsComponent';
import UserProfileComponent from './components/userProfile/UserProfileComponent';
import ConfirmEmail from './components/MainPage/confirm/ConfirmEmail';

const Routes = () => (
    <Switch>
        <Route exact path="/" component={MainPage} />
        <Route exact path="/courses" component={CoursesComponent} />
        <Route path="/coursedetails/:id" component={CourseDetails} />
        <Route exact path="/aboutus" component={AboutUsComponent} />
        <Route exact path="/admin" component={AdminComponent} />
        <Route exact path="/profile" component={UserProfileComponent} />
        <Route exact path="/confirmEmail" component={ConfirmEmail} />
    </Switch>
);
export default Routes;