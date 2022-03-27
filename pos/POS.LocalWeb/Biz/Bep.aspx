<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Bep.aspx.cs" Inherits="POS.LocalWeb.Biz.Bep" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACE SOTF - BEP</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no"/>
    <link href="~/Content/bootstrap5/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/css/bep.min.css" rel="stylesheet" />
</head>
<body class="bep">
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Path="~/Content/bootstrap5/jquery.min.js" />
                <asp:ScriptReference Path="~/Content/bootstrap5/bootstrap.min.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <div>
            hello bep
        </div>
    </form>
</body>
</html>
