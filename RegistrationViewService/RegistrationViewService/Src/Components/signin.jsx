import React from 'react';

export default class SignIn extends React.Component {
    render() {
        return (
            <div className="card">
                <div className="card-header text-center">
                    <h3>Sign in</h3>
                </div>
                <div className="card-body">
                    <form>
                        <label htmlFor="email">Email</label>
                        <input type="text" className="form-control" id="email" />
                        <label htmlFor="password">Password</label>
                        <input type="password" className="form-control" id="password" />
                    </form>
                    <div className="mt-3 text-center">
                        <button type="button" className="btn btn-primary">Submit</button>
                    </div>
                </div>
            </div>
        );
    }
};