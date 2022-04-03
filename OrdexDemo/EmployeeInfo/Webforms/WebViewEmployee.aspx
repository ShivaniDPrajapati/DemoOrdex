<%@ Page Title="" Language="C#" MasterPageFile="~/Webforms/Site1.Master" AutoEventWireup="true" CodeBehind="WebViewEmployee.aspx.cs" Inherits="EmployeeInfo.Webforms.WebViewEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table-responsive tr th:last-child {
    text-align: left;
}

.table-responsive tr td:last-child {
    text-align: left;
}
    </style>
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
                    <p class="formtitle">View Employees</p>
                </div>
            </div>
            <div class="mainform" style="margin-bottom: 30px;">
                <div class="container borcontainer">
                    <div class="row row1">
                        <div class="col-md-12">
                            <div class="">
                                <div class="row cap5" style="overflow-x: auto">
                                    <div class="col-xs-12 col-lg-12 col-md-12">
                                        <div class="table-responsive">
                                            <asp:GridView class="table" ID="GrdEmployee" runat="server" Style="border: 3px solid #0094da;" OnDataBound="GrdEmployee_DataBound" GridLines="None" AutoGenerateColumns="false" PageSize="15" Width="100%" OnPageIndexChanging="GrdEmployee_PageIndexChanging" ShowFooter="false" AllowPaging="true">
                                                <Columns>
                                                    <asp:BoundField DataField="Empid" HeaderText="Id" ItemStyle-Width="150" />
                                                    <asp:BoundField DataField="empname" HeaderText="Name" ItemStyle-Width="150" />
                                                    <asp:BoundField DataField="empage" HeaderText="Age" ItemStyle-Width="150" />
                                                    <asp:BoundField DataField="address" HeaderText="Address" ItemStyle-Width="150" />
                                                    
                                                </Columns>
                                                <EmptyDataTemplate>No records found</EmptyDataTemplate>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
