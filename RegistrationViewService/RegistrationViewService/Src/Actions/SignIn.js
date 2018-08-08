import signInFailed from './SignInFailed';
import signInSuccess from './SignInSuccess';
import axios from 'axios';

export default function signIn(login, password) {
    return async function (dispatch) {
        try {
            let token = await getToken(login, password);
            setTokenToLocalStorage(token);
            showSuccess(dispatch);
        } catch (exception) {
            showErrors(dispatch, exception);
        };
    }
};

var getToken = async function (login, password) {
    let request = createRequest(login, password);
    let response = await axios.request(request);
    return response.data.token;
};

var setTokenToLocalStorage = function (token) {
    localStorage.setItem('token', 'Bearer ' + token);
}

var createRequest = function (login, password) {
    return {
        url: 'api/v1/users/current/token',
        baseURL: sessionStorage['api'],
        method: 'get',
        auth: {
            username: login,
            password: password
        }
    };
};

var showSuccess = function (dispatch) {
    dispatch(signInSuccess());
};

var showErrors = function (dispatch, exception) {
    let errors;
    let status = exception.response && exception.response.status;

    if (status === 401) {
        errors = ['Not Authorized'];
    } else {
        errors = ['Something went wrong'];
    }

    dispatch(signInFailed(errors));
};