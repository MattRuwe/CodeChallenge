@imports OmahaMTG.Challenge.MvcUI
@Code
    ViewData("Title") = "Home Page"
End Code

@section featured
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1>@ViewData("Title").</h1>
                <h2>@ViewData("Message")</h2>
            </hgroup>
            <p>
                To learn more about ASP.NET MVC visit
                <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
                The page features <mark>videos, tutorials, and samples</mark> to help you get the most from ASP.NET MVC.
                If you have any questions about ASP.NET MVC visit
                <a href="http://forums.asp.net/1146.aspx/1?MVC" title="ASP.NET MVC Forum">our forums</a>.
            </p>
        </div>
    </section>
End Section

@section scripts
    <script type="text/javascript" src="@Url.Content("~/Scripts/Homepage.js")"></script>
    <script type="text/javascript">
        codeChallenge.homepage.init({
            announcementsUrl: '@Url.Action("Announcements")',
            announcementsListSelector: '#announcements-listview',
            accountmentsTemplateSelector: '#announcements-template'
        });
    </script>
End Section

@*@(Html.Kendo().DatePicker().Name("Birthday"))*@
<div style="float: right">
    <strong>Announcements:</strong>
    <div id="announcements-listview" style="height: 200px; min-width: 275px; overflow-y: scroll;"></div>
</div>

<script type="text/x-kendo-tmpl" id="announcements-template">
    <p style="cursor: pointer;margin: 5px 12px;"><em>#= Title #</em></p>
</script>




<h3>We suggest the following:</h3>
<ol class="round">
    <li class="one">
        <h5>Getting Started</h5>
        ASP.NET MVC gives you a powerful, patterns-based way to build dynamic websites that
        enables a clean separation of concerns and that gives you full control over markup
        for enjoyable, agile development. ASP.NET MVC includes many features that enable
        fast, TDD-friendly development for creating sophisticated applications that use
        the latest web standards.
        <a href="http://go.microsoft.com/fwlink/?LinkId=245151">Learn more…</a>
    </li>

    <li class="two">
        <h5>Add NuGet packages and jump-start your coding</h5>
        NuGet makes it easy to install and update free libraries and tools.
        <a href="http://go.microsoft.com/fwlink/?LinkId=245153">Learn more…</a>
    </li>

    <li class="three">
        <h5>Find Web Hosting</h5>
        You can easily find a web hosting company that offers the right mix of features
        and price for your applications.
        <a href="http://go.microsoft.com/fwlink/?LinkId=245157">Learn more…</a>
    </li>
</ol>
