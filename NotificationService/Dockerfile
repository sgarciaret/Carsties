#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80

# copy all .csproj files and restore as distinct layers. Use of the same COPY command for every dockerfile ibn the project to take advantage of docker caching
COPY Carsties.sln Carsties.sln
COPY AuctionService/AuctionService.csproj AuctionService/AuctionService.csproj
COPY SearchService/SearchService.csproj SearchService/SearchService.csproj
COPY GatewayService/GatewayService.csproj GatewayService/GatewayService.csproj
COPY Contracts/Contracts.csproj Contracts/Contracts.csproj
COPY IdentityService/IdentityService.csproj IdentityService/IdentityService.csproj
COPY BiddingService/BiddingService.csproj BiddingService/BiddingService.csproj
COPY NotificationService/NotificationService.csproj NotificationService/NotificationService.csproj
COPY AuctionService.UnitTests/AuctionService.UnitTests.csproj AuctionService.UnitTests/AuctionService.UnitTests.csproj
COPY AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj AuctionService.IntegrationTests/AuctionService.IntegrationTests.csproj

# Restore package deps
RUN dotnet restore Carsties.sln

# Copy the app folders over
COPY NotificationService NotificationService
COPY Contracts Contracts
WORKDIR /app/NotificationService
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "NotificationService.dll"]