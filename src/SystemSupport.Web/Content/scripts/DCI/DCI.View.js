/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/15/12
 * Time: 11:19 AM
 * To change this template use File | Settings | File Templates.
 */

// use like this
// this._super("testMethod",arguments);


(function(Backbone) {

  // The super method takes two parameters: a method name
  // and an array of arguments to pass to the overridden method.
  // This is to optimize for the common case of passing 'arguments'.
  function _super(methodName, args) {

    // Keep track of how far up the prototype chain we have traversed,
    // in order to handle nested calls to _super.
    this._superCallObjects || (this._superCallObjects = {});
    var currentObject = this._superCallObjects[methodName] || this,
        parentObject  = findSuper(methodName, currentObject);
    this._superCallObjects[methodName] = parentObject;
    if(parentObject[methodName]){
        var result = parentObject[methodName].apply(this, args || []);
    }
    delete this._superCallObjects[methodName];
    return result;
  }

  // Find the next object up the prototype chain that has a
  // different implementation of the method.
  function findSuper(methodName, childObject) {
    var object = childObject;
    while (object[methodName] === childObject[methodName]) {
      object = object.constructor.__super__;
    }
    return object;
  }

  _.each(["Model", "Collection", "View", "Router"], function(klass) {
    Backbone[klass].prototype._super = _super;
  });

})(Backbone);

DCI.Views = {};

DCI.Views.View = Backbone.View.extend({
    // Remove the child view and close it
    removeChildView: function(item){
      var view = this.children[item.cid];
      if (view){
        view.close();
        delete this.children[item.cid];
      }
    },
    // Store references to all of the child `itemView`
    // instances so they can be managed and cleaned up, later.
    storeChild: function(view){
      if (!this.children){
        this.children = {};
      }
      this.children[view.cid] = view;
    },

    // Handle cleanup and other closing needs for
    // the collection of views.
    close: function(){
        if(this.options.area){
            DCI.notificationService.removeArea(this.options.area);
        }
        this.unbind();
      //this.unbindAll();
        this.remove();

        this.closeChildren(this);
        if (this.onClose){
            this.onClose();
        }
    },

    closeChild:function(view,pointer){
        view.close();
        delete this.children[view.cid];
        delete this[pointer];
    },

    closeChildren: function(){
      if (this.children){
        _.each(this.children, function(childView){
          childView.close();
        });
      }

    },
    viewLoaded:function(){}
  });

DCI.Views.BaseFormView = DCI.Views.View.extend({
    events:{
        'click #save' : 'saveItem',
        'click .cancel' : 'cancel'
    },
    initialize: function(){
       this.options = $.extend({},DCI.formDefaults,this.options);
    },
    config:function(){
        if(extraFormOptions){
            $.extend(true, this.options, extraFormOptions);
        }
        this.options.area = this.options.area? this.options.area: new cc.NotificationArea(this.cid,"#errorMessagesGrid",$("#errorMessagesForm",this.el), DCI.vent);
        DCI.vent.bind(this.options.area.areaName()+":"+this.id+":success",this.successHandler,this);
        DCI.notificationService.addArea(this.options.area);
        $(this.options.crudFormSelector,this.el).crudForm(this.options.crudFormOptions,this.options.area.areaName());
        if(this.options.crudFormOptions.additionBeforeSubmitFunc){
            var array = !$.isArray(this.options.crudFormOptions.additionBeforeSubmitFunc)
                ? [this.options.crudFormOptions.additionBeforeSubmitFunc]
                : this.options.crudFormOptions.additionBeforeSubmitFunc;
            $(array).each($.proxy(function(i,item){
                $(this.options.crudFormSelector,this.el).data('crudForm').setBeforeSubmitFuncs(item);
            },this));
        }
        var config = {
            toolbar:
            [
                ['Bold', 'Italic', 'Underline', '-', 'NumberedList', 'BulletedList', '-','JustifyLeft','JustifyCenter','JustifyRight','JustifyBlock'],
                ['Font','FontSize','TextColor']
            ],
            height:100,
            removePlugins : 'elementspath'
        };
        $(this.el).find(".rte").each(function(i,item){
            $(item).attr("id",Math.floor(Math.random()*100000));
            $(item).ckeditor(config);
        });

    },
    onClose:function(){
        if(typeof(CKEDITOR) !== 'undefined'){
            $.each(CKEDITOR.instances,function(i,item){CKEDITOR.remove(item);});
        }
        DCI.vent.unbind("default:"+this.id+":success");
    },
    sizeScreen:function(){
        cc.utilities.screensize.resize(this.el);
    },
    saveItem:function(){
        var $form = $(this.options.crudFormSelector,this.el);
        $form.data().viewId = this.id;
        $form.submit();
    },
    cancel:function(){
        DCI.vent.trigger("form:"+this.id+":cancel");
        if(!this.options.noBubbleUp) {DCI.WorkflowManager.returnParentView();}
    },
    successHandler:function(result){
        DCI.vent.trigger("form:"+this.id+":success",result);
        if(!this.options.noBubbleUp){DCI.WorkflowManager.returnParentView(result,true);}
    }
});

