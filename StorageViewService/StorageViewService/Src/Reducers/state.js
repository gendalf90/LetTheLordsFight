var descriptions = require("../Descriptions/descriptions.js");

exports.create = function () {
    return {
        id: sessionStorage['id'],
        resourceDesctiptions: descriptions.resources,
        resources: [],
        storagesIds: []
    }
}