# Story Quality Scorer Implementation Guide

---
title: Implementation Guide for Story Quality Scoring System
date: 2025-01-24
owner: @copilot
status: active
priority: P1
tags: [implementation, quality-assessment, technical-guide, api-design, testing]
related-docs: story-quality-assessment-framework.md
---

## Overview

This guide provides practical implementation instructions for developers integrating the Story Quality Assessment and Scoring Framework into BlueMarble. It covers API design, service architecture, testing strategies, and deployment considerations.

**Prerequisites:**
- Review [Story Quality Assessment Framework](story-quality-assessment-framework.md)
- Familiarity with C# and .NET
- Understanding of async/await patterns
- Database knowledge (SQL, Redis)

---

## Architecture Overview

### Service Layer Structure

```
QualityAssessmentService (Main Entry Point)
    ├── NarrativeQualityAssessor
    │   ├── GrammarChecker
    │   ├── CoherenceAnalyzer
    │   └── EngagementScorer
    ├── TechnicalQualityAssessor
    │   ├── CompletabilityValidator
    │   ├── PerformanceProfiler
    │   └── IntegrationChecker
    ├── GeologicalQualityAssessor
    │   ├── TerminologyValidator
    │   ├── ProcessValidator
    │   └── MaterialAuthenticityChecker
    ├── ExperienceQualityAssessor
    │   ├── RewardBalanceCalculator
    │   ├── ClarityAnalyzer
    │   └── AccessibilityChecker
    └── CommunityMetricsCollector
        ├── CompletionRateTracker
        ├── RatingAggregator
        └── ReportMonitor
```

---

## Step 1: Core Data Models

### Story Model

```csharp
/// <summary>
/// Represents a player-created story/quest that can be assessed for quality
/// </summary>
public class Story
{
    public long Id { get; set; }
    public long CreatorId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string FullText { get; set; }
    
    // Objectives and requirements
    public List<QuestObjective> Objectives { get; set; }
    public List<ItemRequirement> ItemRequirements { get; set; }
    public List<LocationRequirement> LocationRequirements { get; set; }
    
    // Metadata
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public StoryStatus Status { get; set; }
    
    // Quality assessment
    public QualityScore CurrentQualityScore { get; set; }
    
    // Community metrics
    public int TimesCompleted { get; set; }
    public int TimesAbandoned { get; set; }
    public List<CommunityRating> Ratings { get; set; }
}

public class QuestObjective
{
    public string Description { get; set; }
    public ObjectiveType Type { get; set; }
    public Dictionary<string, object> Parameters { get; set; }
    public bool IsRequired { get; set; }
}

public enum ObjectiveType
{
    CollectItems,
    VisitLocation,
    TalkToNPC,
    CraftItem,
    PerformAction
}

public enum StoryStatus
{
    Draft,
    AwaitingReview,
    Approved,
    Rejected,
    Published,
    Archived
}
```

### Quality Score Model