DCI.Views.FormView = DCI.Views.BaseFormView.extend({
    render: function(){
        this.config();
        if(this.setBindings){this.setBindings();}
        this.viewLoaded();
        DCI.vent.trigger("form:"+this.id+":pageLoaded",this.options);
        return this;
    }
});

DCI.Views.AjaxFormView = DCI.Views.BaseFormView.extend({
    initialize: function(){
        this._super("initialize",arguments);
        if(this.options.notificationArea){
            this.options.notificationService.addArea(this.options.notificationArea);
            this.options.crudFormOptions.notificationAreaName = this.options.notificationArea.areaName();
        }
    },
    render:function(){
        DCI.repository.ajaxGet(this.options.url, this.options.data, $.proxy(function(result){this.renderCallback(result);},this));
    },
    renderCallback:function(result){
        if(result.LoggedOut){
            window.location.replace(result.RedirectUrl);
            return;
        }
        if(this.setBindings){this.setBindings();}
        $(this.el).html(result);
        this.config();
        //callback for render
        this.viewLoaded();
        //general notification of pageloaded
        this.checkForInstructions();
        DCI.vent.trigger("form:"+this.id+":pageLoaded",this.options);
        //trigger event for global binding on any form
        DCI.vent.trigger("form:pageLoaded",this.options);
        $(this.el).find(".form-scroll-inner").animate({scrollTop:0},200);
        $(this.el).find("form :input:visible:enabled:first").focus();
        this.sizeScreen();
    },
    checkForInstructions:function(){
        if($(".page-instructions")){
             this.instructions = new DCI.Views.Instructions({el:".page-instructions"});
            this.instructions.render();
            this.storeChild(this.instructions);
        }
    }
});

DCI.Views.AjaxDisplayView = DCI.Views.View.extend({
    events:{
        'click .cancel' : 'cancel'
    },
    initialize: function(){
        this.options = $.extend({},DCI.displayDefaults,this.options);
         this.$el = $(this.el);
    },
    render:function(){
        DCI.repository.ajaxGet(this.options.url, this.options.data, $.proxy(this.renderCallback,this));
    },
    renderCallback:function(result){
        if(result.LoggedOut){
            window.location.replace(result.RedirectUrl);
            return;
        }
        $(this.el).html(result);
        if(extraFormOptions){
            $.extend(true,this.options, extraFormOptions);
        }
//        if(typeof this.options.runAfterRenderFunction == 'function'){
//            this.options.runAfterRenderFunction.apply(this,[this.el]);
//        }
      //  $.publish("/contentLevel/display_"+this.id+"/pageLoaded",[this.options]);
        DCI.vent.trigger("display:"+this.id+":pageLoaded",this.options);
    },
    cancel:function(){
        DCI.vent.trigger(this.id+":cancel",this.id);
    }
});

