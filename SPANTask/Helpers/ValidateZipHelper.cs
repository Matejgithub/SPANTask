using System.Text.RegularExpressions;

namespace SPANTask.Helpers
{
    public class ValidateZipHelper
    {
        public bool Validate(string zip)
        {
            if(!Regex.IsMatch(zip, @"^[1-5]\d{4}$"))
            {
                return false;
            }

            return true;
        }
    }
}
