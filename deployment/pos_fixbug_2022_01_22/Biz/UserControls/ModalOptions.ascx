<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModalOptions.ascx.cs" Inherits="POS.LocalWeb.Biz.UserControls.ModalOptions" %>
<div id="modalOptions">
    <h2>Điều chỉnh</h2>
    <div class="panel panel-warning">
        <div class="panel-heading">
            <h3 class="panel-title">Hiển thị</h3>
        </div>
        <div class="panel-body">
            <div class="btn-group" role="group" aria-label="..." id="divColumnOptions">
                <button type="button" class="btn btn-default" id="btnColumnOption2" onclick="onClickBtnColumnOption('2');">2 Cột</button>
                <button type="button" class="btn btn-default" id="btnColumnOption3" onclick="onClickBtnColumnOption('3');">3 Cột</button>
                <button type="button" class="btn btn-default" id="btnColumnOption4" onclick="onClickBtnColumnOption('4');">4 Cột</button>
            </div>
        </div>
    </div>
    <br />
    <br />
    <br />
    <button type="button" class="btn btn-default" onclick="hideModalOptions();">Đóng</button>
    <button type="button" class="btn btn-danger pull-right" onclick="logout();">
        <span class="fa fa-sign-out"></span>Thoát
    </button>
</div>
<div style="display: none;">
    <asp:Button runat="server" ID="btnLogout" OnClick="BtnLogout" />
    <asp:Button runat="server" ID="btnSaveColumnOption" OnClick="BtnSaveColumnOption" />
    <asp:HiddenField runat="server" ID="txtColumnOption"/>
</div>
<script>
    function logout() {
        if (confirm("Bạn muốn thoát ?")) {
            <%=Page.ClientScript.GetPostBackClientHyperlink(btnLogout, "") %>;
        }
    }
    function onClickBtnColumnOption(number) {
        activeColumnOptionButton();
        setColumnOption(number);
    }
    function showModalOptions() {
        $('#modalOptions').show();
    }

    function hideModalOptions() {
        $('#modalOptions').hide();
    }

    $(document).ready(function () {
        activeColumnOptionButton();
<%=POS.LocalWeb.Dal.CacheContext.IsSetColumnOption() ? "" : "setColumnOption(getColumnOption());" %>
    });

    function getColumnOption() {
        if (!localStorage.columnOption) { setColumnOption('3'); }

        return localStorage.columnOption;
    }

    function setColumnOption(value) {
        localStorage.columnOption = value;
        $('#<%=txtColumnOption.ClientID %>').val(value);
        <%=Page.ClientScript.GetPostBackClientHyperlink(btnSaveColumnOption, "") %>
    }

    function activeColumnOptionButton() {
        $('#divColumnOptions').find('button').removeClass('active');
        $('#btnColumnOption' + getColumnOption()).addClass('active');
    }
</script>
