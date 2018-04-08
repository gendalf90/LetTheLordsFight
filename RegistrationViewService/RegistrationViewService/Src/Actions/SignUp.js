import signUpSuccess from './SignUpSuccess';
import signUpFailed from './SignUpFailed';
import axios from 'axios';

export default function signUp(login, password) {
    return async function (dispatch) {
        try {
            await postRegistrationRequest(login, password);
            showSuccess(dispatch, login);
        } catch (exception) {
            showErrors(dispatch, exception);
        };
    };
};

var postRegistrationRequest = async function (login, password) {
    let request = createRequest(login, password);
    await axios.request(request);
};

var createRequest = function (login, password) {
    return {
        url: '/api/v1/users/registration/requests',
        method: 'post',
        baseURL: sessionStorage['api'],
        data: { login, password }
    };
};

var showSuccess = function (dispatch, login) {
    dispatch(signUpSuccess(login));
};

var showErrors = function (dispatch, exception) {
    let errors;
    let data = exception.response && exception.response.data;

    if (!data) {
        errors = ['Something went wrong'];
    } else {
        errors = [...data.login, ...data.password, ...data.request];
    }

    dispatch(signUpFailed(errors));
};