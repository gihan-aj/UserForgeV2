using System.Collections.Generic;

namespace Application.Shared.Requesets
{
    public class BulkIdsRequest<T>
    {
        public BulkIdsRequest(IEnumerable<T> ids) 
        {
            Ids = ids ?? new List<T>();
        }

        public IEnumerable<T> Ids { get; }
    }
}
