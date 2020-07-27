import React from 'react';
import { Route, Switch } from 'react-router-dom';
import MainPage from './components/MainPage/MainPageComponent';
import CoursesComponent from './components/Courses/CoursesComponent';
import AboutUsComponent from './components/AboutUs/AboutUsComponent';
import AdminComponent from './components/AdminComponent/AdminComponent';
import NotAdminPage from './components/AdminComponent/NotAdminPage';
import CourseDetails from './components/Courses/CourseDetailsComponent';
import UserProfileComponent from './components/userProfile/UserProfileComponent';
import ConfirmEmail from './components/MainPage/confirm/ConfirmEmail';
import ConfirmChangeEmail from './components/MainPage/confirm/ConfirmChangeEmail';
import ForgotPassword from './components/MainPage/ForgotPassword';
import ResetPassword from './components/MainPage/ResetPassword';
import EditStudent from './components/AdminComponent/EditStudent';

const Routes = () => (
    <Switch>
        <Route exact path="/" component={MainPage} />
        <Route exact path="/courses" component={CoursesComponent} />
        <Route path="/coursedetails/:id" component={CourseDetails} />
        <Route exact path="/aboutus" component={AboutUsComponent} />
        <Route exact path="/admin" component={AdminComponent} />
        <Route exact path="/admin/editStudent:id" component={EditStudent} />
        <Route exact path="/admin/notAdmin" component={NotAdminPage} />
        <Route exact path="/profile" component={UserProfileComponent} />
        <Route exact path="/forgotPassword" component={ForgotPassword} />
        <Route path="/confirmEmail" component={ConfirmEmail} />
        <Route path="/confirmChangeEmail" component={ConfirmChangeEmail} />
        <Route path="/resetPassword" component={ResetPassword} />
    </Switch>
);
export default Routes;