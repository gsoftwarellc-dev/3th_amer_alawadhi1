// ============================================================
// Static demo shell: injects the shared header/nav/footer and
// the "demo mode" notice banner on every page.
// This site has no backend/database - see index.html for why.
// ============================================================

(function () {
    "use strict";

    var HEADER_HTML =
        '<header class="site-header">' +
        '  <div class="header-inner">' +
        '    <a class="brand" href="index.html">' +
        '      <span class="brand-icon" aria-hidden="true">&#128663;</span>' +
        '      AutoCare Service Center' +
        '    </a>' +
        '    <nav class="site-nav" aria-label="Main navigation">' +
        '      <ul>' +
        '        <li><a href="index.html">Home</a></li>' +
        '        <li><a href="services.html">Services</a></li>' +
        '        <li><a href="customers.html">1. Register Customer</a></li>' +
        '        <li><a href="vehicles.html">2. Register Vehicle</a></li>' +
        '        <li><a href="bookings.html">3. Book Service</a></li>' +
        '        <li><a href="service-history.html">4. Service History &amp; Reports</a></li>' +
        '      </ul>' +
        '    </nav>' +
        '  </div>' +
        '</header>';

    var FOOTER_HTML =
        '<footer class="site-footer">' +
        '  <p>&copy; ' + new Date().getFullYear() + ' AutoCare Service Center. All rights reserved.</p>' +
        '  <p class="footer-credit">Made by Amer Alawadhi &mdash; ID: 3087</p>' +
        '</footer>';

    var DEMO_BANNER_HTML =
        '<div class="demo-banner">' +
        '  <strong>Static Demo</strong> &mdash; this preview has no live database. ' +
        '  Forms on this page are for layout preview only and will not save data. ' +
        '  <a href="https://github.com/gsoftwarellc-dev/3th_amer_alawadhi1" target="_blank" rel="noopener">' +
        '    View the full working application (ASP.NET Core + SQL Server) on GitHub &rarr;' +
        '  </a>' +
        '</div>';

    document.addEventListener("DOMContentLoaded", function () {
        var headerMount = document.getElementById("site-header-mount");
        var footerMount = document.getElementById("site-footer-mount");
        var bannerMount = document.getElementById("demo-banner-mount");

        if (headerMount) headerMount.outerHTML = HEADER_HTML;
        if (footerMount) footerMount.outerHTML = FOOTER_HTML;
        if (bannerMount) bannerMount.outerHTML = DEMO_BANNER_HTML;

        // Highlight the current page's nav link
        var currentPage = window.location.pathname.split("/").pop() || "index.html";
        var navLinks = document.querySelectorAll(".site-nav a");
        for (var i = 0; i < navLinks.length; i++) {
            if (navLinks[i].getAttribute("href") === currentPage) {
                navLinks[i].style.textDecoration = "underline";
            }
        }
    });
})();
