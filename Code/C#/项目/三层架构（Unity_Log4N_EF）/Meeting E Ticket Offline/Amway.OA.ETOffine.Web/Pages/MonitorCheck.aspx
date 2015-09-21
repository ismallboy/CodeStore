<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="MonitorCheck.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.MonitorCheck" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/ETOffine/MonitorCheck.js"></script>
    <style type="text/css">
        .PageMain {
            padding: 0;
        }

        html {
            overflow: scroll;
        }

        body {
            height:auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
         <input id="reLoadTime" type="hidden" value="30000" runat="server" />
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a id="reIndex" class="fLeft page-logo"></a>
                <span>验票情况</span>
                <a id="reBack" class="fRight page-back"></a>
            </div>
            <div class="PageMain clearfix">
                <div class="dataLeft box fLeft">
                    <ul class="dataIPbox clearfix">
                        <asp:Repeater ID="rptIpChecked" runat="server">
                            <ItemTemplate>
                                <li>
                                    <div class="checkmanage-IP">IP：<%#Eval("CheckIP") %><br />
                                        名称：  <%#Eval("MachineName") %></div>
                                    <div class="checkmanage-cont">
                                        <table width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>正常票：</td>
                                                <td><span class="fontGreen w30"><%#Eval("TotalOK") %></span></td>
                                            </tr>
                                            <tr>
                                                <td>异常票：</td>
                                                <td><span class="fontRed w30"><%#Eval("TotalErr") %></span></td>
                                            </tr>
                                            <tr>
                                                <td>最近验票时间：</td>
                                                <td><span class="w30"><%#Eval("NewCheckTime") %></span></td>
                                            </tr>
                                        </table>
                                    </div>
                                </li>

                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                    <div class="dataInfo">
                        <asp:Literal ID="ltrLog" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="dataRight box fRight">
                    <div class="Datebox tCenter">
                        <p class="fontWhite w36"><%=DateTime.Now.ToString("yyyy-MM-dd") %></asp:Literal></p>
                        <p class="fontlGreen w24"><%=GetWeekName(DateTime.Today.DayOfWeek) %></p>
                    </div>
                    <div class="Nobox tCenter">
                        <p class="fontlGreen w24">会议编号</p>
                        <p class="fontWhite w32"><asp:Literal ID="ltrActivitySN" runat="server"></asp:Literal></p>
                    </div>
                    <div class="Countbox tCenter">
                        <p class="fontlGreen w24">门票总数</p>
                        <p class="fontWhite wBfont"><asp:Literal ID="ltrTicketCount" runat="server"></asp:Literal></p>
                    </div>
                    <div class="INbox tCenter clearfix">
                        <div class="INbox-l fLeft box">
                            <p class="fontlGreen w24">已入场数</p>
                            <p class="fontDGreen wBfont"><asp:Literal ID="ltrEntranceCount" runat="server"></asp:Literal></p>
                        </div>
                        <div class="INbox-r fRight">
                            <p class="fontlGreen w24">入场率</p>
                            <p class="fontDGreen wBfont"><asp:Literal ID="ltrEntrancePercent" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                    <div class="Unnormalbox tCenter">
                        <p class="fontlGreen w24">异常票数</p>
                        <p class="fontLRed wBfont"><asp:Literal ID="ltrError" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
