using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BoardTab.Common;
using BoardTab.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardTab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            #region 初始化数据
            //string sql = "";
            //for (int i = 10; i < 210; i++)
            //{
            //    sql = $"INSERT INTO Product VALUES (" +
            //                            $"{i}," +
            //                           $"{i}," +
            //                            $"{i}," +
            //                             $"{i}," +
            //                           $"{i}," +
            //                           $"{i}," +
            //                       $"{i}," +
            //                             $"{i}," +
            //                              $"{i}," +
            //                             $"{i}," +
            //                            $"{i}," +
            //                            $"{i}," +
            //                              $"{i}," +
            //                             $"{i}," +
            //                            $"{i}) ";
            //    SQLiteHelper.ExecuteNonQuery(sql);
            //}

            #endregion

            DataTable dt2 = SQLiteHelper.ExecuteDataTable("select * from Settings");
            var setMol = ModelConvertHelper<Settings>.ConvertToModel(dt2).FirstOrDefault();

            return View();
        }


        public IActionResult EquipIndex()
        {

            #region 初始化数据


            #endregion

            DataTable dt2 = SQLiteHelper.ExecuteDataTable("select * from Settings");
            var setMol = ModelConvertHelper<Settings>.ConvertToModel(dt2).FirstOrDefault();
            ViewBag.Message = setMol?.Message;
            ViewBag.TimeOut=setMol?.TimeOut;
            return View();
        }

        /// <summary>
        /// 查看
        /// </summary>
        /// <returns></returns>
        public IActionResult ViewIndex()
        {
            DataTable dt2 = SQLiteHelper.ExecuteDataTable("select * from Settings");
            var setMol = ModelConvertHelper<Settings>.ConvertToModel(dt2).FirstOrDefault();
            ViewBag.Message = setMol?.Message;
            return View();
        }
        public IActionResult EquipViewIndex()
        {
            DataTable dt2 = SQLiteHelper.ExecuteDataTable("select * from Settings");
            var setMol = ModelConvertHelper<Settings>.ConvertToModel(dt2).FirstOrDefault();
            ViewBag.TimeOut =Convert.ToInt32((setMol?.TimeOut ?? "10000")) *1000;
            ViewBag.Message = setMol?.Message;

            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from PlanInfo");
            IList<PlanInfoTemp> datalist = ModelConvertHelper<PlanInfoTemp>.ConvertToModel(dt);
            IList<PlanInfoPlus> dataplus = new List<PlanInfoPlus>();

            DateTime NowDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            foreach (var model in datalist)
            {
                dataplus.Add(new PlanInfoPlus()
                {
                    CreateTimeStr = Convert.ToDateTime(model.CreateTime).ToString("yyyy年MM月dd日"),
                    CreateTime = Convert.ToDateTime(model.CreateTime),
                    ModuleA = model.ModuleA,
                    ModuleACustomer = model.ModuleACustomer,
                    ModuleAInStockCount = model.ModuleAInStockCount,
                    ModuleAPlanCount = model.ModuleAPlanCount,
                    ModuleB = model.ModuleB,
                    ModuleBCustomer = model.ModuleBCustomer,
                    ModuleBInStockCount = model.ModuleBInStockCount,
                    ModuleBPlanCount = model.ModuleBPlanCount
                });
            }

            dataplus = dataplus.Where(s => s.CreateTime >= NowDate).ToList();
            if (datalist.Count() < 5)
            {
                int LenthCount = 5 - datalist.Count();
                string EmptyStr = "-";
                for (int i = 0; i < LenthCount; i++)
                {
                    dataplus.Add(new PlanInfoPlus()
                    {
                        CreateTimeStr = EmptyStr,
                        CreateTime =DateTime.Now,
                        ModuleA = EmptyStr,
                        ModuleACustomer = EmptyStr,
                        ModuleAInStockCount = 0,
                        ModuleAPlanCount =0,
                        ModuleB = EmptyStr,
                        ModuleBCustomer = EmptyStr,
                        ModuleBInStockCount = 0,
                        ModuleBPlanCount = 0
                    });
                }
            }
            dataplus = dataplus.Where(s => s.CreateTime >= NowDate).OrderBy(s => s.CreateTime).Take(5).ToList();
            ViewBag.TimeList = dataplus.Select(s => s.CreateTimeStr).ToList();
            ViewBag.ModuleA = dataplus.Select(s => s.ModuleA).ToList();
            ViewBag.ModuleACustomer = dataplus.Select(s => s.ModuleACustomer).ToList();


            ViewBag.ModuleB = dataplus.Select(s => s.ModuleB).ToList();
            ViewBag.ModuleBCustomer = dataplus.Select(s => s.ModuleBCustomer).ToList();
          
            string ACountStr = string.Empty;
            string BCountStr = string.Empty;
            foreach (var model in dataplus)
            {
                ACountStr += (model.ModuleAPlanCount.ToString() + "," + model.ModuleAInStockCount.ToString() + ";");
                BCountStr += (model.ModuleBPlanCount.ToString() + "," + model.ModuleBInStockCount.ToString() + ";");
            }
            ViewBag.ModuleACount = ACountStr.TrimEnd(';').Split(';');
            ViewBag.ModuleBCount = BCountStr.TrimEnd(';').Split(';');

            ViewBag.NowDate = DateTime.Now.ToString("yyyy年MM月dd日");
         

            return View();
        }

        public string AllChild(PageRequest request)
        {

            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from PlanInfo");

            IList<PlanInfoTemp> datalist = ModelConvertHelper<PlanInfoTemp>.ConvertToModel(dt);
            data.data = datalist;
            data.count = datalist.Count();
            return JsonHelper.Instance.Serialize(data);
        }


        

        public string All(PageRequest request)
        {

            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from Equipment");

            IList<Equipment> datalist = ModelConvertHelper<Equipment>.ConvertToModel(dt);
            data.data = datalist;
            data.count = datalist.Count();
            return JsonHelper.Instance.Serialize(data);
        }
        public string Initial(PageRequest request)
        {

            TableData data = new TableData();
            DataTable dt2 = SQLiteHelper.ExecuteDataTable("select * from Settings");
            var setMol = ModelConvertHelper<Settings>.ConvertToModel(dt2).FirstOrDefault();
            ViewBag.TimeOut = Convert.ToInt32((setMol?.TimeOut ?? "10000")) * 1000;
            ViewBag.Message = setMol?.Message;

            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from PlanInfo");
            IList<PlanInfoTemp> datalist = ModelConvertHelper<PlanInfoTemp>.ConvertToModel(dt);
            IList<PlanInfoPlus> dataplus = new List<PlanInfoPlus>();

            DateTime NowDate = Convert.ToDateTime(DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            foreach (var model in datalist)
            {
                dataplus.Add(new PlanInfoPlus()
                {
                    CreateTimeStr = Convert.ToDateTime(model.CreateTime).ToString("yyyy年MM月dd日"),
                    CreateTime = Convert.ToDateTime(model.CreateTime),
                    ModuleA = model.ModuleA,
                    ModuleACustomer = model.ModuleACustomer,
                    ModuleAInStockCount = model.ModuleAInStockCount,
                    ModuleAPlanCount = model.ModuleAPlanCount,
                    ModuleB = model.ModuleB,
                    ModuleBCustomer = model.ModuleBCustomer,
                    ModuleBInStockCount = model.ModuleBInStockCount,
                    ModuleBPlanCount = model.ModuleBPlanCount
                });
            }
            if (datalist.Count() < 5)
            {
                int LenthCount = 5 - datalist.Count();
                string EmptyStr = "-";
                for (int i = 0; i <= LenthCount; i++)
                {
                    dataplus.Add(new PlanInfoPlus()
                    {
                        CreateTimeStr = EmptyStr,
                        CreateTime = DateTime.Now,
                        ModuleA = EmptyStr,
                        ModuleACustomer = EmptyStr,
                        ModuleAInStockCount = 0,
                        ModuleAPlanCount = 0,
                        ModuleB = EmptyStr,
                        ModuleBCustomer = EmptyStr,
                        ModuleBInStockCount = 0,
                        ModuleBPlanCount = 0
                    });
                }
            }
            PlanInfoInView datatem = new PlanInfoInView();
            dataplus = dataplus.Where(s => s.CreateTime >= NowDate).OrderBy(s => s.CreateTime).Take(5).ToList();
            datatem.TimeList = dataplus.Select(s => s.CreateTimeStr).ToList();
            datatem.ModuleA = dataplus.Select(s => s.ModuleA).ToList();
            datatem.ModuleACustomer = dataplus.Select(s => s.ModuleACustomer).ToList();


            datatem.ModuleB = dataplus.Select(s => s.ModuleB).ToList();
            datatem.ModuleBCustomer = dataplus.Select(s => s.ModuleBCustomer).ToList();

            string ACountStr = string.Empty;
            string BCountStr = string.Empty;
            foreach (var model in dataplus)
            {
                ACountStr += (model.ModuleAPlanCount.ToString() + "," + model.ModuleAInStockCount.ToString() + ";");
                BCountStr += (model.ModuleBPlanCount.ToString() + "," + model.ModuleBInStockCount.ToString() + ";");
            }
            datatem.ModuleACount = ACountStr.TrimEnd(';').Split(';').ToList();
            datatem.ModuleBCount = BCountStr.TrimEnd(';').Split(';').ToList(); 
            datatem.Message = setMol?.Message;
            data.data = datatem;
            data.count = datalist.Count();
            return JsonHelper.Instance.Serialize(data);
        }

        public string All2(PageRequest request)
        {
            TableData data = new TableData();
            DataTable dt = SQLiteHelper.ExecuteDataTable("select * from Equipment");
            IList<EquipmentTemp> datalist = ModelConvertHelper<EquipmentTemp>.ConvertToModel(dt);
            data.count = datalist.Count();
            if (request.limit > 0 && request.page > 0)
            {
                datalist = datalist.Skip((request.page - 1) * request.limit).Take(request.limit).ToList();
            }
            foreach (var model in datalist)
            {
                model.Percent = Math.Round(Convert.ToDecimal((model.InStockCount * 100)) / Convert.ToDecimal(model.PlanCount), 2).ToString("0.00") + "%";
                model.UnFinished = model.PlanCount - model.InStockCount;
            }
            data.data = datalist;
            int pageTotal = data.count / request.limit;
            pageTotal = (data.count % request.limit > 0 ? pageTotal + 1 : pageTotal);
            data.msg = (request.page >= pageTotal ? 1 : request.page + 1).ToString();
            return JsonHelper.Instance.Serialize(data);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        public string Add2(string datalist)
        {

            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<StudentTemp>>();
            PageResponse resp = new PageResponse();
            try
            {

                string sql = "INSERT INTO Student VALUES (1, 1);INSERT INTO Student VALUES (2, 2)";
                SQLiteHelper.ExecuteNonQuery(sql);


            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        //保存
        [HttpPost]
        public string Add(string datalist)
        {
            //LogHelper.WriteLogs("提交数据");
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");

            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<EquipmentTemp>>();
            PageResponse resp = new PageResponse();
            //StringBuilder sb = new StringBuilder();
            string sql = string.Empty;
            SQLiteHelper.ExecuteNonQuery("delete from Equipment");
            try
            {

                foreach (var dataMol in datalist2)
                {
                    sql = $"INSERT INTO Equipment VALUES (" +
                                            $"'{dataMol.Number}'," +
                                            $"'{dataMol.PlanCount}'," +
                                            $"'{dataMol.InStockCount}'," +
                                            $"'{dataMol.Remarks}') ";
                    // LogHelper.WriteLogs(sql);
                    int Count = SQLiteHelper.ExecuteNonQuery(sql);
                    //LogHelper.WriteLogs(Count.ToString());
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

        [HttpPost]
        public string AddChild(string datalist)
        {
            //LogHelper.WriteLogs("提交数据");
            datalist = datalist.Replace(",[]", "");
            datalist = datalist.Replace("[],", "");
            datalist = datalist.Replace("[]", "");

            var datalist2 = Newtonsoft.Json.Linq.JArray.Parse(datalist).ToObject<List<PlanInfoTemp>>();
            PageResponse resp = new PageResponse();
            //StringBuilder sb = new StringBuilder();
            string sql = string.Empty;
            SQLiteHelper.ExecuteNonQuery("delete from PlanInfo");
            try
            {

                foreach (var dataMol in datalist2)
                {
                    sql = $"INSERT INTO PlanInfo VALUES (" +
                                            $"'{dataMol.CreateTime}'," +
                                            $"'{dataMol.ModuleA}'," +
                                            $"'{dataMol.ModuleAPlanCount}'," +
                                            $"'{dataMol.ModuleAInStockCount}'," +
                                            $"'{dataMol.ModuleACustomer}'," +
                                            $"'{dataMol.ModuleB}'," +
                                            $"'{dataMol.ModuleBPlanCount}'," +
                                            $"'{dataMol.ModuleBInStockCount}'," +
                                            $"'{dataMol.ModuleBCustomer}') ";
                    // LogHelper.WriteLogs(sql);
                    int Count = SQLiteHelper.ExecuteNonQuery(sql);
                    //LogHelper.WriteLogs(Count.ToString());
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

        [HttpPost]
        public string AddSettings(Settings model)
        {

            LogHelper.WriteLogs("提交数据AddSettings");
            PageResponse resp = new PageResponse();

            try
            {
                SQLiteHelper.ExecuteNonQuery("delete from  Settings");
                SQLiteHelper.ExecuteNonQuery($"INSERT INTO Settings VALUES('A','A','{model.TimeOut}','{model.Message}')");

            }
            catch (Exception e)
            {
                resp.Status = false;
                resp.Message = e.Message;
                LogHelper.WriteLogs(e.Message);
            }
            return JsonHelper.Instance.Serialize(resp);
        }

        public class StudentTemp : Student
        {
            public int LAY_TABLE_INDEX { get; set; }
        }

        public class Student
        {
            public string Name { get; set; }

            public string Age { get; set; }
        }
    }
}
