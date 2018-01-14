var setResources = require('./SetResources.js');
var setStorageError = require('./SetStorageError.js');
var axios = require('axios');

module.exports = function () {
    return async function (dispatch, getState) {
        let { id } = getState();

        try {
            let storage = await getStorage(id);
            let resources = storage.items;
            dispatch(setResources(resources));
        } catch (error) {
            handleError(dispatch, error);
        };
    }
};

var getStorage = async function (id) {
    let { data } = await axios.get(`api/v1/storage/${id}`);
    return data;
};

var handleError = function (dispatch, error) {
    let storageError = {};
    let status = error.response && error.response.status;

    if (status === 403) {
        storageError.type = 'NOT_AUTHORIZED';
    } else if (status === 404) {
        storageError.type = 'NOT_FOUND';
    }

    dispatch(setStorageError(storageError));
};