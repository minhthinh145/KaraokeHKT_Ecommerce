# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY QLQuanKaraokeHKT.Presentation/QLQuanKaraokeHKT.Presentation.csproj QLQuanKaraokeHKT.Presentation/
COPY QLQuanKaraokeHKT.Application/QLQuanKaraokeHKT.Application.csproj QLQuanKaraokeHKT.Application/
COPY QLQuanKaraokeHKT.Domain/QLQuanKaraokeHKT.Domain.csproj QLQuanKaraokeHKT.Domain/
COPY QLQuanKaraokeHKT.Infrastructure/QLQuanKaraokeHKT.Infrastructure.csproj QLQuanKaraokeHKT.Infrastructure/
COPY QLQuanKaraokeHKT.Shared/QLQuanKaraokeHKT.Shared.csproj QLQuanKaraokeHKT.Shared/

RUN dotnet restore QLQuanKaraokeHKT.Presentation/QLQuanKaraokeHKT.Presentation.csproj

# Copy source code and build
COPY . .
WORKDIR /src/QLQuanKaraokeHKT.Presentation
RUN dotnet publish QLQuanKaraokeHKT.Presentation.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "QLQuanKaraokeHKT.Presentation.dll"]
