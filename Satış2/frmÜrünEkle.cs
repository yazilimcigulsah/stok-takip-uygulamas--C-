using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Satış2
{
    public partial class frmÜrünEkle : Form
    {
        public frmÜrünEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-0RHUDLC7\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        bool durum;
        private void barkodkontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString() || txtBarkodNo.Text=="")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void Kategorigetir()
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from KategoriBilgileri", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                comboKategori.Items.Add(reader["kategori"].ToString());
            }
            baglanti.Close();
        }
        private void FrmÜrünEkle_Load(object sender, EventArgs e)
        {
        Kategorigetir();
        }

        private void ComboKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboMarka.Items.Clear();
            comboMarka.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from MarkaBilgileri where kategori='"+comboKategori.SelectedItem+"'", baglanti);
            SqlDataReader reader = komut.ExecuteReader();
            while (reader.Read())
            {
                comboMarka.Items.Add(reader["marka"].ToString());
            }
            baglanti.Close();
        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            try
            {
                barkodkontrol();
                if (durum==true)
                {
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("insert into urun(barkodno,kategori,marka,urunadi,miktari,alisfiyati,satisfiyati,tarih) values(@barkodno,@kategori,@marka,@urunadi,@miktari,@alisfiyati,@satisfiyati,@tarih)", baglanti);
                    komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                    komut.Parameters.AddWithValue("@kategori", comboKategori.Text);
                    komut.Parameters.AddWithValue("@marka", comboMarka.Text);
                    komut.Parameters.AddWithValue("@urunadi", txtÜrünAdı.Text);
                    komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktarı.Text));
                    komut.Parameters.AddWithValue("@alisfiyati", double.Parse(txtAlışFiyatı.Text));
                    komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatışFiyatı.Text));
                    komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                    komut.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("Ürün eklendi");
                }
                else
                {
                    MessageBox.Show("Böyle bir barkodno var", "Uyarı");
                }
               
                foreach (Control item in groupBox1.Controls)
                {
                    if (item is TextBox)
                    {
                        item.Text = "";
                    }
                    if (item is ComboBox)
                    {
                        item.Text = "";
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BarkodNotxt_TextChanged(object sender, EventArgs e)
        {
            if (BarkodNotxt.Text == "")
            {
                lblMiktari.Text = "";
                foreach(Control item in groupBox2.Controls)
                {
                    if(item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like'"+BarkodNotxt.Text+"'",baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                Kategoritxt.Text = read["kategori"].ToString();
                Markatxt.Text = read["marka"].ToString();
                ÜrünAdıtxt.Text = read["urunadi"].ToString();
                lblMiktari.Text = read["miktari"].ToString();
                AlışFiyatıtxt.Text = read["alisfiyati"].ToString();
                SatışFiyatıtxt.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void btnVarOlanaEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktari=miktari+'" + int.Parse(Miktarıtxt.Text) + "' where barkodno='" + BarkodNotxt.Text + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var olan ürüne ekleme yapıldı");
        }
    }
}
