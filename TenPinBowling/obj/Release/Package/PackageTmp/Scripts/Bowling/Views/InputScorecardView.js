// InputScorecardView.js
// -------
define(["jquery", "backbone",
        "text!Templates/InputScorecards.html",
        "text!Templates/Frame.html",
        "text!Templates/FinalFrame.html",
        "Collections/BowlingUserCollection",
        "Collections/BowlingCardCollection",
        "Models/BowlingScoreModel"
],

    function ($, Backbone,
                template,
                frameTemplate,
                finalFrameTemplate,
                BowlingUserCollection,
                BowlingCardCollection,
                BowlingScoreModel) {

        var InputScorecardView = Backbone.View.extend({

            // The DOM Element associated with this view
            el: "#defaultContainer",

            // View constructor
            initialize: function () {
                this.gameFinished = false;

                // found that event handlers would persist between reccreation of views
                this._destroyEvents();

                this.bowlingCardCollection = new BowlingCardCollection();
                //this.bowlingCardCollection.on("reset", this.drawScreen, this); // todo use this instead of success

                this.fetchBowlingCard();
            },

            fetchBowlingCard: function () {
                var view = this;

                //fetch users
                view.bowlingUserCollection = new BowlingUserCollection();
                view.bowlingUserCollection.fetch({
                    success: function () {
                        // fetch bowling card 
                        view.setDefaults();
                        view.bowlingCardCollection.fetch({
                            success: function () {
                                view.setUserTurn();
                                view.drawScreen();
                            }
                        });
                    }
                });
            },

            setDefaults: function () {
                var view = this;
                view.playerPlaying = view.bowlingUserCollection.at(0).get("UserName");
                view.playerPlayingId = view.bowlingUserCollection.at(0).get("UserId");
                view.bowl = 1;
                view.frame = 1;
            },

            setUserTurn: function () {
                var view = this;

                view.bowlingUserCollection.each(function (bowlingUserModel) {
                    console.log(bowlingUserModel);
                });

                var foundPosition = false;

                for (var frameIndex = 1, numFrame = 10; frameIndex <= numFrame; frameIndex++) {
                    view.bowlingCardCollection.each(function (bowlingCardModel) {
                        var frameModel = new Backbone.Model(bowlingCardModel.get("Frames")[frameIndex - 1]);

                        if (!foundPosition) {
                            if (frameModel.get("FirstBowl") == "") {
                                foundPosition = true;
                                view.bowl = 1;
                                view.frame = frameIndex;
                                view.playerPlayingId = bowlingCardModel.get("UserId");
                                view.playerPlaying = view.bowlingUserCollection.findWhere({ UserId: bowlingCardModel.get("UserId") }).get("UserName");
                            } else if (frameModel.get("SecondBowl") == "") {
                                if (frameModel.get("FirstBowl").toUpperCase() != "X" || frameIndex == numFrame) {
                                    foundPosition = true;
                                    view.bowl = 2;
                                    view.frame = frameIndex;
                                    view.playerPlayingId = bowlingCardModel.get("UserId");
                                    view.playerPlaying = view.bowlingUserCollection.findWhere({ UserId: bowlingCardModel.get("UserId") }).get("UserName");
                                }
                            } else if (frameModel.get("FinalBowl") == "") {
                                if (frameIndex == numFrame) {
                                    if (frameModel.get("SecondBowl").toUpperCase() == "X" || frameModel.get("SecondBowl") == "/") {
                                        foundPosition = true;
                                        view.bowl = 3;
                                        view.frame = frameIndex;
                                        view.playerPlayingId = bowlingCardModel.get("UserId");
                                        view.playerPlaying = view.bowlingUserCollection.findWhere({ UserId: bowlingCardModel.get("UserId") }).get("UserName");
                                    }
                                }
                            }
                        }

                        bowlingCardModel.set("Order", frameModel.get("Total"));
                    });
                }

                if (!foundPosition) {
                    console.log("game finished");
                    view.gameFinished = true;
                }
            },

            drawScreen: function () {
                this.render();
                this.renderUserScoreCards();

                if (this.gameFinished) {
                    this.$("#scoreInputHeader").hide();
                    this.$("#gameFinishedHeader").show();
                } else {
                    this.$("#scoreInput").focus();
                }

            },

            // Renders the view's template to the UI
            render: function () {
                var viewModel = {
                    PlayerPlaying: this.playerPlaying,
                    Bowl: this.bowl,
                    Frame: this.frame
                };
                this.$el.html(_.template(template, viewModel));
                return this;
            },

            renderUserScoreCards: function () {
                var view = this;

                console.log(view.bowlingCardCollection);
                var bowlingCardCollectionOrdered = new Backbone.Collection();
                if (view.gameFinished) {
                    view.bowlingCardCollection.sortVar = "Order";
                    bowlingCardCollectionOrdered.reset(view.bowlingCardCollection.sort({ silent: true }).models.reverse());
                    var userName = view.bowlingUserCollection.findWhere({ UserId: bowlingCardCollectionOrdered.at(0).get("UserId") }).get("UserName");
                    view.$("#winnerName").text(userName);
                } else {
                    bowlingCardCollectionOrdered.reset(view.bowlingCardCollection.models);
                }

                //for each user create score card of ten frames
                bowlingCardCollectionOrdered.each(function (bowlingCardModel) {

                    var bowlingUserModel = view.bowlingUserCollection.findWhere({ UserId: bowlingCardModel.get("UserId") });

                    var $frameRow = $("<div>", {
                        "class": "row frame-row"
                    });
                    view.$("#userScorecards").append($frameRow);

                    var $userName = $("<div>", {
                        "class": "col-md-1 username-for-row",
                        text: bowlingUserModel.get("UserName")
                    });
                    $frameRow.append($userName);

                    var usersFrameCollection = new Backbone.Collection();
                    usersFrameCollection.reset(view.bowlingCardCollection.findWhere({ UserId: bowlingUserModel.get("UserId") }).get("Frames"));

                    for (var frameIndex = 1, frameCount = 10; frameIndex <= frameCount; frameIndex++) {
                        var frameModel = usersFrameCollection.at(frameIndex - 1);
                        if (frameIndex < frameCount) {
                            $frameRow.append(_.template(frameTemplate, frameModel.toJSON()));
                        } else {
                            $frameRow.append(_.template(finalFrameTemplate, frameModel.toJSON()));
                        }
                    }
                });
            },

            // View Event Handlers
            events: {
                "click #scoreInputSubmit": "addScore"
            },

            addScore: function () {
                var view = this;
                var bowlingScoreModel = new BowlingScoreModel();

                //TODO validate score
                var score = view.$("#scoreInput").val();
                //var re = new RegExp("/\d/");
                //if (re.test(score)) {

                bowlingScoreModel.save({
                    UserId: view.playerPlayingId,
                    FrameNumber: view.frame,
                    BowlNumber: view.bowl,
                    Score: score
                }, {
                    success: function () {
                        view.bowlingCardCollection.fetch({
                            success: function () {
                                view.setUserTurn();
                                view.drawScreen();
                            }
                        });
                    }
                });
            },

            _destroyEvents: function () {
                this.undelegateEvents();
                this.$el.removeData().unbind();
            }

        });

        // Returns the View module
        return InputScorecardView;
    }
);