# GrpcDemo
A look at the different type of gRPC types available: Unary, Server streaming, Client streaming, Bi-directional streaming

gRPC can be used for interprocess communication such as comms between microservices.
Here a grpc server talks to a server side blazor service.  
(Blazor WASM has to use gRPC-Web and is limited due to being inside the browser sandbox)

# Notes

The .proto file is a language agnostic definition of the service defintion(contract) and the messages(models). It is the single source of truth.


## Client

The Client C# code is generated from the .proto file via the following in the .csproj file:

```
  <ItemGroup>
    <Protobuf Include="..\ServiceA\Protos\greet.proto" GrpcServices="Client">
      <Link>Protos\greet.proto</Link>
    </Protobuf>
  </ItemGroup>
```
Client Code is viewable at ServiceB\obj\Debug\net5.0\GreetGrpc.cs

It can be added as a Connected Service in Visual Studio to point to a local file or a hosted url.
Generation options include are Client, Server, Client and Server, Messages only.

## Server

The Server code creates a base class which the service can inherit from. The default implementation throws a RPCException of not implemented.
The service methods are overridden to provide the actual [implementation](ServiceA/Services/GreeterService.cs)


# Links

[MS Docs](https://docs.microsoft.com/en-us/aspnet/core/grpc/?view=aspnetcore-5.0)
[Proto3 Language Defiintion](https://developers.google.com/protocol-buffers/docs/proto3)

