# Radius server library for .NET Standard

This project can be used to create a Radius server in for example a Windows Service.
   
Packets are handled in multiple threads without running several instances. This can be useful when packet handlers do something slow, like lookups from external dependencies.


Pluggable packet handlers for different remote IPs. 
Conditionally compliant with RFCs  
https://tools.ietf.org/html/rfc2865  
https://tools.ietf.org/html/rfc2866  
https://tools.ietf.org/html/rfc5997  
  

  
[![Build status](https://ci.appveyor.com/api/projects/status/dbc6ua1ypa9eas3p?svg=true)](https://ci.appveyor.com/project/vforteli/radiusserver)

# RadiusServer usage  
See https://github.com/vforteli/RadiusServerService/tree/Base for an example implementation  

```csharp
var path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\radius.dictionary";
var dictionary = new RadiusDictionary(path);
var radiusPacketParser = new RadiusPacketParser(NullLogger<RadiusPacketParser>.Instance, dictionary);

var packetHandler = new TestPacketHandler();
var repository = new PacketHandlerRepository();
repository.AddPacketHandler(IPAddress.Any, packetHandler, secret);

var authenticationServer = new RadiusServer(new IPEndPoint(IPAddress.Any, 1812), radiusPacketParser, RadiusServerType.Authentication, repository);                
authenticationServer.AddPacketHandler(IPAddress.Parse("127.0.0.1"), "secret", packetHandler);
authenticationServer.Start();
```  

The packet handler should implement IPacketHandler  

https://www.nuget.org/packages/Flexinets.Radius.RadiusServer/
