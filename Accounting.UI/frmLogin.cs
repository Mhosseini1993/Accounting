using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ValidationComponents;
using Accounting.DataLayer.UnitOfWork;
using Accounting.UI.Forms;

namespace Accounting.UI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                using (UnitOfwork unit = new UnitOfwork())
                {
                    var IsExist = unit.UserRepository.GetAll(u => u.UserNaame == txtUserName.Text && u.PassWord == txtPass.Text).Any();

                    if (IsExist)
                    {
                        Form1 frm = new Form1();
                        frm.ShowDialog();
                        this.Hide();
                    }
                    else
                    {
                        RtlMessageBox.Show("رمز عبور یا نام کاربری اشتباه است");
                    }
                }
            }
        }
    }
}
