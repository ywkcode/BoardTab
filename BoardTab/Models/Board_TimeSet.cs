using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Models
{
    public class Board_TimeSet
    {
    }

    public class tempA
    {

        public Int64 mynum { get; set; } = 0;
        public string value { get; set; }
        public string classname { get; set; }
    }



    public class TimeListTemp
    {
        public List<tempA> timelist { get; set; }

        public List<tempA> timenum { get; set; }


        public TimeListTemp()
        {
            this.timelist = new List<tempA>();
            this.timenum = new List<tempA>();
        }
    }

    public class TimeSetTemp
    {
        public Int64 Tkid { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime EndDate { get; set; }
        public Int64 Targetnum { get; set; }
        public decimal Xiaolv { get; set; }

        public Int64 Sort { get; set; }
        public Int64 IsNext { get; set; }
    }

    public class TimeSetting
    {
        public Int64 Tkid { get; set; }
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public Int64 Targetnum { get; set; }
        public decimal Xiaolv { get; set; }
        public Int64 Sort { get; set; }
        public Int64 IsNext { get; set; } 
        public Int64 Version { get; set; } 
        public decimal XiaoLv { get; set; }
    }

    public class TimeSettingResponse: TimeSetting
    {
        public int LAY_TABLE_INDEX { get; set; }
    }

    public class OneMinute
    {
        public Int64 ActualNumd { get; set; } = 0;
        public Int64 ActualNumM { get; set; } = 0;

        //public Int64 ActualDisNumd { get; set; } = 0;
        //public Int64 ActualDisNumM { get; set; } = 0;

        public tempA ActualDisNumd { get; set; } = new tempA();

        public tempA ActualDisNumM { get; set; } = new tempA();

        public List<tempA> FActualList { get; set; } = new List<tempA>();

        public List<tempA> SActualList { get; set; } = new List<tempA>();
    }
    public class CurrentDayNum
    {
        public Int64 Tkid { get; set; }
        public string FromDate { get; set; }

        public string EndDate { get; set; }

        public Int64 Targetnum { get; set; }
    }
    public class CurrentDayNumRequest
    {
        public string Tkid { get; set; }
        public string FromDate { get; set; }

        public string EndDate { get; set; }

        public Int64 Targetnum { get; set; }
    }
    public class CurrentDayNumResponse: CurrentDayNum
    {
        public int LAY_TABLE_INDEX { get; set; }
    }
    public class FiveMinute
    {
        /// <summary>
        /// 当日计划数量
        /// </summary>
        public Int64  PlanNumd { get; set; } = 0;

        //本月计划总数
        public Int64 PlanNumM { get; set; } = 0;

        public Int64 CurrentPlanNumd { get; set; } = 0;



        public Int64 CurrentPlanNumM { get; set; } = 0;

        public string Message { get; set; }

        public string CurrentDay { get; set; }
    }

    public class CurrentData
    {
        public Int64 Tkid { get; set; }

        public DateTime Currentdate { get; set; }

        public Int64 TimeSettingId { get; set; }

        public Int64 ActualNum { get; set; }  

        public Int64 TargetNum { get; set; }

        public Int64 Version { get; set; }

        public decimal XiaoLv { get; set; }
    }

    public class BasicSetting
    {
        public Int64 Tkid { get; set; }

        public DateTime Basictime { get; set; }
    }

    public class BasicResponse
    {
        public Int64 Tkid { get; set; }

        public string Basictime { get; set; }
    }

    public class BasicDaySetting: BasicSetting
    {
        public Int64 PlannumD { get; set; }

        public Int64 Version { get; set; }
    }

    public class BasicDaySettingResponse : BasicDaySetting
    {
        public int LAY_TABLE_INDEX { get; set; }
    }

    public class BasicMonthSetting : BasicSetting
    {
        public Int64 PlannumM { get; set; }

        public Int64 CurrentplannumM { get; set; }
    }
    public class BasicMonthSettingResponse : BasicMonthSetting
    {
        public int LAY_TABLE_INDEX { get; set; }
    }
    public class BasicDaySettingRequest
    {
        public string Basictime { get; set; }

        public string PlannumD { get; set; }
    }

    public class BasicMonthSettingRequest
    {
        public string Basictime { get; set; }

        public string PlannumM { get; set; }

        public string CurrentplannumM { get; set; }
    }

    public class TimeSetRequest
    {
       
        public string FromDate { get; set; }
        public string EndDate { get; set; }
        public string Targetnum { get; set; }
       
    }

    public class ExportMessage
    { 
        public string A1 { get; set; } 
        public string A2 { get; set; }
        public string A3 { get; set; }
        public string A4 { get; set; }
        public string A5 { get; set; }
        public string A6 { get; set; }
        public string A7 { get; set; }
        public string A8 { get; set; }
        public string A9 { get; set; }
        public string A10 { get; set; }
        public string A11 { get; set; } 
        public string A12 { get; set; } 
        public string A13 { get; set; }
    }
     
}
