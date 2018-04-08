const initialState = {
    result: null
};

export default function confirm(state = initialState, action) {
    switch (action.type) {
        case 'CONFIRMATION_FAILED':
            return { ...state, result: { isSuccess: false, errors: action.errors } };
        case 'CONFIRMATION_SUCCESS':
            return { ...state, result: { isSuccess: true } };
        default:
            return state;
    }
};