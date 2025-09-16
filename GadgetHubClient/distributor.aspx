<%@ Page Title="Distributor Dashboard" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="distributor.aspx.cs" Inherits="GadgetHubClient.distributor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="products-styles.css" rel="stylesheet" type="text/css" />

    <!-- Header -->
    <div class="page-header">
        <h2>Distributor Dashboard</h2>
        <p>Manage orders and add new products</p>
    </div>
               <div style="position: absolute; top: 8px; right: 20px; z-index: 1000;">
    
    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn-order" OnClick="btnDistributorLogout_Click" />
</div>

    <div class="container">
        <!-- Pending Orders Section -->
        <div class="section">
            <h3 class="section-title">Pending Customer Orders</h3>
            <asp:GridView ID="gvPendingOrders" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="gvPendingOrders_SelectedIndexChanged" CssClass="GridView">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Id" HeaderText="Order ID" />
                    <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="CustomerId" HeaderText="Customer ID" />
                    
                    <asp:BoundField DataField="RequestDate" HeaderText="Request Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    
                </Columns>
            </asp:GridView>
        </div>

        <!-- Submit Quote Section -->
        <div class="section">
            <h3 class="section-title">Submit Quote</h3>
            <asp:Label ID="lblSelectedOrderId" runat="server" CssClass="form-label" Text="Selected Order ID: " Visible="False"></asp:Label>
            <div class="form-group">
                <asp:TextBox ID="txtQuotePrice" runat="server" CssClass="form-input" placeholder="Enter quote price" TextMode="Number" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnSubmitQuote" runat="server" Text="Submit Quote" CssClass="btn-primary" OnClick="btnSubmitQuote_Click" />
            </div>
            <asp:Label ID="lblQuoteMessage" runat="server" CssClass="message" />
        </div>

        <!-- Add Product Section -->
        <!-- Add Product Section -->
<div class="section">
    <h3 class="section-title">Add New Product</h3>
    <table class="custom-gridview">
        <tr>
            <td><asp:TextBox ID="txtProductName" runat="server" CssClass="form-input" placeholder="Product Name" /></td>
            <td><asp:TextBox ID="txtProductDescription" runat="server" CssClass="form-input" placeholder="Description" /></td>
            <td>
                <asp:DropDownList ID="ddlProductCategory" runat="server" CssClass="form-input">
                    <asp:ListItem Text="Home" />
                    <asp:ListItem Text="Business" />
                    <asp:ListItem Text="Computer Accessories" />
                </asp:DropDownList>
            </td>
            <!-- NEW: image upload -->
            <td>
                <asp:FileUpload ID="fuProductImage" runat="server" CssClass="form-input" />
                <small style="display:block; color:#64748b;">PNG/JPG up to 5MB</small>
            </td>
            <td><asp:Button ID="btnAddProduct" runat="server" Text="Confirm" CssClass="btn-order" OnClick="btnAddProduct_Click" /></td>
        </tr>
    </table>
    <asp:Label ID="lblProductAddMessage" runat="server" CssClass="message" />
</div>


       

        <!-- Confirmed Orders Grid -->
        <div class="section">
            <h4 class="section-title">Confirmed Orders</h4>
            <asp:GridView ID="gvConfirmedOrders" runat="server" AutoGenerateColumns="False" OnRowCommand="gvConfirmedOrders_RowCommand" CssClass="GridView">
    <Columns>
        <asp:BoundField DataField="OrderId" HeaderText="Order ID" />
        <asp:BoundField DataField="ProductId" HeaderText="Product ID" />
        <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
        <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
        <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="${0:N2}" />
        <asp:ButtonField Text="Complete Order" CommandName="CompleteOrder" ButtonType="Button" />
    </Columns>
</asp:GridView>

            <asp:Label ID="lblConfirmedOrdersMessage" runat="server" CssClass="message" />
        </div>
    </div>
</asp:Content>
