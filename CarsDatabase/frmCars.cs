using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SQLConstants;
using ValidateControls;

namespace CarsDatabase
{
    public partial class frmCars : Form
    {
        private MySqlConnection connection;
        private string connectionString;
        private DataTable dt;
        private int currentRecordIndex = 0;
        public frmCars()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            LoadData();
        }

        private void frmCars_Load(object sender, EventArgs e)
        {

        }

        private void InitializeDatabaseConnection()
        {
            connectionString = SQLQueries.CONNSTRING;
            connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message);
            }

        }
        private void LoadData()
        {
            string query = SQLQueries.SELECTALL;
            MySqlCommand cmd = new MySqlCommand(query, connection);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            adapter.Fill(dt);
            ShowRecord(0);
        }

        private void ShowRecord(int index)
        {
            if (index >= 0 && index < dt.Rows.Count)
            {
                DataRow row = dt.Rows[index];
                txtRegNumber.Text = row["VehicleRegNo"].ToString();
                txtMake.Text = row["Make"].ToString();
                txtEngine.Text = row["EngineSize"].ToString();
                dtpRegisteredDate.Value = Convert.ToDateTime(row["DateRegistered"]);
                decimal RentalPerDay = Convert.ToDecimal(row["RentalPerDay"]);
                txtRentalCost.Text = $"€{RentalPerDay:N2}";
                cbAvaliable.Checked = Convert.ToBoolean(row["Available"]);
                txtRecordsCount.Text = $"{index + 1} of {dt.Rows.Count}";
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (frmSearch searchForm = new frmSearch())
            {
                searchForm.ShowDialog();
                this.Show();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            currentRecordIndex = 0;
            ShowRecord(currentRecordIndex);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentRecordIndex > 0)
            {
                currentRecordIndex--;
                ShowRecord(currentRecordIndex);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentRecordIndex < dt.Rows.Count - 1)
            {
                currentRecordIndex++;
                ShowRecord(currentRecordIndex);
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            currentRecordIndex = dt.Rows.Count - 1;
            ShowRecord(currentRecordIndex);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dt.RejectChanges();
            ShowRecord(currentRecordIndex);
        }

        private void UpdateData()
        {
            MySqlCommandBuilder builder = new MySqlCommandBuilder();
            builder.DataAdapter = new MySqlDataAdapter(SQLQueries.SELECTALL, connection);
            builder.DataAdapter.Update(dt);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(Validation.ValidateControl(txtRegNumber, "Registration Number") && 
                Validation.ValidateControl(txtMake, "Make") && 
                Validation.ValidateControl(txtEngine, "Engine Size") && 
                Validation.ValidateControl(txtRentalCost, "Rental per day"))
            {

            try
            {
                DataRow newRow = dt.NewRow();
                newRow["VehicleRegNo"] = txtRegNumber.Text;
                newRow["Make"] = txtMake.Text;
                newRow["EngineSize"] = txtEngine.Text;
                newRow["DateRegistered"] = dtpRegisteredDate.Value;
                newRow["RentalPerDay"] = Convert.ToDecimal(txtRentalCost.Text.Replace("€", "").Replace(",", ""));
                newRow["Available"] = cbAvaliable.Checked;
                dt.Rows.Add(newRow);
                UpdateData();
                currentRecordIndex = dt.Rows.Count - 1;
                ShowRecord(currentRecordIndex);
                lblFeedback.Text = "Record added successfully";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error message:" + ex.Message);
            }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            DataRow row = dt.Rows[currentRecordIndex];
            row["Make"] = txtMake.Text;
            row["EngineSize"] = txtEngine.Text;
            row["DateRegistered"] = dtpRegisteredDate.Value;
            row["RentalPerDay"] = Convert.ToDecimal(txtRentalCost.Text.Replace("€", "").Replace(",", ""));
            row["Available"] = cbAvaliable.Checked;
            UpdateData();
            lblFeedback.Text = "Record Updated successfully";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dt.Rows[currentRecordIndex].Delete();
            UpdateData();

            if (currentRecordIndex >= dt.Rows.Count)
            {
                currentRecordIndex = dt.Rows.Count - 1;
            }
            ShowRecord(currentRecordIndex);
            lblFeedback.Text = "Record deleted successfully";
        }
    }
}
