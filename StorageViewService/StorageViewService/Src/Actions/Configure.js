module.exports = function (id, api, token) {
    return {
        type: 'CONFIGURE',
        id,
        api,
        token
    }
};