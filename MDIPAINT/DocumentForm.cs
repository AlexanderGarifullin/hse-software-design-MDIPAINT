using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlTypes;

namespace MDIPAINT
{
    public partial class DocumentForm : Form
    {

        private int x, y;
        public static int cntStarBeams = 4;
        public static float radiiRatio = 0.5f;
        public bool wasChange = false;
        public bool isOpened = false;
        public Bitmap bitmap;
        public Bitmap tmp;
        public Graphics img;

        public Bitmap originalBitmap;

        public MainForm mainForm;
        public int WidhtImage;
        public int HeightImage;
        public Image Image;

        public float scale = 1f;

        public DocumentForm(MainForm mainForm)
        {
            Width = mainForm.WidthImage;
            Height = mainForm.HeightImage;
            HeightImage = Height;
            WidhtImage = Width;
 
            InitializeComponent();
            bitmap = new Bitmap(mainForm.WidthImage, mainForm.HeightImage);
            tmp = new Bitmap(mainForm.WidthImage, mainForm.HeightImage);
            originalBitmap = bitmap;
            img = Graphics.FromImage(bitmap);
            img.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Image = bitmap;
            img.Clear(Color.White);
            Text = $"Image {mainForm.count}";
            this.mainForm = mainForm;
   
            isOpened = false;
        }
        public DocumentForm(MainForm mainForm,  OpenFileDialog dlg)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            Text = dlg.FileName;
            Width = Image.FromFile(dlg.FileName).Width;
            Height = Image.FromFile(dlg.FileName).Height;
            HeightImage = Height;
            WidhtImage = Width;
            bitmap = new Bitmap(Image.FromFile(dlg.FileName));
            originalBitmap = bitmap;
            tmp = new Bitmap(Image.FromFile(dlg.FileName));
            img = Graphics.FromImage(bitmap);
            img.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            Image = bitmap;
            isOpened = true; 
        }
        private void DocumentForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                x = e.X; y = e.Y;
            }
        }

        private void DocumentForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                switch (mainForm.tools)
                {
                    case Tools.Pen:
                        wasChange = true;
                        Refresh();
                        var pen = new Pen(MainForm.Color, MainForm.penSize);
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        img = Graphics.FromImage(bitmap);
                        img.DrawLine(pen, x, y, e.X, e.Y);
                        x = e.X;
                        y = e.Y;
                        Invalidate();
                        break;
                    case Tools.Eraser:
                        wasChange = true;
                        Refresh();
                        img = Graphics.FromImage(bitmap);
                        var eraser = new Pen(Color.White, MainForm.penSize);
                        eraser.StartCap = LineCap.Round;
                        eraser.EndCap = LineCap.Round;
                        img.DrawLine(eraser, x, y, e.X, e.Y);
                        x = e.X;
                        y = e.Y;
                        Invalidate();
                        break;
                    case Tools.Line:
                        Refresh();
                        wasChange = true;
                        tmp = new Bitmap(bitmap.Width, bitmap.Height);
                        var modelLine = Graphics.FromImage(tmp);
                        modelLine.DrawLine(new Pen(MainForm.Color, MainForm.penSize), x, y, e.X, e.Y);          
                        Invalidate();
                        break;
                    case Tools.Ellipse:
                        Refresh();
                        wasChange = true;
                        tmp = new Bitmap(bitmap.Width, bitmap.Height);
                        var modelEllipse = Graphics.FromImage(tmp);
                        modelEllipse.DrawEllipse(new Pen(MainForm.Color, MainForm.penSize), x, y, e.X-x, e.Y-y);
                        Invalidate();
                        break;
                    case Tools.Star:
                        Refresh();
                        tmp = new Bitmap(bitmap.Width, bitmap.Height);
                        var modelStar = Graphics.FromImage(tmp);
                        modelStar.DrawLines(new Pen(MainForm.Color, MainForm.penSize), StarPoints(x, y, e.X, e.Y));
                        wasChange = true;
                        Invalidate();
                        break;
                }
                originalBitmap = bitmap;
            }
            mainForm.ChangeXY(e.X, e.Y);
        }
        
        private void DocumentForm_MouseUp(object sender, MouseEventArgs e)
        {
            
            switch (mainForm.tools)
            {
                case Tools.Line:
                    wasChange = true;
                    img = Graphics.FromImage(bitmap);
                    img.DrawLine(new Pen(MainForm.Color, MainForm.penSize), x, y, e.X, e.Y);
                    x = e.X;
                    y = e.Y;
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    break;
                case Tools.Ellipse:
                    wasChange = true;
                    img = Graphics.FromImage(bitmap);
                    img.DrawEllipse(new Pen(MainForm.Color, MainForm.penSize), x, y, e.X-x, e.Y-y);
                    x = e.X;
                    y = e.Y;
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    break;
                case Tools.Star:
                    wasChange = true;
                    img = Graphics.FromImage(bitmap);
                    img.DrawLines(new Pen(MainForm.Color, MainForm.penSize), StarPoints(x, y, e.X, e.Y));
                    tmp = new Bitmap(1, 1);
                    Invalidate();
                    break;
            }
            originalBitmap = bitmap;
        }

        private PointF[] StarPoints(int x, int y, int ex, int ey)
        {
            PointF[] points = new PointF[2 * cntStarBeams + 1];
            double r = Math.Sqrt((ex - x) * (ex - x) + (ey - y) * (ey - y));
            double R = r * radiiRatio;
            double x0 = x;
            double y0 = y;
            double a = 0;
            double da = Math.PI / cntStarBeams;
            double l;
            for (int i = 0; i < 2 * cntStarBeams + 1; i++)
            {
                l = i % 2 == 0 ? r : R;
                points[i] = new PointF((float)(x0 + l * Math.Cos(a)), (float)(y0 + l * Math.Sin(a)));
                a += da;
            }
            return points;

        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(bitmap, 0, 0);
            if (mainForm.tools == Tools.Line || mainForm.tools == Tools.Star || mainForm.tools == Tools.Ellipse)
                e.Graphics.DrawImage(tmp, 0, 0);
            originalBitmap = bitmap.Clone() as Bitmap;
        }

        private void DocumentForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wasChange)
            {
                var dialog = MessageBox.Show($"Вы хотите сохранить изменения в файле {Text}?", "MDIPAINT", MessageBoxButtons.YesNoCancel);
                switch (dialog)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        mainForm.сохранитьToolStripMenuItem_Click(sender, e);
                        break;
                    case DialogResult.No:
                        wasChange = false;
                        break;
                }
            }
        }

        private void DocumentForm_MouseLeave(object sender, EventArgs e)
        {
            mainForm.ChangeXY(0,0);
        }

        

        public void changeSize()
        {
            Bitmap tmp = (Bitmap)bitmap.Clone();
            bitmap = new Bitmap(WidhtImage, HeightImage);

            Width = mainForm.WidthImage;
            Height = mainForm.HeightImage;
        
            img = Graphics.FromImage(bitmap);
            
            img.Clear(Color.White);
            for (int x = 0; x < Math.Min(tmp.Width, bitmap.Width); x++)
            {
                for (int y = 0; y < Math.Min(tmp.Height, bitmap.Height);  y++)
                {
                    bitmap.SetPixel(x, y, tmp.GetPixel(x, y));
                }
            }
            Invalidate();
            wasChange = true;

        } //изменение размера холста


        public void ZoomIn()
        {
            if (scale != 0.125f)
            {
            
                if (scale > 1)
                {
                    scale--;
                }
                else
                {
                    scale /= 2;
                }
                Zoom();
            }
        }
        public void ZoomOut()
        {
            if (scale <= 6)
            {
        
                if (scale >= 1)
                {
                    scale++;
                }
                else
                {
                    scale *= 2;
                }
                Zoom();
            }
        }
    
        public void Zoom()
        {
            int new_widht = Convert.ToInt32(bitmap.Width * scale);  
            int new_height = Convert.ToInt32(bitmap.Height * scale);
            Bitmap bm = new Bitmap(bitmap, new_widht, new_height);
            img = Graphics.FromImage(bm);
            bitmap = bm;
            tmp = bm;
            img.InterpolationMode = InterpolationMode.HighQualityBicubic;  
        }
    }
}
