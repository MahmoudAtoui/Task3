
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Task3.Models;
using Task3.Services;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<UmsdbSettings>(
    builder.Configuration.GetSection("umsdb"));

builder.Services.AddSingleton<LogsService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Initializing RabbitMQ
var factory = new ConnectionFactory
{
    HostName = "localhost"
};
var connection = factory.CreateConnection();
using var channel = connection.CreateModel();
channel.QueueDeclare("log");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" Log message received {message}" );
    
    var log = Newtonsoft.Json.JsonConvert.DeserializeObject<Log>(message);
    
    using var scope = app.Services.CreateScope();
    var logsService = builder.Services.BuildServiceProvider().GetService<LogsService>();
    logsService.CreateAsync(log);
    
    
    
};

channel.BasicConsume("log", true, consumer);
Console.ReadKey();



app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "Hello World!");

app.Run();
