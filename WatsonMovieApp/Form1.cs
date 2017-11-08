using IBM.WatsonDeveloperCloud.Conversation.v1;
using IBM.WatsonDeveloperCloud.Conversation.v1.Model;
using Newtonsoft.Json;
using Nito.AsyncEx.Synchronous;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TMDbLib.Client;

namespace WatsonMovieApp
{
    public partial class Form1 : Form
    {

        ConversationService _conversation = new ConversationService();
        dynamic _context;
        int _counter;
        List<Movie> _movies = new List<Movie>();

        public Form1()
        {
            InitializeComponent();
            SetupWatson();
            SetupTMDB();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            

            if(_counter < 1)
            {
                _context = getMessageWithNoContext();
            }
            else
            {
                _context = getMessageWithContext();
            }

   
        }

        private dynamic getMessageWithNoContext ()
        {
            MessageRequest request = new MessageRequest()
            {
                Input = new InputData()
                {
                    Text = textBox1.Text
                },

            };

            updateRichTextBoxWithQuestion(textBox1.Text);

            //  send a message to the conversation instance
            var result = _conversation.Message("0d84c38d-11af-4579-92a1-4bce37774451", request);

            updateRichTextBox(result.Output.Text);

            _counter++;
            return result.Context;
        }

        private dynamic getMessageWithContext()
        {
            MessageRequest request = new MessageRequest()
            {
                Input = new InputData()
                {
                    Text = textBox1.Text
                },
                Context = _context

            };
            updateRichTextBoxWithQuestion(textBox1.Text);

            //  send a message to the conversation instance
            var result = _conversation.Message("0d84c38d-11af-4579-92a1-4bce37774451", request);
           
            updateRichTextBox(result.Output.Text);

            if(result.Output.Text[0] == "Let me check that for you.")
            {
                HashSet<Movie> temp = new HashSet<Movie>();

                foreach(var x in result.Entities)
                {
                    foreach(Movie m in _movies)
                    {
                        if(m.genres.Contains(x.Value))
                        {
                            temp.Add(m);
                        }
                    }
                }

                updateMovieImages(temp);
            }
 
            return result.Context;
        }

        private void updateMovieImages(HashSet<Movie> temp)
        {
            string baseURL = "http://image.tmdb.org/t/p/w154/";
            int count = 0;
            List<PictureBox> boxes = new List<PictureBox>();

            foreach(PictureBox p in this.Controls.OfType<PictureBox>())
            {
                boxes.Add(p);
            }

            foreach(Movie m in temp)
            {
                if (count == 4) break;

                boxes[count].LoadAsync(baseURL + m.posterPath);

                ToolTip tt = new ToolTip();
                tt.SetToolTip(boxes[count], buildToolTip(m));

                count++;
            }

        }

        private string buildToolTip(Movie m)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("TITLE: " + m.title);
            builder.AppendLine();
            builder.Append("RELEASE DATE: " + m.releaseDate);
            builder.AppendLine();
            builder.Append("OVERVIEW: " + m.overview);

            // Get a reference to the StringBuilder's buffer content.
            return builder.ToString();
        }

        private void updateRichTextBox (List<String> list)
        {
            foreach (String s in list)
            {

                richTextBox1.AppendText("\r\nWATSON: " + s);
            }
        }

        private void updateRichTextBoxWithQuestion(string s)
        {

                richTextBox1.AppendText("\r\nYou: " + s);
        }


        private void SetupTMDB()
        {
            TMDbClient client = new TMDbClient("030d813b34225f29e442115260143d25");


            var task = client.GetMovieUpcomingListAsync();
            var movies = task.WaitAndUnwrapException().Results;

            foreach(var x in movies)
            {
                Movie m = new Movie();
                m.title = x.Title;
                m.id = x.Id;
                m.releaseDate = x.ReleaseDate;
                m.overview = x.Overview;
                m.posterPath = x.PosterPath;
                m.genres = m.getGenres(x.GenreIds);

                _movies.Add(m);
            }
        }

        private void SetupWatson()
        {
            _conversation.VersionDate = "2017-05-26";
            _conversation.SetCredential("64ecb226-4262-46de-b46c-62d1dc331027", "JHD6vPCKQRDY");
            _context = null;
            _counter = 0;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
                textBox1.Text = "";
            }
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            MessageBox.Show("LOL");
        }
    }
    }

