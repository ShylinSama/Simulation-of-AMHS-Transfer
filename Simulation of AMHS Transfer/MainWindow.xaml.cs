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

        public MainWindow()
        {
            InitializeComponent();

            Thread thread1 = new Thread(Simulation);
            thread1.Start();
        }

        //主程序
        public void Simulation()
        {
            LoadIndexInfoOfStk();

            LoadProcessStation();

            string ProcessPath;

            foreach (var ProcessStation in Process_Station)
            {
                ProcessStation.Selection_Index_List_All
            }
        }

        //加载各个制程站点信息
        public void LoadProcessStation()
        {

            //示例
            //Process_Station.Enqueue(new StationModel { 
            //    Number=1,
            //    Process_Station_Number="10000",
            //    Selection_Ratio=1,
            //    Selection_Index_String_All="2AIC01",
            //    Selection_Index_List_All=new List<IndexModel> {}
            //});

            Process_Station.Enqueue(new StationModel { Number = 1, Process_Station_Number = "10000", Selection_Ratio = 1, Selection_Index_String_All = "2ACI01" });
            Process_Station.Enqueue(new StationModel { Number = 2, Process_Station_Number = "10010", Selection_Ratio = 0.3333, Selection_Index_String_All = "2AMP01" });
            Process_Station.Enqueue(new StationModel { Number = 3, Process_Station_Number = "10400", Selection_Ratio = 1, Selection_Index_String_All = "2AIC01/2AIC02/2AIC03" });
            Process_Station.Enqueue(new StationModel { Number = 4, Process_Station_Number = "10800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV01/2AOV02" });
            Process_Station.Enqueue(new StationModel { Number = 5, Process_Station_Number = "10820", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 6, Process_Station_Number = "10840", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 7, Process_Station_Number = "10870", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 8, Process_Station_Number = "10876", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 9, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
            Process_Station.Enqueue(new StationModel { Number = 10, Process_Station_Number = "10070", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 11, Process_Station_Number = "10076", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 12, Process_Station_Number = "10020", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 13, Process_Station_Number = "10100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC01/2AFC02" });
            Process_Station.Enqueue(new StationModel { Number = 14, Process_Station_Number = "10140", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 15, Process_Station_Number = "10170", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 16, Process_Station_Number = "10176", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 17, Process_Station_Number = "10120", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 18, Process_Station_Number = "10401", Selection_Ratio = 1, Selection_Index_String_All = "2AIC01/2AIC02/2AIC03" });
            Process_Station.Enqueue(new StationModel { Number = 19, Process_Station_Number = "10801", Selection_Ratio = 1, Selection_Index_String_All = "2AOV01/2AOV02" });
            Process_Station.Enqueue(new StationModel { Number = 20, Process_Station_Number = "10821", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 21, Process_Station_Number = "10841", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 22, Process_Station_Number = "10871", Selection_Ratio = 1, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 23, Process_Station_Number = "10877", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 24, Process_Station_Number = "10005", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
            Process_Station.Enqueue(new StationModel { Number = 25, Process_Station_Number = "10071", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 26, Process_Station_Number = "10077", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 27, Process_Station_Number = "10021", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 28, Process_Station_Number = "10081", Selection_Ratio = 1, Selection_Index_String_All = "2ATB01/2ATB02" });
            Process_Station.Enqueue(new StationModel { Number = 29, Process_Station_Number = "10101", Selection_Ratio = 1, Selection_Index_String_All = "2AFC01/2AFC02" });
            Process_Station.Enqueue(new StationModel { Number = 30, Process_Station_Number = "10141", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 31, Process_Station_Number = "10121", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 32, Process_Station_Number = "10171", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 33, Process_Station_Number = "10177", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 34, Process_Station_Number = "11100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC06" });
            Process_Station.Enqueue(new StationModel { Number = 35, Process_Station_Number = "11140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 36, Process_Station_Number = "11170", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 37, Process_Station_Number = "11120", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 38, Process_Station_Number = "11176", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 39, Process_Station_Number = "11300", Selection_Ratio = 1, Selection_Index_String_All = "2AFE01/2AFE02/2AFE05/2AFE06" });
            Process_Station.Enqueue(new StationModel { Number = 40, Process_Station_Number = "11370", Selection_Ratio = 0.5, Selection_Index_String_All = "2AMA02" });
            Process_Station.Enqueue(new StationModel { Number = 41, Process_Station_Number = "11376", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 42, Process_Station_Number = "11320", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 43, Process_Station_Number = "113A0", Selection_Ratio = 1, Selection_Index_String_All = "2AMF01" });
            Process_Station.Enqueue(new StationModel { Number = 44, Process_Station_Number = "11400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP02" });
            Process_Station.Enqueue(new StationModel { Number = 45, Process_Station_Number = "11450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 46, Process_Station_Number = "11451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 47, Process_Station_Number = "11430", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMT01" });
            Process_Station.Enqueue(new StationModel { Number = 48, Process_Station_Number = "11431", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMT01" });
            Process_Station.Enqueue(new StationModel { Number = 49, Process_Station_Number = "11432", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMT01/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 50, Process_Station_Number = "11470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 51, Process_Station_Number = "11476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 52, Process_Station_Number = "11420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 53, Process_Station_Number = "11500", Selection_Ratio = 1, Selection_Index_String_All = "2AED01" });
            Process_Station.Enqueue(new StationModel { Number = 54, Process_Station_Number = "11570", Selection_Ratio = 1, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 55, Process_Station_Number = "11600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 56, Process_Station_Number = "11650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 57, Process_Station_Number = "11670", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 58, Process_Station_Number = "11676", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 59, Process_Station_Number = "11620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 60, Process_Station_Number = "11681", Selection_Ratio = 1, Selection_Index_String_All = "2ATB01/2ATB02/2ATR01/2ATR02/2ATR03/2ATR04/2ATR07" });
            Process_Station.Enqueue(new StationModel { Number = 61, Process_Station_Number = "11632", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMT01/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 62, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
            Process_Station.Enqueue(new StationModel { Number = 63, Process_Station_Number = "11700", Selection_Ratio = 1, Selection_Index_String_All = "2AEI01/2AEI02" });
            Process_Station.Enqueue(new StationModel { Number = 64, Process_Station_Number = "11720", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 65, Process_Station_Number = "12100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC07" });
            Process_Station.Enqueue(new StationModel { Number = 66, Process_Station_Number = "12140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 67, Process_Station_Number = "12170", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 68, Process_Station_Number = "12176", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 69, Process_Station_Number = "12120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 70, Process_Station_Number = "12200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS01" });
            Process_Station.Enqueue(new StationModel { Number = 71, Process_Station_Number = "12260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
            Process_Station.Enqueue(new StationModel { Number = 72, Process_Station_Number = "12270", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 73, Process_Station_Number = "12276", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 74, Process_Station_Number = "12220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 75, Process_Station_Number = "12400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP04/2APP01/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 76, Process_Station_Number = "12450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 77, Process_Station_Number = "12451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 78, Process_Station_Number = "12452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 79, Process_Station_Number = "12470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 80, Process_Station_Number = "12476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 81, Process_Station_Number = "12420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 82, Process_Station_Number = "12500", Selection_Ratio = 1, Selection_Index_String_All = "2AED04/2AED05" });
            Process_Station.Enqueue(new StationModel { Number = 83, Process_Station_Number = "12600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 84, Process_Station_Number = "12650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 85, Process_Station_Number = "12670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 86, Process_Station_Number = "12676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 87, Process_Station_Number = "12620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 88, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
            Process_Station.Enqueue(new StationModel { Number = 89, Process_Station_Number = "12700", Selection_Ratio = 1, Selection_Index_String_All = "2AEI01/2AEI02" });
            Process_Station.Enqueue(new StationModel { Number = 90, Process_Station_Number = "12720", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 91, Process_Station_Number = "13100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC08" });
            Process_Station.Enqueue(new StationModel { Number = 92, Process_Station_Number = "13140", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 93, Process_Station_Number = "13170", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 94, Process_Station_Number = "13176", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 95, Process_Station_Number = "13120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 96, Process_Station_Number = "13200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS01" });
            Process_Station.Enqueue(new StationModel { Number = 97, Process_Station_Number = "13260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
            Process_Station.Enqueue(new StationModel { Number = 98, Process_Station_Number = "13270", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 99, Process_Station_Number = "13276", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 100, Process_Station_Number = "13220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 101, Process_Station_Number = "13400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP01/2APP04/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 102, Process_Station_Number = "13450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 103, Process_Station_Number = "13451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 104, Process_Station_Number = "13452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 105, Process_Station_Number = "13470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 106, Process_Station_Number = "13476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 107, Process_Station_Number = "13420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 108, Process_Station_Number = "13500", Selection_Ratio = 1, Selection_Index_String_All = "2AED04/2AED05" });
            Process_Station.Enqueue(new StationModel { Number = 109, Process_Station_Number = "13600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 110, Process_Station_Number = "13650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 111, Process_Station_Number = "13670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 112, Process_Station_Number = "13676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 113, Process_Station_Number = "13620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 114, Process_Station_Number = "14101", Selection_Ratio = 1, Selection_Index_String_All = "2AFC10" });
            Process_Station.Enqueue(new StationModel { Number = 115, Process_Station_Number = "14100", Selection_Ratio = 1, Selection_Index_String_All = "2AFC10" });
            Process_Station.Enqueue(new StationModel { Number = 116, Process_Station_Number = "14140", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 117, Process_Station_Number = "14170", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 118, Process_Station_Number = "14176", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 119, Process_Station_Number = "14120", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 120, Process_Station_Number = "10002", Selection_Ratio = 1, Selection_Index_String_All = "2ACP01" });
            Process_Station.Enqueue(new StationModel { Number = 121, Process_Station_Number = "14800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV04/2AOV05/2AOV06" });
            Process_Station.Enqueue(new StationModel { Number = 122, Process_Station_Number = "14870", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 123, Process_Station_Number = "14876", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 124, Process_Station_Number = "14820", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 125, Process_Station_Number = "14400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP06/2APP01/2APP02" });
            Process_Station.Enqueue(new StationModel { Number = 126, Process_Station_Number = "14450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 127, Process_Station_Number = "14451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 128, Process_Station_Number = "14452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 129, Process_Station_Number = "14470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 130, Process_Station_Number = "14476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 131, Process_Station_Number = "14420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 132, Process_Station_Number = "14500", Selection_Ratio = 1, Selection_Index_String_All = "2AED08/2AED09" });
            Process_Station.Enqueue(new StationModel { Number = 133, Process_Station_Number = "14600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 134, Process_Station_Number = "14650", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 135, Process_Station_Number = "14670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 136, Process_Station_Number = "14676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 137, Process_Station_Number = "14620", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 138, Process_Station_Number = "14801", Selection_Ratio = 1, Selection_Index_String_All = "2AOV05/2AOV06" });
            Process_Station.Enqueue(new StationModel { Number = 139, Process_Station_Number = "14821", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 140, Process_Station_Number = "17200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS03" });
            Process_Station.Enqueue(new StationModel { Number = 141, Process_Station_Number = "17260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
            Process_Station.Enqueue(new StationModel { Number = 142, Process_Station_Number = "17270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 143, Process_Station_Number = "17276", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 144, Process_Station_Number = "17220", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 145, Process_Station_Number = "17400", Selection_Ratio = 1, Selection_Index_String_All = "2APP04/2APP02/2APP01/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 146, Process_Station_Number = "17450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 147, Process_Station_Number = "17451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 148, Process_Station_Number = "17452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 149, Process_Station_Number = "17470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 150, Process_Station_Number = "17476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 151, Process_Station_Number = "17420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 152, Process_Station_Number = "17500", Selection_Ratio = 1, Selection_Index_String_All = "2AED12" });
            Process_Station.Enqueue(new StationModel { Number = 153, Process_Station_Number = "17600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 154, Process_Station_Number = "17650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 155, Process_Station_Number = "17670", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 156, Process_Station_Number = "17676", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 157, Process_Station_Number = "17620", Selection_Ratio = 1, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 158, Process_Station_Number = "17682", Selection_Ratio = 1, Selection_Index_String_All = "2ATL01" });
            Process_Station.Enqueue(new StationModel { Number = 159, Process_Station_Number = "17690", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATE01/2ATE02" });
            Process_Station.Enqueue(new StationModel { Number = 160, Process_Station_Number = "15400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP06/2APP04/2APP02" });
            Process_Station.Enqueue(new StationModel { Number = 161, Process_Station_Number = "15450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 162, Process_Station_Number = "15451", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 163, Process_Station_Number = "15452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 164, Process_Station_Number = "15470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 165, Process_Station_Number = "15476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 166, Process_Station_Number = "15420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 167, Process_Station_Number = "15500", Selection_Ratio = 1, Selection_Index_String_All = "2AED08/2AED09" });
            Process_Station.Enqueue(new StationModel { Number = 168, Process_Station_Number = "15600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 169, Process_Station_Number = "15650", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 170, Process_Station_Number = "15670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 171, Process_Station_Number = "15676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 172, Process_Station_Number = "15620", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 173, Process_Station_Number = "15830", Selection_Ratio = 0.0714, Selection_Index_String_All = "2AMT01" });
            Process_Station.Enqueue(new StationModel { Number = 174, Process_Station_Number = "19400", Selection_Ratio = 1, Selection_Index_String_All = "2APP05/2APP03/2APP01/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 175, Process_Station_Number = "19450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 176, Process_Station_Number = "19451", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 177, Process_Station_Number = "19452", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 178, Process_Station_Number = "19470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 179, Process_Station_Number = "19476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 180, Process_Station_Number = "19420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 181, Process_Station_Number = "19800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
            Process_Station.Enqueue(new StationModel { Number = 182, Process_Station_Number = "19840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 183, Process_Station_Number = "19870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 184, Process_Station_Number = "19876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 185, Process_Station_Number = "19820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 186, Process_Station_Number = "19900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
            Process_Station.Enqueue(new StationModel { Number = 187, Process_Station_Number = "19920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 188, Process_Station_Number = "1A201", Selection_Ratio = 1, Selection_Index_String_All = "2ACP03" });
            Process_Station.Enqueue(new StationModel { Number = 189, Process_Station_Number = "1A200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS03" });
            Process_Station.Enqueue(new StationModel { Number = 190, Process_Station_Number = "1A260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
            Process_Station.Enqueue(new StationModel { Number = 191, Process_Station_Number = "1A270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 192, Process_Station_Number = "1A276", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 193, Process_Station_Number = "1A220", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 194, Process_Station_Number = "1A400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP06/2APP04/2APP02" });
            Process_Station.Enqueue(new StationModel { Number = 195, Process_Station_Number = "1A450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 196, Process_Station_Number = "1A451", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 197, Process_Station_Number = "1A452", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 198, Process_Station_Number = "1A470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 199, Process_Station_Number = "1A476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 200, Process_Station_Number = "1A420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 201, Process_Station_Number = "1A500", Selection_Ratio = 1, Selection_Index_String_All = "2AED12" });
            Process_Station.Enqueue(new StationModel { Number = 202, Process_Station_Number = "1A600", Selection_Ratio = 1, Selection_Index_String_All = "2AES02/2AES03/2AES04" });
            Process_Station.Enqueue(new StationModel { Number = 203, Process_Station_Number = "1A650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 204, Process_Station_Number = "1A670", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 205, Process_Station_Number = "1A676", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 206, Process_Station_Number = "1A620", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 207, Process_Station_Number = "1B400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP03/2APP05/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 208, Process_Station_Number = "1B450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 209, Process_Station_Number = "1B451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 210, Process_Station_Number = "1B452", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 211, Process_Station_Number = "1B470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 212, Process_Station_Number = "1B476", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 213, Process_Station_Number = "1B420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 214, Process_Station_Number = "1B800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
            Process_Station.Enqueue(new StationModel { Number = 215, Process_Station_Number = "1B840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 216, Process_Station_Number = "1B870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 217, Process_Station_Number = "1B876", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 218, Process_Station_Number = "1B820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 219, Process_Station_Number = "1B900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
            Process_Station.Enqueue(new StationModel { Number = 220, Process_Station_Number = "1B920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 221, Process_Station_Number = "1B400", Selection_Ratio = 1, Selection_Index_String_All = "2APP01/2APP03/2APP05/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 222, Process_Station_Number = "1B450", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 223, Process_Station_Number = "1B451", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 224, Process_Station_Number = "1B452", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 225, Process_Station_Number = "1B470", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 226, Process_Station_Number = "1B476", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 227, Process_Station_Number = "1B420", Selection_Ratio = 0.25, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 228, Process_Station_Number = "1B840", Selection_Ratio = 0.0909, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 229, Process_Station_Number = "1B870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 230, Process_Station_Number = "1B876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 231, Process_Station_Number = "1B820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 232, Process_Station_Number = "1B900", Selection_Ratio = 1, Selection_Index_String_All = "2AED02/2AED05" });
            Process_Station.Enqueue(new StationModel { Number = 233, Process_Station_Number = "1B920", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 234, Process_Station_Number = "1C200", Selection_Ratio = 1, Selection_Index_String_All = "2AFS05" });
            Process_Station.Enqueue(new StationModel { Number = 235, Process_Station_Number = "1C260", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMR01" });
            Process_Station.Enqueue(new StationModel { Number = 236, Process_Station_Number = "1C270", Selection_Ratio = 0.1667, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 237, Process_Station_Number = "1C276", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 238, Process_Station_Number = "1C220", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 239, Process_Station_Number = "1C400", Selection_Ratio = 1, Selection_Index_String_All = "2APP02/2APP01/2APP04/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 240, Process_Station_Number = "1C450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 241, Process_Station_Number = "1C470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 242, Process_Station_Number = "1C476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 243, Process_Station_Number = "1C420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 244, Process_Station_Number = "1C500", Selection_Ratio = 1, Selection_Index_String_All = "2AEW01" });
            Process_Station.Enqueue(new StationModel { Number = 245, Process_Station_Number = "1C501", Selection_Ratio = 1, Selection_Index_String_All = "2AEW02/2AEW03" });
            Process_Station.Enqueue(new StationModel { Number = 246, Process_Station_Number = "1C600", Selection_Ratio = 1, Selection_Index_String_All = "2AES01" });
            Process_Station.Enqueue(new StationModel { Number = 247, Process_Station_Number = "1C650", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 248, Process_Station_Number = "1C670", Selection_Ratio = 0.1111, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 249, Process_Station_Number = "1C676", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 250, Process_Station_Number = "1C620", Selection_Ratio = 0.1667, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 251, Process_Station_Number = "1H400", Selection_Ratio = 1, Selection_Index_String_All = "2APP03/2APP01/2APP05/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 252, Process_Station_Number = "1H440", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 253, Process_Station_Number = "1H450", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 254, Process_Station_Number = "1H470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 255, Process_Station_Number = "1H476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 256, Process_Station_Number = "1H420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 257, Process_Station_Number = "1H800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
            Process_Station.Enqueue(new StationModel { Number = 258, Process_Station_Number = "1H840", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 259, Process_Station_Number = "1H841", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 260, Process_Station_Number = "1H850", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 261, Process_Station_Number = "1H870", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 262, Process_Station_Number = "1H876", Selection_Ratio = 0.0526, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 263, Process_Station_Number = "1H820", Selection_Ratio = 0.1429, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 264, Process_Station_Number = "1H400", Selection_Ratio = 1, Selection_Index_String_All = "2APP03/2APP01/2APP05/2APP06" });
            Process_Station.Enqueue(new StationModel { Number = 265, Process_Station_Number = "1H440", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 266, Process_Station_Number = "1H450", Selection_Ratio = 0.0769, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 267, Process_Station_Number = "1H470", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 268, Process_Station_Number = "1H476", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 269, Process_Station_Number = "1H420", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 270, Process_Station_Number = "1H800", Selection_Ratio = 1, Selection_Index_String_All = "2AOV08/2AOV10" });
            Process_Station.Enqueue(new StationModel { Number = 271, Process_Station_Number = "1H840", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 272, Process_Station_Number = "1H841", Selection_Ratio = 0.2, Selection_Index_String_All = "2AMS01/2AMS03" });
            Process_Station.Enqueue(new StationModel { Number = 273, Process_Station_Number = "1H850", Selection_Ratio = 0.1429, Selection_Index_String_All = "2AMC02/2AMC03/2AMC04/2AMC05" });
            Process_Station.Enqueue(new StationModel { Number = 274, Process_Station_Number = "1H870", Selection_Ratio = 0.25, Selection_Index_String_All = "2AMA01/2AMA04" });
            Process_Station.Enqueue(new StationModel { Number = 275, Process_Station_Number = "1H876", Selection_Ratio = 0.3333, Selection_Index_String_All = "2AMM01/2AMM03/2AMM05/2AMM09" });
            Process_Station.Enqueue(new StationModel { Number = 276, Process_Station_Number = "1H820", Selection_Ratio = 0.3333, Selection_Index_String_All = "2ATO01/2ATO04/2ATO05/2ATO07/2ATO09/2ATO10/2ATO13/2ATO14" });
            Process_Station.Enqueue(new StationModel { Number = 277, Process_Station_Number = "1H890", Selection_Ratio = 0.2, Selection_Index_String_All = "2ATE01/2ATE02" });

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

            //写入Index所属STK信息
            foreach (var item1 in Process_Station)
            {
                foreach (var item in item1.Selection_Index_List_All)
                {
                    item.In_STK = STK_Contain_Index[item.IndexID];
                }
            }

            //写入Index进行各个制程需要的时间


        }

        //加载Index所属STK信息
        public void LoadIndexInfoOfStk()
        {
            STK_Contain_Index.Add("2AIC01", "STK01");
            STK_Contain_Index.Add("2AIC02", "STK01");
            STK_Contain_Index.Add("2AIC03", "STK01");
            STK_Contain_Index.Add("2AIC04", "STK01");
            STK_Contain_Index.Add("2AIC05", "STK01");
            STK_Contain_Index.Add("2AOV01", "STK01");
            STK_Contain_Index.Add("2AOV02", "STK01");
            STK_Contain_Index.Add("2AOV03", "STK01");
            STK_Contain_Index.Add("2ATO01", "STK01");
            STK_Contain_Index.Add("2ACP01", "STK02");
            STK_Contain_Index.Add("2AED08", "STK02");
            STK_Contain_Index.Add("2AED09", "STK02");
            STK_Contain_Index.Add("2AED03", "STK02");
            STK_Contain_Index.Add("2AED11", "STK02");
            STK_Contain_Index.Add("2AFC10", "STK02");
            STK_Contain_Index.Add("2AMS01", "STK02");
            STK_Contain_Index.Add("2AFC04", "STK02");
            STK_Contain_Index.Add("2AMA01", "STK02");
            STK_Contain_Index.Add("2ATB01", "STK02");
            STK_Contain_Index.Add("2ATB02", "STK02");
            STK_Contain_Index.Add("2ATO02", "STK02");
            STK_Contain_Index.Add("2ATO03", "STK02");
            STK_Contain_Index.Add("2AMM01", "STK03");
            STK_Contain_Index.Add("2AFC01", "STK03");
            STK_Contain_Index.Add("2AFC02", "STK03");
            STK_Contain_Index.Add("2AFC12", "STK03");
            STK_Contain_Index.Add("2AFC03", "STK03");
            STK_Contain_Index.Add("2AFC13", "STK03");
            STK_Contain_Index.Add("2AFE07", "STK03");
            STK_Contain_Index.Add("2AFE06", "STK03");
            STK_Contain_Index.Add("2AFE05", "STK03");
            STK_Contain_Index.Add("2AMF01", "STK03");
            STK_Contain_Index.Add("2AMA02", "STK04");
            STK_Contain_Index.Add("2AML01", "STK04");
            STK_Contain_Index.Add("2AFE01", "STK04");
            STK_Contain_Index.Add("2AFE02", "STK04");
            STK_Contain_Index.Add("2AFE03", "STK04");
            STK_Contain_Index.Add("2AFE04", "STK04");
            STK_Contain_Index.Add("2AFC11", "STK04");
            STK_Contain_Index.Add("2AFC06", "STK04");
            STK_Contain_Index.Add("2AFC08", "STK04");
            STK_Contain_Index.Add("2AFC07", "STK04");
            STK_Contain_Index.Add("2ACP03", "STK04");
            STK_Contain_Index.Add("2ATO04", "STK05");
            STK_Contain_Index.Add("2ATO05", "STK05");
            STK_Contain_Index.Add("2AMS02", "STK05");
            STK_Contain_Index.Add("2ATR08", "STK05");
            STK_Contain_Index.Add("2ATR12", "STK05");
            STK_Contain_Index.Add("2ATO06", "STK05");
            STK_Contain_Index.Add("2ATE05", "STK05");
            STK_Contain_Index.Add("2AMM02", "STK05");
            STK_Contain_Index.Add("2AED04", "STK05");
            STK_Contain_Index.Add("2AED05", "STK05");
            STK_Contain_Index.Add("2AED06", "STK05");
            STK_Contain_Index.Add("2AMH01", "STK05");
            STK_Contain_Index.Add("2AEW04", "STK05");
            STK_Contain_Index.Add("2AEW05", "STK05");
            STK_Contain_Index.Add("2AMM03", "STK06");
            STK_Contain_Index.Add("2AED10", "STK06");
            STK_Contain_Index.Add("2AED07", "STK06");
            STK_Contain_Index.Add("2AED02", "STK06");
            STK_Contain_Index.Add("2AOV04", "STK06");
            STK_Contain_Index.Add("2AOV05", "STK06");
            STK_Contain_Index.Add("2AOV06", "STK06");
            STK_Contain_Index.Add("2AOV07", "STK06");
            STK_Contain_Index.Add("2ACP05", "STK06");
            STK_Contain_Index.Add("2AMS03", "STK07");
            STK_Contain_Index.Add("2AED01", "STK07");
            STK_Contain_Index.Add("2AED12", "STK07");
            STK_Contain_Index.Add("2AED13", "STK07");
            STK_Contain_Index.Add("2AEI04", "STK07");
            STK_Contain_Index.Add("2ATO08", "STK07");
            STK_Contain_Index.Add("2ATO07", "STK07");
            STK_Contain_Index.Add("2AOV11", "STK07");
            STK_Contain_Index.Add("2AMA03", "STK07");
            STK_Contain_Index.Add("2ATB03", "STK07");
            STK_Contain_Index.Add("2ATR05", "STK07");
            STK_Contain_Index.Add("2ATR06", "STK07");
            STK_Contain_Index.Add("2AOV10", "STK07");
            STK_Contain_Index.Add("2AMM05", "STK08");
            STK_Contain_Index.Add("2AMA04", "STK08");
            STK_Contain_Index.Add("2ATO09", "STK08");
            STK_Contain_Index.Add("2ATR07", "STK08");
            STK_Contain_Index.Add("2AMC01", "STK08");
            STK_Contain_Index.Add("2ATO17", "STK08");
            STK_Contain_Index.Add("2APP09", "STK08");
            STK_Contain_Index.Add("2APP08", "STK08");
            STK_Contain_Index.Add("2APP07", "STK08");
            STK_Contain_Index.Add("2APP06", "STK08");
            STK_Contain_Index.Add("2APP05", "STK08");
            STK_Contain_Index.Add("2AMS04", "STK08");
            STK_Contain_Index.Add("2APP01", "STK09");
            STK_Contain_Index.Add("2AMT01", "STK09");
            STK_Contain_Index.Add("2APP02", "STK09");
            STK_Contain_Index.Add("2APP03", "STK09");
            STK_Contain_Index.Add("2AMC02", "STK09");
            STK_Contain_Index.Add("2APP04", "STK09");
            STK_Contain_Index.Add("2ATO10", "STK09");
            STK_Contain_Index.Add("2ATE03", "STK09");
            STK_Contain_Index.Add("2ATE04", "STK09");
            STK_Contain_Index.Add("2AMM06", "STK09");
            STK_Contain_Index.Add("2ATO16", "STK09");
            STK_Contain_Index.Add("2ATR09", "STK09");
            STK_Contain_Index.Add("2ATR10", "STK09");
            STK_Contain_Index.Add("2ATR11", "STK09");
            STK_Contain_Index.Add("2ATO19", "STK09");
            STK_Contain_Index.Add("2AFS07", "STK10");
            STK_Contain_Index.Add("2AFS08", "STK10");
            STK_Contain_Index.Add("2AEI01", "STK10");
            STK_Contain_Index.Add("2AEI02", "STK10");
            STK_Contain_Index.Add("2AEI03", "STK10");
            STK_Contain_Index.Add("2ACP04", "STK10");
            STK_Contain_Index.Add("2ASS01", "STK10");
            STK_Contain_Index.Add("2ATL01", "STK11");
            STK_Contain_Index.Add("2AMM04", "STK11");
            STK_Contain_Index.Add("2ATO18", "STK11");
            STK_Contain_Index.Add("2ATO14", "STK11");
            STK_Contain_Index.Add("2ATR01", "STK11");
            STK_Contain_Index.Add("2ATR02", "STK11");
            STK_Contain_Index.Add("2ATR03", "STK11");
            STK_Contain_Index.Add("2ATR04", "STK11");
            STK_Contain_Index.Add("2ACP02", "STK11");
            STK_Contain_Index.Add("2AFS03", "STK11");
            STK_Contain_Index.Add("2AFS04", "STK11");
            STK_Contain_Index.Add("2AFS02", "STK12");
            STK_Contain_Index.Add("2AFS05", "STK12");
            STK_Contain_Index.Add("2AMY01", "STK12");
            STK_Contain_Index.Add("2AMR01", "STK12");
            STK_Contain_Index.Add("2AFS01", "STK12");
            STK_Contain_Index.Add("2AFS06", "STK12");
            STK_Contain_Index.Add("2AES05", "STK13");
            STK_Contain_Index.Add("2AES06", "STK13");
            STK_Contain_Index.Add("2AES07", "STK13");
            STK_Contain_Index.Add("2AEW01", "STK13");
            STK_Contain_Index.Add("2AEW02", "STK13");
            STK_Contain_Index.Add("2AEW03", "STK13");
            STK_Contain_Index.Add("2AES01", "STK13");
            STK_Contain_Index.Add("2AMC03", "STK13");
            STK_Contain_Index.Add("2ATO13", "STK13");
            STK_Contain_Index.Add("2ATO11", "STK13");
            STK_Contain_Index.Add("2ATO12", "STK13");
            STK_Contain_Index.Add("2ATO15", "STK13");
            STK_Contain_Index.Add("2ATO20", "STK13");
            STK_Contain_Index.Add("2AMM07", "STK13");
            STK_Contain_Index.Add("2AMM08", "STK13");
            STK_Contain_Index.Add("2AOV09", "STK14");
            STK_Contain_Index.Add("2AES08", "STK14");
            STK_Contain_Index.Add("2AES02", "STK14");
            STK_Contain_Index.Add("2AES03", "STK14");
            STK_Contain_Index.Add("2AES04", "STK14");
            STK_Contain_Index.Add("2AMC04", "STK14");
            STK_Contain_Index.Add("2ATT01", "STK14");
            STK_Contain_Index.Add("2ATT02", "STK14");
            STK_Contain_Index.Add("2ATT03", "STK14");
            STK_Contain_Index.Add("2ATT04", "STK14");
            STK_Contain_Index.Add("2AMC05", "STK14");
            STK_Contain_Index.Add("2APP10", "STK15");
            STK_Contain_Index.Add("2AMT02", "STK15");
            STK_Contain_Index.Add("2APP11", "STK15");
            STK_Contain_Index.Add("2AMC06", "STK15");
            STK_Contain_Index.Add("2APP12", "STK15");
            STK_Contain_Index.Add("2APP13", "STK15");
            STK_Contain_Index.Add("2AMM09", "STK15");
            STK_Contain_Index.Add("2AMP01", "STK15");
            STK_Contain_Index.Add("2ATE01", "STK15");
            STK_Contain_Index.Add("2ATE02", "STK15");
            STK_Contain_Index.Add("2AOV08", "STK15");
            STK_Contain_Index.Add("2ACI01", "STK15");

        }
    }
}
