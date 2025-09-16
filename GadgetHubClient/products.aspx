<%@ Page Title="Customer Products" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="GadgetHubClient.products" %>
<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Include the custom CSS -->
    <link href="products-styles.css" rel="stylesheet" type="text/css" />

    <style>
        .page-header .header-title { text-align: center; margin: 0 auto; }
        .page-header .header-actions { position: absolute; top: 8px; right: 20px; z-index: 1000; }
    </style>
    
    <div class="container">
        <!-- Page Header -->
        <div class="page-header">
            <div class="header-row">
                <div class="header-title">
                    <h2> Available Products</h2>
                    <p>Browse and select products for your business needs</p>
                </div>
                <div style="position: absolute; top: 8px; right: 20px; z-index: 1000;">
                    <asp:Button ID="btnProfile" runat="server" Text="Profile" CssClass="btn-primary" OnClientClick="openProfileModal(); return false;" />
                    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn-order" OnClick="btnLogout_Click" />
                </div>
            </div>
        </div>

        <!-- Search Section (Optional Enhancement) -->
        <div class="search-controls">
            <input type="text" class="search-input" placeholder="Search products..." />
            <select class="filter-dropdown">
                <option>All Categories</option>
                <option>Electronics</option>
                <option>Computers</option>
                <option>Accessories</option>
            </select>
            <button type="button" class="btn-view-quotes" onclick="scrollToQuotes()">View Quotations</button>
        </div>

        <!-- Original GridView (Hidden but preserved for functionality) -->
        <asp:GridView ID="gvProducts" runat="server" AutoGenerateColumns="False" 
                      OnSelectedIndexChanged="gvProducts_SelectedIndexChanged" 
                      CssClass="GridView hidden">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Description" HeaderText="Description" />
                <asp:BoundField DataField="ImageUrl" HeaderText="Image URL" />
            </Columns>
        </asp:GridView>

        <!-- Custom Products Grid -->
        <div class="products-grid custom-gridview" id="customProductsGrid">
            <!-- Products will be dynamically populated here -->
            <!-- Sample Product Card Structure -->
            <div class="product-card" style="display: none;">
                <div class="product-image">
                    <img src="" alt="Product Image" />
                </div>
                <div class="product-content">
                    <div class="product-id">ID: <span class="id-value"></span></div>
                    <h3 class="product-title"></h3>
                    <p class="product-description"></p>
                    <div class="product-actions">
                        <button type="button" class="btn-select" onclick="selectProduct(this)">
                            Select Product
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Request Section -->
        <div class="request-section">
            <h3 class="section-title">Submit Request</h3>
            
            <!-- Selected Product Info -->
            <div class="form-group">
                <asp:Label ID="lblProductId" runat="server" Text="Selected Product ID: " 
                           CssClass="form-label" Visible="False"></asp:Label>
                <asp:Label ID="lblSelectedProductId" runat="server" 
                           CssClass="quote-amount" Visible="False"></asp:Label>
            </div>

            <!-- Quantity Input -->
            <div class="form-group">
                <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" 
                           CssClass="form-label"></asp:Label>
                <asp:TextBox ID="txtQuantity" runat="server" Text="1" 
                             CssClass="form-input" TextMode="Number" min="1"></asp:TextBox>
            </div>

            <!-- Submit Button -->
            <div class="form-group">
                <asp:Button ID="btnSubmitRequest" runat="server" Text="Submit Request" 
                            OnClick="btnSubmitRequest_Click" CssClass="btn-primary" Enabled="False" />
            </div>

            <!-- Request Message -->
            <asp:Label ID="lblRequestMessage" runat="server" CssClass="message message-error"></asp:Label>
        </div>

        <!-- Quote Section -->
        <div class="quote-section">
            <h3 class="section-title">Current Quotes</h3>
            
            <!-- Quote Information -->
            <div class="quote-info">
                <asp:Label ID="lblCheapestQuote" runat="server" Text="Cheapest Quote: N/A" 
                           CssClass="quote-amount"></asp:Label>
            </div>

            <!-- Place Order Button -->
            <div class="form-group">
                <asp:Button ID="btnPlaceOrder" runat="server" Text="Place Order" 
                            OnClick="btnPlaceOrder_Click" Enabled="False" CssClass="btn-order" />
            </div>

            <!-- Order Message -->
            <asp:Label ID="lblOrderMessage" runat="server" CssClass="message message-error"></asp:Label>
        </div>
    </div>

    <!-- Profile Modal -->
    <div id="profileModal" class="modal">
        <div class="modal-content">
            <span class="close" onclick="closeProfileModal()">&times;</span>
            <h3 style="margin-bottom: 1rem;">My Profile</h3>

            <div class="form-group">
                <label for="txtProfileName" class="form-label">Name:</label>
                <asp:TextBox ID="txtProfileName" runat="server" CssClass="form-input" />
            </div>

            <div class="form-group">
                <label for="txtProfileEmail" class="form-label">Email:</label>
                <asp:TextBox ID="txtProfileEmail" runat="server" CssClass="form-input" TextMode="Email" />
            </div>

            <div class="form-group">
                <label for="txtProfilePassword" class="form-label">Password:</label>
                <asp:TextBox ID="txtProfilePassword" runat="server" CssClass="form-input" TextMode="Password" />
            </div>

            <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes" CssClass="btn-primary" OnClick="btnSaveProfile_Click" />
            <asp:Label ID="lblProfileMessage" runat="server" CssClass="message" />
        </div>
    </div>

    <!-- JavaScript for Enhanced Functionality -->
    <script type="text/javascript">
        // Base URL of your API (serves /images/*). Adjust if your port changes.
        const API_BASE = "https://localhost:44376";

        // Function to populate custom product grid from GridView data
        function populateCustomGrid() {
            var gridView = document.getElementById("<%= gvProducts.ClientID %>");
            var customGrid = document.getElementById("customProductsGrid");
            var template = customGrid.querySelector(".product-card");

            if (!gridView || !gridView.rows) return;

            // Clear existing cards (except template)
            var existingCards = customGrid.querySelectorAll('.product-card:not([style*="display: none"])');
            existingCards.forEach(card => card.remove());

            // Iterate through GridView rows (skip header)
            for (var i = 1; i < gridView.rows.length; i++) {
                var row = gridView.rows[i];
                var cells = row.cells;

                if (cells.length >= 5) {
                    // Create new card from template
                    var newCard = template.cloneNode(true);
                    newCard.style.display = "block";

                    // Extract data from GridView cells
                    var id = cells[1].innerText || cells[1].textContent;
                    var name = cells[2].innerText || cells[2].textContent;
                    var description = cells[3].innerText || cells[3].textContent;
                    var imageUrl = cells[4].innerText || cells[4].textContent;

                    // Populate card data
                    newCard.querySelector(".id-value").textContent = id;
                    newCard.querySelector(".product-title").textContent = name;
                    newCard.querySelector(".product-description").textContent = description;

                    // Handle image (supports full URLs or filenames stored in DB)
                    var img = newCard.querySelector(".product-image img");
                    if (imageUrl && imageUrl.trim() !== "") {
                        var val = imageUrl.trim();

                        // If not absolute URL, treat it as a filename under API /images
                        if (!/^https?:\/\//i.test(val)) {
                            // remove any leading slashes
                            val = API_BASE + "/images/" + val.replace(/^\/+/, "");
                        }

                        img.src = val;
                        img.alt = name + " image";
                        img.style.display = "block";
                        img.parentElement.style.background = "none";
                    } else {
                        img.style.display = "none";
                    }

                    // Set up select button
                    var selectBtn = newCard.querySelector(".btn-select");
                    selectBtn.setAttribute("data-row-index", i - 1);
                    selectBtn.setAttribute("data-product-id", id);
                    selectBtn.setAttribute("data-product-name", name);

                    // Add to grid
                    customGrid.appendChild(newCard);
                }
            }
        }

        function scrollToQuotes() {
            const quoteSection = document.querySelector(".quote-section");
            if (quoteSection) {
                quoteSection.scrollIntoView({ behavior: "smooth", block: "start" });
            }
        }

        function openProfileModal() {
            document.getElementById('profileModal').style.display = 'block';
        }

        function closeProfileModal() {
            document.getElementById('profileModal').style.display = 'none';
        }

        // Optional: Close when clicking outside the modal
        window.onclick = function (event) {
            var modal = document.getElementById('profileModal');
            if (event.target == modal) {
                modal.style.display = "none";
            }
        };

        // Function to handle product selection
        function selectProduct(button) {
            var rowIndex = button.getAttribute("data-row-index");
            var productId = button.getAttribute("data-product-id");
            var productName = button.getAttribute("data-product-name");

            // Update selected product labels
            var lblProductId = document.getElementById("<%= lblProductId.ClientID %>");
            var lblSelectedProductId = document.getElementById("<%= lblSelectedProductId.ClientID %>");

            if (lblProductId && lblSelectedProductId) {
                lblProductId.style.display = "block";
                lblSelectedProductId.style.display = "block";
                lblSelectedProductId.textContent = productId + " - " + productName;

                // Ensure visibility in CSS class
                lblProductId.className = "form-label";
                lblSelectedProductId.className = "quote-amount";
            }

            // Trigger server-side selection via hidden field and postback
            var hiddenField = document.getElementById("<%= hfSelectedProductId.ClientID %>");
            if (hiddenField) {
                hiddenField.value = productId;
                __doPostBack("<%= gvProducts.UniqueID %>", "Select$" + rowIndex);
            }
            
            // Store scroll target for postback
            sessionStorage.setItem("scrollTarget", ".request-section");
            
            // Visual feedback
            document.querySelectorAll(".btn-select").forEach(btn => {
                btn.textContent = "Select Product";
                btn.style.background = "linear-gradient(135deg, #667eea 0%, #764ba2 100%)";
            });
            
            button.textContent = "Selected ✓";
            button.style.background = "linear-gradient(135deg, #10b981 0%, #059669 100%)";
        }
        
        // Function to handle postback scroll
        function scrollAfterPostback() {
            var scrollTarget = sessionStorage.getItem("scrollTarget");
            if (scrollTarget) {
                var target = document.querySelector(scrollTarget);
                if (target) {
                    target.scrollIntoView({ behavior: "smooth", block: "start" });
                }
                sessionStorage.removeItem("scrollTarget");
            }
        }
        
        // Function to update message styling
        function updateMessageStyling() {
            var requestMsg = document.getElementById("<%= lblRequestMessage.ClientID %>");
            var orderMsg = document.getElementById("<%= lblOrderMessage.ClientID %>");

            [requestMsg, orderMsg].forEach(function (msg) {
                if (msg && msg.textContent.trim() !== "") {
                    if (msg.textContent.toLowerCase().includes("success") ||
                        msg.textContent.toLowerCase().includes("completed")) {
                        msg.className = "message message-success";
                    } else {
                        msg.className = "message message-error";
                    }
                }
            });
        }

        // Initialize on page load
        document.addEventListener("DOMContentLoaded", function () {
            // Small delay to ensure GridView is populated
            setTimeout(function () {
                populateCustomGrid();
                updateMessageStyling();
                scrollAfterPostback(); // Scroll after initial load or postback
            }, 100);
        });

        // Re-populate and scroll after postback
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm) {
            prm.add_endRequest(function () {
                setTimeout(function () {
                    populateCustomGrid();
                    updateMessageStyling();
                    scrollAfterPostback(); // Scroll after async postback
                }, 100);
            });
        }
    </script>

    <!-- Hidden Field for Product Selection -->
    <asp:HiddenField ID="hfSelectedProductId" runat="server" Value="" />
</asp:Content>
