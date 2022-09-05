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
    public partial class frmSatış : Form
    {
        public frmSatış()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=LAPTOP-0RHUDLC7\\SQLEXPRESS;Initial Catalog=Stok_Takip;Integrated Security=True");
        DataSet daset = new DataSet();
        private void sepetlistele()
        {
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet ", baglanti);
            adtr.Fill(daset, "sepet");
            dataGridView1.DataSource = daset.Tables["sepet"];
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMüşteriListele listele = new frmMüşteriListele();
            listele.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmMüşteriEkle ekle = new frmMüşteriEkle();
            ekle.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmÜrünEkle ekle = new frmÜrünEkle();
            ekle.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmKategori kategori = new frmKategori();
            kategori.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmMarka marka = new frmMarka();
            marka.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            frmÜrünListele listele = new frmÜrünListele();
            listele.ShowDialog();
        }
        private void hesapla()
        {
            try
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyat) from sepet", baglanti);
                lblGenelToplam.Text = komut.ExecuteScalar() + "TL";
                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();
            }
            catch (Exception)
            {

                
            }
        }
        private void frmSatış_Load(object sender, EventArgs e)
        {
            sepetlistele();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelefon.Text = "";
            }
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from musteri where tc like '" + txtTc.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["adsoyad"].ToString();
                txtTelefon.Text = read["telefon"].ToString();
            }
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
        }

        private void txtBarkodNo_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodno like '" + txtBarkodNo.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtÜrünAdi.Text = read["urunadi"].ToString();
                txtSatisFiyati.Text = read["satisfiyati"].ToString();
            }
            if (baglanti.State != ConnectionState.Closed)
                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();
        }

        private void Temizle()
        {
            if (txtBarkodNo.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktari)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        bool durum;
        private int i;

        private void barkodkontrol()
        {
            durum = true;
            if (baglanti.State != ConnectionState.Open)
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from sepet", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text == read["barkodno"].ToString())
                {
                    durum = false;
                }
            }
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodkontrol();
            if (durum == true)
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into sepet(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyat,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunadi", txtÜrünAdi.Text);
                komut.Parameters.AddWithValue("@miktari", int.Parse(txtMiktari.Text));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@toplamfiyat", double.Parse(txtToplamFiyat.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();
            }
            else
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();
                SqlCommand komut2 = new SqlCommand("update sepet set  miktari=miktari+'" + int.Parse(txtMiktari.Text) + "' where barkodno='" + txtBarkodNo.Text + "'", baglanti);
                komut2.ExecuteNonQuery();
                SqlCommand komut3 = new SqlCommand("update sepet set toplamfiyat=miktari * satisfiyati where barkodno='" + txtBarkodNo.Text + "'", baglanti);
                komut3.ExecuteNonQuery();
                 if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
            }

            txtMiktari.Text = "1";
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktari)
                    {
                        item.Text = "";
                    }
                }

            }
        }

        private void txtMiktari_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void txtSatışFiyatı_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktari.Text) * double.Parse(txtSatisFiyati.Text)).ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void txtToplamFiyati_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodno='" + dataGridView1.CurrentRow.Cells["barkodno"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
            MessageBox.Show("Ürün sepetten çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();

        }

        private void btnSatışİptal_Click(object sender, EventArgs e)
        {
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet ", baglanti);
            komut.ExecuteNonQuery();
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
            MessageBox.Show("Ürünler sepetten çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            frmSatışListele listele = new frmSatışListele();
            listele.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnSatışYap_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++) ;
            {
                if (baglanti.State != ConnectionState.Open)
                    baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyat,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelefon.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", double.Parse(dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamfiyat", double.Parse(dataGridView1.Rows[i].Cells["toplamfiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "' where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "'", baglanti);
                komut2.ExecuteNonQuery();
                if (baglanti.State != ConnectionState.Closed)
                    baglanti.Close();

            }
            if (baglanti.State != ConnectionState.Open)
                baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet ", baglanti);
            komut3.ExecuteNonQuery();
            if (baglanti.State != ConnectionState.Closed)
                baglanti.Close();
            daset.Tables["sepet"].Clear();
            sepetlistele();
            hesapla();
        }
    }
}
