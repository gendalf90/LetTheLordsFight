import React from 'react';

export default class SignIn extends React.Component {
    render() {
        return (
            <div className="card">
                <div className="card-header text-center">
                    <h3>Sign un</h3>
                </div>
                <div className="card-body">
                    <form>
                        <div>
                            <label for="email">Email</label>
                            <input type="text" className="form-control is-invalid" id="email" />
                            <div className="invalid-feedback">
                                <ul>
                                    <li>error 1</li>
                                    <li>error 2</li>
                                </ul>
                            </div>
                        </div>
                        <div>
                            <label for="password">Password</label>
                            <input type="password" className="form-control" id="password" />
                            <div className="invalid-feedback">
                                <ul>
                                    <li>error 1</li>
                                    <li>error 2</li>
                                </ul>
                            </div>
                        </div>
                    </form>
                    <div className="mt-3 text-center">
                        <button type="button" className="btn btn-primary">Submit</button>
                    </div>
                </div>
            </div>
        );
    }
};