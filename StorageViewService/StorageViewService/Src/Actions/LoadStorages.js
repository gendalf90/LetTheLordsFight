var setStorages = require('./SetStorages.js');
var setStorageError = require('./SetStorageError.js');

module.exports = function () {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;

        try {
            let mapObjectRepsonse = await fetch(`${api}api/v1/map/objects/${id}`);
            let mapObject = await getJsonOrThrowError(mapObjectRepsonse);
            let segmentResponse = await fetch(`${api}api/v1/map/segments/i/${mapObject.segment.i}/j/${mapObject.segment.j}`);
            let segment = await getJsonOrThrowError(segmentResponse);
            let storages = segment.objects.filter(obj => obj.id != id).map(obj => obj.id);
            dispatch(setStorages(storages));
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