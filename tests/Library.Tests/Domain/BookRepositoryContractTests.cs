using System.Reflection;
using Library.Domain.Entities;
using Library.Domain.Repositories;

namespace Library.Tests.Domain;

public class BookRepositoryContractTests
{
    [Fact]
    public void IBookRepository_DefinesExpectedQueries()
    {
        AssertQuery(nameof(IBookRepository.GetAllAsync), typeof(Task<IReadOnlyList<Book>>));
        AssertQuery(nameof(IBookRepository.GetByIdAsync), typeof(Task<Book>), typeof(int));
        AssertQuery(nameof(IBookRepository.GetByCategoryAsync), typeof(Task<IReadOnlyList<Book>>), typeof(int));
    }

    [Fact]
    public void DomainAssembly_DoesNotReferenceEntityFrameworkCore()
    {
        var referenced = typeof(IBookRepository).Assembly
            .GetReferencedAssemblies()
            .Select(a => a.Name ?? string.Empty);

        Assert.DoesNotContain(referenced, name => name.StartsWith("Microsoft.EntityFrameworkCore"));
    }

    private static void AssertQuery(string methodName, Type returnType, params Type[] parameterTypes)
    {
        var method = typeof(IBookRepository).GetMethod(methodName);

        Assert.NotNull(method);
        Assert.Equal(returnType, method.ReturnType);

        var parameters = method.GetParameters();
        Assert.Equal(parameterTypes.Length + 1, parameters.Length);
        Assert.Equal(parameterTypes, parameters.Take(parameterTypes.Length).Select(p => p.ParameterType));
        Assert.Equal(typeof(CancellationToken), parameters[^1].ParameterType);
        Assert.True(parameters[^1].HasDefaultValue);
    }
}
