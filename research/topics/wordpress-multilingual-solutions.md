# WordPress Multilingual Solutions Research

---
title: Free WordPress Multilingual Solutions with API Support
date: 2025-11-21
owner: @copilot
status: complete
tags: [wordpress, multilingual, api, cms, translation, content-management]
---

## Problem / Context

The BlueMarble project requires research into free multilingual solutions for WordPress that provide API capabilities for:
- Creating draft content
- Editing draft content
- Publishing content

This research evaluates available open-source and free WordPress multilingual plugins, focusing on their API capabilities and programmatic content management features.

## Key Findings

- **Polylang (Free)**: Best free option with comprehensive REST API support for multilingual content management
  - Full REST API integration for programmatic access
  - Supports draft creation, editing, and publishing via API
  - Well-documented API endpoints
  - Active development and community support

- **WPML**: Most feature-rich but requires paid license for API access
  - API functionality only available in premium versions
  - Not suitable for free solution requirement

- **TranslatePress**: Limited API capabilities in free version
  - Free version lacks comprehensive API support
  - Translation management via interface only
  - Programmatic access requires premium add-ons

- **MultilingualPress**: Good free alternative with REST API support
  - Uses WordPress multisite architecture
  - Built on WordPress REST API
  - More complex setup but powerful capabilities

## Evidence

### Polylang - Free Plugin with REST API

**Link**: https://polylang.pro/doc/rest-api/

**Key Points:**
- REST API support included in free version
- Extends WordPress REST API with language parameters
- Language filtering for posts, pages, and custom post types
- Draft creation and management through standard WordPress API
- Publishing workflow supported

**API Capabilities:**
```php
// Get posts in specific language
GET /wp-json/wp/v2/posts?lang=en

// Create draft in specific language
POST /wp-json/wp/v2/posts
{
  "title": "Post Title",
  "content": "Post content",
  "status": "draft",
  "lang": "en"
}

// Update draft
PUT /wp-json/wp/v2/posts/{id}
{
  "content": "Updated content",
  "lang": "en"
}

// Publish post
PUT /wp-json/wp/v2/posts/{id}
{
  "status": "publish",
  "lang": "en"
}

// Link translations
POST /wp-json/pll/v1/translations
{
  "post_id": 123,
  "translations": {
    "en": 123,
    "fr": 456
  }
}
```

**Relevance**: Polylang provides comprehensive API support in its free version, making it the strongest candidate for programmatic multilingual content management.

### WPML - Premium Solution

**Link**: https://wpml.org/documentation/support/wpml-rest-api/

**Key Points:**
- REST API only available in WPML Multilingual CMS package (paid)
- Free version lacks API capabilities
- Most mature and feature-rich solution
- Extensive documentation and support

**Relevance**: While WPML is the industry standard, it requires a paid license for API access, making it unsuitable for free solution requirements.

### TranslatePress - Visual Translation

**Link**: https://translatepress.com/docs/developers/wordpress-translation-api/

**Key Points:**
- Free version has limited API support
- Focuses on visual, front-end translation
- API access requires Business plan
- Good for content translation but limited programmatic control

**Relevance**: TranslatePress is user-friendly but lacks free API access needed for automated content management.

### MultilingualPress - Multisite Solution

**Link**: https://multilingualpress.org/docs/

**Key Points:**
- Free and open-source
- Built on WordPress multisite architecture
- Integrates with WordPress REST API
- Each language as separate site
- Cross-site content relationships

**API Example:**
```php
// Uses standard WordPress REST API per site
// Site 1 (English): https://site.com/en/wp-json/wp/v2/posts
// Site 2 (French): https://site.com/fr/wp-json/wp/v2/posts

// Content relationships managed through plugin
// Each site maintains its own content
```

**Relevance**: MultilingualPress offers a different architectural approach using multisite, providing full API access through native WordPress endpoints.

### WordPress Core Translation Functions

**Key Points:**
- WordPress provides native internationalization (i18n) framework
- `get_locale()` and `switch_to_locale()` functions
- REST API supports locale parameter
- Content translation requires plugin extension

**Basic Implementation:**
```php
// WordPress native locale switching
switch_to_locale('fr_FR');
// ... create/edit content
restore_current_locale();

// REST API with locale
GET /wp-json/wp/v2/posts?locale=fr_FR
```

**Relevance**: WordPress core provides foundation, but plugins extend with content translation management.

## Comparison Matrix

