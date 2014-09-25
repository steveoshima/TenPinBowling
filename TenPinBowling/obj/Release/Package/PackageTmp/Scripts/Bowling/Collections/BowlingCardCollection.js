// BowlingCardCollection.js
// -------------
define(["jquery", "backbone"],

  function ($, Backbone) {

      // Creates a new Backbone Collection class object
      var BowlingCardCollection = Backbone.Collection.extend({

          initilize: function() {
              this.sortVar = 'UserId';
          },
          // Tells the Backbone Collection that all of it's models will be of type Model (listed up top as a dependency)
          //model: BowlingCard,
          //parse: function (response) {
          //    return response;
          //},
          url: '/Bowling/BowlingCards',
          updateBatch: function (models) {
              var options = {
                  url: this.url,
                  cache: false
              };
              return Backbone.sync('update', models, options);
          },

          comparator: function (collection) {
              var that = this;
              return (collection.get(that.sortVar));
          }

      });

      // Returns the Model class
      return BowlingCardCollection;

  }

);