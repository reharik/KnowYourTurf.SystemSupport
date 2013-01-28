$(document).ready(function() {
    // this needs to be made dynamic but since I"m hardcoding the freakin
    //grid name fuck it.
   origionalSearchParameters = "";
   $('.dci_search').click(
        function(e){
            var form = $(e.target).parentsUntil("#gridSearch").last().parent().find("#searchArea");
            /*var postDataFilters = $("#gridContainer").getPostData().filters;
            if(postDataFilters)
                postDataFilters = JSON.parse(postDataFilters);
            var filters = origionalSearchParameters?origionalSearchParameters:postDataFilters;
            */var origional = jQuery.extend(true, {}, origionalSearchParameters||{});
            var filterItems = cc.gridHelper.buildSearchCriteria(form,origional);
            var obj = {"filters":""  + JSON.stringify(filterItems) + ""};
            //var parse = JSON.parse("{filtersearchCriteria);
            $("#gridContainer").setPostData(obj);
            $("#gridContainer").trigger("reloadGrid");

        }
   );
   $('.dci_clearForm').click(function(e) {
        var form = $(e.target).parentsUntil("#gridSearch").last().parent().find("#searchArea");
        $(form).clearForm();
        var obj = origionalSearchParameters?{"filters":""  + JSON.stringify(origionalSearchParameters) + ""}:{};

        $("#gridContainer").setPostData(obj);
        $("#gridContainer").trigger("reloadGrid");
   });

});

cc.gridHelper= (function(){
    return {
        buildSearchCriteria:function(form, crit){
            var formFields = "input, checkbox, select, textarea";
            var filterItems = crit;
            if(!filterItems.rules)
                filterItems = {group:"AND",rules:[]};
            $(form).find(formFields).each(function(i,item) {
                if ($(this).attr("type") != "submit" && $(this).attr("type") != "hidden") {
                    var idAttr = $(this).attr("name");
                    var value = $(this).val();
                    if(value && value != ""){
                        filterItems.rules.push( {"field": idAttr ,"data": value } );
                    }
                }
            });
           return filterItems;
        },
        adjustSize: function(gridSelector){
            var $el = $(gridSelector).parents(".content-inner:eq(0)");
            var h = $(window).height()-$("#main-header").height();
            $el.height(h-80);
            var gridHeight = $el.data("gridHeight");
            if(gridHeight != $el.height()-31)
                $(gridSelector).setGridHeight($el.height()-31  );
            $el.data("gridHeight",$el.height()-31);

            $(gridSelector).setGridWidth($el.width()-2);
        },
        adjustWidth:function(gridSelector){
           var $el = $(gridSelector).parents(".content-inner:eq(0)");
           var strangeOffset = 27;
           var rowsHeight = $(gridSelector +".ui-jqgrid-btable").height();
           $(gridSelector).setGridWidth($el.width() );
           if(rowsHeight>$el.height()-strangeOffset){
               $(gridSelector +".ui-jqgrid-btable").width($el.width()-17)
           }
        },
        adjustSortStyles: function(index,iCol,sortorder) {
            $('.ui-jqgrid th').removeClass('is-sorted');
            $('.ui-jqgrid th[aria-selected="true"]').addClass('is-sorted');
        }
    }

}());


cc.gridMultiSelect = (function(){
    return {
        getCheckedBoxes: function() {
            var ids = [];
            $("#gridContainer").find("[role='checkbox']:checked").each(function(i,item){
                var tr = $(item).closest("tr");
                if($(tr).attr("role")!="rowheader"){
                    ids.push($(tr).attr("id"));
                }
            });
            return ids;
        }
    }
}());


