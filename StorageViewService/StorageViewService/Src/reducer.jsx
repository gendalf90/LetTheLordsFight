var reducer = function (state = {}, action) {
    switch (action.type) {
        case "SET_ITEMS":
            return Object.assign({}, state, {
                items: action.items
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
        case "SET_ERROR":
            return Object.assign({}, state, {
                error: action.error
            });
        default:
            return Object.assign({}, state);
    }
};

module.exports = reducer;