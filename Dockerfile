# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the project
COPY . ./

# Publish app
RUN dotnet publish -c Release -o /app/publish
RUN dotnet restore --disable-parallel --force

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/publish .

# Create folder for DataProtection keys
RUN mkdir -p /root/.aspnet/DataProtection-Keys

# Expose port inside container
EXPOSE 8080

# Set environment variable for ASP.NET to listen on all IPs
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_HOST=0.0.0.0
ENV ASPNETCORE_URLS=http://+:8080

# Start the app
ENTRYPOINT ["dotnet", "myblog.dll"]
