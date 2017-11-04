var reducer = function (state = {}, action) {
    switch (action.type) {
        case "SET_ITEMS":
            return Object.assign({}, state, {
                items: action.items
            });
        case "CONFIGURE":
            return Object.assign({}, state, {
                storage: action.storage,
                api: action.api,
                token: action.token
            });
        default:
            return Object.assign({}, state);
    }
};

module.exports = reducer;