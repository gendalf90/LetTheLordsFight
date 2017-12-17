var React = require("react");

class ResourceDescriptionView extends React.Component {
    render() {
        return this.renderBody(this.props.data);
    }

    renderBody(data) {
        return (
            <div className="media">
                <img className="d-flex mr-3" src={data.img} />
                <div className="media-body">
                    <h5 className="mt-0">{data.name}</h5>
                    {data.desc}
                </div>
            </div>
        );
    }
};

module.exports = ResourceDescriptionView;