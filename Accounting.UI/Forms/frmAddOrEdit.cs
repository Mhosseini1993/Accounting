using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.UnitOfWork;



namespace Accounting.UI.Forms
{
    public partial class frmAddOrEdit : Form
    {
        public frmAddOrEdit()
        {
            InitializeComponent();
        }

        private void frmAddOrEdit_Load(object sender, EventArgs e)
        {
            BuildGridView();
            RefreshData();
        }
        void RefreshData()
        {
            using (UnitOfwork unit = new UnitOfwork())
            {
                dgCustomers.DataSource = unit.CustomerRepository.GetAll();
            }
        }
        void BuildGridView()
        {
            dgCustomers.AutoGenerateColumns = false;
            dgCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.DataPropertyName = "ID";
            c1.Name = "ID";
            c1.HeaderText = "کد";
            c1.Width = 50;
            c1.Visible = false;
            dgCustomers.Columns.Add(c1);

            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            c2.DataPropertyName = "FullName";
            c2.Name = "FullName";
            c2.HeaderText = "نام و نام خانوادگی";
            c2.Width = 50;
            dgCustomers.Columns.Add(c2);

            DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
            c3.DataPropertyName = "Mobile";
            c3.Name = "Mobile";
            c3.HeaderText = "شماره تلفن";
            c3.Width = 50;
            dgCustomers.Columns.Add(c3);

            DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
            c4.DataPropertyName = "Email";
            c4.Name = "Email";
            c4.HeaderText = "ایمیل";
            c4.Width = 50;
            dgCustomers.Columns.Add(c4);

            DataGridViewTextBoxColumn c5 = new DataGridViewTextBoxColumn();
            c5.DataPropertyName = "Address";
            c5.Name = "Address";
            c5.HeaderText = "آدرس";
            c5.Width = 50;
            dgCustomers.Columns.Add(c5);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            RefreshData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfwork unit = new UnitOfwork())
            {
                dgCustomers.DataSource = unit.CustomerRepository.Search(txtSearch.Text.Trim());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                string fullname = dgCustomers.CurrentRow.Cells[1].Value.ToString();
                int id = Convert.ToInt32(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                DialogResult = RtlMessageBox.Show("آیا از حذف " + fullname + " مطمئن هستید ؟", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (DialogResult == DialogResult.Yes)
                {
                    using (UnitOfwork unit = new UnitOfwork())
                    {
                        if (unit.CustomerRepository.DeleteCustomer(id))
                            RtlMessageBox.Show("طرف حساب با موفقیت حذف شد");
                        RefreshData();
                    }
                }
            }
            else
            {
                MessageBox.Show("لطفا یک طرف حساب را برای حذف انتخاب کنید-");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmNewCustomer frm = new frmNewCustomer();
            frm.Text = "افزودن شخص جدید";
            if (frm.ShowDialog() == DialogResult.OK)
                RefreshData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgCustomers.CurrentRow != null)
            {
                int customerID = Convert.ToInt32(dgCustomers.CurrentRow.Cells[0].Value.ToString());
                frmNewCustomer frm = new frmNewCustomer();
                frm.CustomerID = customerID;
                frm.Text = "ویرایش مخاطب";
                if (frm.ShowDialog() == DialogResult.OK)
                    RefreshData();

            }
            else
            {
                RtlMessageBox.Show("برای ویرایش لطفا یک مشتری را انتخاب کنید");
            }
        }
    }
}
