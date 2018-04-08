export default function confirmationFailed(errors) {
    return {
        type: 'CONFIRMATION_FAILED',
        errors
    }
};