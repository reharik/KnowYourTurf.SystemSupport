/**
 * Created by .
 * User: RHarik
 * Date: 4/20/11
 * Time: 11:22 AM
 * To change this template use File | Settings | File Templates.
 */
if (typeof dci == "undefined") {
            var dci = {};
}

if (typeof dci.repeatableInputSection== "undefined") {
            dci.repeatableInputSection= {};
}

// this is not a static instance!
dci.repeatableInputSection.controller = function () {
    var myOptions;
    return {
        init: function(options) {
            myOptions = $.extend({ }, dci.repeatableInputSection.defaults, options || { });
            $(myOptions.container).data().repeatableSection = this;
            $(myOptions.container).delegate("#addNewSection", "click", function(e) {
                $(this).closest("div.dci_repeatableSection").data().repeatableSection.addNewSection(e) });
            $(myOptions.container).delegate(".dci_remove", "click", function(e) { $(this).closest("div.dci_repeatableSection").data().repeatableSection.removeSection(e) });
            this.addNewSection();
        },
        addNewSection: function(e) {
            var section = $(myOptions.template).children("li");
            var newSection = $(section).clone();
            cc.utilities.clearInputs(newSection);
            $(myOptions.container).find("ul").append(newSection);
            return newSection;
        },
        removeSection: function(e) {
            $(e.target).closest("li").remove();
        }
    };
};

dci.repeatableInputSection.defaults = {
    container:"#repeatableItems",
    template:"#repeatableItemTemplate"
};