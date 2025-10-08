# Story Quality Assessment and Scoring Framework

---
title: Story Quality Assessment and Scoring Framework for Player-Created Content
date: 2025-01-24
owner: @copilot
status: active
priority: P1
tags: [quality-assessment, story-scoring, content-validation, player-created-quests, quality-control]
related-research: trust-player-created-quests-reputation-systems.md, community-content-ugc-quest-tools-sandbox.md, content-velocity-quest-generation-sandbox-mmorpg.md
---

## Executive Summary

This document defines a comprehensive quality assessment and scoring system for evaluating story-based content in BlueMarble, with specific focus on player-created quests and narrative elements. The framework provides automated validation, human-readable metrics, and integration guidelines to ensure high-quality content while supporting player creativity.

**Key Features:**
- Multi-dimensional quality scoring (0-100 scale)
- Automated validation checks
- Community-driven quality signals
- Integration with reputation systems
- Educational content alignment scoring

**Quality Tiers:**
- **Exceptional** (85-100): Featured content, high visibility
- **High Quality** (70-84): Standard approved content
- **Acceptable** (55-69): Approved with minor issues
- **Needs Improvement** (40-54): Requires revision
- **Rejected** (<40): Does not meet minimum standards

---

## Overview

### Purpose

The Story Quality Assessment and Scoring Framework ensures that player-created content maintains quality standards while encouraging creativity and educational value. This system balances automated validation with community feedback to create a sustainable content ecosystem.

### Design Goals

1. **Quality Consistency**: Maintain minimum quality standards across all content
2. **Player Agency**: Support creative freedom within defined boundaries
3. **Educational Alignment**: Ensure content supports BlueMarble's geological education mission
4. **Scalability**: Handle thousands of submissions with minimal manual review
5. **Transparency**: Provide clear feedback to content creators
6. **Community Trust**: Build confidence in player-created content through reliable scoring

---

## Quality Assessment Dimensions

### 1. Narrative Quality (Weight: 25%)

**Purpose**: Evaluate the story's coherence, engagement, and writing quality.

#### Scoring Criteria

**Grammar and Clarity (0-25 points)**
- Spelling and grammar errors (auto-detected)
- Sentence structure and readability
- Clear objective descriptions
- Consistent terminology

**Scoring Rubric:**
```
23-25: Professional quality, no errors detected
20-22: Minor issues (1-2 errors per 100 words)
15-19: Moderate issues (3-5 errors per 100 words)
10-14: Significant issues (6-10 errors per 100 words)
0-9:   Severe issues (10+ errors per 100 words)
```

**Story Coherence (0-25 points)**
- Logical progression of events
- Clear cause and effect relationships
- Consistent character motivations
- Satisfying resolution

**Engagement Factor (0-25 points)**
- Compelling opening
- Interesting conflict or challenge
- Player agency and meaningful choices
- Emotional resonance or excitement

**Character Development (0-25 points)**
- Distinct character voices
- Believable motivations
- Character growth or change
- Relationship dynamics

**Calculation:**
```
Narrative_Score = (Grammar + Coherence + Engagement + Character) / 4
```

---

### 2. Technical Quality (Weight: 30%)

**Purpose**: Ensure quests are technically sound, completable, and performant.

#### Automated Validation Checks

**Completability (0-30 points)**
- ✅ All objectives can be completed
- ✅ Required items/NPCs exist in game
- ✅ Location coordinates are valid
- ✅ No circular dependencies
- ✅ Success conditions are achievable

**Scoring:**
```
30: All checks pass
24: 1 minor issue (e.g., long travel distance)
18: 2 minor issues or 1 moderate issue
12: 3+ minor issues or 2 moderate issues
0:  Critical failure (quest uncompletable)
```

**Performance Impact (0-30 points)**
- Server load assessment
- Client performance impact
- Database query efficiency
- Asset loading requirements

**Scoring:**
```
30: Negligible impact (<1ms processing)
24: Low impact (1-5ms processing)
18: Moderate impact (5-10ms processing)
12: High impact (10-50ms processing)
0:  Unacceptable impact (>50ms processing)
```

**Integration Quality (0-40 points)**
- Proper use of game systems
- Integration with existing content
- No conflicts with other quests
- Appropriate difficulty gating
- Proper reward scaling

