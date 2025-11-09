namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using Xunit;

    /// <summary>
    /// Tests for XssProtection.
    /// Verifies XSS protection mechanisms work correctly.
    /// </summary>
    public class XssProtectionTests
    {
        #region HTML Encoding Tests

        [Fact]
        public void EncodeForHtml_SafeString_ReturnsUnchanged()
        {
            // Arrange
            string input = "Hello World";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void EncodeForHtml_LessThan_EncodesCorrectly()
        {
            // Arrange
            string input = "<div>";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("&lt;div&gt;", result);
        }

        [Fact]
        public void EncodeForHtml_GreaterThan_EncodesCorrectly()
        {
            // Arrange
            string input = "5 > 3";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("5 &gt; 3", result);
        }

        [Fact]
        public void EncodeForHtml_Ampersand_EncodesCorrectly()
        {
            // Arrange
            string input = "Tom & Jerry";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("Tom &amp; Jerry", result);
        }

        [Fact]
        public void EncodeForHtml_DoubleQuote_EncodesCorrectly()
        {
            // Arrange
            string input = "Say \"Hello\"";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("Say &quot;Hello&quot;", result);
        }

        [Fact]
        public void EncodeForHtml_SingleQuote_EncodesCorrectly()
        {
            // Arrange
            string input = "It's working";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("It&#x27;s working", result);
        }

        [Fact]
        public void EncodeForHtml_Slash_EncodesCorrectly()
        {
            // Arrange
            string input = "path/to/file";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("path&#x2F;to&#x2F;file", result);
        }

        [Fact]
        public void EncodeForHtml_ScriptTag_EncodesCorrectly()
        {
            // Arrange
            string input = "<script>alert('XSS')</script>";

            // Act
            string result = XssProtection.EncodeForHtml(input);

            // Assert
            Assert.Equal("&lt;script&gt;alert(&#x27;XSS&#x27;)&lt;&#x2F;script&gt;", result);
        }

        [Fact]
        public void EncodeForHtml_NullInput_ReturnsEmptyString()
        {
            // Act
            string result = XssProtection.EncodeForHtml(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EncodeForHtml_EmptyInput_ReturnsEmptyString()
        {
            // Act
            string result = XssProtection.EncodeForHtml(string.Empty);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion

        #region HTML Attribute Encoding Tests

        [Fact]
        public void EncodeForHtmlAttribute_SafeString_ReturnsUnchanged()
        {
            // Arrange
            string input = "HelloWorld";

            // Act
            string result = XssProtection.EncodeForHtmlAttribute(input);

            // Assert
            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void EncodeForHtmlAttribute_SpecialCharacters_EncodesCorrectly()
        {
            // Arrange
            string input = "<div class=\"test\">";

            // Act
            string result = XssProtection.EncodeForHtmlAttribute(input);

            // Assert
            Assert.Contains("&lt;", result);
            Assert.Contains("&gt;", result);
            Assert.Contains("&quot;", result);
        }

        [Fact]
        public void EncodeForHtmlAttribute_ControlCharacters_EncodesCorrectly()
        {
            // Arrange
            string input = "test\u0001value";

            // Act
            string result = XssProtection.EncodeForHtmlAttribute(input);

            // Assert
            Assert.Contains("&#x01;", result);
        }

        #endregion

        #region JavaScript Encoding Tests

        [Fact]
        public void EncodeForJavaScript_SafeString_ReturnsUnchanged()
        {
            // Arrange
            string input = "HelloWorld";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void EncodeForJavaScript_SingleQuote_EncodesCorrectly()
        {
            // Arrange
            string input = "It's working";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("It\\'s working", result);
        }

        [Fact]
        public void EncodeForJavaScript_DoubleQuote_EncodesCorrectly()
        {
            // Arrange
            string input = "Say \"Hello\"";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("Say \\\"Hello\\\"", result);
        }

        [Fact]
        public void EncodeForJavaScript_Backslash_EncodesCorrectly()
        {
            // Arrange
            string input = "C:\\path\\to\\file";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("C:\\\\path\\\\to\\\\file", result);
        }

        [Fact]
        public void EncodeForJavaScript_Newline_EncodesCorrectly()
        {
            // Arrange
            string input = "Line1\nLine2";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("Line1\\nLine2", result);
        }

        [Fact]
        public void EncodeForJavaScript_CarriageReturn_EncodesCorrectly()
        {
            // Arrange
            string input = "Line1\rLine2";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("Line1\\rLine2", result);
        }

        [Fact]
        public void EncodeForJavaScript_Tab_EncodesCorrectly()
        {
            // Arrange
            string input = "Column1\tColumn2";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("Column1\\tColumn2", result);
        }

        [Fact]
        public void EncodeForJavaScript_LessThan_EncodesCorrectly()
        {
            // Arrange
            string input = "a < b";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.Equal("a \\x3C b", result);
        }

        [Fact]
        public void EncodeForJavaScript_ScriptBreakout_EncodesCorrectly()
        {
            // Arrange
            string input = "</script><script>alert('XSS')</script>";

            // Act
            string result = XssProtection.EncodeForJavaScript(input);

            // Assert
            Assert.DoesNotContain("</script>", result);
            Assert.Contains("\\x3C", result);
            Assert.Contains("\\x3E", result);
        }

        #endregion

        #region URL Encoding Tests

        [Fact]
        public void EncodeForUrl_SafeString_ReturnsUnchanged()
        {
            // Arrange
            string input = "HelloWorld";

            // Act
            string result = XssProtection.EncodeForUrl(input);

            // Assert
            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void EncodeForUrl_SpaceCharacter_EncodesCorrectly()
        {
            // Arrange
            string input = "Hello World";

            // Act
            string result = XssProtection.EncodeForUrl(input);

            // Assert
            Assert.Equal("Hello%20World", result);
        }

        [Fact]
        public void EncodeForUrl_SpecialCharacters_EncodesCorrectly()
        {
            // Arrange
            string input = "a=b&c=d";

            // Act
            string result = XssProtection.EncodeForUrl(input);

            // Assert
            Assert.Contains("%3D", result); // =
            Assert.Contains("%26", result); // &
        }

        [Fact]
        public void EncodeForUrl_NullInput_ReturnsEmptyString()
        {
            // Act
            string result = XssProtection.EncodeForUrl(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion

        #region HTML Tag Stripping Tests

        [Fact]
        public void StripHtmlTags_PlainText_ReturnsUnchanged()
        {
            // Arrange
            string input = "Hello World";

            // Act
            string result = XssProtection.StripHtmlTags(input);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void StripHtmlTags_SingleTag_RemovesTag()
        {
            // Arrange
            string input = "<div>Hello</div>";

            // Act
            string result = XssProtection.StripHtmlTags(input);

            // Assert
            Assert.Equal("Hello", result);
        }

        [Fact]
        public void StripHtmlTags_MultipleTags_RemovesAllTags()
        {
            // Arrange
            string input = "<p>Hello <b>World</b></p>";

            // Act
            string result = XssProtection.StripHtmlTags(input);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void StripHtmlTags_ScriptTag_RemovesTag()
        {
            // Arrange
            string input = "<script>alert('XSS')</script>Safe Text";

            // Act
            string result = XssProtection.StripHtmlTags(input);

            // Assert
            Assert.Equal("alert('XSS')Safe Text", result);
            Assert.DoesNotContain("<script>", result);
        }

        [Fact]
        public void StripHtmlTags_SelfClosingTag_RemovesTag()
        {
            // Arrange
            string input = "Text<br/>More Text";

            // Act
            string result = XssProtection.StripHtmlTags(input);

            // Assert
            Assert.Equal("TextMore Text", result);
        }

        [Fact]
        public void StripHtmlTags_NullInput_ReturnsEmptyString()
        {
            // Act
            string result = XssProtection.StripHtmlTags(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion

        #region Sanitize Tests

        [Fact]
        public void Sanitize_PlainText_ReturnsUnchanged()
        {
            // Arrange
            string input = "Hello World";

            // Act
            string result = XssProtection.Sanitize(input);

            // Assert
            Assert.Equal("Hello World", result);
        }

        [Fact]
        public void Sanitize_ScriptTag_RemovesAndEncodes()
        {
            // Arrange
            string input = "<script>alert('XSS')</script>";

            // Act
            string result = XssProtection.Sanitize(input);

            // Assert
            Assert.DoesNotContain("<script>", result);
            Assert.DoesNotContain("</script>", result);
        }

        [Fact]
        public void Sanitize_MixedContent_SanitizesCorrectly()
        {
            // Arrange
            string input = "<b>Bold</b> & <i>Italic</i>";

            // Act
            string result = XssProtection.Sanitize(input);

            // Assert
            Assert.DoesNotContain("<b>", result);
            Assert.DoesNotContain("<i>", result);
            Assert.Contains("&amp;", result);
        }

        [Fact]
        public void Sanitize_NullInput_ReturnsEmptyString()
        {
            // Act
            string result = XssProtection.Sanitize(null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        #endregion

        #region Dangerous Content Detection Tests

        [Fact]
        public void ContainsDangerousContent_SafeString_ReturnsFalse()
        {
            // Arrange
            string input = "Hello World";

            // Act & Assert
            Assert.False(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_ScriptTag_ReturnsTrue()
        {
            // Arrange
            string input = "<script>alert('XSS')</script>";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_JavaScriptProtocol_ReturnsTrue()
        {
            // Arrange
            string input = "javascript:alert('XSS')";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_OnErrorHandler_ReturnsTrue()
        {
            // Arrange
            string input = "<img src=x onerror=alert('XSS')>";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_OnLoadHandler_ReturnsTrue()
        {
            // Arrange
            string input = "<body onload=alert('XSS')>";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_OnClickHandler_ReturnsTrue()
        {
            // Arrange
            string input = "<button onclick=alert('XSS')>Click</button>";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_IframeTag_ReturnsTrue()
        {
            // Arrange
            string input = "<iframe src='evil.com'></iframe>";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_EvalFunction_ReturnsTrue()
        {
            // Arrange
            string input = "eval(maliciousCode)";

            // Act & Assert
            Assert.True(XssProtection.ContainsDangerousContent(input));
        }

        [Fact]
        public void ContainsDangerousContent_NullInput_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(XssProtection.ContainsDangerousContent(null));
        }

        [Fact]
        public void ContainsDangerousContent_EmptyInput_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(XssProtection.ContainsDangerousContent(string.Empty));
        }

        #endregion

        #region Safe for Display Tests

        [Fact]
        public void IsSafeForDisplay_AlphanumericString_ReturnsTrue()
        {
            // Arrange
            string input = "HelloWorld123";

            // Act & Assert
            Assert.True(XssProtection.IsSafeForDisplay(input));
        }

        [Fact]
        public void IsSafeForDisplay_SafePunctuation_ReturnsTrue()
        {
            // Arrange
            string input = "Hello, World! How are you?";

            // Act & Assert
            Assert.True(XssProtection.IsSafeForDisplay(input));
        }

        [Fact]
        public void IsSafeForDisplay_ScriptTag_ReturnsFalse()
        {
            // Arrange
            string input = "<script>alert('XSS')</script>";

            // Act & Assert
            Assert.False(XssProtection.IsSafeForDisplay(input));
        }

        [Fact]
        public void IsSafeForDisplay_SpecialCharacters_ReturnsFalse()
        {
            // Arrange
            string input = "Hello@World#Test";

            // Act & Assert
            Assert.False(XssProtection.IsSafeForDisplay(input));
        }

        [Fact]
        public void IsSafeForDisplay_NullInput_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(XssProtection.IsSafeForDisplay(null));
        }

        [Fact]
        public void IsSafeForDisplay_EmptyInput_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(XssProtection.IsSafeForDisplay(string.Empty));
        }

        #endregion
    }
}
