var path = require('path');
var webpack = require('webpack');
var clean = require('clean-webpack-plugin');
var uglify = require('uglifyjs-webpack-plugin');

const output = "./wwwroot/dist/";

module.exports = {
    entry: './src/testthree.ts',
    output: {
        path: path.join(__dirname, output),
        filename: 'bundle.js'
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
          rules: [
            {
              test: /\.ts$/,
              use: ['awesome-typescript-loader', 'angular2-template-loader'],
              exclude: [path.resolve(__dirname, 'node_modules')]
            },
            {
              test: /\.(png|jpg|gif)$/,
              use: 'url-loader'
            },
            {
              test: /\.html$/,
              use: 'html-loader?minimize=false'
            }
          ]
        },
    plugins: [
        new clean(output)
    ]
}