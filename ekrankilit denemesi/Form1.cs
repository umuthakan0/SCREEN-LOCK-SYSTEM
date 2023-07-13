using Microsoft.Win32;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ekrankilit_denemesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //değişkenler..
        int failelelogin = 0, login_ottantication = 0, UserID;
        string name, surname, userıd, logıntıme;
        int sayac = 0;
        int minute = 0;
        SqlConnection conn = new SqlConnection(DatabasePath.users_path);
        bool ifade = true;
       
        private void no(object sender, FormClosingEventArgs e)
        {
            e.Cancel = ifade;
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            ifade = false;
            Application.Exit();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BackgroundImage=Image.FromFile(DatabasePath.form_bacgroundımage_path) ;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            yukluprogramlar();
            datetimelabel();
            datetimeconfig.Start();
            run();
            textBox1.Focus();
            hizala();
            conn.Open();
            lstboxlstlgn();
            conn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            giris();
        }
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            minute += 1;
            if (minute > 3)
            {
                panel1.Enabled = true;
                timer1.Stop();
            }
        }

        private void datetimeconfig_Tick(object sender, EventArgs e)
        {
            datetimelabel();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            sayac += 1;
            if (sayac >= 45)
            {
                this.Show();
                timer2.Stop();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            forgetmypassword frm=new forgetmypassword();
            frm.ShowDialog();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ifade = false;
            Application.Exit();
        }




         //----------------------------------------------------
        //METODLOAR..
       //-----------------------------------------------------
        void giris()
        {
            conn.Open();
            SqlCommand otantication_user = new SqlCommand("select * from adminstrator where UserName=@p1 and Password=@p2", conn);
            otantication_user.Parameters.AddWithValue("@p1", textBox1.Text);
            otantication_user.Parameters.AddWithValue("@p2", textBox2.Text);
            SqlDataReader read_user = otantication_user.ExecuteReader();
            if (read_user.Read())
            {
                Form2 admin_form = new Form2();
                admin_form.Show();
            }
            else
                login_ottantication = 1;
            read_user.Close();

            SqlCommand otantication = new SqlCommand("select * from users where UserName=@p1 and Password=@p2", conn);
            otantication.Parameters.AddWithValue("@p1", textBox1.Text);
            otantication.Parameters.AddWithValue("@p2", textBox2.Text);
            SqlDataReader read = otantication.ExecuteReader();
            if (read.Read())
            {
                UserID = Convert.ToInt32(read[0]);
                read.Close();
                lastlogin();
                this.Hide();
                timer2.Start();
                lstboxlstlgn();
            }
            else
            {
                read.Close();
                if (login_ottantication == 1)
                {
                    MessageBox.Show("Kullanıcıadı veya şifre hatalı..", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    login_ottantication = 0;
                }
                failelelogin += 1;
                if (failelelogin > 3)
                {
                    timer1.Start();
                    panel1.Enabled = false;
                    failelelogin = 0;
                    MessageBox.Show("giriş başarısız! Lütfen 3 dk. sonra tekrar deneyiniz...", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            conn.Close();
        }
        void yukluprogramlar()
        {
            conn.Open();
            SqlCommand look = new SqlCommand("select * from yukluprogramlar where PRG_ID = 1 and ProgramName = 'boş' and ProgramVersion = 'boş'", conn);
            SqlDataReader reader = look.ExecuteReader();
            if (reader.Read())
            {
                reader.Close();
                SqlCommand cmd_update = new SqlCommand("update yukluprogramlar set ProgramName='1' , ProgramVersion='1' where PRG_ID=1", conn);
                cmd_update.ExecuteNonQuery();
                MessageBox.Show("gorev tamamlandı..");
                string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
                using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey, false))
                {
                    int a = 2;
                    foreach (string skName in rk.GetSubKeyNames())
                    {
                        using (RegistryKey sk = rk.OpenSubKey(skName, false))
                        {
                            try
                            {
                                listBox1.Items.Add(sk.GetValue("DisplayName"));
                                SqlCommand cmd = new SqlCommand("INSERT INTO yukluprogramlar (ProgramName,ProgramVersion,P_InstallDate,InstallString,PRG_ID) values (@p1,@p2,@p3,@p4,@p5)", conn);
                                cmd.Parameters.AddWithValue("@p1", sk.GetValue("DisplayName"));
                                cmd.Parameters.AddWithValue("@p2", sk.GetValue("DisplayVersion"));
                                cmd.Parameters.AddWithValue("@p3", sk.GetValue("InstallDate"));
                                cmd.Parameters.AddWithValue("@p4", sk.GetValue("InstallSource"));
                                cmd.Parameters.AddWithValue("@p5", a);
                                cmd.ExecuteNonQuery();
                                a++;
                            }
                            catch (Exception ex)
                            {
                                //MessageBox.Show("Beklenmedik bir hata ile karşılaşıldı, Hata: "+ex.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            else
            {
                reader.Close();
            }
            conn.Close();
        }
        void run()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            key.SetValue("ApplicationSettings", "\"" + Application.ExecutablePath + "\"");
        }
        void hizala()
        {
            button1.Location = new Point(Screen.PrimaryScreen.Bounds.Width - button1.Size.Width - 5, 0);
            int width = (Screen.PrimaryScreen.Bounds.Width / 2) - (panel1.Size.Width / 2), height = (Screen.PrimaryScreen.Bounds.Height / 2) - (panel1.Size.Height / 2);
            panel1.Location = new Point(width, height);
        }
        void datetimelabel()
        {
            DateTime dateTime = DateTime.Now;
            label3.Text = dateTime.ToString("g");
        }
        void lastlogin()
        {
            SqlCommand lastlogınpersons = new SqlCommand("select * from usersinformations where UserID=@p1", conn);
            lastlogınpersons.Parameters.AddWithValue("@p1", UserID);
            SqlDataReader lastlog = lastlogınpersons.ExecuteReader();
            if (lastlog.Read())
            {
                name = lastlog[2].ToString();
                surname = lastlog[3].ToString();
                userıd = lastlog[0].ToString();
                logıntıme = DateTime.Now.ToString("G");
                lastlog.Close();
                SqlCommand cmd = new SqlCommand("insert into lastloginpersons (UserID,Name,Surname,LoginDate) values (@p1,@p2,@p3,@p4)", conn);
                cmd.Parameters.AddWithValue("@p2", name);
                cmd.Parameters.AddWithValue("@p1", userıd);
                cmd.Parameters.AddWithValue("@p3", surname);
                cmd.Parameters.AddWithValue("@p4", logıntıme);
                cmd.ExecuteNonQuery();
            }
        }
        void lstboxlstlgn()
        {
            SqlCommand lstboxlstlgndb = new SqlCommand("select Name,Surname,LoginDate from lastloginpersons   ", conn);
            SqlDataReader sqlDataReader = lstboxlstlgndb.ExecuteReader();
            while (sqlDataReader.Read())
            {
                listBox1.Items.Add(sqlDataReader[0].ToString() + " " + sqlDataReader[1].ToString() + "  " + sqlDataReader[2].ToString());
            }
        }
        
        
    }
}