**Calculation:**
```
Technical_Score = (Completability × 0.3) + (Performance × 0.3) + (Integration × 0.4)
```

---

### 3. Geological Accuracy (Weight: 20%)

**Purpose**: Ensure content aligns with BlueMarble's educational mission and geological realism.

#### Scoring Criteria

**Scientific Accuracy (0-40 points)**
- Correct geological terminology
- Accurate material properties
- Realistic geological processes
- Proper scale and time frames

**Scoring Rubric:**
```
36-40: Scientifically accurate, could be used for education
30-35: Generally accurate with minor simplifications
20-29: Some inaccuracies but doesn't mislead
10-19: Multiple inaccuracies or confusing information
0-9:  Scientifically incorrect or misleading
```

**Educational Value (0-30 points)**
- Teaches geological concepts
- Encourages exploration and discovery
- Connects to real-world geology
- Age-appropriate complexity

**Resource Authenticity (0-30 points)**
- Materials exist in correct formations
- Appropriate extraction methods
- Realistic processing requirements
- Proper quality ranges for materials

**Calculation:**
```
Geological_Score = (Accuracy × 0.4) + (Educational × 0.3) + (Authenticity × 0.3)
```

---

### 4. Player Experience (Weight: 15%)

**Purpose**: Measure how enjoyable and engaging the content is for players.

#### Metrics

**Time Investment vs Rewards (0-25 points)**
- Appropriate reward scaling
- Time-to-complete reasonable
- Effort-to-benefit ratio balanced
- No exploitation potential

**Clarity and Direction (0-25 points)**
- Clear objectives
- Helpful hints and waypoints
- Progression tracking
- Success/failure feedback

**Fun Factor (0-25 points)**
- Variety in gameplay
- Interesting mechanics
- Surprise or delight elements
- Replay value

**Accessibility (0-25 points)**
- Appropriate skill level
- Multiple solution paths
- Solo and group options
- Inclusive design (various playstyles)

**Calculation:**
```
Experience_Score = (Investment + Clarity + Fun + Accessibility) / 4
```

---

### 5. Community Validation (Weight: 10%)

**Purpose**: Incorporate player feedback and community assessment.

#### Metrics Collection

**Completion Rate (0-25 points)**
- Percentage of players who finish the quest
- Time to abandonment tracking
- Retry attempts

**Scoring:**
```
25: 80%+ completion rate
20: 65-79% completion rate
15: 50-64% completion rate
10: 35-49% completion rate
0:  <35% completion rate
```

**Player Ratings (0-25 points)**
- 5-star rating system
- Weighted by player reputation
- Verified completions only

**Scoring:**
```
25: 4.5+ average rating
20: 4.0-4.4 average rating
15: 3.5-3.9 average rating
10: 3.0-3.4 average rating
0:  <3.0 average rating
```

**Reports and Issues (0-25 points)**
- Bug reports filed
- Exploit reports
- Content guideline violations
- Positive community feedback

**Scoring (Penalty-based):**
```
25: No reports, positive feedback
20: 1-2 minor reports (resolved)
10: 3-5 minor reports or 1 moderate
5:  6+ minor or 2+ moderate reports
0:  Critical violations or major exploits
```

**Creator Reputation (0-25 points)**
- Historical quality score
- Previous content performance
- Community standing
- Response to feedback

**Calculation:**
```
Community_Score = (Completion + Rating + (25 - Reports) + Reputation) / 4
```

---

## Overall Quality Score Calculation

### Formula

```
Quality_Score = (Narrative × 0.25) + (Technical × 0.30) + (Geological × 0.20) + 
                (Experience × 0.15) + (Community × 0.10)
```

### Quality Tiers

**Exceptional (85-100)**
- Featured on main quest board
- Bonus rewards for creator
- Highlighted in community showcases
- Eligible for "Verified Creator" badge

**High Quality (70-84)**
- Standard approval and visibility
- Normal creator rewards
- Regular quest board listing
- Positive reputation impact

**Acceptable (55-69)**
- Approved with limitations
- Lower visibility placement
- Standard creator rewards
- Neutral reputation impact
- Recommendations for improvement provided

**Needs Improvement (40-54)**
- Rejected pending revisions
- Detailed feedback provided
- Resubmission encouraged
- No reputation penalty
- Access to improvement resources

