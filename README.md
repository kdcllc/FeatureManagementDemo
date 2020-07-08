# FeatureManagementDemo


## Project setup

1. Create `dotnet new mvc FeatureManagementWeb`

2. Add `dotnet add package Microsoft.FeatureManagement.AspNetCore`

3. Add Feature Configuration

The library allows for custom configuration to be passed thru usage of `Parameters`

```json
    "FeatureManagement": {
        "Beta": false,
        "Alpha": true
    }
```

4. Add `FeatureFlags.cs` file

```csharp
    public enum FeatureFlags
    {
        Beta,
        Alpha
    }
```

5. Add FeatureManagement to DI 

```csharp
    public class Startup 
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //...
            services.AddFeatureManagement();
        }
    }
```

6. Add MVC Controller `BetaController.cs` and corresponding view


```csharp
    public class BetaController : Controller
    {
        [FeatureGate(RequirementType.All, FeatureFlags.Beta, FeatureFlags.Alpha)]
        public IActionResult Index()
        {
            return View();
        }
    }
```

Then `Beta/Index.cshtml`

```razor
    @{
        ViewData["Title"] = "Beta Page";
    }
    
    <div class="text-center">
        <h1 class="display-4">This is beta MVC Controller</h1>
        <p>Learn about <a href="https://github.com/microsoft/FeatureManagement-Dotnet">Feature Management</a>.</p>
    </div>

```

7. Add Tag Helper to `_ViewImports.cshtml`

```razor
    Add `@addTagHelper *, Microsoft.FeatureManagement.AspNetCore` to `_ViewImports.cshtml`.
```

8. Add Menu item based on the features status `_Layout.cshtml`

```razor
        <!-- Check if the feature is enabled using FeatureTagHelper -->
        <feature name="@FeatureFlags.Beta">
            <li class="nav-item">
                <a class="nav-link text-dark" asp-area="" asp-controller="Beta" asp-action="Index">Beta</a>
            </li>
        </feature>
```