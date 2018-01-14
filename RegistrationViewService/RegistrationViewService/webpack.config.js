var path = require('path');
var uglify = require('uglifyjs-webpack-plugin');

const entryFilePath = "./src/main.jsx";
const devOutputFileName = "bundle.js";
const prodOutputFileName = "bundle.min.js";
const outputPath = "./wwwroot/dist/";
const srcPath = "src/";

module.exports = env => {

    var result = {
        entry: entryFilePath,
        output: {
            path: path.join(__dirname, outputPath)
        },
        resolve: {
            extensions: ['.js', '.jsx']
        },
        module: {
            rules: [
                {
                    test: /\.js$/,
                    use: 'babel-loader',
                    include: [path.resolve(__dirname, srcPath)]
                },
                {
                    test: /\.jsx$/,
                    use: 'babel-loader',
                    include: [path.resolve(__dirname, srcPath)]
                }
            ]
        }
    };

    result.output.filename = env.production ? prodOutputFileName : devOutputFileName;

    if (env.production) {
        result.plugins = [
            new uglify()
        ];
    };

    return result;
}