**Rejected (<40)**
- Not approved for publication
- Comprehensive feedback report
- Major revisions required
- Possible content guideline violations
- May impact creator reputation if repeated

---

## Automated Validation System

### Pre-Publication Checks

**Phase 1: Technical Validation (Required)**
```
✓ Quest is completable (simulation test)
✓ All referenced items exist
✓ All locations are valid
✓ No game-breaking bugs detected
✓ Performance benchmarks met
✓ No offensive content detected
✓ Appropriate rating (Teen/PEGI 12)
```

**Phase 2: Content Analysis (Required)**
```
✓ Minimum description length (100 characters)
✓ Grammar and spelling check
✓ Geological terminology validation
✓ Material property accuracy
✓ Reward-to-effort ratio check
✓ No duplicate content detected
```

**Phase 3: Community Review (Optional)**
```
○ Beta tester feedback (24-48 hour period)
○ Community voting (optional)
○ Expert geological review (for featured content)
○ Accessibility review
```

### Auto-Rejection Criteria

**Critical Failures (Immediate Rejection):**
- Quest is impossible to complete
- Contains offensive/inappropriate content
- Exploitative rewards or mechanics
- Violates content rating guidelines
- Plagiarized or duplicate content
- Malicious code or exploits
- Performance impact >50ms

**Automatic Actions:**
```python
if technical_score < 30:
    action = "REJECT_TECHNICAL"
elif offensive_content_detected:
    action = "REJECT_CONTENT_VIOLATION"
elif quality_score < 40:
    action = "REJECT_NEEDS_IMPROVEMENT"
elif quality_score < 55:
    action = "APPROVE_LIMITED"
elif quality_score < 70:
    action = "APPROVE_STANDARD"
else:
    action = "APPROVE_FEATURED"
```

---

## Implementation Architecture

### Quality Scoring Service

```csharp
public class StoryQualityScorer
{
    // Core scoring methods
    public QualityScore CalculateQualityScore(Story story);
    public NarrativeScore AssessNarrative(Story story);
    public TechnicalScore AssessTechnical(Story story);
    public GeologicalScore AssessGeological(Story story);
    public ExperienceScore AssessExperience(Story story);
    public CommunityScore GetCommunityMetrics(Story story);
    
    // Validation
    public ValidationResult RunAutomatedChecks(Story story);
    public bool IsCompletable(Story story);
    public PerformanceMetrics TestPerformance(Story story);
    
    // Feedback generation
    public QualityReport GenerateFeedbackReport(Story story, QualityScore score);
    public List<Improvement> SuggestImprovements(Story story);
}

public class QualityScore
{
    public float OverallScore { get; set; } // 0-100
    public float NarrativeScore { get; set; }
    public float TechnicalScore { get; set; }
    public float GeologicalScore { get; set; }
    public float ExperienceScore { get; set; }
    public float CommunityScore { get; set; }
    
    public QualityTier Tier { get; set; }
    public ApprovalStatus Status { get; set; }
    public DateTime AssessedAt { get; set; }
    public List<string> Strengths { get; set; }
    public List<string> WeaknessAreas { get; set; }
}

public enum QualityTier
{
    Rejected = 0,        // <40
    NeedsImprovement,    // 40-54
    Acceptable,          // 55-69
    HighQuality,         // 70-84
    Exceptional          // 85-100
}

public enum ApprovalStatus
{
    PendingReview,
    Approved,
    ApprovedLimited,
    RejectedTechnical,
    RejectedContent,
    RejectedQuality
}
```

### Database Schema

