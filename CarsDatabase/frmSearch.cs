using BindingDataControls;
using SQLConstants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidateControls;

namespace CarsDatabase
{
    public partial class frmSearch : Form
    {
        string connectionString = SQLQueries.CONNSTRING;

        public frmSearch()
        {
            InitializeComponent();
           // dgvSearch.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvSearch_CellFormatting);

        }
        private void dgvSearch_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (e.ColumnIndex == dgvSearch.Columns["RentalPerDay"].Index && e.Value != null)
            //{
               
            //    decimal rentalPerDay = Convert.ToDecimal(e.Value);
            //    e.Value = $"€{rentalPerDay:N2}";
            //    e.FormattingApplied = true;

            //}
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            DataBinding dataBindingObj = new DataBinding();
            dataBindingObj.LoadData(connectionString, SQLQueries.SELECTALL, dgvSearch);
            
        }

        private void searchQuery()
        {
            String fieldName = (string)cboField.SelectedItem;
            String operatorQuery = (string)cboOperator.SelectedItem;
            String value = "";
            if (fieldName == "Available")
            {
                if (txtValue.Text == "yes")
                {
                    value = "1";
                }
                else if (txtValue.Text == "no")
                {
                    value = "0";
                }
            }
            else if(fieldName == "EngineSize")
            {
                value = txtValue.Text + "L";
            }
            else
            {
                value = txtValue.Text;
            }
            String searchCriteria = fieldName + " " + operatorQuery + " " + "'" + value + "'";
            String mainQuery = "Select * from tblcar where " + searchCriteria;
            DataBinding dataBindingObj = new DataBinding();
            dataBindingObj.LoadData(connectionString, mainQuery, dgvSearch);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (Validation.ValidateControl(cboField, "Field") &&
                 Validation.ValidateControl(cboOperator, "Operator") &&
                 Validation.ValidateControl(txtValue, "Value"))
            {
                searchQuery();
            }
        }

        private void cboField_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedField = cboField.SelectedItem as string;
            if (selectedField == "Make" || selectedField == "Available")
            {
                cboOperator.Enabled = false;
                cboOperator.SelectedItem = "=";
                if(selectedField == "Available")
                {
                    lblInfo.Text = "Enter Yes or No";
                }
            }
            else
            {
                cboOperator.Enabled = true;
                cboOperator.SelectedItem = null;
            }
        }

       
    }
}
