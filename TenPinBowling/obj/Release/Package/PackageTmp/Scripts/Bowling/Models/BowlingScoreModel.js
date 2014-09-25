// BowlingScoreModel.js
// --------
define(["jquery", "backbone"],

    function ($, Backbone) {

        // Creates a new Backbone Model class object
        var BowlingScoreModel = Backbone.Model.extend({

            // Model Constructor
            initialize: function () {

            },

            // Default values for all of the Model attributes
            defaults: {
            },

            parse: function (response) {

            },

            // Get's called automatically by Backbone when the set and/or save methods are called (Add your own logic)
            validate: function (attrs) {

            },

            urlRoot: '/Bowling/BowlingScore', // urlRoot allows a model outside of a collection to pass the id in the request

        });

        // Returns the Model class
        return BowlingScoreModel;

    }

);