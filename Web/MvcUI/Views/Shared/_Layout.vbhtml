<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>@ViewData("Title") - My ASP.NET MVC Application</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    @Styles.Render("~/Content/css")
    @Styles.Render("~/Content/kendo/css")
    @Scripts.Render("~/bundles/modernizr")
    <!--This bundle was moved by the Kendo UI VS Extensions for compatibility reasons-->
    @Scripts.Render("~/bundles/jquery")
    <!-- Kendo -->
    @Scripts.Render("~/bundles/kendo")
    @*<!--This CSS entry was added by the Kendo UI VS Extensions for compatibility reasons-->
    <link href="@Url.Content("~/Content/kendo.compatibility.css")" rel="stylesheet" type="text/css" />
	<link href="@Url.Content("~/Content/kendo/2013.2.918/kendo.common.min.css")" rel="stylesheet" type="text/css" />
	<link href="@Url.Content("~/Content/kendo/2013.2.918/kendo.dataviz.min.css")" rel="stylesheet" type="text/css" />
	<link href="@Url.Content("~/Content/kendo/2013.2.918/kendo.default.min.css")" rel="stylesheet" type="text/css" />
	<link href="@Url.Content("~/Content/kendo/2013.2.918/kendo.dataviz.default.min.css")" rel="stylesheet" type="text/css" />
	<script src="@Url.Content("~/Scripts/kendo/2013.2.918/jquery.min.js")"></script>
	<script src="@Url.Content("~/Scripts/kendo/2013.2.918/kendo.all.min.js")"></script>
	<script src="@Url.Content("~/Scripts/kendo/2013.2.918/kendo.aspnetmvc.min.js")"></script>*@
	<script src="@Url.Content("~/Scripts/kendo.modernizr.custom.js")"></script>
</head>
<body>
    <header>
        <div class="content-wrapper">
            <div class="float-left">
                <p class="site-title">@Html.ActionLink("your logo here", "Index", "Home")</p>
            </div>
            <div class="float-right">
                <section id="login">
                    @Html.Partial("_LoginPartial")
                </section>
                <nav>
                    <ul id="menu">
                        <li>@Html.ActionLink("Home", "Index", "Home")</li>
                        <li>@Html.ActionLink("About", "About", "Home")</li>
                        <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                    </ul>
                </nav>
            </div>
        </div>
    </header>
    <div id="body">
        @RenderSection("featured", required:=False)
        <section class="content-wrapper main-content clear-fix">
            @RenderBody()
        </section>
    </div>
    <footer>
        <div class="content-wrapper">
            <div class="float-left">
                <p>&copy; @DateTime.Now.Year - My ASP.NET MVC Application</p>
            </div>
        </div>
    </footer>


    @RenderSection("scripts", required:=False)
</body>
</html>
