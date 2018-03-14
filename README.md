# Cake.SendGrid

[![License](http://img.shields.io/:license-mit-blue.svg)](http://cake-contrib.mit-license.org)

Cake.SendGrid is an Addin for [Cake](http://cakebuild.net/) which allows sending of email via SendGrid.

## Usage

First, include a reference to this addin in your script like this:
```csharp
#addin nuget:?package=Cake.SendGrid&version=0.6.0&loaddependencies=true
```

Second, we highly recommend that you add the following 'using' statement in your script. Technically, this is not necesary, but it simplifies dealing with attachements: 
```csharp
using Cake.Email.Common;
```

Also, this addin is designed to take advantage of some of the new features released in CakeBuild version `0.26.0` therefore your `tools\package.config` should look like this:
```xml
<packages>
    <package id="Cake" version="0.26.0" />
</packages>
```

## Information

| |Stable|Pre-release|
|:--:|:--:|:--:|
|GitHub Release|-|[![GitHub release](https://img.shields.io/github/release/cake-contrib/Cake.SendGrid.svg)](https://github.com/cake-contrib/Cake.SendGrid/releases/latest)|
|Package|[![NuGet](https://img.shields.io/nuget/v/Cake.SendGrid.svg)](https://www.nuget.org/packages/Cake.SendGrid)|[![MyGet](https://img.shields.io/myget/cake-contrib/vpre/Cake.SendGrid.svg)](http://myget.org/feed/cake-contrib/package/nuget/Cake.SendGrid)|

## Build Status

|Develop|Master|
|:--:|:--:|
|[![Build status](https://ci.appveyor.com/api/projects/status/fheg6neg8kv1803h/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-sendgrid/branch/develop)|[![Build status](https://ci.appveyor.com/api/projects/status/fheg6neg8kv1803h/branch/develop?svg=true)](https://ci.appveyor.com/project/cakecontrib/cake-sendgrid/branch/master)|

## Code Coverage

[![Coverage Status](https://coveralls.io/repos/github/cake-contrib/Cake.SendGrid/badge.svg)](https://coveralls.io/github/cake-contrib/Cake.SendGrid)

## Quick Links

- [Documentation](https://cake-contrib.github.io/Cake.SendGrid/)

## Chat Room
Come join in the conversation about Cake.SendGrid in our Gitter Chat Room

[![Join the chat at https://gitter.im/cake-contrib/Lobby](https://badges.gitter.im/cake-contrib/Lobby.svg)](https://gitter.im/cake-contrib/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
