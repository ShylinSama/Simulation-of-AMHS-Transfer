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
    /// _1数据修改.xaml 的交互逻辑
    /// </summary>
    public partial class _1数据修改 : Window
    {
        //声明 制程各站点信息：序号、站点、可选机台
        public Queue<StationModel> Process_Station=new Queue<StationModel>();

        //声明 Index与STK所属关系
        public Dictionary<string, string> STK_Contain_Index=new Dictionary<string, string>();
        public _1数据修改()
        {
            InitializeComponent();

            Process_Station = MainWindow.main.Process_Station_OLD;
            STK_Contain_Index = MainWindow.main.STK_Contain_Index_OLD;

            ProcessDataGrid.ItemsSource = Process_Station;

            
        }
    }
}

