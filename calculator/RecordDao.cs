using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace calculator
{
    class RecordDao
    {

        public static void InsertTheRecord(RecordObj record)
        {
            MySqlConnection conn = null;
            try
            {
                string insertSql = "INSERT INTO cscalculator (Expression, Preorder, Postorder, Decim, Bina) VALUES (@Expression, @Preorder, @Postorder, @Decim, @Bina)";
                conn = GetSqlConnection();
                MySqlCommand cmd = new MySqlCommand(insertSql, conn);
                cmd.Parameters.AddWithValue("@Expression", record.Infix);
                cmd.Parameters.AddWithValue("@Preorder", record.Prefix);
                cmd.Parameters.AddWithValue("@Postorder", record.Postfix);
                cmd.Parameters.AddWithValue("@Decim", record.Deci);
                cmd.Parameters.AddWithValue("@Bina", record.Bin);
                int cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    System.Windows.MessageBox.Show("Expression inserts successfully");
                }
                else
                {
                    System.Windows.MessageBox.Show("Fail to insert the expression");
                }
            }
            catch (MySqlException mse)
            {
                if (mse.Source != null)
                    System.Windows.MessageBox.Show("MySqlException source: {0}", mse.Source);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public static bool CheckTheRecord(RecordObj record)
        {
            MySqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM cscalculator WHERE Expression = @Expression", conn);
                cmd.Parameters.AddWithValue("@Expression", record.Infix);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return true;
                }
                return false;
            }
            catch (MySqlException mse)
            {
                if (mse.Source != null)
                    System.Windows.MessageBox.Show("MySqlException source: {0}", mse.Source);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public static void DeleteTheRecord(RecordObj record)
        {
            MySqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM cscalculator WHERE Expression = '" + record.Infix + "'", conn);
                int cnt = cmd.ExecuteNonQuery();
                if (cnt > 0)
                {
                    System.Windows.MessageBox.Show(cnt + " records are deleted");
                }
                else
                {
                    System.Windows.MessageBox.Show("No records is deleted");
                }
            }
            catch (MySqlException mse)
            {
                if (mse.Source != null)
                    System.Windows.MessageBox.Show("MySqlException source: {0}", mse.Source);
                throw;
            }
            finally
            {
                conn.Close();
            }
        }
        public static List<RecordObj> QueryAllRecord()
        {
            List<RecordObj> records = new List<RecordObj>();
            MySqlConnection conn = null;
            try
            {
                conn = GetSqlConnection();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM cscalculator", conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                RecordObj record;
                while (reader.Read())
                {
                    record = new RecordObj(
                        reader.GetString("Expression"),
                        reader.GetString("Preorder"),
                        reader.GetString("Postorder"),
                        reader.GetString("Decim"),
                        reader.GetString("Bina")
                        );
                    records.Add(record);
                }
            }
            catch (MySqlException mse)
            {
                if (mse.Source != null)
                    Console.WriteLine("MySqlException source: {0}", mse.Source);
                throw;
            }
            finally
            {
                conn.Close();
            }
            return records;
        }
        public static MySqlConnection GetSqlConnection()
        {
            MySqlConnection conn = new MySqlConnection("server=localhost;user id=root;database=summerpractice;port=3308");
            conn.Open();
            return conn;
        }
    }
}
