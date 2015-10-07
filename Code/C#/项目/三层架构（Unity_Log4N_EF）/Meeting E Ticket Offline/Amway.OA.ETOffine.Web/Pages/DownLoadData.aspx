<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="DownLoadData.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.DownLoadData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/ETOffine/DownLoadData.js"></script>
    <script type="text/javascript">
        $(function () {
            //div自定义高度
            document.getElementById('lstAcivityList').style.height = (document.documentElement.clientHeight) / 2 - 50 + 'px';

        });
        window.onresize = function () {
            document.getElementById('lstAcivityList').style.height = (document.documentElement.clientHeight) / 2 - 50 + 'px';
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="PageContent box">
            <div class="PageHeader clearfix">
                <a href="../default.aspx" class="fLeft page-logo"></a>
                <span>下载门票数据</span>
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
                            <asp:LinkButton ID="lbtUpdate" runat="server" CssClass="page-btnBig lhbig" OnClick="lbtUpdate_Click">更新门票数据</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>

                <div class="Activities-btn">
                    <a onclick="DownLoadData.btnView()" class="page-btn-blue">查看活动信息</a>
                    <a onclick="DownLoadData.btnAdd()" class="page-btn-blue">添加活动</a>
                    <asp:LinkButton ID="lbtDel" CssClass="page-btn-blue" runat="server" OnClick="ltnDel_Click">移除活动</asp:LinkButton>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
