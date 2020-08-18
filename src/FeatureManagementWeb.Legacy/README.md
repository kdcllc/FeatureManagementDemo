
```bash

# restore based on packages.config file
nuget restore  packages.config -PackagesDirectory  packages

Update-Package -reinstall

# automatic binding redirect building
Get-Project | Add-BindingRedirect
```
