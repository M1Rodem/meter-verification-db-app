using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Models
{
    public class Organization
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty; // название поверителя
        public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>(); // Список поверок выполненных этой организацией
        public override string ToString() => Name;
    }
}