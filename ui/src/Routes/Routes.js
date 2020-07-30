import React from 'react';
import { Route, Switch } from 'react-router-dom';

import {
    main, courses, coursesDetails, aboutUs, admin,
    editStudent, forbidden, userProfile, forgotPassword,
    confirmEmail, confirmChangeEmail, resetPassword,
    confirmEmailForm
} from './RoutersDirections';

import {
    MainPage, Courses, AboutUs, Admin, CourseDetails,
    UserProfile, ConfirmEmail, ConfirmEmailForm, ConfirmChangeEmail,
    ForgotPassword, ResetPassword, EditStudent,
    NoAccessPage
} from './RouteComponents';

const Routes = () => (
    <Switch>
        <Route exact path={main} component={MainPage} />
        <Route exact path={forbidden} component={NoAccessPage} />
        <Route exact path={courses} component={Courses} />
        <Route path={coursesDetails} component={CourseDetails} />
        <Route exact path={aboutUs} component={AboutUs} />
        <Route exact path={admin} component={Admin} />
        <Route path={editStudent} component={EditStudent} />
        <Route exact path={userProfile} component={UserProfile} />
        <Route exact path={forgotPassword} component={ForgotPassword} />
        <Route path={confirmEmail} component={ConfirmEmail} />
        <Route path={confirmEmailForm} component={ConfirmEmailForm} />
        <Route path={confirmChangeEmail} component={ConfirmChangeEmail} />
        <Route path={resetPassword} component={ResetPassword} />
    </Switch>
);
export default Routes;