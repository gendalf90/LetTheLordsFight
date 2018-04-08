export default function signInFailed(errors) {
    return {
        type: 'SIGNIN_FAILED',
        errors
    }
};