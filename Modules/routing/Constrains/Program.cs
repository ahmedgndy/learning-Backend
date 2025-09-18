var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add<MonthConstraints>("validMonth");
});
var app = builder.Build();

//int constraint
//bool / decimal / datetime / guid /float 
app.MapGet("/int/{id:int}", (int id) => $"your id is {id}");
//if i send a string it will give me 404 not found

//length Constraint 
app.MapGet("/length/{value:length(5)}", (string value) => $"your value is {value}");
//must be 5 it will give me 404 not found if more or less than five
//maxLength() 
//minLength()
//length(5 , 20)

app.MapGet("/age/{age:min(18)}", (int age) => $"your age is {age}");
//max()
//rang(10 , 29) // those with numbers

// ✅ Range constraint
app.MapGet("/range/{age:int:range(18,120)}", (int age) =>
    $"Range(18-120): {age}");

// ✅ Alpha constraint
app.MapGet("/alpha/{name:alpha}", (string name) =>
    $"Alpha: {name}");

// ✅ Regex constraint (e.g., SSN format: 123-45-6789)
app.MapGet("/regex/{ssn:regex(^\\d{{3}}-\\d{{2}}-\\d{{4}}$)}", (string ssn) =>
    $"Regex Match (SSN): {ssn}");

// ✅ Required parameter
app.MapGet("/required/{name:required}", (string name) =>
    $"Required: {name}");

//custom constrains
app.MapGet("/month/{month:validMonth}", (int month) => $"your month is {month}");
//if i send a string it will give me 404 not found


app.Run();
