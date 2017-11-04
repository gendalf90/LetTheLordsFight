var configure = function (storage, api, token) {
    return {
        type: 'CONFIGURE',
        storage,
        api,
        token
    }
};

var setItems = function (items) {
    return {
        type: 'SET_ITEMS',
        items
    }
};

var loadItems = function () {
    return function (dispatch, getState) {
        let state = getState();
        let storage = state.storage;
        let api = state.api;
        let getItemsUrl = api + '/api/v1/storage/' + storage;
        fetch(getItemsUrl).then(response => response.json())
                          .then(json => dispatch(setItems(json.items)))
                          .catch(reason => console.log(reason));
    }
};

module.exports = { configure, setItems, loadItems };