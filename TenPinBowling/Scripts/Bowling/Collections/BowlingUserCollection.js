// BowlingUserCollection.js
// -------------
define(["jquery", "backbone"],

  function ($, Backbone) {

      // Creates a new Backbone Collection class object
      var BowlingUserCollection = Backbone.Collection.extend({

          // Tells the Backbone Collection that all of it's models will be of type Model (listed up top as a dependency)
          //model: TimeRecordModel,
          //parse: function (response) {
          //    return response;
          //},
          url: '/BowlingUsers/BowlingUsers',
          updateBatch: function (models) {
              var options = {
                  url: this.url,
                  cache: false
              };
              return Backbone.sync('update', models, options);
          },
          save: function(options) {
              var collection = this;
              options = {
                  success: function (model, resp, xhr) {
                      collection.reset(model);
                  }
              };
              return Backbone.sync('create', this, options);
          }
      });

      // Returns the Model class
      return BowlingUserCollection;

  }

);