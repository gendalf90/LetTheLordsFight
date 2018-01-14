var validator = require('validator');
var setInputError = require('./SetInputError.js');
var reloadResources = require('./ReloadResources.js');
var axios = require('axios');

module.exports = function (name, count) {
    return async function (dispatch, getState) {
        let { id } = getState();

        try {
            validateCount(count);
            await dropResource(id, name, count);
            dispatch(reloadResources());
        } catch (error) {
            handleError(dispatch, error);
        };
    }
};

var validateCount = function (count) {
    if (!validator.isInt(count.toString(), { gt: 0 })) {
        throw {
            validation: true,
            description: 'count is incorrect'
        }
    }
};

var dropResource = async function (id, name, count) {
    await axios.post(`api/v1/storage/${id}/item/${name}/quantity/${count}/decrease`);
}

var handleError = function (dispatch, error) {
    let inputError = {};
    let status = error.response && error.response.status;

    if (status === 403) {
        inputError.type = 'NOT_AUTHORIZED';
    } else if (status === 404) {
        inputError.type = 'NOT_FOUND';
    } else if (status === 400 || error.validation) {
        inputError.type = 'VALIDATION';
        inputError.description = error.description;
    }

    dispatch(setInputError(inputError));
};