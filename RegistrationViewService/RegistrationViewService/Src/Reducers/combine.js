import { combineReducers } from 'redux';
import signin from './signin';
import signup from './signup';
import error from './error';

export default combineReducers({
    signin,
    signup,
    error
});