DCI.Views.GridView = DCI.Views.View.extend({
    events:{
        'click .new' : 'addNew',
        'click .delete' : 'deleteItems'
    },
    initialize: function(){
        this.options = $.extend({},DCI.gridDefaults,this.options);
        this.id=this.options.id;
        DCI.vent.bind("gridLoadComplete",this.resizeWidth,this);
    },
    render:function(){
        if(this.onPreRender){this.onPreRender();}
        DCI.repository.ajaxGet(this.options.url, this.options.data, $.proxy(function(result){this.renderCallback(result);},this));
    },
    renderCallback:function(result){
        if(result.LoggedOut){
            window.location.replace(result.RedirectUrl);
            return;
        }
        if(this.setBindings){this.setBindings();}

        $(this.el).html($("#gridTemplate").tmpl(result));
        $.extend(this.options,result);

        var gridOptions = this.options.gridOptions|| {};
        gridOptions.searchOptions = result.differentSearchField||gridOptions.searchOptions;

        $(this.el).html($("#gridTemplate").tmpl(result));
        $.each(this.options.headerButtons,function(i,item){
            $("."+item).show();
        });

        var gridContainer = this.options.gridContainer;
        $(gridContainer,this.el).AsGrid(this.options.gridDef, gridOptions);

        $(this.el).gridSearch({onClear:$.proxy(this.removeSearch,this),onSubmit:$.proxy(this.search,this)});
        //general notification of pageloaded
        this.checkForInstructions();
        DCI.vent.trigger("grid:"+this.id+":pageLoaded",this.options);
        DCI.vent.trigger("grid:pageLoaded",this.options);
        DCI.vent.bind("AddUpdateItem",this.editItem,this);
        this.resizeWindow();
    },
    resizeWindow:function(){
        var gridSelector = this.options.gridContainer;
        $(window).bind('resize', function() {
            cc.gridHelper.adjustSize(gridSelector);
        }).trigger('resize');
    },
    resizeWidth:function(){
        var gridSelector = this.options.gridContainer;
        cc.gridHelper.adjustWidth(gridSelector);
    },
    checkForInstructions:function(){
        if($(".page-instructions").size()>0){
            this.instructions =  this.instructions = new DCI.Views.Instructions({el:".page-instructions"});
            this.instructions.render();
            this.storeChild(this.instructions);
        }
    },
    addNew:function(){
        DCI.vent.trigger("route",this.options.addUpate,true);
        //$.publish('/contentLevel/grid_'+this.id+'/AddUpdateItem', [this.options.addUpdateUrl]);
    },
    editItem:function(id,itemType){
        var _itemType = itemType?itemType:"";
        // fix this for all assets
        DCI.vent.trigger("route",this.options.addUpate+"/"+id,true);
    },
    deleteItems:function(){
        if (confirm("Are you sure you would like to delete this Item?")) {
            var ids = cc.gridMultiSelect.getCheckedBoxes();
            DCI.repository.ajaxGet(this.options.deleteMultipleUrl,
                $.param({"EntityIds":ids},true),
                $.proxy(function(){this.reloadGrid();},this));
        }
    },
    search:function(v){
        var searchItem = {"field": this.options.searchField ,"data": v };
        var filter = {"group":"AND",rules:[searchItem]};
        var obj = {"filters":""  + JSON.stringify(filter) + ""};
        $(this.options.gridContainer).jqGrid('setGridParam',{postData:obj});
        this.reloadGrid();
    },
    removeSearch:function(){
        delete $(this.options.gridContainer).jqGrid('getGridParam' ,'postData').filters;
        this.reloadGrid();
        return false;
    },
    reloadGrid:function(){
        DCI.vent.unbind("AddUpdateItem");
        $(this.options.gridContainer).trigger("reloadGrid");
        DCI.vent.bind("AddUpdateItem",this.editItem,this);
    },
    callbackAction:function(){
        this.reloadGrid();
    },
    onClose:function(){
        DCI.vent.unbind("AddUpdateItem");
        DCI.vent.unbind("gridLoadComplete",this.resizeWidth,this);
        this._super("onClose",arguments);
    }

});

DCI.Views.AjaxPopupFormModule  = DCI.Views.View.extend({
    initialize:function(){
        this.registerSubscriptions();
        this._super("initialize",arguments);
    },

    render: function(){
        this.options.noBubbleUp=true;
        this.popupForm = this.options.view ? new DCI.Views[this.options.view](this.options) : new DCI.Views.AjaxFormView(this.options);
        this.popupForm.render();
        this.storeChild(this.popupForm);
        $(this.el).append(this.popupForm.el);
    },
    registerSubscriptions: function(){
        DCI.vent.bind("form:"+this.id+":pageLoaded",this.viewLoaded,this);
        DCI.vent.bind("popup:"+this.id+":cancel", this.popupCancel, this);
        DCI.vent.bind("popup:"+this.id+":save", this.formSave, this);
        DCI.vent.bind("popupNotificationArea:"+this.id+":success", this.formSuccess, this);
    },
    onClose:function(){
         DCI.vent.unbind("form:"+this.id+":pageLoaded");
         DCI.vent.unbind("popup:"+this.id+":cancel");
        DCI.vent.unbind("popup:"+this.id+":save");
        DCI.vent.unbind("popupNotificationArea:"+this.id+":success");
        this._super("onClose",arguments);
    },
    viewLoaded:function(formOptions){
        var buttons = formOptions.buttons?formOptions.buttons:DCI.Views.popupButtonBuilder.builder(formOptions.id).standardEditButons();
        var popupOptions = {
            id:this.id,
            el:this.el, // we pass the el here so we can call the popup on it
            buttons: buttons,
            title:formOptions.title,
            height:this.options.height
        };
        var view = new DCI.Views.PopupView(popupOptions);
        view.render();
        this.storeChild(view);
    },
    formSave:function(){
        this.popupForm.saveItem();
    },
    //just catching and re triggering with the module name
    formSuccess:function(result){
        DCI.vent.trigger("ajaxPopupFormModule:"+this.id+":success",result);
    },
    popupCancel:function(){
        DCI.vent.trigger("ajaxPopupFormModule:"+this.id+":cancel",[]);
    }
});