| Feature | Polylang (Free) | WPML | TranslatePress | MultilingualPress |
|---------|-----------------|------|----------------|-------------------|
| **Cost** | Free | Paid ($39-$199/year) | Free/$99-$239/year | Free |
| **REST API Support** | ✓ Yes (Free) | ✓ Yes (Paid only) | ✗ No (Premium only) | ✓ Yes (Free) |
| **Draft Creation API** | ✓ Full support | ✓ Full support | ✗ Limited | ✓ Full support |
| **Draft Edit API** | ✓ Full support | ✓ Full support | ✗ Limited | ✓ Full support |
| **Publishing API** | ✓ Full support | ✓ Full support | ✗ Limited | ✓ Full support |
| **Architecture** | Single site | Single site | Single site | Multisite |
| **Setup Complexity** | Low | Low | Low | Medium-High |
| **Active Development** | ✓ Yes | ✓ Yes | ✓ Yes | ✓ Yes |
| **Documentation** | Good | Excellent | Good | Good |
| **Community Support** | Large | Largest | Growing | Medium |

## Detailed Solution Analysis

### Polylang (Recommended Free Solution)

**Strengths:**
- Comprehensive REST API in free version
- Clean integration with WordPress core
- Supports all post types and taxonomies
- Active development and updates
- Large user base (600,000+ active installations)
- Compatible with most WordPress themes and plugins

**API Workflow:**
1. **Create Draft**: Use standard WordPress REST API with `lang` parameter
2. **Edit Draft**: Update post via REST API maintaining language context
3. **Publish**: Change post status to 'publish' via API
4. **Link Translations**: Use Polylang API to connect related translations

**Example Implementation:**
```javascript
// JavaScript example using WordPress REST API with Polylang

// 1. Create English draft
async function createDraft(title, content) {
  const response = await fetch('/wp-json/wp/v2/posts', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      title: title,
      content: content,
      status: 'draft',
      lang: 'en'
    })
  });
  return await response.json();
}

// 2. Edit draft
async function editDraft(postId, newContent) {
  const response = await fetch(`/wp-json/wp/v2/posts/${postId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      content: newContent
    })
  });
  return await response.json();
}

