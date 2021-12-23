using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminPanel
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        baglantı baglantı = new baglantı();
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                baglantı.client = new FireSharp.FirebaseClient(baglantı.config);
            }
            catch (Exception)
            {
                MessageBox.Show("İnternet YOK");
            }

        }
        private void Login()
        {
            
            
                
              
               
            
            
            
        }
        private void button1_Click(object sender, EventArgs e)
        {

            baglantı.client = new FireSharp.FirebaseClient(baglantı.config);
            baglantı.response = baglantı.client.Get("kullanıcılar/" + textBox1.Text);
          kullanici  user1 = null;
            user1 = baglantı.response.ResultAs<kullanici>();
            if (user1 == null)
            {

                MessageBox.Show("kullanıcı adı veya sifre yanlıs");
            }

         else  if (textBox1.Text == user1.userName && textBox2.Text == user1.password)
            {
                MessageBox.Show("giriş basarılı");
                Form1 form1 = new Form1();
                form1.Show();

            }
           
            else 
            {
                MessageBox.Show("kullanıcı adı veya sifre yanlıs");
                
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            kullanici user = new kullanici()
            {
                userName = textBox1.Text,
                password = textBox2.Text
            };

            var setter = baglantı.client.Set("kullanıcılar/" + textBox1.Text, user);
            MessageBox.Show("Kayıt yapıldı");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var setter = baglantı.client.Delete("kullanıcılar/" + textBox1.Text);
            MessageBox.Show("Kullanıcı Silindi");
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            kullanici user3 = new kullanici()
            {
                userName = textBox4.Text,
                password= textBox5.Text
            };

            var setter = baglantı.client.Set("kullanıcılar/" + textBox4.Text, user3);
            MessageBox.Show("Güncelleme Başarılı");
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
