import showError from './ShowError';
import axios from 'axios';

export default function signIn(login, password) {
    return async function (dispatch) {
        try {
            let token = await getToken(login, password);
            setTokenToLocalStorage(token);
            goToMapPage();
        } catch (exception) {
            handleError(dispatch, exception);
        };
    }
};

var getToken = async function (login, password) {
    let request = createRequest(login, password);
    let response = await axios.request(request);
    return response.data;
};

var setTokenToLocalStorage = function (token) {
    localStorage.setItem('token', 'Bearer ' + token);
}

var goToMapPage = function () {
     window.location = 'map';
};

var createRequest = function (login, password) {
    return {
        url: 'api/v1/users/current/token',
        baseURL: sessionStorage['api'],
        auth: {
            username: login,
            password: password
        }
    };
};

var handleError = function (dispatch, exception) {
    let status = exception.response && exception.response.status;

    if (status === 401) {
        dispatch(showError('not authorized'));
    } else {
        dispatch(showError());
    }
};