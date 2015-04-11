var gulp = require('gulp');
var runSequence = require('run-sequence');


gulp.task('build-all', function(done) {

    runSequence(
        ['assemblyInfo', 'restore-dependencies'],
        'compile',
        //'test',
        'package',
        done);

});