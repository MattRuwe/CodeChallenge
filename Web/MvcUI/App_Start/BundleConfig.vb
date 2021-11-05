Imports System.Web
Imports System.Web.Optimization

Public Class BundleConfig
    ' For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
        bundles.Add(New ScriptBundle("~/bundles/jquery").Include(
                   "~/Scripts/jquery-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryui").Include(
                    "~/Scripts/jquery-ui-{version}.js"))

        bundles.Add(New ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.unobtrusive*",
                    "~/Scripts/jquery.validate*"))

        ' Use the development version of Modernizr to develop with and learn from. Then, when you're
        ' ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"))

        bundles.Add(New ScriptBundle("~/bundles/kendo").
                    Include("~/Scripts/Kendo/2013.2.918/kendo.web.*").
                    Include("~/Scripts/kendo/2013.2.918/kendo.aspnetmvc.*"))

        bundles.Add(New StyleBundle("~/Content/kendo/css").
                    Include("~/Content/kendo/2013.2.918/kendo.common.*").
                    Include("~/Content/kendo/2013.2.918/kendo.default.*"))


        bundles.Add(New StyleBundle("~/Content/css").Include("~/Content/site.css"))

        bundles.Add(New StyleBundle("~/Content/themes/base/css").Include(
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.theme.css"))


        'Clear all items from the ignore list to allow minified CSS and JavaScript files in debug mode
        Dim ignoreList as IgnoreList = bundles.IgnoreList
        ignoreList.Clear()

        'Add back the default ignore list rules sans the ones which affect minified files and debug mode
        bundles.IgnoreList.Ignore("*.intellisense.js")
        bundles.IgnoreList.Ignore("*-vsdoc.js")
        bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled)

    End Sub
End Class