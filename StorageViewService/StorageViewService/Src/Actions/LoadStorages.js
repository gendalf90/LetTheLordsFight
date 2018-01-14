var setStorages = require('./SetStorages.js');
var setStorageError = require('./SetStorageError.js');
var axios = require('axios');

module.exports = function () {
    return async function (dispatch, getState) {
        let { id } = getState();

        try {
            let mapObject = await getMapObject(id);
            let segment = await getSegment(mapObject.segment.i, mapObject.segment.j);
            let storages = segment.objects.filter(obj => obj.id != id).map(obj => obj.id);
            dispatch(setStorages(storages));
        } catch (error) {
            handleError(dispatch, error);
        };
    }
};

var getMapObject = async function (id) {
    let { data } = await axios.get(`api/v1/map/objects/${id}`);
    return data;
}

var getSegment = async function (i, j) {
    let { data } = await axios.get(`api/v1/map/segments/i/${i}/j/${j}`);
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