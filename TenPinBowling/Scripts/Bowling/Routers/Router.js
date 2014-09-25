// Router.js
// ----------------
define(["jquery", "backbone", "Views/InputUsersView", "Views/InputScorecardView"],

    function ($, Backbone, InputUsersView, InputScorecardView) {

        var Router = Backbone.Router.extend({

            initialize: function () {
                // Tells Backbone to start watching for hashchange events
                Backbone.history.start();
            },

            // All of your Backbone Routes (add more)
            routes: {
                // When there is no hash on the url, the home method is called
                "": "chooseUsers",
                "scorecard": "scorecardInput"
            },

            chooseUsers: function () {
                var inputUsersView = new InputUsersView({
                });

            },

            scorecardInput: function() {
                var inputScorecardView = new InputScorecardView({
                });
            }

        });

        // Returns the Router class
        return Router;
    }
);