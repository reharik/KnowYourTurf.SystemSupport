/**
 * Created with JetBrains WebStorm.
 * User: Rharik
 * Date: 1/9/13
 * Time: 8:56 AM
 * To change this template use File | Settings | File Templates.
 */


KYT.Routing.SystemSupportApp = (function(KYT, Backbone){
    var SystemSupportApp = {};

    // Router
    // ------
    SystemSupportApp.Router = Backbone.Marionette.AppRouter.extend({
        appRoutes: {
            "*path/:entityId/:parentId/:rootId/:string": "showViews",
            "*path/:entityId/:parentId/:rootId": "showViews",
            "*path/:entityId/:parentId": "showViews",
            "*path/:entityId": "showViews",
            "*path": "showViews"
        }
    });

    // Initialization
    // --------------

    // Initialize the router when the application starts
    KYT.addInitializer(function(){
        KYT.SystemSupportApp.router = new KYT.Routing.SystemSupportApp.Router({
            controller: KYT.Controller
        });
    });

    return SystemSupportApp;
})(KYT, Backbone);