```csharp
/// <summary>
/// Comprehensive quality assessment result
/// </summary>
public class QualityScore
{
    public long Id { get; set; }
    public long StoryId { get; set; }
    
    // Overall score
    public float OverallScore { get; set; } // 0-100
    
    // Dimension scores
    public float NarrativeScore { get; set; } // 0-100
    public float TechnicalScore { get; set; } // 0-100
    public float GeologicalScore { get; set; } // 0-100
    public float ExperienceScore { get; set; } // 0-100
    public float CommunityScore { get; set; } // 0-100
    
    // Detailed breakdown
    public NarrativeScoreDetails NarrativeDetails { get; set; }
    public TechnicalScoreDetails TechnicalDetails { get; set; }
    public GeologicalScoreDetails GeologicalDetails { get; set; }
    public ExperienceScoreDetails ExperienceDetails { get; set; }
    public CommunityScoreDetails CommunityDetails { get; set; }
    
    // Classification
    public QualityTier Tier { get; set; }
    public ApprovalStatus Status { get; set; }
    
    // Metadata
    public DateTime AssessedAt { get; set; }
    public string AssessorType { get; set; } // "automated", "community", "expert"
    public int AssessmentVersion { get; set; } // Track scoring algorithm version
    
    // Feedback
    public List<string> Strengths { get; set; }
    public List<string> ImprovementAreas { get; set; }
    public List<ValidationError> Errors { get; set; }
}

public class NarrativeScoreDetails
{
    public float GrammarScore { get; set; } // 0-25
    public float CoherenceScore { get; set; } // 0-25
    public float EngagementScore { get; set; } // 0-25
    public float CharacterScore { get; set; } // 0-25
    
    public int SpellingErrors { get; set; }
    public int GrammarErrors { get; set; }
    public float ReadabilityScore { get; set; }
    public int WordCount { get; set; }
}

public class TechnicalScoreDetails
{
    public float CompletabilityScore { get; set; } // 0-30
    public float PerformanceScore { get; set; } // 0-30
    public float IntegrationScore { get; set; } // 0-40
    
    public bool IsCompletable { get; set; }
    public float AverageProcessingTimeMs { get; set; }
    public List<string> IntegrationIssues { get; set; }
}

public class GeologicalScoreDetails
{
    public float AccuracyScore { get; set; } // 0-40
    public float EducationalScore { get; set; } // 0-30
    public float AuthenticityScore { get; set; } // 0-30
    
    public List<string> TerminologyIssues { get; set; }
    public List<string> ProcessInaccuracies { get; set; }
}

public class ExperienceScoreDetails
{
    public float InvestmentRewardScore { get; set; } // 0-25
    public float ClarityScore { get; set; } // 0-25
    public float FunScore { get; set; } // 0-25
    public float AccessibilityScore { get; set; } // 0-25
    
    public float EstimatedCompletionTimeMinutes { get; set; }
    public float RewardValueGold { get; set; }
}

public class CommunityScoreDetails
{
    public float CompletionRateScore { get; set; } // 0-25
    public float RatingScore { get; set; } // 0-25
    public float ReportScore { get; set; } // 0-25
    public float CreatorReputationScore { get; set; } // 0-25
    
    public float CompletionRate { get; set; }
    public float AverageRating { get; set; }
    public int TotalRatings { get; set; }
    public int BugReports { get; set; }
    public int ExploitReports { get; set; }
}

public class ValidationError
{
    public string Code { get; set; }
    public ErrorSeverity Severity { get; set; }
    public string Message { get; set; }
    public string Field { get; set; }
    public string Suggestion { get; set; }
}

public enum ErrorSeverity
{
    Info,
    Warning,
    Error,
    Critical
}
```

---

## Step 2: Core Service Implementation

### Main Quality Assessment Service

