using armAPI;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
  options.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    ValidateAudience = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
    ValidateLifetime = false, // In any other application other then demo this needs to be true,
    ValidateIssuerSigningKey = true
  };
});

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

var app = builder.Build();
app.UseAuthorization();
app.UseAuthentication();

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

app.MapGet("/user",/*[Authorize] */ async (DataContext context) =>
  await context.Users.ToListAsync());

app.MapGet("/user/{id}",/*[Authorize] */ async (DataContext context, int id) =>
  await context.Users.FindAsync(id) is User user ?
    Results.Ok(user) :
    Results.NotFound("No user of id:" + id));

app.MapPost("/user",/*[Authorize] */ async (DataContext context, User user) =>
{
  context.Users.Add(user);
  await context.SaveChangesAsync();
  return Results.Ok(await GetAllUsers(context));

});

app.MapPut("/user/{id}",/*[Authorize] */async (DataContext context, User user, int id) =>
{
  var dbuser = await context.Users.FindAsync(id);
  if (dbuser == null) return Results.NotFound("No user found. :/");

  dbuser.Firstname = user.Firstname;
  dbuser.Lastname = user.Lastname;
  dbuser.Authlevel = user.Authlevel;
  dbuser.Email = user.Email;
  dbuser.Password = user.Password;
  dbuser.Role = user.Role;
  dbuser.Phone = user.Phone;
  dbuser.Modifiedon=user.Modifiedon;
  await context.SaveChangesAsync();

  return Results.Ok(await GetAllUsers(context));
});

app.MapDelete("/user/{id}",/*[Authorize] */ async (DataContext context, int id) =>
{
 var dbuser = await context.Users.FindAsync(id);
  if (dbuser == null) return Results.NotFound("who dat??");

  context.Users.Remove(dbuser);
  await context.SaveChangesAsync();

  return Results.Ok(await GetAllUsers(context));
});

app.MapPost("/login", [AllowAnonymous] (DataContext context, User user, string Email, string Password) =>
 {
   var dbuser = context.Users.SingleOrDefault(x => x.Email == Email && x.Password == Password);
   if (dbuser != null)
   {
     var secureKey = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]);

     var issuer = builder.Configuration["Jwt:Issuer"];
     var audience = builder.Configuration["Jwt:Audience"];
     var securityKey = new SymmetricSecurityKey(secureKey);
     var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

     var jwtTokenHandler = new JwtSecurityTokenHandler();

     var tokenDescriptor = new SecurityTokenDescriptor
     {
       Subject = new System.Security.Claims.ClaimsIdentity(new[]
       {
         //new Claim(JwtRegisteredClaimNames.id)
         //new Claim(JwtRegisteredClaimNames.Sub, userLogin.Email),
         //new Claim(JwtRegisteredClaimNames.Email, userLogin.Email),
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

       }),
       Expires = DateTime.Now.AddMinutes(10),
       Audience = audience,
       Issuer = issuer,
       SigningCredentials = credentials
     };

     var token = jwtTokenHandler.CreateToken(tokenDescriptor);
     var jwtToken = jwtTokenHandler.WriteToken(token);
     return Results.Ok(jwtToken);


   }
   return Results.Unauthorized();

   //var dbuser = await context.Users.FindAsync(user.Email, user.Password);
   //if (dbuser == null) return Results.NotFound("User Not Found");

   //var claims = new[]
   //{
   //  new Claim(ClaimTypes.NameIdentifier, dbuser.Firstname),
   //  new Claim(ClaimTypes.Email, dbuser.Email),
   //  new Claim(ClaimTypes.Surname, dbuser.Lastname),
   //  new Claim(ClaimTypes.Role, dbuser.Role),
   //  new Claim(ClaimTypes.MobilePhone, dbuser.Phone),
   //  new Claim(ClaimTypes.AuthorizationDecision, dbuser.Authlevel)

   // };
 });


app.Run();
