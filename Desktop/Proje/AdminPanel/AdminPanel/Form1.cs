
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Newtonsoft.Json;
using System.IO;

using System.Threading;




using System.Net;
using System.Reflection.Emit;



namespace AdminPanel
{
    public partial class Form1 : Form
    {
        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "TSPvj8woDfK6iGb3Nv0ZZk5XvibwQJ66zqKXSoT7",
            BasePath = "https://yazlab1-328310-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        List<lokasyon> lokasyons = new List<lokasyon>();

        private EventStreamResponse listener;
        public Form1()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(fcon);
       Control.CheckForIllegalCrossThreadCalls = false;


            SetListener();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }

        async void SetListener()
        {

            //MessageBox.Show("furkan");



            listener = await client.OnAsync("Edges",
                     added: (sender, args, context) => { oku(); },
                     changed: (sender, args, context) => { oku(); },
                     removed: (sender, args, context) => { oku(); });


        }


        private void label2_Click(object sender, EventArgs e)
        {

        }
        public class EdgeFireBase
        {
            public string ID { get; set; }
            public bool OkunsunMu { get; set; }
            public bool kargocuMu { get; set; }
            public string lokx { get; set; }
            public string loky { get; set; }
            public string status { get; set; }
        }

        public class Root
        {
            public List<EdgeFireBase> Edges { get; set; }
        }

        public void oku()
        {
           dataGridView1.Rows.Clear();
            string adres = "https://yazlab1-328310-default-rtdb.firebaseio.com/.json";
            WebRequest istek = HttpWebRequest.Create(adres);
            WebResponse cevap;
            cevap = istek.GetResponse();

            StreamReader donenbilgiler = new StreamReader(cevap.GetResponseStream());
            string bilgilerial = donenbilgiler.ReadToEnd();

            Root myDeserializedClassFireBase = JsonConvert.DeserializeObject<Root>(bilgilerial);

            for (int t = 0; t < myDeserializedClassFireBase.Edges.Count; t++)
            {

                if (myDeserializedClassFireBase.Edges[t] == null || myDeserializedClassFireBase.Edges[t].OkunsunMu == false)
                {
                   
                }
                else
                {
                    dataGridView1.Rows.Add(myDeserializedClassFireBase.Edges[t].ID, myDeserializedClassFireBase.Edges[t].kargocuMu, myDeserializedClassFireBase.Edges[t].status);
                   


                }
           
            }
        }
        private async void button1_Click(object sender, EventArgs e)
        {
            lokasyon loknew = new lokasyon()
            {

                ID = textBox5.Text,
                lokx = textBox1.Text,
                loky = textBox2.Text,
               kargocuMu=false,
               OkunsunMu=true
              
             
            };
        
            var response =  await client.SetAsync("Edges/" + textBox5.Text + "/", loknew);
            MessageBox.Show("Lokasyon Girildi");
            //var response = await client.SetAsync("Edges/" + textBox1.Text + "/", loknew);
            // var setter = client.Set("Edges/"+textBox1.Text,loknew);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {

            lokasyon loknew = new lokasyon()
            {

                ID = textBox6.Text,
                lokx = "0",
                loky = "0",
                kargocuMu = false,
                OkunsunMu=false
                


            };

            var response = await client.UpdateAsync("Edges/" + textBox6.Text + "/", loknew);
            MessageBox.Show("Lokasyon Silindi");
            //var response = await client.SetAsync("Edges/" + textBox1.Text + "/", loknew);
            // var setter = client.Set("Edges/"+textBox1.Text,loknew);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void button3_Click(object sender, EventArgs e)
        {
            lokasyon loknew = new lokasyon()
            {

                ID = textBox5.Text,
                lokx = textBox1.Text,
                loky = textBox2.Text,
                kargocuMu = true,
                OkunsunMu = true


            };

            SetResponse response = await client.SetAsync("Edges/" + textBox5.Text + "/", loknew);
            MessageBox.Show("Lokasyon Girildi");
            //var response = await client.SetAsync("Edges/" + textBox1.Text + "/", loknew);
            // var setter = client.Set("Edges/"+textBox1.Text,loknew);
        }

        private  void button4_Click(object sender, EventArgs e)
        {
            
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