DCI.Views.AjaxPopupDisplayModule  = DCI.Views.View.extend({
    initialize:function(){
        this.registerSubscriptions();
    },
    render: function(){
        this.popupDisplay = this.options.view ? new this.options.view(this.options) : new DCI.Views.AjaxDisplayView(this.options);
        this.popupDisplay.render();
        this.storeChild(this.popupDisplay);
        $(this.el).append(this.popupDisplay.el);
    },

    registerSubscriptions: function(){
        DCI.vent.bind("display:"+this.id+":pageLoaded", this.loadPopupView, this);
        DCI.vent.bind("popup:"+this.id+":cancel", this.popupCancel, this);
    },
    onClose:function(){
         DCI.vent.unbind("display:"+this.id+":pageLoaded");
         DCI.vent.unbind("popup:"+this.id+":cancel");
        this._super("onClose",arguments);
    },

    loadPopupView:function(formOptions){
        var buttons = formOptions.buttons?formOptions.buttons:DCI.Views.popupButtonBuilder.builder(formOptions.id).standardEditButons();
        var popupOptions = {
            id:this.id,
            el:this.el, // we pass the el here so we can call the popup on it
            buttons: buttons,
            width:this.options.popupWidth,
            title:formOptions.title
        };
        var view = new DCI.Views.PopupView(popupOptions);
        view.render();
        this.storeChild(view);
    },
    popupCancel:function(){
        DCI.vent.trigger("module:"+this.id+":cancel",[]);
    }
});

DCI.Views.PopupView = DCI.Views.View.extend({
    render:function(){
        $(".ui-dialog").remove();
        var errorMessages = $("div[id*='errorMessages']", this.el);
        if(errorMessages){
            var id = errorMessages.attr("id");
            errorMessages.attr("id","errorMessagesPU").removeClass(id).addClass("errorMessagesPU");
        }

        $(this.el).dialog({
            modal: true,
            width: this.options.width||550,
            height:this.options.height,
            buttons:this.options.buttons,
            title: this.options.title,
            close:function(){
                DCI.vent.trigger("popup:"+id+":cancel");
            }
        });
        return this;
    },
    close:function(){
        $(this.el).dialog("close");
    }
});

DCI.Views.popupButtonBuilder = (function(){
    return {
        builder: function(id){
        var buttons = {};
        var _addButton = function(name,func){ buttons[name] = func; };
        var saveFunc = function() {
            DCI.vent.trigger("popup:"+id+":save");
        };
        var editFunc = function(event) {DCI.popupCrud.controller.editFromDisplay(event);};
        var cancelFunc = function(){
                            DCI.vent.trigger("popup:"+id+":cancel");
                            $(this).dialog("close");
                            $(".ui-dialog").remove();
                        };
        return{
            getButtons:function(){return buttons;},
            getSaveFunc:function(){return saveFunc;},
            getCancelFunc:function(){return cancelFunc;},
            addSaveButton:function(){_addButton("Save",saveFunc); return this;},
            addUpdateButton:function(){_addButton("Edit",editFunc);return this;},
            addCancelButton:function(){_addButton("Cancel",cancelFunc);return this;},
            addButton:function(name,func){_addButton(name,func);return this;},
            clearButtons:function(){buttons = {};return this;},
            standardEditButons: function(){
                _addButton("Save",saveFunc);
                _addButton("Cancel",cancelFunc);
                return buttons;
            },
            standardDisplayButtons: function(){
                _addButton("Cancel",cancelFunc);
                return buttons;
            }
        };
    }
    };
}());