```csharp
public interface IQualityAssessmentService
{
    Task<QualityScore> AssessStoryAsync(long storyId, CancellationToken cancellationToken = default);
    Task<ValidationResult> ValidateStoryAsync(long storyId, CancellationToken cancellationToken = default);
    Task<QualityScore> GetLatestScoreAsync(long storyId, CancellationToken cancellationToken = default);
    Task<List<QualityScore>> GetScoreHistoryAsync(long storyId, CancellationToken cancellationToken = default);
}

public class QualityAssessmentService : IQualityAssessmentService
{
    private readonly IStoryRepository _storyRepository;
    private readonly IQualityScoreRepository _qualityScoreRepository;
    private readonly INarrativeQualityAssessor _narrativeAssessor;
    private readonly ITechnicalQualityAssessor _technicalAssessor;
    private readonly IGeologicalQualityAssessor _geologicalAssessor;
    private readonly IExperienceQualityAssessor _experienceAssessor;
    private readonly ICommunityMetricsCollector _communityMetrics;
    private readonly ILogger<QualityAssessmentService> _logger;
    
    public QualityAssessmentService(
        IStoryRepository storyRepository,
        IQualityScoreRepository qualityScoreRepository,
        INarrativeQualityAssessor narrativeAssessor,
        ITechnicalQualityAssessor technicalAssessor,
        IGeologicalQualityAssessor geologicalAssessor,
        IExperienceQualityAssessor experienceAssessor,
        ICommunityMetricsCollector communityMetrics,
        ILogger<QualityAssessmentService> logger)
    {
        _storyRepository = storyRepository;
        _qualityScoreRepository = qualityScoreRepository;
        _narrativeAssessor = narrativeAssessor;
        _technicalAssessor = technicalAssessor;
        _geologicalAssessor = geologicalAssessor;
        _experienceAssessor = experienceAssessor;
        _communityMetrics = communityMetrics;
        _logger = logger;
    }
    
    public async Task<QualityScore> AssessStoryAsync(
        long storyId, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting quality assessment for story {StoryId}", storyId);
        
        var story = await _storyRepository.GetByIdAsync(storyId, cancellationToken);
        if (story == null)
        {
            throw new NotFoundException($"Story {storyId} not found");
        }
        
        // Run all assessments in parallel for performance
        var assessmentTasks = new[]
        {
            _narrativeAssessor.AssessAsync(story, cancellationToken),
            _technicalAssessor.AssessAsync(story, cancellationToken),
            _geologicalAssessor.AssessAsync(story, cancellationToken),
            _experienceAssessor.AssessAsync(story, cancellationToken),
            _communityMetrics.CollectAsync(story, cancellationToken)
        };
        
        var results = await Task.WhenAll(assessmentTasks);
        
        var narrativeScore = results[0];
        var technicalScore = results[1];
        var geologicalScore = results[2];
        var experienceScore = results[3];
        var communityScore = results[4];
        
        // Calculate overall score using weighted average
        var overallScore = CalculateOverallScore(
            narrativeScore, 
            technicalScore, 
            geologicalScore, 
            experienceScore, 
            communityScore);
        
        var qualityScore = new QualityScore
        {
            StoryId = storyId,
            OverallScore = overallScore,
            NarrativeScore = narrativeScore,
            TechnicalScore = technicalScore,
            GeologicalScore = geologicalScore,
            ExperienceScore = experienceScore,
            CommunityScore = communityScore,
            Tier = DetermineQualityTier(overallScore),
            Status = DetermineApprovalStatus(overallScore, technicalScore),
            AssessedAt = DateTime.UtcNow,
            AssessorType = "automated",
            AssessmentVersion = 1
        };
        
        // Generate feedback
        qualityScore.Strengths = GenerateStrengths(qualityScore);
        qualityScore.ImprovementAreas = GenerateImprovements(qualityScore);
        
        // Save to database
        await _qualityScoreRepository.AddAsync(qualityScore, cancellationToken);
        
        _logger.LogInformation(
            "Completed quality assessment for story {StoryId}. Score: {Score}, Tier: {Tier}",
            storyId, overallScore, qualityScore.Tier);
        
        return qualityScore;
    }
    
    private float CalculateOverallScore(
        float narrative, 
        float technical, 
        float geological, 
        float experience, 
        float community)
    {
        // Weights from framework: N:25%, T:30%, G:20%, E:15%, C:10%
        return (narrative * 0.25f) + 
               (technical * 0.30f) + 
               (geological * 0.20f) + 
               (experience * 0.15f) + 
               (community * 0.10f);
    }
    
    private QualityTier DetermineQualityTier(float score)
    {
        if (score >= 85) return QualityTier.Exceptional;
        if (score >= 70) return QualityTier.HighQuality;
        if (score >= 55) return QualityTier.Acceptable;
        if (score >= 40) return QualityTier.NeedsImprovement;
        return QualityTier.Rejected;
    }
    
    private ApprovalStatus DetermineApprovalStatus(float overallScore, float technicalScore)
    {
        // Critical technical failures override everything
        if (technicalScore < 30)
            return ApprovalStatus.RejectedTechnical;
        
        if (overallScore < 40)
            return ApprovalStatus.RejectedQuality;
        
        if (overallScore < 55)
            return ApprovalStatus.ApprovedLimited;
        
        return ApprovalStatus.Approved;
    }
    
    private List<string> GenerateStrengths(QualityScore score)
    {
        var strengths = new List<string>();
        
        if (score.NarrativeScore >= 80)
            strengths.Add("Excellent narrative quality with clear, engaging storytelling");
        if (score.TechnicalScore >= 85)
            strengths.Add("Technically sound implementation with no issues detected");
        if (score.GeologicalScore >= 75)
            strengths.Add("Strong geological accuracy and educational value");
        if (score.ExperienceScore >= 75)
            strengths.Add("Well-balanced player experience with clear objectives");
        if (score.CommunityScore >= 70)
            strengths.Add("Positive community reception and high completion rate");
        
        return strengths;
    }
    
    private List<string> GenerateImprovements(QualityScore score)
    {
        var improvements = new List<string>();
        
        if (score.NarrativeScore < 60)
            improvements.Add("Improve narrative quality: check grammar, enhance story coherence");
        if (score.TechnicalScore < 60)
            improvements.Add("Address technical issues: verify completability and performance");
        if (score.GeologicalScore < 60)
            improvements.Add("Enhance geological accuracy: verify terminology and processes");
        if (score.ExperienceScore < 60)
            improvements.Add("Improve player experience: balance rewards, clarify objectives");
        if (score.CommunityScore < 50)
            improvements.Add("Consider community feedback and work on completion rate");
        
        return improvements;
    }
}
```

---

## Step 3: Dimension Assessors

### Narrative Quality Assessor