// 3. Publish post
async function publishPost(postId) {
  const response = await fetch(`/wp-json/wp/v2/posts/${postId}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      status: 'publish'
    })
  });
  return await response.json();
}

// 4. Create French translation and link
async function createTranslation(originalPostId, title, content) {
  // Create French draft
  const frPost = await fetch('/wp-json/wp/v2/posts', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      title: title,
      content: content,
      status: 'draft',
      lang: 'fr'
    })
  });
  
  const frPostData = await frPost.json();
  
  // Link translations
  await fetch('/wp-json/pll/v1/translations', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      post_id: originalPostId,
      translations: {
        en: originalPostId,
        fr: frPostData.id
      }
    })
  });
  
  return frPostData;
}
```

**Limitations:**
- Pro version ($99/year) adds advanced features like automatic translation
- Free version requires manual translation management
- Some advanced features require custom development

### MultilingualPress (Alternative Free Solution)

**Strengths:**
- Completely free and open-source
- Full access to WordPress REST API per site
- Separation of content by language (each language is separate site)
- Enterprise-level scalability
- Good for large, complex multilingual sites

**Architecture:**
- Requires WordPress multisite installation
- Each language operates as separate site in network
- Content relationships managed across sites
- Independent API endpoints per language site

**API Workflow:**
```javascript
// MultilingualPress uses separate API endpoints per site

// English site API
const enSiteApi = 'https://example.com/en/wp-json/wp/v2';

// French site API  
const frSiteApi = 'https://example.com/fr/wp-json/wp/v2';

// Create draft on English site
async function createEnglishDraft(title, content) {
  const response = await fetch(`${enSiteApi}/posts`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      title: title,
      content: content,
      status: 'draft'
    })
  });
  return await response.json();
}

// Create related French draft
async function createFrenchDraft(title, content, relatedEnPostId) {
  const response = await fetch(`${frSiteApi}/posts`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + token
    },
    body: JSON.stringify({
      title: title,
      content: content,
      status: 'draft',
      meta: {
        multilingualpress_related_post: relatedEnPostId
      }
    })
  });
  return await response.json();
}
```

**Limitations:**
- Requires multisite setup (more complex infrastructure)
- Separate databases per language site
- Higher resource requirements
- More complex content synchronization

## Implementation Recommendations

### For Simple Projects (Recommended: Polylang)

**Use Case:** Single WordPress installation, straightforward multilingual content

**Setup:**
1. Install Polylang free plugin from WordPress.org
2. Configure languages in WordPress admin
3. Enable REST API support in Polylang settings
4. Use WordPress REST API authentication (Application Passwords, OAuth, JWT)
5. Implement content creation using documented API endpoints

**Benefits:**
- Quick setup and deployment
- Low maintenance overhead
- Excellent documentation
- Large community support
- Suitable for most use cases

### For Enterprise Projects (Consider: MultilingualPress)

**Use Case:** Large-scale multilingual sites, separate content teams per language, high traffic

**Setup:**
1. Configure WordPress multisite network
2. Create site per language
3. Install MultilingualPress plugin
4. Configure site relationships
5. Use standard WordPress API per site

**Benefits:**
- Complete content separation
- Independent scaling per language
- Team isolation (separate admin per language)
- Enterprise-level reliability
- Full REST API access per site

## Implications for Design

### For BlueMarble Project

**Primary Recommendation: Polylang**

- **Low Complexity**: Single site architecture, straightforward API integration
- **Cost Effective**: Free version provides all necessary API capabilities
- **Developer Friendly**: Well-documented REST API extensions
- **Scalable**: Supports growth from small to medium-large sites
- **Community Support**: Large user base and active development

**Integration Considerations:**

1. **Authentication**: Implement WordPress Application Passwords or JWT authentication
2. **Content Workflow**: Design clear draft → review → publish pipeline
3. **Translation Management**: Decide on manual vs automatic translation approach
4. **API Rate Limiting**: Implement appropriate request throttling
5. **Caching Strategy**: Cache translated content appropriately
6. **Fallback Language**: Define default language when translation unavailable

**Technical Architecture:**
```
External Application
    ↓ (HTTP/REST)
WordPress REST API + Polylang
    ↓
WordPress Core (Posts, Pages, Custom Post Types)
    ↓
Database (with language metadata)
```

### Alternative: MultilingualPress

**When to Consider:**
- Enterprise-scale multilingual requirements
- Need for complete content separation
- Multiple content teams per language
- Regulatory requirements for data isolation
- High-traffic sites requiring independent scaling

**Additional Complexity:**
- Multisite infrastructure management
- Cross-site authentication
- Content synchronization logic
- Higher hosting requirements

## Security Considerations

### API Authentication

**Required for all solutions:**
- Use WordPress Application Passwords (WordPress 5.6+)
- Implement OAuth 2.0 for advanced scenarios
- JWT tokens for stateless authentication
- Always use HTTPS for API communication

**Example Authentication:**
```javascript
// Application Password authentication
const username = 'admin';
const applicationPassword = 'xxxx xxxx xxxx xxxx xxxx xxxx';
const token = btoa(`${username}:${applicationPassword}`);

fetch('/wp-json/wp/v2/posts', {
  headers: {
    'Authorization': 'Basic ' + token
  }
});
```

### Permission Management

- Define appropriate user roles and capabilities
- Restrict API access to authorized users only
- Implement rate limiting to prevent abuse
- Log API access for security monitoring
- Regular security updates for WordPress and plugins

## Performance Considerations

### Caching Strategy

**Polylang:**
- Cache API responses per language
- Use WordPress transients API
- Implement object caching (Redis/Memcached)
- CDN for static translated content

**Optimization Tips:**
- Lazy load translations
- Preload frequently accessed content
- Use database query optimization
- Enable opcache for PHP
- Implement full-page caching per language

## Open Questions / Next Steps

### Open Questions

- What specific languages need to be supported?
- What is the expected content volume per language?
- Will translations be manual, automatic, or hybrid?
- What is the content approval workflow?
- Are there specific compliance requirements (GDPR, etc.)?

### Next Steps

- [ ] Evaluate Polylang in test WordPress installation
- [ ] Test REST API endpoints for draft creation, editing, and publishing
- [ ] Document complete API workflow with authentication
- [ ] Create proof-of-concept implementation
- [ ] Benchmark API performance with multiple languages
- [ ] Test compatibility with required WordPress themes/plugins
- [ ] Develop content synchronization strategy
- [ ] Define translation workflow and approval process
- [ ] Document deployment and maintenance procedures
- [ ] Create monitoring and logging strategy

## Related Documents

- [Research Index](../index.md) - Master research index
- [Documentation Best Practices](../../DOCUMENTATION_BEST_PRACTICES.md) - Documentation guidelines
- [WordPress REST API Documentation](https://developer.wordpress.org/rest-api/)
- [Polylang Documentation](https://polylang.pro/doc/)
- [MultilingualPress Documentation](https://multilingualpress.org/docs/)

## Conclusion

**Polylang** is the recommended free solution for WordPress multilingual content management with comprehensive API support. It provides:

✓ Full REST API access in free version
✓ Complete support for draft creation, editing, and publishing
✓ Well-documented API endpoints
✓ Large community and active development
✓ Low complexity and maintenance overhead
✓ Compatible with WordPress ecosystem

**MultilingualPress** is a viable alternative for enterprise scenarios requiring complete content separation and independent scaling per language, though it requires more complex multisite infrastructure.

Both solutions provide the necessary API capabilities for programmatic content management while remaining free and open-source.
