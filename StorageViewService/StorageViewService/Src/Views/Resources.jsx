var React = require("react");
var ResourceDescriptionView = require("./ResourceDescription.jsx");

class ResourcesView extends React.Component {
    render() {
        return (
            <div className="list-group hidden" id="accordion" role="tablist">
                {this.renderItems()}
            </div>
        );
    }

    renderItems() {
        return this.props.items.map(item => this.renderItem(item));
    }

    renderItem(item) {
        return (
            <div id={'tab' + item.name} key={item.name} role="tab" >
                {this.renderItemTabButton(item)}
                {this.renderItemTabBody(item)}
            </div>    
        );
    }

    renderItemTabButton(item) {
        return (
            <button data-toggle="collapse" onClick={this.onExpand.bind(this)} resource-name={item.name} aria-expanded="false" data-target={'#' + 'body' + item.name} aria-controls={'body' + item.name} type="button" className="list-group-item list-group-item-action d-flex justify-content-between align-items-center">
                <span className="font-weight-bold">{item.name}</span>
                <span className="badge badge-secondary">{item.count}</span>
            </button>
        );
    }

    onExpand(e) {
        let isExpanded = e.target.getAttribute('aria-expanded') === 'true';
        let selectedResourceName = e.target.getAttribute('resource-name');

        if (isExpanded) {
            this.props.actions.setSelectedResource(selectedResourceName);
        } else {
            this.props.actions.clearSelectedResource();
        }
    }

    renderItemTabBody(item) {
        return (
            <div id={'body' + item.name} className="collapse" role="tabpanel" aria-labelledby={'tab' + item.name} data-parent="#accordion" >
                <div className="container-fluid">
                    {this.renderItemDescription(item)}
                </div>
            </div>
        );
    }

    renderItemDescription(item) {
        if (item.description) {
            return <ResourceDescriptionView data={item.description} />
        }
    }
};

module.exports = ResourcesView;