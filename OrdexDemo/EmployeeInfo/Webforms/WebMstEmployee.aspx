<%@ Page Title="" Language="C#" MasterPageFile="~/Webforms/Site1.Master" AutoEventWireup="true" CodeBehind="WebMstEmployee.aspx.cs" Inherits="EmployeeInfo.Webforms.WebMstEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
    function IsNumeric(e) {
        var keyCode = (e.which) ? e.which : e.keyCode;
        if ((keyCode >= 48 && keyCode <= 57) || (keyCode == 8))
            return true;
        else if (keyCode == 46) {
            return false;
        }
        else
            return false;
    }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="sm" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id="progressBackgroundFilter">
                        </div>
                        <div id="processMessage">
                            <asp:Image ID="Image1" runat="server" ImageAlign="Middle" ImageUrl="~/img/loader.gif" />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

            </div>
            <div class="table-heading">
                <div class="container bookingdetail">
                    <p class="formtitle">Employee</p>
                </div>
            </div>
            <div class="mainform" style="margin-bottom: 30px;">
                <div class="container borcontainer">
                    <div class="row row1">
                        <div class="col-md-6">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-inline">
                                            <label class="searchname" for="name" style="min-width: 110px">Id:</label>
                                            <asp:TextBox ID="txtId" runat="server" type="text" CssClass="form-control" TabIndex="1" Enabled="false" AutoComplete="off" onkeypress="return IsNumeric(event);"></asp:TextBox>
                                            
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-inline">
                                            <label class="searchname" for="name" style="min-width: 110px">Name:</label>
                                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" type="text" TabIndex="2" AutoComplete="off" ValidationGroup="Emp"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtName" ValidationGroup="Emp"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="form-inline">
                                            <label class="searchname" for="name" style="min-width: 110px">Age:</label>
                                            <asp:TextBox ID="txtAge" runat="server" type="text" TabIndex="3" CssClass="form-control" AutoComplete="off" onkeypress="return IsNumeric(event);"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                             <div class="row cap5">
                                <div class="col-xs-12 col-lg-12 col-md-12">
                                    <div class="table-responsive">
                                        <asp:GridView ID="grd_F_Add" runat="server" CssClass="table table2" ShowFooter="True" Style="border: 3px solid #0094da; margin-bottom: 0px; border-bottom: 0px" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No."></asp:TemplateField>
                                                <asp:TemplateField HeaderText="Address">
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txt_F_Address" TextMode="MultiLine" Width="383px" TabIndex="4" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Action">
                                                    <FooterTemplate>
                                                        <asp:Button ID="btn_add" runat="server" class="btn adddelete" TabIndex="5" Text="Add" OnClick="btn_add_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:GridView ID="grd_I_Add" runat="server" CssClass="table table2" ShowFooter="false" ShowHeader="false" OnRowDeleting="grd_I_Add_RowDeleting" Style="border: 3px solid #0094da; border-top: 0px" AutoGenerateColumns="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr No.">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRowNumber" Width="42px" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Code">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_I_Address" TextMode="MultiLine" Width="383px" Text='<%# Eval("Address") %>' TabIndex="6" CssClass="form-control" runat="server"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnk_btn_delete" runat="server" class="btn adddelete" TabIndex="7" CommandName="Delete">Delete</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                            <div class="row btngroup" style="margin-left: -20px;">
                                <div class="col-md-12" style="text-align: center">
                                    <asp:Button ID="btn_Submit" runat="server" CssClass="btn scb" Text="Submit" TabIndex="8" OnClick="btn_Submit_Click" ValidationGroup="Emp"/>
                                    <asp:Button ID="btn_Cancel" runat="server" CssClass="btn scb" Text="Cancel" TabIndex="9" OnClick="btn_Cancel_Click" />

                                </div>
                            </div>

                           
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
