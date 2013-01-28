/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 10/26/11
 * Time: 4:52 PM
 * To change this template use File | Settings | File Templates.
 */
if (typeof dci == "undefined") {
            var dci = {};
}


dci.UserController  = dci.Controller.extend({
    events:_.extend({
    }, dci.Controller.prototype.events),

    initialize:function(options){
        $.extend(this,this.defaults());
        dci.contentLevelControllers["userController"]=this;
        $.unsubscribeByPrefix("/contentLevel");
        this.id="userController";
        this.registerSubscriptions();

        var _options = $.extend({},this.options, options);
        _options.el="#masterArea";
        this.views.gridView = new dci.GridView(_options);
    },

    registerSubscriptions: function(){
        $.subscribe('/contentLevel/grid_/AddUpdateItem',$.proxy(this.addUpdateItem,this), this.cid);
        $.subscribe('/contentLevel/grid_/Login',$.proxy(this.loginAsUser,this),this.cid);
        //
        //
        $.subscribe('/contentLevel/form_mainForm/success',$.proxy(this.formSuccess,this),this.cid);
        $.subscribe('/contentLevel/form_mainForm/cancel',$.proxy(this.formCancel,this), this.cid);
    },

    //from grid
    addUpdateItem: function(url,id){
        var loginInfoId = $("td","tr#"+id).last().text();
        url = url + "?ParentId="+loginInfoId;
        var formOptions = {
            el: "#detailArea",
            id: "mainForm",
            url: url
        };
        $("#masterArea","#contentInner").after("<div id='detailArea'/>");
        this.views.formView = new dci.UserProfileView(formOptions);
        this.views.formView.render();
        $("#masterArea").hide();
    },

    loginAsUser:function(url,id){
       var loginInfoId = $("td","tr#"+id).last().text();
        url = url + "?ParentId="+loginInfoId;
        dci.repository.ajaxGet(url,null,$.proxy(this.redirectToApp,this));
    },
    redirectToApp:function(result){
        if(!result.Variable)return null;
        var url = this.options.redirectUrl +"/login/log_in?guid="+result.Variable+"&EntityId="+result.EntityId+"&bypass=true";
        window.open(url);
    },
    formSuccess:function(){
        this.formCancel();
        this.views.gridView.reloadGrid();
    },
    formCancel: function(){
        this.views.formView.remove();
        $("#masterArea").show();
    }

});
