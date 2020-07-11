# FeatureManagementWorkerDemo

## Pre-requisites

- [Visual Studio.NET](https://visualstudio.microsoft.com/downloads/)
- If [Visual Studio Code](https://code.visualstudio.com/) is used then please make sure [Azure CLI is installed](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest) installed.



Make sure that Azure AD User assigned `App Configuration Data Reader` permissions.

```bash
    appauthentication run --local  --verbose:debug
```

```bash
    az appconfig kv set -n featuremanagementworkshop --key "Site:Description" --label Site2 --value "Reviews of the best sunglasses online"

    az appconfig credential list -n featuremanagementworkshop -g [your-resource-group] -o json

```