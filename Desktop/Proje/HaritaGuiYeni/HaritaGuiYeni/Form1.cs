

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Linq;
using Timer = System.Threading.Timer;

namespace HaritaGuiYeni
{
    public partial class Form1 : Form
    {

        Kruskal kruskal = new Kruskal();
        List<Edge> edges = new List<Edge>();
        List<lokasyon> lokasyons = new List<lokasyon>();

        IFirebaseConfig fcon = new FirebaseConfig()
        {
            AuthSecret = "TSPvj8woDfK6iGb3Nv0ZZk5XvibwQJ66zqKXSoT7",
            BasePath = "https://yazlab1-328310-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;
        private EventStreamResponse listener;
       
     
        public Form1()
        {
            client = new FireSharp.FirebaseClient(fcon);

            Control.CheckForIllegalCrossThreadCalls = false;


            InitializeComponent();
            
             SetListener();

          
        }
        
       
        
        private List<PointLatLng> points = new List<PointLatLng>();

        private void Form1_Load(object sender, EventArgs e)
        {
            
            Control.CheckForIllegalCrossThreadCalls = false;
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyCuHa6ysM83enPlNdAdbHUUFxIUClCDeuc";
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 100;
            gMapControl1.Zoom = 10;
            gMapControl1.ShowCenter = false;
        }

        GMapOverlay routes = new GMapOverlay("routes");
        GMapOverlay markers = new GMapOverlay("markers");



        async void SetListener()
        {

            //MessageBox.Show("furkan");



            listener = await client.OnAsync("Edges",
                     added: (sender, args, context) => { oku(); },
                     changed: (sender, args, context) => { oku(); },
                     removed: (sender, args, context) => { oku(); });
          

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



        private String DataBaseKargocuID;
        public void oku()
        {
             points.Clear();
             markers.Clear();
             lokasyons.Clear();
            gMapControl1.Overlays.Clear();
            routes.Clear();

            // MessageBox.Show("calıstım");
            string adres = "https://yazlab1-328310-default-rtdb.firebaseio.com/.json";
            WebRequest istek = HttpWebRequest.Create(adres);
            WebResponse cevap;
            cevap = istek.GetResponse();

            StreamReader donenbilgiler = new StreamReader(cevap.GetResponseStream());
            string bilgilerial = donenbilgiler.ReadToEnd();
           
            Root myDeserializedClassFireBase = JsonConvert.DeserializeObject<Root>(bilgilerial);
           
            // richTextBox1.Text = bilgilerial;
            /* for (int x = 0; x < myDeserializedClass.Edges.Count; x++)
             { MessageBox.Show(myDeserializedClass.Edges[x].lokx);
           MessageBox.Show(myDeserializedClass.Edges[x].loky);

             }*/
            //haritaya lokasyonlar eklıyoruz
            //  MessageBox.Show(myDeserializedClassFireBase.EdgesFirebase.Count.ToString());

            //   routes.Routes.Clear();
            //    MessageBox.Show("CALISIYOM");
            
            
            for (int t = 0; t < myDeserializedClassFireBase.Edges.Count; t++)
            {
              
                if (myDeserializedClassFireBase.Edges[t] == null || myDeserializedClassFireBase.Edges[t].OkunsunMu==false)
                {
                    //MessageBox.Show("saaaaaaaaaaaa");
                }
                else
                {
                    //MessageBox.Show(myDeserializedClassFireBase.Edges[t].lokx);
                    lokasyon lokasyonFireBase = new lokasyon();
                    lokasyonFireBase.x = Convert.ToDouble(myDeserializedClassFireBase.Edges[t].lokx);
                    lokasyonFireBase.y = Convert.ToDouble(myDeserializedClassFireBase.Edges[t].loky);
                    lokasyonFireBase.DataBaseID = myDeserializedClassFireBase.Edges[t].ID;
                    
                    lokasyons.Add(lokasyonFireBase);
                    if (myDeserializedClassFireBase.Edges[t].kargocuMu==false)
                    {
                        esitle(lokasyonFireBase.x, lokasyonFireBase.y,false);
                    }
                    else
                    {
                        DataBaseKargocuID = myDeserializedClassFireBase.Edges[t].ID;
                        esitle(lokasyonFireBase.x, lokasyonFireBase.y, true);
                    }
                   

                }
            



            }
              mainfonk();

        }

        public String formatla(String gelenx)
        {
            // , olan bir string google distance api çin . lı formata çevirir

            char[] x = gelenx.ToCharArray();

            for (int a = 0; a < x.Length; a++)
            {
                if (x[a] == ',')
                {
                    x[a] = '.';
                }

            }
            gelenx = new string(x);

            return gelenx;

        }
        
        public int mesafeBul(string destinationX, string destinationY, string originsX, string originsY)
        {



            string adres = "https://maps.googleapis.com/maps/api/distancematrix/json?destinations=" + destinationX + "%2C" + destinationY + "&origins=" + originsX + "%2C" + originsY + "&key=AIzaSyCuHa6ysM83enPlNdAdbHUUFxIUClCDeuc";
            WebRequest istek = HttpWebRequest.Create(adres);
            WebResponse cevap;
            cevap = istek.GetResponse();
            StreamReader donenBilgiler = new StreamReader(cevap.GetResponseStream());
            string bilgilerial = donenBilgiler.ReadToEnd();
            Root2 myDeserializedClass = JsonConvert.DeserializeObject<Root2>(bilgilerial);
            int mesafe = myDeserializedClass.rows[0].elements[0].distance.value;
            return mesafe;

        }

       List<Edge> kuruskalEdge = new List<Edge>();
        public void mainfonk()
        {
            edges.Clear();


            for (int a = 0; a < lokasyons.Count; a++)
            {
                for (int b = a + 1; b < lokasyons.Count; b++)
                {
                    string destinationX = formatla(Convert.ToString(lokasyons[a].x));
                    string destinationY = formatla(Convert.ToString(lokasyons[a].y));

                    string originsX = formatla(Convert.ToString(lokasyons[b].x));
                    string originsY = formatla(Convert.ToString(lokasyons[b].y));
                    /* MessageBox.Show(destinationX);
                     MessageBox.Show(destinationY);
                     MessageBox.Show(originsX);
                     MessageBox.Show(originsY);*/
                    int mesafe = mesafeBul(destinationX, destinationY, originsX, originsY);
                    // MessageBox.Show(mesafe.ToString());

                    Edge edge = new Edge();
                    edge.Source = a;
                    edge.SourceLok = lokasyons[a];
                    edge.DataBaseIdSourceLok = lokasyons[a].DataBaseID;
                    edge.Destination = b;
                    edge.DestinationLok = lokasyons[b];
                    edge.DataBaseIdDestinationLok = lokasyons[b].DataBaseID;
                    edge.Weight = mesafe;
                    edges.Add(edge);
                }
            }

           // List<Edge> kuruskalEdge = new List<Edge>();
            kuruskalEdge = kruskal.Kruskalmain(lokasyons.Count, edges.Count, edges.ToArray()).ToList();

          /*  for (int x=0;x< kuruskalEdge.Count()-1;x++)
            {
              //  MessageBox.Show(  "Destination ID: " +  kuruskalEdge[x].DataBaseIdDestinationLok + "  Source ID: " + kuruskalEdge[x].DataBaseIdSourceLok + "----" + kuruskalEdge[x].Weight);
            }*/
           




            
            yolciz(kuruskalEdge);




        }



        void yolciz(List<Edge> edges)
        {

           GMapProviders.GoogleMap.ApiKey = @"AIzaSyCuHa6ysM83enPlNdAdbHUUFxIUClCDeuc";
            for (int x = 0; x < edges.Count - 1; x++)
            {
                PointLatLng pointSource = new PointLatLng();
                pointSource.Lat = edges[x].SourceLok.x;
                pointSource.Lng = edges[x].SourceLok.y;
                PointLatLng pointDestination = new PointLatLng();
                pointDestination.Lat = edges[x].DestinationLok.x;
                pointDestination.Lng = edges[x].DestinationLok.y;

              
                

                var route = GoogleMapProvider.Instance.GetRoute(pointSource, pointDestination, false, false, 14);


                var r = new GMapRoute(route.Points, "My Route");

                routes.Routes.Add(r);
                gMapControl1.Overlays.Add(routes);
            }
            //BURAYA BAKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK
            //  listener.Dispose();
          
          //  routes.Clear();
            
        }

        void esitle(double x, double y,Boolean kargocumu)
        {
            if (kargocumu == false)
            {
                PointLatLng point = new PointLatLng(x, y);
                points.Add(point);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.purple_pushpin);

                //    GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                gMapControl1.Overlays.Add(markers);

            }
            else
            {
                PointLatLng point = new PointLatLng(x, y);
                points.Add(point);
                GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green_pushpin);

                //    GMapOverlay markers = new GMapOverlay("markers");
                markers.Markers.Add(marker);
                gMapControl1.Overlays.Add(markers);
            }
        }

