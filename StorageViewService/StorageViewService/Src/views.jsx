var React = require("react");
var { connect } = require("react-redux");

class AppView extends React.Component {
    render() {
        let items = this.props.items;
        let errors = this.props.errors;
        return (
            <div>
                <NotFoundView visible={errors.notFound} />
                <NotAuthorizedView visible={errors.notAuthorized} />
                <ErrorView visible={errors.unknown} />
                <ItemsView items={items} />
            </div>
        );
    }
};

class ItemsView extends React.Component {
    render() {
        let items = this.props.items;
        return (
            <div className="list-group hidden" id="accordion" role="tablist">
                {items.map(item => <ItemView item={item} />)}
            </div>
        );
    }
};

class ItemView extends React.Component {
    get Item() {
        return this.props.item;
    }

    renderError() {
        if (this.Item.error) {
            return (
                <div className="row">
                    <div className="col alert alert-danger" role="alert">
                        Some kind error. Try to update.
                    </div>
                </div>
            );
        }
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
                    <ItemInputView item={this.Item} />
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
                    {this.renderError()}
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
    render() {
        let item = this.props.item;
        return (
            <div className="input-group">
                <div className="input-group-btn">
                    <button type="button" className="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        SendTo
                    </button>
                    <div className="dropdown-menu dropdown-menu-right">
                        {item.storages.map(storage => <StorageInputView storage={storage} />)}
                        <div role="separator" className="dropdown-divider" />
                        <DropInputView item={item} />
                    </div>
                </div>
                <input type="text" className="form-control" placeholder="Count" />
            </div>
        );
    }
};

class StorageInputView extends React.Component {
    render() {
        let storage = this.props.storage;
        return (
            <button className="dropdown-item" type="button">{storage}</button>
        );
    }
};

class DropInputView extends React.Component {
    render() {
        let item = this.props.item;
        return (
            <button className="dropdown-item" type="button">Drop</button>
        );
    }
};

class NotFoundView extends React.Component {
    render() {
        let visible = { display: this.props.visible ? 'block' : 'none' };
        return (
            <div className="alert alert-warning" role="alert" style={visible}>
                <h4 className="alert-heading">Not found</h4>
                <p>storage not found</p>
            </div>
        );
    }
};

class NotAuthorizedView extends React.Component {
    render() {
        let visible = { display: this.props.visible ? 'block' : 'none' };
        return (
            <div className="alert alert-warning" role="alert" style={visible}>
                <h4 className="alert-heading">Not authorized</h4>
                <p>you do not have privileges to access the storage</p>
            </div>
        );
    }
};

class ErrorView extends React.Component {
    render() {
        let visible = { display: this.props.visible ? 'block' : 'none' };
        return (
            <div className="alert alert-danger" role="alert" style={visible}>
                <h4 className="alert-heading">Error</h4>
                <p>something went wrong</p>
            </div>
        );
    }
};

function mapItems(state) {
    return state.items.map(item => ({
        name: item.name,
        count: item.count,
        desc: state.itemDescriptions.get(item.name),
        storages: state.storages,
        error: state[item.name + 'error']
    }));
};

function mapErrors(state) {
    var result = {};

    if (state.storageError === 403) {
        result.notAuthorized = true;
    } else if (state.storageError === 404) {
        result.notFound = true;
    } else if (state.storageError) {
        result.unknown = true;
    }

    return result;
};

function mapStateToProps(state) {
    return {
        items: mapItems(state),
        errors: mapErrors(state)
    };
}

module.exports = connect(mapStateToProps)(AppView);