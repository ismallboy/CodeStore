<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true"
    CodeBehind="UnionSearchActivityList.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.UnionSearchActivityList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(function () {
            // 加入活动
            $("#ltnJoinActivity").click(function () {
                if (!IsSelectedActivity()) { return false; };
                return true;
            });
        });

        function IsSelectedActivity() {
            var selected = $("#lstAcivityList").val();
            if (selected == null) {
                alert("请选择一个活动");
                return false;
            } else {
                return true;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a href="../default.aspx" class="fLeft page-logo"></a>
                <span>查找其他活动</span>
                <a href="../Pages/UnionCheckingActivityList.aspx" class="fRight page-back"></a>
            </div>
            <div class="PageMain">
                <table width="100%" cellspacing="0" cellpadding="0" style="padding: 20px 0;">
                    <tr>
                        <td rowspan="2">
                            <asp:ListBox CssClass="selectbox" ID="lstAcivityList" runat="server"></asp:ListBox>
                        </td>
                        <td width="20" rowspan="2">&nbsp;</td>
                        <td width="180" align="center" class="bg-shadow verticalTop">
                            <asp:LinkButton ID="ltnJoinActivity" runat="server" CssClass="page-btnBig lhbig" OnClick="ltnJoinActivity_Click">加入</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</asp:Content>
