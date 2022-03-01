using BoardTab.Common;
using BoardTab.Models;
using BoardTab.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Controllers
{
    public class BoardController : Controller
    {
        private readonly IBoardService _boardService;
        private IHostingEnvironment _hostingEnvironment;
        public BoardController(IBoardService boardService, IHostingEnvironment hostingEnvironment)
        {
            _boardService=boardService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Setting_Index()
        {
            string sql = $"select Message from Settings";
            ViewBag.Message = SQLiteHelper.ExecuteScalar(sql).ToString();
            ViewBag.FromDate = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            ViewBag.EndDate = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }

        [HttpGet("board/num")]
        public int GetNumber()
        {
            return 1;
        }
        [HttpGet("board/time")]
        public TimeListTemp GetTime(bool IsFirst)
        {
            return _boardService.GetTime(IsFirst);
        }
        [HttpGet("board/five")]
        public FiveMinute GetFive()
        {
            return _boardService.GetFive();
        }

        [HttpGet("board/one")]
        public OneMinute GetOne()
        {
            return _boardService.GetOne();
        }

        [HttpPost("board/basicsetting")]
        public void AddBasicSetting(BasicSetting model)
        { 
            
        }

        #region 参数配置
        public string TimeSettingList(PageRequest request)
        {
            return _boardService.GetTimeSet(request);
        }
        public string DaySettingList(PageRequest request)
        {
            return _boardService.GetDaySet(request);
        }
        public string MonthSettingList(PageRequest request)
        {
            return _boardService.GetMonthSet(request);
        }

        [HttpPost]
        public string AddTimeSet(string datalist)
        {
            return _boardService.AddTimeSet(datalist);
        }
        [HttpPost]
        public string AddDaySet(string datalist)
        {
            return _boardService.AddDaySet(datalist);
        }
        [HttpPost]
        public string AddMonthSet(string datalist)
        {
            return _boardService.AddMonthSet(datalist);
        }

        public string AddTitle(string Message)
        {
            return _boardService.AddTitle(Message);
        }
        #endregion


        public IActionResult Export(string FromDate, string EndDate)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
             var datalist = _boardService.GetExportMessage(FromDate, EndDate);

            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));  //Path.Combine把多个字符串组成一个路径
            using (ExcelPackage package = new ExcelPackage(file))   //ExcelPackage 操作excel的主要对象
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("aspnetcore");
                //添加头
                //worksheet.Cells[1, 1].Value = "日期";
                //worksheet.Cells[1, 2].Value = "时间段1";
                //worksheet.Cells[1, 3].Value = "时间段2";
                //worksheet.Cells[1, 4].Value = "时间段3";
                //worksheet.Cells[1, 5].Value = "教室照度均匀";
                //worksheet.Cells[1, 6].Value = "黑板照度";
                //worksheet.Cells[1, 7].Value = "黑板照度均匀";
                //worksheet.Cells[1, 8].Value = "色温";
                //worksheet.Cells[1, 9].Value = "显色指数";
                //worksheet.Cells[1, 10].Value = "炫光";
                int Count = 1;
                foreach (var model in datalist)
                {
                    worksheet.Cells["A" + Count.ToString()].Value = model.A1;
                    worksheet.Cells["B" + Count.ToString()].Value = model.A2;
                    worksheet.Cells["C" + Count.ToString()].Value = model.A3;
                    worksheet.Cells["D" + Count.ToString()].Value = model.A4;
                    worksheet.Cells["E" + Count.ToString()].Value = model.A5;
                    worksheet.Cells["F" + Count.ToString()].Value = model.A6;
                    worksheet.Cells["G" + Count.ToString()].Value = model.A7;
                    worksheet.Cells["H" + Count.ToString()].Value = model.A8;
                    worksheet.Cells["I" + Count.ToString()].Value = model.A9;
                    worksheet.Cells["J" + Count.ToString()].Value = model.A10;
                    worksheet.Cells["K" + Count.ToString()].Value = model.A11;
                    worksheet.Cells["L" + Count.ToString()].Value = model.A12;
                    worksheet.Cells["M" + Count.ToString()].Value = model.A13;
                    Count++;
                }

                package.Save();
            }
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public PageResponse SysLogin(string UserName, string Password)
        {
            PageResponse pageResponse = new PageResponse();

            if (UserName == "admin" && Password == "123456")
            {
                pageResponse.Message = "Station";
            }
            else
            {
                ModelState.AddModelError("Error", "");
                pageResponse.Status = false;
            }
            return pageResponse;

        }

        /// <summary>
        /// 当前日期配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetCurrentDayNum(PageRequest request)
        {
            return _boardService.GetCurrentDayNum(request);
        }

        [HttpPost]
        public string UpdateCurrentDayNum(string datalist)
        {
            return _boardService.UpdateCurrentDayNum(datalist);
        } 
    }
}
