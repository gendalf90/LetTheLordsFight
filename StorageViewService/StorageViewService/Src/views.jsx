var React = require("react");
var connect = require("react-redux").connect;

class AppView extends React.Component {

    render() {
        const items = this.props.items;
        const list = items.map((item, index) =>
            <ItemView index={index} item={item} />
        );

        return (
            <div class="list-group" id="accordion" role="tablist">
                {list}
            </div>
        );
    }
};

class ItemView extends React.Component {

    render() {
        const index = this.props.index;
        const item = this.props.item;
        return (
            <div id={'tab' + index} role="tab" >
                <button data-toggle="collapse" aria-expanded="false" data-target={'#' + 'body' + index} aria-controls={'body' + index} type="button" class="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
                    <span class="font-weight-bold">{item.name}</span>
                    <span class="badge badge-secondary">{item.count}</span>
                </button>
                <div id={'body' + index} class="collapse" role="tabpanel" aria-labelledby={'tab' + index} data-parent="#accordion" >
                    {item.desc}
                </div>
            </div>
        );
    }
};

function mapStateToProps(state) {
    return {
        items: state.items
    };
}

module.exports = connect(mapStateToProps)(AppView);