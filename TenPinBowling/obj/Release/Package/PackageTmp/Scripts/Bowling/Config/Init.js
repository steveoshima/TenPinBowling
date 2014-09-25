// to build Init.min.js -> 
// cd C:\Projects\TenPinBowling\TenPinBowling" -> 
// C:\Program Files\nodejs\node.exe" "C:\Projects\TenPinBowling\TenPinBowling\OptimizeScriptBowling.js"
// Init.js
// --------------
require.config({

    // Sets the js folder as the base directory for all future relative paths
    // Bit of pain as libs are in root Content and app another folder
    baseUrl: "/Scripts/",

    // 3rd party script alias names (Easier to type "jquery" than "libs/jquery, etc")
    // probably a good idea to keep version numbers in the file names for updates checking
    paths: {

        // Core Libraries
        // --------------
        "jquery": "jquery-1.10.2", //http://jquery.com/
        "jquery-ui": "jquery-ui", // this is folder pointing to jquery amd files 
        "underscore": "underscore-min", //http://underscorejs.org/
        "backbone": "backbone", //http://backbonejs.org
        "bootstrap": "bootstrap",

        // Plugins
        // -------
        "text": "text", //http://requirejs.org/docs/api.html#text

        // Application Folders
        // -------------------
        "Routers": "Bowling/Routers",
        "Collections": "Bowling/Collections",
        "Models": "Bowling/Models",
        "Templates": "Bowling/Templates",
        "Views": "Bowling/Views",

    },

    // Sets the configuration for your third party scripts that are not AMD compatible
    shim: {

        // Backbone
        "backbone": {
            // Depends on underscore/lodash and jQuery
            deps: ["underscore", "jquery"],

            // Exports the global window.Backbone object
            exports: "Backbone"
        },

        "bootstrap": {
            "deps": ['jquery']
        },
        
    }

});

// Includes Desktop Specific JavaScript files here (or inside of your Desktop router)
require(["jquery", "backbone", "Routers/Router", "bootstrap"],

  function ($, Backbone, Router) {

      // Instantiates a new Router instance
      window.bowlingRouter = new Router();
      
      //if someones starts to use console.log then IE wont throw errors
      if (typeof console === "undefined" || typeof console.log === "undefined") {
          console = {};
          console.log = function (msg) {
              //alert(msg);
          };
      }

      // jquery helper plugin, if more move to helpers file
      $.fn.insertAt = function (index, element) {
          var lastIndex = this.children().size();
          if (index < 0) {
              index = Math.max(0, lastIndex + 1 + index);
          }
          this.append(element);
          if (index < lastIndex) {
              this.children().eq(index).before(this.children().last());
          }
          return this;
      }
      

  }

);