```sql
-- Story quality scores table
CREATE TABLE story_quality_scores (
    id BIGINT PRIMARY KEY,
    story_id BIGINT NOT NULL,
    overall_score FLOAT NOT NULL,
    narrative_score FLOAT,
    technical_score FLOAT,
    geological_score FLOAT,
    experience_score FLOAT,
    community_score FLOAT,
    quality_tier VARCHAR(20),
    approval_status VARCHAR(30),
    assessed_at TIMESTAMP,
    assessor_type VARCHAR(20), -- 'automated', 'community', 'expert'
    
    FOREIGN KEY (story_id) REFERENCES stories(id),
    INDEX idx_story_score (story_id, assessed_at),
    INDEX idx_quality_tier (quality_tier, overall_score)
);

-- Quality feedback table
CREATE TABLE quality_feedback (
    id BIGINT PRIMARY KEY,
    score_id BIGINT NOT NULL,
    dimension VARCHAR(30),
    strength_text TEXT,
    improvement_text TEXT,
    priority VARCHAR(10), -- 'critical', 'high', 'medium', 'low'
    
    FOREIGN KEY (score_id) REFERENCES story_quality_scores(id)
);

-- Community ratings table
CREATE TABLE community_ratings (
    id BIGINT PRIMARY KEY,
    story_id BIGINT NOT NULL,
    player_id BIGINT NOT NULL,
    rating INT CHECK (rating BETWEEN 1 AND 5),
    completed BOOLEAN DEFAULT FALSE,
    rated_at TIMESTAMP,
    
    FOREIGN KEY (story_id) REFERENCES stories(id),
    FOREIGN KEY (player_id) REFERENCES players(id),
    UNIQUE (story_id, player_id)
);
```

---

## Content Creator Guidelines

### Best Practices for High Scores

**Narrative Excellence (Target: 75+)**
1. Write clear, concise quest descriptions
2. Proofread for grammar and spelling
3. Create logical story progression
4. Include character motivations
5. End with satisfying resolution

**Technical Quality (Target: 80+)**
1. Test quest completion thoroughly
2. Verify all item and location references
3. Optimize for performance
4. Follow integration guidelines
5. Use provided templates

**Geological Accuracy (Target: 70+)**
1. Research geological concepts
2. Use correct terminology
3. Verify material properties
4. Consult geological knowledge base
5. Request expert review for complex content

**Player Experience (Target: 75+)**
1. Balance difficulty appropriately
2. Provide clear objectives
3. Offer multiple solution paths
4. Test with beta players
5. Iterate based on feedback

**Community Validation (Target: 65+)**
1. Engage with player feedback
2. Update based on bug reports
3. Build positive reputation
4. Create consistently quality content
5. Support other creators

### Common Pitfalls to Avoid

**Narrative Issues:**
- ❌ Spelling and grammar errors
- ❌ Confusing or contradictory information
- ❌ Unclear objectives
- ❌ Abrupt or unsatisfying endings
- ❌ Inconsistent character behavior

**Technical Problems:**
- ❌ Uncompletable quests
- ❌ References to non-existent items
- ❌ Invalid coordinates
- ❌ Performance bottlenecks
- ❌ Conflicts with other content

**Geological Inaccuracies:**
- ❌ Impossible material combinations
- ❌ Incorrect geological processes
- ❌ Unrealistic timescales
- ❌ Wrong formation types
- ❌ Misleading educational content

**Experience Issues:**
- ❌ Overly difficult for target audience
- ❌ Boring or repetitive tasks
- ❌ Excessive time requirements
- ❌ Poor reward balance
- ❌ Lack of clear direction

---

## Quality Improvement Tools

### Automated Assistance

**Grammar and Spell Checker**
- Real-time error detection
- Suggestion system
- Context-aware corrections
- Terminology dictionary

**Geological Validator**
- Material property checker
- Formation compatibility checker
- Process accuracy validator
- Educational content reviewer

**Technical Tester**
- Automated quest completion simulation
- Performance benchmarking
- Integration conflict detection
- Reward balance analyzer

### Manual Review Process

**When Manual Review is Triggered:**
1. Quality score between 40-54 (borderline)
2. Community reports (3+ reports)
3. Creator requests review
4. Featured content nomination
5. Educational content certification

**Review Team Structure:**
- Technical reviewers (completability, performance)
- Content reviewers (narrative, experience)
- Geological experts (accuracy, educational value)
- Community moderators (guideline compliance)

---

## Integration with Existing Systems

### Reputation System Integration

