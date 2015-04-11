var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var mkdirp = require('mkdirp');
var Nuget = require('nuget-runner');
var glob = require("glob");
var Promise = require('es6-promise').Promise;
var path = require('path');
var args = require('yargs').argv;
var config = require('../config');
var xmlpoke = require('xmlpoke');

var downloadLocation= 'http://nuget.org/nuget.exe';
var outputDir = config.outputDir;
var toolsDir = config.toolsDir;
var nugetDir = toolsDir + '/nuget';
var runnerFileName =  nugetDir + '/nuget.exe';

gulp.task('package', ['get-nuget', 'nuegt-set-dependency-version'], function(done) {

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

	glob('./**/*.nuspec', function (er, files) {
		var promise = new Promise(function(resolve) { resolve(); });
		files.forEach(function(file) {
			if(file.indexOf("packages") > -1) return;

			var run = function(){

                var nextPromise = new Promise(function(resolve, reject) {

                    console.log(nuget);
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


gulp.task('nuegt-set-dependency-version', function(done){
    //xmlpoke

    if(config.buildVersion == null
        || config.command.package == null
        || config.command.package.dependencyNameOverride == null
        || config.command.package.dependencyNameOverride == '') {
        done();
        return;
    }

    xmlpoke('./**/*.nuspec', function(xml) {
        xml
            .addNamespace('x', 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd')
            .set(config.command.package.dependencyNameOverride, config.buildVersion);
    });

    done();

});

gulp.task('get-nuget',  function(done) {

	mkdirp.sync(nugetDir);
	var zipExists = fs.existsSync(nugetDir);

	if(zipExists){
		done();
        return;
	}

    console.log('getting nuget');

	request(downloadLocation)
		.pipe(fs.createWriteStream(runnerFileName))
		.on('end', function () { done(); });
});