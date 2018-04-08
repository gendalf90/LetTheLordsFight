import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import signIn from '../Actions/SignIn';

class SignIn extends React.Component {
    render() {
        return (
            <div>
                {this.renderErrorsIfExist()}
                {this.renderSuccessIfExist()}
                {this.renderInput()}
            </div>
        );
    }

    renderInput() {
        return (
            <div className="card">
                <div className="card-header text-center">
                    <h3>Sign in</h3>
                </div>
                <div className="card-body">
                    <form>
                        <label htmlFor="email">Email</label>
                        <input type="text" className="form-control" id="email" ref="login" />
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" id="password" ref="password" />
                    </form>
                    <div className="mt-3 text-center">
                        <button type="button" className="btn btn-primary" onClick={this.signInCurrentUser.bind(this)}>Submit</button>
                    </div>
                </div>
            </div>
        );
    }

    renderErrorsIfExist() {
        let result = this.props.result;

        if (!result || result.isSuccess) {
            return;
        }

        return (
            <div className="alert alert-danger" role="alert">
                <h4 className="alert-heading">Error</h4>
                <p>
                    <ul>
                        {result.errors.map(error => <li key={error}>{error}</li>)}
                    </ul>
                </p>
            </div>
        );
    }

    renderSuccessIfExist() {
        let result = this.props.result;

        if (!result || !result.isSuccess) {
            return;
        }

        return (
            <div className="alert alert-success" role="alert">
                <h4 class="alert-heading">Success</h4>
                <p>You have signed in</p>
            </div>
        );
    }

    signInCurrentUser() {
        let login = this.refs.login.value;
        let password = this.refs.password.value;
        this.props.actions.signIn(login, password);
    }
};

export default connect(state => ({ result: state.signin.result }), dispatch => ({ actions: bindActionCreators({ signIn }, dispatch) }))(SignIn);