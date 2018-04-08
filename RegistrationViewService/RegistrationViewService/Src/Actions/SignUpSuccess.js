export default function signUpSuccess(email) {
    return {
        type: 'SIGNUP_SUCCESS',
        email
    }
};