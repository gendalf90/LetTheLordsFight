﻿var clearInputError = require('./ClearInputError.js');
var loadResources = require('./LoadResources.js');

module.exports = function () {
    return function (dispatch) {
        dispatch(clearInputError());
        dispatch(loadResources());
    }
};