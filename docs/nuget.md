# create nugets!

- add below tag in your csproject file or Directory.Build.props

`
  <IsPackable>true</IsPackable>
`


- run below command

`
dotnet pack --configuration Release
`

# using local nuget source
create and use nugets from your local machine

- see your nuget sources:

`
dotnet nuget list source
`

- add your custom nuget source with specific path:

`
dotnet nuget add source D:\nugets --name nuget.my
`
- see your nuget sources again:

`
dotnet nuget list source
`

- push your package to your local source

`
dotnet nuget push D:\projects\QuickStartCore\src\Server\bin\Release\Server.1.0.0.nupkg --source nuget.my
`

# Specify package version

- add below tag in your csproject file or Directory.Build.props

`
<VersionPrefix>1.0.1</VersionPrefix>
`

# use custom nuget source in child projects

- add `nuget.config` file in root directory that contains below content

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <clear/>
        <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
        <add key="nuget.my" value="D:\nugets" />
    </packageSources>
</configuration>
```

- and in your IDE (rider jetbrain) in my case, click on nuget manager and then restore button to load the package sources