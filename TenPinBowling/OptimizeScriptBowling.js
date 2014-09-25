// Node.js - Require.js Build Script

// Install nodejs first from here http://nodejs.org/

// To run the build type the following: node UserReportFilterOptimizeScript.js

// Loads the Require.js Optimizer
var requirejs = require('./Scripts/r.js');

// Sets up the basic configuration
var baseConfig = {

    // All modules are located relative to this path
    baseUrl: "./Scripts/",

    // Sets path names and paths for modules relative to the baseUrl
    paths: {
        "DashInit": "Dash/config/Init",
    },

    // Wraps all scripts in an IIFE (Immediately Invoked Function Expression)
    // (function() { + content + }());
    wrap: true,

    // The optimized build file will use almond.js (AMD shim) instead of the larger Require.js
    name: "almond",

    // Removes third-party license comments
    preserveLicenseComments: false,

    // Uses uglify.js for minification
    optimize: "uglify"

};

// Creates an array of build configs, the baseConfig will
// be mixed into both the mobile and desktop builds below.

var configs = [
    {
        // Tells Require.js to look at Init.js for all mobile shim and path configurations
        mainConfigFile: "./Scripts/Bowling/config/Init.js",

        // Points to Init.js (Remember that "UserReportFilterInit" is the module name for Init.js)
        include: ["BowlingInit"],

        // The optimized mobile build file will put into the app directory
        out: "./Scripts/Bowling/config/Init.min.js"
    }
];

// Function used to mix in baseConfig to a new config target
function mix(target) {
    for (var prop in baseConfig) {
        if (baseConfig.hasOwnProperty(prop)) {
            target[prop] = baseConfig[prop];
        }
    }
    return target;
}

//Create a runner that will run a separate build for each item
//in the configs array. Thanks to @jwhitley for this cleverness
var runner = configs.reduceRight(function (prev, currentConfig) {
    return function (buildReportText) {
        requirejs.optimize(mix(currentConfig), prev);
    };

}, function (buildReportText) {

    console.log(buildReportText);

});

console.log("Building Bowling... this might take a few seconds");

//Run the builds
runner();
