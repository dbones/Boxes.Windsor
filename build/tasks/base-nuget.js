var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var mkdirp = require('mkdirp');
var path = require('path');
var config = require('../config');
var xmlpoke = require('xmlpoke');

var downloadLocation= 'http://nuget.org/nuget.exe';
var toolsDir = config.toolsDir;
var nugetDir = toolsDir + '/nuget';
var runnerFileName =  nugetDir + '/nuget.exe';


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
    var nugetExists = fs.existsSync(runnerFileName);

    if(nugetExists){
        done();
        return;
    }

    console.log('getting nuget');

    request(downloadLocation)
        .pipe(fs.createWriteStream(runnerFileName))
        .on('finish', function () { done(); });
});