import showSignIn from './ShowSignIn';
import setSignUpErrors from './SetSignUpErrors';
import clearSignUpErrors from './ClearSignUpErrors';
import showError from './ShowError';
import axios from 'axios';

export default function signUp(login, password) {
    return async function (dispatch) {
        try {
            await postNewUser(login, password);
            dispatch(showSignIn());
        } catch (exception) {
            showErrors(dispatch, exception);
        };
    };
};

var postNewUser = async function (login, password) {
    let request = createRequest(login, password);
    await axios.request(request);
};

var createRequest = function (login, password) {
    return {
        url: 'api/v1/users',
        method: 'post',
        baseURL: sessionStorage['api'],
        data: { login, password }
    };
};

var showErrors = function (dispatch, exception) {
    let errors = createErrorsFromException(exception);

    if (errors) {
        dispatch(setSignUpErrors(errors));
    } else {
        dispatch(showError());
    };
};

var createErrorsFromException = function (exception) {
    let data = exception.response && exception.response.data;

    if (!data) {
        return;
    }

    return {
        login: data.login,
        password: data.password
    };
};