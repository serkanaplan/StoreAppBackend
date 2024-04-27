using System.Dynamic;
using Store.Entities.Models;

namespace Store.Service.Contracts;
public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string? fieldsString);
    ExpandoObject ShapeData(T entity, string? fieldsString);
}