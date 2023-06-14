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
using ValidationComponents;
using Accounting.DataLayer.EF_Model;
using Accounting.Utilities.Utilities;

namespace Accounting.UI.Forms
{
    public partial class frmNewTransaction : Form
    {
        public int TransID = 0;
        public frmNewTransaction()
        {
            InitializeComponent();
        }

        private void frmNewTransaction_Load(object sender, EventArgs e)
        {
            BuildGridView();
            RefreshData();

            if (TransID != 0)
            {
                btnSave.Text = "ویرایش";

                using (UnitOfwork unit = new UnitOfwork())
                {
                    var account = unit.AccountRepository.GetByID(TransID);

                    if (account != null)
                    {
                        txtName.Text = account.Customer.FullName;

                        if (account.TypeID == 1)
                            rbnGet.Checked = true;
                        if (account.TypeID == 2)
                            rbnGive.Checked = true;

                        txtAmount.Text = account.Amount.ToString();

                        lblDateTime.Text = account.DateTime.Value.ToShamsi();

                        txtDesc.Text = account.Description;
                    }//if
                }//usnig
            }//if
            
        }
        public void RefreshData()
        {
            dgCustomer.DataSource = "";
            dgCustomer.AutoGenerateColumns = false;
            dgCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            using (UnitOfwork unit = new UnitOfwork())
            {
                dgCustomer.DataSource = unit.CustomerRepository.Getcustomernames();
            }

        }
        public void BuildGridView()
        {
            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.DataPropertyName = "ID";
            c1.Name = "";
            c1.HeaderText = "شماره";
            c1.Width = 10;
            c1.Visible = false;

            dgCustomer.Columns.Add(c1);

            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            c2.DataPropertyName = "FullName";
            c2.Name = "fullName";
            c2.HeaderText = "نام و نام خانوادگی";
            c2.Width = 40;

            dgCustomer.Columns.Add(c2);

        }
        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            using (UnitOfwork unit = new UnitOfwork())
            {

                dgCustomer.DataSource = unit.CustomerRepository.Getcustomernames(txtCustomerName.Text);
            }
        }

        private void dgCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgCustomer.CurrentRow != null)
            {
                txtName.Text = dgCustomer.CurrentRow.Cells[1].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfwork unit = new UnitOfwork())
                {
                    Account account = new Account();

                    account.Amount = Convert.ToInt32(txtAmount.Text);
                    account.Description = txtDesc.Text;
                    account.DateTime = DateTime.Now;
                    account.CustomerID = Convert.ToInt32(dgCustomer.CurrentRow.Cells[0].Value.ToString());

                    if (rbnGet.Checked)
                        account.TypeID = 1;
                    else if (rbnGive.Checked)
                        account.TypeID = 2;

                    if (TransID == 0)
                        unit.AccountRepository.Insert(account);

                    else
                    {
                        account.ID = TransID;
                        unit.AccountRepository.Update(account);
                    }

                    unit.Save();
                    DialogResult = DialogResult.OK;

                    RtlMessageBox.Show("تراکنش با موفقیت ذخیره شد");

                }//using

            }//ok
        }
    }
}