```csharp
public interface INarrativeQualityAssessor
{
    Task<float> AssessAsync(Story story, CancellationToken cancellationToken = default);
    Task<NarrativeScoreDetails> GetDetailsAsync(Story story, CancellationToken cancellationToken = default);
}

public class NarrativeQualityAssessor : INarrativeQualityAssessor
{
    private readonly IGrammarChecker _grammarChecker;
    private readonly ICoherenceAnalyzer _coherenceAnalyzer;
    private readonly IEngagementScorer _engagementScorer;
    
    public async Task<float> AssessAsync(Story story, CancellationToken cancellationToken)
    {
        var details = await GetDetailsAsync(story, cancellationToken);
        
        // Average of all components
        return (details.GrammarScore + 
                details.CoherenceScore + 
                details.EngagementScore + 
                details.CharacterScore) / 4.0f;
    }
    
    public async Task<NarrativeScoreDetails> GetDetailsAsync(
        Story story, 
        CancellationToken cancellationToken)
    {
        var fullText = $"{story.Title}\n{story.Description}\n{story.FullText}";
        
        // Run checks in parallel
        var grammarTask = _grammarChecker.CheckAsync(fullText, cancellationToken);
        var coherenceTask = _coherenceAnalyzer.AnalyzeAsync(story, cancellationToken);
        var engagementTask = _engagementScorer.ScoreAsync(story, cancellationToken);
        
        await Task.WhenAll(grammarTask, coherenceTask, engagementTask);
        
        var grammarResult = await grammarTask;
        var coherenceScore = await coherenceTask;
        var engagementScore = await engagementTask;
        
        return new NarrativeScoreDetails
        {
            GrammarScore = CalculateGrammarScore(grammarResult),
            CoherenceScore = coherenceScore,
            EngagementScore = engagementScore,
            CharacterScore = CalculateCharacterScore(story),
            SpellingErrors = grammarResult.SpellingErrors.Count,
            GrammarErrors = grammarResult.GrammarErrors.Count,
            ReadabilityScore = grammarResult.ReadabilityScore,
            WordCount = grammarResult.WordCount
        };
    }
    
    private float CalculateGrammarScore(GrammarCheckResult result)
    {
        // 25 points maximum
        var errorsPerHundredWords = 
            (result.SpellingErrors.Count + result.GrammarErrors.Count) * 100.0f / 
            Math.Max(result.WordCount, 1);
        
        if (errorsPerHundredWords < 1) return 25f;
        if (errorsPerHundredWords < 2) return 22f;
        if (errorsPerHundredWords < 5) return 18f;
        if (errorsPerHundredWords < 10) return 12f;
        return 8f;
    }
    
    private float CalculateCharacterScore(Story story)
    {
        // Simplified character development scoring
        // In production, this would use NLP for character analysis
        
        var hasCharacters = !string.IsNullOrEmpty(story.FullText) && 
                           story.FullText.Contains("NPC");
        var hasDialogue = story.FullText?.Contains("\"") ?? false;
        var hasMeaningfulInteraction = story.Objectives?.Any(o => 
            o.Type == ObjectiveType.TalkToNPC) ?? false;
        
        float score = 10f; // Base score
        if (hasCharacters) score += 5f;
        if (hasDialogue) score += 5f;
        if (hasMeaningfulInteraction) score += 5f;
        
        return Math.Min(score, 25f);
    }
}
```

### Technical Quality Assessor

