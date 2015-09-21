<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    window.onload = function () {
        //alert(history.length);
        if (history.length == 1) {
            setTimeout(function () {
                var wsh = new ActiveXObject("WScript.Shell");
                wsh.sendKeys("{F11}");
            }, 200);
        }
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="PageContent box">
        <div class="PageHeader clearfix">
            <a href="Default.aspx" class="fLeft page-logo"></a><span>电子门票验票客户端</span> <div  class="fRight fRightNull" >&nbsp;
            </div>
        </div>
        <div class="PageMain">
            <table cellpadding="0" cellspacing="0" border="0" class="appbox">
                <tr>
                    <td align="center" class="bg-shadow">
                        <a href="Pages/DownLoadData.aspx" class="page-btnBig">下载门票数据</a>
                    </td>
                    <td align="center" class="bg-shadow">
                        <a href="Pages/UpLoadData.aspx" class="page-btnBig">上传验票数据</a>
                    </td>
                </tr>
                <tr>
                    <td align="center" class="bg-shadow">
                        <a href="Pages/SingleCheckingActivityList.aspx" class="page-btnBig">单机验票</a>
                    </td>
                    <td align="center" class="bg-shadow">
                        <a href="Pages/UnionCheckingActivityList.aspx" class="page-btnBig">多机验票</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
    <%--<div class="contentBorder">
            <div class="closeButton" onclick="window.close();">
                <img src="Styles/Images/close.JPG" />
            </div>
            <div class="contentTitle">电子门票验票客户端</div>
            <div class="indexContent">                
                <asp:Button CssClass="inputButtonL" Width="150px" ID="btnLoad" runat="server" Text="下载门票数据" />&nbsp;&nbsp;
                <asp:Button CssClass="inputButtonL" Width="150px" ID="btnUpdate" runat="server" Text="上传验票数据" />&nbsp;&nbsp;
                <asp:Button CssClass="inputButtonL" Width="150px" ID="btnSingleCheck" runat="server" Text="单机验票" />&nbsp;&nbsp;
                <asp:Button CssClass="inputButtonL" Width="150px" ID="btnUnionCheck" runat="server" Text="多机验票" />
            </div>
        </div>--%>
</asp:Content>
