var descriptions = require("../Descriptions/descriptions.js");

exports.create = function () {
    return {
        id: sessionStorage['id'],
        api: sessionStorage['api'],
        token: localStorage['token'],
        resourceDesctiptions: descriptions.resources,
        resources: [],
        storagesIds: []
    }
}