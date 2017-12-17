var clearInputError = require('./ClearInputError');
var clearSelectedResource = require('./ClearSelectedResource.js');
var loadResources = require('./LoadResources.js');

module.exports = function () {
    return function (dispatch) {
        dispatch(clearInputError());
        dispatch(clearSelectedResource());
        dispatch(loadResources());
    }
};