<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="ActivityInfo.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.ActivityInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/ETOffine/ActivityInfo.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" id="form1">
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a href="../default.aspx" class="fLeft page-logo"></a>
                <span>活动详情</span>
                <a id="reBack" class="fRight page-back"></a>
            </div>

            <div class="PageMain">
                <table width="100%" cellspacing="0" cellpadding="0" border="1" bordercolor="#9ca6ae" class="Activities w16" style="border-collapse: collapse">
                    <tr>
                        <td class="tdbg">活动编号：</td>
                        <td>
                            <asp:Label ID="lblActivitySN" runat="server" Text=""></asp:Label></td>
                        <td class="tdbg">活动名称：</td>
                        <td>
                            <asp:Label ID="lblActivityName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tdbg">活动城市：</td>
                        <td>
                            <asp:Label ID="lblActivityCity" runat="server" Text=""></asp:Label></td>
                        <td class="tdbg">举办场所：</td>
                        <td>
                            <asp:Label ID="lblActivityAddress" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tdbg">活动开始时间：</td>
                        <td>
                            <asp:Label ID="lblStartTime" runat="server" Text=""></asp:Label></td>
                        <td class="tdbg">活动结束时间：</td>
                        <td>
                            <asp:Label ID="lblActivityFinishTime" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tdbg">活动类别：</td>
                        <td>
                            <asp:Label ID="lblActivityCategory" runat="server" Text=""></asp:Label></td>
                        <td class="tdbg">活动明细：</td>
                        <td>
                            <asp:Label ID="lblActivityDetail" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tdbg">活动子明细：</td>
                        <td>
                            <asp:Label ID="lblActivityChildDetail" runat="server" Text=""></asp:Label></td>
                        <td class="tdbg">活动人数：</td>
                        <td>
                            <asp:Label ID="lblActivityCount" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</asp:Content>
