/// <binding BeforeBuild='build' />
'use strict';

var gulp = require("gulp"),
    del = require("del"),
    concat = require("gulp-concat"),
    uglify = require("gulp-uglify"),
    browserify = require("browserify"),
    babelify = require("babelify"),
    buffer = require('vinyl-buffer'),
    source = require("vinyl-source-stream"),
    server = require('gulp-json-srv');

var config = {
    delPattern: '**/*',
    src: './Src/main.jsx',
    testOutputFile: 'main.js',
    prodOutputFile: 'main.min.js',
    outputDir: './wwwroot/dist/'
};

gulp.task('clean', function () {
    del.sync(config.outputDir + config.delPattern);
});

gulp.task('test', ['clean'], function () {
    return browserify(config.src)
        //.external(['react', 'react-dom', 'redux', 'react-redux', 'redux-thunk'])
        .transform(babelify.configure({
            presets: ['react']
        }))
        .bundle()
        .pipe(source(config.testOutputFile))
        .pipe(gulp.dest(config.outputDir));
});

gulp.task('production', ['clean'], function () {
    return browserify(config.src)
        .transform(babelify.configure({
            presets: ['react', 'env']
        }))
        .bundle()
        .pipe(source(config.prodOutputFile))
        .pipe(buffer())
        .pipe(uglify())
        .pipe(gulp.dest(config.outputDir));
});

gulp.task('build', ['test', 'production']);

var testServer = server.create({
    port: 25000,
    baseUrl: '/api/v1',
    //rewriteRules: {
    //    '/': '/api/',
    //    '/blog/:resource/:id/show': '/api/:resource/:id'
    //},
    //customRoutes: {
    //    '/api/v1/storage/:storage': {
    //        method: 'get',
    //        handler: (req, res) => res.json({
    //            "storage": req.params.storage,
    //            "items": [
    //                {
    //                    "name": "iron",
    //                    "count": 20,
    //                    "desc": "It is iron"
    //                }
    //            ]
    //        })
    //    }
    //}
});

gulp.task('test-server', function () {
    gulp.src('data.json')
        .pipe(testServer.pipe());
});
