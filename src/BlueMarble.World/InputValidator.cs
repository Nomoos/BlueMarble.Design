namespace BlueMarble.World
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides server-side input validation to prevent injection attacks and invalid data.
    /// Implements whitelist approach for allowed values with comprehensive type and range checking.
    /// </summary>
    public static class InputValidator
    {
        // Common dangerous patterns for SQL injection detection
        private static readonly string[] SqlInjectionPatterns = new[]
        {
            @"(\bOR\b|\bAND\b).*=.*",
            @"';.*--",
            @"UNION.*SELECT",
            @"DROP.*TABLE",
            @"INSERT.*INTO",
            @"DELETE.*FROM",
            @"UPDATE.*SET",
            @"EXEC(\s|\()",
            @"EXECUTE(\s|\()",
            @"xp_.*\(",
            @"sp_.*\("
        };

        // Compiled regex patterns for performance
        private static readonly Regex[] CompiledSqlPatterns = Array.ConvertAll(
            SqlInjectionPatterns,
            pattern => new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled)
        );

        /// <summary>
        /// Validates that a string input does not contain SQL injection patterns.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if the input is safe from SQL injection, false otherwise</returns>
        public static bool IsSafeSqlInput(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true; // Empty strings are safe
            }

            foreach (var pattern in CompiledSqlPatterns)
            {
                if (pattern.IsMatch(input))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates that a string does not exceed maximum length.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <param name="maxLength">Maximum allowed length</param>
        /// <returns>True if within length limit, false otherwise</returns>
        public static bool IsWithinLengthLimit(string? input, int maxLength)
        {
            if (maxLength < 0)
            {
                throw new ArgumentException("Maximum length must be non-negative", nameof(maxLength));
            }

            return input == null || input.Length <= maxLength;
        }

        /// <summary>
        /// Validates that a string meets minimum and maximum length requirements.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <param name="minLength">Minimum required length</param>
        /// <param name="maxLength">Maximum allowed length</param>
        /// <returns>True if within length range, false otherwise</returns>
        public static bool IsWithinLengthRange(string? input, int minLength, int maxLength)
        {
            if (minLength < 0)
            {
                throw new ArgumentException("Minimum length must be non-negative", nameof(minLength));
            }
            if (maxLength < minLength)
            {
                throw new ArgumentException("Maximum length must be greater than or equal to minimum length", nameof(maxLength));
            }

            if (input == null)
            {
                return minLength == 0;
            }

            return input.Length >= minLength && input.Length <= maxLength;
        }

        /// <summary>
        /// Validates that a numeric value is within an acceptable range.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed value</param>
        /// <returns>True if within range, false otherwise</returns>
        public static bool IsWithinRange(long value, long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum must be less than or equal to maximum", nameof(min));
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// Validates that a numeric value is within an acceptable range.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed value</param>
        /// <returns>True if within range, false otherwise</returns>
        public static bool IsWithinRange(int value, int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum must be less than or equal to maximum", nameof(min));
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// Validates that a numeric value is within an acceptable range.
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <param name="min">Minimum allowed value</param>
        /// <param name="max">Maximum allowed value</param>
        /// <returns>True if within range, false otherwise</returns>
        public static bool IsWithinRange(double value, double min, double max)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum must be less than or equal to maximum", nameof(min));
            }

            return value >= min && value <= max;
        }

        /// <summary>
        /// Validates that a numeric value is positive (greater than zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if positive, false otherwise</returns>
        public static bool IsPositive(long value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates that a numeric value is positive (greater than zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if positive, false otherwise</returns>
        public static bool IsPositive(int value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates that a numeric value is positive (greater than zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if positive, false otherwise</returns>
        public static bool IsPositive(double value)
        {
            return value > 0;
        }

        /// <summary>
        /// Validates that a numeric value is non-negative (greater than or equal to zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if non-negative, false otherwise</returns>
        public static bool IsNonNegative(long value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Validates that a numeric value is non-negative (greater than or equal to zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if non-negative, false otherwise</returns>
        public static bool IsNonNegative(int value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Validates that a numeric value is non-negative (greater than or equal to zero).
        /// </summary>
        /// <param name="value">The value to validate</param>
        /// <returns>True if non-negative, false otherwise</returns>
        public static bool IsNonNegative(double value)
        {
            return value >= 0;
        }

        /// <summary>
        /// Validates that a string contains only alphanumeric characters.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if alphanumeric only, false otherwise</returns>
        public static bool IsAlphanumeric(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates that a string contains only alphanumeric characters and specified allowed characters.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <param name="allowedChars">Additional characters to allow (e.g., "-_")</param>
        /// <returns>True if contains only allowed characters, false otherwise</returns>
        public static bool ContainsOnlyAllowedCharacters(string? input, string allowedChars)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c) && !allowedChars.Contains(c))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates that a string matches a specific pattern.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <param name="pattern">Regular expression pattern to match</param>
        /// <returns>True if matches pattern, false otherwise</returns>
        public static bool MatchesPattern(string? input, string pattern)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            return Regex.IsMatch(input, pattern, RegexOptions.Compiled);
        }

        /// <summary>
        /// Validates that an email address has a valid format.
        /// </summary>
        /// <param name="email">The email address to validate</param>
        /// <returns>True if valid email format, false otherwise</returns>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // Basic email validation pattern
            const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }
    }
}
