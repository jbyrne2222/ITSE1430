/*
 * Jonathan Byrne
 * ITSE 1430
 * Section 3 classwork
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Nile.Data;
using Nile.Data.IO;
using Nile.Data.Memory;

namespace Nile.Windows
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad(e);

            _database = new FileProductDatabase("products.csv");

            RefreshUI();
        }

        //private void PlayingWithProductMembers ()
        //{ 
        //    var product = new Product();
        //
        //    Decimal.TryParse("123", out var price);
        //    product.Price = price;
        //
        //    var name = product.Name;
        //    //var name = product.GetName();
        //    product.Name = "Product A";
        //    product.Price = 50;
        //    product.IsDiscontinued = true;
        //
        //    //product.ActualPrice = 10;
        //    var price2 = product.ActualPrice;
        //
        //    //product.SetName("Product A");
        //    //product.Description = "None";
        //    var error = product.Validate();
        //
        //    var str = product.ToString();
        //
        //    var productB = new Product();
        //    //productB.Name = "Product B";
        //    //productB.SetName("Product B");
        //    //productB.Description = product.Description;
        //    error = productB.Validate();
        //
        //}

        #region Event Handlers

        private void OnProductAdd ( object sender, EventArgs e )
        {
            var button = sender as ToolStripMenuItem;

            var form = new ProductDetailForm("Add Product");          

            //Show form modally
            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            //Add to database
            //_database.Add(form.Product);
            try
            {
                _database.Add(null);
            } catch(NotImplementedException)
            {
                MessageBox.Show("not implemented yet");
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            };

            RefreshUI();
        }

        private void OnProductEdit( object sender, EventArgs e )
        {
            //Get selected product
            var product = GetSelectedProduct();
            if (product == null)
            {
                MessageBox.Show(this, "No product selected", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

                EditProduct(product);
        }

        private void EditProduct ( Product product )
        {
            var form = new ProductDetailForm(product);
            var result = form.ShowDialog(this);
            if (result != DialogResult.OK)
                return;

            //Update the product
            form.Product.Id = product.Id;
            try
            {
                _database.Update(form.Product);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            };

            RefreshUI();
        }

        private void OnProductRemove( object sender, EventArgs e )
        {
            //Get the selected product
            var product = GetSelectedProduct();
            if (product == null)
            {
                MessageBox.Show(this, "No product selected", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            };

            //var index = FindEmptyProductIndex() - 1;
            //if (index < 0)
            //   return;

            DeleteProduct(product);
        }

        private void DeleteProduct( Product product )
        {
            if (!ShowConfirmation("Are you sure?", "Remove Product"))
                return;

            //Remove product
            try
            {
                _database.Remove(product.Id);
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            };

            RefreshUI();
        }

        private void OnFileExit( object sender, EventArgs e )
        {
            Close();
        }

        private void OnHelpAbout( object sender, EventArgs e )
        {
            MessageBox.Show(this, "Not implemented", "Help About", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        #endregion

        private Product GetSelectedProduct ()
        {
            if (dataGridView1.SelectedRows.Count > 0)
                return dataGridView1.SelectedRows[0].DataBoundItem as Product;

            return null;
        }

        private void RefreshUI ()
        {
            //Get products
            IEnumerable<Product> products = null;
            try
            {
                products = _database.GetAll();
            } catch (Exception)
            {
                MessageBox.Show("Error loading products");
            };

            productBindingSource.DataSource = products?.ToList();
        }

        private bool ShowConfirmation ( string message, string title )
        {
            return MessageBox.Show(this, message, title,
               MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.Yes;
        }

        private IProductDatabase _database;

        private void OnCellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            var product = GetSelectedProduct();
            if (product == null)
                return;

            EditProduct(product);
        }

        private void OnCellKeyDown( object sender, KeyEventArgs e )
        {
            var product = GetSelectedProduct();
            if (product == null)
                return;

            if (e.KeyCode == Keys.Delete)                           
            {
                e.Handled = true;                   
                DeleteProduct(product);
            } else if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                EditProduct(product);
            };
        }
    }
}
