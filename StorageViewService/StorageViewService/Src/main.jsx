var React = require("react");
var ReactDOM = require("react-dom");
var redux = require("redux");
var { Provider } = require("react-redux");
var reducer = require("./reducer.jsx");
var AppView = require("./views.jsx");
var thunk = require('redux-thunk').default;
var actions = require("./actions.jsx");
var itemDescriptions = require("./descriptions.js");

var store = redux.createStore(reducer, redux.applyMiddleware(thunk));

store.dispatch(actions.configure(sessionStorage['id'], sessionStorage['api'], localStorage['token']));
store.dispatch(actions.initialize(itemDescriptions));
store.dispatch(actions.clearItems());
store.dispatch(actions.loadItems());

ReactDOM.render(
    <Provider store={store}>
        <AppView />
    </Provider>,
    document.getElementById("content")
);