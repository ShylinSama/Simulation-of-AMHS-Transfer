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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
//using Microsoft.Win32;
using Path = System.IO.Path;
using System.Runtime.InteropServices;
using System.Data;
using System.Windows.Forms;


namespace Simulation_of_AMHS_Transfer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //声明 制程各站点信息：序号、站点、可选机台
        public Queue<StationModel> Process_Station = new Queue<StationModel>();

        //声明 Index与STK所属关系
        public Dictionary<string, string> STK_Contain_Index = new Dictionary<string, string>();

        //声明 单个Index处理全部制程的总数
        public Dictionary<string, int> CST_Number_Count_Index = new Dictionary<string, int>();

        //声明 每个制程处理CST的总数，用来核算抽检频率
        public Dictionary<string, int> CST_Number_Count_Process = new Dictionary<string, int>();

        //显示界面的内容-路径
        public MyTextshow TextShow_Path = new MyTextshow();

        //显示界面的内容-Index处理CST数量
        public MyTextshow TextShow_Index = new MyTextshow();

        //定义循环次数-投入CST数量
        public int Cycle_Time;

        //定义STK内部Index的权重
        public int Weight_In_STK;

        //定义STK外部Index的权重
        public int Weight_Outside_STK;

        //定义文件保存路径和文件名
        public string file = System.Environment.CurrentDirectory + "\\LOG" + "\\ProcessesLog" + "\\ProcessesLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //定义文件保存路径和文件名
        public string file_Transfer = System.Environment.CurrentDirectory + "\\LOG" + "\\TransferLog" + "\\TransferLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //定义路径搬送量
        public string file_Transfer_Count_Per = System.Environment.CurrentDirectory + "\\LOG" + "\\TransferCountPer" + "\\TransferCountPer" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //定义其他Log保存路径：每个机台进行CST的数目、随机比例
        public string OtherLog = System.Environment.CurrentDirectory + "\\LOG" + "\\OtherLog" + "\\OtherLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //随机数生成次数
        public int RandomCycleTime = 10000;

        //周期次数，一般按1个月算
        public int AllTime;

        //定义 STK list
        public List<string> STK_List = new List<string>();

        //定义 各个搬送路径的搬送量
        public Dictionary<string, int> Transfer_Count = new Dictionary<string, int>();

        public MainWindow()
        {
            InitializeComponent();

            //绑定数据
            TextShow_Index.show = "";
            Output_Index.DataContext = TextShow_Index;
        }

        //循环模拟
        public void CycleSimulation()
        {


            //循环进行路径生成
            for (int i = 0; i < Cycle_Time; i++)
            {
                Console.WriteLine(i);
                Simulation();
            }

            //更新各机台访问量
            //清空原有数据
            string Index_CST_Count = "";

            Index_CST_Count = Index_CST_Count + "各个机台访问次数：\n";

            foreach (var item in CST_Number_Count_Index)
            {
                Index_CST_Count = Index_CST_Count + item.Key + "\t" + item.Value.ToString() + "\n";
            }

            Index_CST_Count = Index_CST_Count + "各个制程的实际抽检比例：\n";

            foreach (var station in CST_Number_Count_Process)
            {
                Index_CST_Count = Index_CST_Count + station.Key + "\t" + (((double)station.Value/ Cycle_Time)).ToString("0.0000") + "\n";
            }

            TextShow_Index.Show = TextShow_Index.Show+"模拟完成！";

            File.AppendAllText(OtherLog, Index_CST_Count);

            //选择处理的log
            Analysis_Transfer_Log();

            Count_Transfer_All();
            
        }

        //主程序
        public void Simulation()
        {

            //定义搬送路径
            string ProcessPath="";

            //定义前一站点选择的 Index
            string SelectedIndex_Previous = "";

            //定义当前站点选择的 Index
            string SelectedIndex = "";

            foreach (var ProcessStation in Process_Station)
            {
                //清除
                SelectedIndex = "";

                //获得逻辑判断结果→IndexID
                SelectedIndex = CheckIndex(ProcessStation, SelectedIndex_Previous);

                //如果反馈结果为空，说明此站跳过
                if (SelectedIndex=="")
                {
                    //生成当前站信息 并合并到总机台路径中
                    //ProcessPath = ProcessPath + ProcessStation.Number.ToString() + ":" + ProcessStation.Process_Station_Number + ":" + "跳过此站" + ":" + ProcessStation.Selection_Ratio.ToString() + "→";
                }
                else
                {
                    //生成当前站信息 并合并到总机台路径中
                    ProcessPath = ProcessPath + ProcessStation.Number.ToString() + ":" + ProcessStation.Process_Station_Number + ":" + SelectedIndex + ":" + ProcessStation.Selection_Ratio.ToString() + "-";

                    SelectedIndex_Previous = SelectedIndex;
                }
            }

            //最后添加 结尾
            ProcessPath = ProcessPath + "19999\n";

            //显示赋值
            //TextShow_Path.show= TextShow_Path.show+ProcessPath;

            //输出各条路径Log
            //File.AppendAllText("log.txt", "\r\n" + ProcessPath);
            //string file = System.Environment.CurrentDirectory +"\\Log"+ DateTime.Now.ToString("yyyymmddHHmmss") + ".txt";
            File.AppendAllText(file, ProcessPath);

        }

        //加载各个制程站点信息
        //public void LoadProcessStation()
        //{

        //    //示例
        //    //Process_Station.Enqueue(new StationModel { 
        //    //    Number=1,
        //    //    Process_Station_Number="10000",
        //    //    Selection_Ratio=1,
        //    //    Selection_Index_String_All="2AIC01",
        //    //    Selection_Index_List_All=new List<IndexModel> {}
        //    //});

        //    Process_Station.Enqueue(new StationModel { Number = 1, Process_Station_Number = "10000", Selection_Ratio = 1, Selection_Index_String_All = "2ACI01" });
        //    Process_Station.Enqueue(new StationModel { Number = 2, Process_Station_Number = "10010", Selection_Ratio = 0.3333, Selection_Index_String_All = "2AMP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 3, Process_Station_Number = "10400", Selection_Ratio = 1, Selection_Index_String_All = "2AIC01/2AIC02/2AIC03" });
        //    Process_Station.Enqueue(new StationModel { Number = 4, Process_Station_Number = "10800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV01/2AOV02" });
        //    Process_Station.Enqueue(new StationModel { Number = 5, Process_Station_Number = "10820", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 6, Process_Station_Number = "10840", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 7, Process_Station_Number = "10870", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 8, Process_Station_Number = "10876", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 9, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 10, Process_Station_Number = "10070", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 11, Process_Station_Number = "10076", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 12, Process_Station_Number = "10020", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 13, Process_Station_Number = "10100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC01/2AFC02" });
        //    Process_Station.Enqueue(new StationModel { Number = 14, Process_Station_Number = "10140", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 15, Process_Station_Number = "10170", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 16, Process_Station_Number = "10176", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 17, Process_Station_Number = "10120", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 18, Process_Station_Number = "10401", Selection_Ratio = 1, Selection_Index_String_All = "2AIC01/2AIC02/2AIC03" });
        //    Process_Station.Enqueue(new StationModel { Number = 19, Process_Station_Number = "10801", Selection_Ratio = 1, Selection_Index_String_All = "2AOV01/2AOV02" });
        //    Process_Station.Enqueue(new StationModel { Number = 20, Process_Station_Number = "10821", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 21, Process_Station_Number = "10841", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 22, Process_Station_Number = "10871", Selection_Ratio = 1, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 23, Process_Station_Number = "10877", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 24, Process_Station_Number = "10005", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 25, Process_Station_Number = "10071", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 26, Process_Station_Number = "10077", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 27, Process_Station_Number = "10021", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 28, Process_Station_Number = "10081", Selection_Ratio = 1, Selection_Index_String_All = "2ATB01/2ATB02" });
        //    Process_Station.Enqueue(new StationModel { Number = 29, Process_Station_Number = "10101", Selection_Ratio = 1, Selection_Index_String_All = "2AFC01/2AFC02" });
        //    Process_Station.Enqueue(new StationModel { Number = 30, Process_Station_Number = "10141", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 31, Process_Station_Number = "10121", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 32, Process_Station_Number = "10171", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 33, Process_Station_Number = "10177", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 34, Process_Station_Number = "11100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC06" });
        //    Process_Station.Enqueue(new StationModel { Number = 35, Process_Station_Number = "11140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 36, Process_Station_Number = "11170", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 37, Process_Station_Number = "11120", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 38, Process_Station_Number = "11176", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 39, Process_Station_Number = "11300", Selection_Ratio = 1, Selection_Index_String_All = "2AFE01/2AFE02/2AFE05/2AFE06" });
        //    Process_Station.Enqueue(new StationModel { Number = 40, Process_Station_Number = "11370", Selection_Ratio = 0.5, Selection_Index_String_All = "2AMA02" });
        //    Process_Station.Enqueue(new StationModel { Number = 41, Process_Station_Number = "11376", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 42, Process_Station_Number = "11320", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 43, Process_Station_Number = "113A0", Selection_Ratio = 1, Selection_Index_String_All = "2AMF01" });
        //    Process_Station.Enqueue(new StationModel { Number = 44, Process_Station_Number = "11400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP02" });
        //    Process_Station.Enqueue(new StationModel { Number = 45, Process_Station_Number = "11450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 46, Process_Station_Number = "11451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 47, Process_Station_Number = "11430", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMT01" });
        //    Process_Station.Enqueue(new StationModel { Number = 48, Process_Station_Number = "11431", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMT01" });
        //    Process_Station.Enqueue(new StationModel { Number = 49, Process_Station_Number = "11432", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMT01/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 50, Process_Station_Number = "11470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 51, Process_Station_Number = "11476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 52, Process_Station_Number = "11420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 53, Process_Station_Number = "11500", Selection_Ratio = 1, Selection_Index_String_All = "2AED01" });
        //    Process_Station.Enqueue(new StationModel { Number = 54, Process_Station_Number = "11570", Selection_Ratio = 1, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 55, Process_Station_Number = "11600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 56, Process_Station_Number = "11650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 57, Process_Station_Number = "11670", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 58, Process_Station_Number = "11676", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 59, Process_Station_Number = "11620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 60, Process_Station_Number = "11681", Selection_Ratio = 1, Selection_Index_String_All = "2ATB01/2ATB02/2ATR01/2ATR02/2ATR03/2ATR04/2ATR07" });
        //    Process_Station.Enqueue(new StationModel { Number = 61, Process_Station_Number = "11632", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMT01/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 62, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 63, Process_Station_Number = "11700", Selection_Ratio = 1, Selection_Index_String_All = "2AEI01/2AEI02" });
        //    Process_Station.Enqueue(new StationModel { Number = 64, Process_Station_Number = "11720", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 65, Process_Station_Number = "12100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC07" });
        //    Process_Station.Enqueue(new StationModel { Number = 66, Process_Station_Number = "12140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 67, Process_Station_Number = "12170", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 68, Process_Station_Number = "12176", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 69, Process_Station_Number = "12120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 70, Process_Station_Number = "12200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS01" });
        //    Process_Station.Enqueue(new StationModel { Number = 71, Process_Station_Number = "12260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
        //    Process_Station.Enqueue(new StationModel { Number = 72, Process_Station_Number = "12270", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 73, Process_Station_Number = "12276", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 74, Process_Station_Number = "12220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 75, Process_Station_Number = "12400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP04/2APP01/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 76, Process_Station_Number = "12450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 77, Process_Station_Number = "12451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 78, Process_Station_Number = "12452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 79, Process_Station_Number = "12470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 80, Process_Station_Number = "12476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 81, Process_Station_Number = "12420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 82, Process_Station_Number = "12500", Selection_Ratio = 1, Selection_Index_String_All = "2AED04/2AED05" });
        //    Process_Station.Enqueue(new StationModel { Number = 83, Process_Station_Number = "12600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 84, Process_Station_Number = "12650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 85, Process_Station_Number = "12670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 86, Process_Station_Number = "12676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 87, Process_Station_Number = "12620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 88, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 89, Process_Station_Number = "12700", Selection_Ratio = 1, Selection_Index_String_All = "2AEI01/2AEI02" });
        //    Process_Station.Enqueue(new StationModel { Number = 90, Process_Station_Number = "12720", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 91, Process_Station_Number = "13100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC08" });
        //    Process_Station.Enqueue(new StationModel { Number = 92, Process_Station_Number = "13140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 93, Process_Station_Number = "13170", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 94, Process_Station_Number = "13176", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 95, Process_Station_Number = "13120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 96, Process_Station_Number = "13200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS01" });
        //    Process_Station.Enqueue(new StationModel { Number = 97, Process_Station_Number = "13260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
        //    Process_Station.Enqueue(new StationModel { Number = 98, Process_Station_Number = "13270", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 99, Process_Station_Number = "13276", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 100, Process_Station_Number = "13220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 101, Process_Station_Number = "13400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP01/2APP04/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 102, Process_Station_Number = "13450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 103, Process_Station_Number = "13451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 104, Process_Station_Number = "13452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 105, Process_Station_Number = "13470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 106, Process_Station_Number = "13476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 107, Process_Station_Number = "13420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 108, Process_Station_Number = "13500", Selection_Ratio = 1, Selection_Index_String_All = "2AED04/2AED05" });
        //    Process_Station.Enqueue(new StationModel { Number = 109, Process_Station_Number = "13600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 110, Process_Station_Number = "13650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 111, Process_Station_Number = "13670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 112, Process_Station_Number = "13676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 113, Process_Station_Number = "13620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 114, Process_Station_Number = "14101", Selection_Ratio = 1, Selection_Index_String_All = "2AFC10" });
        //    Process_Station.Enqueue(new StationModel { Number = 115, Process_Station_Number = "14100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC10" });
        //    Process_Station.Enqueue(new StationModel { Number = 116, Process_Station_Number = "14140", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 117, Process_Station_Number = "14170", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 118, Process_Station_Number = "14176", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 119, Process_Station_Number = "14120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 120, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
        //    Process_Station.Enqueue(new StationModel { Number = 121, Process_Station_Number = "14800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV04/2AOV05/2AOV06" });
        //    Process_Station.Enqueue(new StationModel { Number = 122, Process_Station_Number = "14870", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 123, Process_Station_Number = "14876", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 124, Process_Station_Number = "14820", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 125, Process_Station_Number = "14400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP06/2APP01/2APP02" });
        //    Process_Station.Enqueue(new StationModel { Number = 126, Process_Station_Number = "14450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 127, Process_Station_Number = "14451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 128, Process_Station_Number = "14452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 129, Process_Station_Number = "14470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 130, Process_Station_Number = "14476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 131, Process_Station_Number = "14420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 132, Process_Station_Number = "14500", Selection_Ratio = 1, Selection_Index_String_All = "2AED08/2AED09" });
        //    Process_Station.Enqueue(new StationModel { Number = 133, Process_Station_Number = "14600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 134, Process_Station_Number = "14650", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 135, Process_Station_Number = "14670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 136, Process_Station_Number = "14676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 137, Process_Station_Number = "14620", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 138, Process_Station_Number = "14801", Selection_Ratio = 1, Selection_Index_String_All = "2AOV05/2AOV06" });
        //    Process_Station.Enqueue(new StationModel { Number = 139, Process_Station_Number = "14821", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 140, Process_Station_Number = "17200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 141, Process_Station_Number = "17260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
        //    Process_Station.Enqueue(new StationModel { Number = 142, Process_Station_Number = "17270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 143, Process_Station_Number = "17276", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 144, Process_Station_Number = "17220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 145, Process_Station_Number = "17400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP02/2APP01/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 146, Process_Station_Number = "17450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 147, Process_Station_Number = "17451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 148, Process_Station_Number = "17452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 149, Process_Station_Number = "17470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 150, Process_Station_Number = "17476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 151, Process_Station_Number = "17420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 152, Process_Station_Number = "17500", Selection_Ratio = 1, Selection_Index_String_All = "2AED12" });
        //    Process_Station.Enqueue(new StationModel { Number = 153, Process_Station_Number = "17600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 154, Process_Station_Number = "17650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 155, Process_Station_Number = "17670", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 156, Process_Station_Number = "17676", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 157, Process_Station_Number = "17620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 158, Process_Station_Number = "17682", Selection_Ratio = 1, Selection_Index_String_All = "2ATL01" });
        //    Process_Station.Enqueue(new StationModel { Number = 159, Process_Station_Number = "17690", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATE01/2ATE02" });
        //    Process_Station.Enqueue(new StationModel { Number = 160, Process_Station_Number = "15400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP06/2APP04/2APP02" });
        //    Process_Station.Enqueue(new StationModel { Number = 161, Process_Station_Number = "15450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 162, Process_Station_Number = "15451", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 163, Process_Station_Number = "15452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 164, Process_Station_Number = "15470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 165, Process_Station_Number = "15476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 166, Process_Station_Number = "15420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 167, Process_Station_Number = "15500", Selection_Ratio = 1, Selection_Index_String_All = "2AED08/2AED09" });
        //    Process_Station.Enqueue(new StationModel { Number = 168, Process_Station_Number = "15600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 169, Process_Station_Number = "15650", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 170, Process_Station_Number = "15670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 171, Process_Station_Number = "15676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 172, Process_Station_Number = "15620", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 173, Process_Station_Number = "15830", Selection_Ratio = 0.0714, Selection_Index_String_All = "2AMT01" });
        //    Process_Station.Enqueue(new StationModel { Number = 174, Process_Station_Number = "19400", Selection_Ratio = 1, Selection_Index_String_All = "2APP05/2APP03/2APP01/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 175, Process_Station_Number = "19450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 176, Process_Station_Number = "19451", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 177, Process_Station_Number = "19452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 178, Process_Station_Number = "19470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 179, Process_Station_Number = "19476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 180, Process_Station_Number = "19420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 181, Process_Station_Number = "19800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
        //    Process_Station.Enqueue(new StationModel { Number = 182, Process_Station_Number = "19840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 183, Process_Station_Number = "19870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 184, Process_Station_Number = "19876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 185, Process_Station_Number = "19820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 186, Process_Station_Number = "19900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
        //    Process_Station.Enqueue(new StationModel { Number = 187, Process_Station_Number = "19920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 188, Process_Station_Number = "1A201", Selection_Ratio = 1, Selection_Index_String_All = "2ACP03" });
        //    Process_Station.Enqueue(new StationModel { Number = 189, Process_Station_Number = "1A200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 190, Process_Station_Number = "1A260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
        //    Process_Station.Enqueue(new StationModel { Number = 191, Process_Station_Number = "1A270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 192, Process_Station_Number = "1A276", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 193, Process_Station_Number = "1A220", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 194, Process_Station_Number = "1A400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP06/2APP04/2APP02" });
        //    Process_Station.Enqueue(new StationModel { Number = 195, Process_Station_Number = "1A450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 196, Process_Station_Number = "1A451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 197, Process_Station_Number = "1A452", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 198, Process_Station_Number = "1A470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 199, Process_Station_Number = "1A476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 200, Process_Station_Number = "1A420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 201, Process_Station_Number = "1A500", Selection_Ratio = 1, Selection_Index_String_All = "2AED12" });
        //    Process_Station.Enqueue(new StationModel { Number = 202, Process_Station_Number = "1A600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
        //    Process_Station.Enqueue(new StationModel { Number = 203, Process_Station_Number = "1A650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 204, Process_Station_Number = "1A670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 205, Process_Station_Number = "1A676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 206, Process_Station_Number = "1A620", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 207, Process_Station_Number = "1B400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP03/2APP05/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 208, Process_Station_Number = "1B450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 209, Process_Station_Number = "1B451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 210, Process_Station_Number = "1B452", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 211, Process_Station_Number = "1B470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 212, Process_Station_Number = "1B476", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 213, Process_Station_Number = "1B420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 214, Process_Station_Number = "1B800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
        //    Process_Station.Enqueue(new StationModel { Number = 215, Process_Station_Number = "1B840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 216, Process_Station_Number = "1B870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 217, Process_Station_Number = "1B876", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 218, Process_Station_Number = "1B820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 219, Process_Station_Number = "1B900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
        //    Process_Station.Enqueue(new StationModel { Number = 220, Process_Station_Number = "1B920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 221, Process_Station_Number = "1B400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP03/2APP05/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 222, Process_Station_Number = "1B450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 223, Process_Station_Number = "1B451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 224, Process_Station_Number = "1B452", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 225, Process_Station_Number = "1B470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 226, Process_Station_Number = "1B476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 227, Process_Station_Number = "1B420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 228, Process_Station_Number = "1B840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 229, Process_Station_Number = "1B870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 230, Process_Station_Number = "1B876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 231, Process_Station_Number = "1B820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 232, Process_Station_Number = "1B900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
        //    Process_Station.Enqueue(new StationModel { Number = 233, Process_Station_Number = "1B920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 234, Process_Station_Number = "1C200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS05" });
        //    Process_Station.Enqueue(new StationModel { Number = 235, Process_Station_Number = "1C260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
        //    Process_Station.Enqueue(new StationModel { Number = 236, Process_Station_Number = "1C270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 237, Process_Station_Number = "1C276", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 238, Process_Station_Number = "1C220", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 239, Process_Station_Number = "1C400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP01/2APP04/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 240, Process_Station_Number = "1C450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 241, Process_Station_Number = "1C470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 242, Process_Station_Number = "1C476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 243, Process_Station_Number = "1C420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 244, Process_Station_Number = "1C500", Selection_Ratio = 1, Selection_Index_String_All = "2AEW01" });
        //    Process_Station.Enqueue(new StationModel { Number = 245, Process_Station_Number = "1C501", Selection_Ratio = 1, Selection_Index_String_All = "2AEW02/2AEW03" });
        //    Process_Station.Enqueue(new StationModel { Number = 246, Process_Station_Number = "1C600", Selection_Ratio = 1, Selection_Index_String_All = "2AES01" });
        //    Process_Station.Enqueue(new StationModel { Number = 247, Process_Station_Number = "1C650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 248, Process_Station_Number = "1C670", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 249, Process_Station_Number = "1C676", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 250, Process_Station_Number = "1C620", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 251, Process_Station_Number = "1H400", Selection_Ratio = 1, Selection_Index_String_All = "2APP03/2APP01/2APP05/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 252, Process_Station_Number = "1H440", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 253, Process_Station_Number = "1H450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 254, Process_Station_Number = "1H470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 255, Process_Station_Number = "1H476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 256, Process_Station_Number = "1H420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 257, Process_Station_Number = "1H800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
        //    Process_Station.Enqueue(new StationModel { Number = 258, Process_Station_Number = "1H840", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 259, Process_Station_Number = "1H841", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 260, Process_Station_Number = "1H850", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 261, Process_Station_Number = "1H870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 262, Process_Station_Number = "1H876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 263, Process_Station_Number = "1H820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 264, Process_Station_Number = "1H400", Selection_Ratio = 1, Selection_Index_String_All = "2APP03/2APP01/2APP05/2APP06" });
        //    Process_Station.Enqueue(new StationModel { Number = 265, Process_Station_Number = "1H440", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 266, Process_Station_Number = "1H450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 267, Process_Station_Number = "1H470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 268, Process_Station_Number = "1H476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 269, Process_Station_Number = "1H420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 270, Process_Station_Number = "1H800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
        //    Process_Station.Enqueue(new StationModel { Number = 271, Process_Station_Number = "1H840", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 272, Process_Station_Number = "1H841", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
        //    Process_Station.Enqueue(new StationModel { Number = 273, Process_Station_Number = "1H850", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
        //    Process_Station.Enqueue(new StationModel { Number = 274, Process_Station_Number = "1H870", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
        //    Process_Station.Enqueue(new StationModel { Number = 275, Process_Station_Number = "1H876", Selection_Ratio = 0.3333, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
        //    Process_Station.Enqueue(new StationModel { Number = 276, Process_Station_Number = "1H820", Selection_Ratio = 0.3333, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
        //    Process_Station.Enqueue(new StationModel { Number = 277, Process_Station_Number = "1H890", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATE01/2ATE02" });
        

        //    //写入Index可选列表 信息
        //    foreach (var item in Process_Station)
        //    {
        //        //拆分可选Index字符串，成数组
        //        string[] arr = item.Selection_Index_String_All.Split('/');

        //        //实例化 可选Index列表
        //        item.Selection_Index_List_All = new List<IndexModel>();

        //        //写入当前站点可选Index列表
        //        foreach (var id in arr)
        //        {
        //            IndexModel indexModel = new IndexModel();
        //            indexModel.IndexID = id;
        //            item.Selection_Index_List_All.Add(indexModel);
        //        }
        //    }

        //    //写入Index所属STK信息
        //    foreach (var item1 in Process_Station)
        //    {
        //        foreach (var item in item1.Selection_Index_List_All)
        //        {
        //            item.In_STK = STK_Contain_Index[item.IndexID];
        //        }
        //    }

        //    //写入Index进行各个制程需要的时间


        //}

        //加载Index所属STK信息
        //public void LoadIndexInfoOfStk()
        //{
        //    STK_Contain_Index.Add("2AIC01", "STK01");
        //    STK_Contain_Index.Add("2AIC02", "STK01");
        //    STK_Contain_Index.Add("2AIC03", "STK01");
        //    STK_Contain_Index.Add("2AIC04", "STK01");
        //    STK_Contain_Index.Add("2AIC05", "STK01");
        //    STK_Contain_Index.Add("2AOV01", "STK01");
        //    STK_Contain_Index.Add("2AOV02", "STK01");
        //    STK_Contain_Index.Add("2AOV03", "STK01");
        //    STK_Contain_Index.Add("2ATO01", "STK01");
        //    STK_Contain_Index.Add("2ACP01", "STK02");
        //    STK_Contain_Index.Add("2AED08", "STK02");
        //    STK_Contain_Index.Add("2AED09", "STK02");
        //    STK_Contain_Index.Add("2AED03", "STK02");
        //    STK_Contain_Index.Add("2AED11", "STK02");
        //    STK_Contain_Index.Add("2AFC10", "STK02");
        //    STK_Contain_Index.Add("2AMS01", "STK02");
        //    STK_Contain_Index.Add("2AFC04", "STK02");
        //    STK_Contain_Index.Add("2AMA01", "STK02");
        //    STK_Contain_Index.Add("2ATB01", "STK02");
        //    STK_Contain_Index.Add("2ATB02", "STK02");
        //    STK_Contain_Index.Add("2ATO02", "STK02");
        //    STK_Contain_Index.Add("2ATO03", "STK02");
        //    STK_Contain_Index.Add("2AMM01", "STK03");
        //    STK_Contain_Index.Add("2AFC01", "STK03");
        //    STK_Contain_Index.Add("2AFC02", "STK03");
        //    STK_Contain_Index.Add("2AFC12", "STK03");
        //    STK_Contain_Index.Add("2AFC03", "STK03");
        //    STK_Contain_Index.Add("2AFC13", "STK03");
        //    STK_Contain_Index.Add("2AFE07", "STK03");
        //    STK_Contain_Index.Add("2AFE06", "STK03");
        //    STK_Contain_Index.Add("2AFE05", "STK03");
        //    STK_Contain_Index.Add("2AMF01", "STK03");
        //    STK_Contain_Index.Add("2AMA02", "STK04");
        //    STK_Contain_Index.Add("2AML01", "STK04");
        //    STK_Contain_Index.Add("2AFE01", "STK04");
        //    STK_Contain_Index.Add("2AFE02", "STK04");
        //    STK_Contain_Index.Add("2AFE03", "STK04");
        //    STK_Contain_Index.Add("2AFE04", "STK04");
        //    STK_Contain_Index.Add("2AFC11", "STK04");
        //    STK_Contain_Index.Add("2AFC06", "STK04");
        //    STK_Contain_Index.Add("2AFC08", "STK04");
        //    STK_Contain_Index.Add("2AFC07", "STK04");
        //    STK_Contain_Index.Add("2ACP03", "STK04");
        //    STK_Contain_Index.Add("2ATO04", "STK05");
        //    STK_Contain_Index.Add("2ATO05", "STK05");
        //    STK_Contain_Index.Add("2AMS02", "STK05");
        //    STK_Contain_Index.Add("2ATR08", "STK05");
        //    STK_Contain_Index.Add("2ATR12", "STK05");
        //    STK_Contain_Index.Add("2ATO06", "STK05");
        //    STK_Contain_Index.Add("2ATE05", "STK05");
        //    STK_Contain_Index.Add("2AMM02", "STK05");
        //    STK_Contain_Index.Add("2AED04", "STK05");
        //    STK_Contain_Index.Add("2AED05", "STK05");
        //    STK_Contain_Index.Add("2AED06", "STK05");
        //    STK_Contain_Index.Add("2AMH01", "STK05");
        //    STK_Contain_Index.Add("2AEW04", "STK05");
        //    STK_Contain_Index.Add("2AEW05", "STK05");
        //    STK_Contain_Index.Add("2AMM03", "STK06");
        //    STK_Contain_Index.Add("2AED10", "STK06");
        //    STK_Contain_Index.Add("2AED07", "STK06");
        //    STK_Contain_Index.Add("2AED02", "STK06");
        //    STK_Contain_Index.Add("2AOV04", "STK06");
        //    STK_Contain_Index.Add("2AOV05", "STK06");
        //    STK_Contain_Index.Add("2AOV06", "STK06");
        //    STK_Contain_Index.Add("2AOV07", "STK06");
        //    STK_Contain_Index.Add("2ACP05", "STK06");
        //    STK_Contain_Index.Add("2AMS03", "STK07");
        //    STK_Contain_Index.Add("2AED01", "STK07");
        //    STK_Contain_Index.Add("2AED12", "STK07");
        //    STK_Contain_Index.Add("2AED13", "STK07");
        //    STK_Contain_Index.Add("2AEI04", "STK07");
        //    STK_Contain_Index.Add("2ATO08", "STK07");
        //    STK_Contain_Index.Add("2ATO07", "STK07");
        //    STK_Contain_Index.Add("2AOV11", "STK07");
        //    STK_Contain_Index.Add("2AMA03", "STK07");
        //    STK_Contain_Index.Add("2ATB03", "STK07");
        //    STK_Contain_Index.Add("2ATR05", "STK07");
        //    STK_Contain_Index.Add("2ATR06", "STK07");
        //    STK_Contain_Index.Add("2AOV10", "STK07");
        //    STK_Contain_Index.Add("2AMM05", "STK08");
        //    STK_Contain_Index.Add("2AMA04", "STK08");
        //    STK_Contain_Index.Add("2ATO09", "STK08");
        //    STK_Contain_Index.Add("2ATR07", "STK08");
        //    STK_Contain_Index.Add("2AMC01", "STK08");
        //    STK_Contain_Index.Add("2ATO17", "STK08");
        //    STK_Contain_Index.Add("2APP09", "STK08");
        //    STK_Contain_Index.Add("2APP08", "STK08");
        //    STK_Contain_Index.Add("2APP07", "STK08");
        //    STK_Contain_Index.Add("2APP06", "STK08");
        //    STK_Contain_Index.Add("2APP05", "STK08");
        //    STK_Contain_Index.Add("2AMS04", "STK08");
        //    STK_Contain_Index.Add("2APP01", "STK09");
        //    STK_Contain_Index.Add("2AMT01", "STK09");
        //    STK_Contain_Index.Add("2APP02", "STK09");
        //    STK_Contain_Index.Add("2APP03", "STK09");
        //    STK_Contain_Index.Add("2AMC02", "STK09");
        //    STK_Contain_Index.Add("2APP04", "STK09");
        //    STK_Contain_Index.Add("2ATO10", "STK09");
        //    STK_Contain_Index.Add("2ATE03", "STK09");
        //    STK_Contain_Index.Add("2ATE04", "STK09");
        //    STK_Contain_Index.Add("2AMM06", "STK09");
        //    STK_Contain_Index.Add("2ATO16", "STK09");
        //    STK_Contain_Index.Add("2ATR09", "STK09");
        //    STK_Contain_Index.Add("2ATR10", "STK09");
        //    STK_Contain_Index.Add("2ATR11", "STK09");
        //    STK_Contain_Index.Add("2ATO19", "STK09");
        //    STK_Contain_Index.Add("2AFS07", "STK10");
        //    STK_Contain_Index.Add("2AFS08", "STK10");
        //    STK_Contain_Index.Add("2AEI01", "STK10");
        //    STK_Contain_Index.Add("2AEI02", "STK10");
        //    STK_Contain_Index.Add("2AEI03", "STK10");
        //    STK_Contain_Index.Add("2ACP04", "STK10");
        //    STK_Contain_Index.Add("2ASS01", "STK10");
        //    STK_Contain_Index.Add("2ATL01", "STK11");
        //    STK_Contain_Index.Add("2AMM04", "STK11");
        //    STK_Contain_Index.Add("2ATO18", "STK11");
        //    STK_Contain_Index.Add("2ATO14", "STK11");
        //    STK_Contain_Index.Add("2ATR01", "STK11");
        //    STK_Contain_Index.Add("2ATR02", "STK11");
        //    STK_Contain_Index.Add("2ATR03", "STK11");
        //    STK_Contain_Index.Add("2ATR04", "STK11");
        //    STK_Contain_Index.Add("2ACP02", "STK11");
        //    STK_Contain_Index.Add("2AFS03", "STK11");
        //    STK_Contain_Index.Add("2AFS04", "STK11");
        //    STK_Contain_Index.Add("2AFS02", "STK12");
        //    STK_Contain_Index.Add("2AFS05", "STK12");
        //    STK_Contain_Index.Add("2AMY01", "STK12");
        //    STK_Contain_Index.Add("2AMR01", "STK12");
        //    STK_Contain_Index.Add("2AFS01", "STK12");
        //    STK_Contain_Index.Add("2AFS06", "STK12");
        //    STK_Contain_Index.Add("2AES05", "STK13");
        //    STK_Contain_Index.Add("2AES06", "STK13");
        //    STK_Contain_Index.Add("2AES07", "STK13");
        //    STK_Contain_Index.Add("2AEW01", "STK13");
        //    STK_Contain_Index.Add("2AEW02", "STK13");
        //    STK_Contain_Index.Add("2AEW03", "STK13");
        //    STK_Contain_Index.Add("2AES01", "STK13");
        //    STK_Contain_Index.Add("2AMC03", "STK13");
        //    STK_Contain_Index.Add("2ATO13", "STK13");
        //    STK_Contain_Index.Add("2ATO11", "STK13");
        //    STK_Contain_Index.Add("2ATO12", "STK13");
        //    STK_Contain_Index.Add("2ATO15", "STK13");
        //    STK_Contain_Index.Add("2ATO20", "STK13");
        //    STK_Contain_Index.Add("2AMM07", "STK13");
        //    STK_Contain_Index.Add("2AMM08", "STK13");
        //    STK_Contain_Index.Add("2AOV09", "STK14");
        //    STK_Contain_Index.Add("2AES08", "STK14");
        //    STK_Contain_Index.Add("2AES02", "STK14");
        //    STK_Contain_Index.Add("2AES03", "STK14");
        //    STK_Contain_Index.Add("2AES04", "STK14");
        //    STK_Contain_Index.Add("2AMC04", "STK14");
        //    STK_Contain_Index.Add("2ATT01", "STK14");
        //    STK_Contain_Index.Add("2ATT02", "STK14");
        //    STK_Contain_Index.Add("2ATT03", "STK14");
        //    STK_Contain_Index.Add("2ATT04", "STK14");
        //    STK_Contain_Index.Add("2AMC05", "STK14");
        //    STK_Contain_Index.Add("2APP10", "STK15");
        //    STK_Contain_Index.Add("2AMT02", "STK15");
        //    STK_Contain_Index.Add("2APP11", "STK15");
        //    STK_Contain_Index.Add("2AMC06", "STK15");
        //    STK_Contain_Index.Add("2APP12", "STK15");
        //    STK_Contain_Index.Add("2APP13", "STK15");
        //    STK_Contain_Index.Add("2AMM09", "STK15");
        //    STK_Contain_Index.Add("2AMP01", "STK15");
        //    STK_Contain_Index.Add("2ATE01", "STK15");
        //    STK_Contain_Index.Add("2ATE02", "STK15");
        //    STK_Contain_Index.Add("2AOV08", "STK15");
        //    STK_Contain_Index.Add("2ACI01", "STK15");

        //    //加载STK列表信息
        //    STK_List.Add("STK01");
        //    STK_List.Add("STK02");
        //    STK_List.Add("STK03");
        //    STK_List.Add("STK04");
        //    STK_List.Add("STK05");
        //    STK_List.Add("STK06");
        //    STK_List.Add("STK07");
        //    STK_List.Add("STK08");
        //    STK_List.Add("STK09");
        //    STK_List.Add("STK10");
        //    STK_List.Add("STK11");
        //    STK_List.Add("STK12");
        //    STK_List.Add("STK13");
        //    STK_List.Add("STK14");
        //    STK_List.Add("STK15");
        //}

        //加载每个Index 制程CST数目
        public void LoadCST_Number_Count()
        {
            //写入所有index ID，并初始化制程CST数目为0
            foreach (var item in STK_Contain_Index)
            {
                CST_Number_Count_Index.Add(item.Key,0);
            }
        }

        //加载制程字典
        public void Load_CST_Number_Count_Process()
        {
            foreach (var item in Process_Station)
            {
                CST_Number_Count_Process.Add(item.Number+"\t"+item.Process_Station_Number, 0);
            }
        }

        //加载总的搬送量
        public void Load_Transfer_Count_All()
        {
            //Index -> STK、Index -> STK 路径加载
            foreach (var item in STK_Contain_Index)
            {
                Transfer_Count.Add(item.Key+"\t"+item.Value+"-ZONE",0);
                Transfer_Count.Add(item.Value + "-ZONE" + "\t"+item.Key,0);
            }

            //STK->OHS、OHS->STK
            foreach (var item in STK_List)
            {
                Transfer_Count.Add(item + "-ZONE" + "\tOHS", 0);
                Transfer_Count.Add("OHS\t"+item + "-ZONE", 0);
            }
        }

        //选择满足条件的Index，逻辑判断
        public string CheckIndex(StationModel CurrentStation,string SelectedIndexPrevious)
        {
            //初始化 当前站点的Index
            string SelectedIndex="";

            //初始化 总权重
            int[] TotalWeight = new int[CurrentStation.Selection_Index_List_All.Count];

            //如果是第一站
            if (CurrentStation.Number == 1)
            {
                //如果是第一站，直接反馈IndexID
                SelectedIndex = CurrentStation.Selection_Index_List_All.First().IndexID;

                //Index处理CST数量+1
                CST_Number_Count_Index[SelectedIndex] = CST_Number_Count_Index[SelectedIndex] + 1;

                //制程处理CST数量
                CST_Number_Count_Process[CurrentStation.Number + "\t" + CurrentStation.Process_Station_Number] = CST_Number_Count_Process[CurrentStation.Number + "\t" + CurrentStation.Process_Station_Number] + 1;
            }

            //如果不是第一站
            else 
            {
                //声明前一站index所在的STK
                string SelectedIndexPrevious_STK ="";

                try
                {
                    SelectedIndexPrevious_STK = STK_Contain_Index[SelectedIndexPrevious];
                }
                catch (Exception)
                {
                    Console.WriteLine("查询Index所属STK时出错"+SelectedIndexPrevious);
                    throw;
                }

                //如果随机结果为True进行此站
                if (GetRandomByGuid_1(CurrentStation.Selection_Ratio))
                {
                    //计算所有备选Index的总权重
                    foreach (var item in CurrentStation.Selection_Index_List_All)
                    {
                        //查询当前Index的索引号
                        int idx = CurrentStation.Selection_Index_List_All.IndexOf(item);

                        if (item.In_STK == SelectedIndexPrevious_STK)
                        {
                            //同STK权重为4
                            TotalWeight[idx] = CST_Number_Count_Index[item.IndexID] * Weight_Outside_STK;
                        }
                        else
                        {
                            //同STK权重为6
                            TotalWeight[idx] = CST_Number_Count_Index[item.IndexID] * Weight_In_STK;
                        }
                    }

                    //返回最小权重的Index
                    SelectedIndex = CurrentStation.Selection_Index_List_All[TotalWeight.ToList().IndexOf(TotalWeight.Min())].IndexID;

                    //Index处理CST数量+1
                    CST_Number_Count_Index[SelectedIndex] = CST_Number_Count_Index[SelectedIndex] + 1;

                    //制程处理CST数量
                    CST_Number_Count_Process[CurrentStation.Number + "\t" + CurrentStation.Process_Station_Number] = CST_Number_Count_Process[CurrentStation.Number + "\t" + CurrentStation.Process_Station_Number] + 1;
                }
                //如果随机结果为 不进行此站
                else
                {
                    
                }
            }

            return SelectedIndex;
        }

        // 使用Guid产生的种子生成真随机数
        public bool GetRandomByGuid_1(double Seletion_Ratio)
        {
            //先判断该站是否需要进行-利用抽检比例
            int[] RandomArray = new int[1];

            //第三种随机方法
            GetRandomByRNGCryptoServiceProvider(RandomArray);

            //如果随机结果大于抽检比例
            if (RandomArray[0] <= (Seletion_Ratio * 10000))
            {
                //如果随机总结果大于抽检比例，则返回True
                return true;
            }
            else
            {
                return false;
            }
        }

        // 使用Guid产生的种子生成真随机数
        //public bool GetRandomByGuid(double Seletion_Ratio)
        //{
        //    //先判断该站是否需要进行-利用抽检比例
        //    int[] RandomArray = new int[RandomCycleTime];

        //    //新建变量-统计结果
        //    int CountResult = 0;

        //    //第三种随机方法
        //    GetRandomByRNGCryptoServiceProvider(RandomArray);

        //    for (int i = 0; i < RandomCycleTime; i++)
        //    {
        //        //如果随机结果大于抽检比例
        //        if (RandomArray[i] <= (Seletion_Ratio * 10000))
        //        {
        //            CountResult = CountResult + 1;
        //        }
        //    }

        //    //如果随机总结果大于抽检比例，则返回True
        //    if ((CountResult) >= (Seletion_Ratio * RandomCycleTime))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //使用RNGCryptoServiceProvider产生的种子生成真随机数
        static void GetRandomByRNGCryptoServiceProvider(int[] array)
        {
            int len = array.Length;
            Random random = new Random(GetRandomSeed());
            for (int i = 0; i < len; i++)
            {
                array[i] = random.Next(0, 10000);
            }
        }

        // 使用RNGCryptoServiceProvider生成种子
        static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);

        }

        //选择路径文件

        public string AnalyticPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择数据源文件";
            openFileDialog.Filter = "xml文件|*.xml";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "txt";
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return "";
            }
            string txtFile = openFileDialog.FileName;

            return txtFile;
        }

        //解析单条搬送路径
        public void Analysis_Transfer_Per(string path ,string transfer)
        {
            
            //前一站Index
            string Index_Pre = "";

            //AMHS搬送路径
            string AMHS_Transfer = "";

            //将搬送路径按照箭头拆分
            List<string> list = transfer.Split(new char[1] { '-' }).ToList<string>();

            foreach (var item in list)
            {
                string[] sarray= item.Split(':');

                //判断当前是不是第一
                if (sarray[0] == "1")
                {
                    AMHS_Transfer = AMHS_Transfer + STK_Contain_Index[sarray[2]] + "-ZONE" + ">" + sarray[2] ;

                    //路径解析写入
                    Index_Pre = sarray[2];
                }
                else
                {
                    if (sarray[0]=="19999")
                    {
                        AMHS_Transfer = AMHS_Transfer + ">" + STK_Contain_Index[Index_Pre] + "-ZONE";
                    }
                    else
                    {
                        //如果同STK内搬送
                        if (STK_Contain_Index[Index_Pre] == STK_Contain_Index[sarray[2]])
                        {
                            //路径解析写入
                            AMHS_Transfer = AMHS_Transfer+ ">"+ STK_Contain_Index[Index_Pre] + "-ZONE" + ">" + sarray[2];

                            //前站Inde更新
                            Index_Pre = sarray[2];
                        }
                        //如果不同STK内搬送
                        else
                        {
                            //路径解析写入
                            AMHS_Transfer = AMHS_Transfer + ">" + STK_Contain_Index[Index_Pre] + "-ZONE" + ">" + "OHS" + ">" + STK_Contain_Index[sarray[2]] + "-ZONE" + ">" + sarray[2];

                            //前站Inde更新
                            Index_Pre = sarray[2];
                        }
                    }
                }
            }

            //写入到Log记录中
            File.AppendAllText(file_Transfer, AMHS_Transfer+"\n");
        }

        //解析搬送路径的主程序
        public void Analysis_Transfer_Log()
        {
            //选择需要解析的Index站点文件
            //string path = AnalyticPath();

            string  path = file;

            //读取文件中的每一行
            foreach (string str in System.IO.File.ReadAllLines(path, Encoding.Default))
            {
                if (str!="")
                {
                    Analysis_Transfer_Per(file_Transfer, str);
                }
            }
        }

        public void Count_Transfer_All()
        {
            //选择需要解析的Index站点文件
            //string path = AnalyticPath();

            string path = file_Transfer;

            //读取文件中的每一行
            foreach (string str in System.IO.File.ReadAllLines(path, Encoding.Default))
            {
                if (str != "")
                {
                    //拆分各个路径点
                    string[] sarray = str.Split('>');

                    for (int i = 0; i < sarray.Length-1; i++)
                    {
                        Transfer_Count[sarray[i] + "\t" + sarray[i + 1]] = Transfer_Count[sarray[i] + "\t" + sarray[i + 1]] + 1;
                    }
                }
            }

            //将结果写入到txt文件中
            File.AppendAllText(file_Transfer_Count_Per, "StartPosition" + "\t" + "EndPosition" + "\t" + "TotalNumberOfTransfers" + "\t" + "NumberOfTransportsPerHour" + "\n");

            foreach (var item in Transfer_Count)
            {
                File.AppendAllText(file_Transfer_Count_Per, item.Key+"\t"+item.Value + "\t" + ((float)item.Value/ (AllTime*24)).ToString("0.0000") + "\n");
            }
        }

        //开始
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Process_Station.Count != 0 && STK_Contain_Index.Count != 0)
            {
                //初始化
                //Instil();

                //加载Index对应STK信息
                //LoadIndexInfoOfStk();

                //加载站点信息
                // LoadProcessStation();

                //读取设定参数
                ConvertToNumber();

                //初始化 每个Index处理CST总数目
                LoadCST_Number_Count();

                //初始化 各个制程处理CST的数据
                Load_CST_Number_Count_Process();

                //初始化 各个搬送路径列表
                Load_Transfer_Count_All();

                //开始进程
                Thread thread1 = new Thread(CycleSimulation);
                thread1.Start();

                //委托主线程进行更改
                Start.Dispatcher.Invoke(new Action(
                delegate
                {
                // ------- 需要完成什么操作,写在这里就可以了, 主线程会触发该Action来完成-------
                Start.IsEnabled = false;
                    btn_ProcessData.IsEnabled = false;
                    btn_IndexInfo.IsEnabled = false;
                    CST_Count.IsEnabled = false;
                    Time_All.IsEnabled = false;
                    Index_Wight_InSTK.IsEnabled = false;
                    Index_Wight_OutSTK.IsEnabled = false;
                    ProcessData.IsEnabled = false;
                    IndexInfo.IsEnabled = false;
                    ResultCopy.Visibility = Visibility.Visible;
                })
                );

            }
            else
            {
                TextShow_Index.Show = TextShow_Index.Show + "数据未导入完全！\n";
            }
            
        }

        //加载制程信息
        private void btn_ProcessData_Click(object sender, RoutedEventArgs e)
        {
            //选择需要解析的Index站点文件
            string path = AnalyticPath();

            //显示路径信息
            ProcessData.Text = path;

            try
            {
                //读取文件中的每一行
                foreach (string str in System.IO.File.ReadAllLines(path, Encoding.Default))
                {
                    if (str != "")
                    {
                        //拆分各个路径点
                        string[] sarray = str.Split('\t');
                        if (sarray.Length == 4)
                        {
                            //写入制程信息
                            Process_Station.Enqueue(new StationModel { Number = Convert.ToInt32(sarray[0]), Process_Station_Number = sarray[1], Selection_Ratio = Convert.ToDouble(sarray[2]), Selection_Index_String_All = sarray[3] });
                        }

                    }
                }

                //写入Index可选列表 信息
                foreach (var item in Process_Station)
                {
                    //拆分可选Index字符串，成数组
                    string[] arr = item.Selection_Index_String_All.Split('/');

                    //实例化 可选Index列表
                    item.Selection_Index_List_All = new List<IndexModel>();

                    //写入当前站点可选Index列表
                    foreach (var id in arr)
                    {
                        IndexModel indexModel = new IndexModel();
                        indexModel.IndexID = id;
                        item.Selection_Index_List_All.Add(indexModel);
                    }
                }
                TextShow_Index.Show = TextShow_Index.Show + "制程信息导入完成！\n";
                Console.WriteLine("制程信息导入完成！\n");
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + "制程信息导入失败！\n";
                Console.WriteLine("制程信息导入失败！\n");
                //throw;
            }
            
            
        }

        //加载Index所属STK信息
        private void btn_IndexInfo_Click(object sender, RoutedEventArgs e)
        {
            //选择需要解析的Index站点文件
            string path = AnalyticPath();

            //显示路径信息
            IndexInfo.Text = path;

            try
            {
                //读取文件中的每一行
                foreach (string str in System.IO.File.ReadAllLines(path, Encoding.Default))
                {
                    if (str != "")
                    {
                        //拆分各个路径点
                        string[] sarray = str.Split('\t');
                        if (sarray.Length == 2)
                        {
                            //判断字典里是否已经存在
                            if (!STK_Contain_Index.ContainsKey(sarray[0]))
                            {
                                //写入Index所属STK信息,0为Index ID ；1为所属STK ID
                                STK_Contain_Index.Add(sarray[0], sarray[1]);
                            }

                            //判断列表里是否存在，
                            if (!STK_List.Contains(sarray[1]))
                            {
                                //加载STK列表信息
                                STK_List.Add(sarray[1]);
                            }
                        }

                    }
                }
                TextShow_Index.Show = TextShow_Index.Show + "Index所属STK信息加载完成！\n";
                Console.WriteLine("Index所属STK信息加载完成！\n");
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + "Index所属STK信息加载失败！\n";
                Console.WriteLine("Index所属STK信息加载失败！\n");
                //throw;
            }

        }

        //字符转换成数字
        public void ConvertToNumber()
        {
        //定义循环次数-投入CST数量
        Cycle_Time = Convert.ToInt32(CST_Count.Text);

        //定义STK内部Index的权重
        Weight_In_STK = Convert.ToInt32(Index_Wight_InSTK.Text);

        //定义STK外部Index的权重
        Weight_Outside_STK = Convert.ToInt32(Index_Wight_OutSTK.Text);

        //周期次数，一般按照30天算
        AllTime = Convert.ToInt32(Time_All.Text);
    }

        //初始化所有的参数
        public void Instil()
        {
        //声明 制程各站点信息：序号、站点、可选机台
        Process_Station = new Queue<StationModel>();

        //声明 Index与STK所属关系
        STK_Contain_Index = new Dictionary<string, string>();

        //声明 单个Index处理全部制程的总数
        CST_Number_Count_Index = new Dictionary<string, int>();

        //声明 每个制程处理CST的总数，用来核算抽检频率
        CST_Number_Count_Process = new Dictionary<string, int>();

        ////显示界面的内容-路径
        //TextShow_Path = new MyTextshow();

        ////显示界面的内容-Index处理CST数量
        //TextShow_Index = new MyTextshow();

        //定义循环次数-投入CST数量
        Cycle_Time=0;

        //定义STK内部Index的权重
        Weight_In_STK=0;

        //定义STK外部Index的权重
        Weight_Outside_STK=0;

        //定义文件保存路径和文件名
        file = System.Environment.CurrentDirectory + "\\ProcessesLog" + "\\ProcessesLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //定义文件保存路径和文件名
        file_Transfer = System.Environment.CurrentDirectory + "\\TransferLog" + "\\TransferLog" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //定义路径搬送量
        file_Transfer_Count_Per = System.Environment.CurrentDirectory + "\\TransferCountPer" + "\\TransferCountPer" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";

        //随机数生成次数
        RandomCycleTime = 10000;

        //周期次数，一般按1个月算
        AllTime=0;

        //定义 STK list
        STK_List = new List<string>();

        //定义 各个搬送路径的搬送量
        Transfer_Count = new Dictionary<string, int>();
    }

        private void Window_Closed(object sender, EventArgs e)
        {
            _0声明.mainSM.Show();
        }

        //获得文件夹路径并且另存
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;

                string path = foldPath + "\\模拟结果" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

                foreach (string str in System.IO.File.ReadAllLines(file_Transfer_Count_Per, Encoding.Default))
                {
                    if (str != "")
                    {
                        string Sarray = str.Replace('\t',',');
                        File.AppendAllText(path, Sarray + "\n");
                    }
                }
            }
        }
    }
}
