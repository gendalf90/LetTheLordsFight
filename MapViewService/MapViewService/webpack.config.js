var path = require('path');
var uglify = require('uglifyjs-webpack-plugin');

module.exports = env => {

    var result = {
        entry: './src/main.ts',
        output: {
            path: path.join(__dirname, "./wwwroot/dist/")
        },
        resolve: {
            extensions: ['.ts', '.js']
        },
        module: {
            rules: [
                {
                    test: /\.ts$/,
                    use: ['awesome-typescript-loader', 'angular2-template-loader'],
                    include: path.resolve(__dirname, "src/")
                },
                {
                    test: /\.(png|jpg|gif)$/,
                    use: 'url-loader',
                    include: path.resolve(__dirname, "src/")
                },
                {
                    test: /\.html$/,
                    use: 'html-loader?minimize=false',
                    include: path.resolve(__dirname, "src/")
                },
                {
                    test: /\.css$/,
                    include: path.resolve(__dirname, "src/"),
                    loader: 'raw-loader'
                }
            ]
        },
    };

    result.output.filename = env.production ? 'bundle.min.js' : 'bundle.js';

    if (env.production) {
        result.plugins = [
            new uglify({
                parallel: true
            })
        ];
    };

    return result;
};