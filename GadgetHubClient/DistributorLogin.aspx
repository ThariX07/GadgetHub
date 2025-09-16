<%@ Page Title="Distributor Login" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="DistributorLogin.aspx.cs" Inherits="GadgetHubClient.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="products-styles.css" rel="stylesheet" type="text/css" />

    <style>
        .auth-wrapper{
            max-width: 960px; margin: 2rem auto; padding: 0 1rem;
        }
        .auth-hero{
            background: linear-gradient(135deg,#667eea 0%,#764ba2 100%);
            color:#fff; border-radius:12px; padding:2rem 1.75rem; margin-bottom:1.25rem;
            box-shadow:0 4px 10px rgba(0,0,0,.08);
            display:flex; align-items:center; justify-content:space-between; gap:1rem;
        }
        .auth-hero h2{ margin:0 0 .35rem; font-size:2rem; }
        .auth-hero p{ margin:0; opacity:.95 }
        .auth-grid{
            display:grid; grid-template-columns: 1fr 1fr; gap:1.25rem;
        }
        .auth-card{
            background:#fff; border:1px solid #e2e8f0; border-radius:12px;
            padding:1.5rem; box-shadow:0 1px 3px rgba(0,0,0,.06);
        }
        .auth-title{ font-size:1.25rem; font-weight:600; color:#1e293b; margin-bottom:.75rem; }
        .form-label{ display:block; color:#374151; font-weight:500; margin:.5rem 0 .35rem; }
        .form-input{
            width:100%; padding:.75rem 1rem; border:2px solid #e2e8f0; border-radius:8px;
            transition:border-color .15s; font-size:1rem;
        }
        .form-input:focus{ outline:none; border-color:#667eea; box-shadow:0 0 0 3px rgba(102,126,234,.12); }
        .btn-wide{ width:100%; margin-top:1rem; }
        .message{ display:block; margin-top:.75rem; }
        @media (max-width:900px){ .auth-grid{ grid-template-columns:1fr; } }
    </style>

    <div class="auth-wrapper">
        <div class="auth-hero">
            <div>
                <h2>Hello, Distributor 🧰</h2>
                <p>Sign in to view pending requests, submit quotes, and manage confirmed orders.</p>
            </div>
        </div>

        <div class="auth-grid">
            <div class="auth-card">
                <div class="auth-title">Distributor Login</div>

                <label for="txtEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" />

                <label for="txtPassword" class="form-label">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" />

                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-primary btn-wide" OnClick="btnLogin_Click" />
                <asp:Label ID="lblMessage" runat="server" CssClass="message message-error"></asp:Label>
            </div>

            <div class="auth-card">
                <div class="auth-title">What you can do</div>
                <p class="product-description">• See all customer requests awaiting your quote.</p>
                <p class="product-description">• Submit prices and win orders quickly.</p>
                <p class="product-description">• View confirmed orders and complete fulfillment.</p>
            </div>
        </div>
    </div>
</asp:Content>
