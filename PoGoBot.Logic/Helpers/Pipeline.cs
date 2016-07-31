using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Interfaces;

namespace PoGoBot.Logic.Helpers
{
    public class Pipeline<T>
    {
        private readonly List<IFilter<T>> _filters = new List<IFilter<T>>();

        public void Register(IFilter<T> filter)
        {
            _filters.Add(filter);
        }

        public void Register(IEnumerable<IFilter<T>> filters)
        {
            _filters.AddRange(filters);
        }

        public T Execute(T input)
        {
            return _filters.Where(f => f.Enabled).Aggregate(input, (current, filter) => filter.Process(current));
        }
    }
}