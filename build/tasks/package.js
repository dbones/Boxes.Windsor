var gulp = require('gulp');
var request = require('request');
var mkdirp = require('mkdirp');
var Nuget = require('nuget-runner');
var glob = require("glob");
var Promise = require('es6-promise').Promise;
var path = require('path');
var config = require('../config');

var outputDir = config.outputDir;
var toolsDir = config.toolsDir;
var nugetDir = toolsDir + '/nuget';
var runnerFileName =  nugetDir + '/nuget.exe';


var defaultSettings = {
    nugetPath:  runnerFileName ,
    verbosity: 'detailed'
};

if(config.command.package.configFile != null) {
    // The NuGet configuation file. If not specified, file
    // %AppData%\NuGet\NuGet.config is used as configuration file.
    //configFile: 'path/to/nuget.config'
    defaultSettings.configFile = config.command.package;
}

var nuget = Nuget(defaultSettings);




gulp.task('package', ['get-nuget'], function(done) {

    mkdirp.sync(outputDir);

	glob('./**/*.nuspec', function (er, files) {
		var promise = new Promise(function(resolve) { resolve(); });
		files.forEach(function(file) {
			if(file.indexOf("packages") > -1) return;

			var run = function(){

                var nextPromise = new Promise(function(resolve, reject) {

                    //TODO: custom packages repo
                    var arg = {
                        spec: path.resolve(file),
                        outputDirectory: outputDir,
                        basePath: path.resolve('./')
                    };

                    if(config.buildVersion) {
                        arg.version = config.buildVersion;
                    }

                    nuget.pack(arg)
                    .then(function(){
                        console.log('done');
                        resolve();
                    })
                    .fail(function(error) {
                        console.log(error.message);
                        reject(error.message);
                    });

                });
                return nextPromise;
            };

			promise = promise.then(function() {return run();});
		});
		promise.then(function(){ done(); });
	});

});

gulp.task('restore-dependencies', ['get-nuget'], function(done) {

    mkdirp.sync(outputDir);

    //there should only be one for the solution
    glob('./**/*.sln', function (er, files) {
        var promise = new Promise(function(resolve) { resolve(); });
        files.forEach(function(file) {
            //if(file.indexOf("packages") > -1 && file.indexOf("packages.config") == -1) return;

            var run = function(){

                var nextPromise = new Promise(function(resolve, reject) {

                    var arg = {

                        // Specify the solution path or path to a packages.config file.
                        packages: path.resolve(file),

                        // A list of packages sources to use for the install.
                        // Can either be a path, url or config value.
                        //source: ['http://mynugetserver.org', 'path/to/source', 'ConfigValue'],

                        noCache: true,
                        requireConsent: false,
                        disableParallelProcessing: false,
                        verbosity: 'detailed'
                        //configFile: 'path/to/nuget.config'

                    };

                    nuget.restore(arg)
                        .then(function(){
                            console.log('done');
                            resolve();
                        })
                        .fail(function(error) {
                            console.log(error.message);
                            reject(error.message);
                        });

                });
                return nextPromise;
            };

            promise = promise.then(function() {return run();});
        });
        promise.then(function(){ done(); });
    });

});