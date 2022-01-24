
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseContentRoot(Directory.GetCurrentDirectory());

builder.Services.AddCors();
builder.Services.AddDbContext<BookContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("BookDB")));

var app = builder.Build();

app.UseStaticFiles();

app.UseCors(options =>
{
    options.AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();
});

app.MapGet("/api/books", async (BookContext db) => await db.Books.ToListAsync());

app.MapGet("/api/books/{id}", async (BookContext db, int id) => await db.Books.FindAsync(id));

app.MapPost("/api/books", async (BookContext db, Book book) =>
{
    await db.Books.AddAsync(book);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPut("/api/books/{id}", async (BookContext db, int id, Book book) =>
{
    if (id != book.Id) return Results.BadRequest();
    db.Books.Update(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/books/{id}", async (BookContext db, int id) =>
{
    var book = await db.Books.FindAsync(id);
    if (book == null) return Results.NotFound();

    db.Books.Remove(book);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