```csharp
public class CreatorReputationUpdate
{
    public void UpdateReputation(Player creator, QualityScore score)
    {
        float reputationChange = 0f;
        
        if (score.OverallScore >= 85)
            reputationChange = +15; // Exceptional content
        else if (score.OverallScore >= 70)
            reputationChange = +10; // High quality
        else if (score.OverallScore >= 55)
            reputationChange = +5;  // Acceptable
        else if (score.OverallScore >= 40)
            reputationChange = 0;   // Needs improvement (neutral)
        else
            reputationChange = -5;  // Rejected (penalty)
        
        // Apply reputation bonuses
        creator.CreatorReputation += reputationChange;
        
        // Check for badges
        CheckCreatorBadges(creator);
    }
    
    private void CheckCreatorBadges(Player creator)
    {
        // Award badges based on consistent quality
        var recentScores = GetRecentQualityScores(creator, count: 10);
        
        if (recentScores.All(s => s >= 85))
            AwardBadge(creator, "Master Creator");
        else if (recentScores.All(s => s >= 70))
            AwardBadge(creator, "Verified Creator");
        else if (recentScores.Average() >= 75)
            AwardBadge(creator, "Quality Creator");
    }
}
```

### Quest Visibility Algorithm

```csharp
public class QuestVisibilityRanking
{
    public float CalculateVisibilityScore(Story story)
    {
        var qualityScore = story.QualityScore.OverallScore;
        var communityScore = story.QualityScore.CommunityScore;
        var creatorReputation = story.Creator.CreatorReputation;
        var recency = GetRecencyScore(story.PublishedAt);
        var completionRate = story.CompletionRate;
        
        return (qualityScore × 0.35) +
               (communityScore × 0.25) +
               (creatorReputation × 0.15) +
               (recency × 0.15) +
               (completionRate × 0.10);
    }
    
    private float GetRecencyScore(DateTime publishedAt)
    {
        var daysSincePublish = (DateTime.UtcNow - publishedAt).TotalDays;
        
        if (daysSincePublish < 7)
            return 100f; // New content boost
        else if (daysSincePublish < 30)
            return 80f;
        else if (daysSincePublish < 90)
            return 60f;
        else
            return 40f; // Older content
    }
}
```

### Economic Reward Scaling

```csharp
public class CreatorRewardCalculator
{
    public Reward CalculateCreatorReward(Story story, int completions)
    {
        var baseReward = story.BaseCreatorReward;
        var qualityMultiplier = GetQualityMultiplier(story.QualityScore.OverallScore);
        var popularityBonus = CalculatePopularityBonus(completions);
        
        return new Reward
        {
            Currency = baseReward × qualityMultiplier + popularityBonus,
            Experience = (int)(baseReward × qualityMultiplier × 0.5f),
            ReputationPoints = GetReputationReward(story.QualityScore.OverallScore)
        };
    }
    
    private float GetQualityMultiplier(float qualityScore)
    {
        if (qualityScore >= 85) return 1.5f;  // Exceptional
        if (qualityScore >= 70) return 1.2f;  // High quality
        if (qualityScore >= 55) return 1.0f;  // Acceptable
        return 0.5f; // Below standard (reduced rewards)
    }
    
    private int CalculatePopularityBonus(int completions)
    {
        // Reward popular content creators
        if (completions >= 1000) return 500;
        if (completions >= 500) return 250;
        if (completions >= 100) return 100;
        if (completions >= 50) return 50;
        return 0;
    }
    
    private int GetReputationReward(float qualityScore)
    {
        if (qualityScore >= 85) return 15;
        if (qualityScore >= 70) return 10;
        if (qualityScore >= 55) return 5;
        return 0;
    }
}
```

---

## Testing and Validation

### Unit Testing Requirements

**Quality Scoring Tests:**
```csharp
[TestClass]
public class StoryQualityScorerTests
{
    [TestMethod]
    public void ExceptionalStory_ShouldScore85Plus()
    {
        var story = CreateExceptionalStory();
        var scorer = new StoryQualityScorer();
        
        var result = scorer.CalculateQualityScore(story);
        
        Assert.IsTrue(result.OverallScore >= 85);
        Assert.AreEqual(QualityTier.Exceptional, result.Tier);
    }
    
    [TestMethod]
    public void IncompletableQuest_ShouldFailTechnicalValidation()
    {
        var story = CreateIncompletableStory();
        var scorer = new StoryQualityScorer();
        
        var result = scorer.RunAutomatedChecks(story);
        
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.Contains("not completable")));
    }
    
    [TestMethod]
    public void QualityScore_ShouldBeWeightedCorrectly()
    {
        var story = CreateTestStory(
            narrative: 80,
            technical: 90,
            geological: 70,
            experience: 75,
            community: 65
        );
        var scorer = new StoryQualityScorer();
        
        var result = scorer.CalculateQualityScore(story);
        
        var expected = (80 × 0.25) + (90 × 0.30) + (70 × 0.20) + 
                      (75 × 0.15) + (65 × 0.10);
        Assert.AreEqual(expected, result.OverallScore, 0.1);
    }
}
```

