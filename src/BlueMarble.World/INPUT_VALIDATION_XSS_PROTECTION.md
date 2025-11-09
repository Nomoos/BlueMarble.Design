# Input Validation and XSS Protection

## Overview

This document describes the input validation and XSS protection utilities implemented in the BlueMarble.World namespace. These utilities provide comprehensive security measures to prevent injection attacks and cross-site scripting (XSS) vulnerabilities.

## InputValidator

The `InputValidator` class provides server-side input validation to prevent injection attacks and ensure data integrity.

### Features

- **SQL Injection Prevention**: Detects common SQL injection patterns
- **Length Validation**: Enforces minimum and maximum string lengths
- **Range Validation**: Validates numeric values are within acceptable ranges
- **Type Validation**: Ensures values are positive or non-negative
- **Character Validation**: Validates alphanumeric and custom character sets
- **Pattern Matching**: Validates input against regular expressions
- **Email Validation**: Validates email address format

### Usage Examples

#### SQL Injection Detection

```csharp
using BlueMarble.World;

// Safe input
string safeInput = "username123";
bool isSafe = InputValidator.IsSafeSqlInput(safeInput); // true

// Malicious input
string maliciousInput = "'; DROP TABLE users--";
bool isValid = InputValidator.IsSafeSqlInput(maliciousInput); // false
```

#### Length Validation

```csharp
// Check maximum length
string username = "player123";
bool isValid = InputValidator.IsWithinLengthLimit(username, 50); // true

// Check length range
string password = "securePassword123!";
bool isValid = InputValidator.IsWithinLengthRange(password, 8, 64); // true
```

#### Range Validation

```csharp
// Validate numeric ranges
long playerId = 12345;
bool isValid = InputValidator.IsWithinRange(playerId, 1L, 1000000L); // true

int level = 42;
bool isValid = InputValidator.IsWithinRange(level, 1, 100); // true

double health = 75.5;
bool isValid = InputValidator.IsWithinRange(health, 0.0, 100.0); // true
```

#### Positive/Non-Negative Validation

```csharp
// Check if value is positive (> 0)
int score = 100;
bool isPositive = InputValidator.IsPositive(score); // true

int zero = 0;
bool isPositive = InputValidator.IsPositive(zero); // false

// Check if value is non-negative (>= 0)
long experience = 0;
bool isValid = InputValidator.IsNonNegative(experience); // true
```

#### Character Validation

```csharp
// Alphanumeric only
string input = "Player123";
bool isValid = InputValidator.IsAlphanumeric(input); // true

// Alphanumeric with allowed special characters
string username = "Player-123_Name";
bool isValid = InputValidator.ContainsOnlyAllowedCharacters(username, "-_"); // true
```

#### Pattern Matching

```csharp
// Validate against a pattern
string code = "ABC123";
bool matches = InputValidator.MatchesPattern(code, @"^[A-Z]{3}\d{3}$"); // true
```

#### Email Validation

```csharp
// Validate email format
string email = "player@example.com";
bool isValid = InputValidator.IsValidEmail(email); // true

string invalidEmail = "not-an-email";
bool isValid = InputValidator.IsValidEmail(invalidEmail); // false
```

## XssProtection

The `XssProtection` class provides context-aware output encoding to prevent XSS attacks.

### Features

- **HTML Encoding**: Encodes characters for safe HTML content display
- **HTML Attribute Encoding**: Encodes for safe use in HTML attributes
- **JavaScript Encoding**: Encodes for safe use in JavaScript strings
- **URL Encoding**: Percent-encodes for safe use in URLs
- **HTML Tag Stripping**: Removes all HTML tags from input
- **Content Sanitization**: Combines stripping and encoding for comprehensive protection
- **Dangerous Content Detection**: Identifies common XSS attack patterns
- **Safe Display Validation**: Validates content is safe without encoding

### Usage Examples

#### HTML Encoding

```csharp
using BlueMarble.World;

// Encode for HTML content
string userInput = "<script>alert('XSS')</script>";
string safe = XssProtection.EncodeForHtml(userInput);
// Result: "&lt;script&gt;alert(&#x27;XSS&#x27;)&lt;&#x2F;script&gt;"

// Encode for HTML attributes
string attributeValue = "test\"value";
string safe = XssProtection.EncodeForHtmlAttribute(attributeValue);
// Result: "test&quot;value"
```

