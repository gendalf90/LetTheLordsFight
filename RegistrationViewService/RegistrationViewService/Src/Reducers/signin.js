﻿const initialState = {
    result: null
};

export default function signin(state = initialState, action) {
    switch (action.type) {
        case 'SIGNIN_FAILED':
            return { ...state, result: { isSuccess: false, errors: action.errors } };
        case 'SIGNIN_SUCCESS':
            return { ...state, result: { isSuccess: true } };
        default:
            return state;
    }
};