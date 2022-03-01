using BoardTab.Common;
using BoardTab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Services
{
    public interface IBoardService
    {
        bool AddCurrentNum(); 

        void InitailData();

        TimeListTemp GetTime(bool IsFirst);

        FiveMinute GetFive();

        OneMinute GetOne();

        string AddDaySet(string datalist);

        string AddMonthSet(string datalist);

        string AddTimeSet(string datalist);

        string GetDaySet(PageRequest request);

        string GetMonthSet(PageRequest request);

        string GetTimeSet(PageRequest request);

        string GetCurrentDayNum(PageRequest request);

        string UpdateCurrentDayNum(string datalist);
        string AddTitle(string Message);

        List<ExportMessage> GetExportMessage(string FromDate, string EndDate);



    }
}