```csharp
public interface ITechnicalQualityAssessor
{
    Task<float> AssessAsync(Story story, CancellationToken cancellationToken = default);
    Task<TechnicalScoreDetails> GetDetailsAsync(Story story, CancellationToken cancellationToken = default);
}

public class TechnicalQualityAssessor : ITechnicalQualityAssessor
{
    private readonly ICompletabilityValidator _completabilityValidator;
    private readonly IPerformanceProfiler _performanceProfiler;
    private readonly IIntegrationChecker _integrationChecker;
    private readonly ILogger<TechnicalQualityAssessor> _logger;
    
    public async Task<float> AssessAsync(Story story, CancellationToken cancellationToken)
    {
        var details = await GetDetailsAsync(story, cancellationToken);
        
        // Weighted average: Completability: 30%, Performance: 30%, Integration: 40%
        return (details.CompletabilityScore * 0.3f) + 
               (details.PerformanceScore * 0.3f) + 
               (details.IntegrationScore * 0.4f);
    }
    
    public async Task<TechnicalScoreDetails> GetDetailsAsync(
        Story story, 
        CancellationToken cancellationToken)
    {
        try
        {
            // Run technical checks
            var completabilityTask = _completabilityValidator.ValidateAsync(story, cancellationToken);
            var performanceTask = _performanceProfiler.ProfileAsync(story, cancellationToken);
            var integrationTask = _integrationChecker.CheckAsync(story, cancellationToken);
            
            await Task.WhenAll(completabilityTask, performanceTask, integrationTask);
            
            var completabilityResult = await completabilityTask;
            var performanceResult = await performanceTask;
            var integrationResult = await integrationTask;
            
            return new TechnicalScoreDetails
            {
                CompletabilityScore = CalculateCompletabilityScore(completabilityResult),
                PerformanceScore = CalculatePerformanceScore(performanceResult),
                IntegrationScore = CalculateIntegrationScore(integrationResult),
                IsCompletable = completabilityResult.IsCompletable,
                AverageProcessingTimeMs = performanceResult.AverageProcessingTimeMs,
                IntegrationIssues = integrationResult.Issues
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during technical assessment of story {StoryId}", story.Id);
            
            // Return minimal score on error
            return new TechnicalScoreDetails
            {
                CompletabilityScore = 0,
                PerformanceScore = 0,
                IntegrationScore = 0,
                IsCompletable = false,
                IntegrationIssues = new List<string> { "Assessment failed: " + ex.Message }
            };
        }
    }
    
    private float CalculateCompletabilityScore(CompletabilityResult result)
    {
        if (!result.IsCompletable) return 0f;
        if (result.MinorIssues.Count == 0) return 30f;
        if (result.MinorIssues.Count <= 1) return 24f;
        if (result.MinorIssues.Count <= 2) return 18f;
        return 12f;
    }
    
    private float CalculatePerformanceScore(PerformanceResult result)
    {
        var avgTime = result.AverageProcessingTimeMs;
        
        if (avgTime < 1) return 30f;
        if (avgTime < 5) return 24f;
        if (avgTime < 10) return 18f;
        if (avgTime < 50) return 12f;
        return 0f; // Unacceptable performance
    }
    
    private float CalculateIntegrationScore(IntegrationResult result)
    {
        var issueCount = result.Issues.Count;
        var criticalIssues = result.Issues.Count(i => i.Contains("CRITICAL"));
        
        if (criticalIssues > 0) return 0f;
        if (issueCount == 0) return 40f;
        if (issueCount <= 2) return 32f;
        if (issueCount <= 4) return 24f;
        return 16f;
    }
}
```

---

## Step 4: Validation Pipeline

### Pre-Publication Validation

```csharp
public interface IValidationPipeline
{
    Task<ValidationResult> RunAsync(Story story, CancellationToken cancellationToken = default);
}

public class ValidationPipeline : IValidationPipeline
{
    private readonly List<IValidator> _validators;
    private readonly ILogger<ValidationPipeline> _logger;
    
    public ValidationPipeline(
        IEnumerable<IValidator> validators,
        ILogger<ValidationPipeline> logger)
    {
        _validators = validators.OrderBy(v => v.Priority).ToList();
        _logger = logger;
    }
    
    public async Task<ValidationResult> RunAsync(
        Story story, 
        CancellationToken cancellationToken)
    {
        var result = new ValidationResult
        {
            StoryId = story.Id,
            IsValid = true,
            Errors = new List<ValidationError>(),
            Warnings = new List<ValidationError>()
        };
        
        foreach (var validator in _validators)
        {
            try
            {
                var validationErrors = await validator.ValidateAsync(story, cancellationToken);
                
                result.Errors.AddRange(validationErrors.Where(e => 
                    e.Severity == ErrorSeverity.Critical || 
                    e.Severity == ErrorSeverity.Error));
                
                result.Warnings.AddRange(validationErrors.Where(e => 
                    e.Severity == ErrorSeverity.Warning));
                
                // Stop on critical errors
                if (result.Errors.Any(e => e.Severity == ErrorSeverity.Critical))
                {
                    result.IsValid = false;
                    _logger.LogWarning(
                        "Critical validation errors found for story {StoryId}. Stopping pipeline.",
                        story.Id);
                    break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Validator {ValidatorType} failed for story {StoryId}", 
                    validator.GetType().Name, 
                    story.Id);
                
                result.Errors.Add(new ValidationError
                {
                    Code = "VALIDATOR_EXCEPTION",
                    Severity = ErrorSeverity.Critical,
                    Message = $"Validation failed: {ex.Message}",
                    Field = validator.GetType().Name
                });
                
                result.IsValid = false;
                break;
            }
        }
        
        result.IsValid = result.Errors.Count == 0;
        return result;
    }
}

public interface IValidator
{
    int Priority { get; } // Lower = run first
    Task<List<ValidationError>> ValidateAsync(Story story, CancellationToken cancellationToken);
}

// Example validators
public class CompletabilityValidator : IValidator
{
    public int Priority => 1; // Run first
    
    public async Task<List<ValidationError>> ValidateAsync(
        Story story, 
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();
        
        // Check if all objectives are valid
        foreach (var objective in story.Objectives ?? Enumerable.Empty<QuestObjective>())
        {
            if (objective.Type == ObjectiveType.CollectItems)
            {
                var itemId = objective.Parameters.GetValueOrDefault("itemId") as string;
                if (string.IsNullOrEmpty(itemId))
                {
                    errors.Add(new ValidationError
                    {
                        Code = "INVALID_OBJECTIVE",
                        Severity = ErrorSeverity.Critical,
                        Message = "Collect items objective missing itemId parameter",
                        Field = "Objectives",
                        Suggestion = "Add itemId parameter to collection objective"
                    });
                }
            }
        }
        
        return errors;
    }
}

public class OffensiveContentValidator : IValidator
{
    public int Priority => 2;
    
    private readonly IContentModerationService _moderationService;
    
    public async Task<List<ValidationError>> ValidateAsync(
        Story story, 
        CancellationToken cancellationToken)
    {
        var errors = new List<ValidationError>();
        
        var fullText = $"{story.Title} {story.Description} {story.FullText}";
        var moderationResult = await _moderationService.CheckAsync(fullText, cancellationToken);
        
        if (moderationResult.HasOffensiveContent)
        {
            errors.Add(new ValidationError
            {
                Code = "OFFENSIVE_CONTENT",
                Severity = ErrorSeverity.Critical,
                Message = "Story contains inappropriate or offensive content",
                Field = "Content",
                Suggestion = "Remove offensive language and content"
            });
        }
        
        return errors;
    }
}
```