#### JavaScript Encoding

```csharp
// Encode for JavaScript strings
string userInput = "It's working";
string safe = XssProtection.EncodeForJavaScript(userInput);
// Result: "It\\'s working"

// Prevents script breakout
string malicious = "</script><script>alert('XSS')</script>";
string safe = XssProtection.EncodeForJavaScript(malicious);
// Result contains \\x3C and \\x3E instead of < and >
```

#### URL Encoding

```csharp
// Encode for URLs
string queryParam = "value with spaces";
string safe = XssProtection.EncodeForUrl(queryParam);
// Result: "value%20with%20spaces"
```

#### HTML Tag Stripping

```csharp
// Remove all HTML tags
string htmlInput = "<p>Hello <b>World</b></p>";
string textOnly = XssProtection.StripHtmlTags(htmlInput);
// Result: "Hello World"
```

#### Content Sanitization

```csharp
// Comprehensive sanitization
string untrustedInput = "<script>alert('XSS')</script>Safe Text";
string safe = XssProtection.Sanitize(untrustedInput);
// Result: "alert(&#x27;XSS&#x27;)Safe Text"
```

#### Dangerous Content Detection

```csharp
// Detect XSS patterns
string input1 = "Hello World";
bool isDangerous = XssProtection.ContainsDangerousContent(input1); // false

string input2 = "<script>alert('XSS')</script>";
bool isDangerous = XssProtection.ContainsDangerousContent(input2); // true

string input3 = "javascript:alert('XSS')";
bool isDangerous = XssProtection.ContainsDangerousContent(input3); // true
```

#### Safe Display Validation

```csharp
// Check if content is safe for display
string safe = "Hello, World!";
bool isSafe = XssProtection.IsSafeForDisplay(safe); // true

string unsafe = "<script>alert('XSS')</script>";
bool isSafe = XssProtection.IsSafeForDisplay(unsafe); // false
```

## Best Practices

### Input Validation

1. **Always validate on the server**: Never trust client-side validation alone
2. **Use whitelist approach**: Define what is allowed rather than what is forbidden
3. **Validate early**: Check inputs as soon as they enter your system
4. **Validate at boundaries**: Check data at API endpoints, before database operations
5. **Use appropriate validators**: Choose the right validation method for your data type
6. **Provide clear error messages**: Help users understand what went wrong

### XSS Protection

1. **Encode context-appropriately**: Use the right encoding for HTML, JavaScript, URLs, etc.
2. **Never trust user input**: Treat all user-provided data as potentially malicious
3. **Sanitize before storage**: Clean data before saving to database
4. **Encode before display**: Always encode when displaying user content
5. **Use Content Security Policy**: Implement CSP headers as an additional layer
6. **Validate and sanitize together**: Use both validation and encoding

## Integration Example

Here's a complete example of validating and sanitizing user input:

```csharp
using BlueMarble.World;

public class UserProfileService
{
    public Result UpdateUserProfile(string username, string bio, int age)
    {
        // Validate username
        if (!InputValidator.IsWithinLengthRange(username, 3, 20))
        {
            return Result.Error("Username must be between 3 and 20 characters");
        }
        
        if (!InputValidator.ContainsOnlyAllowedCharacters(username, "-_"))
        {
            return Result.Error("Username can only contain letters, numbers, hyphens, and underscores");
        }
        
        if (!InputValidator.IsSafeSqlInput(username))
        {
            return Result.Error("Username contains invalid characters");
        }
        
        // Validate bio
        if (!InputValidator.IsWithinLengthLimit(bio, 500))
        {
            return Result.Error("Bio must be 500 characters or less");
        }
        
        if (XssProtection.ContainsDangerousContent(bio))
        {
            return Result.Error("Bio contains potentially dangerous content");
        }
        
        // Sanitize bio for safe storage and display
        string safeBio = XssProtection.Sanitize(bio);
        
        // Validate age
        if (!InputValidator.IsWithinRange(age, 13, 120))
        {
            return Result.Error("Age must be between 13 and 120");
        }
        
        // Proceed with safe data
        return SaveUserProfile(username, safeBio, age);
    }
    
    public string GetUserBioForDisplay(string bio)
    {
        // Encode for safe HTML display
        return XssProtection.EncodeForHtml(bio);
    }
    
    public string GetUserBioForJavaScript(string bio)
    {
        // Encode for safe JavaScript use
        return XssProtection.EncodeForJavaScript(bio);
    }
}
```

