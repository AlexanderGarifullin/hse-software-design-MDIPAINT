using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPAINT
{
    public partial class MainForm : Form
    {
        public  Dictionary<string, IPlugin> plugins = new Dictionary<string, IPlugin>();
        public static Color Color { get; set; }
        public static int penSize { get; set; }

        public int WidthImage { get; set; }
        public int HeightImage { get; set; }

        public int count { get; set; }

        public Tools tools { get; set; }


        public MainForm()
        {
            InitializeComponent();
            tools = Tools.Pen;
            Color = Color.Black;
            penSize = 3;
            WidthImage = Width;
            HeightImage = Height;
            count = 1;
            toolStripTextBox1.Text = $"{penSize}";
            FindPlugins();
            CreatePluginsMenu();
        }
        void FindPlugins()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();

            string configFileName = "PluginPaths.config";
            string configFilePath;
            try
            {
                configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFileName);
            }
            catch (FileNotFoundException)
            {
                return;
            }
            configFileMap.ExeConfigFilename = configFilePath;

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            string pluginPathsValue;
            try
            {
                 pluginPathsValue = config.AppSettings.Settings["PluginPaths"].Value;
            }
            catch (NullReferenceException)
            {
                return;
            }
            string[] pluginPaths = pluginPathsValue.Split(';');

            foreach (string path in pluginPaths)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFile(path);

                    foreach (Type type in assembly.GetTypes())
                    {
                        Type iface = type.GetInterface("PluginInterface.IPlugin");

                        if (iface != null)
                        {
                            IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                            plugins.Add(plugin.Name, plugin);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка загрузки плагина ({path})\n" + ex.Message);
                }
            }
        }
        private void CreatePluginsMenu()
        {
            foreach (var p in plugins)
            {
                var item = filtersToolStripMenuItem.DropDownItems.Add(p.Value.Name);
                item.Click += OnPluginClick;
            }
        }

        private void OnPluginClick(object sender, EventArgs args)
        {
            IPlugin plugin = plugins[((ToolStripMenuItem)sender).Text];
            try
            {
                plugin.Transform((Bitmap)((DocumentForm)ActiveMdiChild).Image);
                ((DocumentForm)ActiveMdiChild).Invalidate();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Вы не выбрали изображение!");
            }
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new DocumentForm(this);
            frm.MdiParent = this;
            frm.Show();
            count++;
        }

        private void зелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Green;
        }

  

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmAbout = new AboutForm();
            frmAbout.ShowDialog();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var canvasSizeForm = new CanvasSizeForm(this);
            if (canvasSizeForm.ShowDialog() == DialogResult.OK)
                ((DocumentForm)ActiveMdiChild).changeSize();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Red;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color = Color.Blue;
        }

        private void другойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var colorChoice = new ColorDialog();
            if (colorChoice.ShowDialog() == DialogResult.OK)
                Color = colorChoice.Color;
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        private void DeleteImageTools()
        {
            перToolStripMenuItem.Image = null;
            линияToolStripMenuItem.Image = null;
            эллипсToolStripMenuItem.Image = null;
            ластикToolStripMenuItem.Image = null;
            звездаToolStripMenuItem.Image = null;
        }

        private void перToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImageTools();
            tools = Tools.Pen;
            перToolStripMenuItem.Image = MDIPAINT.Properties.Resources.black_circle;
        }

        private void линияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImageTools();
            tools = Tools.Line;
            линияToolStripMenuItem.Image = MDIPAINT.Properties.Resources.black_circle;

        }

        private void звездаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImageTools();
            tools = Tools.Star;
            звездаToolStripMenuItem.Image = MDIPAINT.Properties.Resources.black_circle;
        }

        private void ластикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImageTools();
            tools = Tools.Eraser;
            ластикToolStripMenuItem.Image = MDIPAINT.Properties.Resources.black_circle;
        }

        private void эллипсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteImageTools();
            tools = Tools.Ellipse;
            эллипсToolStripMenuItem.Image = MDIPAINT.Properties.Resources.black_circle;
        }

        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }



        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(toolStripTextBox1.Text, out int newPenSize) || toolStripTextBox1.Text == "")
            {
                if (newPenSize <= 0 && toolStripTextBox1.Text != "")
                {
                    MessageBox.Show("Можно вводить лишь целые положительные числа! Вы ввели неположительное целое число!");
                    toolStripTextBox1.Clear();
                }
            }
            else
            {
                MessageBox.Show("Вы ввели слишком больше число, либо вы ввели некорректный символ!");
                toolStripTextBox1.Clear();
                toolStripTextBox1.Text = $"{penSize}";
            }

        }

        private void toolStripTextBox1_Leave(object sender, EventArgs e)
        {
            if (toolStripTextBox1.Text == "")
                toolStripTextBox1.Text = $"{penSize}";
            else 
                penSize = int.Parse(toolStripTextBox1.Text);
            
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (toolStripTextBox1.Text == "")
                    toolStripTextBox1.Text = $"{penSize}";
                else
                {
                    penSize = int.Parse(toolStripTextBox1.Text);
                }
            }
        }

   

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null) {
                ((DocumentForm)ActiveMdiChild).ZoomIn();
                ((DocumentForm)ActiveMdiChild).Invalidate(); 
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                ((DocumentForm)ActiveMdiChild).ZoomOut();
                ((DocumentForm)ActiveMdiChild).Invalidate();

            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg|Все файлы ()*.*|*.*";
            if (dlg.ShowDialog() == DialogResult.OK && CheckOpenFile(dlg.FileName))
            {

                DocumentForm image = new DocumentForm(this, dlg);
                image.MdiParent = this;
                image.Show();
            }

        }
        private bool CheckOpenFile(string fileName)
        {

            string fileExtension = Path.GetExtension(fileName).ToLower();
            string[] allowedExtensions = { ".bmp", ".jpeg", ".jpg" };
            if (Array.Exists(allowedExtensions, ext => ext == fileExtension))
            {
                // Формат файла верный
                return true;
            }
            else
            {
                MessageBox.Show("Неверный формат файла! Можно открывать лишь файлы с расширениями bmp, jpeg, jpg!");
                return false;
            }
        }

        private void рисунокToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
            размерЗвездыToolStripMenuItem.Enabled = !(ActiveMdiChild == null);
        }

        private void окноToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            каскадомToolStripMenuItem.Enabled = ActiveMdiChild != null;
            слеваНаправоToolStripMenuItem.Enabled = ActiveMdiChild != null;
            сверхуВнизToolStripMenuItem.Enabled = ActiveMdiChild != null;
            упорядочитьЗначкиToolStripMenuItem.Enabled = ActiveMdiChild != null;
        }

        private void файлToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            сохранитьКакToolStripMenuItem.Enabled = ActiveMdiChild != null;
            сохранитьToolStripMenuItem.Enabled = ActiveMdiChild != null;
        }

        public void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (ActiveMdiChild != null && !((DocumentForm)ActiveMdiChild).isOpened)
                сохранитьКакToolStripMenuItem_Click(sender, e);
            else
            {
                ((DocumentForm)ActiveMdiChild).bitmap.Save(ActiveMdiChild.Text);
                ((DocumentForm)ActiveMdiChild).wasChange = false;
            }
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
            dlg.FileName = $"{((DocumentForm)ActiveMdiChild).Text}";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ((DocumentForm)ActiveMdiChild).bitmap.Save(dlg.FileName);
                ((DocumentForm)ActiveMdiChild).wasChange = false;
                ActiveMdiChild.Text = dlg.FileName;
                ((DocumentForm)ActiveMdiChild).isOpened = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ActiveMdiChild != null && ((DocumentForm)ActiveMdiChild).wasChange)
            {
                var dialog = MessageBox.Show($"Вы хотите сохранить изменения в файле {((DocumentForm)ActiveMdiChild).Text}?", "MDIPAINT", MessageBoxButtons.YesNoCancel);
                switch (dialog)
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                    case DialogResult.Yes:
                        сохранитьToolStripMenuItem_Click(sender, e);
                        break;                    
                }
            }
        }
        public void ChangeXY(int X, int Y)
        {
            toolStripStatusLabel1.Text = $"X:{X}, Y:{Y}";
        } //показ координат на документе

        private void размерЗвездыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StarForm starForm = new StarForm(this);
            if (starForm.ShowDialog() == DialogResult.OK)
                звездаToolStripMenuItem_Click(sender, e);
            
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void оФилToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var pluginForm = new PluginsForm(this);
            if (pluginForm.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void filtersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