        public class Distance
        {
            public string text { get; set; }
            public int value { get; set; }
        }



        public class Duration
        {
            public string text { get; set; }
            public int value { get; set; }
        }

        public class Element
        {
            public Distance distance { get; set; }
            public Duration duration { get; set; }
            public string status { get; set; }
        }

        public class Row
        {
            public List<Element> elements { get; set; }
        }

        public class Root2
        {
            public List<string> destination_addresses { get; set; }
            public List<string> origin_addresses { get; set; }
            public List<Row> rows { get; set; }
            public string status { get; set; }
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void gMapControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            var point = gMapControl1.FromLocalToLatLng(e.X, e.Y);
            double x = point.Lat;
            double y = point.Lng;

            
            textBox1.Text = x.ToString();
            textBox2.Text = y.ToString();
        }

        private void gMapControl1_Click(object sender, EventArgs e)
        {
            
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            EdgeFireBase loknew = new EdgeFireBase
            {
                ID = textBox3.Text,
                lokx = textBox1.Text,
                loky = textBox2.Text,
                kargocuMu = false,
                status="0",
                OkunsunMu=true
                
              
            };
            SetResponse response =  await client.SetAsync("Edges/" + textBox3.Text , loknew);
           
            MessageBox.Show("Lokasyon Girildi");
            //var response = await client.SetAsync("Edges/" + textBox1.Text + "/", loknew);
            // var setter = client.Set("Edges/"+textBox1.Text,loknew);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int minDistance = int.MaxValue;
            string index="Boş";
            lokasyon lokasyonUpdate = new lokasyon();

            for (int x = 0; x < kuruskalEdge.Count() - 1; x++)
            {
                if (kuruskalEdge[x].DataBaseIdDestinationLok == DataBaseKargocuID )
                {

                    if (kuruskalEdge[x].Weight < minDistance)
                    {
                        minDistance = kuruskalEdge[x].Weight;
                        index = kuruskalEdge[x].DataBaseIdSourceLok;
                        lokasyonUpdate = kuruskalEdge[x].SourceLok;
                    }

                }else if ( kuruskalEdge[x].DataBaseIdSourceLok == DataBaseKargocuID)
                {
                    if (kuruskalEdge[x].Weight < minDistance)
                    {
                        minDistance = kuruskalEdge[x].Weight;
                        //tam tersinı gonderelim cunku oraya gidecek 
                        index = kuruskalEdge[x].DataBaseIdDestinationLok;
                        lokasyonUpdate = kuruskalEdge[x].DestinationLok;



                    }



                }
            }
            if (kuruskalEdge.Count > 1)
            {
                Sil(DataBaseKargocuID, index, lokasyonUpdate);
            }
            else
            {
                MessageBox.Show("Tüm Kargolar Dağıtıldı");
            }
           
        }

        public class lokSave
        {
            public String ID { get; set; }
            public String lokx { get; set; }
            public String loky { get; set; }

            public String status { get; set; } = "0";
            public Boolean kargocuMu { get; set; }

            public Boolean OkunsunMu { get; set; }
        }

        private  void Sil(String ID2,String newID,lokasyon lokasyon)
        {

            lokSave loknew = new lokSave()
            {

                ID = ID2,
                lokx = "0",
                loky = "0",
                kargocuMu = false,
                OkunsunMu = false



            };

            lokSave loknewUpdate = new lokSave()
            {

                ID = newID,
                lokx = lokasyon.x.ToString(),
                loky = lokasyon.y.ToString(),
                kargocuMu = true,
                OkunsunMu = true



            };

            var response =  client.Update("Edges/" + ID2 + "/", loknew);
            var responseNew =  client.Update("Edges/" + newID + "/", loknewUpdate);

            MessageBox.Show("Lokasyon Silindi Yeni");
            


        }








        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 10000;
            timer1.Enabled = true;
        }
    }





































}



























