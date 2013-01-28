
var DCI = new Backbone.Marionette.Application();


DCI.addRegions({
    header: "#main-header",
    content:"#contentInner",
    menu:"#left-navigation"
});

  // an initializer to run this functional area
  // when the app starts up
DCI.addInitializer(function(){
  $.ajaxSetup({
        cache: false,
        complete:function(){dci.showThrob = false; $("#ajaxLoading").hide();},
        beforeSend:function(){setTimeout(function() {if(dci.showThrob) $("#ajaxLoading").show(); }, 500)}
    });
    $("#ajaxLoading").hide();

    jQuery.validator.addMethod("number", function(value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
    }, "Please enter a valid number.");

    DCI.vent.bind("route",function(route,triggerRoute){
        DCI.Routing.showRoute(route,triggerRoute);
    });
    DCI.notificationService = new cc.MessageNotficationService();
});

DCI.bind("initialize:after", function(){
  if (Backbone.history){
    Backbone.history.start();
  }
});

// calling start will run all of the initializers
// this can be done from your JS file directly, or
// from a script block in your HTML
$(function(){
  DCI.start();
});