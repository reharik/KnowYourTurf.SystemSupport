/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 2:01 PM
 * To change this template use File | Settings | File Templates.
 */

DCI.Routing = (function(DCI, Backbone){
  var Routing = {};

  // Public API
  // ----------

  // The `showRoute` method is a private method used to update the
  // url's hash fragment route. It accepts a base route and an
  // unlimited number of optional parameters for the route:
  // `showRoute("foo", "bar", "baz", "etc");`.
  Routing.showRoute = function(route,triggerAction){
    //var route = getRoutePath(arguments);
    Backbone.history.navigate(route, triggerAction);
  };

  // Helper Methods
  // --------------

  // Creates a proper route based on the `routeParts`
  // that are passed to it.
  var getRoutePath = function(routeParts){
    var base = routeParts[0];
    var length = routeParts.length;
    var route = base;

    if (length > 1){
      for(var i = 1; i < length; i++) {
        var arg = routeParts[i];
        if (arg){
          route = route + "/" + arg;
        }
      }
    }

    return route;
  };

  Routing.getCurrentRoute=function(){
      return window.location.hash;
  };

  return Routing;
})(DCI, Backbone);
