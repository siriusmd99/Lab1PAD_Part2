using Config;
using System;
using System.Threading;
using System.Windows.Forms;
using static Config.RabbitConfig;
using static Config.LogExtensions;

namespace Subscriber
{
    public partial class Form1 : Form
    {
        
        private int tooltip_lastX;
        private int tooltip_lastY;
        private string subreddit;
        private Subscriber subscriber;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            subreddit = radioButton1.Text;
            radioButton1.Checked = true;
            Thread thread = new Thread(SubscriberThread);
            thread.Start();
        }


        private void SubscriberThread()
        {
            StartConsumer();
        }

        private int StartConsumer()
        {

            subscriber = new Subscriber();

            if (!subscriber.Connect(RabbitHostname, RabbitPort, RabbitUsername, RabbitPassword))
                return LogLine($"Could not connect to Rabbit Broker ({RabbitHostname}) on Port: {RabbitPort}");

            if (!subscriber.CreateExchange(Exchange_Name))
                return LogLine($"Could not create exchange : {Exchange_Name} at Broker: {RabbitHostname}");

            if (!subscriber.NewQueue(Exchange_Name))
                return LogLine($"Could not create new queue on Exchange : {Exchange_Name} at Broker: {RabbitHostname}");

            if (!subscriber.BeginConsume(ConsumeCallback))
                return LogLine($"Could not create new queue on Exchange : {Exchange_Name} at Broker: {RabbitHostname}");


            return LogLine($"Consumer Started Succesfully on Queue : {subscriber.GetQueue()} , Exchange:  {Exchange_Name} , Broker: {RabbitHostname}");

        }

        public void ConsumeCallback(Meme meme)
        {
            UpdateMeme(meme);
        }

        private void UpdateMeme(Meme meme)
        {
            pictureBox2.Load(meme.url);
            pictureBox2.Tag = meme;
        }

        private void PictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            Meme meme = (Meme) pictureBox2.Tag;
            if(meme !=  null)
            {
                if (e.X != tooltip_lastX || e.Y != tooltip_lastY)
                {
                    toolTip1.SetToolTip(pictureBox2, $"Title: {meme.title}\nAuthor: {meme.author}");

                    tooltip_lastX = e.X;
                    tooltip_lastY = e.Y;
                }

            }
               
        }

        private void ChangeSubReddit(string subreddit)
        {
            this.subreddit = subreddit;
            subscriber.DropQueue();
            subscriber.NewQueue(subreddit);
            subscriber.BeginConsume(ConsumeCallback);
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && subreddit != radioButton1.Text)
                ChangeSubReddit(radioButton1.Text);
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && subreddit != radioButton2.Text)
                ChangeSubReddit(radioButton2.Text);
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && subreddit != radioButton3.Text)
                ChangeSubReddit(radioButton3.Text);
        }
    }
}
