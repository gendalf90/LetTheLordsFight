export default function showError(description = 'something went wrong') {
    return {
        type: 'SHOW_ERROR',
        description
    }
};