### Integration Testing

**End-to-End Quality Assessment:**
```csharp
[TestClass]
public class QualityAssessmentIntegrationTests
{
    [TestMethod]
    public async Task NewStory_CompletesFullAssessmentPipeline()
    {
        var story = CreateNewStory();
        var service = new QualityAssessmentService();
        
        // Submit for assessment
        var assessmentId = await service.SubmitForAssessment(story);
        
        // Wait for automated checks
        var result = await service.WaitForAssessment(assessmentId);
        
        // Verify results
        Assert.IsNotNull(result.QualityScore);
        Assert.IsTrue(result.Status != ApprovalStatus.PendingReview);
        Assert.IsNotNull(result.FeedbackReport);
    }
}
```

---

## Metrics and Monitoring

### Key Performance Indicators (KPIs)

**System Health:**
- Assessment processing time: <5 seconds for automated checks
- Manual review queue depth: <100 pending
- False positive rate: <5% (inappropriate rejections)
- False negative rate: <2% (inappropriate approvals)

**Content Quality:**
- Average quality score: Target >70
- Exceptional content rate: Target >10%
- Rejection rate: Target <15%
- Resubmission success rate: Target >60%

**Community Engagement:**
- Average player rating: Target >4.0 stars
- Completion rate: Target >65%
- Creator satisfaction: Target >75%
- Report resolution time: <48 hours

### Monitoring Dashboard

```
Quality Assessment Dashboard:

Submissions (Last 30 Days):
├── Total Submissions: 1,247
├── Approved: 956 (76.7%)
├── Rejected: 187 (15.0%)
├── Pending Review: 104 (8.3%)
└── Average Processing Time: 3.2 seconds

Quality Distribution:
├── Exceptional (85-100): 142 (14.9%)
├── High Quality (70-84): 423 (44.2%)
├── Acceptable (55-69): 391 (40.9%)
└── Below Standard (<55): 291 (-)

Community Metrics:
├── Average Rating: 4.2 stars
├── Completion Rate: 68.5%
├── Active Reports: 23
└── Creator Satisfaction: 78%
```

---

## Future Enhancements

### Planned Features

**Version 2.0 (Q2 2026)**
- Machine learning-based narrative quality assessment
- Advanced plagiarism detection
- Automated geological fact-checking with knowledge base
- Real-time collaborative editing for creators
- A/B testing framework for quest variations

**Version 3.0 (Q4 2026)**
- Community-driven review system expansion
- Creator mentorship program integration
- Advanced analytics for creators
- Quality prediction before submission
- Multi-language support

### Research Areas

1. **AI-Assisted Quality Assessment**
   - Natural language processing for narrative analysis
   - Automated story arc detection
   - Character consistency checking

2. **Adaptive Difficulty Scoring**
   - Player skill level matching
   - Dynamic difficulty adjustment
   - Personalized recommendations

3. **Educational Impact Measurement**
   - Learning outcome assessment
   - Knowledge retention tracking
   - Engagement with geological concepts

---

## Appendices

### Appendix A: Detailed Scoring Examples

**Example 1: Exceptional Quality Quest (Score: 89)**
```
Title: "The Iron Vein Mystery"

Narrative Quality: 85/100
- Grammar: 24/25 (one minor typo)
- Coherence: 23/25 (strong logical flow)
- Engagement: 22/25 (compelling mystery)
- Character: 20/25 (good NPC development)

Technical Quality: 92/100
- Completability: 30/30 (fully tested)
- Performance: 28/30 (minimal impact)
- Integration: 34/40 (minor balance adjustment needed)

Geological Accuracy: 88/100
- Scientific Accuracy: 38/40 (excellent)
- Educational Value: 27/30 (teaches ore formation)
- Resource Authenticity: 23/30 (minor location issue)

Player Experience: 86/100
- Investment/Reward: 23/25 (well balanced)
- Clarity: 22/25 (clear objectives)
- Fun Factor: 21/25 (engaging gameplay)
- Accessibility: 20/25 (slightly difficult for beginners)

Community Validation: 91/100
- Completion Rate: 24/25 (82% completion)
- Player Rating: 23/25 (4.6 stars)
- Reports: 25/25 (no issues)
- Creator Reputation: 19/25 (established creator)

Overall Score: 89.0
Tier: Exceptional
Status: Approved - Featured Content
```

