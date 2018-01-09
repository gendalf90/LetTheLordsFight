import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import SignIn from './signin';
import Error from './error';

class App extends React.Component {
    render() {
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

    }

    renderSignUp() {

    }
};

function mapStateToProps(state) {
    return {
        error: state.error
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators({ }, dispatch)
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(App);