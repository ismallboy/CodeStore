<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="CheckTicketData.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.CheckTicketData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.4.1.js"></script>
    <script src="../Scripts/ETOffine/CheckTicketData.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="contentBorder">
            <div class="closeButton" onclick="window.location.href='../Default.aspx';">
                <img src="../Styles/Images/close.JPG" />
            </div>
            <div class="contentTitle">验票</div>
            <div class="checkDataContent">
                <div class="left">
                    <asp:TextBox CssClass="inputText" ID="txtKey" runat="server"></asp:TextBox>&nbsp;&nbsp;
                    <asp:Button CssClass="inputButtonL" ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="查找" />
                    <br />
                    <asp:ListBox CssClass="inputListBox" ID="lstAcivityList" runat="server"></asp:ListBox>
                    <br />
                    <asp:Button CssClass="inputButtonM" ID="btnViewActivityInfo" runat="server" Text="查看活动信息" />
                </div>
                <div class="right">
                    <asp:Button CssClass="inputButtonL" ID="btnCheck" runat="server" Text="离线验票" />
                    <br />
                    <asp:Button CssClass="inputButtonL" ID="btnMonitor" runat="server" Text="监视验票情况" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
