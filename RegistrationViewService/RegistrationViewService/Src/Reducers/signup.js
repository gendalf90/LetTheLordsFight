const initialState = {
    result: null
};

export default function signup(state = initialState, action) {
    switch (action.type) {
        case 'SIGNUP_FAILED':
            return { ...state, result: { isSuccess: false, errors: action.errors } }; 
        case 'SIGNUP_SUCCESS':
            return { ...state, result: { isSuccess: true, email: action.email } };
        default:
            return state;
    }
};