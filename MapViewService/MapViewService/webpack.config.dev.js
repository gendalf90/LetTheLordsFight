var path = require('path');

const output = "./wwwroot/dist/";
const src = "src/";

module.exports = {
    entry: './src/main.ts',
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
        }
}