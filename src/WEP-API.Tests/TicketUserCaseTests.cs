using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using Web_API.Models;

namespace WEP_API.Tests;

public class TicketUserCaseTests
{
    HttpClient _client;

    private const string TicketEndpoint = "api/Ticket";
    private const string User = "Fabian";
    private const int numberOfTickets = 100;

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
    public async Task ReserveTicketBasedOnUsername()
    {
        // Create dummy ticket for test
        var ticket = await AddDummyRequest(new Ticket
        {
            ReservedBy = null,
            IsPaid = false,
            IsDrawn = false
        });

        // Reserve dummy ticket
        var response = await _client.PostAsync($"{TicketEndpoint}/{ticket!.Number}/reserve?user={User}", null);
        response.EnsureSuccessStatusCode();

        // Make sure ticket is marked as reserved
        var ticketFromPutResponse = await response.Content.ReadFromJsonAsync<Ticket>();
        Assert.That(ticketFromPutResponse.ReservedBy, Is.EqualTo(User));
    }

    [Test]
    public async Task PayTicketsBasedOnUsername()
    {
        // Create dummy tickets for test
        var tickets = await Task.WhenAll(Enumerable.Range(1, 5)
            .Select(async i => await AddDummyRequest(new Ticket
            {
                ReservedBy = null,
                IsPaid = false,
                IsDrawn = false
            })));

        // Reserve all dummy tickets
        foreach (var ticket in tickets)
        {
            var response = await _client.PostAsync($"{TicketEndpoint}/{ticket!.Number}/reserve?user={User}", null);
            response.EnsureSuccessStatusCode();
        }

        // Pay for tickets by username
        var payResponse = await _client.PostAsync($"{TicketEndpoint}/pay?user={User}", null);
        payResponse.EnsureSuccessStatusCode();

        // Ensure that all tickets are marked as payed
        var allTickets = await _client.GetFromJsonAsync<List<Ticket>>(TicketEndpoint);

        foreach (var ticket in tickets.Where(t => t.ReservedBy.Equals(User)))
        {
            Assert.That(ticket.IsPaid);
        }
    }

    [Test]
    public async Task InitializeTickets()
    {
        var response = await _client.PostAsync($"{TicketEndpoint}/initialize?numberOfTickets={numberOfTickets}", null);
        response.EnsureSuccessStatusCode();

        var ticketsFromResponse = await response.Content.ReadFromJsonAsync<List<Ticket>>();
        Assert.That(ticketsFromResponse, Is.Not.Null);
        Assert.That(ticketsFromResponse.Count, Is.EqualTo(numberOfTickets));

        foreach (var ticket in ticketsFromResponse)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ticket.ReservedBy, Is.Null);
                Assert.That(ticket.IsDrawn, Is.False);
                Assert.That(ticket.IsPaid, Is.False);
            });
        }
    }

    [Test]
    public async Task DrawRandom()
    {
        // Create dummy tickets for test
        var ticketsBeforeDraw = await Task.WhenAll(Enumerable.Range(1, 5)
            .Select(async i => await AddDummyRequest(new Ticket
        {
            ReservedBy = User,
            IsPaid = true,
            IsDrawn = i % 2 == 0
        })));

        // Draw ticket
        var drawnTicket = await _client.GetFromJsonAsync<Ticket>($"{TicketEndpoint}/draw");
        var drawnTicketBeforeDraw = ticketsBeforeDraw.FirstOrDefault(t => t.Number.Equals(drawnTicket.Number));

        // Ensure that the drawn ticket has been updated corretly
        Assert.Multiple(() =>
        {
            Assert.That(drawnTicketBeforeDraw, Is.Not.Null);
            Assert.That(drawnTicket, Is.Not.Null);
            Assert.That(drawnTicket.ReservedBy, Is.Not.Null);
            Assert.That(drawnTicket.IsPaid, Is.True);
            Assert.That(drawnTicket.IsDrawn, Is.True);
        });
    }

    private async Task<Ticket> AddDummyRequest(Ticket ticket)
    {
        var response = await _client.PutAsJsonAsync(TicketEndpoint, ticket);
        response.EnsureSuccessStatusCode();

        var ticketFromResponse = await response.Content.ReadFromJsonAsync<Ticket>();
        return ticketFromResponse;
    }

}