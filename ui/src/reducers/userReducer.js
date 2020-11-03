import { SET_EMAIL, SET_ROLE, SET_ID, SET_EMAIL_CONFIRMED, SET_AVATAR } from './reducersActions';

const localRole = localStorage.getItem("current_user_role");
const localId = localStorage.getItem("current_user_id");
const localEmail = localStorage.getItem("current_user_email");
const localEmailConfirmed = localStorage.getItem("current_user_email_confirmed");
const localUserAvatar = localStorage.getItem("user_avatar") === 'null' ? null : localStorage.getItem("user_avatar");

const initialState = {
    id: localId,
    email: localEmail,
    role: localRole,
    emailConfirmed: localEmailConfirmed,
    userAvatar: localUserAvatar
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

    if (action.type === SET_AVATAR) {
        return {
            ...state,
            userAvatar: action.payload
        }
    }
    return state;
};

export default userReducer;