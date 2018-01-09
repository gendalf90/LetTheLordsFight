import React from 'react';
import { render } from 'react-dom';
import { createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux';
import ReduxThunk from 'redux-thunk';
import combineReducers from './Reducers/combine';
import App from './components/app';

const store = createStore(combineReducers, applyMiddleware(ReduxThunk));

render(
    <Provider store={store}>
        <App />
    </Provider>,
    document.getElementById('content')
)