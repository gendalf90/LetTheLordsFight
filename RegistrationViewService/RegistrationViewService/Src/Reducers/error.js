const initialState = {
    show: false,
    description: ''
};

export default function error(state = initialState, action) {
    switch (action.type) {
        case 'SHOW_SIGNIN':
        case 'SHOW_SIGNUP':
        case 'HIDE_ERROR':
            return { ...state, show: false };
        case 'SHOW_ERROR':
            return { ...state, show: true, description: action.description };
        default:
            return state;
    }
};