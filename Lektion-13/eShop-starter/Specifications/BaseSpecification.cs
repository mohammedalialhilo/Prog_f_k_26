using System.Linq.Expressions;
using eShop.Interfaces;

namespace eShop.Specifications;

public class BaseSpecification<T>(Expression<Func<T,bool>>? predicate) : ISpecification<T>
{
    protected BaseSpecification():this(null){}
    public Expression<Func<T, bool>>? Predicate => predicate;
}
