var clearResources = require('./ClearResources.js');
var clearStorages = require('./ClearStorages.js');
var clearInputError = require('./ClearInputError.js');
var clearSelectedResource = require('./ClearSelectedResource.js');

module.exports = function () {
    return function (dispatch) {
        dispatch(clearResources());
        dispatch(clearStorages());
        dispatch(clearInputError());
        dispatch(clearSelectedResource());
    }
};