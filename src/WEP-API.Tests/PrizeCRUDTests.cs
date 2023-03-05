using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_API.Models;

namespace WEP_API.Tests;

public class PrizeCRUDTests
{
    HttpClient _client;

    private const string PrizeEndpoint = "api/Prize";
    private static Prize ExamplePrize = new()
    {
        Name = "Renieri Chianti Classico 2016",
        Description = "Italia, Toscana, Chianti Classico",
        Cost = 199.90m
    };

    [SetUp]
    public void Setup()
    {
        var application = new WebApplicationFactory<Program>();
        _client = application.CreateClient();
    }
    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
    }

    [Test]
    public async Task AddPrize()
    {
        // Add dummy prize
        var prize = await AddPrizeRequest();

        Assert.Multiple(() =>
        {
            Assert.That(prize, Is.Not.Null);
            Assert.That(prize.Name, Is.EqualTo(ExamplePrize.Name));
            Assert.That(prize.Description, Is.EqualTo(ExamplePrize.Description));
            Assert.That(prize.Cost, Is.EqualTo(ExamplePrize.Cost));
            Assert.That(prize.TicketNumber, Is.EqualTo(ExamplePrize.TicketNumber));
        });
    }

    [Test]
    public async Task GetPrize()
    {
        // Add dummy prize
        var prize = await AddPrizeRequest();

        var prizes = await _client.GetFromJsonAsync<Prize>($"{PrizeEndpoint}/{prize.Id}");
        Assert.That(prizes, Is.Not.Null);
    }

    [Test]
    public async Task GetAllPrizes()
    {
        var prizes = await _client.GetFromJsonAsync<List<Prize>>(PrizeEndpoint);
        Assert.That(prizes, Is.Not.Null);
    }


    [Test]
    public async Task UpdatePrize()
    {
        // Add dummy prize
        var prize = await AddPrizeRequest();

        // Update the dummy prize using its ID
        prize.TicketNumber = 1;
        var putResponse = await _client.PostAsJsonAsync($"{PrizeEndpoint}", prize);
        putResponse.EnsureSuccessStatusCode();

        var prizeFromPutResponse = await putResponse.Content.ReadFromJsonAsync<Prize>();
        Assert.That(prizeFromPutResponse.Id, Is.EqualTo(prize.Id));
        Assert.That(prizeFromPutResponse.TicketNumber, Is.EqualTo(prize.TicketNumber));
    }

    [Test]
    public async Task DeletePrize()
    {
        // Add dummy prize
        var prize = await AddPrizeRequest();

        // Delete the dummy prize using its ID
        var deleteResponse = await _client.DeleteAsync($"{PrizeEndpoint}/{prize!.Id}");
        deleteResponse.EnsureSuccessStatusCode();

        // Ensure that the dummy prize was deleted by trying to retrieve it again
        var getResponse = await _client.GetAsync($"{PrizeEndpoint}/{prize.Id}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task<Prize> AddPrizeRequest()
    {
        var response = await _client.PutAsJsonAsync(PrizeEndpoint, ExamplePrize);
        response.EnsureSuccessStatusCode();

        var prizeFromResponse = await response.Content.ReadFromJsonAsync<Prize>();
        return prizeFromResponse;
    }

}