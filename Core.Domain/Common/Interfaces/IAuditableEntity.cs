using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Common.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime? CreatedAt { get; set; }

        string? CreatedBy { get; set; }

        DateTime? UpdatedAt { get; set; }

        string? UpdatedBy { get; set; }

        bool? IsDeleted { get; set; }
    }
}
