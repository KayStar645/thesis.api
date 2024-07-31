using Core.Domain.Entities;

namespace Persistence.BusinessData.Common
{
    public static class CommonBusinessData
    {
       public static List<Type> ImmediateDeleteTypes = new List<Type> 
       { 
           typeof(Product),
       };
    }
}
