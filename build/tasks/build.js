var gulp = require('gulp');
var runSequence = require('run-sequence');


gulp.task('build-all', function(done) {

    runSequence(
        'assemblyInfo',
        'compile',
        'test',
        'package',
        done);

});