var setResources = require('./SetResources.js');
var setStorageError = require('./SetStorageError.js');

module.exports = function () {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;

        try {
            let storageResponse = await fetch(`${api}api/v1/storage/${id}`);
            let storage = await getJsonOrThrowError(storageResponse);
            let resources = storage.items;
            dispatch(setResources(resources));
        } catch (error) {
            handleError(dispatch, error);
        };
    }
};

var getJsonOrThrowError = function (response) {
    if (!response.ok) {
        throw response;
    }

    return response.json();
};

var handleError = function (dispatch, error) {
    var storageError = {};

    if (error.status === 403) {
        storageError.type = 'NOT_AUTHORIZED';
    } else if (error.status === 404) {
        storageError.type = 'NOT_FOUND';
    }

    dispatch(setStorageError(storageError));
};