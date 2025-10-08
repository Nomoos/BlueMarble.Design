# Story Quality Scorer - Quick Reference

---
title: Story Quality Scorer Quick Reference Guide
date: 2025-01-24
owner: @copilot
status: active
priority: P1
tags: [quick-reference, quality-assessment, cheat-sheet]
---

## Overview

This quick reference guide provides a summary of the Story Quality Assessment and Scoring system for BlueMarble. For detailed information, see the [full framework](story-quality-assessment-framework.md) and [implementation guide](story-quality-scorer-implementation-guide.md).

---

## Quality Score Formula

```
Overall Score = (Narrative × 0.25) + (Technical × 0.30) + 
                (Geological × 0.20) + (Experience × 0.15) + 
                (Community × 0.10)
```

**Range:** 0-100 points

---

## Quality Tiers

| Tier | Score Range | Description | Action |
|------|-------------|-------------|--------|
| **Exceptional** | 85-100 | Outstanding quality | Featured content, bonus rewards |
| **High Quality** | 70-84 | Strong quality | Standard approval |
| **Acceptable** | 55-69 | Good with minor issues | Approved with recommendations |
| **Needs Improvement** | 40-54 | Significant issues | Rejected, revision required |
| **Rejected** | 0-39 | Does not meet standards | Not approved |

---

## Scoring Dimensions

### 1. Narrative Quality (25% weight)

**Components:**
- Grammar & Clarity: 25 points
- Story Coherence: 25 points
- Engagement Factor: 25 points
- Character Development: 25 points

**Total:** 100 points → scaled to 0-100 score

**Quick Check:**
- ✓ No spelling/grammar errors
- ✓ Logical story progression
- ✓ Compelling opening and conflict
- ✓ Distinct characters with motivations

---

### 2. Technical Quality (30% weight)

**Components:**
- Completability: 30 points
- Performance Impact: 30 points
- Integration Quality: 40 points

**Total:** 100 points → scaled to 0-100 score

**Quick Check:**
- ✓ Quest can be completed
- ✓ All items/NPCs exist
- ✓ Valid locations
- ✓ Performance < 10ms
- ✓ No conflicts with other content

---

### 3. Geological Accuracy (20% weight)

**Components:**
- Scientific Accuracy: 40 points
- Educational Value: 30 points
- Resource Authenticity: 30 points

**Total:** 100 points → scaled to 0-100 score

**Quick Check:**
- ✓ Correct geological terminology
- ✓ Accurate material properties
- ✓ Realistic processes
- ✓ Materials in correct formations

---

### 4. Player Experience (15% weight)

**Components:**
- Time Investment vs Rewards: 25 points
- Clarity & Direction: 25 points
- Fun Factor: 25 points
- Accessibility: 25 points

**Total:** 100 points → scaled to 0-100 score

**Quick Check:**
- ✓ Balanced rewards
- ✓ Clear objectives
- ✓ Engaging gameplay
- ✓ Multiple solution paths

---

### 5. Community Validation (10% weight)

**Components:**
- Completion Rate: 25 points
- Player Ratings: 25 points
- Reports & Issues: 25 points
- Creator Reputation: 25 points

**Total:** 100 points → scaled to 0-100 score

**Quick Check:**
- ✓ >65% completion rate
- ✓ >4.0 star rating
- ✓ No major bug reports
- ✓ Positive creator history

---

## Auto-Rejection Criteria

**Immediate Rejection (Critical Failures):**
- ❌ Quest is impossible to complete
- ❌ Contains offensive/inappropriate content
- ❌ Exploitative rewards or mechanics
- ❌ Violates content rating guidelines
- ❌ Plagiarized content
- ❌ Performance impact >50ms

**Technical Score Threshold:**
- Technical score < 30 → Automatic rejection

---

## API Quick Reference

### Assess Story Quality

```http
POST /api/v1/stories/{storyId}/quality/assess
Authorization: Bearer {token}
```

**Response:**
```json
{
  "overallScore": 78.5,
  "tier": "HighQuality",
  "status": "Approved",
  "breakdown": {
    "narrative": 75.0,
    "technical": 85.0,
    "geological": 72.0,
    "experience": 80.0,
    "community": 65.0
  },
  "feedback": {
    "strengths": [
      "Excellent narrative quality",
      "Technically sound implementation"
    ],
    "improvementAreas": [
      "Consider adding more geological detail"
    ]
  },
  "assessedAt": "2025-01-24T10:30:00Z"
}
```

### Get Latest Score

```http
GET /api/v1/stories/{storyId}/quality/latest
```

### Validate Before Submission

```http
POST /api/v1/stories/{storyId}/quality/validate
Authorization: Bearer {token}
```

