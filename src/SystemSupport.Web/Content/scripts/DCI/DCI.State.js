/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/16/12
 * Time: 2:10 PM
 * To change this template use File | Settings | File Templates.
 */


DCI.State = (function(DCI, Backbone){
    var State =  Backbone.Model.extend({
        defaults: {
            parentStack: []
        },
        application:"",
        currentView:""
    });
    return new State();
})(DCI, Backbone);



