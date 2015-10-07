<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true"
    CodeBehind="UnionCheckingActivityList.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.UnionCheckingActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery-1.7.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            //验票
            $("#btnCheck").click(function () {
                if (!IsSelectedActivity()) { return false; };
                GotoPage("check");
                return false;
            });

            //监测验票情况
            $("#btnMonitor").click(function () {
                if (!IsSelectedActivity()) { return false; };
                GotoPage("monitor");
                return false;
            });



            //Del活动信息
            $("#lbtDel").click(function () {
                if (!IsSelectedActivity()) { return false; };
                if (confirm("确认移除活动吗？")) {
                    return true;
                } else {
                    return false;
                }
            });

            // 多机查找
            $("#btnUnionSearch").click(function () {
                window.location.href = "UnionSearchActivityList.aspx";
                return false;
            });
        });

        //查看活动信息
        function View() {
            if (IsSelectedActivity()) {
                GotoPage("view");
            }
        }

        function IsSelectedActivity() {
            var selected = $("#lstAcivityList").val();
            if (selected == null) {
                alert("请选择一个活动");
                return false;
            } else {
                return true;
            }

        }

        function GotoPage(t) {
            var url = "UnionRouter.aspx?RID=" + $("#lstAcivityList").val() + "&t=" + t;
            window.location.href = url;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
    <div class="PageContent box">
        <div class="PageHeader clearfix">
            <a href="../default.aspx" class="fLeft page-logo"></a><span>多机验票活动列表</span> <a href="../default.aspx"
                class="fRight page-back"></a>
        </div>
        <div class="PageMain">
            <table width="100%" cellpadding="0" cellspacing="0" border="0" class="searchbar">
                <tr>
                    <td width="30%" align="center" class="bg-shadow">
                        <asp:TextBox CssClass="page-input" ID="txtKey" runat="server"></asp:TextBox>
                    </td>
                    <td width="140" align="center" class="bg-shadow">
                        <div class="page-btn">
                            <span>
                                <asp:LinkButton ID="lbtSearch" runat="server" OnClick="lbtSearch_Click">查找</asp:LinkButton></span>
                            <span class="icon-arrow"></span>
                        </div>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
            <table width="100%" cellspacing="0" cellpadding="0" style="padding: 20px 0;">
                <tr>
                    <td rowspan="3">
                        <asp:ListBox CssClass="selectbox" ID="lstAcivityList" runat="server"></asp:ListBox>
                    </td>
                    <td width="20" rowspan="3">
                        &nbsp;
                    </td>
                    <td width="180" align="center" class="bg-shadow-union verticalTop">
                        <a class="page-btnBig-union" id="btnCheck">开始验票</a>
                    </td>
                </tr>
                <tr>
                    <td width="180" align="center" class="bg-shadow-union verticalTop">
                        <a class="page-btnBig-union" id="btnUnionSearch">查找其他活动</a>
                    </td>
                </tr>
                <tr>
                    <td width="180" align="center" class="bg-shadow-union verticalTop">
                        <a class="page-btnBig-union" id="btnMonitor">监视验票情况</a>
                    </td>
                </tr>
            </table>
            <div class="Activities-btn">
                <a onclick="View()" class="page-btn-blue">查看活动信息</a>
                <asp:LinkButton ID="lbtDel" CssClass="page-btn-blue" runat="server" OnClick="lbtDel_Click">移除活动</asp:LinkButton>
            </div>
        </div>
    </div>    
    </form>
</asp:Content>
