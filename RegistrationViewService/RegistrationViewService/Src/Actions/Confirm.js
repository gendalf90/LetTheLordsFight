import confirmationFailed from './ConfirmationFailed';
import confirmationSuccess from './ConfirmationSuccess';
import axios from 'axios';

export default function confirm(requestId) {
    return async function (dispatch) {
        try {
            await confirmNewUser(requestId);
            showSuccess(dispatch);
        } catch (exception) {
            showErrors(dispatch, exception);
        };
    }
};

var confirmNewUser = async function (requestId) {
    let request = createRequest(requestId);
    await axios.request(request);
}

var createRequest = function (requestId) {
    return {
        url: `/api/v1/users/confirmation/request/${requestId}`,
        method: 'post',
        baseURL: sessionStorage['api']
    };
};

var showSuccess = function (dispatch) {
    dispatch(confirmationSuccess());
};

var showErrors = function (dispatch, exception) {
    let errors;
    let data = exception.response && exception.response.data;

    if (!data) {
        errors = ['Something went wrong'];
    } else {
        errors = [...data.user, ...data.request];
    }

    dispatch(confirmationFailed(errors));
};