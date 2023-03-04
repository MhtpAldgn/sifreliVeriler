using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.ComponentModel.Design;

namespace sifreliVeriler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-B2AV4A5\\SQLDEVELOPER;Initial Catalog=Test;Integrated Security=True");
        
        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from TBLVERILER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        string verisifrele(string metin)
        {
            byte[] veridizi = ASCIIEncoding.ASCII.GetBytes(metin);
            string sifreliveri = Convert.ToBase64String(veridizi);
            return sifreliveri;
        }
        string sifrecoz(string veri)
        {
            byte[] veridizi = Convert.FromBase64String(veri);
            string veriad = ASCIIEncoding.ASCII.GetString(veridizi);
            return veriad;
        }
        void datagridegosterme()
        {
            dataGridView1.DataSource = null;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "ID";
            dataGridView1.Columns[1].Name = "AD";
            dataGridView1.Columns[2].Name = "SOYAD";
            dataGridView1.Columns[3].Name = "MAIL";
            dataGridView1.Columns[4].Name = "SIFRE";
            dataGridView1.Columns[5].Name = "HESAP NO";

            baglanti.Open();
            SqlCommand kmt = new SqlCommand("select * from TBLVERILER", baglanti);
            SqlDataReader dr = kmt.ExecuteReader();
            while (dr.Read())
            {
                string ad = sifrecoz(dr[1].ToString());
                string soyad = sifrecoz(dr[2].ToString());
                string mail = sifrecoz(dr[3].ToString());
                string sifre = sifrecoz(dr[4].ToString());
                string hesap = sifrecoz(dr[5].ToString());

                string[] veri = { dr[0].ToString(), ad, soyad, mail, sifre, hesap };
                dataGridView1.Rows.Add(veri);

            }
            baglanti.Close();

        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("insert into TBLVERILER(AD,SOYAD,MAIL,SIFRE,HESAPNO) VALUES(@P1,@P2,@P3,@P4,@P5)", baglanti);
            kmt.Parameters.AddWithValue("@P1", verisifrele(txtAd.Text));
            kmt.Parameters.AddWithValue("@P2", verisifrele(txtSoyad.Text));
            kmt.Parameters.AddWithValue("@P3", verisifrele(txtMail.Text));
            kmt.Parameters.AddWithValue("@P4", verisifrele(txtSifre.Text));
            kmt.Parameters.AddWithValue("@P5", verisifrele(txtHesapNo.Text));
            kmt.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("EKLEME YAPILDI");
            listele();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            datagridegosterme();

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtSoyad.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtMail.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtSifre.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtHesapNo.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
        }
    }
}
