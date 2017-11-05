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
                {items.map((item, index) => <ItemView index={index} item={item} />)}
            </div>
        );
    }
};

class ItemView extends React.Component {
    render() {
        let index = this.props.index;
        let item = this.props.item;
        return (
            <div id={'tab' + index} role="tab" >
                <button data-toggle="collapse" aria-expanded="false" data-target={'#' + 'body' + index} aria-controls={'body' + index} type="button" className="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
                    <span className="font-weight-bold">{item.name}</span>
                    <span className="badge badge-secondary">{item.count}</span>
                </button>
                <div id={'body' + index} className="collapse" role="tabpanel" aria-labelledby={'tab' + index} data-parent="#accordion" >
                    {item.desc}
                </div>
            </div>
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
        desc: state.itemDescriptions.get(item.name)
    }));
};

function mapErrors(state) {
    var result = {};

    if (state.error === 403) {
        result.notAuthorized = true;
    } else if (state.error === 404) {
        result.notFound = true;
    } else if (state.error) {
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