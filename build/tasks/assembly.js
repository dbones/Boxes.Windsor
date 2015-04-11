var gulp = require('gulp');
var assemblyInfo = require('gulp-dotnet-assembly-info');
var config = require('../config');

gulp.task('assemblyInfo', function() {
    return gulp
        .src('**/AssemblyInfo.cs')
        .pipe(assemblyInfo({
            version: config.buildVersion,
            fileVersion: config.buildVersion,
            copyright: config.command.assembly.copyright
        }))
        .pipe(gulp.dest('.'));
});