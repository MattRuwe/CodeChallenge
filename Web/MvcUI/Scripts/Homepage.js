var codeChallenge = codeChallenge || {};
codeChallenge.homepage = codeChallenge.homepage || function () {
    var announcementsDataSource = null;
    function init(args) {
        announcementsDataSource = new kendo.data.DataSource({
            transport: {
                read: {
                    url: args.announcementsUrl,
                    contentType: "application/json; charset=utf-8",
                    type: "GET",
                    dataType: "json"
                }
            }
        });

        $(args.announcementsListSelector).kendoListView({
            selectable: true,
            dataSource: announcementsDataSource,
            template: kendo.template($(args.accountmentsTemplateSelector).html()),
            change: announcementSelected
        });
    }

    function announcementSelected(e) {
        var data = announcementsDataSource.view(),
                    selected = $.map(this.select(), function (item) {
                        return data[$(item).index()].Title;
                    });

        kendoConsole.log("Selected: " + selected.length + " item(s), [" + selected.join(", ") + "]");
    }

    return {
        init: init
    };
}();