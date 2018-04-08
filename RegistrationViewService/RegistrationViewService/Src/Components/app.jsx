import React from 'react';
import { connect } from 'react-redux';
import SignIn from './signin';
import SignUp from './signup';
import Confirm from './confirm';
import { Switch, Route } from 'react-router-dom';

class App extends React.Component {
    render() {
        return (
            <div>
                {this.renderHead()}
                {this.renderBody()}
            </div>
        );
    }

    renderHead() {
        return (
            <nav className="navbar navbar-light bg-light justify-content-between">
                <div className="navbar-brand">
                    <img src="https://png.icons8.com/ios/50/000000/castle.png" />
                </div>
                <div className="btn-group" role="group" aria-label="Basic example">
                    <button type="button" className="btn btn-outline-primary" onClick={() => this.props.history.push('/registration/signup')}>Sign Up</button>
                    <button type="button" className="btn btn-outline-primary" onClick={() => this.props.history.push('/registration/signin')}>Sign In</button>
                </div>
            </nav>
        );
    }

    renderBody() {
        return (
            <div className="container-fluid">
                <div className="row">
                    <div className="col-sm">
                    </div>
                    <div className="col-sm">
                        {this.renderRouteData()}
                    </div>
                    <div className="col-sm">
                    </div>
                </div>
            </div>
        );
    }

    renderRouteData() {
        return (
            <Switch>
                <Route exact path="/registration/signin" component={SignIn} />
                <Route exact path="/registration/signup" component={SignUp} />
                <Route exact path="/registration/confirm/:requestId" component={Confirm} />
            </Switch>
        );
    }
};

export default connect(state => ({}), dispatch => ({}))(App);