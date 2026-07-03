// ============================================================
// Static demo shell: injects the shared header/nav/footer on
// every page, matching the real application's layout.
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

    document.addEventListener("DOMContentLoaded", function () {
        var headerMount = document.getElementById("site-header-mount");
        var footerMount = document.getElementById("site-footer-mount");

        if (headerMount) headerMount.outerHTML = HEADER_HTML;
        if (footerMount) footerMount.outerHTML = FOOTER_HTML;

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
