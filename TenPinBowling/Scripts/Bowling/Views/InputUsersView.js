// InputUsersView.js
// -------
define(["jquery", "backbone",
        "text!Templates/InputUsers.html",
        "Collections/BowlingUserCollection"
       ],

    function ($, Backbone,
                template,
                BowlingUserCollection
                ) {

        var InputUsersView = Backbone.View.extend({

            // The DOM Element associated with this view
            el: "#defaultContainer",

            // View constructor
            initialize: function () {
                // found that event handlers would persist between reccreation of views
                this._destroyEvents();
 
                // Calls the view's render methods
                this.render();
                },

            // Renders the view's template to the UI
            render: function () {
                var viewModel = {
                    Title: this.options.title
                };
                this.$el.html(_.template(template, viewModel));
                return this;
            },          

            // View Event Handlers
            events: {
                "click input#userCountSubmit": "updateUserCount",
                "click input#createScoreCard": "createScoreCard"
            },

            updateUserCount: function (e) {
                var view = this;

                view.$("#userCountSubmit").hide();
                view.$("#createScoreCard").show();

                var numberOfPlayers = parseInt(view.$("#userCount").val());

                for (var i = 1; i <= numberOfPlayers; i++) {
                    var inputId = "userCount" + i;
                    var userNameInput = $("<input>", {
                        id: inputId,
                        type: "text",
                        "class": "user-name"
                    });
                    //if more complex and time would create a new template
                    view.$("#userNames").append("Enter User " + i + " name: ");
                    view.$("#userNames").append(userNameInput);
                    view.$("#userNames").append("<br>");
                }
            },

            createScoreCard: function () {
                var view = this;

                var bowlingUserCollection = new BowlingUserCollection();
              
                view.$("#userNames").find("input.user-name").each(function(index, obj) {
                    bowlingUserCollection.add({
                        UserId: index+1,
                        UserName: obj.value
                    });
                });

                bowlingUserCollection.save();

                // go to route with score card
                bowlingRouter.navigate("scorecard", { trigger: true });
            },
            

            _destroyEvents: function () {
                this.undelegateEvents();
                this.$el.removeData().unbind();
            }

        });

        // Returns the View module
        return InputUsersView;
    }
);