var reducer = function (state = {}, action) {
    switch (action.type) {
        case "SET_ITEMS":
            return Object.assign({}, state, {
                items: action.items,
                storages: action.storages
            });
        case "CONFIGURE":
            return Object.assign({}, state, {
                id: action.id,
                api: action.api,
                token: action.token
            });
        case "INITIALIZE":
            return Object.assign({}, state, {
                itemDescriptions: action.itemDescriptions
            });
        case "SET_STORAGE_ERROR":
            return Object.assign({}, state, {
                storageError: action.error
            });
        case "SET_ITEM_ERROR":
            return Object.assign({}, state, {
                itemError: {
                    name: action.name,
                    error: action.error
                }
            });
        case "CLEAR_ITEM_ERROR":
            return Object.assign({}, state, {
                itemError: null
            });
        default:
            return Object.assign({}, state);
    }
};

module.exports = reducer;