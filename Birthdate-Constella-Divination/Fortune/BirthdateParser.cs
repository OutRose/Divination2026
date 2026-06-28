using System;

namespace BirthdateConstellaDivination.Fortune
{
    public enum BirthdateError
    {
        Empty,
        NotEightDigits,
        NotNumeric,
        InvalidDate,
    }

    public static class BirthdateParser
    {
        public static bool TryParse(string text, out DateTime date, out BirthdateError? error)
        {
            date = default;

            if (string.IsNullOrEmpty(text))
            {
                error = BirthdateError.Empty;
                return false;
            }

            if (text.Length != 8)
            {
                error = BirthdateError.NotEightDigits;
                return false;
            }

            foreach (char c in text)
            {
                if (c < '0' || c > '9')
                {
                    error = BirthdateError.NotNumeric;
                    return false;
                }
            }

            int n = int.Parse(text);
            int year = n / 10000;
            int month = (n / 100) % 100;
            int day = n % 100;

            try
            {
                date = new DateTime(year, month, day);
                error = null;
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                error = BirthdateError.InvalidDate;
                return false;
            }
        }

        public static string DescribeError(BirthdateError error) => error switch
        {
            BirthdateError.Empty          => "誕生日を入力してください。",
            BirthdateError.NotEightDigits => "誕生日は8桁の数字で入力してください (例: 20020523)。",
            BirthdateError.NotNumeric     => "誕生日は数字のみで入力してください。",
            BirthdateError.InvalidDate    => "誕生日として無効な日付です。",
            _ => throw new ArgumentOutOfRangeException(nameof(error)),
        };
    }
}
