module.exports = function (error) {
    return {
        type: 'SET_STORAGE_ERROR',
        error
    }
};