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

//var dropItem = function (name, count) {
//    return async function (dispatch, getState) {
//        let state = getState();
//        let id = state.id;
//        let api = state.api;

//        try {
            
//        } catch (error) {
            
//        };
//    }
//};

var getJsonOrThrowError = function (response) {
    if (!response.ok) {
        throw response;
    }

    return response.json();
};

var handleStorageError = function (dispatch, error) {
    if (error.status === undefined) {
        dispatch(setStorageError(error));
    } else {
        dispatch(setStorageError(error.status));
    }
};

//var handleItemError = function (dispatch, name, error) {
//    if (error.status === undefined) {
//        dispatch(setItemError(error));
//    } else {
//        dispatch(setItemError(error.status));
//    }
//};

module.exports = { configure, initialize, setItems, clearItems, loadItems };