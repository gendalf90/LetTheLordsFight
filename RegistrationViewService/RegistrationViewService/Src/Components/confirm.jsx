import React from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import confirm from '../Actions/Confirm';

class Confirm extends React.Component {
    componentDidMount() {
        this.confirmRequest();
    }

    confirmRequest() {
        this.props.actions.confirm(this.props.match.params.requestId);
    }

    render() {
        return (
            <div>
                {this.renderErrorsIfExist()}
                {this.renderSuccessIfExist()}
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
                <p>User is created. Go to Sign In form and enter your credentials.</p>
            </div>
        );
    }
};

export default connect(state => ({ result: state.confirm.result }), dispatch => ({ actions: bindActionCreators({ confirm }, dispatch) }))(Confirm);