var React = require("react");
var { connect } = require("react-redux");
var { bindActionCreators } = require("redux");
var actions = require("./actions.jsx");

class AppView extends React.Component {
    renderErrors() {
        let errors = this.props.errors;

        if (!errors) {
            return;
        }

        if (errors.notFound) {
            return <NotFoundView />
        } else if (errors.notAuthorized) {
            return <NotAuthorizedView />
        } else if (errors.unknown) {
            return <ErrorView />
        }
    }

    renderItems() {
        let items = this.props.items;
        let actions = this.props.actions;
        return <ItemsView items={items} actions={actions} />
    }

    render() {
        return (
            <div>
                {this.renderErrors()}
                {this.renderItems()}
            </div>
        );
    }
};

class ItemsView extends React.Component {
    render() {
        let items = this.props.items;
        let actions = this.props.actions;
        return (
            <div className="list-group hidden" id="accordion" role="tablist">
                {items.map(item => <ItemView item={item} actions={actions} />)}
            </div>
        );
    }
};

class ItemView extends React.Component {
    get Item() {
        return this.props.item;
    }

    get Actions() {
        return this.props.actions;
    }

    renderErrors() {
        let error = this.Item.error;

        if (!error) {
            return;
        }

        if (error.validation) {
            return this.renderValidationError(error.desc)
        } else if (error.unknown) {
            return this.renderUnknownError();
        }
    }

    renderValidationError(desc = 'Validation error.') {
        return (
            <div className="row">
                <div className="col alert alert-warning" role="alert">
                    {desc}
                </div>
            </div>
        );
    }

    renderUnknownError() {
        return (
            <div className="row">
                <div className="col alert alert-danger" role="alert">
                    Unknown error. Try to update.
                </div>
            </div>
        );
    }

    renderDescription() {
        return (
            <div className="row">
                <div className="col">
                    {this.Item.desc}
                </div>
            </div>
        );
    }

    renderInput() {
        return (
            <div className="row">
                <div className="col">
                    <ItemInputView item={this.Item} actions={this.Actions} />
                </div>
                <div className="col" />
            </div>
        );
    }

    renderTabButton() {
        return (
            <button data-toggle="collapse" aria-expanded="false" data-target={'#' + 'body' + this.Item.name} aria-controls={'body' + this.Item.name} type="button" className="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
                <span className="font-weight-bold">{this.Item.name}</span>
                <span className="badge badge-secondary">{this.Item.count}</span>
            </button>
        );
    }

    renderTabBody() {
        return (
            <div id={'body' + this.Item.name} className="collapse" role="tabpanel" aria-labelledby={'tab' + this.Item.name} data-parent="#accordion" >
                <div className="container-fluid">
                    {this.renderErrors()}
                    {this.renderDescription()}
                    {this.renderInput()}
                </div>
            </div>
        );
    }

    render() {
        return (
            <div id={'tab' + this.Item.name} role="tab" >
                {this.renderTabButton()}
                {this.renderTabBody()}
            </div>
        );
    }
};

class ItemInputView extends React.Component {
    sendTo(storage) {
        this.props.actions.sendItem(this.props.item.name, this.refs.countInput.value, storage);
    }

    drop() {
        this.props.actions.dropItem(this.props.item.name, this.refs.countInput.value);
    }

    render() {
        let item = this.props.item;
        let actions = this.props.actions;
        return (
            <div className="input-group">
                <div className="input-group-btn">
                    <button type="button" className="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        SendTo
                    </button>
                    <div className="dropdown-menu dropdown-menu-right">
                        {item.storages.map(storage => <StorageInputView storage={storage} send={this.sendTo.bind(this)} />)}
                        <div role="separator" className="dropdown-divider" />
                        <DropInputView drop={this.drop.bind(this)} />
                    </div>
                </div>
                <input ref="countInput" className="form-control" placeholder="Count" type="text" />
            </div>
        );
    }
};

class StorageInputView extends React.Component {
    render() {
        let storage = this.props.storage;
        let send = this.props.send;
        return (
            <button className="dropdown-item" type="button" onClick={() => send(storage)}>{storage}</button>
        );
    }
};

class DropInputView extends React.Component {
    render() {
        let drop = this.props.drop;
        return (
            <button className="dropdown-item" type="button" onClick={drop}>Drop</button>
        );
    }
};

class NotFoundView extends React.Component {
    render() {
        return (
            <div className="alert alert-warning" role="alert">
                <h4 className="alert-heading">Not found</h4>
                <p>storage not found</p>
            </div>
        );
    }
};

class NotAuthorizedView extends React.Component {
    render() {
        return (
            <div className="alert alert-warning" role="alert">
                <h4 className="alert-heading">Not authorized</h4>
                <p>you do not have privileges to access the storage</p>
            </div>
        );
    }
};

class ErrorView extends React.Component {
    render() {
        return (
            <div className="alert alert-danger" role="alert">
                <h4 className="alert-heading">Error</h4>
                <p>something went wrong</p>
            </div>
        );
    }
};

function mapItems(state) {
    return state.items.map(item => mapItem(state, item));
};

function mapItem(state, item) {
    let desc = state.itemDescriptions.get(item.name);
    let error = mapItemError(state.itemError, item.name);

    return {
        name: item.name,
        count: item.count,
        desc: desc,
        storages: state.storages,
        error: error
    };
};

function mapItemError(itemError, itemName) {
    if (itemError && itemError.name == itemName) {
        return itemError.error;
    };
}

function mapErrors(state) {
    return state.storageError;
};

function mapStateToProps(state) {
    return {
        items: mapItems(state),
        errors: mapErrors(state)
    };
}

function mapDispatchToProps(dispatch) {
    return {
        actions: bindActionCreators(actions, dispatch)
    };
}

module.exports = connect(mapStateToProps, mapDispatchToProps)(AppView);