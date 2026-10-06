using System.Collections.Immutable;

namespace Lecture06.Demo;

internal class AgregatedExpenceRepository : IExpenseRepository
{
	private readonly ImmutableArray<IExpenseRepository> _expenseRepositories;

	public AgregatedExpenceRepository(params IExpenseRepository[] repositories)
	{
		foreach (var repository in repositories)
		{
			if (repository is AgregatedExpenceRepository)
			{
				throw new ArgumentException($"{nameof(AgregatedExpenceRepository)} can't use another {nameof(AgregatedExpenceRepository)}");
			}
		}

		_expenseRepositories = ImmutableArray.Create(repositories);
	}

	public void Add(Expense expense)
	{
		foreach (var r in _expenseRepositories)
		{
			r.Add(expense);
		}
	}

	public bool DeleteById(int id)
	{
		bool? result = null;
		foreach (var r in _expenseRepositories)
		{
			bool response = r.DeleteById(id);
			result ??= response;
		}

		return result ?? false;
	}

	public List<Expense> GetAll()
	{
		return _expenseRepositories[0].GetAll();
	}
}