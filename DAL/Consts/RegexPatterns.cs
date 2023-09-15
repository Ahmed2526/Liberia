namespace DAL.Consts
{
    public static class RegexPatterns
    {
        public const string PasswordPattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$";

        public const string EmailPattern = "^\\S+@\\S+\\.\\S+$";

        public const string EgyPhonePattern = "^01[0125][0-9]{8}$";

        public const string NumericsonlyPattern = "^\\d+$";

        public const string EnglishLettersOnlyPattern ="^[a-zA-Z]+$";
        public const string EnglishLettersOnlyWithSpacePattern = "^[a-zA-Z ]+$";

        public const string EnglishLettersandDotPattern = "^[a-zA-Z. ]+$";

        public const string EnglishLettersAndNumbersOnlyPattern = "^[a-zA-Z0-9]+$";

        //Need Improvement To Allow only one symbol
        public const string EnglishLettersAndNumbersAndSpecificSymbolsPattern = "^[a-zA-Z0-9@#]+$";
    }
}
