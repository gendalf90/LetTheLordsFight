/// <binding BeforeBuild='build' />
'use strict';

var gulp = require("gulp"),
    del = require("del"),
    concat = require("gulp-concat"),
    babel = require('gulp-babel'),
    uglify = require("gulp-uglify");

var src = [
    './Src/app.jsx',
    './Src/test.js'
];
const delPattern = '**/*';
const testBabelSettings = { presets: ['react'] };
const prodBabelSettings = { presets: ['react', 'env'] };
const testResultFileName = 'main.js';
const prodResultFileName = 'main.min.js';
const distPath = './wwwroot/dist/';

gulp.task('clean', function () {
    del.sync(distPath + delPattern);
});

gulp.task('test', ['clean'], function () {
    return gulp.src(src)
        .pipe(babel(testBabelSettings))
        .pipe(concat(testResultFileName))
        .pipe(gulp.dest(distPath));
});

gulp.task('production', ['clean'], function () {
    return gulp.src(src)
        .pipe(babel(prodBabelSettings))
        .pipe(concat(prodResultFileName))
        .pipe(uglify())
        .pipe(gulp.dest(distPath));
});

gulp.task('build', ['test', 'production']);
