<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="CheckTicket.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.CheckTicket" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    html
    {
        overflow:hidden;
    }
    </style>
    <script type="text/javascript" src="../Scripts/ETOffine/CheckTicket.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a id="reIndex" class="fLeft page-logo"></a>
                <span>
                    <asp:Literal ID="ltlCheckTitle" runat="server"></asp:Literal></span>
                <a id="reBack" class="fRight page-back"></a>
            </div>
            <div class="PageMain">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="searchbar" style="padding: 0 20%;">
                    <tr>
                        <td height="68" width="110"><span class="w22">
                            <asp:Literal ID="ltlTitle" runat="server">电子门票号：</asp:Literal></span></td>
                        <td align="center" class="bg-shadow">
                            <asp:TextBox ID="txtTicket" runat="server" CssClass="page-input"></asp:TextBox></td>
                        <td width="140" align="center" class="bg-shadow">
                            <div id="btnOK" class="page-btn" onclick="CheckTicket.Check()">
                                <span>确定</span>
                                <span class="icon-arrow"></span>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="checkbox">
                    <div class="w18 checkbox-tittle">验票结果</div>
                    <!--这里的显示需要判断作判断
			1、灰色底（默认样式checkbox-result），文字是“等待输入票号...”，
			2、绿色底（后面添加checkbox-result-yes样式变成绿色底），文字变成“恭喜您！验证通过！”
			3、红色底（后面添加checkbox-result-no样式变成红色底），文字变成“600，异常-无此票！“-->
                    <div class="checkbox-result" id="AutoBox">
                        <span id="ResultTxt" class="resutl-txt">等待输入票号...</span>
                    </div>
                </div>
                <div class="clearfix" style="padding: 0 20%;"><span class="fLeft w18">活动名称：<asp:Literal ID="ltlActivityName" runat="server"></asp:Literal></span></div>
                <div class="clearfix" style="padding: 0 20%;"><span class="fLeft w18">活动编号：<asp:Label runat="server" ID="lbActivitySN"></asp:Label></span><span class="fRight w18">验票时间： <span id="spChecktime" class="checkTime"></span></span></div>
            </div>
        </div>
    </form>
     <script type="text/javascript">
         $(function () {
             //div自定义高度
             document.getElementById('AutoBox').style.height = (document.documentElement.clientHeight) / 2 - 100 + 'px';

         });
         window.onresize = function () {
             document.getElementById('AutoBox').style.height = (document.documentElement.clientHeight) / 2 - 100 + 'px';
         }

    </script>
</asp:Content>
