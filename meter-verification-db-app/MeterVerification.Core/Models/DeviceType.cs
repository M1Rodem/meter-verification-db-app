
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Models
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty; // рег номер типа СИ
        public string TypeName { get; set; } = string.Empty; // полное название типа
        public string? Designation { get; set; } // обозначение
        public int? VerificationInterval { get; set; } // через сколько поверять
        public int? MeasurementUnitId { get; set; } // месяц/год
        public virtual MeasurementUnit? MeasurementUnit { get; set; }
        public virtual ICollection<DeviceModification> Modifications { get; set; } = new List<DeviceModification>();
        public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();
        public override string ToString() => $"{RegistrationNumber} - {TypeName}";
        public string VerificationIntervalText
        {
            get
            {
                if (!VerificationInterval.HasValue) return "не указано";

                return MeasurementUnit?.ShortName != null
                    ? $"{VerificationInterval} {MeasurementUnit.ShortName}"
                    : VerificationInterval.Value.ToString();
            }
        }
    }
}