---

## Step 5: API Endpoints

### REST API Design

```csharp
[ApiController]
[Route("api/v1/stories/{storyId}/quality")]
public class StoryQualityController : ControllerBase
{
    private readonly IQualityAssessmentService _qualityService;
    private readonly ILogger<StoryQualityController> _logger;
    
    [HttpPost("assess")]
    [Authorize]
    public async Task<ActionResult<QualityScoreResponse>> AssessStory(
        [FromRoute] long storyId,
        CancellationToken cancellationToken)
    {
        try
        {
            var score = await _qualityService.AssessStoryAsync(storyId, cancellationToken);
            return Ok(MapToResponse(score));
        }
        catch (NotFoundException)
        {
            return NotFound(new { error = $"Story {storyId} not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assessing story {StoryId}", storyId);
            return StatusCode(500, new { error = "Quality assessment failed" });
        }
    }
    
    [HttpGet("latest")]
    public async Task<ActionResult<QualityScoreResponse>> GetLatestScore(
        [FromRoute] long storyId,
        CancellationToken cancellationToken)
    {
        var score = await _qualityService.GetLatestScoreAsync(storyId, cancellationToken);
        if (score == null)
        {
            return NotFound(new { error = "No quality score found for this story" });
        }
        
        return Ok(MapToResponse(score));
    }
    
    [HttpGet("history")]
    public async Task<ActionResult<List<QualityScoreResponse>>> GetScoreHistory(
        [FromRoute] long storyId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken)
    {
        var scores = await _qualityService.GetScoreHistoryAsync(storyId, cancellationToken);
        var pagedScores = scores
            .OrderByDescending(s => s.AssessedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        return Ok(pagedScores.Select(MapToResponse));
    }
    
    [HttpPost("validate")]
    [Authorize]
    public async Task<ActionResult<ValidationResponse>> ValidateStory(
        [FromRoute] long storyId,
        CancellationToken cancellationToken)
    {
        var result = await _qualityService.ValidateStoryAsync(storyId, cancellationToken);
        return Ok(new ValidationResponse
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(e => new ErrorResponse
            {
                Code = e.Code,
                Severity = e.Severity.ToString(),
                Message = e.Message,
                Field = e.Field,
                Suggestion = e.Suggestion
            }).ToList()
        });
    }
    
    private QualityScoreResponse MapToResponse(QualityScore score)
    {
        return new QualityScoreResponse
        {
            OverallScore = score.OverallScore,
            Tier = score.Tier.ToString(),
            Status = score.Status.ToString(),
            Breakdown = new ScoreBreakdown
            {
                Narrative = score.NarrativeScore,
                Technical = score.TechnicalScore,
                Geological = score.GeologicalScore,
                Experience = score.ExperienceScore,
                Community = score.CommunityScore
            },
            Feedback = new QualityFeedback
            {
                Strengths = score.Strengths,
                ImprovementAreas = score.ImprovementAreas
            },
            AssessedAt = score.AssessedAt
        };
    }
}

// Response DTOs
public class QualityScoreResponse
{
    public float OverallScore { get; set; }
    public string Tier { get; set; }
    public string Status { get; set; }
    public ScoreBreakdown Breakdown { get; set; }
    public QualityFeedback Feedback { get; set; }
    public DateTime AssessedAt { get; set; }
}

public class ScoreBreakdown
{
    public float Narrative { get; set; }
    public float Technical { get; set; }
    public float Geological { get; set; }
    public float Experience { get; set; }
    public float Community { get; set; }
}

public class QualityFeedback
{
    public List<string> Strengths { get; set; }
    public List<string> ImprovementAreas { get; set; }
}

public class ValidationResponse
{
    public bool IsValid { get; set; }
    public List<ErrorResponse> Errors { get; set; }
}

public class ErrorResponse
{
    public string Code { get; set; }
    public string Severity { get; set; }
    public string Message { get; set; }
    public string Field { get; set; }
    public string Suggestion { get; set; }
}
```

