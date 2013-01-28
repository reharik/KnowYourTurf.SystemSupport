/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/14/12
 * Time: 3:35 PM
 * To change this template use File | Settings | File Templates.
 */

DCI.Header = (function(DCI, Backbone){
    var Header = {};

    Header.HeaderView = Backbone.View.extend({
    });

    // Initialize the header functionality when the
    // application starts.
    DCI.addInitializer(function(){
        Header.view = new Header.HeaderView({
            el: $("#main-header")
        }).render();
        // dont' really need to add this to the DCI.header.show() here
        // cuz there will only ever be one header but if we do you need a
        // surrounding div for main-header and set the DCI.header to that

        // probably trigger the router for portfolio dashboard here
        // but maybe not cuz portapp may not be loaded yet
    });

    return Header;
})(DCI, Backbone);