namespace BSE.Identity.Blazor.Client
{
    public static class StringExtensions
    {
        public static string ToUpperFirstChar(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        }
    }
}
