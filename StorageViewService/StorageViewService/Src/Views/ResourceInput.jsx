var React = require("react");

class ResourceInputView extends React.Component {
    render() {
        return (
            <div className="input-group">
                <span className="input-group-addon">send</span>
                {this.renderDestinationGroup()}
                <input ref="resourceCountInput" className="form-control" placeholder="count" type="text" />
                <span className="input-group-addon">{this.props.resource.description.name}</span>
            </div>
        );
    }

    renderDestinationGroup() {
        return (
            <div className="input-group-btn">
                <button type="button" className="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    to
                </button>
                <div className="dropdown-menu dropdown-menu-right">
                    {this.renderStorages()}
                    {this.renderSeparatorIfNeeded()}
                    {this.renderDrop()}
                </div>
            </div>
        );
    }

    renderStorages() {
        return this.props.storages.map(storageId => this.renderStorage(storageId, this.sendTo.bind(this)));
    }

    renderStorage(storageId, sendAction) {
        return <button className="dropdown-item" key={storageId} type="button" onClick={() => sendAction(storageId)}>{storageId}</button>
    }

    renderSeparatorIfNeeded() {
        if (this.props.storages.length > 0) {
            return <div role="separator" className="dropdown-divider" />
        }
    }

    renderDrop() {
        return <button className="dropdown-item" type="button" onClick={this.drop.bind(this)}>Drop</button>
    }

    sendTo(storageId) {
        this.props.actions.sendResource(this.props.resource.name, this.refs.resourceCountInput.value, storageId);
    }

    drop() {
        this.props.actions.dropResource(this.props.resource.name, this.refs.resourceCountInput.value);
    }
};

module.exports = ResourceInputView;