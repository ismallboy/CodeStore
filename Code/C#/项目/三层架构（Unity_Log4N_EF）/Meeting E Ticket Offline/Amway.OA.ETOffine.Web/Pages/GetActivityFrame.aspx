<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetActivityFrame.aspx.cs" Inherits="Amway.OA.ETOffine.Web.Pages.GetActivityFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EDGE">
    <title>在线下载门票数据</title>
    <script src="../Scripts/jquery-1.7.1.min.js"></script>
    <script src="../Scripts/ETOffine/ComFunction.js"></script>
    <script src="../Scripts/json2.js"></script>
    <script src="../Scripts/ETOffine/GetActivityFrame.js"></script>
    <style type="text/css">
        body{
            margin: 0px;
            padding: 0px;         
        }
    </style>
</head>
<body style="background-color:#022237;">
    <form id="form1" runat="server">
        <asp:HiddenField ID="hidQueryUrl" runat="server" />
        <div id="divIFrame" style="background-color:#022237;">
        </div>
    </form>
</body>
</html>