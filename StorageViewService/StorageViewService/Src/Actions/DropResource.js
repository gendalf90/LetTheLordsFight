﻿var validator = require('validator');
var setInputError = require('./SetInputError.js');
var reloadResources = require('./ReloadResources.js');

module.exports = function (name, count) {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;

        try {
            validateCount(count);
            let response = await fetch(`${api}api/v1/storage/${id}/item/${name}/quantity/${count}/decrease`, { method: "POST" });
            throwErrorIfNotOk(response);
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

var throwErrorIfNotOk = function (response) {
    if (!response.ok) {
        throw response;
    }
};

var handleError = function (dispatch, error) {
    var inputError = {};
    
    if (error.status === 403) {
        inputError.type = 'NOT_AUTHORIZED';
    } else if (error.status === 404) {
        inputError.type = 'NOT_FOUND';
    } else if (error.status === 400 || error.validation === true) {
        inputError.type = 'VALIDATION';
        inputError.description = error.description;
    }

    dispatch(setInputError(inputError));
};