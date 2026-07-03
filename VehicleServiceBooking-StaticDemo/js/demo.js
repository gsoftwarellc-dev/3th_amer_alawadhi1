// ============================================================
// Static demo interactivity: service-type detail box preview,
// and demo-mode form submit interception.
// ============================================================

(function () {
    "use strict";

    var SERVICE_DETAILS = {
        "Oil Change": "Includes oil and filter replacement. Estimated duration: 30 minutes. Starting from AED 120.",
        "Tire Rotation": "Rotates and balances all four tires, checks tire pressure. Estimated duration: 30 minutes. Starting from AED 80.",
        "Brake Service": "Inspects and replaces brake pads/rotors as needed, checks brake fluid. Estimated duration: 1-2 hours. Starting from AED 250.",
        "Battery Replacement": "Tests and replaces the battery, checks the charging system. Estimated duration: 20 minutes. Starting from AED 300.",
        "Full Inspection": "Comprehensive multi-point inspection of engine, brakes, suspension, and electrical systems. Estimated duration: 1 hour. Starting from AED 150.",
        "Air Conditioning Service": "Checks AC performance, recharges refrigerant, cleans filter. Estimated duration: 45 minutes. Starting from AED 180."
    };

    document.addEventListener("DOMContentLoaded", function () {
        wireServiceTypeDetails();
        wireDemoForms();
    });

    function wireServiceTypeDetails() {
        var serviceSelect = document.getElementById("serviceTypeSelect");
        var detailsBox = document.getElementById("serviceDetailsBox");
        if (!serviceSelect || !detailsBox) return;

        serviceSelect.addEventListener("change", function () {
            var selected = serviceSelect.value;
            var detailText = SERVICE_DETAILS[selected];
            if (detailText) {
                detailsBox.innerHTML = "<strong>" + selected + ":</strong> " + detailText;
                detailsBox.style.display = "block";
            } else {
                detailsBox.innerHTML = "";
                detailsBox.style.display = "none";
            }
        });
    }

    function wireDemoForms() {
        var forms = document.querySelectorAll("form.js-demo-form");
        for (var i = 0; i < forms.length; i++) {
            forms[i].addEventListener("submit", function (event) {
                event.preventDefault();
                alert("This is a static demo - no database is connected here.\n\nThe full working version (with real Create/Read/Update/Delete against SQL Server) is available in the GitHub repository linked in the banner above.");
            });
        }

        var deleteButtons = document.querySelectorAll(".js-demo-delete");
        for (var j = 0; j < deleteButtons.length; j++) {
            deleteButtons[j].addEventListener("click", function (event) {
                event.preventDefault();
                alert("This is a static demo - delete actions are disabled here.");
            });
        }
    }
})();