**Example 2: Needs Improvement Quest (Score: 47)**
```
Title: "Get Some Rocks"

Narrative Quality: 35/100
- Grammar: 12/25 (multiple spelling errors)
- Coherence: 10/25 (unclear story)
- Engagement: 8/25 (generic, boring)
- Character: 5/25 (no character development)

Technical Quality: 58/100
- Completability: 24/30 (works but has issues)
- Performance: 22/30 (slightly slow)
- Integration: 12/40 (reward balance off)

Geological Accuracy: 42/100
- Scientific Accuracy: 15/40 (inaccurate terms)
- Educational Value: 12/30 (minimal learning)
- Resource Authenticity: 15/30 (wrong formation type)

Player Experience: 51/100
- Investment/Reward: 10/25 (poor balance)
- Clarity: 15/25 (vague objectives)
- Fun Factor: 12/25 (repetitive)
- Accessibility: 14/25 (no guidance)

Community Validation: N/A
- (Not yet published)

Overall Score: 47.0
Tier: Needs Improvement
Status: Rejected - Revisions Required

Feedback:
- Fix spelling and grammar errors
- Add clearer objectives
- Research correct geological terminology
- Improve reward balance
- Add more engaging story elements
```

### Appendix B: Content Guidelines Reference

**Approved Content:**
✅ Educational geological concepts
✅ Exploration and discovery themes
✅ Resource gathering and processing
✅ Scientific investigation stories
✅ Historical geology references
✅ Terraforming and engineering challenges
✅ Cooperative team objectives
✅ Environmental stewardship themes

**Prohibited Content:**
❌ Violence, gore, or mature themes
❌ Offensive or discriminatory content
❌ Real-world political commentary
❌ Sexually suggestive material
❌ Copyrighted content without permission
❌ Personal information or harassment
❌ Exploitative or manipulative mechanics
❌ Misleading educational information

### Appendix C: Glossary

**Quality Score**: Numerical assessment (0-100) of content quality across multiple dimensions.

**Approval Status**: Current state of content in the review process (pending, approved, rejected, etc.).

**Quality Tier**: Categorical assessment based on overall score (Exceptional, High Quality, etc.).

**Completability**: Technical measure of whether a quest can be successfully finished.

**Geological Accuracy**: Measure of scientific correctness and educational value.

**Community Validation**: Aggregate of player feedback, ratings, and engagement metrics.

**Creator Reputation**: Player's standing as a content creator based on historical quality.

**Featured Content**: Highest quality content highlighted for maximum visibility.

---

## References

### Related Documentation

- [Content Design Workflow](../../step-1-foundation/content-design/content-design-workflow.md)
- [Trust and Player-Created Quests](../../../topics/trust-player-created-quests-reputation-systems.md)
- [Community Content and UGC Quest Tools](../../../topics/community-content-ugc-quest-tools-sandbox.md)
- [Content Velocity and Quest Generation](../../../topics/content-velocity-quest-generation-sandbox-mmorpg.md)
- [Quality Assurance Framework](../../../../docs/QA_FRAMEWORK.md)
- [Content Rating Guidelines](../../../../docs/gameplay/content-rating-guidelines.md)

### External Resources

- Game User Research Guidelines (GUR)
- Natural Language Processing for Content Analysis
- Automated Testing Best Practices
- Community Moderation Frameworks
- Educational Content Standards (NGSS)

---

## Version History

| Version | Date       | Author    | Changes |
|---------|------------|-----------|---------|
| 1.0     | 2025-01-24 | @copilot  | Initial framework creation |

---

**Document Owner:** Game Design Team  
**Last Updated:** 2025-01-24  
**Next Review:** 2025-04-24  
**Status:** Active
