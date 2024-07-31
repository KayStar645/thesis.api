using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Core.Application.Common.Validators
{
    public static class ValidatorCustom
    {
        public static bool IsAtLeastNYearsOld(DateTime? pDateOfBirth, int pYear)
        {
            DateTime currentDate = DateTime.Now;
            DateTime minimumBirthDate = currentDate.AddYears(-pYear);
            return pDateOfBirth <= minimumBirthDate;
        }

        public static bool BeValidEmail(string pEmail)
        {
            if (string.IsNullOrEmpty(pEmail))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$";
            return Regex.IsMatch(pEmail, pattern);
        }

        public static bool IsEqualOrAfterDay(DateTime? pTime, DateTime? pDay)
        {
            return pTime >= pDay;
        }

        public static bool IsAfterDay(DateTime? pTime, DateTime? pDay)
        {
            return pTime > pDay;
        }

        public static bool IsValidFile(string pFilePath)
        {
            if (string.IsNullOrEmpty(pFilePath))
                return false;

            string pattern = @"^[^.]+\.[a-zA-Z]+$";

            return Regex.IsMatch(pFilePath, pattern) && pFilePath.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }

        public static bool BeValidImage(string pImagePath)
        {
            if (string.IsNullOrEmpty(pImagePath))
                return false;

            string[] allowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".svg" };
            string extension = Path.GetExtension(pImagePath);
            return allowedImageExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase) ||
                pImagePath.Contains("firebasestorage.googleapis.com") && pImagePath.Contains("?alt=media&token=");
        }

        public static bool IsValidGender(string pGender)
        {
            string[] genders = ValidatorCustom.GetGender();

            return genders.Contains(pGender);
        }

        public static string[] GetGender()
        {
            return new string[]
            {
                "Nam",
                "Nữ",
                "Khác",
            };
        }
    }
}
