var args = require('yargs').argv;
var path = require('path');

var config = {

    outputDir : path.resolve('./output'),
    toolsDir : path.resolve('./tools'),
    buildVersion : args.buildNumber ? '0.4.' + args.buildNumber  : undefined,
    company: 'dbones.co.uk'


};

//command config
config.command = {
    assembly: {
        copyright: 'Copyright '+ config.company +' 2013-' + new Date().getFullYear()
    },
    test:{
        dllName: '*.Test.dll'
    },
    package: {
        //dependencyNameOverride: '//x:dependency[starts-with(@x:id, \'Boxes.\')]/@x:version'
        //configFile: null
    }
}

module.exports = config;