using PluginInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDIPAINT
{
    public partial class PluginsForm : Form
    {
        public PluginsForm(MainForm mainForm)
        {
            InitializeComponent();
            dataGridView.Columns.Add("Name", "Название");
            dataGridView.Columns.Add("Author", "Автор");
            dataGridView.Columns.Add("Version", "Версия");
            foreach (var namePlugin in mainForm.plugins)
            {
                IPlugin plugin = namePlugin.Value;

                string name = plugin.Name;

                string author = plugin.Author;

                string ver = "-";


                Type type = plugin.GetType();
                var attributes = type.GetCustomAttributes(typeof(VersionAttribute), false);
                if (attributes.Length > 0)
                {
                    var attribute = (VersionAttribute)attributes[0];
                    int major = attribute.Major;
                    int minor = attribute.Minor;
                    ver = major.ToString() + "." + minor.ToString();
                }


                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell { Value = name });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = author });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = ver });
                dataGridView.Rows.Add(row); 
            }
        }
    }
}
