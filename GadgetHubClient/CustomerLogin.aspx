<%@ Page Title="Customer Login" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="CustomerLogin.aspx.cs"
    Inherits="GadgetHubClient.CustomerLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="products-styles.css" rel="stylesheet" type="text/css" />

    <style>
        .auth-wrapper{ max-width: 760px; margin: 2rem auto; padding: 0 1rem; }
        .auth-hero{
            background: linear-gradient(135deg,#667eea 0%,#764ba2 100%);
            color:#fff; border-radius:12px; padding:1.5rem 1.25rem; margin-bottom:1rem;
            box-shadow:0 4px 10px rgba(0,0,0,.08);
        }
        .auth-hero h2{ margin:.1rem 0 .25rem; font-size:2rem; }
        .auth-card{
            background:#fff; border:1px solid #e2e8f0; border-radius:12px;
            padding:1.5rem; box-shadow:0 1px 3px rgba(0,0,0,.06);
        }
        .auth-actions{ display:flex; gap:.75rem; flex-wrap:wrap; }
        .btn-ghost {
            background: transparent; border:2px solid #e2e8f0; color:#374151;
            padding:.8rem 1.2rem; border-radius:8px; font-weight:500; cursor:pointer;
            transition: all .15s ease;
        }
        .btn-ghost:hover{ border-color:#667eea; color:#1e293b; }
        .form-label{ display:block; color:#374151; font-weight:500; margin:.5rem 0 .35rem; }
        .form-input{
            width:100%; padding:.75rem 1rem; border:2px solid #e2e8f0; border-radius:8px;
            transition:border-color .15s; font-size:1rem;
        }
        .form-input:focus{ outline:none; border-color:#667eea; box-shadow:0 0 0 3px rgba(102,126,234,.12); }
        .btn-wide{ width:100%; margin-top:1rem; }
        .message{ display:block; margin-top:.75rem; }

        /* Make the modal body nicely centered */
        .modal-content .form-row { margin-bottom: .75rem; }
        .modal .form-label { text-align:center; }
        .modal .form-input { text-align:center; }
        .modal .actions { display:flex; gap:.5rem; justify-content:center; margin-top:.75rem; }

        /* Ghost button consistent with theme */
.btn-ghost.register-btn {
    background: transparent;
    border: 2px solid rgba(255,255,255,0.85);
    color: #fff;
    padding: 0.8rem 1.5rem;
    border-radius: 8px;
    font-weight: 500;
    cursor: pointer;
    transition: all 0.15s ease;
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
}
.btn-ghost.register-btn:hover {
    background: rgba(255,255,255,0.12);
    transform: translateY(-1px);
}

/* If your login form background is white, invert ghost colors */
.auth-card .btn-ghost.register-btn {
    border-color: #667eea;
    color: #667eea;
}
.auth-card .btn-ghost.register-btn:hover {
    background: rgba(102,126,234,0.1);
}

    </style>

    <div class="auth-wrapper">
        <div class="auth-hero">
            <h2>Customer Login</h2>
            <p>Sign in to browse products, request quotes, and place orders.</p>
        </div>

        <div class="auth-card">
            <label for="txtEmail" class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" />

            <label for="txtPassword" class="form-label">Password</label>
            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password" />

            <div class="auth-actions">
    <asp:Button ID="btnLogin" runat="server" Text="Login" 
        CssClass="btn-primary" OnClick="btnLogin_Click" />

    <!-- Ghost-style Register button -->
    <button type="button" class="btn-ghost register-btn" onclick="openReg()">
        <i class="fas fa-user-plus"></i> Register as Customer
    </button>
</div>


            <asp:Label ID="lblMessage" runat="server" CssClass="message message-error"></asp:Label>
            <asp:Label ID="lblRegisterMessage" runat="server" CssClass="message"></asp:Label>
        </div>
    </div>

    <!-- REGISTER MODAL -->
    <div id="regModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeReg()">&times;</span>
            <h3 style="text-align:center; margin-top:.25rem;">Create your customer account</h3>

            <div class="form-row">
                <label for="txtRegName" class="form-label">Full Name</label>
                <asp:TextBox ID="txtRegName" runat="server" CssClass="form-input" />
            </div>
            <div class="form-row">
                <label for="txtRegEmail" class="form-label">Email</label>
                <asp:TextBox ID="txtRegEmail" runat="server" CssClass="form-input" TextMode="Email" />
            </div>
            <div class="form-row">
                <label for="txtRegPassword" class="form-label">Password</label>
                <asp:TextBox ID="txtRegPassword" runat="server" CssClass="form-input" TextMode="Password" />
            </div>

            <div class="actions">
                <asp:Button ID="btnRegisterSubmit" runat="server" Text="Register"
                    CssClass="btn-primary" OnClick="btnRegisterSubmit_Click" />
                <button type="button" class="btn-ghost" onclick="closeReg()">Cancel</button>
            </div>
        </div>
    </div>

    <script>
        function openReg(){ document.getElementById('regModal').style.display = 'block'; }
        function closeReg(){ document.getElementById('regModal').style.display = 'none'; }
        window.addEventListener('click', function(e){
            var m = document.getElementById('regModal');
            if(e.target === m){ m.style.display='none'; }
        });
    </script>
</asp:Content>