---

## Step 6: Testing Strategy

### Unit Tests

```csharp
[TestClass]
public class QualityAssessmentServiceTests
{
    private Mock<IStoryRepository> _mockStoryRepo;
    private Mock<INarrativeQualityAssessor> _mockNarrativeAssessor;
    private Mock<ITechnicalQualityAssessor> _mockTechnicalAssessor;
    private QualityAssessmentService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _mockStoryRepo = new Mock<IStoryRepository>();
        _mockNarrativeAssessor = new Mock<INarrativeQualityAssessor>();
        _mockTechnicalAssessor = new Mock<ITechnicalQualityAssessor>();
        // ... setup other mocks
        
        _service = new QualityAssessmentService(
            _mockStoryRepo.Object,
            // ... other dependencies
        );
    }
    
    [TestMethod]
    public async Task AssessStory_WithExceptionalContent_ReturnsExceptionalTier()
    {
        // Arrange
        var story = CreateTestStory();
        _mockStoryRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(story);
        
        _mockNarrativeAssessor.Setup(a => a.AssessAsync(story, It.IsAny<CancellationToken>()))
            .ReturnsAsync(90f);
        _mockTechnicalAssessor.Setup(a => a.AssessAsync(story, It.IsAny<CancellationToken>()))
            .ReturnsAsync(95f);
        // ... setup other assessors to return high scores
        
        // Act
        var result = await _service.AssessStoryAsync(story.Id);
        
        // Assert
        Assert.IsTrue(result.OverallScore >= 85);
        Assert.AreEqual(QualityTier.Exceptional, result.Tier);
        Assert.AreEqual(ApprovalStatus.Approved, result.Status);
    }
    
    [TestMethod]
    public async Task AssessStory_WithLowTechnicalScore_ReturnsRejectedTechnical()
    {
        // Arrange
        var story = CreateTestStory();
        _mockStoryRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(story);
        
        _mockTechnicalAssessor.Setup(a => a.AssessAsync(story, It.IsAny<CancellationToken>()))
            .ReturnsAsync(25f); // Below 30 threshold
        
        // Act
        var result = await _service.AssessStoryAsync(story.Id);
        
        // Assert
        Assert.AreEqual(ApprovalStatus.RejectedTechnical, result.Status);
    }
    
    [TestMethod]
    public async Task CalculateOverallScore_UsesCorrectWeights()
    {
        // Arrange
        var story = CreateTestStory();
        _mockStoryRepo.Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(story);
        
        _mockNarrativeAssessor.Setup(a => a.AssessAsync(story, It.IsAny<CancellationToken>()))
            .ReturnsAsync(80f);
        _mockTechnicalAssessor.Setup(a => a.AssessAsync(story, It.IsAny<CancellationToken>()))
            .ReturnsAsync(90f);
        // ... setup with known values
        
        // Act
        var result = await _service.AssessStoryAsync(story.Id);
        
        // Assert
        var expected = (80 * 0.25) + (90 * 0.30) + /* ... */;
        Assert.AreEqual(expected, result.OverallScore, 0.1);
    }
}
```

### Integration Tests

