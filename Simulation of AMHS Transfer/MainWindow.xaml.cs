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
        public static MainWindow main;

        //声明 制程各站点信息：序号、站点、可选机台
        public Queue<StationModel> Process_Station ;

        //声明 Index与STK所属关系
        public Dictionary<string, string> STK_Contain_Index;

        //声明 制程各站点信息：序号、站点、可选机台-------------旧数据
        public Queue<StationModel> Process_Station_OLD;

        //声明 Index与STK所属关系----------------旧数据
        public Dictionary<string, string> STK_Contain_Index_OLD;

        //声明 单个Index处理全部制程的总数
        public Dictionary<string, int> CST_Number_Count_Index;

        //声明 每个制程处理CST的总数，用来核算抽检频率
        public Dictionary<string, int> CST_Number_Count_Process;

        //定义循环次数-投入CST数量
        public int Cycle_Time;

        //定义STK内部Index的权重
        public int Weight_In_STK;

        //定义STK外部Index的权重
        public int Weight_Outside_STK;

        //随机数生成次数
        public int RandomCycleTime;

        //周期次数，一般按1个月算
        public int AllTime;

        //定义 STK list
        public List<string> STK_List;

        //定义 各个搬送路径的搬送量
        public Dictionary<string, int> Transfer_Count;

        //显示界面的内容-路径
        public MyTextshow TextShow_Path = new MyTextshow();

        //显示界面的内容-Index处理CST数量
        public MyTextshow TextShow_Index = new MyTextshow();

        //初始化Log保存的文件夹路径
        public string path;

        //定义文件保存路径和文件名
        public string file;

        //定义文件保存路径和文件名
        public string file_Transfer;

        //定义路径搬送量
        public string file_Transfer_Count_Per;

        //定义其他Log保存路径：每个机台进行CST的数目、随机比例
        public string OtherLog;

        //选择的制程文件保存路径
        public string path_制程信息;

        //选择的Index信息文件保存路径
        public string path_Index信息;

        public MainWindow()
        {
            InitializeComponent();
            main = this;

            //绑定数据
            TextShow_Index.show = "";
            Output_Index.DataContext = TextShow_Index;

            //按钮屏蔽
            ResultCopy.IsEnabled = false;
            ChangeData.IsEnabled = false;
        }

        //初始化所有的参数
        public void 初始化()
        {
            //初始化 制程各站点信息：序号、站点、可选机台
            Process_Station = new Queue<StationModel>();

            //初始化 Index与STK所属关系
            STK_Contain_Index = new Dictionary<string, string>();

            //初始化 单个Index处理全部制程的总数
            CST_Number_Count_Index = new Dictionary<string, int>();

            //初始化 每个制程处理CST的总数，用来核算抽检频率
            CST_Number_Count_Process = new Dictionary<string, int>();

            //定义循环次数-投入CST数量
            Cycle_Time = 0;

            //定义STK内部Index的权重
            Weight_In_STK = 0;

            //定义STK外部Index的权重
            Weight_Outside_STK = 0;

            //随机数生成次数
            RandomCycleTime = 10000;

            //周期次数，一般按1个月算
            AllTime = 0;

            //定义 STK list
            STK_List = new List<string>();

            //定义 各个搬送路径的搬送量
            Transfer_Count = new Dictionary<string, int>();

            //初始化Log保存的文件夹路径
            path = System.IO.Path.Combine(System.Environment.CurrentDirectory + "\\LOG", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));

            //创建Log文件夹
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            //初始化制程Log文件
            file = path + "\\ProcessesLog.txt";

            //初始化文件保存路径和文件名
            file_Transfer = path + "\\TransferLog.txt";

            //初始化路径搬送量
            file_Transfer_Count_Per = path + "\\TransferCountPer.txt";

            //初始化其他Log保存路径：每个机台进行CST的数目、随机比例
            OtherLog = path + "\\OtherLog.txt";

        }

        //将文本转换成数字，并赋值给变量
        public bool 加载数据_设定数据()
        {
            //定义返回值bool
            bool iReturn;
            //将字符型数字转换成int数字
            iReturn = ConvertToNumber(CST_Count.Text, out Cycle_Time) && ConvertToNumber(Index_Wight_InSTK.Text, out Weight_In_STK) && ConvertToNumber(Index_Wight_OutSTK.Text, out Weight_Outside_STK) && ConvertToNumber(Time_All.Text, out AllTime);

            //如果转换失败，输出提示
            if (iReturn)
            {
                return true;
            }
            else
            {
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString()+"\n";
                TextShow_Index.Show = TextShow_Index.Show + "输入设定存在非法字符！\n";
                return false;
            }
        }

        //加载制程和Index列表
        public bool 加载数据_导入数据()
        {
            bool iReturn;

            iReturn = 制程信息导入(path+"\\制程信息.xml",out Process_Station) && Index信息导入(path + "\\Index所属STK.xml", out STK_Contain_Index,out STK_List);

            if (iReturn)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //字符型数字转换成int数字通用的函数
        public bool ConvertToNumber(string InputText,out int OutputInt)
        {
            try
            {
                //正确转换数值
                OutputInt = Convert.ToInt32(InputText);

                //返回true
                return true;
            }
            catch (Exception)
            {
                //出错时输出的数值为0
                OutputInt = 0;

                return false;
                //throw;
            }
            
        }

        //循环模拟主函数
        public void CycleSimulation()
        {
            //循环进行路径生成
            for (int i = 0; i < Cycle_Time; i++)
            {
                Console.WriteLine(i);
                Simulation(Process_Station,file);
            }


            //选择处理的log
            Analysis_Transfer_Log();

            Count_Transfer_All();

            //导出OtherLog
            输出OtherLog(CST_Number_Count_Index, CST_Number_Count_Process, Cycle_Time);

            Dispatcher.Invoke(new Action(
            delegate
            {
                // ------- 需要完成什么操作,写在这里就可以了, 主线程会触发该Action来完成-------
                //按钮解除
                ResultCopy.IsEnabled = true;
                ChangeData.IsEnabled = true;
                Start.IsEnabled = true;
            })
            );
        }

        //将每个机台访问次数、各个制程的抽检比例写入到txt
        private bool 输出OtherLog(Dictionary<string, int> iCST_Number_Count_Index, Dictionary<string, int> iCST_Number_Count_Process,int iCycle_Time)
        {
            try
            {
                string Index_CST_Count = "";

                Index_CST_Count = Index_CST_Count + "各个机台访问次数：\n";

                foreach (var item in iCST_Number_Count_Index)
                {
                    Index_CST_Count = Index_CST_Count + item.Key + "\t" + (item.Value - 1).ToString() + "\n";
                }

                Index_CST_Count = Index_CST_Count + "各个制程的实际抽检比例：\n";

                foreach (var station in iCST_Number_Count_Process)
                {
                    Index_CST_Count = Index_CST_Count + station.Key + "\t" + (((double)station.Value / iCycle_Time)).ToString("0.0000") + "\n";
                }
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString()+"\n";
                TextShow_Index.Show = TextShow_Index.Show + "模拟完成！\n";
                TextShow_Index.Show = TextShow_Index.Show + "----------------------------\n";

                File.AppendAllText(OtherLog, Index_CST_Count);

                return true;
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString()+"\n";
                TextShow_Index.Show = TextShow_Index.Show + "OtherLog导出失败！\n";
                TextShow_Index.Show = TextShow_Index.Show + "----------------------------\n";
                return false;
                //throw;
            }

        }

        //主程序
        public void Simulation(Queue<StationModel> _Process_Station,string LogFile)
        {

            //定义搬送路径
            string ProcessPath="";

            //定义前一站点选择的 Index
            string SelectedIndex_Previous = "";

            //定义当前站点选择的 Index
            string SelectedIndex = "";

            foreach (var iProcessStation in _Process_Station)
            {
                //清除
                SelectedIndex = "";

                //获得逻辑判断结果→IndexID
                SelectedIndex = CheckIndex(iProcessStation, SelectedIndex_Previous);

                //如果反馈结果为空，说明此站跳过
                if (SelectedIndex=="")
                {

                }
                else
                {
                    //生成当前站信息 并合并到总机台路径中
                    ProcessPath = ProcessPath + iProcessStation.Number.ToString() + ":" + iProcessStation.Process_Station_Number + ":" + SelectedIndex + ":" + iProcessStation.Selection_Ratio.ToString() + "-";

                    SelectedIndex_Previous = SelectedIndex;
                }
            }

            //最后添加 结尾
            ProcessPath = ProcessPath + "19999\n";

            //输出各条路径Log
            File.AppendAllText(LogFile, ProcessPath);

        }


        //初始化每个Index 制程CST数目
        public void LoadCST_Number_Count()
        {
            //写入所有index ID，并初始化制程CST数目为1
            foreach (var item in STK_Contain_Index)
            {
                CST_Number_Count_Index.Add(item.Key,1);
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
        private string CheckIndex(StationModel CurrentStation,string SelectedIndexPrevious)
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

        private string AnalyticPath()
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
        private bool Analysis_Transfer_Per(string path ,string transfer)
        {
            try
            {
                //前一站Index
                string Index_Pre = "";

                //AMHS搬送路径
                string AMHS_Transfer = "";

                //将搬送路径按照箭头拆分
                List<string> list = transfer.Split(new char[1] { '-' }).ToList<string>();

                foreach (var item in list)
                {
                    string[] sarray = item.Split(':');

                    //判断当前是不是第一
                    if (sarray[0] == "1")
                    {
                        AMHS_Transfer = AMHS_Transfer + STK_Contain_Index[sarray[2]] + "-ZONE" + ">" + sarray[2];

                        //路径解析写入
                        Index_Pre = sarray[2];
                    }
                    else
                    {
                        if (sarray[0] == "19999")
                        {
                            AMHS_Transfer = AMHS_Transfer + ">" + STK_Contain_Index[Index_Pre] + "-ZONE";
                        }
                        else
                        {
                            //如果同STK内搬送
                            if (STK_Contain_Index[Index_Pre] == STK_Contain_Index[sarray[2]])
                            {
                                //路径解析写入
                                AMHS_Transfer = AMHS_Transfer + ">" + STK_Contain_Index[Index_Pre] + "-ZONE" + ">" + sarray[2];

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
                File.AppendAllText(file_Transfer, AMHS_Transfer + "\n");

                return true;
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "搬送路径解析失败！" + "\n";
                return false;
                //throw;
            }
            
        }

        //解析搬送路径的主程序
        private void Analysis_Transfer_Log()
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

        private void Count_Transfer_All()
        {
            //选择需要解析的Index站点文件
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
            ResultCopy.IsEnabled = false;
            ChangeData.IsEnabled = false;

            //将所有数据进行初始化，不包含消息显示内容
            初始化();

            //将本次导入数据保存到Log文件夹
            SaveXML();

            //将字符数字转换成数字，并赋值给变量
            加载数据_设定数据();

            //加载制程和Index列表
            加载数据_导入数据();

            //加载每个Index进行的制程，并对处理CST数目赋值1
            LoadCST_Number_Count();

            //加载制程字典
            Load_CST_Number_Count_Process();

            //加载总的搬送量
            Load_Transfer_Count_All();

            if (Process_Station.Count != 0 && STK_Contain_Index.Count != 0)
            {

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
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "数据未导入完全！\n";
            }
        }

        //加载制程信息
        private bool 制程信息导入(string path,out Queue<StationModel> iProcess_Station)
        {
            iProcess_Station = new Queue<StationModel>();
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
                            iProcess_Station.Enqueue(new StationModel { Number = Convert.ToInt32(sarray[0]), Process_Station_Number = sarray[1], Selection_Ratio = Convert.ToDouble(sarray[2]), Selection_Index_String_All = sarray[3] });
                        }
                    }
                }

                //写入Index可选列表 信息
                foreach (var item in iProcess_Station)
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
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "制程信息导入完成！\n";
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "制程信息导入失败！\n";
                //throw;
                return false;
            }

            return true;
        }

        //加载Index所属STK信息
        private bool Index信息导入(string path,out Dictionary<string, string> iSTK_Contain_Index, out List<string> iSTK_List)
        {
            iSTK_Contain_Index = new Dictionary<string, string>();
            iSTK_List=new List<string>();

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
                            if (!iSTK_Contain_Index.ContainsKey(sarray[0]))
                            {
                                //写入Index所属STK信息,0为Index ID ；1为所属STK ID
                                iSTK_Contain_Index.Add(sarray[0], sarray[1]);
                            }

                            //判断列表里是否存在，
                            if (!iSTK_List.Contains(sarray[1]))
                            {
                                //加载STK列表信息
                                STK_List.Add(sarray[1]);
                            }
                        }

                    }
                }
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "Index所属STK信息加载完成！\n";
                Console.WriteLine("Index所属STK信息加载完成！\n");
            }
            catch (Exception)
            {
                TextShow_Index.Show = TextShow_Index.Show + DateTime.Now.ToString() + "\n";
                TextShow_Index.Show = TextShow_Index.Show + "Index所属STK信息加载失败！\n";
                Console.WriteLine("Index所属STK信息加载失败！\n");
                //throw;
                return false;
            }
            return true;
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

        private void btn_导入数据(object sender, RoutedEventArgs e)
        {

        }

        private void btn_ProcessData_Click(object sender, RoutedEventArgs e)
        {
            //声明 制程各站点信息：序号、站点、可选机台
            Process_Station_OLD = new Queue<StationModel>();

            bool iReturn;

            path_制程信息 = AnalyticPath();

            //显示路径信息
            ProcessData.Text = path_制程信息;

            iReturn = 制程信息导入(path_制程信息, out Process_Station_OLD);

            
        }

        private void btn_IndexInfo_Click(object sender, RoutedEventArgs e)
        {
            bool iReturn;

            //声明 Index与STK所属关系
            STK_Contain_Index_OLD = new Dictionary<string, string>();

            path_Index信息 = AnalyticPath();

            //显示路径信息
            IndexInfo.Text = path_Index信息;

            iReturn = Index信息导入(path_Index信息, out STK_Contain_Index_OLD,out STK_List);
        }

        private void ChangeData_Click(object sender, RoutedEventArgs e)
        {
            //新建窗体
            _1数据修改 DataWindow = new _1数据修改();

            //显示新建窗体
            DataWindow.Show();
        }

        private bool SaveXML()
        {
            string path_xml_p;

            string path_xml_I;

            path_xml_p = path + "\\制程信息.xml";

            path_xml_I = path + "\\Index所属STK.xml";

            string str_P ="";

            string str_I = "";

            int i = 1;
            int m = 1;

            foreach (var item in Process_Station_OLD)
            {
                if (i< Process_Station_OLD.Count)
                {
                    str_P = str_P + item.Number + "\t" + item.Process_Station_Number + "\t" + item.Selection_Ratio.ToString("0.0000") + "\t" + item.Selection_Index_String_All + "\n";
                    i=i+1;
                }
                else
                {
                    str_P = str_P + item.Number + "\t" + item.Process_Station_Number + "\t" + item.Selection_Ratio.ToString("0.0000") + "\t" + item.Selection_Index_String_All;
                }
            }

            File.AppendAllText(path_xml_p, str_P);

            foreach (var item in STK_Contain_Index_OLD)
            {
                if (m<STK_Contain_Index_OLD.Count)
                {
                    str_I = str_I + item.Key + "\t" + item.Value + "\n";
                    m = m + 1;
                }
                else
                {
                    str_I = str_I + item.Key + "\t" + item.Value;
                }
                
            }
            
            File.AppendAllText(path_xml_I, str_I);

            return true;
        }

    }
}
