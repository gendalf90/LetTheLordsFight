var clearInputError = require('./ClearInputError.js');
var clearSelectedResource = require('./ClearSelectedResource.js');

module.exports = function () {
    return function (dispatch) {
        dispatch(clearInputError());
        dispatch(clearSelectedResource());
    }
};