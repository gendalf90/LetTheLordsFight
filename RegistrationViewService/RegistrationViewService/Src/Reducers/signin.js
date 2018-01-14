const initialState = {
    show: false
};

export default function signin(state = initialState, action) {
    switch (action.type) {
        case 'SHOW_SIGNIN':
            return { ...state, show: true };
        case 'SHOW_SIGNUP':
            return { ...state, show: false };
        default:
            return state;
    }
};