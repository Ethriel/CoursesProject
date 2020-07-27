import {SET_ROLE, GET_ROLE} from './reducersActions';

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
    } else if(action.type === GET_ROLE){
        return newState;
    }
    return newState;
}

export default userRoleReducer;