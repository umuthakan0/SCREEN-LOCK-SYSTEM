using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace ekrankilit_denemesi
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        void hizala()
        {
            int width = (Screen.PrimaryScreen.Bounds.Width / 2) - (panel1.Size.Width / 2), height = (Screen.PrimaryScreen.Bounds.Height / 2) - (panel1.Size.Height / 2);
            panel1.Location = new Point(width, height);
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            hizala();
        }
        public static Int32 UserID;
        SqlConnection conn = new SqlConnection(DatabasePath.users_path);
        private void button2_Click(object sender, EventArgs e)
        {
            if (txttc.TextLength == 11 && txtadres.TextLength>0 && txtisim.TextLength > 0 && txtsoyisim.TextLength > 0 && maskedTextBox1.TextLength > 0)
            {
                UserID = Convert.ToInt16(txttc.Text.Substring(7, 4)) + Convert.ToInt32(DateTime.Now.ToString("dd"));
                try
                {
                    conn.Open();
                    SqlCommand add_usr_info = new SqlCommand("insert into usersinformations (UserID,TCIdentityNumber,Name,Surname,BirthDate,PhoneNumber,Adres) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7)", conn);
                    add_usr_info.Parameters.AddWithValue("@p1", UserID);
                    add_usr_info.Parameters.AddWithValue("@p2", txttc.Text);
                    add_usr_info.Parameters.AddWithValue("@p3", txtisim.Text.Substring(0,1).ToUpperInvariant()+txtisim.Text.Substring(1));
                    add_usr_info.Parameters.AddWithValue("@p4", txtsoyisim.Text.Substring(0, 1).ToUpperInvariant() + txtsoyisim.Text.Substring(1));
                    add_usr_info.Parameters.AddWithValue("@p5", dateTimePicker1.Value);
                    add_usr_info.Parameters.AddWithValue("@p6", maskedTextBox1.Text);
                    add_usr_info.Parameters.AddWithValue("@p7", txtadres.Text.ToUpper());
                    add_usr_info.ExecuteNonQuery();
                    SqlCommand add = new SqlCommand("insert into users (UserID,UserName,Password) values(@p1,@p2,@p3)", conn);
                    add.Parameters.AddWithValue("@p1", UserID);
                    add.Parameters.AddWithValue("@p2", textBox1.Text);
                    add.Parameters.AddWithValue("@p3", textBox2.Text);
                    add.ExecuteNonQuery();
                    MessageBox.Show("Kişi başarıyla kaydedildi...", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Veritabanı bağlantısında beklenmedik bir hata oluştu.. HATA: "+ex.Message, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Lütfen belirtilen yerlei doldurunuz..", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtsoyisim_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
