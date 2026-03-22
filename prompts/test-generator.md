# Test Generator

Generate tests for a pure C# gameplay/domain class from this repo.

Requirements:
- prioritize deterministic logic
- include edge cases
- avoid engine-heavy integration where a pure unit test will do
- prefer readable names and direct assertions

Return:
1. the test file
2. assumptions made
3. gaps that still require manual play validation
