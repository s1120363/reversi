using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace ggame
{
    public partial class UI : Form
    {
        public UI()
        {
            InitializeComponent();
            SetBackgroundImage();
        }

        private void SetBackgroundImage()
        {
            // 确保你的项目资源中有一张名为 background.jpg 的图片
            this.BackgroundImage = Properties.Resources.background;
            this.BackgroundImageLayout = ImageLayout.Stretch; // 或者使用其他布局方式，例如 Tile, Center, Zoom
        }
        private void button1_Click(object sender, EventArgs e)
        {

            // 隐藏当前表单
            this.Hide();
            // 创建并显示Form1
            Form1 form1 = new Form1();
            form1.FormClosed += (s, args) => this.Show(); // 当Form1关闭时重新显示UI
            form1.Show();
            form1.setModeToPVP();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 隐藏当前表单
            this.Hide();
            // 创建并显示Form1
            Form1 form1 = new Form1();
            form1.FormClosed += (s, args) => this.Show(); // 当Form1关闭时重新显示UI
            form1.Show();
            form1.setModeToPVE();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("你確定要退出碼？", "確認", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
