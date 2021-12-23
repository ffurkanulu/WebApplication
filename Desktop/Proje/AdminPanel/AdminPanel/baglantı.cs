using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel
{
    class baglantı
    {
        public IFirebaseConfig config = new FirebaseConfig
        {

            
             AuthSecret = "TSPvj8woDfK6iGb3Nv0ZZk5XvibwQJ66zqKXSoT7",
            BasePath = "https://yazlab1-328310-default-rtdb.firebaseio.com/"
        };

        public IFirebaseClient client;
        public FirebaseResponse response;
    }
}
