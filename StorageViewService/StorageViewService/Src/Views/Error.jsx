var React = require("react");

class ErrorView extends React.Component {
    render() {
        let data = this.props.data;

        switch (data.type) {
            case 'NOT_FOUND': {
                return this.notFound;
            }
            case 'NOT_AUTHORIZED': {
                return this.notAuthorized;
            }
            case 'VALIDATION': {
                return this.validation;
            }
            default: {
                return this.unknown;
            }
        }
    }

    get validation() {
        return (
            <div className="alert alert-warning" role="alert">
                <h4 className="alert-heading">Validation error</h4>
                {this.renderDescription()}
            </div>
        );
    }

    get notFound() {
        return (
            <div className="alert alert-warning" role="alert">
                Not found
            </div>
        );
    }

    get notAuthorized() {
        return (
            <div className="alert alert-warning" role="alert">
                <h4 className="alert-heading">Not authorized</h4>
                <p>you do not have privileges to access</p>
            </div>
        );
    }

    get unknown() {
        return (
            <div className="alert alert-danger" role="alert">
                <h4 className="alert-heading">Error</h4>
                <p>something went wrong</p>
            </div>
        );
    }

    renderDescription() {
        if (this.props.data.description) {
            return <p>{this.props.data.description}</p>
        }
    }
};

module.exports = ErrorView;