# Use the official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore as separate layers for better caching
COPY ["HospitalOPD.csproj", "./"]
RUN dotnet restore "./HospitalOPD.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "HospitalOPD.csproj" -c Release -o /app/publish

# Use a runtime image for a smaller final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port your API listens on (adjust if needed)
EXPOSE 80
ENTRYPOINT ["dotnet", "HospitalOPD.dll"]