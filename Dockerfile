# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoApp.csproj", "./"]
RUN dotnet restore "TodoApp.csproj"
COPY . .
RUN dotnet publish "TodoApp.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "TodoApp.dll", "--urls", "http://0.0.0.0:80"]