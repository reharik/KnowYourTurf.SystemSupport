KYT.SystemSupportApp.Menu = (function(KYT, Backbone, $){
    var Menu = {};

    Menu.show = function(){
        var routeToken = _.find(KYT.routeTokens,function(item){
            return item.id == "systemSupportMenu";
        });
        var view = new MenuView(routeToken);
        KYT.menu.show(view);
        $("#left-navigation").show();
    };

    var MenuView =  KYT.Views.View.extend({
        render:function(){
            KYT.repository.ajaxGet(this.options.url, this.options.data).done($.proxy(this.renderCallback,this));
        },
        renderCallback:function(result){
            var that = this;
            $(this.el).html(result);
            KYT.vent.bind("menuItem", this.menuItemClick,this);
            $(this.el).find(".ccMenu").ccMenu({ backLink: false, width : 220 });

            KYT.State.bind("change:ClientId", function(e,f,g){
                that.toggleMenu(KYT.State.get("ClientId"));
            });

            return this;
        },
        toggleMenu:function(clientId){
            if(clientId>0){
                $("#left-navigation").show();
            }else{
                $("#left-navigation").hide();
            }
        },
        menuItemClick:function(name){
            KYT.vent.trigger("route",name,true);
        },
        onClose:function(){
            KYT.vent.unbind("menuItem");
        }
    });

    return Menu;
})(KYT, Backbone, jQuery);
