var React = require("react");
var { connect } = require("react-redux");
var { bindActionCreators } = require("redux");
var ErrorView = require("./Error.jsx");
var ResourcesView = require("./Resources.jsx");
var ResourceInputVew = require("./ResourceInput.jsx");
var sendResource = require("../Actions/SendResource.js");
var dropResource = require("../Actions/DropResource.js");
var setSelectedResource = require("../Actions/SetSelectedResource.js");
var clearSelectedResource = require("../Actions/ClearSelectedResource.js");

class AppView extends React.Component {
    render() {
        return (
            <div>
                <div className="container-fluid">
                    <div className="row">
                        <div className="col-2">
                        </div>
                        <div className="col-8">
                            {this.renderStorageError()}
                            {this.renderResources()}
                        </div>
                        <div className="col-2">
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    renderResourceInput() {
        let resource = this.props.selectedResource;
        let storages = this.props.storagesIds;
        let actions = this.props.actions;

        if (resource && storagesIds && actions) {
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
        let resources = this.props.resources;
        let actions = this.props.actions;

        if (resources && actions) {
            return <ResourcesView items={resources} actions={actions} />
        }
    }
};

function mapStateToProps(state) {
    return {
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
        actions: bindActionCreators({ sendResource, dropResource, setSelectedResource, clearSelectedResource }, dispatch)
    };
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(AppView);