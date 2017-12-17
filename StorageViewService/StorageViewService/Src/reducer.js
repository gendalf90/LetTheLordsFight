var reducer = function (state = {}, action) {
    switch (action.type) {
        case "CONFIGURE":
            return Object.assign({}, state, {
                id: action.id,
                api: action.api,
                token: action.token
            });
        case "INITIALIZE":
            return Object.assign({}, state, {
                resourceDesctiptions: action.descriptions
            });
        case "SET_RESOURCES":
            return Object.assign({}, state, {
                resources: action.resources
            });
        case "SET_SELECTED_RESOURCE":
            return Object.assign({}, state, {
                selectedResource: {
                   name: action.resourceName
                }
            });
        case "SET_STORAGE_ERROR":
            return Object.assign({}, state, {
                storageError: action.error
            });
        case "SET_INPUT_ERROR":
            return Object.assign({}, state, {
                inputError: action.error
            });
        case "SET_STORAGES":
            return Object.assign({}, state, {
                storagesIds: action.ids
            });
        default:
            return Object.assign({}, state);
    }
};

module.exports = reducer;