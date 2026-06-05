FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files
COPY CharacterLib/CharacterLib.csproj CharacterLib/
COPY CharacterApi/CharacterApi.csproj CharacterApi/

# Restore dependencies
RUN dotnet restore CharacterApi/CharacterApi.csproj

# Copy source code
COPY CharacterLib/ CharacterLib/
COPY CharacterApi/ CharacterApi/

# Build and publish
RUN dotnet publish CharacterApi/CharacterApi.csproj -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

# Railway sets PORT env variable automatically
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

ENTRYPOINT ["dotnet", "CharacterApi.dll"]
