#pragma checksum "D:\MyPrj\2022\2022_Board\BoardTab\Views\Home\ViewTest.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "26ae26a0c0929f722676ef160de99429ed067b0e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_ViewTest), @"mvc.1.0.view", @"/Views/Home/ViewTest.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\MyPrj\2022\2022_Board\BoardTab\Views\_ViewImports.cshtml"
using BoardTab;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\MyPrj\2022\2022_Board\BoardTab\Views\_ViewImports.cshtml"
using BoardTab.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"26ae26a0c0929f722676ef160de99429ed067b0e", @"/Views/Home/ViewTest.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"b893fa2f9f29c81c60eddc82cddc6ac6ab46446b", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_ViewTest : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/layui/layui.all.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/layui/css/layui.css"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "26ae26a0c0929f722676ef160de99429ed067b0e4433", async() => {
                WriteLiteral("\r\n    <meta charset=\"UTF-8\">\r\n    <title>Title</title>\r\n    <style>\r\n        ");
                WriteLiteral(@"@keyframes lbody {
          /* from {top:0px;}
	       to {bottom:520px;}*/

            0% {
                transform: translate3d(0, 0, 0);
            }
            100% {
                transform: translate3d(0, -200px, 0);       //滚动内容div高度和
            }
        }
        .box{
            width: 800px;
            border: 1px solid #000000;
            position: absolute;
            height: 300px;
            overflow: hidden;
        }
        .box .lbody{
            animation: 15s lbody linear infinite normal;
            position: absolute;
        }
    </style>


");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "26ae26a0c0929f722676ef160de99429ed067b0e6130", async() => {
                WriteLiteral("\r\n    <div class=\"box\">\r\n        <div class=\"lbody\">\r\n");
                WriteLiteral("\r\n            <div>\r\n            <table id=\"demo\" lay-filter=\"demo\" class=\"layui-table\"\r\n                     lay-size=\"sm\" style=\"height:200px;display:block;padding:0px !Important;\"></table></div>\r\n");
                WriteLiteral("        </div>\r\n    </div>\r\n      ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "26ae26a0c0929f722676ef160de99429ed067b0e6773", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "26ae26a0c0929f722676ef160de99429ed067b0e7872", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
    <script>
        layui.use('table', function () {
            var table = layui.table;

            //展示已知数据
            table.render({
                elem: '#demo'
                , cols: [[ //标题栏
                    { field: 'id', title: 'ID', width: 80, sort: true }
                    , { field: 'username', title: '用户名', width: 120 }
                    , { field: 'email', title: '邮箱', minWidth: 150 }
                    , { field: 'sign', title: '签名', minWidth: 160 }
                    , { field: 'sex', title: '性别', width: 80 }
                    , { field: 'city', title: '城市', width: 100 }
                    , { field: 'experience', title: '积分', width: 80, sort: true }
                ]]
                , data: [{
                    ""id"": ""10001""
                    , ""username"": ""杜甫""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""exp");
                WriteLiteral(@"erience"": ""116""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""108""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10002""
                    , ""username"": ""李白""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""12""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                    , ""LAY_CHECKED"": true
                }, {
                    ""id"": ""10003""
                    , ""username"": ""王勃""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""65""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
            ");
                WriteLiteral(@"        , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10004""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""666""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10005""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""86""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10006""
                    , ""username"": ""贤心""
                    ");
                WriteLiteral(@", ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""12""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10007""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""16""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10008""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
        ");
                WriteLiteral(@"            , ""experience"": ""106""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }]
                //,skin: 'line' //表格风格
                , even: true
                //,page: true //是否显示分页
                //,limits: [5, 7, 10]
                //,limit: 5 //每页默认显示的数量
            });

                //展示已知数据
            table.render({
                elem: '#demo1'
                , cols: [[ //标题栏
                    { field: 'id', title: 'ID', width: 80, sort: true }
                    , { field: 'username', title: '用户名', width: 120 }
                    , { field: 'email', title: '邮箱', minWidth: 150 }
                    , { field: 'sign', title: '签名', minWidth: 160 }
                    , { field: 'sex', title: '性别', width: 80 }
                    , { field: 'city', title: '城市', width: 100 }
                    , { field: 'experience', title: '积分', width: 80, sort: true }
              ");
                WriteLiteral(@"  ]]
                , data: [{
                    ""id"": ""10001""
                    , ""username"": ""杜甫""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""116""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""108""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10002""
                    , ""username"": ""李白""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""12""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                    , ""LAY_CHECKED"": true
                }, {
                    ""id"": ""10003""
                    , ""username"": ""王勃""
 ");
                WriteLiteral(@"                   , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""65""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10004""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""666""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10005""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ");
                WriteLiteral(@"""人生恰似一场修行""
                    , ""experience"": ""86""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10006""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""12""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10007""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""16""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                  ");
                WriteLiteral(@"  , ""joinTime"": ""2016-10-14""
                }, {
                    ""id"": ""10008""
                    , ""username"": ""贤心""
                    , ""email"": ""xianxin@layui.com""
                    , ""sex"": ""男""
                    , ""city"": ""浙江杭州""
                    , ""sign"": ""人生恰似一场修行""
                    , ""experience"": ""106""
                    , ""ip"": ""192.168.0.8""
                    , ""logins"": ""106""
                    , ""joinTime"": ""2016-10-14""
                }]
                //,skin: 'line' //表格风格
                , even: true
                //,page: true //是否显示分页
                //,limits: [5, 7, 10]
                //,limit: 5 //每页默认显示的数量
            });
        });
    </script>
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</html>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
