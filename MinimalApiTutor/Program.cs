using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MinimalApiTutor.Data;
using MinimalApiTutor.Dtos;
using MinimalApiTutor.Models;
using MinimalApiTutor.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var sqlConnBuilder = new SqlConnectionStringBuilder();

sqlConnBuilder.ConnectionString = builder.Configuration.GetConnectionString("Default");
//sqlConnBuilder.UserID = builder.Configuration["UserId"];
//sqlConnBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(sqlConnBuilder.ConnectionString);
});

builder.Services.AddScoped<ICommandRepo, CommandRepo>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.MapGet("api/commands", async (ICommandRepo repo, IMapper mapper) =>
{
    var commands = await repo.GetAllCommands();

    return Results.Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
});

app.MapGet("api/commands/{id}", async (ICommandRepo repo, IMapper mapper, [FromRoute]int id) =>
{
    var command = await repo.GetCommandById(id);

    if(command != null)
       return Results.Ok(mapper.Map<CommandReadDto>(command));

    return Results.NotFound("Not found the command");
});

app.MapPost("api/commands", async (ICommandRepo repo, IMapper mapper, 
    [FromBody]CommandCreateDto newCmdDto) =>
{
    var commandModel = mapper.Map<Command>(newCmdDto);

    await repo.CreateCommand(commandModel);

    var cmdReadDto = mapper.Map<CommandReadDto>(commandModel);

    return Results.Created($"api/commands/{cmdReadDto.Id}", cmdReadDto);
});

app.MapPut("api/commands/{id}", async (ICommandRepo repo, IMapper mapper,
    [FromRoute]int id,
    [FromBody] CommandUpdateDto updateCmdDto) =>
{
    var command = await repo.GetCommandById(id);

    if (command == null)
        return Results.NotFound("Not found the command");

    mapper.Map(updateCmdDto,command);

    await repo.SaveChanges();

    return Results.NoContent();
});

app.MapDelete("api/commands/{id}", async (ICommandRepo repo, IMapper mapper,
    [FromRoute] int id) =>
{
    var cmd = await repo.GetCommandById(id);

    if (cmd == null)
        return Results.NotFound("Not found the command");

    repo.DeleteCommand(cmd);

    return Results.NoContent();
});


app.Run();
