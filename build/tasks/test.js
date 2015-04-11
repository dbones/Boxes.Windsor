var nunit = require('gulp-nunit-runner');
var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var unzip = require('unzip');
var mkdirp = require('mkdirp');
var config = require('../config');

var nunitLocation= 'http://github.com/nunit/nunitv2/releases/download/2.6.4/NUnit-2.6.4.zip';
var toolsDir = config.toolsDir;
var outputDir = config.outputDir +'/test';
var nunitDir = toolsDir + '/nunit';
var zipFileName = toolsDir + '/nunit.zip';
var runnerFileName =  nunitDir + '/NUnit-2.6.4/bin/nunit-console.exe';


gulp.task('test', ['get-nunit'], function () {

	mkdirp.sync(outputDir);

	var setup = {

		executable: runnerFileName,

		// The options below map directly to the NUnit console runner. See here
		// for more info: http://www.nunit.org/index.php?p=consoleCommandLine&r=2.6.3
		options: {

			// Name of XML result file (Default: TestResult.xml)
			result: 'test-results.xml',

			// Suppress XML result output.
			noresult: false,

			// Work directory for output files.
			work: outputDir,

			// Label each test in stdOut.
			labels: true

			//// Set internal trace level.
			//trace: 'Off|Error|Warning|Info|Verbose',

			// Framework version to be used for tests.
			//framework: 'net-4.0'
		}

	};


    return gulp
        .src(['**/bin/**/' + config.command.test.dllName], { read: false })
        .pipe(nunit(setup));
});


gulp.task('get-nunit', ['nunit-unzip'], function(done) {
	done();
});

gulp.task('nunit-unzip', ['nunit-download'], function(done) {

	var file = runnerFileName;
	var fileExists = fs.existsSync(file);

	if(fileExists) {
		done();
		return;
	}
	
	fs.createReadStream(zipFileName)
		.pipe(unzip.Extract({ path: nunitDir }))
		.on('close', function () {done();});
});


gulp.task('nunit-download', function(done) {

	mkdirp.sync(toolsDir);

	var file = zipFileName;
	var zipExists = fs.existsSync(nunitDir);

	if(zipExists) {
		done();
		return;
	}

	request(nunitLocation)
		.pipe(fs.createWriteStream(file))
		.on('finish', function () { done(); });

});