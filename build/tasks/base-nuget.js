var gulp = require('gulp');
var request = require('request');
var fs = require('fs');
var mkdirp = require('mkdirp');
var path = require('path');
var config = require('../config');
var xmlpoke = require('xmlpoke');
var glob = require("glob");
var Promise = require('es6-promise').Promise;
var xml2js = require('xml2js');
var eyes = require('eyes');

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



gulp.task('nuget-sync-dependency-version', function(done){

    //ugly code
    //find nuspec and package files
    var packageFile = function(projectName){
        projectName = path.basename(projectName, '.nuspec');
        return  new Promise(function(resolve, reject){
            var toFind = './**/' + projectName + '/packages.config';
            console.log(toFind);
            glob(toFind, function(er, files){
                if(files.length === 0){
                    resolve();
                } else {
                    resolve(files[0]);
                }
            });

        });
    };

    var projectFile = function(projectName){
        projectName = path.basename(projectName, '.nuspec');
        return new Promise(function(resolve, reject){
            var toFind = './**/' + projectName + '/'+ projectName +'.*proj';
            glob(toFind, function(er, files){
                if(files.length === 0){
                    resolve();
                } else {
                    console.log(files);
                    resolve(files[0]);
                }
            });

        });
    };

    var specFiles = function(){
        return new Promise(function(resolve, reject) {
            glob('./**/*.nuspec', function (er, files) {
                resolve(files);
            });
        });
    };

    var getAll= function(files, delegte){
        var promises = [];

        files.forEach(function(file){
            promises.push(delegte(file));
        });
        console.log(promises);

        return Promise.all(promises);
    };

    var packageDependencies = function(ctx){
        return new Promise(function(resolve, reject) {
            console.log(ctx.package);

            var parser = new xml2js.Parser();

            parser.on('end', function(cfg) {
                cfg.packages.package.forEach(function(p){

                    var target = p.$.targetFramework;
                    var key;

                    if (target == null || target === '') {
                        key = 'default'
                    }
                    else {
                        key = target;
                    }
                    if (ctx.dependencies[key] == null) {
                        ctx.dependencies[key] = [];
                    }

                    var dependency = {
                        id: p.$.id,
                        version: p.$.version
                    };

                    ctx.dependencies[key].push(dependency);

                });
                resolve(ctx);
            });

            fs.readFile(ctx.package, function(err, data) {
                parser.parseString(data);
            });

        });
    };

    var projectDependencies = function(ctx){
        return new Promise(function(resolve, reject) {

            var parser = new xml2js.Parser();

            parser.on('end', function(result) {
                //console.log(result);
                eyes.inspect(result);
                resolve(ctx);
            });

            fs.readFile(ctx.project, function(err, data) {
                parser.parseString(data);
            });

            /*
            xmlreader.read(ctx.project, function (err, cfg) {
                if (err) {
                    reject(err);
                    return;
                }

                cfg.Project.ItemGroup.each(function (i, ig) {

                    ig.ProjectReference.each(function (i, pr) {

                        var key = 'default';

                        if (ctx.dependencies[key] == null) {
                            ctx.dependencies[key] = [];
                        }

                        var dependency = {
                            id: pr.Name.text(),
                            version: config.buildVersion
                        };

                        ctx.dependencies[key].push(dependency);

                    });

                });
                resolve(ctx);
            });*/
        });
    };



    var specs = specFiles();
    var packages = specs.then(function(files) {return getAll(files, packageFile)}).then((function(f){console.log('done');return f;}));
    var projects = specs.then(function(files) {return getAll(files, projectFile)}).then((function(f){console.log('done');return f;}));


    Promise.all([specs, packages, projects]).then(function(results){
        console.log('listed files');
        var items=[];
        for(var i = 0; i < results[0].length; i++){
            items.push({
                nuspec : results[0][i],
                package : results[1][i],
                project : results[2][i],
                dependencies: {}
            });
        }
        return items;
    }).then(function(results){
        console.log('created contexts');
        //clear dependencies
        results.forEach(function(result){
            xmlpoke(result.nuspec, function(xml) {
                xml
                    .addNamespace('x', 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd')
                    .clear('//x:dependencies');
            });
        });
        return results;
    }).then(function(results){
        //get dependecies
        console.log('cleared dependecies');
        var getPackageDependencies = getAll(results, packageDependencies);
        var getProjectDependencies = getAll(results, projectDependencies);

        return Promise.all([getPackageDependencies, getProjectDependencies]).then(function(){return results;});

    }).then(function(results) {
        console.log('extracted new dependecies');
        //set dependencies
        results.forEach(function(ctx){

            xmlpoke(ctx.nuspec, function(xml) {
                xml.addNamespace('x', 'http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd');
                for(var propertyName in ctx.dependencies) {
                    var group = propertyName === 'default' ? 'x:group' : 'x:group[@x:targetFramework=\''+ propertyName +'\']';
                    xml.ensure('x:package/x:metadata/x:dependencies/' + group);

                    ctx.dependencies[propertyName].forEach(function(dependency){
                        xml.add('//x:dependencies/' + group + '/dependency', {
                            '@id': dependency.id,
                            '@version': dependency.version
                        });
                    });
                }
            });
        }).catch(function(er){console.log(er);});
        done();
    });
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



function defer(){

    var d = {};
    d.promise = new Promise(function(rl, rj) {
        d.resolve = rl;
        d.reject = rj;
    });

    //return d;
}