var msbuild = require('gulp-msbuild');
var gulp = require('gulp');

gulp.task('compile', function() {
    return gulp
        .src('**/*.sln')
        //.pipe(msbuild());
		.pipe(msbuild({
            //targets: ['Clean', 'Release'],
            errorOnFail: true,
            stdout: true
        }));
});