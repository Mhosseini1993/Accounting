using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accounting.UI.Forms;
using Accounting.Utilities.Utilities;
using Accounting.DataLayer.UnitOfWork;
using Accounting.BusinessLayer;

namespace Accounting.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmAddOrEdit frm = new frmAddOrEdit();
            frm.ShowDialog();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            var OutPut = CalculateAccountBalance.Calculate();

            lblGet.Text = OutPut.Get.ToString("0,#");
            lblPay.Text = OutPut.Pay.ToString("0,#");
            lblRemain.Text = OutPut.Remain.ToString("0,#");
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            frmNewTransaction frm = new frmNewTransaction();
            frm.ShowDialog();
        }

        private void btnReportPay_Click(object sender, EventArgs e)
        {
            frmReport frm = new frmReport();
            frm.Text = "'گزارش پرداختی ها";
            frm.TypeID = 2;
            frm.ShowDialog();
        }

        private void btnReportReceive_Click(object sender, EventArgs e)
        {
            frmReport frm = new frmReport();
            frm.Text = "'گزافرش دریافتی ها";
            frm.TypeID = 1;
            frm.ShowDialog();
        }

        private void btnReportPay_Click_1(object sender, EventArgs e)
        {
            frmReport frm = new frmReport();
            frm.Text = "'گزافرش دریافتی ها";
            frm.TypeID = 2;
            frm.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDate.Text = DateTime.Now.ToShamsi();
        }
    }
}
