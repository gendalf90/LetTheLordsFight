var state = require('./state.js');

var reducer = function (currentState = state.create(), action) {
    switch (action.type) {
        case "SET_RESOURCES":
            return Object.assign({}, currentState, {
                resources: action.resources
            });
        case "SET_SELECTED_RESOURCE":
            return Object.assign({}, currentState, {
                selectedResource: {
                   name: action.resourceName
                }
            });
        case "SET_STORAGE_ERROR":
            return Object.assign({}, currentState, {
                storageError: action.error
            });
        case "SET_INPUT_ERROR":
            return Object.assign({}, currentState, {
                inputError: action.error
            });
        case "SET_STORAGES":
            return Object.assign({}, currentState, {
                storagesIds: action.ids
            });
        default:
            return currentState;
    }
};

module.exports = reducer;