## API Endpoint Example

```csharp
using BlueMarble.World;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class PlanetController : ControllerBase
{
    [HttpPost]
    public IActionResult CreatePlanet([FromBody] CreatePlanetRequest request)
    {
        // Validate planet name
        if (!InputValidator.IsWithinLengthRange(request.Name, 1, 100))
        {
            return BadRequest(new { error = "Planet name must be between 1 and 100 characters" });
        }
        
        if (!InputValidator.IsSafeSqlInput(request.Name))
        {
            return BadRequest(new { error = "Planet name contains invalid characters" });
        }
        
        if (XssProtection.ContainsDangerousContent(request.Name))
        {
            return BadRequest(new { error = "Planet name contains potentially dangerous content" });
        }
        
        // Sanitize name
        string safeName = XssProtection.Sanitize(request.Name);
        
        // Validate radius
        if (!InputValidator.IsPositive(request.Radius))
        {
            return BadRequest(new { error = "Radius must be positive" });
        }
        
        if (!InputValidator.IsWithinRange(request.Radius, 1000.0, 100000.0))
        {
            return BadRequest(new { error = "Radius must be between 1000 and 100000" });
        }
        
        // Proceed with validated and sanitized data
        var planet = CreatePlanet(safeName, request.Radius);
        return Ok(planet);
    }
}
```

## Security Considerations

### Defense in Depth

These utilities are part of a defense-in-depth security strategy:

1. **Input Validation**: First line of defense - reject invalid data
2. **Output Encoding**: Prevent XSS when displaying data
3. **Parameterized Queries**: Use with validation to prevent SQL injection
4. **Content Security Policy**: Additional browser-level protection
5. **Regular Security Audits**: Continuously review and update security measures

### What These Utilities Don't Do

- **Database Layer Protection**: Always use parameterized queries/ORMs
- **Authentication**: Implement separate authentication mechanisms
- **Authorization**: Use role-based access control
- **Rate Limiting**: Implement API rate limiting separately
- **Advanced Sanitization**: For complex HTML, consider dedicated libraries like HtmlSanitizer

## Testing

Both utilities come with comprehensive test suites:

- **InputValidatorTests**: 70+ tests covering all validation scenarios
- **XssProtectionTests**: 42+ tests covering all encoding and sanitization scenarios

Run tests with:

```bash
dotnet test BlueMarble.World.sln
```

## Performance Considerations

- **Regex Compilation**: SQL injection patterns are pre-compiled for performance
- **StringBuilder Usage**: Encoding methods use StringBuilder for efficient string building
- **Minimal Allocations**: Methods are optimized to minimize memory allocations
- **Early Returns**: Validation methods return early when possible

## Future Enhancements

Potential improvements for future versions:

- [ ] Configurable SQL injection patterns
- [ ] HTML sanitization allowlist support
- [ ] Internationalization support for validation messages
- [ ] Custom validation rule chaining
- [ ] Performance monitoring and metrics
- [ ] Additional encoding contexts (XML, CSS, etc.)

## References

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [OWASP XSS Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html)
- [OWASP Input Validation Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Input_Validation_Cheat_Sheet.html)
- [Security Framework Design](../../docs/systems/security-framework-design.md)
- [API Specifications](../../docs/systems/api-specifications.md)

## Related Documentation

- [Security Framework Design](../../docs/systems/security-framework-design.md)
- [API Specifications](../../docs/systems/api-specifications.md)
- [QA Test Plan](../../docs/systems/qa-test-plan-spherical-planet.md)
- [Technical Design Document](../../docs/core/technical-design-document.md)
