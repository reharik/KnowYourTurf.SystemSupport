/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 2/17/12
 * Time: 10:34 AM
 * To change this template use File | Settings | File Templates.
 */
DCI.WorkflowManager = (function(DCI, Backbone){
    var WFM =  Backbone.Model.extend({
        defaults: {
            parentStack: []
        },
        addChildView:function(child){
            var parent = DCI.State.get("currentView");
            if(!parent)return null;
            DCI.State.set({"currentView":child});
            DCI.State.set({"childView":child});
            var stack =  this.get("parentStack");
            stack.push(parent);
            $.when(child.render()).then(function () {
                $(parent.el).after(child.el);
            });
            $(parent.el).hide();
            return child;
        },
        returnParentView:function(result, triggerCallback){
            var stack =  this.get("parentStack");
            var parent = stack.pop();
            DCI.State.get("currentView").close();
            if(parent){
                if(triggerCallback&&parent.callbackAction){
                    parent.callbackAction(result);
                }
                $(parent.el).show();
                DCI.vent.trigger("route",parent.options.route,false);
                DCI.State.set({"currentView":parent});
            }
        },
        loadBottomLevel:function(url){
            var last = _.last(this.get("parentStack"));
            if(last && last.options.url == url){
                this.returnParentView("",false);
                return false;
            }
            return true;
        },
        cleanAllViews:function(){
            var currentView = DCI.State.get("currentView");
            if(currentView)currentView.close();
            var stack =  this.get("parentStack");
            while (stack.length>0){
                stack.pop().close();
            }
        }
    });
    return new WFM();

})(DCI, Backbone);



