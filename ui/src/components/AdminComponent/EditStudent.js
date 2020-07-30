import React, { useState, useEffect } from 'react';
import { withRouter } from "react-router";
import { useStore } from 'react-redux';
import axios from 'axios';
import UserProfileComponent from '../userProfile/UserProfileComponent';
import { ADMIN } from '../common/roles';
import { forbidden } from '../../Routes/RoutersDirections';

const EditStudent = (props) => {
    const [isLoading, setIsLoading] = useState(true);
    const [userId, setUserId] = useState(-1);
    const store = useStore().getState();
    useEffect(() => {
        const userRole = store.userRoleReducer.role;
        if (userRole !== ADMIN) {
            props.history.push(forbidden);
        }
        const signal = axios.CancelToken.source();
        const id = props.match.params.id;
        setUserId(id);
        setIsLoading(false);
        return function cleanUp() {
            signal.cancel("CANCEL IN EDIT STUDENT");
        }
    }, [props.match.params.id])

    return (
        <>
            {
                isLoading === false && <UserProfileComponent userId={userId} />
            }
        </>
    );
};

export default withRouter(EditStudent);