using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_API.Models;

namespace WEP_API.Tests;

public class TicketCRUDTests
{
    HttpClient _client;

    private const string TicketEndpoint = "api/Ticket";
    private static Ticket ExampleTicket = new Ticket
    {
        ReservedBy = null,
        IsPaid = false,
        IsDrawn = false
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
    public async Task AddTicket()
    {
        // Add dummy ticket
        var ticket = await AddTicketRequest();

        Assert.Multiple(() =>
        {
            Assert.That(ticket, Is.Not.Null);
            Assert.That(ticket.ReservedBy, Is.EqualTo(ExampleTicket.ReservedBy));
            Assert.That(ticket.IsPaid, Is.EqualTo(ExampleTicket.IsPaid));
            Assert.That(ticket.IsDrawn, Is.EqualTo(ExampleTicket.IsDrawn));
        });
    }

    [Test]
    public async Task GetTicket()
    {
        // Add dummy ticket
        var ticket = await AddTicketRequest();

        var tickets = await _client.GetFromJsonAsync<Ticket>($"{TicketEndpoint}/{ticket.Number}");
        Assert.That(tickets, Is.Not.Null);
    }

    [Test]
    public async Task GetAllTickets()
    {
        var tickets = await _client.GetFromJsonAsync<List<Ticket>>(TicketEndpoint);
        Assert.That(tickets, Is.Not.Null);
    }


    [Test]
    public async Task UpdateTicket()
    {
        // Add dummy ticket
        var ticket = await AddTicketRequest();

        // Update the dummy ticket using its ID
        ticket.IsDrawn = true;
        var putResponse = await _client.PostAsJsonAsync($"{TicketEndpoint}", ticket);
        putResponse.EnsureSuccessStatusCode();

        var ticketFromPutResponse = await putResponse.Content.ReadFromJsonAsync<Ticket>();
        Assert.That(ticketFromPutResponse.Number, Is.EqualTo(ticket.Number));
        Assert.That(ticketFromPutResponse.IsDrawn, Is.EqualTo(ticket.IsDrawn));
    }

    [Test]
    public async Task DeleteTicket()
    {
        // Add dummy ticket
        var ticket = await AddTicketRequest();

        // Delete the dummy ticket using its ID
        var deleteResponse = await _client.DeleteAsync($"{TicketEndpoint}/{ticket!.Number}");
        deleteResponse.EnsureSuccessStatusCode();

        // Ensure that the dummy ticket was deleted by trying to retrieve it again
        var getResponse = await _client.GetAsync($"{TicketEndpoint}/{ticket.Number}");
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    private async Task<Ticket> AddTicketRequest()
    {
        var response = await _client.PutAsJsonAsync(TicketEndpoint, ExampleTicket);
        response.EnsureSuccessStatusCode();

        var ticketFromResponse = await response.Content.ReadFromJsonAsync<Ticket>();
        return ticketFromResponse;
    }

}