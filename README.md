# mediator-core

#### Dependency Injection
```csharp
builder.Services
    .AddMediatorCore(typeof(Program).Assembly,
    cfg =>
    {
        cfg.AddValidators(typeof(Program).Assembly);
        cfg.AddEventHandlers(typeof(Program).Assembly);
    })
```

---
### Request
```csharp
public class CreateCommand : IRequest<bool>
{
    [Required, StringLength(50)]
    public string Name { get; set; } = null!;
}
```

---
### Validation
```csharp
public class CreateCommandValidator
  : IValidator<CreateCommand>
{
    public (bool isValid, IEnumerable<ValidationResult> Errors) Validate(CreateCommand request)
    {
        ValidationContext context = new(request);
        var results = new List<ValidationResult>();
        
        bool isValid = Validator.TryValidateObject(
            instance: request,
            validationContext: context,
            validationResults: results,
            validateAllProperties: true);
        
        return (isValid, results);
    }
}
```

---
### Handler
```csharp
public class CreateCommandHandler
  : IRequestHandler<CreateCommand, bool>
{
  public async Task<Result> Handle(CreateCommand request, CancellationToken cancellationToken)
  {
    //do validations or something
    await Task.Delay(1_000, cancellationToken);
    return true;
  }
}
```
