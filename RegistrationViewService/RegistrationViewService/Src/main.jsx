import React from 'react';
import { render } from 'react-dom';
import { createStore, applyMiddleware } from 'redux';
import { Provider } from 'react-redux';
import ReduxThunk from 'redux-thunk';
import combineReducers from './Reducers/combine';
import App from './components/app';
import { Route, Router } from 'react-router';
import { BrowserRouter } from 'react-router-dom';
import { routerMiddleware } from 'react-router-redux';
import { createBrowserHistory } from 'history';

const history = createBrowserHistory();

const store = createStore(combineReducers, applyMiddleware(ReduxThunk, routerMiddleware(history)));

render(
    <Provider store={store}>
        <Router history={history}>
            <Route path="/" component={App} />
        </Router>
    </Provider>,
    document.getElementById('content')
)