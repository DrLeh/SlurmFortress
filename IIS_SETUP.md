

## Setup with IIS


### Step 1: Configure manifest.json

Follow steps to configure manifest.json

1. Set name of "new-app-pool" to the top-level app name e.g.  "myapp.ascentral.com"
2. Set website name to match
3. Set website app pool to match
4. Set Physical path to d:\websites\my-app-directory
5. Configure host header on HTTPS and HTTP bindings to local path

### Step 2: Run Manifest
1. Run PowerShell as admin

2. type: `New-WebProject D:/Websites/{{ app }}`

e.g. `New-WebProject D:\Websites\my-app-directory\`

It will find the iis-manifest.json file and run it

3. If .NET Core: Set managed runtime version to No Managed for .NET Core+: iis manifest is not able to configure the "No Managed Code" option,
 so you have to go into IIS app pool settings and configure it manually


## Step 3: Update web.config
1. Update web.config `<aspNetCore processPath` to point to the bin\debug exe path


### Step 4: Add file to hosts

1. Open file `%SystemRoot%\System32\drivers\etc\hosts`
2. Add a line pointing to Step 1.5 e.g. `myapp.local-asicentral.com`
    
`127.0.0.1 myapp.local-asicentral.com`

3. Save file

### Step 5: Configure launch settings

1. Open MyApp.Web.Api\Properties\launchSettings.json
2. Use the below as an example of how to configure IIS

```
{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iis": {
      "applicationUrl": "http://myapp.local-asicentral.com/",
      "sslPort": 0
    }
  },
  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "IIS": {
      "commandName": "IIS",
      "launchBrowser": true,
      "launchUrl": "http://myapp.local-asicentral.com/home",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### Step 6: Done?

Hopefully everything works. 

If using HTTP in local, you may need to reconfigure the app to not do Automatic HTTPS redirection.

In startup.cs, change the following code:

```
app.UseHttpsRedirection();
app.ConfigureSecureHeaders(Configuration);
```

To only be when `!app.IsDevelopment()`.



### IIS_WP_LocalDev user password command

The password for this should be 

D3v3l0PeR5

You can run the following command to get the password for `IIS_WP_LocalDev`
C:\windows\system32\inetsrv>appcmd list apppool CITKA_Orders_x64_v4 /text:*
