using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Models
{
    public class MeasurementUnit
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; //месяц год день
        public string ShortName { get; set; } = string.Empty; // месяц год день
        public virtual ICollection<DeviceType> DeviceTypes { get; set; } = new List<DeviceType>();
    }
}
