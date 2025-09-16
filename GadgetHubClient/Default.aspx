<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GadgetHubClient._Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Reuse the shared styles -->
    <link href="products-styles.css" rel="stylesheet" type="text/css" />

    <!-- Page-specific styles (lightweight) -->
    <style>
        .hero {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: #fff;
            border-radius: 12px;
            padding: 3rem 2rem;
            margin-bottom: 1.5rem;
            display: grid;
            grid-template-columns: 1.2fr 1fr;
            gap: 2rem;
            align-items: center;
            box-shadow: 0 4px 6px rgba(0,0,0,.08);
        }
        .hero h1 { font-size: 2.5rem; margin: 0 0 .75rem; }
        .hero p { font-size: 1.05rem; opacity: .95; margin: 0 0 1.25rem; }
        .hero-cta { display: flex; gap: .75rem; flex-wrap: wrap; }
        .btn-ghost {
            background: transparent;
            border: 2px solid rgba(255,255,255,.85);
            color: #fff;
            padding: .8rem 1.5rem;
            border-radius: 8px;
            font-weight: 500;
            cursor: pointer;
            transition: all .15s ease;
        }
        .btn-ghost:hover { background: rgba(255,255,255,.12); transform: translateY(-1px); }
        .hero-figure {
            background: rgba(255,255,255,.12);
            border: 1px solid rgba(255,255,255,.25);
            border-radius: 12px;
            display: flex; align-items: center; justify-content: center;
            padding: 1rem;
        }
        .hero-figure img { width: 100%; height: auto; display: block; border-radius: 8px; }

        /* Sections */
        .home-sections { display: grid; gap: 1.5rem; }
        .card-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(240px, 1fr));
            gap: 1rem;
        }
        .card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 12px;
            padding: 1.25rem;
            box-shadow: 0 1px 3px rgba(0,0,0,.06);
        }
        .card h4 { margin: 0 0 .5rem; color: #1e293b; }
        .card p { margin: 0; color: #64748b; font-size: .95rem; }

        @media (max-width: 900px) {
            .hero { grid-template-columns: 1fr; }
            .hero-figure { order: -1; }
        }

        /* New About & Contact sections */
        .section {
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 6px 18px rgba(15, 23, 42, .06);
            padding: 28px;
            margin-top: 2rem;
        }
        .section h2 {
            font-size: 1.8rem;
            font-weight: 700;
            color: #1e293b;
            margin-bottom: .75rem;
            text-align: center;
        }
        .muted { color: #64748b; text-align:center; }

        .contact-grid {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 24px;
            margin-top: 1.5rem;
        }
        .contact-card {
            border: 1px solid #e2e8f0;
            background: #f8fafc;
            border-radius: 12px;
            padding: 20px;
        }
        .contact-row {
            display: grid;
            grid-template-columns: 48px auto;
            gap: 12px;
            align-items: center;
            padding: 10px 0;
            border-bottom: 1px dashed #e2e8f0;
        }
        .contact-row:last-child { border-bottom: 0; }
        .contact-icon {
            width: 48px; height: 48px; border-radius: 12px;
            background: linear-gradient(135deg, #10b981 0%, #059669 100%);
            color: #fff; display: grid; place-items: center; font-size: 20px;
        }
        .contact-form label {
            font-weight: 600; color: #374151; margin-bottom: 6px; display: inline-block;
        }
        .contact-form input, .contact-form textarea {
            width: 100%;
            border: 2px solid #e2e8f0;
            border-radius: 10px;
            padding: 10px 12px;
            font-size: 1rem;
            outline: none;
            transition: border-color .15s, box-shadow .15s;
            background: #fff;
        }
        .contact-form textarea { min-height: 120px; resize: vertical; }
        .contact-form input:focus, .contact-form textarea:focus {
            border-color: #667eea;
            box-shadow: 0 0 0 3px rgba(102,126,234,.12);
        }
    </style>

    <div class="container">
        <!-- HERO -->
        <section class="hero" id="home">
            <div>
                <h1>Hello. Welcome to <span style="font-weight:700;">GadgetHub</span>.</h1>
                <p>Compare quotes, place orders, and manage your catalog — all in one place.</p>
                <div class="hero-cta">
                    <!-- Keep your server-side events -->
                    <asp:Button ID="btnCustomerLogin" runat="server" Text="Login as Customer"
                        CssClass="btn-primary" OnClick="btnCustomerLogin_Click" />
                    <asp:Button ID="btnDistributorLogin" runat="server" Text="Login as Distributor"
                        CssClass="btn-ghost" OnClick="btnDistributorLogin_Click" />
                </div>
            </div>

            <!-- Use your attached image -->
            <div class="hero-figure">
                <img src="<%= ResolveUrl("~/assets/img/home-hero.png") %>" alt="GadgetHub illustration" />
            </div>
        </section>

        <!-- FEATURE CARDS -->
        <section class="home-sections">
            <div class="card-row">
                <div class="card">
                    <h4>Browse Products</h4>
                    <p>See the catalog and submit requests in seconds.</p>
                </div>
                <div class="card">
                    <h4>Get Quotes Fast</h4>
                    <p>Distributors reply with prices — pick the best and order.</p>
                </div>
                <div class="card">
                    <h4>Manage Orders</h4>
                    <p>Track pending, confirmed, and completed orders.</p>
                </div>
                <div class="card">
                    <h4>Add New Products</h4>
                    <p>Distributors can quickly grow the catalog.</p>
                </div>
            </div>
        </section>

        <!-- ABOUT SECTION -->
        <section class="section" id="about">
            <h2>About GadgetHub</h2>
            <p class="muted">We connect customers and distributors with a streamlined request-to-quote flow and real-time order status.</p>
            <p class="muted" style="margin-top:1rem;">
                GadgetHub keeps your product catalog, quote requests, and confirmations organized — 
                with a clean, modern UI your team actually enjoys using.
            </p>
        </section>

        <!-- CONTACT SECTION -->
        <section class="section" id="contact">
            <h2>Contact</h2>
            <p class="muted">Questions, feedback, or partnership ideas? We’d love to hear from you.</p>

            <div class="contact-grid">
                <!-- Info card -->
                <div class="contact-card">
                    <div class="contact-row">
                        <div class="contact-icon">📍</div>
                        <div>
                            <strong>Office</strong><br />
                            <span class="muted">GadgetHub HQ, Anywhere</span>
                        </div>
                    </div>
                    <div class="contact-row">
                        <div class="contact-icon">☎️</div>
                        <div>
                            <strong>Phone</strong><br />
                            <span class="muted">+1 (555) 010-0101</span>
                        </div>
                    </div>
                    <div class="contact-row">
                        <div class="contact-icon">✉️</div>
                        <div>
                            <strong>Email</strong><br />
                            <span class="muted">support@gadgethub.example</span>
                        </div>
                    </div>
                </div>

                <!-- Simple form -->
                <div class="contact-card contact-form">
                    <label for="cname">Name</label>
                    <input id="cname" type="text" placeholder="Your name" />
                    <label for="cemail" style="margin-top:12px;">Email</label>
                    <input id="cemail" type="email" placeholder="you@example.com" />
                    <label for="cmsg" style="margin-top:12px;">Message</label>
                    <textarea id="cmsg" placeholder="How can we help?"></textarea>
                    <button type="button" class="btn-primary" style="margin-top:14px;" onclick="alert('Message sent!')">Send Message</button>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
