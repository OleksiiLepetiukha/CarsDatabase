using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace BindingDataControls
{
    internal class DataBinding
    {
        public void LoadData(String connectionString, String sqlQuery, DataGridView dataGridView)
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                string passedQuery = sqlQuery;
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(passedQuery, connection);
                adapter.Fill(dt);
                dataGridView.DataSource = dt;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection unsuccessful" + ex.Message);
            }
        }
    }
}
