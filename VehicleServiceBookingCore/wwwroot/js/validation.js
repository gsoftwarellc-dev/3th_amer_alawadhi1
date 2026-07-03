// ============================================================
// Vehicle Maintenance Service Booking System
// External JavaScript - client-side validation, DOM manipulation,
// event handling, and interactivity.
// ============================================================

(function () {
    "use strict";

    // Regular expressions (also mirrored server-side in the C# controllers/models)
    var EMAIL_REGEX = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    var PLATE_REGEX = /^[A-Za-z]{1,3}[\s-]?\d{1,6}$/;
    var CURRENT_YEAR = new Date().getFullYear();
    var MIN_YEAR = 1980;

    // Service details shown when a service type is selected (Booking page).
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
        wireVehicleYearValidation();
        wirePlateNumberValidation();
        wireConfirmButtons();
        wireEmailValidation();
        wireRequiredNameValidation();
    });

    // ------------------------------------------------------------
    // Show service details based on selected service type
    // (DOM manipulation via innerHTML + event handling)
    // ------------------------------------------------------------
    function wireServiceTypeDetails() {
        var serviceSelect = document.getElementById("serviceTypeSelect");
        var detailsBox = document.getElementById("serviceDetailsBox");

        if (!serviceSelect || !detailsBox) {
            return;
        }

        serviceSelect.addEventListener("change", function () {
            var selected = serviceSelect.value;
            var detailText = "";

            // Selection statement + lookup loop over known service types.
            for (var key in SERVICE_DETAILS) {
                if (Object.prototype.hasOwnProperty.call(SERVICE_DETAILS, key)) {
                    if (key === selected) {
                        detailText = SERVICE_DETAILS[key];
                        break;
                    }
                }
            }

            if (detailText !== "") {
                detailsBox.innerHTML = "<strong>" + selected + ":</strong> " + detailText;
                detailsBox.style.display = "block";
            } else {
                detailsBox.innerHTML = "";
                detailsBox.style.display = "none";
            }
        });
    }

    // ------------------------------------------------------------
    // Validate vehicle year using a regular expression + range check
    // ------------------------------------------------------------
    function wireVehicleYearValidation() {
        var yearInput = document.getElementById("Year");
        var yearError = document.getElementById("yearError");

        if (!yearInput) {
            return;
        }

        yearInput.addEventListener("input", function () {
            validateYearField(yearInput, yearError);
        });
        yearInput.addEventListener("blur", function () {
            validateYearField(yearInput, yearError);
        });
    }

    function validateYearField(yearInput, yearError) {
        var yearRegex = /^\d{4}$/;
        var value = yearInput.value.trim();
        var isFormatValid = yearRegex.test(value);
        var yearNumber = parseInt(value, 10);
        var isInRange = isFormatValid && yearNumber >= MIN_YEAR && yearNumber <= (CURRENT_YEAR + 1);

        if (value === "") {
            setFieldMessage(yearError, "");
            return false;
        }

        if (!isFormatValid || !isInRange) {
            setFieldMessage(yearError, "Enter a valid 4-digit year between " + MIN_YEAR + " and " + (CURRENT_YEAR + 1) + ".");
            return false;
        }

        setFieldMessage(yearError, "");
        return true;
    }

    // ------------------------------------------------------------
    // Validate plate number using a regular expression
    // ------------------------------------------------------------
    function wirePlateNumberValidation() {
        var plateInput = document.getElementById("PlateNumber");
        var plateError = document.getElementById("plateError");

        if (!plateInput) {
            return;
        }

        plateInput.addEventListener("input", function () {
            validatePlateField(plateInput, plateError);
        });
        plateInput.addEventListener("blur", function () {
            validatePlateField(plateInput, plateError);
        });
    }

    function validatePlateField(plateInput, plateError) {
        var value = plateInput.value.trim();

        if (value === "") {
            setFieldMessage(plateError, "");
            return false;
        }

        if (!PLATE_REGEX.test(value)) {
            setFieldMessage(plateError, "Enter a valid plate number, e.g. A12345 or DXB 12345.");
            return false;
        }

        setFieldMessage(plateError, "");
        return true;
    }

    // ------------------------------------------------------------
    // Validate email format using a regular expression
    // ------------------------------------------------------------
    function wireEmailValidation() {
        var emailInputs = document.querySelectorAll("input[type='email']");

        // Loop statement over a NodeList
        for (var i = 0; i < emailInputs.length; i++) {
            (function (input) {
                var errorSpan = document.getElementById("emailError");
                input.addEventListener("blur", function () {
                    if (input.value.trim() !== "" && !EMAIL_REGEX.test(input.value.trim())) {
                        if (errorSpan && input.id === "Email") {
                            setFieldMessage(errorSpan, "Enter a valid email address.");
                        }
                        console.log("Invalid email entered: " + input.value);
                    } else if (errorSpan && input.id === "Email") {
                        setFieldMessage(errorSpan, "");
                    }
                });
            })(emailInputs[i]);
        }
    }

    // ------------------------------------------------------------
    // Required customer name check
    // ------------------------------------------------------------
    function wireRequiredNameValidation() {
        var nameInput = document.getElementById("FullName");
        var nameError = document.getElementById("fullNameError");

        if (!nameInput) {
            return;
        }

        nameInput.addEventListener("blur", function () {
            if (nameInput.value.trim() === "") {
                setFieldMessage(nameError, "Customer name is required.");
            } else {
                setFieldMessage(nameError, "");
            }
        });
    }

    // ------------------------------------------------------------
    // Confirm booking cancellation / delete actions
    // ------------------------------------------------------------
    function wireConfirmButtons() {
        var confirmButtons = document.querySelectorAll(".js-confirm-delete, .js-cancel-booking");

        for (var i = 0; i < confirmButtons.length; i++) {
            confirmButtons[i].addEventListener("click", function (event) {
                var message = this.getAttribute("data-confirm-message") || "Are you sure?";
                var confirmed = confirm(message);
                if (!confirmed) {
                    event.preventDefault();
                }
            });
        }
    }

    // ------------------------------------------------------------
    // Helper: update a status/error message element via DOM manipulation
    // ------------------------------------------------------------
    function setFieldMessage(element, message) {
        if (element) {
            element.textContent = message;
        }
    }

})();
