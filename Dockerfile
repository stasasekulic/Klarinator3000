# Use the official .NET 7 image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build

# Set the working directory
WORKDIR /src

# Copy the project files into the container
COPY . .

# Restore NuGet packages
RUN dotnet restore

# Build the application
RUN dotnet build --configuration Release

# Publish the application
RUN dotnet publish --configuration Release --output /app/out

# Use the official .NET 7 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine

# Set the working directory
WORKDIR /app

# Copy the published files from the build stage
COPY --from=build /app/out .

# Expose the port for the Web API
EXPOSE 80

# Set the command for the container
ENTRYPOINT ["dotnet", "Klarinator3000.dll"]