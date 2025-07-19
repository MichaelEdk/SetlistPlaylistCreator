---
mode: 'agent'
description: 'Ensure .NET/C# code meets best practices for the solution/project.'
---
# .NET/C# Best Practices

Your task is to ensure .NET/C# code in ${selection} meets the best practices specific to this solution/project. This includes:

## Testing standards

- Use MSTest for unit tests.
- Use NSubstitute for mocking dependencies in tests.
- Test both success and failure scenarios.
- Include null parameter validation tests.
- Use the naming convention `MethodName_StateUnderTest_ExpectedBehavior` for test methods.
- Test projects should be named with the format `ProjectName.UnitTests`.
- Test classes should be named with the format `ClassNameTests`.
- If there is no existing test project, create a new one with the appropriate name and structure.
- When creating the subject under test, have a method like the following example, at the bottom of the class, but don't call it literally `CreateSubjectUnderTest`:
  
  ```csharp
  private static SubjectUnderTest CreateSubjectUnderTest(
        IDependency1? dependency1 = null,
        IDependency2? dependency2 = null)
  {
      return new(
            dependency1 ?? Substitute.For<IDependency1>(),
            dependency2 ?? Substitute.For<IDependency2>()
      );
  }
  ```

  - If a dependency can be substituted without setting any values, let the static helper method handle it.
  - If reflection is required to test a code path, don't test that code path. Leave a comment explaining why it is not tested.