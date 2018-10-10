/// <binding BeforeBuild='clean' AfterBuild='min-sass, copy' />
var gulp = require('gulp'),
    fs = require("fs"),
    del = require("del"),
    sass = require('gulp-sass')
    cssmin = require("gulp-cssmin")
    rename = require("gulp-rename");

var paths = {
    webroot: "./wwwroot/",
    node_modules: "./node_modules/",
    lib: "./wwwroot/lib/"
};

gulp.task('clean', function () {
    del([paths.lib + "*"]);
});

gulp.task("copy", function () {
    var npm = {
        "font-awesome/": {
            "font-awesome-5-css/webfonts/*": "webfonts",
            "font-awesome-5-css/css/fontawesome-all.min.css": "css"
        },
        "purecss/": {
            "purecss/build/pure-min.css": "",
            "purecss/build/grids-responsive-min.css": ""
        }
     }
  
    for (var destinationDir in npm) {
        for (var subdir in npm[destinationDir]) {
            gulp.src(paths.node_modules + subdir)
                .pipe(gulp.dest(paths.lib + destinationDir + npm[destinationDir][subdir]));
        }        
    }
});

gulp.task('min-sass', function () {
    return gulp.src('assets/scss/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cssmin())
        .pipe(rename({
            suffix: ".min"
        }))
        .pipe(gulp.dest(paths.webroot + '/css'));
});