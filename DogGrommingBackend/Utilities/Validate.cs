using System.Text.RegularExpressions;

namespace DogGrommingBackend.Utilities
{
    public static class Validate
    {
        //basice validatin
        public static bool ValidateFullName(string fullName)
        {
            return fullName.Length <= 20;
        }

        public static bool ValidateUserName(string userName)
        {
            return userName.Length <= 50;
        }
        
        public static bool ValidatePassword(string password)
        {          
            return  password.Length >= 8 && Regex.IsMatch(password, "\\d");
        }
    }
}
