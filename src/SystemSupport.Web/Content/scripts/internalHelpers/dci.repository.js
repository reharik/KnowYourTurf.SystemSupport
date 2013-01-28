/**
 * Created by JetBrains RubyMine.
 * User: RHarik
 * Date: 10/13/11
 * Time: 10:52 AM
 * To change this template use File | Settings | File Templates.
 */

DCI.repository= (function(){
    var repositoryCallback = function(result,callback){
        if(result.LoggedOut){
            window.location.replace(result.RedirectUrl);
            return;
        }
        callback(result);
    };
    return {
        ajaxPost:function(url, data, callback){
            $.post(url,data,function(result){ repositoryCallback(result,callback)});
        },
        ajaxGet:function(url, data, callback){
            $.get(url,data,function(result){
                repositoryCallback(result,callback);
            });
        }
    }
}());