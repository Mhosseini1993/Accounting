using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.DataLayer.EF_Model;
using Accounting.DataLayer.UnitOfWork;
using Accounting.DataLayer.GenericRepository;
using System.Linq.Expressions;
using Accounting.Utilities.Utilities;
using Accounting.ViewModel.CustomerViewModels;
using Stimulsoft.Report;

namespace Accounting.UI.Forms
{
    public partial class frmReport : Form
    {
        public int TypeID = 0;
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            BuildGrid();
            RefreshData();
        }

        private void RefreshData()
        {
            using (UnitOfwork unit = new UnitOfwork())
            {
                var Result1 = unit.AccountRepository.GetAll(t => t.TypeID == TypeID).ToList();

                var x = (from t in Result1 select new { t.ID, t.Customer.FullName, t.Amount, t.DateTime, t.Description }).ToList();

                dgTransactions.DataSource = null;
                dgTransactions.DataSource = x;


                List<ListCustomerViewModel> list1 = new List<ListCustomerViewModel>();
                list1.Add(new ListCustomerViewModel()
                {
                    FullName = "[انتخاب کنید]",
                    ID = 0
                });

                var Result2 = unit.CustomerRepository.Getcustomernames();

                list1.AddRange(Result2);

                cmbcustomers.DataSource = list1;
                cmbcustomers.DisplayMember = "FullName";
                cmbcustomers.ValueMember = "ID";

            }//using
        }

        public void BuildGrid()
        {

            dgTransactions.AutoGenerateColumns = false;
            dgTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewTextBoxColumn c1 = new DataGridViewTextBoxColumn();
            c1.DataPropertyName = "ID";
            c1.Name = "ID";
            c1.HeaderText = "کد";
            c1.Width = 10;
            c1.Visible = false;
            dgTransactions.Columns.Add(c1);


            DataGridViewTextBoxColumn c22 = new DataGridViewTextBoxColumn();
            c22.DataPropertyName = "FullName";
            c22.Name = "customertName";
            c22.HeaderText = "طرف حساب";
            c22.Width = 20;
            dgTransactions.Columns.Add(c22);

            DataGridViewTextBoxColumn c2 = new DataGridViewTextBoxColumn();
            c2.DataPropertyName = "Amount";
            c2.Name = "Amount";
            c2.HeaderText = "مبلغ";
            c2.DefaultCellStyle.Format = "0,#";
            c2.Width = 20;
            dgTransactions.Columns.Add(c2);

            DataGridViewTextBoxColumn c3 = new DataGridViewTextBoxColumn();
            c3.DataPropertyName = "DateTime";
            c3.Name = "DateTime";
            c3.DefaultCellStyle.Format = "yyyy/dd/MM";
            c3.HeaderText = "تاریخ تراکنش";
            c3.Width = 20;
            dgTransactions.Columns.Add(c3);

            DataGridViewTextBoxColumn c4 = new DataGridViewTextBoxColumn();
            c4.DataPropertyName = "Description";
            c4.Name = "Description";
            c4.HeaderText = "توضیخات";
            c4.Width = 20;
            dgTransactions.Columns.Add(c4);

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgTransactions.CurrentRow != null)
            {
                int TransID = Convert.ToInt32(dgTransactions.CurrentRow.Cells[0].Value.ToString());
                if (RtlMessageBox.Show("آیا از حذف این تراکنش مطمئن هستید", "هشدار", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (UnitOfwork unit = new UnitOfwork())
                    {
                        unit.AccountRepository.Delete(TransID);
                        unit.Save();
                        RtlMessageBox.Show("تراکنش با موفقیت حذف شد");
                        RefreshData();
                    }
                }
            }
            else
            {
                RtlMessageBox.Show("برای حذف لطفا یک تراکنش را مشخص کیند-");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgTransactions.CurrentRow != null)
            {
                int TransID = Convert.ToInt32(dgTransactions.CurrentRow.Cells[0].Value.ToString());
                frmNewTransaction frm = new frmNewTransaction();
                frm.TransID = TransID;
                frm.Text = "ویرایش تراکنش";

                if (frm.ShowDialog() == DialogResult.OK)
                    RefreshData();
            }
            else
            {
                RtlMessageBox.Show("برای ویرایش لطفا یک تراکنش را انتخاب کنید");
            }
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {

            using (UnitOfwork unit = new UnitOfwork())
            {
                DateTime FromDate = new DateTime();
                DateTime ToDate = new DateTime();




                var Result1 = unit.AccountRepository.GetAll(t => t.TypeID == TypeID).ToList();

                if (cmbcustomers.SelectedIndex != 0)
                    Result1 = Result1.Where(t => t.Customer.FullName.Contains(cmbcustomers.Text)).ToList();


                if (txtfromdate.Text != "    /  /")
                {
                    FromDate = Convert.ToDateTime(txtfromdate.Text).ToMiladi();
                    Result1 = Result1.Where(t => t.DateTime >= FromDate).ToList();
                }
                if (txttodate.Text != "    /  /")
                {
                    ToDate = Convert.ToDateTime(txttodate.Text).ToMiladi();
                    Result1 = Result1.Where(t => t.DateTime <= ToDate).ToList();
                }
                
                var x = (from t in Result1 select new { t.ID, t.Customer.FullName, t.Amount, t.DateTime, t.Description }).ToList();

                dgTransactions.DataSource = null;
                dgTransactions.DataSource = x;

            }//using
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtReport = new DataTable();

            dtReport.Columns.Add("Customer", typeof(string));
            dtReport.Columns.Add("Amount", typeof(double));
            dtReport.Columns.Add("Date", typeof(string));
            dtReport.Columns.Add("Des", typeof(string));

            foreach (DataGridViewRow item in dgTransactions.Rows)
            {
                dtReport.Rows.Add(dtReport.NewRow());

                dtReport.Rows[dtReport.Rows.Count - 1]["Customer"] = item.Cells[1].Value.ToString();
                dtReport.Rows[dtReport.Rows.Count - 1]["Amount"] = item.Cells[2].Value.ToString();
                dtReport.Rows[dtReport.Rows.Count - 1]["Date"] = item.Cells[3].Value.ToString();
                dtReport.Rows[dtReport.Rows.Count - 1]["Des"] = item.Cells[4].Value.ToString();
            }

            stiReport1.RegData("DT", dtReport);
            stiReport1.Load(Application.StartupPath + "/Report.mrt");
            stiReport1.Show();
        }
    }
}