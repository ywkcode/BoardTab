using BoardTab.Common;
using BoardTab.Models;
using BoardTab.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Services
{
    public class BoardService : IBoardService
    {
        public bool AddCurrentNum()
        {
            //var currenttime = DateTime.Now;
            //var timesetingid = ConfigurationCache.TimeSetInCallist.FirstOrDefault(s => currenttime >= s.FromDate && currenttime <= s.EndDate)?.Tkid ?? 0;
            //if (timesetingid == 0)
            //{
            //    timesetingid = ConfigurationCache.TimeSetInCallist.OrderByDescending(s => s.EndDate).FirstOrDefault(s => currenttime >= s.EndDate)?.Tkid ?? 0;
            //    if (timesetingid == 0)
            //    {
            //        timesetingid = ConfigurationCache.TimeSetInCallist.OrderBy(s => s.FromDate).FirstOrDefault(s => currenttime <= s.FromDate)?.Tkid ?? 0;
            //        if (timesetingid == 0) return false;
            //    }
            //}
            //string sql = string.Format($"update CurrentData set actualnum=actualnum+1 where currentdate='{DateTime.Now.ToString("yyyy-MM-dd")}' and timesettingid={timesetingid}");
            var currntday = DateTime.Now;
            var currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate);
            if (currenttime != null)
            {
                LogHelper.WriteLogs($"显示时间：{currenttime?.FromDate.ToString("yyyy-MM-dd HH:mm:ss")}-{currenttime?.EndDate.ToString("yyyy-MM-dd HH:mm:ss")}");
            }
            
            if (currenttime == null)
            {
                //1.小于最小时间 或大于最大时间
                if (currntday <= ConfigurationCache.TimeSetlist.FirstOrDefault().FromDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault();
                    currntday = currenttime.FromDate;
                }
                else if (currntday >= ConfigurationCache.TimeSetlist.LastOrDefault().EndDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.LastOrDefault();
                    currntday = currenttime.EndDate;
                }
                //2.时间空隙里 那就是最接近的时间
                else
                {
                    currenttime = ConfigurationCache.TimeSetlist.OrderByDescending(s=>s.EndDate).FirstOrDefault(s => currntday >= s.EndDate);
                }
            }
            string UpdateStr = DateTime.Now.ToString("yyyy-MM-dd");
            if (currenttime.IsNext == 2)
            {
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 4)
                {
                    UpdateStr = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                }
                
            }
            string sql = string.Format($"update CurrentData set actualnum=actualnum+1 where currentdate='{UpdateStr}' and timesettingid={currenttime.Tkid}");
            LogHelper.WriteLogs(sql);
            SQLiteHelper.ExecuteNonQuery(sql);


            return true;
        }

        /// <summary>
        /// 获得实际的日期
        /// </summary>
        /// <returns></returns>
        protected string GetCurrentDay()
        {
            DateTime RetTime = DateTime.Now;
            if (RetTime.Hour >= 0 && RetTime.Hour <= 4)
            {
                RetTime = DateTime.Now.AddDays(-1);
            }
            return RetTime.ToString("yyyy-MM-dd");
        }
        protected DateTime GetDate(int CountNum,string CurrentDay, string NextDay,string FromDate,string EndDate, long NextType,bool isFrom)
        {
            DateTime RetDate = DateTime.Now;
            switch (NextType)
            {
                case 0:
                    RetDate = isFrom ? Convert.ToDateTime(CurrentDay + " " + FromDate) : Convert.ToDateTime(CurrentDay + " " + EndDate); 
                    break;
                case 1:
                    RetDate = isFrom ? Convert.ToDateTime(NextDay + " " + FromDate) : Convert.ToDateTime(NextDay + " " + EndDate);
                    break;
                case 2:
                    RetDate = isFrom ? Convert.ToDateTime(CurrentDay + " " + FromDate) : Convert.ToDateTime(NextDay + " " + EndDate); 
                    break;
                default:break;
            }
            return RetDate;
        }
        protected DateTime GetInCalDate(int CountNum, string CurrentDay, string NextDay, string FromDate, string EndDate, long NextType, bool isFrom)
        {
            DateTime RetDate = DateTime.Now;
            switch (NextType)
            {
                case 0:
                    RetDate = isFrom ? Convert.ToDateTime(CurrentDay + " " + FromDate) : Convert.ToDateTime(CurrentDay + " " + EndDate);
                    if (CountNum == 0 && isFrom)
                    {
                        RetDate = Convert.ToDateTime(CurrentDay + " 05:00");
                    }
                    break;
                case 1:
                    RetDate = isFrom ? Convert.ToDateTime(NextDay + " " + FromDate) : Convert.ToDateTime(NextDay + " " + EndDate);
                    break;
                case 2:
                    RetDate = isFrom ? Convert.ToDateTime(CurrentDay + " " + FromDate) : Convert.ToDateTime(NextDay + " " + EndDate);
                    if (CountNum == 11 && !isFrom)
                    {
                        RetDate = Convert.ToDateTime(NextDay + " 04:59");
                    }
                    break;
                default: break;
            }
            return RetDate;
        }
        public void InitailData()
        {
            
            try
            {

                #region 初始化ConfigurationCache 数据  每天凌晨五点更新一次
                //如果当前时间的小时在0-4的 按前一天计算
                DateTime CurrentDay = DateTime.Now;

                if (CurrentDay.Hour >= 0 && CurrentDay.Hour <= 4)
                {
                    //第二天凌晨0-5点间隔 
                    ConfigurationCache.IsInNext = true;
                }
                //时间间隔数据
                DataTable dt = SQLiteHelper.ExecuteDataTable("select * from TimeSetting order by sort asc");
                IList<TimeSetting> datalist = ModelConvertHelper<TimeSetting>.ConvertToModel(dt);

                var timedatalist = datalist.ToList();
                ConfigurationCache.CurrentDay = CurrentDay.ToString("yyyy-MM-dd");
                ConfigurationCache.NextDay = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

                DateTime FirstDay = Convert.ToDateTime(CurrentDay.ToString("yyyy-MM-01 00:00:00"));
                ConfigurationCache.FirstDay = FirstDay;
                ConfigurationCache.LastDay = Convert.ToDateTime(FirstDay.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd 23:59:59"));

                ConfigurationCache.MaxVersion = timedatalist.Max(s => s.Version);
                string Previous = CurrentDay.ToString("yyyy-MM-dd");
                if (CurrentDay.Hour >= 0 && CurrentDay.Hour <= 4)
                {
                    Previous = CurrentDay.AddDays(-1).ToString("yyyy-MM-dd");
                }
                //校验数据
                CheckCurrentData(timedatalist);

                ConfigurationCache.Version = (Int64)SQLiteHelper.ExecuteScalar($"select version from BasicDaySetting where basictime='{Previous}' ");
                
                var timetemplist = new List<TimeSetTemp>();
                int CountNum = 0;
                foreach (var model in timedatalist.Where(s => s.Version == ConfigurationCache.Version))
                {
                    timetemplist.Add(new TimeSetTemp()
                    {
                        Tkid = model.Tkid,
                        IsNext = model.IsNext  ,
                        Sort = model.Sort,
                        //FromDate = model.IsNext == 1 ? Convert.ToDateTime(ConfigurationCache.NextDay + " " + model.FromDate) : Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + model.FromDate),
                        //EndDate = model.IsNext == 1 ? Convert.ToDateTime(ConfigurationCache.NextDay + " " + model.EndDate) : Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + model.EndDate),
                        FromDate=GetDate(CountNum, Previous, Convert.ToDateTime(Previous).AddDays(1).ToString("yyyy-MM-dd"),model.FromDate,model.EndDate,model.IsNext,true),
                        EndDate= GetDate(CountNum, Previous, Convert.ToDateTime(Previous).AddDays(1).ToString("yyyy-MM-dd"), model.FromDate, model.EndDate, model.IsNext, false),
                        Targetnum = model.Targetnum,
                        Xiaolv = model.Xiaolv
                    });
                    CountNum++;
                }
                ConfigurationCache.TimeSetlist = timetemplist;
                #endregion


                //初始化CurrentData
                string sql = $"select count(1) from CurrentData where currentdate='{ConfigurationCache.CurrentDay}'";
                var count = (Int64)SQLiteHelper.ExecuteScalar(sql);
                if (count < 6)
                {
                    foreach (var model in timetemplist)
                    {
                        sql = $"insert into CurrentData(currentdate,timesettingid,targetnum,actualnum,version,xiaolv) values('{model.FromDate.ToString("yyyy-MM-dd")}','{model.Tkid}','{model.Targetnum}','0',{ConfigurationCache.Version},{model.Xiaolv})";
                        SQLiteHelper.ExecuteNonQuery(sql);
                    }
                }
                else
                {
                    long mintkid = 0;
                    if (ConfigurationCache.IsInNext)
                    {
                        mintkid = (long)SQLiteHelper.ExecuteScalar($"select  min(tkid)  from CurrentData where currentdate = '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}' and version=(select version from BasicDaySetting where basictime= '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}')");
                    }
                    else
                    {
                        mintkid = (long)SQLiteHelper.ExecuteScalar($"select  min(tkid)  from CurrentData where currentdate = '{ConfigurationCache.CurrentDay}' and version=(select version from BasicDaySetting where basictime= '{ConfigurationCache.CurrentDay}')");
                    }
                    sql = $"select * from CurrentData where  tkid>={mintkid} and tkid<{mintkid + 12}";
                    DataTable dt3 = SQLiteHelper.ExecuteDataTable(sql);
                    IList<CurrentData> datalist3 = ModelConvertHelper<CurrentData>.ConvertToModel(dt3);
                    foreach (CurrentData dMol in datalist3)
                    {
                        ConfigurationCache.TimeSetlist.FirstOrDefault(s => s.Tkid == dMol.TimeSettingId).Xiaolv = dMol.XiaoLv;
                    }
                }

                //初始化时间段
                InitailCalTimeList(ConfigurationCache.TimeSetlist);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLogs(ex.Message);

            } 
        }

        private void CheckCurrentData(List<TimeSetting> timedatalist)
        {
           
            string sql = $"select count(1) from CurrentData where currentdate='{GetCurrentDay()}'";
            var count = (Int64)SQLiteHelper.ExecuteScalar(sql);
            if (count < 6)
            {
                //插入到CurrentDate
                sql = $"select count(1) from BasicDaySetting where basictime='{GetCurrentDay()}'";
                if ((Int64)SQLiteHelper.ExecuteScalar(sql) == 0)
                {
                    sql = $" insert into BasicDaySetting(basictime,plannumd,version) values('{GetCurrentDay()}',0,{ConfigurationCache.MaxVersion})";
                    SQLiteHelper.ExecuteNonQuery(sql);
                }
              
               
                var timetemplist = new List<TimeSetTemp>();
                int CountNum = 0;
                foreach (var model in timedatalist.Where(s => s.Version == ConfigurationCache.MaxVersion))
                {
                    timetemplist.Add(new TimeSetTemp()
                    {
                        Tkid = model.Tkid,
                        IsNext = model.IsNext ,
                        Sort = model.Sort,
                        FromDate = GetDate(CountNum, ConfigurationCache.CurrentDay, ConfigurationCache.NextDay, model.FromDate, model.EndDate, model.IsNext, true),
                        EndDate = GetDate(CountNum, ConfigurationCache.CurrentDay, ConfigurationCache.NextDay, model.FromDate, model.EndDate, model.IsNext, false),
                        Targetnum = model.Targetnum,
                        Xiaolv = model.Xiaolv
                    });
                    CountNum++;
                }
                ConfigurationCache.TimeSetlist = timetemplist;
            }
        }

        protected void InitailCalTimeList(List<TimeSetTemp> timedatalist)
        {
            var timetemplist = new List<TimeSetTemp>();
            int CountNum = 0;
            foreach (var model in timedatalist)
            {
                timetemplist.Add(new TimeSetTemp()
                {
                    Tkid = model.Tkid,
                    //IsNext = model.IsNext ,
                    Sort = model.Sort,
                    FromDate = GetInCalDate(CountNum, model.FromDate.ToString("yyyy-MM-dd"), model.EndDate.ToString("yyyy-MM-dd"), model.FromDate.ToString("HH:mm"), model.EndDate.ToString("HH:mm"), model.IsNext, true),
                    EndDate = GetInCalDate(CountNum, model.FromDate.ToString("yyyy-MM-dd"), model.EndDate.ToString("yyyy-MM-dd"), model.FromDate.ToString("HH:mm"), model.EndDate.ToString("HH:mm"), model.IsNext, false),
                    Targetnum = model.Targetnum,
                    Xiaolv = model.Xiaolv
                });
                CountNum++;
            }
            ConfigurationCache.TimeSetInCallist = timetemplist;
        }

        public TimeListTemp GetTime(bool IsFirst)
        {
            DateTime currenttime = DateTime.Now;
            TimeListTemp data = new TimeListTemp();

            List<tempA> mytime = new List<tempA>();
            List<tempA> mynumlist = new List<tempA>();
            List<TimeSetTemp> mydatalist = new List<TimeSetTemp>();

            string mysql = $"select * from CurrentData where  currentdate>='{ConfigurationCache.CurrentDay}' order by timesettingid asc";
            DataTable dt3 = SQLiteHelper.ExecuteDataTable(mysql);
            IList<CurrentData> datalist3 = ModelConvertHelper<CurrentData>.ConvertToModel(dt3);

            if (IsFirst)
            {
                mydatalist = ConfigurationCache.TimeSetlist.OrderBy(s => s.Sort).Take(6).ToList();
                // datalist3= datalist3.Take(6).ToList();
            }
            if (!IsFirst)
            {
                mydatalist = ConfigurationCache.TimeSetlist.OrderBy(s => s.Sort).Skip(6).Take(6).ToList();
                //datalist3 = datalist3.Skip(6).Take(6).ToList();

            }
            int CountNum = 0;
            string timeRange = "";
            foreach (var model in mydatalist)
            {
                string classname = "";
                if (currenttime >= model.FromDate && currenttime <= model.EndDate)
                {
                    classname = "bg_Class";
                }
                timeRange = $"{model.FromDate.ToString("HH:mm")}~{model.EndDate.ToString("HH:mm")}";
                //if (IsFirst && CountNum == 0)
                //{
                //    timeRange = $"05:00~{model.EndDate.ToString("HH:mm")}";
                //}
                //if (!IsFirst && CountNum == 5)
                //{
                //    timeRange = $"{model.FromDate.ToString("HH:mm")}~04:59";
                //}
                mytime.Add(new tempA()
                {
                    classname = classname,
                    //value = $"{model.FromDate.ToString("HH:mm")}~{model.EndDate.ToString("HH:mm")}"
                    value= timeRange
                });
                mynumlist.Add(new tempA()
                {
                    classname = classname,
                    //mynum = model.Targetnum
                    mynum = datalist3.FirstOrDefault(s => s.TimeSettingId == model.Tkid)?.TargetNum ?? 0
                });
                CountNum++;
            }

            data.timelist = mytime;
            data.timenum = mynumlist;
            return data;
        }

        public FiveMinute GetFive()
        {
            FiveMinute model = new FiveMinute();
            string sql = String.Empty;
            //根据当前时间 找到对应时间的序列
            DateTime currntday = DateTime.Now;
            var currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate);
            if (currenttime == null)
            {
                //1.小于最小时间 或大于最大时间
                if (currntday <= ConfigurationCache.TimeSetlist.FirstOrDefault().FromDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault();
                    currntday = currenttime.FromDate;
                }
                else if (currntday >= ConfigurationCache.TimeSetlist.LastOrDefault().EndDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.LastOrDefault();
                    currntday = currenttime.EndDate;
                }
                //2.时间空隙里 那就是最接近的时间
                else
                {
                    currenttime = ConfigurationCache.TimeSetlist.OrderByDescending(s => s.EndDate).FirstOrDefault(s => currntday >= s.EndDate);
                }
            }

            if (currenttime != null)
            {


                //var timesetid = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate)?.Tkid ?? 0;
                var timesetid = currenttime.Tkid;
                if (timesetid == 0)
                {
                    timesetid = ConfigurationCache.TimeSetInCallist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate)?.Tkid ?? 0;
                }
                sql = $"select  tkid from CurrentData where currentdate='{GetCurrentDay()}' and version={ConfigurationCache.Version} order by timesettingid asc  limit 0,1 ";
                Int64 tkid = (Int64)SQLiteHelper.ExecuteScalar(sql);

                if (timesetid != 0)
                {
                    //日当前计划
                    //sql = $"select ifnull(sum(targetnum),0) from CurrentData where timesettingid<{timesetid} and currentdate<='{DateTime.Now.ToString("yyyy-MM-dd")}' and version={ConfigurationCache.Version} ";
                    long tempCount = timesetid - ConfigurationCache.TimeSetlist.FirstOrDefault().Tkid;
                    sql = $"select ifnull(sum(targetnum),0) from CurrentData where tkid>={tkid} and tkid<{tkid+ tempCount}  ";
                    Int64 All_CurrentPlanNumd = (Int64)SQLiteHelper.ExecuteScalar(sql);
                    int Xioalv = (int)(Convert.ToDecimal((currntday - currenttime.FromDate).TotalMinutes) * Convert.ToDecimal((currenttime?.Xiaolv)));
                    model.CurrentPlanNumd = All_CurrentPlanNumd + Xioalv; 
                }
                //月当前计划
                sql = $"select currentplannumm from BasicMonthSetting where basictime='{ConfigurationCache.FirstDay.ToString("yyyy-MM-dd")}' ";
                Int64 currentplannumm = (Int64)SQLiteHelper.ExecuteScalar(sql);
                model.CurrentPlanNumM = currentplannumm + model.CurrentPlanNumd;
            }
            string mysql = $"select Message from Settings";
            model.Message = SQLiteHelper.ExecuteScalar(mysql).ToString();
            //mysql = $"select plannumd from BasicDaySetting where basictime='{ConfigurationCache.CurrentDay}'";
            //model.PlanNumd = (Int64)SQLiteHelper.ExecuteScalar(mysql);
            
            #region 日计划数量
            long mintkid = 0;
            if (ConfigurationCache.IsInNext)
            {
                mintkid = (long)SQLiteHelper.ExecuteScalar($"select  min(tkid)  from CurrentData where currentdate = '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}' and version=(select version from BasicDaySetting where basictime= '{DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")}')");
            }
            else
            {
                mintkid = (long)SQLiteHelper.ExecuteScalar($"select  min(tkid)  from CurrentData where currentdate = '{ConfigurationCache.CurrentDay}' and version=(select version from BasicDaySetting where basictime= '{ConfigurationCache.CurrentDay}')");
            }
            mysql = $"select sum(targetnum) from CurrentData where  tkid>={mintkid} and tkid<{mintkid + 12}";
            model.PlanNumd = (Int64)SQLiteHelper.ExecuteScalar(mysql);
            #endregion

            mysql = $"select plannumm from BasicMonthSetting where basictime='{ConfigurationCache.FirstDay.ToString("yyyy-MM-dd")}' ";
            model.PlanNumM = (Int64)SQLiteHelper.ExecuteScalar(mysql);

            model.CurrentDay = DateTime.Now.ToString("yyyy年MM月dd日");
            return model;
        }

        public OneMinute GetOne()
        {
            OneMinute mymodel = new OneMinute();
            FiveMinute model = new FiveMinute();
            string sql = string.Empty;
            //根据当前时间 找到对应时间的序列
            DateTime currntday = DateTime.Now;
            var currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate);
          
            if (currenttime == null)
            {
                //1.小于最小时间 或大于最大时间
                if (currntday <= ConfigurationCache.TimeSetlist.FirstOrDefault().FromDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.FirstOrDefault();
                    currntday = currenttime.FromDate;
                }
                else if (currntday >= ConfigurationCache.TimeSetlist.LastOrDefault().EndDate)
                {
                    currenttime = ConfigurationCache.TimeSetlist.LastOrDefault();
                    currntday = currenttime.EndDate;
                }
                //2.时间空隙里 那就是最接近的时间
                else
                {
                    currenttime = ConfigurationCache.TimeSetlist.OrderByDescending(s => s.EndDate).FirstOrDefault(s => currntday >= s.EndDate);
                } 
            }

            if (currenttime != null)
            {
                //var timesetid = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate)?.Tkid ?? 0;
                var timesetid = currenttime.Tkid;

                if (timesetid == 0)
                {
                    timesetid = ConfigurationCache.TimeSetInCallist.FirstOrDefault(s => currntday > s.FromDate && currntday <= s.EndDate)?.Tkid ?? 0;
                }

                sql = $"select  tkid from CurrentData where currentdate='{GetCurrentDay()}' and version={ConfigurationCache.Version} order by timesettingid asc  limit 0,1 ";
                Int64 tkid = (Int64)SQLiteHelper.ExecuteScalar(sql);

                if (timesetid != 0)
                {
                    //日当前计划 
                    long tempCount = timesetid - ConfigurationCache.TimeSetlist.FirstOrDefault().Tkid;
                    sql = $"select ifnull(sum(targetnum),0) from CurrentData where tkid>={tkid} and tkid<{tkid + tempCount}  "; 
                    Int64 All_CurrentPlanNumd = (Int64)SQLiteHelper.ExecuteScalar(sql);
                    int Xioalv = (int)(Convert.ToDecimal((currntday - currenttime.FromDate).TotalMinutes) * Convert.ToDecimal((currenttime?.Xiaolv)));
                    model.CurrentPlanNumd = All_CurrentPlanNumd + Xioalv;
                }


                //月当前计划
                sql = $"select currentplannumm from  BasicMonthSetting where basictime='{ConfigurationCache.FirstDay.ToString("yyyy-MM-dd")}' ";
                Int64 All_CurrentPlanNumM = (Int64)SQLiteHelper.ExecuteScalar(sql);
                model.CurrentPlanNumM = All_CurrentPlanNumM + model.CurrentPlanNumd;



               
             

                //日实际数量
                sql = $"select ifnull(sum(actualnum),0) from CurrentData where tkid>={tkid} and tkid<{tkid + 12}  ";
                mymodel.ActualNumd = (Int64)SQLiteHelper.ExecuteScalar(sql);

                //日差异数量
                //mymodel.ActualDisNumd.mynum = Math.Abs(model.CurrentPlanNumd - mymodel.ActualNumd);
                mymodel.ActualDisNumd.mynum =    mymodel.ActualNumd- model.CurrentPlanNumd;
                mymodel.ActualDisNumd.classname = (mymodel.ActualNumd - model.CurrentPlanNumd) >= 0 ? "nbg_Class1" : "dis_Class";

                //月实际数量
                sql = $"select ifnull(sum(actualnum),0) from CurrentData where  datetime(currentdate)>='{ConfigurationCache.FirstDay.ToString("yyyy-MM-dd HH:mm")}' and datetime(currentdate)<= '{ConfigurationCache.LastDay.ToString("yyyy-MM-dd 23:59")}' ";
               // LogHelper.WriteLogs($"月实际数量：{sql}");
                mymodel.ActualNumM = (Int64)SQLiteHelper.ExecuteScalar(sql);

                //月差异数量 
                //mymodel.ActualDisNumM.mynum = Math.Abs(model.CurrentPlanNumM - mymodel.ActualNumM);
                mymodel.ActualDisNumM.mynum = mymodel.ActualNumM - model.CurrentPlanNumM;
                mymodel.ActualDisNumM.classname = (mymodel.ActualNumM - model.CurrentPlanNumM) >= 0 ? "nbg_Class1" : "dis_Class";

              

                DataTable dt2 = SQLiteHelper.ExecuteDataTable($"SELECT * FROM CurrentData where tkid>={tkid} and tkid<{tkid + 12}");
                IList<CurrentData> datalist = ModelConvertHelper<CurrentData>.ConvertToModel(dt2);



                currntday = DateTime.Now;
                var timeid = ConfigurationCache.TimeSetlist.FirstOrDefault(s => currntday > s.FromDate && currntday < s.EndDate)?.Tkid ?? 0;

                var faclist = datalist.ToList().OrderBy(s => s.TimeSettingId).Take(6).ToList();
                foreach (var modelA in faclist)
                {
                    string classname = "nbg_Class";
                    //if (modelA.TimeSettingId == timeid) classname = "bg_Class";
                    mymodel.FActualList.Add(new tempA()
                    {
                        mynum = modelA.ActualNum,
                        classname = classname
                    });
                }
                var saclist = datalist.ToList().OrderBy(s => s.TimeSettingId).Skip(6).Take(6).ToList();
                foreach (var modelA in saclist)
                {
                    string classname = "nbg_Class";
                    //if (modelA.TimeSettingId == timeid) classname = "bg_Class";
                    mymodel.SActualList.Add(new tempA()
                    {
                        mynum = modelA.ActualNum,
                        classname = classname
                    });
                }

            }


            return mymodel;
        }



        public string AddDaySet(string datalist)
        {
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");
            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<BasicDaySettingRequest>>();
            PageResponse resp = new PageResponse();
            string sql = string.Empty;
            SQLiteHelper.ExecuteNonQuery($"delete from BasicDaySetting where datetime(basictime)>'{ConfigurationCache.CurrentDay + " 00:00:00"}' ");
            try
            {

                foreach (var dataMol in datalist2)
                {

                    if (Convert.ToDateTime(dataMol.Basictime) > Convert.ToDateTime(ConfigurationCache.CurrentDay))
                    {
                        sql = $"INSERT INTO  BasicDaySetting (basictime, plannumd,version) values (" +
                                              $"'{Convert.ToDateTime(dataMol.Basictime).ToString("yyyy-MM-dd")}'," +
                                                $"'{dataMol.PlannumD}'," +
                                              $"'{ConfigurationCache.MaxVersion}' ) ";
                        SQLiteHelper.ExecuteNonQuery(sql);
                    }

                }

            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
                LogHelper.WriteLogs(e.Message);
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        public string AddMonthSet(string datalist)
        {
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");
            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<BasicMonthSettingRequest>>();
            PageResponse resp = new PageResponse();
            string sql = string.Empty;
            SQLiteHelper.ExecuteNonQuery("delete from BasicMonthSetting");
            try
            {

                foreach (var dataMol in datalist2)
                {
                    sql = $"INSERT INTO  BasicMonthSetting (basictime, plannumm,currentplannumm) values (" +
                                            $"'{Convert.ToDateTime(dataMol.Basictime).ToString("yyyy-MM-dd")}'," +
                                            $"'{dataMol.PlannumM}'," +
                                            $"'{dataMol.CurrentplannumM}' ) ";

                    SQLiteHelper.ExecuteNonQuery(sql);
                }

            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
                LogHelper.WriteLogs(e.Message);
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        public string AddTimeSet(string datalist)
        {
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");
            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<TimeSetRequest>>();
            PageResponse resp = new PageResponse();
            string sql = string.Empty;
            int NextType = 0; //是否当天
            DateTime mindate = DateTime.Now;  //最小日期
            int sort = 0;
            try
            {
                if (datalist2.Any())
                {
                    ConfigurationCache.MaxVersion = ConfigurationCache.MaxVersion + 1;
                    SQLiteHelper.ExecuteNonQuery($"update BasicDaySetting set version={ConfigurationCache.MaxVersion} where datetime(basictime)>'{ConfigurationCache.CurrentDay} 00:00:00' ");
                    mindate = Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + datalist2.FirstOrDefault().FromDate);
                }

                foreach (var dataMol in datalist2)
                {
                    //时间有三种类型  都在当天 跨天 都在第二天
                    DateTime fromD = Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + dataMol.FromDate);
                    DateTime fromE = Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + dataMol.EndDate);

                    //都在第二天
                    if (fromD < mindate)
                    {
                        NextType = 1;
                        fromD = Convert.ToDateTime(ConfigurationCache.NextDay + " " + dataMol.FromDate);
                        fromE = Convert.ToDateTime(ConfigurationCache.NextDay + " " + dataMol.EndDate);
                    }

                    if (fromE < mindate)
                    {
                        NextType = 2;
                        fromE = Convert.ToDateTime(ConfigurationCache.NextDay + " " + dataMol.EndDate);
                    }
                    Int64 tagetNum = Convert.ToInt64(dataMol.Targetnum);
                    long timeset = (long)(fromE - fromD).TotalMinutes;
                    decimal Xiaolv = Math.Round(Convert.ToDecimal(tagetNum) / Convert.ToDecimal(timeset), 2);
                    sql = $"INSERT INTO  TimeSetting (fromdate,enddate,targetnum,xiaolv,sort,isnext,version) values (" +
                                            $"'{dataMol.FromDate}'," +
                                            $"'{dataMol.EndDate}'," +
                                            $"'{dataMol.Targetnum}'," +
                                            $"'{Xiaolv.ToString("0.00")}'," +
                                            $"'{sort}'," +
                                            $"'{NextType}'," +
                                            $"'{ConfigurationCache.MaxVersion}' ) ";

                    SQLiteHelper.ExecuteNonQuery(sql);
                    sort++;
                }

            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
                LogHelper.WriteLogs(e.Message);
            }
            return JsonHelper.Instance.Serialize(resp);
        }



        public string GetDaySet(PageRequest request)
        {
            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from BasicDaySetting order by basictime desc");
            IList<BasicDaySettingResponse> datalist = ModelConvertHelper<BasicDaySettingResponse>.ConvertToModel(dt);
            data.count = datalist.Count();
            if (request.limit > 0 && request.page > 0)
            {
                datalist = datalist.Skip((request.page - 1) * request.limit).Take(500).ToList();
            }
            data.data = datalist;
            int pageTotal = data.count / request.limit;
            pageTotal = (data.count % request.limit > 0 ? pageTotal + 1 : pageTotal);
            data.msg = (request.page >= pageTotal ? 1 : request.page + 1).ToString();
            return JsonHelper.Instance.Serialize(data);
        }

        public string GetMonthSet(PageRequest request)
        {
            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from BasicMonthSetting order by tkid desc");
            IList<BasicMonthSettingResponse> datalist = ModelConvertHelper<BasicMonthSettingResponse>.ConvertToModel(dt);
            data.count = datalist.Count();
            if (request.limit > 0 && request.page > 0)
            {
                datalist = datalist.Skip((request.page - 1) * request.limit).Take(100).ToList();
            }
            data.data = datalist;
            int pageTotal = data.count / request.limit;
            pageTotal = (data.count % request.limit > 0 ? pageTotal + 1 : pageTotal);
            data.msg = (request.page >= pageTotal ? 1 : request.page + 1).ToString();
            return JsonHelper.Instance.Serialize(data);
        }

        public string GetTimeSet(PageRequest request)
        {
            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable($"select * from TimeSetting where version='{ConfigurationCache.MaxVersion}' order by tkid asc");
            IList<TimeSettingResponse> datalist = ModelConvertHelper<TimeSettingResponse>.ConvertToModel(dt);
            data.count = datalist.Count();
            if (request.limit > 0 && request.page > 0)
            {
                datalist = datalist.Skip((request.page - 1) * request.limit).Take(20).ToList();
            }
            data.data = datalist;
            int pageTotal = data.count / request.limit;
            pageTotal = (data.count % request.limit > 0 ? pageTotal + 1 : pageTotal);
            data.msg = (request.page >= pageTotal ? 1 : request.page + 1).ToString();
            return JsonHelper.Instance.Serialize(data);
        }

        public string AddTitle(string Message)
        {
            PageResponse resp = new PageResponse();
            string sql = $"update Settings set Message='{Message}' ";

            SQLiteHelper.ExecuteNonQuery(sql);
            return JsonHelper.Instance.Serialize(resp);
        }

        public List<ExportMessage> GetExportMessage(string FromDate, string EndDate)
        {
            EndDate = Convert.ToDateTime(EndDate).AddDays(1).ToString("yyyy-MM-dd");
            DataTable dt2 = SQLiteHelper.ExecuteDataTable($"SELECT * FROM CurrentData where  datetime(currentdate)>='{FromDate}' and datetime(currentdate)<='{EndDate}' ");
            IList<CurrentData> datalist = ModelConvertHelper<CurrentData>.ConvertToModel(dt2);
            var currentlist = datalist.ToList();

            dt2 = SQLiteHelper.ExecuteDataTable($"SELECT * FROM TimeSetting ");
            IList<TimeSetting> datalist2 = ModelConvertHelper<TimeSetting>.ConvertToModel(dt2);

            var timesetlist = datalist2.ToList();

            var timelist = currentlist.OrderBy(s => s.Currentdate).Select(s => s.Currentdate).Distinct().ToList();

            List<ExportMessage> RetList = new List<ExportMessage>();
            long fromtkid = 0;
            long endtkid = 0;
            var timeStr = "";
            foreach (var model in timelist)
            {
                timeStr = model.ToString("yyyy-MM-dd");
                endtkid = (currentlist.OrderBy(s => s.Tkid).FirstOrDefault(s => s.Currentdate == model)?.Tkid ?? 0) + 11;
                var mydatalist = currentlist.Where(s => s.Tkid > fromtkid && s.Tkid <= endtkid).OrderBy(s => s.TimeSettingId).ToList();
                var mytimeids = mydatalist.Select(s => s.TimeSettingId).ToList();
                var version = mydatalist.FirstOrDefault().Version;

                //时间间隔信息
                var mytimesetlist = timesetlist.OrderBy(s => s.Tkid).Where(s => s.Version == version && mytimeids.Contains(s.Tkid)).ToList();
                string[] timesetnew = new string[13];
                string[] arrmb = new string[12];
                string[] arrsj = new string[12];
                int innerCount = 0;
                foreach (var timemodel in mytimesetlist.Take(12))
                {
                    timesetnew[innerCount] = timemodel.FromDate + "-" + timemodel.EndDate;
                    innerCount++;
                }
                int innerCount2 = 0;
                foreach (var datamodel in mydatalist.Take(12))
                {
                    arrmb[innerCount2] = datamodel.TargetNum.ToString();
                    arrsj[innerCount2] = datamodel.ActualNum.ToString();
                    innerCount2++;
                }
                ////第一行 时间段
                ExportMessage data = new ExportMessage();
                data.A1 = model.ToString("yyyy/MM/dd");
                data.A2 = timesetnew[0];
                data.A3 = timesetnew[1];
                data.A4 = timesetnew[2];
                data.A5 = timesetnew[3];
                data.A6 = timesetnew[4];
                data.A7 = timesetnew[5];

                data.A8 = timesetnew[6];
                data.A9 = timesetnew[7];
                data.A10 = timesetnew[8];
                data.A11 = timesetnew[9];
                data.A12 = timesetnew[10];
                data.A13 = timesetnew[11];
                RetList.Add(data);
                //第二行 目标数量
                ExportMessage data2 = new ExportMessage();
                data2.A1 = "目标";
                data2.A2 = arrmb[0];
                data2.A3 = arrmb[1];
                data2.A4 = arrmb[2];
                data2.A5 = arrmb[3];
                data2.A6 = arrmb[4];
                data2.A7 = arrmb[5];

                data2.A8 = arrmb[6];
                data2.A9 = arrmb[7];
                data2.A10 = arrmb[8];
                data2.A11 = arrmb[9];
                data2.A12 = arrmb[10];
                data2.A13 = arrmb[11];
                RetList.Add(data2);
                //第三行 实际数量
                ExportMessage data3 = new ExportMessage();
                data3.A1 = "实际";
                data3.A2 = arrsj[0];
                data3.A3 = arrsj[1];
                data3.A4 = arrsj[2];
                data3.A5 = arrsj[3];
                data3.A6 = arrsj[4];
                data3.A7 = arrsj[5];

                data3.A8 = arrsj[6];
                data3.A9 = arrsj[7];
                data3.A10 = arrsj[8];
                data3.A11 = arrsj[9];
                data3.A12 = arrsj[10];
                data3.A13 = arrsj[11];
                RetList.Add(data3);

                fromtkid = endtkid;
            }
            return RetList;
        }


        public string GetCurrentDayNum(PageRequest request)
        {
            //找到当前最小的tkid
            long mintkid = (long)SQLiteHelper.ExecuteScalar($"select  min(tkid)  from CurrentData where currentdate = '{ConfigurationCache.CurrentDay}' and version=(select version from BasicDaySetting where basictime= '{ConfigurationCache.CurrentDay}')");


            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable($"select A.tkid, b.fromdate,b.enddate,A.targetnum from CurrentData A inner join  TimeSetting B on A.timesettingid = B.tkid where A.tkid >= {mintkid} and a.tkid < {mintkid + 12} order by A.tkid asc");
            IList<CurrentDayNumResponse> datalist = ModelConvertHelper<CurrentDayNumResponse>.ConvertToModel(dt);
            data.count = datalist.Count();
            if (request.limit > 0 && request.page > 0)
            {
                datalist = datalist.Skip((request.page - 1) * request.limit).Take(20).ToList();
            }
            data.data = datalist;
            int pageTotal = data.count / request.limit;
            pageTotal = (data.count % request.limit > 0 ? pageTotal + 1 : pageTotal);
            data.msg = (request.page >= pageTotal ? 1 : request.page + 1).ToString();


            return JsonHelper.Instance.Serialize(data);
        }

        public string UpdateCurrentDayNum(string datalist)
        {
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");
            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<CurrentDayNumRequest>>();
            PageResponse resp = new PageResponse();
            string sql = string.Empty;
            DateTime FromDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            long timeset = 0;
            decimal Xiaolv = 0;
            try
            {
                string mysql = $"select * from CurrentData where  currentdate>='{ConfigurationCache.CurrentDay}'";
                DataTable dt3 = SQLiteHelper.ExecuteDataTable(mysql);
                IList<CurrentData> datalist3 = ModelConvertHelper<CurrentData>.ConvertToModel(dt3);

                foreach (var dataMol in datalist2)
                {
                    FromDate = Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + dataMol.FromDate);
                    if (FromDate.Hour >= 0 && FromDate.Hour <= 5) FromDate = FromDate.AddDays(1);
                    EndDate = Convert.ToDateTime(ConfigurationCache.CurrentDay + " " + dataMol.EndDate);
                    if (EndDate.Hour >= 0 && EndDate.Hour <= 5) EndDate = EndDate.AddDays(1);
                    timeset = (long)(EndDate - FromDate).TotalMinutes;
                    Xiaolv = Convert.ToDecimal(dataMol.Targetnum) / Convert.ToDecimal(timeset);
                    sql = $"update CurrentData set xiaolv={Xiaolv},targetnum={dataMol.Targetnum} where tkid={dataMol.Tkid}";
                    SQLiteHelper.ExecuteNonQuery(sql);
                    long timetkid = datalist3.FirstOrDefault(s => s.Tkid == Convert.ToInt64(dataMol.Tkid))?.TimeSettingId ?? 0;
                    ConfigurationCache.TimeSetlist.FirstOrDefault(s => s.Tkid == timetkid).Xiaolv = Xiaolv;
                }

            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
                LogHelper.WriteLogs(e.Message);
            }
            return JsonHelper.Instance.Serialize(resp);
        }
    }
}
