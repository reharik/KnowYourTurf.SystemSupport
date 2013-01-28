/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 11/8/11
 * Time: 4:45 PM
 * To change this template use File | Settings | File Templates.
 */
if (typeof dci == "undefined") {
            var dci = {};
}


dci.PromoController  = dci.CrudController.extend({
    events:_.extend({
    }, dci.CrudController.prototype.events),

    initialize:function(options){
        $.extend(this,this.defaults());
        dci.contentLevelControllers["promoController"]=this;
        $.unsubscribeByPrefix("/contentLevel");
        this.id="promoController";
        this.registerSubscriptions();

        var _options = $.extend({},this.options, options);
        _options.el="#masterArea";
        this.views.gridView = new dci.GridView(_options);
    },

    registerAdditionalSubscriptions: function(){
        $.subscribe("/contentLevel/form_mainForm/pageLoaded", $.proxy(this.pageLoaded, this), this.cid);
    },

    pageLoaded:function(){
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