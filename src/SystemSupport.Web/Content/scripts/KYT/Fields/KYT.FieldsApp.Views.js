/**
 * Created by JetBrains RubyMine.
 * User: Owner
 * Date: 2/26/12
 * Time: 11:24 AM
 * To change this template use File | Settings | File Templates.
 */

KYT.Views.EmployeeDashboardView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
        this.options.noBubbleUp=true;
        KYT.vent.bind("form:"+this.id+":success",this.reload,this);
    },
    viewLoaded:function(){
        this.pendingGridView = new KYT.Views.DahsboardGridView({el:"#pendingTaskGridContainer",
            url:this.model._pendingGridUrl(),
            route:"task",
            gridId:"pendingTaskList",
            gridOptions:{
                multiselect:false
            }
        });
        this.completedGridView = new KYT.Views.DahsboardGridView({el:"#completedTaskGridContainer",
          url:this.model._completedGridUrl(),
            gridId:"completedTaskList",
            route:"taskdisplay",
            gridOptions:{
                multiselect:false
            }
        });
        this.pendingGridView.render();
        this.completedGridView.render();
        this.storeChild(this.pendingGridView);
        this.storeChild(this.completedGridView);
    },
    callbackAction: function(){
        this.pendingGridView.callbackAction();
        this.completedGridView.callbackAction();
    },
    reload:function(){
        this.render();
    }
});

KYT.Views.FieldView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
    },
    viewLoaded:function(){
        this.addIdsToModel();
        $('#colorPickerInput',this.el).miniColors();
    }
});

KYT.Views.EmployeeListView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "ajaxGridMixin");
        KYT.mixin(this, "setupGridMixin");
        KYT.mixin(this, "defaultGridEventsMixin");
        KYT.mixin(this, "setupGridSearchMixin");
    },
    viewLoaded:function(){
        KYT.vent.bind(this.options.gridId+":Redirect",this.showDashboard,this);
        this.setupBindings();
    },
    onClose:function(){
        KYT.vent.unbind(this.options.gridId+":Redirect",this.showDashboard,this);
        this.unbindBindings();
    },
    showDashboard:function(id){
        KYT.vent.trigger("route",KYT.generateRoute("employeedashboard",id),true);
    }
});

KYT.Views.ListTypeListView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
    },
    viewLoaded:function(){
        // the gridview rendercallback over writes deleteMultipleUrl so we have to load it here
        KYT.vent.bind("grid:tasktypelist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultipleTaskTypesUrl();
        },this);
        KYT.vent.bind("grid:eventtypelist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultipleEventTypesUrl();
        },this);
        KYT.vent.bind("grid:photocategorylist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultiplePhotoCatUrl();
        },this);
        KYT.vent.bind("grid:documentcategorylist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultipleDocCatUrl();
        },this);
        KYT.vent.bind("grid:equipmenttasktypelist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultipleEquipTaskTypeUrl();
        },this);
        KYT.vent.bind("grid:equipmentttypelist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultipleEquipTypeUrl();
        },this);
        KYT.vent.bind("grid:partlist:pageLoaded",function(options){
             options.deleteMultipleUrl = this.model._deleteMultiplePartsUrl();
        },this);
        this.taskTypeGridView = new KYT.Views.DahsboardGridView({
            el:"#taskTypeGridContainer",
            url:this.model._taskTypeGridUrl(),
            gridId:"tasktypelist",
            route:"tasktype"
        });
        this.eventTypeGridView = new KYT.Views.DahsboardGridView({
            el:"#eventTypeGridContainer",
            url:this.model._eventTypeGridUrl(),
            gridId:"eventtypelist",
            route:"eventtype"
        });
        this.photoCategoryGridView = new KYT.Views.DahsboardGridView({
            el:"#photoCategoryGridContainer",
            url:this.model._photoCategoryGridUrl(),
            gridId:"photocategorylist",
            route:"photocategory"
        });
        this.documentCategoryGridView = new KYT.Views.DahsboardGridView({
            el:"#documentCategoryGridContainer",
            url:this.model._documentCategoryGridUrl(),
            gridId:"documentcategorylist",
            route:"documentcategory"
        });
        this.equipmentTaskTypeGridView = new KYT.Views.DahsboardGridView({
            el:"#equipmentTaskTypeGridContainer",
            url:this.model._equipmentTaskTypeGridUrl(),
            gridId:"equipmenttasktypelist",
            route:"equipmenttasktype"
        });
        this.equipmentTypeGridView = new KYT.Views.DahsboardGridView({
            el:"#equipmentTypeGridContainer",
            url:this.model._equipmentTypeGridUrl(),
            gridId:"equipmenttypelist",
            route:"equipmenttype"
        });
        this.partGridView = new KYT.Views.DahsboardGridView({
            el:"#partsGridContainer",
            url:this.model._partsGridUrl(),
            gridId:"partlist",
            route:"part"
        });


        this.taskTypeGridView.render();
        this.eventTypeGridView.render();
        this.photoCategoryGridView.render();
        this.documentCategoryGridView.render();
        this.equipmentTaskTypeGridView.render();
        this.equipmentTypeGridView.render();
        this.partGridView.render();
        this.storeChild(this.taskTypeGridView);
        this.storeChild(this.eventTypeGridView);
        this.storeChild(this.photoCategoryGridView);
        this.storeChild(this.documentCategoryGridView);
        this.storeChild(this.equipmentTaskTypeGridView);
        this.storeChild(this.equipmentTypeGridView);
        this.storeChild(this.partGridView);
    },
    callbackAction: function(){
        this.taskTypeGridView.callbackAction();
        this.eventTypeGridView.callbackAction();
        this.photoCategoryGridView.callbackAction();
        this.documentCategoryGridView.callbackAction();
        this.equipmentTaskTypeGridView.callbackAction();
        this.equipmentTypeGridView.callbackAction();
        this.partGridView.callbackAction();
    }
});

KYT.Views.EventTypeFormView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
    },
    viewLoaded:function(){
        $('#colorPickerInput',this.el).miniColors();
    }
});

KYT.Views.TaskTypeFormView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "formMixin");
        KYT.mixin(this, "ajaxFormMixin");
        KYT.mixin(this, "modelAndElementsMixin");
    },
    viewLoaded:function(){
        $('#colorPickerInput',this.el).miniColors();
    }
});

KYT.Views.InventoryDisplayView = KYT.Views.View.extend({
    initialize:function(){
        KYT.mixin(this, "displayMixin");
        KYT.mixin(this, "ajaxDisplayMixin");
        KYT.mixin(this, "modelAndElementsMixin");
        this.options.templateUrl += this.options.url.substr(this.options.url.indexOf("Display/")+7);
    },
    viewLoaded:function(){
        $('#colorPickerInput',this.el).miniColors();
    }
});

