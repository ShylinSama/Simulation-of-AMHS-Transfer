using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulation_of_AMHS_Transfer
{
    //制程模型
    public class ProcessModel
    {
        //所有制程的站号队列
        public Queue<StationModel> Process_Station { get; set; }

    }

    //站点模型
    public class  StationModel
    {
        //序号
        public int Number { get; set; }

        //站号
        public string Process_Station_Number { get; set; }

        //抽检比率
        public Double Selection_Ratio { get; set; }

        //可选择Index列表
        public List<IndexModel> Selection_Index_List_All { get; set; }

        //可选择Index字符串
        public string Selection_Index_String_All { get; set; }

    }

    //Index模型
    public class IndexModel
    {
        //IndexID
        public string IndexID { get; set; }

        //Index累计处理CST的数目和
        public double CST_Number_Count { get; set; }

        //Index所属STK
        public string In_STK { get; set; }

        //Index在所有站点处理1个CST需要的时间
        public Dictionary<string, string> Process_Time { get; set; }

    }
}
