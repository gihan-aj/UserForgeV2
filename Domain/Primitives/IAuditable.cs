using System;

namespace Domain.Primitives
{
    public interface IAuditable
    {
        public DateTime CreatedOn { get; }
        public string CreatedBy { get; }
        public DateTime? LastModifiedOn { get; }
        public string? LastModifiedBy { get; }
        public DateTime? DeletedOn { get; set; }
        public string? DeletedBy { get; set; }
    }
}
