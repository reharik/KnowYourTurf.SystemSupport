/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 10:49 AM
 * To change this template use File | Settings | File Templates.
 */
    DCI.Views.UserSettingsView =  DCI.Views.AjaxFormView.extend({
        events:_.extend({
            'click #addAddress':'addUpdateAddress',
            'click #addEmail':'addUpdateEmail',
            'click #addPhone':'addUpdatePhone',
            'click #removeAddress':'removeAddress',
            'click #removeEmail':'removeEmail',
            'click #removePhone':'removePhone',
            "click #viewHistory":"viewHistory",
            "change [name='NotApplicable']":"noEndDate"

        }, DCI.Views.AjaxFormView.prototype.events),

        viewLoaded:function(){
            $(this.el).find(".content-header").append('<button id="permissions" title="Permissions"></button>');
            $("#addressDisplayTemplate").tmpl(this.options.repeaters.addressItems).appendTo("#addressDisplayContainer");
            $("#emailDisplayTemplate").tmpl(this.options.repeaters.emailItems).appendTo("#emailDisplayContainer");
            $("#phoneDisplayTemplate").tmpl(this.options.repeaters.phoneItems).appendTo("#phoneDisplayContainer");
            DCI.vent.bind("ajaxPopupFormModule:address:success", this.addressSuccess,this);
            DCI.vent.bind("ajaxPopupFormModule:address:cancel", this.addressCancel,this);
            DCI.vent.bind("ajaxPopupFormModule:phone:success", this.phoneSuccess,this);
            DCI.vent.bind("ajaxPopupFormModule:phone:cancel", this.phoneCancel,this);
            DCI.vent.bind("ajaxPopupFormModule:email:success", this.emailSuccess,this);
            DCI.vent.bind("ajaxPopupFormModule:email:cancel", this.emailCancel,this);
            DCI.vent.bind("form_userProfile:success", this.success,this);
            DCI.vent.bind("form_userProfile:cancel", this.cancel,this);
            $("#permissions",this.el).click($.proxy(this.permissions,this));
        },
        onClose:function(){
            // not sure why but seems that the base is being called anyway.
            // probably the base from an other view but we'll see
            //DCI.Views.AjaxFormView.prototype.onClose.call(this);
            DCI.vent.unbind("ajaxPopupFormModule:address:success");
            DCI.vent.unbind("ajaxPopupFormModule:address:cancel");
            DCI.vent.unbind("ajaxPopupFormModule:phone:success");
            DCI.vent.unbind("ajaxPopupFormModule:phone:cancel");
            DCI.vent.unbind("ajaxPopupFormModule:email:success");
            DCI.vent.unbind("ajaxPopupFormModule:email:cancel");
            DCI.vent.unbind("form_userProfile:success");
            DCI.vent.unbind("form_userProfile:cancel");

        },
        addUpdateAddress:function(){
            var id = $(this.el).find("#EntityId").val();
            var options = {
                id: "address",
                url: this.options.addUpdateAddressUrl,
                data:{"EntityId":0, "ParentId":id},
                title:"Address"
            };
            this.addressModule = new DCI.Views.AjaxPopupFormModule(options);
            this.addressModule.render();
            this.storeChild(this.addressModule);
            this.$el.append(this.addressModule.el);
        },
        addUpdateEmail:function(){
            var id = $(this.el).find("#EntityId").val();
            var options = {
                id: "email",
                url: this.options.addUpdateEmailUrl,
                data:{"EntityId":0, "ParentId":id},
                title:"Email"
            };
            this.emailModule = new DCI.Views.AjaxPopupFormModule(options);
            this.emailModule.render();
            this.storeChild(this.emailModule);
            this.$el.append(this.emailModule.el);
        },
        addUpdatePhone:function(){
            var id = $(this.el).find("#EntityId").val();
            var options = {
                id: "phone",
                url: this.options.addUpdatePhoneUrl,
                data:{"EntityId":0, "ParentId":id},
                title:"Phone"
            };
            this.phoneModule = new DCI.Views.AjaxPopupFormModule(options);
            this.phoneModule.render();
            this.storeChild(this.phoneModule);
            this.$el.append(this.phoneModule.el);
        },

        updateEmailList:function(item){
            $("#emailDisplayTemplate").tmpl(item).appendTo("#emailDisplayContainer");
        },
        updatePhoneList:function(item){
            $("#phoneDisplayTemplate").tmpl(item).appendTo("#phoneDisplayContainer");
        },
        removeAddress:function(e){
            var parentId = $(this.el).find("#EntityId").val();
            var id = $(e.target).find("#EntityId").val();
            $(e.target).parent().remove();
            DCI.repository.ajaxGet(this.options.deleteItemUrl,{"EntityId":id,"ParentId":parentId,"ItemType":"Address"},function(){});
        },
        removeEmail:function(e){
            var parentId = $(this.el).find("#EntityId").val();
            var id = $(e.target).find("#EntityId").val();
            $(e.target).parent().remove();
            DCI.repository.ajaxGet(this.options.deleteItemUrl,{"EntityId":id,"ParentId":parentId,"ItemType":"Email"},function(){});
        },
        removePhone:function(e){
            var parentId = $(this.el).find("#EntityId").val();
            var id = $(e.target).find("#EntityId").val();
            $(e.target).parent().remove();
            DCI.repository.ajaxGet(this.options.deleteItemUrl,{"EntityId":id,"ParentId":parentId,"ItemType":"Phone"},function(){});
        },
        addressSuccess:function(result){
            this.addressCancel();
            $("#addressDisplayTemplate").tmpl(result.Target).appendTo("#addressDisplayContainer");
        },
        addressCancel:function(){
            this.addressModule.close();
        },
        emailSuccess:function(result){
            this.emailCancel();
            $("#emailDisplayTemplate").tmpl(result.Target).appendTo("#emailDisplayContainer");
        },
        emailCancel:function(){
            this.emailModule.close();
        },
        phoneSuccess:function(result){
            this.phoneCancel();
            $("#phoneDisplayTemplate").tmpl(result.Target).appendTo("#phoneDisplayContainer");
        },
        phoneCancel:function(){
            this.phoneModule.close();
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
        noEndDate:function(e){
            if($(e.target).is(":checked")){
                $("[name='LatestSubscription.ExpirationDate']").val("");
                $("[name='LatestSubscription.ExpirationDate']").prop("disabled",true);
            }else{
                $("[name='LatestSubscription.ExpirationDate']").prop("disabled",false);
            }
        },
        permissions:function(){
            var id = $(this.el).find("#UserLoginInfoId").val();
            DCI.vent.trigger("route","userpermissionlist/"+id,true);
        }
    });

    DCI.Views.UserGridView =  DCI.Views.GridView.extend({
        onPreRender:function(){
            DCI.vent.bind("Login",this.loginAsUser,this);
        },
        loginAsUser:function(id){
            DCI.repository.ajaxGet(this.options.getLogin +"/"+id,null,$.proxy(this.redirectToApp,this));
        },
        redirectToApp:function(result){
            if(!result.Variable)return null;
            var url = this.options.RedirectUrl + "/login/log_in?guid=" + result.Variable + "&EntityId=" + result.EntityId + "&bypass=true";
            window.open(url);
        },
        onClose:function(){
            DCI.vent.unbind("Login",this.loginAsUser,this);
            this._super("onClose",arguments);
        }

    });

    DCI.Views.PromotionView = DCI.Views.AjaxFormView.extend({
        viewLoaded:function(){
            $("#monthBasePromo").hide();
            $("#percentageBasePromo").hide();

            if($("[name='Promotion.MonthsBeforeNextBilling']").val()>0){
                $("#monthBasePromo").show();
                $("#promoTypeSelector_0").prop("checked",true);
            }

            if($("[name='Promotion.PercentageDiscount']").val()>0){
                $("#percentageBasePromo").show();
                $("#promoTypeSelector_1").prop("checked",true);
            }

            $("input[id*='promoTypeSelector']").click(function(e){
                if($("#promoTypeSelector_0").is(":checked")){
                    $("#monthBasePromo").show();
                    $("#percentageBasePromo").hide();
                    $("input", "#percentageBasePromo").val("");
                }
                if($("#promoTypeSelector_1").is(":checked")){
                    $("#percentageBasePromo").show();
                    $("#monthBasePromo").hide();
                    $("input", "#monthBasePromo").val("");
                }
            });
        }
    });

    DCI.Views.PendingChargesGridView =  DCI.Views.GridView.extend({
        viewLoaded:function(){
            DCI.vent.bind("Login",this.loginAsUser,this);
            DCI.vent.bind("ChargeVoid",this.chargeVoid,this);
        },
        onClose:function(){
            DCI.vent.unbind("Login",this.loginAsUser,this);
            DCI.vent.unbind("ChargeVoid",this.chargeVoid,this);
            this._super("onClose",arguments);
        },
        loginAsUser:function(url){
            DCI.repository.ajaxGet(url,null,$.proxy(this.redirectToApp,this));
        },
        redirectToApp:function(result){
            if(!result.Variable)return null;
            var url = this.options.RedirectUrl + "/login/log_in?guid=" + result.Variable + "&EntityId=" + result.EntityId + "&bypass=true"; 
            window.open(url);
        },
        chargeVoid:function(url){
            var notificationArea = new cc.NotificationArea(this.cid,"#errorMessagesGrid",$("#errorMessagesGrid",this.el), DCI.vent);
            DCI.notificationService.addArea(notificationArea);
            DCI.repository.ajaxGet(url,null,$.proxy(this.chargeVoidResponse, this));
        },
        chargeVoidResponse:function(result){
            DCI.notificationService.processResult(result,this.cid,this.id);
            this.reloadGrid();
            DCI.notificationService.removeArea(this.cid);
        }
    });

    DCI.Views.ClientView = DCI.Views.AjaxFormView.extend({
        events:_.extend({
            'click .emailTemplates': "emailTemplates"
        }, DCI.Views.AjaxFormView.prototype.events),

        emailTemplates:function(){
            var id = $(this.el).find("#EntityId").val();
            DCI.vent.trigger("route","emailtemplatelist/"+id,true);
        }
    });

    DCI.Views.EmailTemplateGridView =  DCI.Views.GridView.extend({
        addNew:function(){
            var parentId = $(this.el).find("#ParentId").val();
            DCI.vent.trigger("route",this.options.addUpate+"/0/"+parentId,true);
        },
        editItem:function(id,itemType){
            var parentId = $(this.el).find("#ParentId").val();
            DCI.vent.trigger("route",this.options.addUpate+"/"+id+"/"+parentId,true);
        }
    });

    DCI.Views.LoginStatisticsGridView = DCI.Views.GridView.extend({
        events:_.extend({
            "click #search":"filterByDate"
        }, DCI.Views.GridView.prototype.events),
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

    DCI.Views.UserGroupPermissionGridView = DCI.Views.GridView.extend({
        onPreRender:function(){
            this.options.gridOptions = { grouping:true,groupingView :{groupField : ['FirstToken'],groupColumnShow : [false]}};
        },
        editItem:function(id,groupId){
            DCI.vent.trigger("route",this.options.addUpdateRoute+"/"+id+"/"+groupId,true);
        },
        addNew:function(){
            var groupId = $(this.el).find("#ParentId").val();
            DCI.vent.trigger("route",this.options.addUpdateRoute+"/0/"+groupId,true);
        }
    });

    DCI.Views.UserPermissionGridView = DCI.Views.GridView.extend({
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
            DCI.vent.bind("AddUpdateItemUserGroup",this.editItemUserGroup,this);
        },
        editItem:function(id,groupId, uliId){
            DCI.vent.trigger("route",this.options.addUpdateRoute+"/"+id+"/"+groupId+"/"+uliId,true);
        },
        addNew:function(){
            var rootId = $(this.el).find("#RootId").val();
            DCI.vent.trigger("route",this.options.addUpdateRoute+"/0/0/"+rootId,true);
        },
        editItemUserGroup:function(id, name){
            DCI.vent.trigger("route",this.options.addUpdateUserGroupRoute+"/"+name,true);
        },
        onClose:function(){
            DCI.vent.unbind("AddUpdateItemUserGroup");
            this._super("onClose",arguments);
        }
    });

    DCI.Views.UserGroupView = DCI.Views.AjaxFormView.extend({
        events:_.extend({
            'click .showPermissions': "showPermissions"
        }, DCI.Views.AjaxFormView.prototype.events),

        showPermissions:function(){
            var id = $(this.el).find("#EntityId").val();
            DCI.vent.trigger("route","usergrouppermissionlist/"+id,true);
        }
    });



