using System.Data;
using QLNS.DongA.Data;

namespace QLNS.DongA.Services
{
    internal sealed class GenericTableService
    {
        private readonly GenericDataRepository repository = new GenericDataRepository();
        public DataTable Load(string tableName) { return repository.GetAll(tableName); }
        public int Save(string tableName, DataTable table) { return repository.SaveChanges(tableName, table); }
    }
}