DCI.Views.TokenizerModule = DCI.Views.View.extend({
    render:function() {
        this.registerSubscriptions();
        this.tokenView = new DCI.Views.TokenView(this.options);
        this.storeChild(this.tokenView);
    },
    registerSubscriptions: function() {
        DCI.vent.bind("token:" + this.id + ":addUpdate", this.addUpdateItem, this);
        DCI.vent.bind("ajaxPopupFormModule:" + this.id + ":success", this.formSuccess, this);
        DCI.vent.bind("ajaxPopupFormModule:" + this.id + ":cancel", this.formCancel, this);
    },
    onClose:function(){
         DCI.vent.unbind("token:" + this.id + ":addUpdate");
         DCI.vent.unbind("ajaxPopupFormModule:" + this.id + ":success");
        DCI.vent.unbind("ajaxPopupFormModule:" + this.id + ":cancel");
        this._super("onClose",arguments);
    },
//from tolkneizer
    addUpdateItem:function() {
        var options = {
            id: this.id,
            url: this.options.tokenizerUrls[this.id + "AddUpdateUrl"],
            crudFormOptions: { errorContainer:"#errorMessagesPU",successContainer:"#errorMessagesForm"},
            view: this.options.addNewView
        };
        this.popupModule = new DCI.Views.AjaxPopupFormModule(options);
        this.popupModule.render();
        this.storeChild(this.popupModule);
        $(this.el).append(this.popupModule.el);
    },
    formSuccess:function(result) {
        this.tokenView.successHandler(result);
        this.popupModule.close();
        this.formCancel();
        return false;
    },
    formCancel:function() {
        this.popupModule.close();
        return false;
// $.publish("/contentLevel/tokenizer_" + this.id + "/formCancel",[this.id]);
    }
});

DCI.Views.TokenView = DCI.Views.View.extend({
    events:{
        'click #addNew' : 'addNew'
    },
    initialize: function(){
        this.options = $.extend({},DCI.tokenDefaults,this.options);
        this.id=this.options.id;

        if(!this.options.availableItems || this.options.availableItems.length===0) {
            $("#noAssets",this.el).show();
            $("#hasAssets",this.el).hide();
        }else{
            $("#noAssets",this.el).hide();
            $("#hasAssets",this.el).show();
            this.inputSetup();
        }
    },
    inputSetup:function(){
        var that = this;
        $(this.options.inputSelector, this.el).tokenInput(this.options.availableItems, {prePopulate: this.options.selectedItems,
            internalTokenMarkup:this.options.internalTokenMarkup?this.options.internalTokenMarkup:function(item) {
//                var cssClass = that.options.tooltipAjaxUrl ? "class='dci_tokenTooltip selectedItem' rel='" + this.options.tooltipAjaxUrl + "?EntityId=" + item.id + "'" : "class='selectedItem'";
                var cssClass = "class='selectedItem'";
                return "<p><a " + cssClass + ">" + item.name + "</a></p>";
            },
            afterTokenSelectedFunction:function() {
            },
            onAdd:$.proxy(that.tokenAdded,that),
            onDelete:$.proxy(that.tokenDeleted,that)
        });
        this.options.instantiated = true;
    },
    addNew:function(e){
        e.preventDefault();
        DCI.vent.trigger("token:"+this.id+":addUpdate",this.options);
    },
    successHandler: function(result){
        if(!this.options.instantiated){
            $("#noAssets",this.el).hide();
            $("#hasAssets",this.el).show();
            this.inputSetup();
        }
        $(this.options.inputSelector,this.el).tokenInput("add",{id:result.EntityId, name:result.Data.Name});
        $(this.options.inputSelector,this.el).tokenInput("addToAvailableList",{id:result.EntityId, name:result.Data.Name});
    },
    tokenAdded: function(){
        this.options.hasSelectedItems=$(this.options.inputSelector,this.el).val().length>0;
        DCI.vent.trigger("token:"+this.id+":tokenAdded",this.options);
    },
    tokenDeleted: function(){
        this.options.hasSelectedItems=$(this.options.inputSelector,this.el).val().length>0;
        DCI.vent.trigger("token:"+this.id+":tokenDeleted",this.options);
    }
});

