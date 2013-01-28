/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 9:28 AM
 * To change this template use File | Settings | File Templates.
 */
DCI.Routing.SystemSupportApp = (function(DCI, Backbone){
    var SystemSupportApp = {};

    // Router
    // ------
    SystemSupportApp.Router = Backbone.Marionette.AppRouter.extend({
          appRoutes: {
//              "*path/permissions/:name": "systemSupportPermissionsViews",
//              "*path/permissions/:name/:parentId": "systemSupportPermissionsViews",
              "*path/:entityId/:parentId/:rootId": "systemSupportViews",
              "*path/:entityId/:parentId": "systemSupportViews",
              "*path/:entityId": "systemSupportViews",
              "*path": "systemSupportViews"
          }
      });

    // Initialization
    // --------------

    // Initialize the router when the application starts
    DCI.addInitializer(function(){
        SystemSupportApp.router = new SystemSupportApp.Router({
            controller: DCI.SystemSupportApp
        });
    });

    return SystemSupportApp;
})(DCI, Backbone);
