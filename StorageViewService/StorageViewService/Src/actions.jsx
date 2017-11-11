var validator = require('validator');

var configure = function (id, api, token) {
    return {
        type: 'CONFIGURE',
        id,
        api,
        token
    }
};

var initialize = function (itemDescriptions) {
    return {
        type: 'INITIALIZE',
        itemDescriptions
    }
};

var clearItems = function () {
    return setItems([], []);
}

var setItems = function (items, storages) {
    return {
        type: 'SET_ITEMS',
        items,
        storages
    }
};

var setStorageError = function (error) {
    return {
        type: 'SET_STORAGE_ERROR',
        error
    }
};

var setItemError = function (name, error) {
    return {
        type: 'SET_ITEM_ERROR',
        name,
        error
    }
};

var clearItemError = function () {
    return {
        type: 'CLEAR_ITEM_ERROR'
    }
};

var loadItems = function () {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;
        
        try {
            let storageResponse = await fetch(`${api}api/v1/storage/${id}`);
            let storage = await getJsonOrThrowError(storageResponse);
            let mapObjectRepsonse = await fetch(`${api}api/v1/map/objects/${id}`);
            let mapObject = await getJsonOrThrowError(mapObjectRepsonse);
            let segmentResponse = await fetch(`${api}api/v1/map/segment/i/${mapObject.segment.i}/j/${mapObject.segment.j}`);
            let segment = await getJsonOrThrowError(segmentResponse);
            let items = storage.items;
            let storages = segment.objects.map(obj => obj.id);
            dispatch(setItems(items, storages));
        } catch (error) {
            handleStorageError(dispatch, error);
        };
    }
};

var dropItem = function (name, count) {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;

        try {
            validateCount(count);
            let response = await fetch(`${api}api/v1/storage/${id}/item/${name}/quantity/${count}/decrease`, { method: "POST" });
            throwErrorIfNotOk(response);
            reloadItems(dispatch);
        } catch (error) {
            handleItemError(dispatch, name, error);
        };
    }
};

var sendItem = function (name, count, storage) {
    return async function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;

        try {
            validateCount(count);
            let response = await fetch(`${api}api/v1/storage/${id}/item/${name}/quantity/${count}/to/${storage}`, { method: "POST" });
            throwErrorIfNotOk(response);
            reloadItems(dispatch);
        } catch (error) {
            handleItemError(dispatch, name, error);
        };
    }
};

var validateCount = function (count) {
    if (!validator.isInt(count.toString(), { gt: 0 })) {
        throw {
            validation: true,
            desc: 'Count is incorrect'
        }
    }
};

var getJsonOrThrowError = function (response) {
    if (!response.ok) {
        throw response;
    }

    return response.json();
};

var throwErrorIfNotOk = function (response) {
    if (!response.ok) {
        throw response;
    }
};

var handleStorageError = function (dispatch, error) {
    var result = {};

    if (error.status === 403) {
        result.notAuthorized = true;
    } else if (error.status === 404) {
        result.notFound = true;
    } else {
        result.unknown = true;
    }

    dispatch(setStorageError(result));
};

var handleItemError = function (dispatch, name, error) {
    var result = {};
    
    if (error.status === 400 || error.validation) {
        result.validation = true;
        result.desc = error.desc;
    } else {
        result.unknown = true;
    }

    dispatch(setItemError(name, result));
};

var reloadItems = function (dispatch) {
    dispatch(clearItemError());
    dispatch(loadItems());
}

module.exports = { configure, initialize, setItems, clearItems, loadItems, dropItem, sendItem };