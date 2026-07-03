using System;
using System.Text.RegularExpressions;

namespace VehicleServiceBooking.App_Code
{
    /// <summary>
    /// Server-side validation rules, mirrored on the client in JS/validation.js.
    /// Server-side checks are the authoritative ones (per project requirements).
    /// </summary>
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex =
            new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", RegexOptions.Compiled);

        // Accepts UAE-style plates (e.g. "A 12345", "DXB-12345") or general
        // alphanumeric plate formats: 2-4 letters/digits, optional separator, 1-6 digits.
        private static readonly Regex PlateRegex =
            new Regex(@"^[A-Za-z]{1,3}[\s-]?\d{1,6}$", RegexOptions.Compiled);

        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());
        }

        public static bool IsValidPlateNumber(string plate)
        {
            return !string.IsNullOrWhiteSpace(plate) && PlateRegex.IsMatch(plate.Trim());
        }

        public static bool IsValidYear(int year)
        {
            int currentYear = DateTime.Now.Year;
            return year >= 1980 && year <= currentYear + 1;
        }

        public static bool IsValidPhone(string phone)
        {
            return !string.IsNullOrWhiteSpace(phone) && Regex.IsMatch(phone.Trim(), @"^[0-9+\s-]{7,20}$");
        }
    }
}
