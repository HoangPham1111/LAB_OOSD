using System;
using System.Data;
using QLNS.DongA.Data;

namespace QLNS.DongA.Services
{
    internal sealed class RecruitmentService
    {
        private readonly RecruitmentRepository repository = new RecruitmentRepository();
        private readonly LookupRepository lookups = new LookupRepository();

        public DataTable GetEmployees() { return lookups.GetEmployees(); }
        public DataTable GetDepartments() { return lookups.GetDepartments(); }
        public DataTable GetTrialDecisions() { return lookups.GetTrialDecisions(); }

        public string CreateTrialDecision(string employeeId, string departmentId, DateTime signDate,
            DateTime startDate, DateTime endDate, decimal trialSalary, string employerTerms, string candidateTerms)
        {
            string id = "QDT-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
            repository.CreateTrialDecision(id, employeeId, departmentId, signDate, startDate, endDate,
                trialSalary, employerTerms, candidateTerms);
            return id;
        }

        public string CreateFormalContract(string decisionId, string contractType, DateTime signDate,
            DateTime startDate, DateTime? endDate)
        {
            string id = "HD-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpperInvariant();
            repository.CreateFormalContract(id, decisionId, contractType, signDate, startDate, endDate);
            return id;
        }
    }
}
