/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 10:35 AM
 * To change this template use File | Settings | File Templates.
 */

DCI.SystemSupportApp.Menu = (function(DCI, Backbone, $){
    var Menu = {};

    Menu.show = function(){
        var routeToken = _.find(DCI.routeTokens,function(item){
            return item.id == "systemSupportMenu";
        });
        var view = new MenuView(routeToken);
        DCI.menu.show(view);
        $("#left-navigation").show();
    };

    var MenuView =  DCI.Views.View.extend({
        render:function(){
            DCI.repository.ajaxGet(this.options.url, this.options.data, $.proxy(this.renderCallback,this));
        },
        renderCallback:function(result){
            if(result.LoggedOut){
                window.location.replace(result.RedirectUrl);
                return;
            }
            $(this.el).html(result);
            DCI.vent.bind("menuItem", this.menuItemClick,this);
            $(this.el).find(".ccMenu").ccMenu({ backLink: false, width : 220 });
            DCI.vent.trigger("systemSupportMenu:pageLoaded");
            return this;
        },
        menuItemClick:function(name,val,route){
                DCI.vent.trigger("route",name,true);
        },
        onClose:function(){
            DCI.vent.unbind("menuItem");
        }
    });

    DCI.addInitializer(function(){
        DCI.SystemSupportApp.Menu.show();
    });
    return Menu;

})(DCI, Backbone, jQuery);
