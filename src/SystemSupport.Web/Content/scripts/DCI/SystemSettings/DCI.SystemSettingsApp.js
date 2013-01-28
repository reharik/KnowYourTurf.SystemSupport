/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 9:18 AM
 * To change this template use File | Settings | File Templates.
 */

DCI.SystemSupportApp = (function(DCI, Backbone){
    var SystemSupportApp = {};

    SystemSupportApp.systemSupportViews=function(splat,entityId, parentId,rootId){
        var routeToken = _.find(DCI.routeTokens,function(item){
            return item.route == splat;
        });
        if(!routeToken)return;

        // this is so you don't set the id to the routetoken which stays in scope
        var viewOptions = $.extend({},routeToken);
        if(entityId){
          viewOptions.url +="/"+entityId;
            viewOptions.route +="/"+entityId;
        }
        if(parentId){
          viewOptions.url +="?ParentId="+parentId;
            viewOptions.route +="/"+parentId;
        }
        if(rootId){
          viewOptions.url +="&RootId="+rootId;
            viewOptions.route +="/"+rootId;
        }
        var item = new DCI.Views[routeToken.viewName](viewOptions);

        if(routeToken.isChild){
            var hasParent = DCI.WorkflowManager.addChildView(item);
            if(!hasParent){
                DCI.WorkflowManager.cleanAllViews();
                DCI.State.set({"currentView":item});
                DCI.content.show(item);
            }
        }else{
            DCI.WorkflowManager.cleanAllViews();
            DCI.State.set({"currentView":item});
            DCI.content.show(item);
        }
        DCI.vent.bind("systemSupportMenu:pageLoaded",function(){ $(DCI.menu.el).find(".ccMenu").data("ccMenu").setMenuByRel(routeToken.route); });
    };

//    SystemSupportApp.systemSupportPermissionsViews=function(splat,name, parentName){
//        var routeToken = _.find(DCI.routeTokens,function(item){
//            return item.route == splat;
//        });
//        if(!routeToken)return;
//
//        // this is so you don't set the id to the routetoken which stays in scope
//        var viewOptions = $.extend({},routeToken);
//        viewOptions.var1 = name;
//        viewOptions.var2 = parentName;
//        if(name){
//          viewOptions.url +="?Name="+name;
//            viewOptions.route +="/"+name;
//        }
//        if(parentName){
//            viewOptions.url += viewOptions.url.indexOf("?")>-1?
//                "&ParentName="+parentName:
//                "?ParentName="+parentName;
//            viewOptions.route +="/"+parentName;
//        }
//        var item = new DCI.Views[routeToken.viewName](viewOptions);
//
//        if(routeToken.isChild){
//            var hasParent = DCI.WorkflowManager.addChildView(item);
//            if(!hasParent){
//                DCI.WorkflowManager.cleanAllViews();
//                DCI.State.set({"currentView":item});
//                DCI.content.show(item);
//            }
//        }else{
//            DCI.WorkflowManager.cleanAllViews();
//            DCI.State.set({"currentView":item});
//            DCI.content.show(item);
//        }
//
//        DCI.vent.bind("systemSupportMenu:pageLoaded",function(){ $(DCI.menu.el).find(".ccMenu").data("ccMenu").setMenuByRel(routeToken.route); });
//    };


    //show user settings and hide the menu
    SystemSupportApp.show = function(){
        var currentRoute = DCI.Routing.getCurrentRoute();
        $("#main-content").removeClass("no-left-nav");
        if(DCI.State.get("application")=="systemSupport" && currentRoute.length >0){
            DCI.vent.trigger("route", DCI.Routing.getCurrentRoute(),true);
        }
        else{
            DCI.vent.trigger("route", "userlist",true);
        }
    };

    DCI.vent.bind("form:pageLoaded grid:pageLoaded",function(formOptions){
        $("select.chosen").chosen();
        $("select.chosen").chosen().change(function(){
            $(this).trigger("onfocusout")});
        $(".dci_datePicker").live('focusin', function() {
            $(this).datepicker({ changeYear: true, changeMonth: true });
        });
        $("a[rel^='prettyPhoto']").prettyPhoto({theme: 'light_rounded', show_title:false,deeplinking:false});
    });
    
    DCI.State.bind("change:application", function(){
        if(DCI.State.get("application")!="systemSupport") SystemSupportApp.show()});
    DCI.addInitializer(function(){
        DCI.State.set({"application":"systemSupport"});
    });

    return SystemSupportApp;
})(DCI, Backbone);
