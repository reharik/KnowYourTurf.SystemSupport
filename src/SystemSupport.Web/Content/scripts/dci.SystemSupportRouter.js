/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 11/1/11
 * Time: 11:02 AM
 * To change this template use File | Settings | File Templates.
 */

if (typeof dci == "undefined") {
            var dci = {};
}


dci.SystemSupportRouter = Backbone.Router.extend({

    routes: {
        "/UserList/ItemList":  "userList_ItemList"    // #help
    },

    userList_ItemList:function(){
        alert("hello");
    }

});