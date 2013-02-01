/**
 * Created by JetBrains RubyMine.
 * User: Owner
 * Date: 2/26/12
 * Time: 11:14 AM
 * To change this template use File | Settings | File Templates.
 */

KYT.SystemSupportApp = (function(KYT, Backbone){
    var SystemSupport = {};

    //show user settings and hide the menu
    SystemSupport.show = function(){
        SystemSupport.Menu.show();
        KYT.vent.trigger("route", "clientlist",true);
    };

    KYT.State.bind("change:application", function(e,f,g){
        if(KYT.State.get("application")=="systemsupport") {
            SystemSupport.show();
        }
    });

    KYT.addInitializer(function(){
    });

    return SystemSupport;
})(KYT, Backbone);
