/// <binding BeforeBuild='build' />
'use strict';

var gulp = require('gulp'),
    del = require('del'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify'),
    browserify = require('browserify'),
    babelify = require('babelify'),
    buffer = require('vinyl-buffer'),
    source = require('vinyl-source-stream'),
    server = require('gulp-json-srv'),
    image = require('gulp-image');

var config = {
    delPattern: '**/*',
    src: './Src/main.jsx',
    images: './Src/Img/*',
    testOutputFile: 'main.js',
    prodOutputFile: 'main.min.js',
    outputDir: './wwwroot/dist/'
};

gulp.task('clean', function () {
    del.sync(config.outputDir + config.delPattern);
});

gulp.task('image', ['clean'], function () {
    gulp.src(config.images)
        .pipe(image())
        .pipe(gulp.dest(config.outputDir));
});

gulp.task('set-dev-env', function () {
    return process.env.NODE_ENV = 'development';
});

gulp.task('development', ['clean', 'set-dev-env'], function () {
    return browserify(config.src)
        .transform(babelify.configure({
            presets: ['react', 'env'],
            plugins: ["transform-runtime"]
        }))
        .bundle()
        .pipe(source(config.testOutputFile))
        .pipe(gulp.dest(config.outputDir));
});

gulp.task('set-prod-env', ['development'], function () {
    return process.env.NODE_ENV = 'production';
});

gulp.task('production', ['set-prod-env'], function () {
    return browserify(config.src)
        .transform(babelify.configure({
            presets: ['react', 'env'],
            plugins: ["transform-runtime"]
        }))
        .bundle()
        .pipe(source(config.prodOutputFile))
        .pipe(buffer())
        .pipe(uglify({
            mangle: false
        }))
        .pipe(gulp.dest(config.outputDir));
});

gulp.task('build', ['production', 'image']);

var testServer = server.create({
    port: 25000,
    rewriteRules: {
        '/api/v1/storage/:storageId/item/:itemName/quantity/:itemCount/decrease': '/decrease',
        '/api/v1/storage/:fromStorageId/item/:itemName/quantity/:itemCount/to/:toStorageId': '/send',
        '/api/v1/map/segments/i/1/j/1': '/segments/1',
        '/api/v1/map/': '/',
        '/api/v1/': '/'
    },
    customRoutes: {
        '/decrease': {
            method: 'post',
            handler: (req, res) => res.sendStatus(200)
        },
        '/send': {
            method: 'post',
            handler: (req, res) => res.sendStatus(200)
        }
    }
});

gulp.task('test-server', function () {
    gulp.src('data.json')
        .pipe(testServer.pipe());
});
