using Core.Domain.Auth;
using Core.Domain.Common;
using Sieve.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Entities
{
    public class Staff : AuditableEntity
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Name { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Gender { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Address { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? PhoneNumber { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Email { get; set; }

        public string? Avatar { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? IdCard { get; set; }

        public string? IdCardImage { get; set; }



        [Sieve(CanFilter = true, CanSort = true)]
        public int? PositionId { get; set; }
        public StaffPosition? Position { get; set; }


        public virtual User? User { get; set; }
    }
}
