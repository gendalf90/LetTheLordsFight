var React = require("react");
var ReactDOM = require("react-dom");
var redux = require("redux");
var { Provider } = require("react-redux");
var reducer = require("./Reducers/reducer.js");
var AppView = require("./Views/App.jsx");
var thunk = require('redux-thunk').default;
var loadResources = require('./Actions/LoadResources.js');
var axios = require('axios');

axios.defaults.baseURL = sessionStorage['api'];
axios.defaults.headers.common['Authorization'] = localStorage['token'];

var store = redux.createStore(reducer, redux.applyMiddleware(thunk));

store.dispatch(loadResources());

ReactDOM.render(
    <Provider store={store}>
        <AppView />
    </Provider>,
    document.getElementById("content")
);