using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace VehicleServiceBooking.App_Code
{
    /// <summary>
    /// Central helper for opening connections and running parameterized
    /// stored procedure commands. All data access in this project goes
    /// through here so no page ever concatenates SQL strings.
    /// </summary>
    public static class DbHelper
    {
        private static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["VehicleServiceDB"].ConnectionString; }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static DataTable ExecuteQuery(string procedureName, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }

        public static int ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalarInsert(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(procedureName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                con.Open();
                return cmd.ExecuteScalar();
            }
        }
    }
}
