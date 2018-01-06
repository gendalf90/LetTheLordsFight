var path = require('path');
var webpack = require('webpack');
var uglify = require('uglifyjs-webpack-plugin');

const output = "./wwwroot/dist/";
const src = "src/";

module.exports = {
    entry: './src/main.ts',
    output: {
        path: path.join(__dirname, output),
        filename: 'bundle.min.js'
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                use: ['awesome-typescript-loader', 'angular2-template-loader'],
                include: path.resolve(__dirname, src)
            },
            {
                test: /\.(png|jpg|gif)$/,
                use: 'url-loader',
                include: path.resolve(__dirname, src)
            },
            {
                test: /\.html$/,
                use: 'html-loader?minimize=false',
                include: path.resolve(__dirname, src)
            },
            {
                test: /\.css$/,
                include: path.resolve(__dirname, src),
                loader: 'raw-loader'
            }
        ]
    },
    plugins: [
        new uglify({
            parallel: true
        })
    ]
}