FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet msbuild -t:restore -p:RestoreSources="https://api.nuget.org/v3/index.json;https://api.bintray.com/nuget/fint/nuget" Fint.SSE.Adapter.Elev.sln
RUN dotnet test Fint.Sse.Adapter.Tests
RUN dotnet publish -c Release -o out Fint.Sse.Adapter.Console

# Build runtime image
FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build-env /app/Fint.Sse.Adapter.Console/out .
COPY --from=build-env /app/Fint.Sse.Adapter.Console/appsettings.json .
ENTRYPOINT ["dotnet", "Fint.Sse.Adapter.Console.dll"]