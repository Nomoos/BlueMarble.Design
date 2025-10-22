#!/bin/bash
# Phase 1 Completion Validation Script
# 
# Purpose: Check completion status of all Phase 1 research groups
#          and validate readiness for Phase 2 planning
#
# Usage: ./scripts/validate-phase1-completion.sh
#
# Exit codes:
#   0 = Phase 1 complete, ready for Phase 2
#   1 = Phase 1 incomplete, shows remaining work
#
# This script checks:

echo "╔════════════════════════════════════════════════════════════╗"
echo "║  Phase 1 Research - Completion Validation                 ║"
echo "║  BlueMarble.Design Research Project                       ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Configuration
RESEARCH_DIR="research/literature"
COMPLETED=0
PARTIAL=0
INCOMPLETE=0
TOTAL=40

# Track specific groups of interest
GROUP_03_STATUS="UNKNOWN"
GROUP_06_STATUS="UNKNOWN"

echo "Checking all 40 research assignment groups..."
echo ""

# Check each group
for i in {01..40}; do
  FILE="$RESEARCH_DIR/research-assignment-group-${i}.md"
  
  if [ ! -f "$FILE" ]; then
    echo "Group ${i}: ❌ FILE MISSING"
    INCOMPLETE=$((INCOMPLETE + 1))
    continue
  fi
  
  # Check for completion indicators
  HAS_COMPLETED=$(grep -i "status.*complete\|Phase 1.*Complete\|✅.*Complete" "$FILE" | head -1)
  HAS_PROGRESS=$(grep "\[x\]" "$FILE" | wc -l)
  TOTAL_TASKS=$(grep "\[\s*[x ]\s*\]" "$FILE" | wc -l)
  
  # Check for completion summary file
  SUMMARY_FILE=$(find "$RESEARCH_DIR" -name "*group-${i}*completion*summary*.md" -o -name "*group-${i}*COMPLETED*.md" 2>/dev/null | head -1)
  
  # Determine status
  STATUS=""
  if [ -n "$SUMMARY_FILE" ]; then
    STATUS="✅"
    COMPLETED=$((COMPLETED + 1))
  elif [ -n "$HAS_COMPLETED" ]; then
    STATUS="✅"
    COMPLETED=$((COMPLETED + 1))
  elif [ $TOTAL_TASKS -gt 0 ] && [ $HAS_PROGRESS -gt 0 ]; then
    PERCENT=$((HAS_PROGRESS * 100 / TOTAL_TASKS))
    if [ $PERCENT -ge 100 ]; then
      STATUS="✅"
      COMPLETED=$((COMPLETED + 1))
    else
      STATUS="🔄"
      PARTIAL=$((PARTIAL + 1))
    fi
  else
    STATUS="⏳"
    INCOMPLETE=$((INCOMPLETE + 1))
  fi
  
  # Track Groups 03 and 06 specifically
  if [ "$i" = "03" ]; then
    GROUP_03_STATUS="$STATUS"
  elif [ "$i" = "06" ]; then
    GROUP_06_STATUS="$STATUS"
  fi
  
  # Only show incomplete or partial groups (to keep output clean)
  if [ "$STATUS" != "✅" ]; then
    if [ $TOTAL_TASKS -gt 0 ] && [ $HAS_PROGRESS -gt 0 ]; then
      PERCENT=$((HAS_PROGRESS * 100 / TOTAL_TASKS))
      echo "Group ${i}: $STATUS PARTIAL (${HAS_PROGRESS}/${TOTAL_TASKS} tasks = ${PERCENT}%)"
    else
      echo "Group ${i}: $STATUS NOT COMPLETE"
    fi
  fi
done

echo ""
echo "╔════════════════════════════════════════════════════════════╗"
echo "║  Summary Statistics                                        ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""
echo "Total Groups:      $TOTAL"
echo "✅ Completed:      $COMPLETED ($(( COMPLETED * 100 / TOTAL ))%)"
echo "🔄 Partial:        $PARTIAL"
echo "⏳ Incomplete:     $INCOMPLETE"
echo ""

# Calculate completion percentage
COMPLETION_PERCENT=$((COMPLETED * 100 / TOTAL))

echo "╔════════════════════════════════════════════════════════════╗"
echo "║  Phase 2 Readiness Check                                   ║"
echo "╚════════════════════════════════════════════════════════════╝"
echo ""

# Check specific groups that are blockers
echo "Critical Groups Status:"
echo "  Group 03 (Energy Systems + Historical Maps): $GROUP_03_STATUS"
echo "  Group 06 (Game Design + Blender):            $GROUP_06_STATUS"
echo ""

# Determine overall readiness
if [ $COMPLETED -eq 40 ]; then
  echo "🎉 Phase 1 Status: COMPLETE!"
  echo "✅ All 40 groups finished"
  echo "✅ Ready to proceed with Phase 2 Planning"
  echo ""
  echo "Next Steps:"
  echo "  1. Aggregate discovered sources from all 40 groups"
  echo "  2. Validate and prioritize sources"
  echo "  3. Create discovery statistics"
  echo "  4. Balance and distribute into Phase 2 groups"
  echo "  5. Create Phase 2 assignment files"
  echo "  6. Update master research queue"
  echo "  7. Create Phase 2 sub-issues"
  exit 0
else
  echo "⚠️  Phase 1 Status: INCOMPLETE ($COMPLETION_PERCENT% complete)"
  echo "❌ Cannot proceed with Phase 2 Planning"
  echo ""
  echo "Remaining Work:"
  
  # Check Group 03 status
  if [ "$GROUP_03_STATUS" != "✅" ]; then
    echo ""
    echo "  Group 03: Energy Systems Collection"
    echo "    - Status: Partial (1 of 2 topics complete)"
    echo "    - Remaining: Energy Systems Collection"
    echo "    - Estimated: 5-7 hours"
    echo "    - Deliverable: survival-content-extraction-energy-systems.md"
    
    # Check if document exists
    if [ -f "$RESEARCH_DIR/survival-content-extraction-energy-systems.md" ]; then
      echo "    ✅ Document EXISTS - may need status update"
    else
      echo "    ❌ Document MISSING - research incomplete"
    fi
  fi
  
  # Check Group 06 status
  if [ "$GROUP_06_STATUS" != "✅" ]; then
    echo ""
    echo "  Group 06: Fundamentals of Game Design"
    echo "    - Status: Partial (1 of 2 topics complete)"
    echo "    - Remaining: Fundamentals of Game Design"
    echo "    - Estimated: 6-8 hours"
    echo "    - Deliverable: game-dev-analysis-fundamentals.md"
    
    # Check if document exists
    if [ -f "$RESEARCH_DIR/game-dev-analysis-fundamentals.md" ]; then
      echo "    ✅ Document EXISTS - may need status update"
    else
      echo "    ❌ Document MISSING - research incomplete"
    fi
  fi
  
  echo ""
  echo "Total Estimated Remaining: 11-15 hours across 2 topics"
  echo ""
  echo "Recommendation:"
  echo "  - Assign Group 03 to team member familiar with survival content"
  echo "  - Assign Group 06 to team member with game design expertise"
  echo "  - With 2 people working in parallel: 1 day to complete"
  exit 1
fi
