using Maybe.App.Extensions;

var builder = WebApplication.CreateBuilder(args);

var url = builder.Configuration["Url"];
if(!string.IsNullOrWhiteSpace(url))
    builder.WebHost.UseUrls(url);

builder.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

var ideasSection = builder.Configuration.GetSection("Ideas");
builder.Services.AddIdeas(ideasSection["Path"]);

var tgSection = builder.Configuration.GetSection("Telegram");
builder.Services.AddTelegram(tgSection["BotToken"], tgSection["ChannelId"], tgSection["NotifyChannelId"]);

builder.Services.AddLogger();

builder.Services.AddJobs();

var app = builder.Build();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.Run();
