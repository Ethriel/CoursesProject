import { SET_EMAIL, SET_ROLE, SET_ID, SET_EMAIL_CONFIRMED } from './reducersActions';

const localRole = localStorage.getItem("current_user_role");
const localId = localStorage.getItem("current_user_id");
const localEmail = localStorage.getItem("current_user_email");
const localEmailConfirmed = localStorage.getItem("current_user_email_confirmed");

const initialState = {
    id: localId,
    email: localEmail,
    role: localRole,
    emailConfirmed: localEmailConfirmed
};

const userReducer = (state = initialState, action) => {
    if (action.type === SET_ID) {
        return {
            ...state,
            id: action.payload
        };
    }

    if (action.type === SET_EMAIL) {
        return {
            ...state,
            email: action.payload
        };
    }

    if (action.type === SET_ROLE) {
        return {
            ...state,
            role: action.payload
        };
    }

    if (action.type === SET_EMAIL_CONFIRMED) {
        return {
            ...state,
            emailConfirmed: action.payload
        }
    }
    return state;
};

export default userReducer;