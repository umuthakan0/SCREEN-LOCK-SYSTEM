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

namespace ekrankilit_denemesi
{
    public partial class forgetmypassword : Form
    {
        public forgetmypassword()
        {
            InitializeComponent();
        }
        SQLiteConnection conn =new SQLiteConnection(DatabasePath.users_path);
        private void button2_Click(object sender, EventArgs e)
        {
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select * from users where UserName=@p1 and UserId=@p2", conn);
            cmd.Parameters.AddWithValue("@p1", textBox1.Text);
            cmd.Parameters.AddWithValue("@p2", textBox2.Text);
            SQLiteDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                MessageBox.Show("şifreniz: " + reader[2].ToString());
            }
            else
            {
                label3.Visible = true;
                label3.Text = "Yanlış Bilgi Girdiniz...";
            }
            reader.Close();
            conn.Close();
        }
    }
}
