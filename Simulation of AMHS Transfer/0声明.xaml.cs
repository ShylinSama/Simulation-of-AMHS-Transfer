using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Simulation_of_AMHS_Transfer
{
    /// <summary>
    /// _0声明.xaml 的交互逻辑
    /// </summary>
    public partial class _0声明 : Window
    {
        public static _0声明 mainSM;

        public _0声明()
        {
            InitializeComponent();
            mainSM = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //新建窗体
            MainWindow main = new MainWindow();

            //获得父窗体
            //Window window = Window.GetWindow(this);//关闭父窗体

            //关闭父窗体
            mainSM.Hide();

            //显示新建窗体
            main.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            mainSM.Close();
        }
    }
}
