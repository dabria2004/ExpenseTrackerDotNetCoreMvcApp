﻿using ExpenseTrackerDotNetCoreMvcApp.Models;
using ExpenseTrackerDotNetCoreMvcApp.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTrackerDotNetCoreMvcApp.Controllers
{
    public class ReportChartController : Controller
    {
        private readonly DapperService _db;

        public ReportChartController(DapperService db)
        {
            _db = db;
        }

        [ActionName("Index")]
        public IActionResult Index()
        {
            ViewBag.line_chart = LineChart();
            ViewBag.pie_chart = PieChart();
            return View();
        }

        public LineChartModel LineChart()
        {
            string query = @"select SUM(Amount) amount, YEAR(Date) show_date,
                            TransactionType transaction_type
                            from [dbo].[Tbl_ExpenseTracker]
                            group by YEAR(Date),TransactionType";
            var list = _db.GetList<MonthlyChartModel>(query);
            //string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            //List<string> lst = monthNames.ToList();
            //lst.RemoveAt(12);
            //monthNames = lst.ToArray();
            List<int> filter = new List<int>();
            List<int> count = new List<int>();
            count = list.GroupBy(x => x).Select(i => i.First().show_date).ToList();
            for (int i = 0; i < count.Count / 2; i++)
            {
                filter.Add(count[i]);
            }
            LineChartModel model = new LineChartModel
            {
                LineChartIncomeLabel = list.Where(x => x.transaction_type == "Income")
                                       .Select(x => x.amount).ToList(),
                LineChartExpenseLabel = list.Where(x => x.transaction_type == "Expense")
                                       .Select(x => x.amount).ToList(),
                LineCharYearLabel = filter,
            };
            return model;
        }

        public PieChartModel PieChart()
        {
            string query = @"select SUM(Amount) amount,CONVERT(varchar(7), Date, 126) show_date,
                                TransactionType transaction_type
                                from [dbo].[Tbl_ExpenseTracker] 
                                where CONVERT(varchar(7), Date, 126) = CONVERT(varchar(7), GETDATE() ,126)
                                group by CONVERT(varchar(7), Date, 126), TransactionType";
            var list = _db.GetList<CurrentMonthModel>(query);
            //List<int> income = new List<int>();
            //List<int> expense = new List<int>();
            //expense = list.Where(x => x.transaction_type == "Expense").Select(x => x.amount).ToList();
            //income = list.Where(x => x.transaction_type == "Income").Select(x => x.amount).ToList();
            //string income_percent = Convert.ToString((Convert.ToDouble(expense[0]) / income[0]) * 100) + "%"; 
            PieChartModel model = new PieChartModel 
            {
                   //PieChartYearLabel = list.Select(x=> x.show_date).ToList(),
                   //PieChartIncomeLabel = list.Where(x => x.transaction_type == "Income")
                   //                    .Select(x => x.amount).ToList(),
                   //PieChartExpenseLabel = list.Where(x => x.transaction_type == "Expense")
                   //                    .Select(x => x.amount).ToList(),
                   PieChartLabel = list.Select(x => x.amount).ToList(),
            };
            return model;
        }
    }
}
