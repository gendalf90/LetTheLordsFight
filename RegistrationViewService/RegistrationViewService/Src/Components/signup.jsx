import React from 'react';

export default class SignUp extends React.Component {
    render() {
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
        let errors = this.props.errors.login;
        let hasErrors = errors.length > 0;
        let invalidClassName = hasErrors ? 'is-invalid' : '';

        return (
            <div>
                <label htmlFor="email">Email</label>
                <input type="text" className={'form-control ' + invalidClassName} id="email" ref="login" />
                {this.renderInputErrorsIfExist(errors)}
            </div>
        );
    }

    renderPasswordInput() {
        let errors = this.props.errors.password;
        let hasErrors = errors.length > 0;
        let invalidClassName = hasErrors ? 'is-invalid' : '';

        return (
            <div>
                <label htmlFor="password">Password</label>
                <input type="password" className={'form-control ' + invalidClassName} id="password" ref="password"/>
                {this.renderInputErrorsIfExist(errors)}
            </div>
        );
    }

    renderInputErrorsIfExist(errors) {
        if (errors.length == 0) {
            return;
        }

        let set = new Set(errors);

        return (
            <div className="invalid-feedback">
                <ul>
                    {errors.map(error => <li key={error}>{error}</li>)}
                </ul>
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