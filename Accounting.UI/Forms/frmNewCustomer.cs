using Accounting.DataLayer.EF_Model;
using Accounting.DataLayer.UnitOfWork;
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

namespace Accounting.UI.Forms
{
    public partial class frmNewCustomer : Form
    {
        public int CustomerID = -1;
        public frmNewCustomer()
        {
            InitializeComponent();
        }

        private void btnChooseImg_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string path = dialog.FileName;
                picCustomer.ImageLocation = path;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BaseValidator.IsFormValid(this.components))
            {
                if (CustomerID == -1)
                {
                    using (UnitOfwork unit = new UnitOfwork())
                    {
                        string imageName = string.Empty;
                        if (!string.IsNullOrEmpty(picCustomer.ImageLocation))
                        {
                            imageName = Guid.NewGuid().ToString().Substring(0, 10) + System.IO.Path.GetExtension(picCustomer.ImageLocation);
                            string Path = (Application.StartupPath + "/Images");
                            if (!System.IO.Directory.Exists(Path))
                            {
                                System.IO.Directory.CreateDirectory(Path);
                            }
                            picCustomer.Image.Save(Path + "/" + imageName);
                        }
                        else
                            imageName = "NoPhoto.jpg";

                        Customer customer = new Customer()
                        {
                            FullName = txtFullName.Text,
                            Mobile = txtMobile.Text,
                            Email = txtEmail.Text,
                            Address = txtAddress.Text,
                            ImageName = imageName
                        };
                        if (unit.CustomerRepository.InsertCustomer(customer))
                            RtlMessageBox.Show("طرف حساب با موفقیت اضافه شد");
                        DialogResult = DialogResult.OK;
                        this.Close();

                    }//using
                }
                else
                {
                    using (UnitOfwork unit = new UnitOfwork())
                    {
                        Customer customer = unit.CustomerRepository.GetByID(CustomerID);

                        string imageName = customer.ImageName;
                        if (imageName != "NoPhoto.jpg")
                        {
                            System.IO.File.Delete(Application.StartupPath + "/Images/" + imageName);
                        }//قبلا عکس داشته است و این عکس را حذف کن

                        customer.FullName = txtFullName.Text;
                        customer.Mobile = txtMobile.Text;
                        customer.Address = txtAddress.Text;
                        customer.Email = txtEmail.Text;
                        customer.ID = CustomerID;

                        if (!string.IsNullOrEmpty(picCustomer.ImageLocation))
                        {
                            customer.ImageName = imageName;
                            string Path = Application.StartupPath + "/Images";

                            if (!System.IO.Directory.Exists(Path))
                            {
                                System.IO.Directory.CreateDirectory(Path);
                            }
                            picCustomer.Image.Save(Path + "/" + imageName);

                        }//کاربر عکس جدید انتخاب کرده است
                        else
                        {
                            customer.ImageName = "NoPhoto.jpg";
                        }

                        if (unit.CustomerRepository.UpdateCustomer(customer))
                            RtlMessageBox.Show("مشتری با موفقیت بروز شد");
                        DialogResult = DialogResult.OK;
                        this.Close();


                    }//using
                }
                
            }//if
        }

        private void frmNewCustomer_Load(object sender, EventArgs e)
        {
            if (CustomerID != -1)
            {
                using (UnitOfwork unit = new UnitOfwork())
                {
                    Customer customer = unit.CustomerRepository.GetByID(CustomerID);

                    txtFullName.Text = customer.FullName;
                    txtMobile.Text = customer.Mobile;
                    txtAddress.Text = customer.Address;
                    txtEmail.Text = customer.Email;

                    picCustomer.ImageLocation = Application.StartupPath + "/Images/" + customer.ImageName;

                }//using
                btnSave.Text = "ویرایش";
            }
            else
            {
                txtFullName.Text = "";
                txtMobile.Text = "";
                txtAddress.Text = "";
                txtEmail.Text = "";
                picCustomer.ImageLocation = Application.StartupPath + "/Images/NoPhoto.jpg";
                btnSave.Text = "ثبت";
            }
        }
    }
}
