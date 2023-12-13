# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 7166

WORKDIR /src

# Copy the solution file and restore dependencies
COPY TicTacToe.sln .
COPY webapi/ webapi/
COPY Lib/ Lib/
RUN dotnet restore

# Copy the entire solution
COPY . .

# Build the application
RUN dotnet build -c Release --no-restore

# Stage 2: Publish the application
FROM build AS publish
WORKDIR /src/webapi
RUN dotnet publish -c Release -o /app --no-restore

# Stage 3: Final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "webapi.dll"]
