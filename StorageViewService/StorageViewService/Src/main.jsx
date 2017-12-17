var React = require("react");
var ReactDOM = require("react-dom");
var redux = require("redux");
var { Provider } = require("react-redux");
var reducer = require("./reducer.js");
var AppView = require("./Views/App.jsx");
var thunk = require('redux-thunk').default;
var descriptions = require("./descriptions.js");
var configure = require('./Actions/Configure.js');
var initialize = require('./Actions/Initialize.js');
var setDefaults = require('./Actions/SetDefaults.js');
var loadResources = require('./Actions/LoadResources.js');

var store = redux.createStore(reducer, redux.applyMiddleware(thunk));

store.dispatch(configure(sessionStorage['id'], sessionStorage['api'], localStorage['token']));
store.dispatch(initialize(descriptions.resources));
store.dispatch(setDefaults());
store.dispatch(loadResources());

ReactDOM.render(
    <Provider store={store}>
        <AppView />
    </Provider>,
    document.getElementById("content")
);