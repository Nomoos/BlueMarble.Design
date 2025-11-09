namespace BlueMarble.World
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Provides XSS (Cross-Site Scripting) protection through context-aware output encoding.
    /// Implements defense-in-depth security for preventing XSS attacks.
    /// </summary>
    public static class XssProtection
    {
        // HTML entities that need to be escaped
        private static readonly Dictionary<char, string> HtmlEntities = new Dictionary<char, string>
        {
            { '<', "&lt;" },
            { '>', "&gt;" },
            { '&', "&amp;" },
            { '"', "&quot;" },
            { '\'', "&#x27;" },
            { '/', "&#x2F;" }
        };

        // JavaScript special characters that need to be escaped
        private static readonly Dictionary<char, string> JavaScriptEntities = new Dictionary<char, string>
        {
            { '\\', "\\\\" },
            { '\'', "\\'" },
            { '"', "\\\"" },
            { '\n', "\\n" },
            { '\r', "\\r" },
            { '\t', "\\t" },
            { '<', "\\x3C" },
            { '>', "\\x3E" },
            { '&', "\\x26" }
        };

        /// <summary>
        /// Encodes a string for safe use in HTML content.
        /// Escapes characters that could be used in XSS attacks.
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>HTML-encoded string safe for display in HTML content</returns>
        public static string EncodeForHtml(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(input.Length + 20); // Pre-allocate with some extra space

            foreach (char c in input)
            {
                if (HtmlEntities.TryGetValue(c, out string? entity))
                {
                    sb.Append(entity);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string for safe use in HTML attribute values.
        /// More aggressive than HTML content encoding.
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>Encoded string safe for use in HTML attributes</returns>
        public static string EncodeForHtmlAttribute(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(input.Length + 20);

            foreach (char c in input)
            {
                if (HtmlEntities.TryGetValue(c, out string? entity))
                {
                    sb.Append(entity);
                }
                else if (c < 32 || c > 126) // Control characters and non-ASCII
                {
                    sb.Append($"&#x{(int)c:X2};");
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string for safe use in JavaScript string contexts.
        /// Prevents breaking out of JavaScript strings to execute malicious code.
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>JavaScript-encoded string</returns>
        public static string EncodeForJavaScript(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(input.Length + 20);

            foreach (char c in input)
            {
                if (JavaScriptEntities.TryGetValue(c, out string? entity))
                {
                    sb.Append(entity);
                }
                else if (c < 32 || c > 126) // Control characters and non-ASCII
                {
                    sb.Append($"\\u{(int)c:X4}");
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Encodes a string for safe use in URLs.
        /// Uses percent-encoding for special characters.
        /// </summary>
        /// <param name="input">The input string to encode</param>
        /// <returns>URL-encoded string</returns>
        public static string EncodeForUrl(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            return Uri.EscapeDataString(input);
        }

        /// <summary>
        /// Removes all HTML tags from a string, leaving only text content.
        /// Useful for stripping potential XSS vectors from user input.
        /// </summary>
        /// <param name="input">The input string potentially containing HTML</param>
        /// <returns>String with all HTML tags removed</returns>
        public static string StripHtmlTags(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // Simple but effective HTML tag removal
            // Note: For production use, consider a more robust HTML sanitizer library
            var sb = new StringBuilder(input.Length);
            bool insideTag = false;

            foreach (char c in input)
            {
                if (c == '<')
                {
                    insideTag = true;
                }
                else if (c == '>')
                {
                    insideTag = false;
                }
                else if (!insideTag)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Sanitizes a string by removing dangerous patterns and encoding for HTML.
        /// Combines multiple protection mechanisms for comprehensive XSS prevention.
        /// </summary>
        /// <param name="input">The input string to sanitize</param>
        /// <returns>Sanitized string safe for display</returns>
        public static string Sanitize(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // First strip any HTML tags
            var stripped = StripHtmlTags(input);

            // Then encode for safe HTML display
            return EncodeForHtml(stripped);
        }

        /// <summary>
        /// Checks if a string contains potentially dangerous content.
        /// Detects common XSS attack patterns.
        /// </summary>
        /// <param name="input">The input string to check</param>
        /// <returns>True if dangerous content is detected, false otherwise</returns>
        public static bool ContainsDangerousContent(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            var lowerInput = input.ToLowerInvariant();

            // Check for common XSS patterns
            string[] dangerousPatterns = new[]
            {
                "<script",
                "javascript:",
                "onerror=",
                "onload=",
                "onclick=",
                "onmouseover=",
                "<iframe",
                "<embed",
                "<object",
                "eval(",
                "expression("
            };

            foreach (var pattern in dangerousPatterns)
            {
                if (lowerInput.Contains(pattern))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Validates that a string is safe for use without encoding.
        /// Only allows alphanumeric characters and common safe punctuation.
        /// </summary>
        /// <param name="input">The input string to validate</param>
        /// <returns>True if the string is safe, false if it needs encoding</returns>
        public static bool IsSafeForDisplay(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return true;
            }

            // Allow alphanumeric, spaces, and common safe punctuation
            const string safeChars = " .,!?;:()-_";

            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c) && !safeChars.Contains(c))
                {
                    return false;
                }
            }

            return !ContainsDangerousContent(input);
        }
    }
}
