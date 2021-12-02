using ComBase.Controls;
using ComBase.Mvc.Utils;
using HC.Core.Common.Extension;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.Core.Service;
using HC.OSHA.Dto;
using HC.OSHA.Model;
using HC.OSHA.Repository;
using HC.OSHA.Service;
using HC_Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HC_OSHA.form.Schedule
{
    public partial class SCheduleRegularForm : CommonForm
    {
        private HcOshaVisitRepository hcOshaVisitRepository;
        private HcUserService hcUserService;
        private HcOshaSiteModelRepository hcOshaSiteModelRepository;
        private HcUsersRepository hcUsersRepository;

        private OshaPriceService oshaPriceService;

        public SCheduleRegularForm()
        {
            InitializeComponent();
            hcUserService = new HcUserService();
            hcOshaSiteModelRepository = new HcOshaSiteModelRepository();
            hcUsersRepository = new HcUsersRepository();
            hcOshaVisitRepository = new HcOshaVisitRepository();

            oshaPriceService = new OshaPriceService();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SCheduleRegularForm_Load(object sender, EventArgs e)
        {
            List<HC_USER> OSHAUsers = hcUserService.GetOsha();
            CboManager.SetItems(OSHAUsers, "Name", "UserId", "전체", "all", AddComboBoxPosition.Top);
            CboManager.SetValue(CommonService.Instance.Session.UserId);

            DateTime dateTime = codeService.CurrentDate;
            for (int i = 0; i <= 5; i++)
            {
                CboMonth.Items.Add(dateTime.AddMonths(-i).ToString("yyyy-MM"));
            }
            CboMonth.SelectedIndex = 0;
        }

        private void Search()
        {
            SSList.Columns().Clear();
            int columnCount = 12;
            SSList.AddColumn("사업장명", "name", 250, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
            SSList.AddColumn("사원수", "WORKERTOTALCOUNT", 70, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Right });

            string selectedYearMonth = CboMonth.GetValue();
            DateTime firstMonth = DateUtil.stringToDateTime(selectedYearMonth, DateTimeType.YYYY_MM);
            Dictionary<int, DateTime> calendars = new Dictionary<int, DateTime>();

            for (int i=0; i< columnCount; i++)
            {
                DateTime tmp = firstMonth.AddMonths(i);
                calendars.Add(i, tmp);

                SSList.AddColumn(DateUtil.DateTimeToStrig(tmp, DateTimeType.YYYY_MM), "m" + 1, 100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsSort = true });
            }

            string userId = CboManager.GetValue();
            HC_USER user = hcUsersRepository.FindOne(userId);
            Role role = Role.ENGINEER;
            if (user != null)
            {
                role = (Role)Enum.Parse(typeof(Role), user.Role);
            }

            SSList.ActiveSheet.RowCount = 0;
            List<HC_OSHA_SITE_MODEL> list = hcOshaSiteModelRepository.FIndByUserId(userId, role);

            SSList.ActiveSheet.RowCount = list.Count;

            int row = 0;
            //  최장 기간이 의사의 6개월이라 조회
            DateTime searchDateTime = firstMonth.AddMonths(-6);

            foreach (HC_OSHA_SITE_MODEL model in list)
            {
                List<OSHA_PRICE> list2 = oshaPriceService.FindAll(model.SITE_ID.ToString(), string.Empty, true);

                foreach (OSHA_PRICE dto in list2)
                {
                    List<OSHA_PRICE> childPrice = oshaPriceService.OshaPriceRepository.FindAllByParent(dto.SITE_ID);
                    foreach (OSHA_PRICE child in childPrice)
                    {
                        dto.WORKERTOTALCOUNT += child.WORKERTOTALCOUNT;
                    }

                    model.WORKERTOTALCOUNT = dto.WORKERTOTALCOUNT;
                }

                string lastVisitDate = string.Empty;
                int lastIndex = 0;
                int addMonth = 1;

                List<HC_OSHA_VISIT> visitList = hcOshaVisitRepository.FindLastVisiDate(model.ID, DateUtil.DateTimeToStrig(searchDateTime, DateTimeType.YYYYMMDDHHMMSS), userId, role);

                if (visitList.Count > 0)
                {
                    foreach (HC_OSHA_VISIT visit in visitList)
                    {
                        DateTime visitDate = Convert.ToDateTime(visit.VISITDATETIME);
                        lastVisitDate = DateUtil.DateTimeToStrig(visitDate, DateTimeType.YYYY_MM_DD);

                        if (visitDate.CompareTo(firstMonth) > 0)
                        {
                            int index = findColumnIndex(calendars, visitDate);
                            lastIndex = index;

                            SSList.ActiveSheet.Cells[row, index + 2].Text = DateUtil.DateTimeToStrig(visitDate, DateTimeType.YYYY_MM_DD);
                            SSList.ActiveSheet.Cells[row, index + 2].BackColor = Color.FromArgb(237, 211, 237);
                        }
                    }
                }

                SSList.ActiveSheet.Cells[row, 0].Text = model.NAME;
                SSList.ActiveSheet.Cells[row, 1].Text = model.WORKERTOTALCOUNT.ToString();

                //  사업장 방문주기
                //  100 이상 사업장
                //  의사 : 3개월
                //  위생 : 2개월
                //  간호 : 1개월
                //
                //  100명 미만 사업장 
                //  의사 : 6개월
                //  위생 : 3개월
                //  간호 : 1개월

                DateTime lastDateTime = DateTime.MinValue;
                #region 간호사
                
                if (role == Role.NURSE)
                {
                    if (lastVisitDate.Empty() || lastVisitDate.To<DateTime>().CompareTo(firstMonth) < 0)
                    {
                        lastIndex = lastIndex + 2;
                        lastDateTime = DateTime.ParseExact(string.Concat(CboMonth.Text, "-05"), "yyyy-MM-dd", null);
                    }
                    else
                    {
                        lastIndex = lastIndex + 3;
                        lastDateTime = DateTime.ParseExact(lastVisitDate, "yyyy-MM-dd", null);
                    }

                    addMonth = 1;
                    
                    if(lastIndex == 2)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth * -1);
                    }

                    for (int i = lastIndex; i < SSList.ColumnCount(); i++)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth);
                        switch (lastDateTime.ToDayOfWeek())
                        {
                            case "토요일":
                                SSList.SetCellValue(row, i, lastDateTime.AddDays(-1).ToString("yyyy-MM-dd"));
                                break;
                            case "일요일":
                                SSList.SetCellValue(row, i, lastDateTime.AddDays(1).ToString("yyyy-MM-dd"));
                                break;
                            default:
                                SSList.SetCellValue(row, i, lastDateTime.ToString("yyyy-MM-dd"));
                                break;
                        }
                    }
                }

                #endregion

                #region 의사

                else if (role == Role.DOCTOR)
                {
                    addMonth = 6;
                    int empCount = SSList.GetCellValue(row, 1).To<int>(0);
                    if (empCount >= 100)
                    {
                        addMonth = 3;
                    }

                    if (lastVisitDate.Empty())
                    {
                        lastIndex = lastIndex + 2;
                        lastDateTime = DateTime.ParseExact(string.Concat(CboMonth.Text, "-05"), "yyyy-MM-dd", null);
                    }
                    else
                    {
                        lastDateTime = DateTime.ParseExact(lastVisitDate, "yyyy-MM-dd", null);
                        lastIndex = lastIndex + 3;
                    }

                    if (lastIndex == 2)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth * -1);
                    }

                    for (int i = lastIndex; i < SSList.ColumnCount(); i++)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth);

                        int index = findColumnIndex(calendars, lastDateTime);
                        if(index < 0)
                        {
                            continue;
                        }
                        index += 2;
                        if(index >= SSList.ColumnCount())
                        {
                            break;
                        }
                        switch (lastDateTime.ToDayOfWeek())
                        {
                            case "토요일":
                                SSList.SetCellValue(row, index, lastDateTime.AddDays(-1).ToString("yyyy-MM-dd"));
                                break;
                            case "일요일":
                                SSList.SetCellValue(row, index, lastDateTime.AddDays(1).ToString("yyyy-MM-dd"));
                                break;
                            default:
                                SSList.SetCellValue(row, index, lastDateTime.ToString("yyyy-MM-dd"));
                                break;
                        }
                    }
                }

                #endregion

                #region 위생사

                else if (role == Role.ENGINEER)
                {
                    addMonth = 3;
                    int empCount = SSList.GetCellValue(row, 1).To<int>(0);
                    if (empCount >= 100)
                    {
                        addMonth = 2;
                    }

                    if (lastVisitDate.Empty())
                    {
                        lastIndex = lastIndex + 2;
                        lastDateTime = DateTime.ParseExact(string.Concat(CboMonth.Text, "-05"), "yyyy-MM-dd", null);
                    }
                    else
                    {
                        lastDateTime = DateTime.ParseExact(lastVisitDate, "yyyy-MM-dd", null);
                        lastIndex = lastIndex + 3;
                    }

                    if (lastIndex == 2)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth * -1);
                    }

                    for (int i = lastIndex; i < SSList.ColumnCount(); i++)
                    {
                        lastDateTime = lastDateTime.AddMonths(addMonth);

                        int index = findColumnIndex(calendars, lastDateTime);
                        if (index < 0)
                        {
                            continue;
                        }

                        index += 2;
                        if (index >= SSList.ColumnCount())
                        {
                            break;
                        }
                        switch (lastDateTime.ToDayOfWeek())
                        {
                            case "토요일":
                                SSList.SetCellValue(row, index, lastDateTime.AddDays(-1).ToString("yyyy-MM-dd"));
                                break;
                            case "일요일":
                                SSList.SetCellValue(row, index, lastDateTime.AddDays(1).ToString("yyyy-MM-dd"));
                                break;
                            default:
                                SSList.SetCellValue(row, index, lastDateTime.ToString("yyyy-MM-dd"));
                                break;
                        }
                    }
                }

                #endregion

                row += 1;
            }
        }

        private string FindDay(long visitWeek, long visitDay, DateTime month)
        {
            int weekNumber = 0;
            Calendar cac = CultureInfo.CurrentCulture.Calendar;

            for (int i=0; i< month.GetLastDate().Day; i++)
            {
                DateTime day = month.AddDays(i);
                if(day <= month.GetLastDate())
                {
                    DayOfWeek yoil = DayOfWeek.Monday;
                    if (visitDay == 1)
                    {
                        yoil = DayOfWeek.Monday;
                    }
                    else if (visitDay == 2)
                    {
                        yoil = DayOfWeek.Tuesday;
                    }
                    else if (visitDay == 3)
                    {
                        yoil = DayOfWeek.Wednesday;
                    }
                    else if (visitDay == 4)
                    {
                        yoil = DayOfWeek.Thursday;
                    }
                    else if (visitDay == 5)
                    {
                        yoil = DayOfWeek.Friday;
                    }
                    else if (visitDay == 6)
                    {
                        yoil = DayOfWeek.Saturday;
                    }
                    else if (visitDay == 7)
                    {
                        yoil = DayOfWeek.Sunday;
                    }

                    weekNumber = GetWeekNumber(day);

                    if (visitWeek == weekNumber && day.DayOfWeek == yoil)
                    {
                        return DateUtil.DateTimeToStrig(day, DateTimeType.YYYY_MM_DD).Substring(5) + "일";
                    }
                }
            }

            return "";
        }

        public int GetWeekNumber(DateTime dt)
        {
            DateTime now = dt;

            int basisWeekOfDay = (now.Day - 1) % 7;
            int thisWeek = (int)now.DayOfWeek;

            double val = Math.Ceiling((double)now.Day / 7);

            if (basisWeekOfDay > thisWeek) val++;

            return Convert.ToInt32(val);
        }

        private int findColumnIndex(Dictionary<int, DateTime> calendars, DateTime date)
        {
            foreach(KeyValuePair<int, DateTime> item in calendars)
            {
                if(item.Value.Year == date.Year && item.Value.Month == date.Month)
                {
                    return item.Key;
                }
            }
            return - 1;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }
    }
}
