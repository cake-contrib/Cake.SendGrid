---
Title: New Release - 0.7.0
Published: 02/07/2018
Category: Release
Author: jericho
---

## Breaking Changes

- [__#23__](https://github.com/cake-contrib/Cake.SendGrid/issues/23) Support Cake 0.28.0


## Note

First, include a reference to this addin in your script like this:
```csharp
#addin nuget:?package=Cake.SendGrid&version=0.7.0&loaddependencies=true
```

Second, we highly recommend that you add the following 'using' statement in your script. Technically, this is not necessary, but it simplifies dealing with attachements: 
```csharp
using Cake.Email.Common;
```

Also, this addin is designed to take advantage of some of the new features released in CakeBuild version `0.28.0` therefore your `tools\package.config` should look like this:
```xml
<packages>
    <package id="Cake" version="0.28.0" />
</packages>
```

Please do not hesitate to reach out in the [Gitter Channel](https://gitter.im/cake-contrib/Lobby) if you have any issues using this addin.
