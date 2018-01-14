var createDefaultErrors = function () {
    return {
        login: [],
        password: []
    };
};

var createErrorsFromAction = function (actionErrors) {
    let result = createDefaultErrors();

    if (!actionErrors) {
        return result;
    }

    if (Array.isArray(actionErrors.login)) {
        result.login = Array.from(actionErrors.login);
    }

    if (Array.isArray(actionErrors.password)) {
        result.password = Array.from(actionErrors.password);
    }

    return result;
};

const initialState = {
    show: false,
    errors: createDefaultErrors()
};

export default function signup(state = initialState, action) {
    switch (action.type) {
        case 'SHOW_SIGNIN':
            return { ...state, show: false };
        case 'SHOW_SIGNUP':
            return { ...state, show: true, errors: createDefaultErrors() };
        case 'SET_SIGNUP_ERRORS':
            return { ...state, errors: createErrorsFromAction(action.errors) };
        case 'CLEAR_SIGNUP_ERRORS':
            return { ...state, errors: createDefaultErrors() };
        default:
            return state;
    }
};