namespace DAL.Consts
{
    public static class Errors
    {
        public const string PasswordMinValue = "The {0} must be at least {2} and at max {1} characters long.";
        public const string PasswordMisMatch = "The password and confirmation password do not match.";
        public const string PasswordPattern = "Password Must have minimum 6 characters with At least 1 uppercase and 1 lowercase English letter and 1 digit and 1 special character";
        public const string Email = "Invalid Email";
        public const string Phone = "Incorrect Egy Number Format";
        public const string NumbersOnly = "Numbers Only allowed";
        public const string EnglishLettersOnly= "Only english letters allowed";

    }
}
