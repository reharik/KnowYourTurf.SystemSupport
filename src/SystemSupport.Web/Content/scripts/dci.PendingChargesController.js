/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 11/18/11
 * Time: 9:05 AM
 * To change this template use File | Settings | File Templates.
 */


dci.PendingChargesController= dci.CrudController.extend({
    events:_.extend({
    }, dci.CrudController.prototype.events),

    registerAdditionalSubscriptions:function(){
        $.subscribe("/contentLevel/grid/ChargeVoid", $.proxy(this.chargeVoid,this))
    },
    chargeVoid:function(url){
        dci.repository.ajaxGet(url,null,$.proxy(this.chargeVoidResponse, this));
    },
    chargeVoidResponse:function(result){
        var emh = cc.utilities.messageHandling.messageHandler();
        var errorMessage;
        var field;
        var message;
        var error;
        if (result.Errors && result.Errors.length > 0) {
            for (var i = 0; i < result.Errors.length; i++) {
                field = result.Errors[i].PropertyName;
                message = result.Errors[i].ErrorMessage;
                errorMessage = field + ": " + message;
                type = "error";
                error = cc.utilities.messageHandling.mhMessage(type,errorMessage,field);
                emh.addMessage(error);
            }
        } else if (result.Message) {
            errorMessage = result.Message + " ";
            var type = "success";
            error = cc.utilities.messageHandling.mhMessage(type,errorMessage,"success");
            emh.addMessage(error);
        }
        emh.showAllMessages("#errorMessagesGrid");
        this.views.gridView.reloadGrid();
    }
});