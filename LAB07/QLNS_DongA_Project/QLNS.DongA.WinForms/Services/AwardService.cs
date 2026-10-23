using System;
using System.Data;
using QLNS.DongA.Data;

namespace QLNS.DongA.Services
{
    internal sealed class AwardService
    {
        private readonly AwardRepository repository = new AwardRepository();
        private readonly LookupRepository lookups = new LookupRepository();

        public DataTable GetEmployees() { return lookups.GetEmployees(); }
        public DataTable GetDepartments() { return lookups.GetDepartments(); }
        public DataTable GetAwardDecisions() { return lookups.GetAwardDecisions(); }

        public string CreateDraft(string targetType, string employeeId, string departmentId,
            DateTime date, string reason, string content, decimal amount)
        {
            if (String.IsNullOrWhiteSpace(reason)) throw new InvalidOperationException("Lý do khen thưởng không được để trống.");
            if (amount < 0) throw new InvalidOperationException("Số tiền khen thưởng không được âm.");
            if (targetType == "CaNhan" && String.IsNullOrWhiteSpace(employeeId)) throw new InvalidOperationException("Vui lòng chọn nhân viên.");
            if (targetType == "TapThe" && String.IsNullOrWhiteSpace(departmentId)) throw new InvalidOperationException("Vui lòng chọn phòng ban/tập thể.");
            string id = "KT-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
            repository.CreateDraft(id, date, targetType,
                targetType == "CaNhan" ? employeeId : null,
                targetType == "TapThe" ? departmentId : null,
                reason.Trim(), content, amount);
            return id;
        }

        public void Sign(string id, DateTime date) { repository.Sign(id, date); }

        public string CreatePayout(string awardId, DateTime date, string method)
        {
            string id = "PC-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
            repository.CreatePayout(id, awardId, date, method);
            return id;
        }
    }
}
