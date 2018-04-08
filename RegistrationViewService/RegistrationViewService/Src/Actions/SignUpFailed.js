export default function signUpFailed(errors) {
    return {
        type: 'SIGNUP_FAILED',
        errors
    }
};