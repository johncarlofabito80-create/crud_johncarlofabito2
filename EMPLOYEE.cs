using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace EMPLOYEE_MANAGEMENT_SYSTEM
{
    public partial class EMPLOYEE : Form
    {
        MySqlConnection con = new MySqlConnection("server=localhost;database=employee_db;uid=root;pwd=12345678;");

        public EMPLOYEE()
        {
            InitializeComponent();
            DisplayEmployees();
        }

        private void DisplayEmployees()
        {
            try
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM employees", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvEmployees.DataSource = dt;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employees: " + ex.Message);
                con.Close();
            }
        }

        private void ClearFields()
        {
            txtID.Clear();
            txtName.Clear();
            txtPosition.Clear();
            txtDepartment.Clear();
            txtSalary.Clear();
        }

        // ADD EMPLOYEE
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "" || txtPosition.Text == "" || txtDepartment.Text == "" || txtSalary.Text == "")
            {
                MessageBox.Show("Please fill all fields before adding.", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.Open();
                string query = "INSERT INTO employees (name, position, department, salary) VALUES (@name, @position, @department, @salary)";
                MySqlCommand cmd = new MySqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@position", txtPosition.Text);
                cmd.Parameters.AddWithValue("@department", txtDepartment.Text);
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtSalary.Text));

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Employee added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DisplayEmployees();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding employee: " + ex.Message);
                con.Close();
            }
        }

        // POPULATE TEXTBOXES ON ROW CLICK
        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = dgvEmployees.Rows[e.RowIndex].Cells["id"].Value.ToString();
                txtName.Text = dgvEmployees.Rows[e.RowIndex].Cells["name"].Value.ToString();
                txtPosition.Text = dgvEmployees.Rows[e.RowIndex].Cells["position"].Value.ToString();
                txtDepartment.Text = dgvEmployees.Rows[e.RowIndex].Cells["department"].Value.ToString();
                txtSalary.Text = dgvEmployees.Rows[e.RowIndex].Cells["salary"].Value.ToString();
            }
        }

        // UPDATE EMPLOYEE
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Please select an employee to update.");
                return;
            }

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "UPDATE employees SET name=@name, position=@position, department=@department, salary=@salary WHERE id=@id", con);

                cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                cmd.Parameters.AddWithValue("@name", txtName.Text);
                cmd.Parameters.AddWithValue("@position", txtPosition.Text);
                cmd.Parameters.AddWithValue("@department", txtDepartment.Text);
                cmd.Parameters.AddWithValue("@salary", decimal.Parse(txtSalary.Text));

                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Employee updated successfully!");
                DisplayEmployees();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating employee: " + ex.Message);
                con.Close();
            }
        }

        // DELETE EMPLOYEE
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtID.Text == "")
            {
                MessageBox.Show("Please select an employee to delete.");
                return;
            }

            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM employees WHERE id=@id", con);
                cmd.Parameters.AddWithValue("@id", int.Parse(txtID.Text));
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Employee deleted successfully!");
                DisplayEmployees();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting employee: " + ex.Message);
                con.Close();
            }
        }

        // READ / REFRESH
        private void btnRead_Click(object sender, EventArgs e)
        {
            DisplayEmployees();
            MessageBox.Show("Employee records loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
