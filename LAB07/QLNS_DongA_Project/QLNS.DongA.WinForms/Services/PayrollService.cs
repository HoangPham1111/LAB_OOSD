using System;
using System.Data;
using QLNS.DongA.Data;

namespace QLNS.DongA.Services
{
    internal sealed class PayrollService
    {
        private readonly PayrollRepository repository = new PayrollRepository();
        public decimal StandardMonthlyHours { get { return ConnectionFactory.StandardMonthlyHours; } }

        public DataTable Generate(int month, int year)
        {
            string id = repository.Generate(month, year, StandardMonthlyHours);
            return repository.GetDetails(id);
        }
    }
}
