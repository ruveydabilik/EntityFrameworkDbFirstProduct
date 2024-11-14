using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityFrameworkDbFirstProduct
{
    public partial class FrmProduct : Form
    {
        public FrmProduct()
        {
            InitializeComponent();
        }

        static string connectionString = ConnectionStringHelper.GetConnectionString("DbProductEntities");
        DbProductEntities db = new DbProductEntities(connectionString);

        void ProductList()
        {
            dataGridView.DataSource = db.TblProduct.ToList();
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            ProductList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TblProduct tblProduct = new TblProduct();
            tblProduct.ProductPrice = decimal.Parse(txtProductPrice.Text);
            tblProduct.ProductName = txtProductName.Text;
            tblProduct.ProductStock = int.Parse(txtProductStock.Text);
            tblProduct.CategoryId = int.Parse(cmbProductCategory.SelectedValue.ToString());
            db.TblProduct.Add(tblProduct);
            db.SaveChanges();
            ProductList();
            txtProductPrice.Text = "";
            txtProductStock.Text = "";
            txtProductName.Text = "";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtProductId.Text);
            var value = db.TblProduct.Find(id);
            db.TblProduct.Remove(value);
            db.SaveChanges();
            txtProductId.Text = "";
            ProductList();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtProductId.Text);
            var value = db.TblProduct.Find(id);
            value.ProductPrice = decimal.Parse(txtProductPrice.Text);
            value.ProductName = txtProductName.Text;
            value.ProductStock = int.Parse(txtProductStock.Text);
            value.CategoryId = int.Parse(cmbProductCategory.SelectedValue.ToString());
            db.SaveChanges();
            txtProductName.Text = "";
            txtProductPrice.Text = "";
            txtProductStock.Text = "";
            txtProductId.Text = "";
            ProductList();
        }

        private void FrmProduct_Load(object sender, EventArgs e)
        {
            var values = db.TblCategory.ToList();
            cmbProductCategory.DisplayMember = "CategoryName";
            cmbProductCategory.ValueMember = "CategoryId";
            cmbProductCategory.DataSource = values;
        }

        private void btnProductListWithCategory_Click(object sender, EventArgs e)
        {
            var values = db.TblProduct.
                Join(db.TblCategory,
                product => product.CategoryId,
                category => category.CategoryId,
                (product,category) => new
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductStock = product.ProductStock,
                    ProductPrice = product.ProductPrice,
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                })
                .ToList();
            dataGridView.DataSource = values;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var values = db.TblProduct.Where(x=> x.ProductName == txtProductName.Text).ToList();
            dataGridView.DataSource = values;
        }
    }
}
