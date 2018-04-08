import { combineReducers } from 'redux';
import signup from './signup';
import signin from './signin';
import confirm from './confirm';

export default combineReducers({
    signup,
    signin,
    confirm
});