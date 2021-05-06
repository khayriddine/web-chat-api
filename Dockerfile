FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app
COPY *.csproj* ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o output

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 As publish
EXPOSE 80
WORKDIR /app
COPY --from=build-env /app/output /app
ENTRYPOINT ["dotnet", "web-chat-api.dll"]