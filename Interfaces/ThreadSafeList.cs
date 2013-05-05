using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Generated.Interfaces
{
    public interface IThreadSafeList<T> : IList<T>
    {
        ICollection<T> Values { get; }
    }
}
