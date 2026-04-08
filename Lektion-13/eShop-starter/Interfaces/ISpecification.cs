using System.Linq.Expressions;

namespace eShop.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Predicate{get;}
}
