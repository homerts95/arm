using armAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<User>> GetAllUsers(DataContext context) =>
   await context.Users.ToListAsync();


app.MapGet("/", () =>"Hi there");

app.MapGet("/user", async (DataContext context) =>
  await context.Users.ToListAsync());

app.MapGet("/user/{id}", async (DataContext context, int id) =>
  await context.Users.FindAsync(id) is User user ?
    Results.Ok(user) :
    Results.NotFound("No user of id:" + id));

app.MapPost("/user", async (DataContext context, User user) =>
{
  context.Users.Add(user);
  await context.SaveChangesAsync();
  return Results.Ok(await GetAllUsers(context));

});

app.MapPut("/superhero/{id}", async (DataContext context, User user, int id) =>
{
  var dbuser = await context.Users.FindAsync(id);
  if (dbuser == null) return Results.NotFound("No user found. :/");

  dbuser.Firstname = user.Firstname;
  dbuser.Lastname = user.Lastname;
  dbuser.IsAdmin = user.IsAdmin;
  dbuser.Email = user.Email;
  dbuser.Password = user.Password;
  dbuser.IsTrainer = user.IsTrainer;
  dbuser.Phone = user.Phone;
  dbuser.Modifiedon=user.Modifiedon;
  await context.SaveChangesAsync();

  return Results.Ok(await GetAllUsers(context));
});

app.MapDelete("/user/{id}", async (DataContext context, int id) =>
{
 var dbuser = await context.Users.FindAsync(id);
  if (dbuser == null) return Results.NotFound("who dat??");

  context.Users.Remove(dbuser);
  await context.SaveChangesAsync();

  return Results.Ok(await GetAllUsers(context));
});


app.Run();
