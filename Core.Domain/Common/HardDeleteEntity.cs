using Core.Domain.Common.Interfaces;

namespace Core.Domain.Common
{
    public abstract class HardDeleteEntity : IHardDeleteEntity
    {
        public int Id { get; set; } = default!;
    }
}
