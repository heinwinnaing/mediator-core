using System.ComponentModel.DataAnnotations;

namespace MediatorCore.Interfaces;

public interface IValidator<T>
{
    (bool isValid, IEnumerable<ValidationResult> Errors) Validate(T request);
}
