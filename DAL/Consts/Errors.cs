namespace DAL.Consts
{
	public static class Errors
	{
		public const string CommonError = "Something bad happened ,Please Try again later.";
		public const string PasswordMinValue = "The {0} must be at least {2} and at max {1} characters long.";
		public const string PasswordMisMatch = "The password and confirmation password do not match.";
		public const string PasswordPattern = "Password Must have minimum 6 characters with At least 1 uppercase and 1 lowercase English letter and 1 digit and 1 special character.";
		public const string Email = "Invalid Email.";
		public const string Phone = "Invalid Egy Number Format.";
		public const string NumbersOnly = "Only Numbers allowed.";
		public const string EnglishLettersOnly = "Only english letters allowed.";
		public const string EnglishLettersAndNumbers = "Only english letters and numbers allowed.";
		public const string EnglishLettersAndNumberswithsymbolExceptions = "Only english letters and numbers and @ or # Allowed.";
		public const string InvalidExtension = "Error: invalid file extension!";
		public const string InvalidFileSize = "Error: invalid file size!";
		public const string InvalidUserId = "Invalid UserId!";
        public const string InvalidNationalNumber = "Invalid egy national ID";
        public const string InvalidPostalNumber = "Incorrect postal number.";


    }
}