```csharp
[TestClass]
public class QualityAssessmentIntegrationTests
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }
    
    [TestMethod]
    public async Task CompleteAssessmentWorkflow_SuccessfullyScoresStory()
    {
        // Create a story
        var createResponse = await _client.PostAsJsonAsync("/api/v1/stories", new
        {
            title = "Test Geological Quest",
            description = "A well-written quest about finding rare minerals",
            fullText = "Detailed quest text here..."
        });
        
        createResponse.EnsureSuccessStatusCode();
        var story = await createResponse.Content.ReadFromJsonAsync<StoryResponse>();
        
        // Request quality assessment
        var assessResponse = await _client.PostAsync(
            $"/api/v1/stories/{story.Id}/quality/assess", 
            null);
        
        assessResponse.EnsureSuccessStatusCode();
        var qualityScore = await assessResponse.Content
            .ReadFromJsonAsync<QualityScoreResponse>();
        
        // Verify score structure
        Assert.IsNotNull(qualityScore);
        Assert.IsTrue(qualityScore.OverallScore >= 0 && qualityScore.OverallScore <= 100);
        Assert.IsNotNull(qualityScore.Tier);
        Assert.IsNotNull(qualityScore.Breakdown);
        
        // Verify assessment was saved
        var getResponse = await _client.GetAsync(
            $"/api/v1/stories/{story.Id}/quality/latest");
        
        getResponse.EnsureSuccessStatusCode();
        var savedScore = await getResponse.Content
            .ReadFromJsonAsync<QualityScoreResponse>();
        
        Assert.AreEqual(qualityScore.OverallScore, savedScore.OverallScore);
    }
}
```

---

## Step 7: Deployment Considerations

### Configuration

```json
{
  "QualityAssessment": {
    "Weights": {
      "Narrative": 0.25,
      "Technical": 0.30,
      "Geological": 0.20,
      "Experience": 0.15,
      "Community": 0.10
    },
    "Thresholds": {
      "ExceptionalTier": 85,
      "HighQualityTier": 70,
      "AcceptableTier": 55,
      "NeedsImprovementTier": 40,
      "TechnicalFailure": 30
    },
    "Performance": {
      "MaxAssessmentTimeSeconds": 30,
      "CacheExpirationMinutes": 60,
      "EnableParallelAssessment": true
    },
    "Features": {
      "EnableCommunityScoring": true,
      "EnableManualReview": true,
      "EnableAutoApproval": true
    }
  }
}
```

### Performance Optimization

```csharp
// Caching layer
public class CachedQualityAssessmentService : IQualityAssessmentService
{
    private readonly IQualityAssessmentService _inner;
    private readonly IDistributedCache _cache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(60);
    
    public async Task<QualityScore> GetLatestScoreAsync(
        long storyId, 
        CancellationToken cancellationToken)
    {
        var cacheKey = $"quality:score:{storyId}";
        var cachedScore = await _cache.GetStringAsync(cacheKey, cancellationToken);
        
        if (cachedScore != null)
        {
            return JsonSerializer.Deserialize<QualityScore>(cachedScore);
        }
        
        var score = await _inner.GetLatestScoreAsync(storyId, cancellationToken);
        
        if (score != null)
        {
            var serialized = JsonSerializer.Serialize(score);
            await _cache.SetStringAsync(
                cacheKey, 
                serialized, 
                new DistributedCacheEntryOptions 
                { 
                    AbsoluteExpirationRelativeToNow = _cacheExpiration 
                },
                cancellationToken);
        }
        
        return score;
    }
}
```

### Monitoring and Metrics

```csharp
public class QualityAssessmentMetrics
{
    private readonly IMetricsCollector _metrics;
    
    public void RecordAssessment(QualityScore score, TimeSpan duration)
    {
        _metrics.Increment("assessments.total");
        _metrics.Histogram("assessments.duration_ms", duration.TotalMilliseconds);
        _metrics.Histogram("assessments.overall_score", score.OverallScore);
        _metrics.Increment($"assessments.tier.{score.Tier.ToString().ToLower()}");
        _metrics.Increment($"assessments.status.{score.Status.ToString().ToLower()}");
    }
    
    public void RecordValidationFailure(string reason)
    {
        _metrics.Increment("validations.failures");
        _metrics.Increment($"validations.failure_reason.{reason}");
    }
}
```

---

## Next Steps

1. **Implement Core Services**: Start with data models and core assessment service
2. **Add Dimension Assessors**: Implement each quality dimension assessor
3. **Build Validation Pipeline**: Create validators for technical and content checks
4. **Create API Endpoints**: Expose REST API for quality assessment
5. **Write Tests**: Comprehensive unit and integration tests
6. **Deploy and Monitor**: Set up metrics, alerts, and monitoring
7. **Iterate**: Gather feedback and improve scoring algorithms

---

## References

- [Story Quality Assessment Framework](story-quality-assessment-framework.md)
- [Quality Assurance Framework](../../../../docs/QA_FRAMEWORK.md)
- [API Specification Template](../../../../templates/api-specification.md)
- [Testing Strategy Documentation](../../../../docs/systems/testing-spherical-planet-generation.md)

---

**Document Owner:** Development Team  
**Last Updated:** 2025-01-24  
**Status:** Active
