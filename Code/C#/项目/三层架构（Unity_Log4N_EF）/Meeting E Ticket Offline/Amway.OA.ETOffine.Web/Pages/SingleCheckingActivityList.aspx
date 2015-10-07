<%@ Page Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="SingleCheckingActivityList.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.SingleCheckingActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
    <script type="text/javascript">
        var hostIP = "http://localhost";
        var returnURL = "http://localhost/Pages/SingleCheckingActivityList.aspx";
        $(function () {
            //验票
            $("#btnCheck").click(function () {
                if (IsSelectedActivity()) {
                    window.location.href = "CheckTicket.aspx?RID=" + $("#lstAcivityList").val() + "&SINGLE=1&URL=" + returnURL;
                }
            });

            //监测验票情况
            $("#btnMonitor").click(function () {
                if (IsSelectedActivity()) {
                    window.location.href = "MonitorCheck.aspx?RID=" + $("#lstAcivityList").val() + "&URL=" + returnURL;
                }
            });

            //Del移除活动
            $("#lbtDel").click(function () {
                if (!IsSelectedActivity()) { return false; };
                if (confirm("确认移除活动吗？")) {
                    return true;
                } else {
                    return false;
                }
            });
        });

        function IsSelectedActivity() {
            var selected = $("#lstAcivityList").val();
            if (selected == null) {
                alert("请选择一个活动");
                return false;
            }
            return true;
        }

        //查看活动信息
        function View() {
            if (IsSelectedActivity()) {
                window.location.href = "ActivityInfo.aspx?RID=" + $("#lstAcivityList").val() + "&URL=" + returnURL;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a href="../default.aspx" class="fLeft page-logo"></a>
                <span>单机验票活动列表</span>
                <a href="../default.aspx" class="fRight page-back"></a>
            </div>
            <div class="PageMain">
                <table width="100%" cellpadding="0" cellspacing="0" border="0" class="searchbar">
                    <tr>
                        <td width="30%" align="center" class="bg-shadow">
                            <asp:TextBox CssClass="page-input" ID="txtKey" runat="server"></asp:TextBox></td>
                        <td width="140" align="center" class="bg-shadow">
                            <div class="page-btn">
                                <span>
                                    <asp:LinkButton ID="lbtSearch" runat="server" OnClick="lbtSearch_Click">查找</asp:LinkButton></span>
                                <span class="icon-arrow"></span>
                            </div>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <table width="100%" cellspacing="0" cellpadding="0" style="padding: 20px 0;">
                    <tr>
                        <td rowspan="2">
                            <asp:ListBox CssClass="selectbox" ID="lstAcivityList" runat="server"></asp:ListBox>
                        </td>
                        <td width="20" rowspan="2">&nbsp;</td>
                        <td width="180" align="center" class="bg-shadow verticalTop">
                            <a ID="btnCheck" Class="page-btnBig lhbig">验票</a>
                        </td>
                    </tr>
                     <tr>
                        <td width="180" align="center" class="bg-shadow verticalTop">
                            <a ID="btnMonitor"  Class="page-btnBig lhbig">监视验票情况</a>
                        </td>
                    </tr>
                </table>

                <div class="Activities-btn">
                    <a onclick="View()" class="page-btn-blue">查看活动信息</a>
                    <asp:LinkButton ID="lbtDel" CssClass="page-btn-blue" runat="server" OnClick="ltnDel_Click">移除活动</asp:LinkButton>
                </div>
            </div>
        </div>       
    </form>
</asp:Content>
