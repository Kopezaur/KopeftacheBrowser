using ScreenShotDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OsProject
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser1.Navigate(new Uri(comboBox1.SelectedItem.ToString()));
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void homeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoHome();
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            webBrowser1.GoHome();
            MessageBox.Show("                                       Welcome to Kopeftache! \n\n     With this browser, surfing the internet has just become easier!");
            webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(delegate (object sender1, WebBrowserProgressChangedEventArgs events)
            {
                if (((int)events.CurrentProgress > 0) && ((int)events.MaximumProgress >0))
                {
                    toolStripProgressBar1.Maximum = Convert.ToInt32((int)events.MaximumProgress);
                    toolStripProgressBar1.Step = Convert.ToInt32((int)events.CurrentProgress);
                    toolStripProgressBar1.PerformStep();
                } else
                {
                    toolStripProgressBar1.Value = (0);
                }
            });
        }

        private void comboBox1_MouseHover(object sender, EventArgs e)
        {
            //comboBox1
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }
        public Bitmap GenerateScreenshot(string url)
        {
            // This method gets a screenshot of the webpage
            // rendered at its full size (height and width)
            return GenerateScreenshot(url, -1, -1);
        }

        public Bitmap GenerateScreenshot(string url, int width, int height)
        {
            // Load the webpage into a WebBrowser control
            WebBrowser wb = new WebBrowser();
            wb.ScrollBarsEnabled = false;
            wb.ScriptErrorsSuppressed = true;
            wb.Navigate(url);
            while (wb.ReadyState != WebBrowserReadyState.Complete) { Application.DoEvents(); }


            // Set the size of the WebBrowser control
            wb.Width = width;
            wb.Height = height;

            if (width == -1)
            {
                // Take Screenshot of the web pages full width
                wb.Width = wb.Document.Body.ScrollRectangle.Width;
            }

            if (height == -1)
            {
                // Take Screenshot of the web pages full height
                wb.Height = wb.Document.Body.ScrollRectangle.Height;
            }

            // Get a Bitmap representation of the webpage as it's rendered in the WebBrowser control
            Bitmap bitmap = new Bitmap(wb.Width, wb.Height);
            wb.DrawToBitmap(bitmap, new Rectangle(0, 0, wb.Width, wb.Height));
            wb.Dispose();

            return bitmap;
        }
        //private Bitmap MyImage;
        private void printScreenButton_Click(object sender, EventArgs e)
        {
            //method 1
            //generate a screenshot
            //Bitmap pic= GenerateScreenshot("http://google.ro");
            //show it in a picturebox
            //pictureBox2.Image = pic;
            //pictureBox2.Visible = true;

            //method 2
            //ScreenCapture sc = new ScreenCapture();
            //// capture entire screen, and save it to a file
            //Image img = sc.CaptureScreen();
            //// display image in a Picture control named imageDisplay
            //pictureBox2.Image = img;
            //pictureBox2.Visible = true;
            //// capture this window, and save it
            //sc.CaptureWindowToFile(Handle, "C://temp2.gif", ImageFormat.Gif);

            //method 3
            var image = ScreenCapture.CaptureActiveWindow();
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.BorderStyle = BorderStyle.Fixed3D;
            pictureBox2.Image = image;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Visible = true;
            //try
            //{
            //    image.Save(@"C:\\temp\\snippetsource.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            //}
            //catch(Exception ex)
            //{
            //    //MessageBox.Show(ex.ToString());
            //}

            //method 4
            //Rectangle bounds = this.Bounds;
            //using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            //{
            //    using (Graphics g = Graphics.FromImage(bitmap))
            //    {
            //        g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            //    }
            //    bitmap.Save("C://test.jpg", ImageFormat.Jpeg);
            //}

            //method 5
            /*
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            MyImage = pic;
            pictureBox2.Visible = true;
            pictureBox2.BackgroundImage = MyImage;
            */
        }

        //event handler for the google search engine
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                webBrowser1.Navigate("http://" +"google.com/search?q=" + searchTextBox.Text);
            }
        }

        //hide pictureBox button
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = false;
        }

        //show pictureBox button
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
        }
        
        //method for saving the image
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image image = pictureBox2.Image;
            try
            {
                image.Save(@"C:\\temp\\snippetsource.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }

        //print and save the document as a PDF
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printDocument1.Print();
        }
        //event handler for the print option
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox2.Image, 0, 0);
        }

        //drawing in the picture box
        bool draw = false;
        int s = 3;
        Color color = Color.Red;

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            draw = true;
            Graphics g = Graphics.FromImage(pictureBox2.Image);
            Pen pen1 = new Pen(color, 4);
            g.DrawRectangle(pen1, e.X, e.Y, 2, 2);
            g.Save();
            pictureBox2.Image = pictureBox2.Image;
        }

        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            draw = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (draw)
            {
                Graphics g = Graphics.FromImage(pictureBox2.Image);
                SolidBrush brush = new SolidBrush(color);
                g.FillRectangle(brush, e.X, e.Y, s, s);
                g.Save();
                pictureBox2.Image = pictureBox2.Image;
            }
        }
    }
}
