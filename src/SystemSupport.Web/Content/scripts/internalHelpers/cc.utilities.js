if (typeof cc == "undefined") {
            var cc = {};
}

cc.namespace = function() {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};

cc.object = function(o){
    function F(){}
    F.prototype = o;
    return new F();
};

cc.namespace("cc.utilities");

cc.utilities.toggleForm = function(type, onlyShowNoHide) {
    if (onlyShowNoHide && $("#" + type + "Collapsible .dci_container:visible").length > 0) return;
    var element = $("#" + type + "Collapsible .dci_container");
    if (!element.is(":hidden")) {
        element.hide();
    } else if (element.is(":hidden")) {
        element.show();
    }
};

cc.utilities.clearInputs = function(element){
    var formFields = "input, checkbox, select, textarea";
    $(element).find(formFields).each(function(){
        if(this.tagName == "SELECT" ){
            $(this).find("option:selected").removeAttr("selected");
        } else if($(this).text()) {
            $(this).text("");
        } else if ( this.type == "radio") {
            $(this).attr("checked",false);
        } else if ( this.type == "checkbox") {
            $(this).attr("checked",false);
        } else if($(this).val() && this.type != "radio") {
            $(this).val("");
        }
    })
};

 cc.utilities.findAndRemoveItem = function(array, item, propertyOnItem){
    if(!$.isArray(array)) return false;
    var indexOfItem;
    $(array).each(function(idx,x){
        if(propertyOnItem){
            if(x[propertyOnItem] == item){
                 indexOfItem=idx;
            }
        }else{
            if(x == item){
                indexOfItem=idx;
            }
        }
    });
    if(indexOfItem >= 0){
        array.splice(indexOfItem,1);
    }
 };

 cc.utilities.cleanAndHideErrorMessageDiv = function(element){
    $(element).html("");
     $(element).hide();
};

 cc.utilities.openDocInNewWindow = function(url){
     window.open(url,'_blank','');
 };

 cc.straightNotification = function(result,form,notification){
    notification.result(result);
};

(function($) {
    $.fn.convertToListItems = function(value, display) {
        var listItems = [];
        this.each(function(i, item) {
            var selectedItem = "";
            if (item.IsDefault) {
                selectedItem = "selected=\"selected\"";
            }
            var option = '<option value="' + item[value] + '" ' + selectedItem + '> ' + item[display] + ' </option>';
            listItems.push(option);
        });
        return listItems;
    };
})(jQuery);

cc.utilities.screensize = (function(){
    return {
            resize:function(context, additionalOffset){
                var offset = 250;
                var screen = $(window).height();
                var containerHeight = additionalOffset ? $(context).find("#contentHeight").height() +additionalOffset:$(context).find("#contentHeight").height();
                if(containerHeight>480){
                    $(context).find(".form-scroll-inner").height(screen-offset);
                }
            }
    }
}());

cc.utilities.trim = function(stringValue){
    return stringValue.replace(/(^\s*|\s*$)/, "");
};

cc.utilities.fixedWidthDropdown = function(){
    if(!$.browser.msie){
        $('select.dci_fixedWidthDropdown').css("width","auto");
    }else{
       $('select.DCI_fixedWidthDropdown')
        .each(function() {
            $(this).data("origWidth", $(this).outerWidth()); // IE 8 will take padding on selects
        })
        .bind("focusin mouseenter", function() {
            $(this).css("width", "auto");
        })
        .bind("blur change", function() {
            $(this).css("width", $(this).data("origWidth"));
        });
    }
};

(function($) {
    $.fn.extend({
        exclusiveCheck: function() {
            var checkboxes = $(this).find("input:checkbox");
            checkboxes.each(function(i,item){
                $(item).click(function(){
                    if(this.checked){
                        checkboxes.each(function() {
                            if ($(this)[0]!==$(item)[0]) this.checked = false;
                        });
                    }
                })
            });
        }});
})(jQuery);


$.fn.clearForm = function() {
  return this.each(function() {
    var type = this.type, tag = this.tagName.toLowerCase();
    if (tag != 'input')
      return $(':input',this).clearForm();
    if (type == 'text' || type == 'password' || tag == 'textarea')
      this.value = '';
    else if (type == 'checkbox' || type == 'radio')
      this.checked = false;
    else if (tag == 'select')
      this.selectedIndex = -1;
  });
};

cc.utilities.preventDoubleClick= function(selector){
    $(selector).data().isHandlerActive = false;
    setTimeout(function(){$(selector).data().isHandlerActive=true;},1500);
};

//function parseDMY(str) {
//    // this example parses dates like "month/date/year"
//    var parts = str.split('/');
//    if (parts.length == 3) {
//        return new XDate(
//            parseInt(parts[2]), // year
//            parseInt(parts[1]), // month
//            parseInt(parts[0]) // date
//        );
//    }
//}
//
//XDate.parsers.push(parseDMY);


