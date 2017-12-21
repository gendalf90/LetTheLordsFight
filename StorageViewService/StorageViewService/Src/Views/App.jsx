var React = require("react");
var { connect } = require("react-redux");
var { bindActionCreators } = require("redux");
var ErrorView = require("./Error.jsx");
var ResourcesView = require("./Resources.jsx");
var ResourceInputVew = require("./ResourceInput.jsx");
var sendResource = require("../Actions/SendResource.js");
var dropResource = require("../Actions/DropResource.js");
var selectResource = require("../Actions/SelectResource.js");
var unselectResource = require("../Actions/UnselectResource.js");

class AppView extends React.Component {
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
            <nav className="navbar sticky-top navbar-light bg-light justify-content-between">
                <div className="navbar-brand">{this.props.myId}</div>
                <div>
                    {this.renderResourceInput()}
                </div>
            </nav>
        );
    }

    renderBody() {
        return (
            <div className="container-fluid">
                <div className="row">
                    <div className="col-2">
                    </div>
                    <div className="col-8">
                        {this.renderStorageError()}
                        {this.renderInputError()}
                        {this.renderResources()}
                    </div>
                    <div className="col-2">
                    </div>
                </div>
            </div>
        );
    }

    renderResourceInput() {
        let resource = this.props.selectedResource;
        let storages = this.props.storagesIds;
        let actions = this.props.actions;

        if (resource) {
            return <ResourceInputVew resource={resource} storages={storages} actions={actions} />
        }
    }

    renderInputError() {
        let error = this.props.inputError;

        if (error) {
            return <ErrorView data={error} />
        }
    }

    renderStorageError() {
        let error = this.props.storageError;

        if (error) {
            return <ErrorView data={error} />
        }
    }

    renderResources() {
        return <ResourcesView items={this.props.resources} actions={this.props.actions} />
    }
};

function mapStateToProps(state) {
    return {
        myId: state.id,
        resources: getResources(state),
        storageError: state.storageError,
        inputError: state.inputError,
        selectedResource: state.selectedResource,
        storagesIds: state.storagesIds
    };
}

function getResources(state) {
    return state.resources.map(item => getResource(state, item));
};

function getResource(state, stateResource) {
    let description = getResourceDescription(state.resourceDesctiptions, stateResource.name);

    return {
        name: stateResource.name,
        count: stateResource.count,
        description: description
    };
};

function getResourceDescription(descriptions, resourceName) {
    if (descriptions) {
        return descriptions[resourceName];
    }
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators({ sendResource, dropResource, selectResource, unselectResource }, dispatch)
    };
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(AppView);