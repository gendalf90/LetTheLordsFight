var React = require("react");

class ResourceInputView extends React.Component {
    render() {
        return (
            <div className="input-group">
                <div className="input-group-btn">
                    <button type="button" className="btn btn-secondary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        SendTo
                    </button>
                    <div className="dropdown-menu dropdown-menu-right">
                        {this.renderStorages()}
                        <div role="separator" className="dropdown-divider" />
                        {this.renderDrop()}
                    </div>
                </div>
                <input ref="resourceCountInput" className="form-control" placeholder="Count" type="text" />
            </div>
        );
    }

    renderStorages() {
        return this.props.storages.map(storageId => this.renderStorage(storageId, this.sendTo.bind(this)));
    }

    renderStorage(storageId, sendAction) {
        return <button className="dropdown-item" type="button" onClick={() => sendAction(storageId)}>{storageId}</button>
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