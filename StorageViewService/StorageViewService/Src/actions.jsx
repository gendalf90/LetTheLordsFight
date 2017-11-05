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
    return setItems([]);
}

var setItems = function (items) {
    return {
        type: 'SET_ITEMS',
        items
    }
};

var setError = function (error) {
    return {
        type: 'SET_ERROR',
        error
    }
};

var loadItems = function () {
    return function (dispatch, getState) {
        let state = getState();
        let id = state.id;
        let api = state.api;
        let getItemsUrl = api + 'api/v1/storage/' + id;
        fetch(getItemsUrl).then(getJsonOrThrowError)
                          .then(json => dispatch(setItems(json.items)))
                          .catch(error => handleError(dispatch, error));
    }
};

var getJsonOrThrowError = function (response) {
    if (!response.ok) {
        throw response;
    }

    return response.json();
};

var handleError = function (dispatch, error) {
    if (error.status === undefined) {
        dispatch(setError(error));
    } else {
        dispatch(setError(error.status));
    }
};

module.exports = { configure, initialize, setItems, clearItems, loadItems };