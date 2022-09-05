using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Satış2
{
    public partial class frmSatışListele : Form
    {
        public frmSatışListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-0RHUDLC7\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();

        private void satışlistele()
        {
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from satis ", baglanti);
            adtr.Fill(daset, "satis");
            dataGridView1.DataSource = daset.Tables["satis"];
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
        }
        private void Hesapla()
        {
           
        }
        private void frmSatışListele_Load(object sender, EventArgs e)
        {
            satışlistele();
        }
    }
}
