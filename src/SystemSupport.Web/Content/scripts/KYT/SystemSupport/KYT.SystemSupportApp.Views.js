

KYT.Views.UserSettingsView =  KYT.Views.AjaxFormView.extend({
    initialize:function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
    },

    events:_.extend({
        "click #viewHistory":"viewHistory"
    }, KYT.Views.AjaxFormView.prototype.events),

    viewLoaded:function(){
        $(this.el).find(".content-header").append('<button id="permissions" title="Permissions"></button>');
    },
    viewHistory:function(e){
        e.preventDefault();
        if($("#userSubscriptionHistoryContainer").is(":visible")){
            $("#userSubscriptionHistoryContainer").hide();
            $("#viewHistory").text(this.options.viewHistoryText);
        }else{
            $("#userSubscriptionHistoryContainer").show();
            $("#viewHistory").text(this.options.hideHistoryText);
        }
    },
    permissions:function(){
        KYT.vent.trigger("route","userpermissionlist/"+this.model.UserId(),true);
    }
});

KYT.Views.UserGridView =  KYT.Views.GridView.extend({
    initialize: function(){
        KYT.mixin(this, "ajaxGridMixin");
        KYT.mixin(this, "setupGridMixin");
        KYT.mixin(this, "defaultGridEventsMixin");
        KYT.mixin(this, "setupGridSearchMixin");
    },
    viewLoaded:function(){
        KYT.vent.bind("Login",this.loginAsUser,this);
        this.setupBindings();
    },
    onClose:function(){
        KYT.vent.unbind("Login",this.loginAsUser,this);
        this.unbindBindings();
    },
    loginAsUser:function(id){
        KYT.repository.ajaxGet(this.options.getLogin +"/"+id,null).done($.proxy(this.redirectToApp,this));
    },
    redirectToApp:function(result){
        if(!result.Variable)return null;
        var url = this.options.RedirectUrl + "/login/log_in?guid=" + result.Variable + "&EntityId=" + result.EntityId + "&bypass=true";
        window.open(url);
    }
});

KYT.Views.ClientView = KYT.Views.AjaxFormView.extend({
    events:_.extend({
        'click .emailTemplates': "emailTemplates"
    }, KYT.Views.AjaxFormView.prototype.events),

    emailTemplates:function(){
        var id = $(this.el).find("#EntityId").val();
        KYT.vent.trigger("route","emailtemplatelist/"+id,true);
    }
});

KYT.Views.LoginStatisticsGridView = KYT.Views.GridView.extend({
    events:_.extend({
        "click #search":"filterByDate"
    }, KYT.Views.GridView.prototype.events),
    viewLoaded:function(){
        $(this.el).find($(".content-header > .search")).hide();
        $(this.el).find($(".content-header").append($("#loginStatSearchTemplate").tmpl()));
        $(this.el).find($(".content-header #start_date")).val(new XDate().toString("MM/dd/yyyy"));
        $(this.el).find($(".content-header #end_date")).val(new XDate().toString("MM/dd/yyyy"));
    },
    filterByDate:function(){
        var start = $(this.el).find($(".content-header #start_date")).val();
        var end = $(this.el).find($(".content-header #end_date")).val();
        var endPlusOneDay = new XDate(end).addDays(1);
        var searchItem1 = {"field": "createdate" ,"data": start,"op":"GreaterThanOrEqual" };
        var searchItem2 = {"field": "createdate" ,"data": endPlusOneDay,"op":"LessThanOrEqual" };
        var filter = {"group":"AND",rules:[searchItem1,searchItem2]};
        var obj = {"filters":""  + JSON.stringify(filter) + ""};
        $(this.options.gridContainer).jqGrid('setGridParam',{postData:obj});
        this.reloadGrid();
    }
});

KYT.Views.UserGroupPermissionGridView = KYT.Views.GridView.extend({
    onPreRender:function(){
        this.options.gridOptions = { grouping:true,groupingView :{groupField : ['FirstToken'],groupColumnShow : [false]}};
    },
    editItem:function(id,groupId){
        KYT.vent.trigger("route",this.options.addUpdateRoute+"/"+id+"/"+groupId,true);
    },
    addNew:function(){
        var groupId = $(this.el).find("#ParentId").val();
        KYT.vent.trigger("route",this.options.addUpdateRoute+"/0/"+groupId,true);
    }
});

KYT.Views.UserPermissionGridView = KYT.Views.GridView.extend({
    onPreRender:function(){
        this.options.gridOptions={loadComplete : function(){
            var ids = $("#gridContainer").jqGrid('getDataIDs');
            for (var i = 0, l = ids.length; i < l; i++) {
                var rowid = ids[i];
                var rowData =$("#gridContainer").jqGrid('getRowData',ids[i]);
                if ($(rowData.OnUser).text()=="False") {
                    var row = $('#' + rowid, this.el);
                    row.find("td:first input").remove();
                }
            }
        },
            grouping:true,groupingView :{groupField : ['FirstToken'],groupColumnShow : [false]}};
        KYT.vent.bind("AddUpdateItemUserGroup",this.editItemUserGroup,this);
    },
    editItem:function(id,groupId, uliId){
        KYT.vent.trigger("route",this.options.addUpdateRoute+"/"+id+"/"+groupId+"/"+uliId,true);
    },
    addNew:function(){
        var rootId = $(this.el).find("#RootId").val();
        KYT.vent.trigger("route",this.options.addUpdateRoute+"/0/0/"+rootId,true);
    },
    editItemUserGroup:function(id, name){
        KYT.vent.trigger("route",this.options.addUpdateUserGroupRoute+"/"+name,true);
    },
    onClose:function(){
        KYT.vent.unbind("AddUpdateItemUserGroup");
        this._super("onClose",arguments);
    }
});

KYT.Views.UserGroupView = KYT.Views.AjaxFormView.extend({
    events:_.extend({
        'click .showPermissions': "showPermissions"
    }, KYT.Views.AjaxFormView.prototype.events),

    showPermissions:function(){
        var id = $(this.el).find("#EntityId").val();
        KYT.vent.trigger("route","usergrouppermissionlist/"+id,true);
    }
});

