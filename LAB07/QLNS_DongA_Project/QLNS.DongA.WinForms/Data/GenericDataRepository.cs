using System;
using System.Data;
using System.Data.SqlClient;

namespace QLNS.DongA.Data
{
    internal sealed class GenericDataRepository
    {
        public DataTable GetAll(string requestedTable)
        {
            string table = TableCatalog.Validate(requestedTable);
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT TOP (1000) * FROM dbo.[" + table + "]", connection))
            {
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                DataTable result = new DataTable(table);
                adapter.FillSchema(result, SchemaType.Source);
                adapter.Fill(result);
                return result;
            }
        }

        public int SaveChanges(string requestedTable, DataTable changes)
        {
            string table = TableCatalog.Validate(requestedTable);
            if (changes == null) throw new ArgumentNullException("changes");
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM dbo.[" + table + "]", connection))
            using (SqlCommandBuilder builder = new SqlCommandBuilder(adapter))
            {
                connection.Open();
                // DataTable da duoc nap schema va khoa chinh boi GetAll; khong FillSchema lan nua de tranh them constraint trung lap.
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.InsertCommand = builder.GetInsertCommand(true);
                adapter.UpdateCommand = builder.GetUpdateCommand(true);
                adapter.DeleteCommand = builder.GetDeleteCommand(true);
                adapter.AcceptChangesDuringUpdate = false;
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    adapter.InsertCommand.Transaction = transaction;
                    adapter.UpdateCommand.Transaction = transaction;
                    adapter.DeleteCommand.Transaction = transaction;
                    try
                    {
                        int affected = adapter.Update(changes);
                        transaction.Commit();
                        changes.AcceptChanges();
                        return affected;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = ConnectionFactory.Create())
            using (SqlCommand command = new SqlCommand(sql, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                if (parameters != null) command.Parameters.AddRange(parameters);
                DataTable result = new DataTable();
                adapter.Fill(result);
                return result;
            }
        }
    }
}
