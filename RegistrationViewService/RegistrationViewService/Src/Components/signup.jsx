import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import signUp from '../Actions/SignUp';

class SignUp extends React.Component {
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
                {this.renderHead()}
                {this.renderBody()}
            </div>
        );
    }

    renderHead() {
        return (
            <div className="card-header text-center">
                <h3>Sign up</h3>
            </div>
        );
    }

    renderBody() {
        return (
            <div className="card-body">
                <form>
                    {this.renderLoginInput()}
                    {this.renderPasswordInput()}
                </form>
                {this.renderSubmitInput()}
            </div>
        );
    }

    renderLoginInput() {
        return (
            <div>
                <label htmlFor="email">Email</label>
                <input type="text" className="form-control" id="email" ref="login" />
            </div>
        );
    }

    renderPasswordInput() {
        return (
            <div>
                <label htmlFor="password">Password</label>
                <input type="password" className="form-control" id="password" ref="password"/>
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
                <h4 class="alert-heading">Error</h4>
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
                <h4 className="alert-heading">Success</h4>
                <p>Please go in your email <strong>{result.email}</strong> and confirm your registration request</p>
            </div>
        );
    }

    renderSubmitInput() {
        return (
            <div className="mt-3 text-center">
                <button type="button" className="btn btn-primary" onClick={this.signUpCurrentUser.bind(this)}>Submit</button>
            </div>
        );
    }

    signUpCurrentUser() {
        let login = this.refs.login.value;
        let password = this.refs.password.value;
        this.props.actions.signUp(login, password);
    }
};

export default connect(state => ({ result: state.signup.result }), dispatch => ({ actions: bindActionCreators({ signUp }, dispatch) }))(SignUp);