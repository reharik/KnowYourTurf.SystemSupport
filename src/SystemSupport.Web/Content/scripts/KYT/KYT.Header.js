/**
 * Created by JetBrains RubyMine.
 * User: Owner
 * Date: 2/26/12
 * Time: 11:04 AM
 * To change this template use File | Settings | File Templates.
 */

KYT.Header = (function(KYT, Backbone){
    var Header = {};

    Header.HeaderView = Backbone.View.extend({
        events:{
            'click #userSettings' : 'userSettings',
            'change #ClientEntityId' : 'clientEntityId'
        },
        initialize: function(){
            KYT.mixin(this, "modelAndElementsMixin");
            this.rawModel = KYT.headerViewModel;
            this.bindModelAndElements();

            var self = this;
            KYT.State.bind("change:application", function(){
                self.setSelection(KYT.State.get("application"));
            });
        },
        clientEntityId:function(){
            var clientEntityId = this.model.ClientEntityId();
            KYT.State.set("ClientId",clientEntityId);
            if(clientEntityId>0){
                KYT.vent.trigger("route","client/"+clientEntityId,true);
            }
        },
        userSettings:function(e){
            e.preventDefault();
            KYT.State.set({"application":"userSettings"});
            KYT.vent.trigger("route","usersettings",true);
        },
        // makes the header show the correct menu item as being selected
        setSelection: function(app){
            this.$("#main-tabs li").removeClass("selected");
            this.$("#userSettings").removeClass("active");

            if(app == "userSettings"){
                this.$("#userSettings").addClass("active");
            }
        }

    });

    // Initialize the header functionality when the
    // application starts.
    KYT.addInitializer(function(){
        Header.view = new Header.HeaderView({
            el: $("#main-header")
        });
        Header.view.render();
        // dont' really need to add this to the KYT.header.show() here
        // cuz there will only ever be one header but if we do you need a
        // surrounding div for main-header and set the KYT.header to that

        // probably trigger the router for portfolio dashboard here
        // but maybe not cuz portapp may not be loaded yet
    });

    return Header;
})(KYT, Backbone);
