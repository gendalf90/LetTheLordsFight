var loadStorages = require('./LoadStorages.js');
var setSelectedResource = require('./SetSelectedResource.js');

module.exports = function (resource) {
    return function (dispatch) {
        dispatch(loadStorages());
        dispatch(setSelectedResource(resource));
    }
};