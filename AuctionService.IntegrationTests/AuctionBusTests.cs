﻿using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.IntegrationTests.Fixtures;
using AuctionService.IntegrationTests.Util;
using Contracts;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace AuctionService.IntegrationTests
{
    [Collection("Shared collection")]
    public class AuctionBusTests : IAsyncLifetime
    {
        private readonly CustomWebAppFactory _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestHarness _testHarness;

        public AuctionBusTests(CustomWebAppFactory factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
            _testHarness = _factory.Services.GetTestHarness();
        }

        [Fact]
        public async Task CreateAuction_WithValidObject_ShouldReturnPublishAuctionCreated()
        {
            // arrange
            var auction = GetAuctionForCreate();
            _httpClient.SetFakeJwtBearerToken(AuthHelper.GetBearerForUser("bob"));

            // act
            var response = await _httpClient.PostAsJsonAsync("api/auctions", auction);

            // assert
            response.EnsureSuccessStatusCode();
            Assert.True(await _testHarness.Published.Any<AuctionCreated>());
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public Task DisposeAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AuctionDbContext>();
            DbHelper.ReinitDbForTests(db);
            return Task.CompletedTask;
        }

        private static CreateAuctionDto GetAuctionForCreate()
        {
            return new CreateAuctionDto
            {
                Make = "test",
                Model = "testModel",
                ImageUrl = "test",
                Color = "test",
                Mileage = 10,
                Year = 10,
                ReservePrice = 10,
                AuctionEnd = DateTime.UtcNow.AddDays(10)
            };
        }
    }
}
