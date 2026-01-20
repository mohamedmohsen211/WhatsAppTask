# ===== Build stage =====
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /app

COPY . .

RUN dotnet restore WhatsAppTask.Api/WhatsAppTask.Api.csproj
RUN dotnet publish WhatsAppTask.Api/WhatsAppTask.Api.csproj -c Release -o out

# ===== Runtime stage =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app/out .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}
EXPOSE 8080

ENTRYPOINT ["dotnet", "WhatsAppTask.Api.dll"]