**Response:**
```json
{
  "isValid": true,
  "errors": [],
  "warnings": [
    {
      "code": "MINOR_SPELLING",
      "severity": "Warning",
      "message": "2 spelling errors detected",
      "field": "Description"
    }
  ]
}
```

---

## Creator Cheat Sheet

### How to Get 85+ (Exceptional)

**Narrative (Target: 85+)**
- Professional writing quality
- Compelling story with clear arc
- Engaging characters
- No grammar/spelling errors

**Technical (Target: 90+)**
- Thoroughly tested
- Fast performance (<5ms)
- No integration issues
- All requirements verified

**Geological (Target: 80+)**
- Research geological concepts
- Use correct terminology
- Verify material properties
- Request expert review if needed

**Experience (Target: 85+)**
- Well-balanced rewards
- Crystal clear objectives
- Multiple approaches
- Fun and engaging

**Community (Target: 75+)**
- Beta test with players
- Quick bug fixes
- Build good reputation
- Respond to feedback

---

## Common Issues and Fixes

### Low Narrative Score

**Problem:** Grammar/spelling errors  
**Fix:** Use spell checker, proofread carefully

**Problem:** Confusing story  
**Fix:** Clear cause-effect, logical progression

**Problem:** Boring content  
**Fix:** Add conflict, choices, character depth

### Low Technical Score

**Problem:** Quest uncompletable  
**Fix:** Test thoroughly, verify all objectives

**Problem:** Performance issues  
**Fix:** Optimize queries, reduce complexity

**Problem:** Integration conflicts  
**Fix:** Check for duplicate content, test interactions

### Low Geological Score

**Problem:** Inaccurate terminology  
**Fix:** Research correct terms, use knowledge base

**Problem:** Wrong materials  
**Fix:** Verify formation types, material properties

**Problem:** Unrealistic processes  
**Fix:** Check timescales, process accuracy

### Low Experience Score

**Problem:** Poor reward balance  
**Fix:** Compare to similar quests, test time-to-complete

**Problem:** Unclear objectives  
**Fix:** Add waypoints, clear descriptions, hints

**Problem:** Too difficult  
**Fix:** Add easier solution paths, adjust requirements

---

## Scoring Examples

### Example 1: Exceptional Quest (89 points)

```
Title: "The Iron Vein Mystery"

Scores:
- Narrative: 85/100 (well-written, engaging)
- Technical: 92/100 (perfect execution)
- Geological: 88/100 (excellent accuracy)
- Experience: 86/100 (great balance)
- Community: 91/100 (82% completion, 4.6 stars)

Overall: 89.0 → Exceptional Tier
Status: Approved - Featured Content
```

### Example 2: Needs Improvement (47 points)

```
Title: "Get Some Rocks"

Scores:
- Narrative: 35/100 (poor writing, errors)
- Technical: 58/100 (works but has issues)
- Geological: 42/100 (inaccurate terms)
- Experience: 51/100 (poor balance)
- Community: N/A (not published yet)

Overall: 47.0 → Needs Improvement Tier
Status: Rejected - Revisions Required

Issues:
- Fix spelling/grammar errors
- Add clearer objectives
- Research correct geological terminology
- Improve reward balance
```

---

## Developer Integration Points

### Service Initialization

```csharp
services.AddScoped<IQualityAssessmentService, QualityAssessmentService>();
services.AddScoped<INarrativeQualityAssessor, NarrativeQualityAssessor>();
services.AddScoped<ITechnicalQualityAssessor, TechnicalQualityAssessor>();
// ... other assessors
```

### Basic Usage

```csharp
// Assess a story
var score = await _qualityService.AssessStoryAsync(storyId);

// Check if approved
if (score.Status == ApprovalStatus.Approved)
{
    await _storyService.PublishAsync(storyId);
}

// Validate before assessment
var validation = await _qualityService.ValidateStoryAsync(storyId);
if (!validation.IsValid)
{
    return BadRequest(validation.Errors);
}
```

---

## Monitoring Metrics

**Key Metrics to Track:**
- Average assessment time (target: <5 seconds)
- Average quality score (target: >70)
- Approval rate (target: 75-85%)
- Exceptional content rate (target: >10%)
- False positive rate (target: <5%)
- Creator satisfaction (target: >75%)

---

## Related Documentation

- **[Full Framework](story-quality-assessment-framework.md)** - Complete specification with all details
- **[Implementation Guide](story-quality-scorer-implementation-guide.md)** - Technical implementation guide for developers
- **[Trust and Reputation Systems](../../../topics/trust-player-created-quests-reputation-systems.md)** - How quality integrates with reputation
- **[Community Content Guidelines](../../../topics/community-content-ugc-quest-tools-sandbox.md)** - UGC best practices

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2025-01-24 | Initial quick reference |

---

**Last Updated:** 2025-01-24  
**Status:** Active
