---
theme: gaia
paginate: true
backgroundColor: #fff
marp: true
backgroundImage: url('./images/background.png')
footer: '![image](./images/ms-azure-logo.png)'
---

<!-- _class: lead -->

![bg left:40% 80%](./images/ms-azure-logo.png)

# **Workshop**

Feature Management And Azure AppConfiguration

https://github.com/kdcllc/FeatureManagementWorkshop

---

# Enable AppConfigurations

Add the following to `IHost`

```csharp
    return Host.CreateDefaultBuilder(args)
                        .UseAzureAppConfiguration(
                        (connect, config) =>
                        {
                            config.Bind("AppConfig", connect);
                        },
                        (interval, config) =>
                        {
                            interval.RefreshInterval = config.GetValue<TimeSpan>("AppConfig:RefreshInterval");
                        });
```

---

# Questions?

:satisfied: