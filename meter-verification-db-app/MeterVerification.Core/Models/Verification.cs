using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterVerification.Core.Models
{
    public class Verification
    {
        public int Id { get; set; }
        public string VersionId { get; set; } = string.Empty; // идентификатор из базы гост
        public string SerialNumber { get; set; } = string.Empty; // заводской номер счетчика
        public DateTime VerificationDate { get; set; } // когда поверили
        public DateTime ValidUntil { get; set; } // до какой даты действует
        public string CertificateNumber { get; set; } = string.Empty; // номер свидетельства
        public bool IsSuitable { get; set; } // пригоден да/нет
        public int OrganizationId { get; set; } //FK
        public int DeviceTypeId { get; set; } //FK
        public int DeviceModificationId { get; set; } //FK
        public virtual Organization Organization { get; set; } = null!;
        public virtual DeviceType DeviceType { get; set; } = null!;
        public virtual DeviceModification DeviceModification { get; set; } = null!;
        public bool IsValid => IsSuitable && ValidUntil >= DateTime.Today;
        public int DaysRemaining => (ValidUntil - DateTime.Today).Days;

        public string ValidityStatus
        {
            get
            {
                if (!IsSuitable) return "Непригоден";
                if (DaysRemaining < 0) return "Просрочен";
                if (DaysRemaining <= 30) return "Скоро истекает";
                return "Действителен";
            }
        }

        public override string ToString() => $"{CertificateNumber} - {SerialNumber}";
    }
}