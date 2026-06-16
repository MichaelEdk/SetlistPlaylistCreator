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

---
description: '.NET WPF component and application patterns'
applyTo: '**/*.xaml, **/*.cs'
---

## Summary

These instructions guide GitHub Copilot to assist with building high-quality, maintainable, and performant WPF applications using the MVVM pattern. It includes best practices for XAML, data binding, UI responsiveness, and .NET performance.

## Ideal project types

- Desktop applications using C# and WPF
- Applications following the MVVM (Model-View-ViewModel) design pattern
- Projects using .NET 8.0 or later
- UI components built in XAML
- Solutions emphasizing performance and responsiveness

## Goals

- Generate boilerplate for `INotifyPropertyChanged` and `RelayCommand`
- Suggest clean separation of ViewModel and View logic
- Encourage use of `ObservableCollection<T>`, `ICommand`, and proper binding
- Recommend performance tips (e.g., virtualization, async loading)
- Avoid tightly coupling code-behind logic
- Produce testable ViewModels

## Example prompt behaviors

### ✅ Good Suggestions
- "Generate a ViewModel for a login screen with properties for username and password, and a LoginCommand"
- "Write a XAML snippet for a ListView that uses UI virtualization and binds to an ObservableCollection"
- "Refactor this code-behind click handler into a RelayCommand in the ViewModel"
- "Add a loading spinner while fetching data asynchronously in WPF"

### ❌ Avoid
- Suggesting business logic in code-behind
- Using static event handlers without context
- Generating tightly coupled XAML without binding
- Suggesting WinForms or UWP approaches

## Technologies to prefer
- C# with .NET 8.0+
- XAML with MVVM structure
- `CommunityToolkit.Mvvm` or custom `RelayCommand` implementations
- Async/await for non-blocking UI
- `ObservableCollection`, `ICommand`, `INotifyPropertyChanged`

## Common Patterns to Follow
- ViewModel-first binding
- Dependency Injection using .NET or third-party containers (e.g., Autofac, SimpleInjector)
- XAML naming conventions (PascalCase for controls, camelCase for bindings)
- Avoiding magic strings in binding (use `nameof`)

## Sample Instruction Snippets Copilot Can Use

```csharp
public class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string password;

    [RelayCommand]
    private void Login()
    {
        // Add login logic here
    }
}
```

```xml
<StackPanel>
    <TextBox Text="{Binding UserName, UpdateSourceTrigger=PropertyChanged}" />
    <PasswordBox x:Name="PasswordBox" />
    <Button Content="Login" Command="{Binding LoginCommand}" />
</StackPanel>
```