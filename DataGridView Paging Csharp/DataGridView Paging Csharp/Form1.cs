using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DataGridView_Paging_Csharp
{

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection connection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=ki.accdb");
        OleDbCommand komut = new OleDbCommand();
        DataSet ds = new DataSet();

        int MaxObj, RowCount, ResidualNumber, LastPageNumber, PageNumber, Start = 0;

        void list()
        {

            ds.Clear();
            OleDbDataAdapter adapter = new OleDbDataAdapter("SELECT * FROM ki", connection);
            if (numericUpDown1.Value > 0)
            {
                DataTable dt = new DataTable();
                adapter.Fill(ds, Start, MaxObj, "ki");
                dataGridView1.DataSource = ds.Tables[0];
            }
            else
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
                RowCount = dataGridView1.RowCount - 1;
            }
            connection.Close();
        }

        void connect()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            else
            {
                connection.Close();
                connection.Open();
            }
        }

        void ReadPageNumber()
        {

            if (ResidualNumber == 0 || RowCount <= MaxObj)
            {
                TextPageNumber.Text = PageNumber + " / " + LastPageNumber;

            }
            else
            {
                TextPageNumber.Text = PageNumber + " / " + (LastPageNumber + 1);
            }
        }

        private void FistPage_Click(object sender, EventArgs e)
        {
            PageNumber = 1;
            ReadPageNumber();
            Start = 0;
            Back.Enabled = false;
            FistPage.Enabled = false;
            Next.Enabled = true;
            LastPage.Enabled = true;

            list();
        }

        private void Back_Click(object sender, EventArgs e)
        {
            --PageNumber;
            ReadPageNumber();
            Start = Start - MaxObj;
            Next.Enabled = true;
            LastPage.Enabled = true;

            if (Start <= 0)
            {
                Start = 0;
                Back.Enabled = false;
                FistPage.Enabled = false;
            }

            list();

        }

        private void Next_Click(object sender, EventArgs e)
        {
            ++PageNumber;
            ReadPageNumber();
            Start = Start + MaxObj;
            Back.Enabled = true;
            FistPage.Enabled = true;

            if (Start >= RowCount || RowCount <= Start + MaxObj)
            {
                Next.Enabled = false;
                LastPage.Enabled = false;
            }

            list();

        }

        private void LastPage_Click(object sender, EventArgs e)
        {
            if (ResidualNumber == 0)
            {
                Start = RowCount - MaxObj;
                PageNumber = LastPageNumber;
            }
            else
            {
                Start = RowCount - ResidualNumber;
                PageNumber = LastPageNumber + 1;
            }

            ReadPageNumber();

            Back.Enabled = true;
            FistPage.Enabled = true;
            Next.Enabled = false;
            LastPage.Enabled = false;

            list();

        }

        private void ShowAll_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            TextPageNumber.Text = "1 / 1";
            connect();
            list();
            connection.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            MaxObj = (int)numericUpDown1.Value;

            if (MaxObj >= RowCount)
            {
                numericUpDown1.Value = 0;
            }

            PageNumber = 1;

            if (MaxObj > 0)
            {
                LastPageNumber = RowCount / MaxObj;
                ResidualNumber = RowCount % MaxObj;

                Start = 0;
                ReadPageNumber();

                ShowAll.Enabled = true;
                Back.Enabled = false;
                FistPage.Enabled = false;
                Next.Enabled = true;
                LastPage.Enabled = true;
            }
            else
            {
                Start = 0;
                ShowAll.Enabled = false;
                Back.Enabled = false;
                FistPage.Enabled = false;
                Next.Enabled = false;
                LastPage.Enabled = false;
                TextPageNumber.Text = "1 / 1";
            }

            list();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                list();
                ShowAll.Enabled = false;
                Back.Enabled = false;
                FistPage.Enabled = false;
                Next.Enabled = false;
                LastPage.Enabled = false;
                TextPageNumber.Text = "1 / 1";
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
                Application.Exit();
            }
        }
    }
}