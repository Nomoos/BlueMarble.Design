namespace BlueMarble.World.Tests
{
    using BlueMarble.World;
    using Xunit;

    /// <summary>
    /// Tests for InputValidator.
    /// Verifies input validation logic prevents injection attacks and validates data correctly.
    /// </summary>
    public class InputValidatorTests
    {
        #region SQL Injection Tests

        [Fact]
        public void IsSafeSqlInput_SafeString_ReturnsTrue()
        {
            // Arrange
            string safeInput = "HelloWorld123";

            // Act & Assert
            Assert.True(InputValidator.IsSafeSqlInput(safeInput));
        }

        [Fact]
        public void IsSafeSqlInput_NullString_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsSafeSqlInput(null));
        }

        [Fact]
        public void IsSafeSqlInput_EmptyString_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsSafeSqlInput(string.Empty));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionWithOr_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "1' OR '1'='1";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionWithUnion_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; UNION SELECT * FROM users--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionDropTable_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; DROP TABLE users--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionInsert_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; INSERT INTO users VALUES ('hacker', 'password')--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionDelete_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; DELETE FROM users WHERE id=1--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionUpdate_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; UPDATE users SET admin=1--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        [Fact]
        public void IsSafeSqlInput_SqlInjectionExec_ReturnsFalse()
        {
            // Arrange
            string maliciousInput = "'; EXEC sp_executesql--";

            // Act & Assert
            Assert.False(InputValidator.IsSafeSqlInput(maliciousInput));
        }

        #endregion

        #region Length Validation Tests

        [Fact]
        public void IsWithinLengthLimit_WithinLimit_ReturnsTrue()
        {
            // Arrange
            string input = "Hello";

            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthLimit(input, 10));
        }

        [Fact]
        public void IsWithinLengthLimit_ExactlyAtLimit_ReturnsTrue()
        {
            // Arrange
            string input = "Hello";

            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthLimit(input, 5));
        }

        [Fact]
        public void IsWithinLengthLimit_ExceedsLimit_ReturnsFalse()
        {
            // Arrange
            string input = "HelloWorld";

            // Act & Assert
            Assert.False(InputValidator.IsWithinLengthLimit(input, 5));
        }

        [Fact]
        public void IsWithinLengthLimit_NullInput_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthLimit(null, 10));
        }

        [Fact]
        public void IsWithinLengthLimit_NegativeMaxLength_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => InputValidator.IsWithinLengthLimit("test", -1));
        }

        [Fact]
        public void IsWithinLengthRange_WithinRange_ReturnsTrue()
        {
            // Arrange
            string input = "Hello";

            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthRange(input, 3, 10));
        }

        [Fact]
        public void IsWithinLengthRange_AtMinimum_ReturnsTrue()
        {
            // Arrange
            string input = "Hi";

            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthRange(input, 2, 10));
        }

        [Fact]
        public void IsWithinLengthRange_AtMaximum_ReturnsTrue()
        {
            // Arrange
            string input = "HelloWorld";

            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthRange(input, 2, 10));
        }

        [Fact]
        public void IsWithinLengthRange_BelowMinimum_ReturnsFalse()
        {
            // Arrange
            string input = "Hi";

            // Act & Assert
            Assert.False(InputValidator.IsWithinLengthRange(input, 3, 10));
        }

        [Fact]
        public void IsWithinLengthRange_AboveMaximum_ReturnsFalse()
        {
            // Arrange
            string input = "HelloWorldExtra";

            // Act & Assert
            Assert.False(InputValidator.IsWithinLengthRange(input, 3, 10));
        }

        [Fact]
        public void IsWithinLengthRange_NullInputWithZeroMin_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinLengthRange(null, 0, 10));
        }

        [Fact]
        public void IsWithinLengthRange_NullInputWithNonZeroMin_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsWithinLengthRange(null, 1, 10));
        }

        #endregion

        #region Numeric Range Tests

        [Fact]
        public void IsWithinRange_LongWithinRange_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinRange(50L, 0L, 100L));
        }

        [Fact]
        public void IsWithinRange_LongAtMinimum_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinRange(0L, 0L, 100L));
        }

        [Fact]
        public void IsWithinRange_LongAtMaximum_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinRange(100L, 0L, 100L));
        }

        [Fact]
        public void IsWithinRange_LongBelowMinimum_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsWithinRange(-1L, 0L, 100L));
        }

        [Fact]
        public void IsWithinRange_LongAboveMaximum_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsWithinRange(101L, 0L, 100L));
        }

        [Fact]
        public void IsWithinRange_IntWithinRange_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinRange(50, 0, 100));
        }

        [Fact]
        public void IsWithinRange_IntBelowMinimum_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsWithinRange(-1, 0, 100));
        }

        [Fact]
        public void IsWithinRange_DoubleWithinRange_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsWithinRange(50.5, 0.0, 100.0));
        }

        [Fact]
        public void IsWithinRange_DoubleBelowMinimum_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsWithinRange(-0.1, 0.0, 100.0));
        }

        [Fact]
        public void IsWithinRange_InvalidRangeMinGreaterThanMax_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => InputValidator.IsWithinRange(50L, 100L, 0L));
        }

        #endregion

        #region Positive/Non-Negative Tests

        [Fact]
        public void IsPositive_LongPositiveValue_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsPositive(1L));
        }

        [Fact]
        public void IsPositive_LongZero_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsPositive(0L));
        }

        [Fact]
        public void IsPositive_LongNegativeValue_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsPositive(-1L));
        }

        [Fact]
        public void IsPositive_IntPositiveValue_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsPositive(1));
        }

        [Fact]
        public void IsPositive_DoublePositiveValue_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsPositive(0.1));
        }

        [Fact]
        public void IsNonNegative_LongPositiveValue_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsNonNegative(1L));
        }

        [Fact]
        public void IsNonNegative_LongZero_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsNonNegative(0L));
        }

        [Fact]
        public void IsNonNegative_LongNegativeValue_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsNonNegative(-1L));
        }

        [Fact]
        public void IsNonNegative_IntZero_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsNonNegative(0));
        }

        [Fact]
        public void IsNonNegative_DoubleZero_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsNonNegative(0.0));
        }

        #endregion

        #region Character Validation Tests

        [Fact]
        public void IsAlphanumeric_AlphanumericString_ReturnsTrue()
        {
            // Arrange
            string input = "Hello123";

            // Act & Assert
            Assert.True(InputValidator.IsAlphanumeric(input));
        }

        [Fact]
        public void IsAlphanumeric_WithSpecialCharacters_ReturnsFalse()
        {
            // Arrange
            string input = "Hello@World";

            // Act & Assert
            Assert.False(InputValidator.IsAlphanumeric(input));
        }

        [Fact]
        public void IsAlphanumeric_WithSpaces_ReturnsFalse()
        {
            // Arrange
            string input = "Hello World";

            // Act & Assert
            Assert.False(InputValidator.IsAlphanumeric(input));
        }

        [Fact]
        public void IsAlphanumeric_EmptyString_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsAlphanumeric(string.Empty));
        }

        [Fact]
        public void IsAlphanumeric_NullString_ReturnsTrue()
        {
            // Act & Assert
            Assert.True(InputValidator.IsAlphanumeric(null));
        }

        [Fact]
        public void ContainsOnlyAllowedCharacters_AllowedCharacters_ReturnsTrue()
        {
            // Arrange
            string input = "Hello-World_123";

            // Act & Assert
            Assert.True(InputValidator.ContainsOnlyAllowedCharacters(input, "-_"));
        }

        [Fact]
        public void ContainsOnlyAllowedCharacters_DisallowedCharacters_ReturnsFalse()
        {
            // Arrange
            string input = "Hello@World";

            // Act & Assert
            Assert.False(InputValidator.ContainsOnlyAllowedCharacters(input, "-_"));
        }

        #endregion

        #region Pattern Matching Tests

        [Fact]
        public void MatchesPattern_MatchingPattern_ReturnsTrue()
        {
            // Arrange
            string input = "ABC123";
            string pattern = @"^[A-Z]{3}\d{3}$";

            // Act & Assert
            Assert.True(InputValidator.MatchesPattern(input, pattern));
        }

        [Fact]
        public void MatchesPattern_NonMatchingPattern_ReturnsFalse()
        {
            // Arrange
            string input = "abc123";
            string pattern = @"^[A-Z]{3}\d{3}$";

            // Act & Assert
            Assert.False(InputValidator.MatchesPattern(input, pattern));
        }

        [Fact]
        public void MatchesPattern_EmptyInput_ReturnsFalse()
        {
            // Arrange
            string pattern = @"^[A-Z]+$";

            // Act & Assert
            Assert.False(InputValidator.MatchesPattern(string.Empty, pattern));
        }

        #endregion

        #region Email Validation Tests

        [Fact]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "user@example.com";

            // Act & Assert
            Assert.True(InputValidator.IsValidEmail(email));
        }

        [Fact]
        public void IsValidEmail_ValidEmailWithSubdomain_ReturnsTrue()
        {
            // Arrange
            string email = "user@mail.example.com";

            // Act & Assert
            Assert.True(InputValidator.IsValidEmail(email));
        }

        [Fact]
        public void IsValidEmail_MissingAt_ReturnsFalse()
        {
            // Arrange
            string email = "userexample.com";

            // Act & Assert
            Assert.False(InputValidator.IsValidEmail(email));
        }

        [Fact]
        public void IsValidEmail_MissingDomain_ReturnsFalse()
        {
            // Arrange
            string email = "user@";

            // Act & Assert
            Assert.False(InputValidator.IsValidEmail(email));
        }

        [Fact]
        public void IsValidEmail_MissingLocal_ReturnsFalse()
        {
            // Arrange
            string email = "@example.com";

            // Act & Assert
            Assert.False(InputValidator.IsValidEmail(email));
        }

        [Fact]
        public void IsValidEmail_EmptyString_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsValidEmail(string.Empty));
        }

        [Fact]
        public void IsValidEmail_NullString_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsValidEmail(null));
        }

        [Fact]
        public void IsValidEmail_WhitespaceOnly_ReturnsFalse()
        {
            // Act & Assert
            Assert.False(InputValidator.IsValidEmail("   "));
        }

        #endregion
    }
}
