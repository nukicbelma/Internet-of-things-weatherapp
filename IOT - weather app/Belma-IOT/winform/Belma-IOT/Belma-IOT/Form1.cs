using FireSharp.Config;
using FireSharp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Belma_IOT
{
    public partial class Form1 : Form
    {
        public class dataFbase
        {
            public string Temperatura { get; set; }
            public String Vlaznost { get; set; }
        }

        public SerialPort myport;
        public Form1()
        {
            InitializeComponent();


            myport = new SerialPort();

            myport.PortName = "COM3";
            myport.BaudRate = 9600;
            myport.DtrEnable = true;
            myport.Open();

            myport.DataReceived += serialPort1_DataReceived;
            client = new FireSharp.FirebaseClient(config);
        }

        IFirebaseConfig config = new FirebaseConfig()
        {
            AuthSecret = "8hNPFHKfLluXQnvpaVkgFFZSMc4PuywZfumtlpOw",
            BasePath = "https://iot-belma-default-rtdb.europe-west1.firebasedatabase.app/"
        };

        IFirebaseClient client;

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string line = myport.ReadLine();
            this.BeginInvoke(new LineReceivedEvent(LineReceived), line);
        }

        private delegate void LineReceivedEvent(string line);
        private void LineReceived(string portporuka)
        {

            var fdata = new dataFbase();

            if (portporuka.Contains("temperature"))
            {
                int vrijednosttemperature = Int32.Parse(portporuka.Substring("temperature = ".Length, 2));

                lblTemp.Text = vrijednosttemperature.ToString() +"C";
                if(vrijednosttemperature>30) {
                    lblTemp.ForeColor = Color.Red;
                }
                else 
                    lblTemp.ForeColor = Color.Green;
            }
            if (portporuka.Contains("humidity"))
            {
                int vrijednostvlaznosti = Int32.Parse(portporuka.Substring("Current humidity = ".Length, 2));
                lblVlaznost.Text = vrijednostvlaznosti.ToString() + "%";
            }

            fdata.Temperatura = lblTemp.Text;
            fdata.Vlaznost = lblVlaznost.Text;
            //client.Push("/", fdata);
            client.UpdateAsync("", fdata);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
