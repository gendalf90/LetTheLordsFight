import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import SignIn from './signin';
import SignUp from './signup';
import Error from './error';
import showSignUp from '../Actions/ShowSignUp';
import showSignIn from '../Actions/ShowSignIn';
import signUp from '../Actions/SignUp';
import signIn from '../Actions/SignIn';

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
                    <button type="button" className="btn btn-outline-primary" onClick={() => this.props.actions.showSignUp()}>SignUp</button>
                    <button type="button" className="btn btn-outline-primary" onClick={() => this.props.actions.showSignIn()}>SignIn</button>
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
                        {this.renderError()}
                        {this.renderSignIn()}
                        {this.renderSignUp()}
                    </div>
                    <div className="col-sm">
                    </div>
                </div>
            </div>
        );
    }

    renderError() {
        let { show, description } = this.props.error;

        if (show) {
            return <Error description={description} />
        }
    }

    renderSignIn() {
        let { show } = this.props.signin;
        let { actions } = this.props;

        if (show) {
            return <SignIn actions={actions} />
        }
    }

    renderSignUp() {
        let { show, errors } = this.props.signup;
        let { actions } = this.props;

        if (show) {
            return <SignUp errors={errors} actions={actions} />
        }
    }
};

function mapStateToProps(state) {
    return {
        error: state.error,
        signin: state.signin,
        signup: state.signup
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators({ showSignIn, showSignUp, signUp, signIn }, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(App);