DCI.Views.Instructions = DCI.Views.FormView.extend({
    events:_.extend({
        'click #EditInstructions': "showForm"
    }, DCI.Views.FormView.prototype.events),
    setBindings:function(){
        DCI.vent.bind("form:instructionEditor:success", this.success,this);
        DCI.vent.bind("form:instructionEditor:cancel", this.cancel,this);
    },
    onClose:function(){
        DCI.vent.unbind("form:instructionEditor:success", this.success,this);
        DCI.vent.unbind("form:instructionEditor:cancel", this.cancel,this);
        this._super("onClose",arguments);
    },
    viewLoaded:function(){
        $("#instructionsEditor").hide();
    },
    showForm:function(){
        if(this.options.formShowing){return;}
        this.options.formShowing = true;
        $("#instructionsEditor").show();
        $(".page-instructions").hide();
        if(!this.editInstructions){
            var formOptions = {
                id:"instructionEditor",
                el: "#instructionsEditor",
                crudFormOptions: { errorContainer:"#errorMessagesInstructions",successContainer:"#errorMessagesForm"},
                editorOptions:{width:"100%"},
                noBubbleUp:true
            };
            this.editInstructions = new DCI.Views.FormView(formOptions);
            this.editInstructions.render();
            this.storeChild(this.editInstructions);
        }
    },
    cancel:function(){
        this.options.formShowing = false;
        $("#instructionsEditor").hide();
        $(".page-instructions").show();
    },
    success:function(result){
        $("#instructionText").html(result.Variable);
        this.cancel();
    }
});

DCI.Views.RepeatableTableRow = DCI.Views.View.extend({
    events:{
            'click .addButton':'addRow',
            'click a.close':'deleteRow',
            'click .move-up':'moveUp',
            'click .move-down':'moveDown',
            'hover .unit':'unitHover'
    },
    render:function(){
        if(this.preRender){this.preRender();}
        if(!this.options.rowClass){this.options.rowClass=".unit";}
        this.clearHoverImages();
        var that = this;
        $(this.options.items).each(function(i,item){
            that.addRow(null,item);
        });
        if(!this.options.items||this.options.items.length<=0){this.addRow();}
        DCI.vent.trigger("repeater:"+this.id+":pageLoaded",this.options);
    },
    addRow:function(e,item){
        if(e){e.preventDefault();}
        var last = $(this.el).find(this.options.rowClass).last();
        if(last.size()<=0){
            last = $(this.el).find("tr").first();
        }
        var template = Handlebars.compile( $(this.options.template).html());
        last.after(template(item||{}));
        this.handleDatePickers(last.next());
        DCI.vent.trigger("repeater:"+this.id+":rowRendered", item,last);
    },
    handleDatePickers:function(row){
        $(row).find(".datePicker").each(function(i,item){
            $(item).attr("id",Math.floor(Math.random()*100000))
        })
    },
    deleteRow:function(e){
        e.preventDefault();
        var $item = $(e.currentTarget).closest("tr");
        var entityId = $item.find("#EntityId").val();
        $item.remove();
        DCI.vent.trigger("repeater:"+this.id+":rowDeleted",entityId);
    },
    moveUp:function(e){
        var unit = $(e.currentTarget).closest(this.options.rowClass);
        var prev = unit.prev(this.options.rowClass);
        if (!prev || prev.find('th').length>0) {  return;  } // don't move before header
        prev.before(unit);
    },
    moveDown:function(e){
        var unit = $(e.currentTarget).closest(this.options.rowClass);
        var next = unit.next(this.options.rowClass);
        if (next.length==0) {  return;  } // don't move before header
        next.after(unit);
    },
    unitHover:function(e){
        this.clearHoverImages();
        $(e.currentTarget).find("a.close").css("visibility","visible");
        $(e.currentTarget).find(".move-up", this).css("visibility","visible");
        $(e.currentTarget).find(".move-down", this).css("visibility","visible");
        $(e.currentTarget).addClass('current');
    },
    clearHoverImages:function () {
         $(this.options.rowClass,this.el).each(function(i,item) {
             $(item).find("a.close").css("visibility","hidden");
             $(item).find(".move-up").css("visibility","hidden");
             $(item).find(".move-down").css("visibility","hidden");
             $(item).removeClass('current');
        });
    }
});


DCI.tokenDefaults = {
    availableItems:[],
    selectedItems: [],
    tooltipAjaxUrl:"",
    inputSelector:"#Input"
};

DCI.popupDefaults = {
    title:""
};

DCI.gridDefaults = {
    searchField:"Name",
    gridContainer: "#gridContainer",
    showSearch:true,
    id:""
};

DCI.formDefaults = {
    id:"",
    data:{},
    crudFormSelector:"#CRUDForm",
    crudFormOptions:{
        errorContainer:"#errorMessagesForm",
        successContainer:"#errorMessagesGrid",
        additionBeforeSubmitFunc:null
    },
    runAfterRenderFunction: null
};

DCI.displayDefaults = {
    id:"",
    data:{},
    runAfterRenderFunction: null
};