using BroadcastService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<BroadcastListenerService>();

var app = builder.Build();

app.Run();
