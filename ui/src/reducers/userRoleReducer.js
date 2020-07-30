import { SET_ROLE } from './reducersActions';

const localRole = localStorage.getItem("current_user_role");
const initialState =
    { role: localRole };

const userRoleReducer = (state = initialState, action) => {
    let newState = state;
    if (action.type === SET_ROLE) {
        newState = {
            ...state,
            role: action.payload.role
        }
    }
    return newState;
}

export default userRoleReducer;