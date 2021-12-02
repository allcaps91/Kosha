using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHaSetExam_Daily.cs
/// Description     : 종합검진 감액코드 관리(종검)
/// Author          : 김민철
/// Create Date     : 2020-03-17
/// Update History  : 
/// </summary>
/// <seealso cref= "Frm검사별요일별정원세팅(Frm검사별요일별정원세팅.frm)" />
namespace ComHpcLibB
{
    public partial class frmHaSetExam_Daily : Form
    {
        clsSpread cSpd = null;
        HeaResvSetService heaResvSetService = null;
        HeaCodeService heaCodeService = null;

        public frmHaSetExam_Daily()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();
            heaResvSetService = new HeaResvSetService();
            heaCodeService = new HeaCodeService();

            SS1.ActiveSheet.Columns.Get(0).Visible = false;
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormload);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void Data_Save()
        {
            string strExGubun = string.Empty;
            string strExCode = string.Empty;
            string strYoil = string.Empty;

            long nAmInwon = 0;
            long nPmInwon = 0;

            int result = 0;

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i += 2)
            {
                strExGubun = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                strExCode = SS1.ActiveSheet.Cells[i, 1].Text.Trim();

                for (int j = 1; j < 8; j++)
                {
                    strYoil = j.To<string>();

                    nAmInwon = SS1.ActiveSheet.Cells[i, j + 2].Text.To<long>();
                    nPmInwon = SS1.ActiveSheet.Cells[i + 1, j + 2].Text.To<long>();

                    HEA_RESV_SET item = heaResvSetService.GetCountByGubunYoil("1999-01-01", "3", strExGubun, strYoil);

                    HEA_RESV_SET dto = new HEA_RESV_SET
                    {
                        AMINWON = nAmInwon,
                        PMINWON = nPmInwon,
                        YOIL = strYoil,
                        SDATE = "1999-01-01",
                        GUBUN = strExGubun,
                        EXAMNAME = strExCode,
                        ENTSABUN = clsType.User.IdNumber.To<long>(),
                        GBRESV = "3",
                        RID = item.RID
                    };

                    if (!item.IsNullOrEmpty())
                    {
                        result = heaResvSetService.UpDate(dto);
                    }
                    else
                    {
                        result = heaResvSetService.Insert(dto);
                    }

                    if (result <= 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        MessageBox.Show("DB 작업시 오류발생", "Rollback");
                        return;
                    }
                }
            }

            for (int j = 2; j < SS2.ActiveSheet.ColumnCount; j++)
            {
                strYoil = (j - 1).To<string>();
                nAmInwon = SS2.ActiveSheet.Cells[0, j].Text.To<long>();
                nPmInwon = SS2.ActiveSheet.Cells[1, j].Text.To<long>();
                strExGubun = "TT";
                HEA_RESV_SET item = heaResvSetService.GetCountByGubunYoil("1999-01-01", "3", strExGubun, strYoil);

                HEA_RESV_SET dto = new HEA_RESV_SET
                {
                    AMINWON = nAmInwon,
                    PMINWON = nPmInwon,
                    YOIL = strYoil,
                    SDATE = "1999-01-01",
                    GUBUN = "TT",
                    EXAMNAME = "종검정원기초",
                    ENTSABUN = clsType.User.IdNumber.To<long>(),
                    GBRESV = "3",
                    RID = item.RID
                };

                if (!item.IsNullOrEmpty())
                {
                    result = heaResvSetService.UpDate(dto);
                }
                else
                {
                    result = heaResvSetService.Insert(dto);
                }

                if (result <= 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("DB 작업시 오류발생", "Rollback");
                    return;
                }
            }

            MessageBox.Show("작업완료", "확인");

            clsDB.setCommitTran(clsDB.DbCon);
        }

        private void eFormload(object sender, EventArgs e)
        {
            Screen_Clear();
            Screen_Display();
        }

        private void Screen_Display()
        {
            int nRow = 0;
            string strExName = string.Empty;

            SS2.ActiveSheet.Cells[0, 0].Text = "종합검진 정원";
            SS2.ActiveSheet.Cells[0, 1].Text = "AM";
            SS2.ActiveSheet.Cells[1, 1].Text = "PM";
            SS2.ActiveSheet.AddSpanCell(0, 0, 2, 1);

            List<HEA_RESV_SET> lst = heaResvSetService.GetListByInwonSet("1999-01-01", "3", "TT");

            if (lst.Count > 0)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    switch (lst[i].YOIL.Trim())
                    {
                        case "1": SS2.ActiveSheet.Cells[0, 2].Text = lst[i].AMINWON.To<string>(); break;
                        case "2": SS2.ActiveSheet.Cells[0, 3].Text = lst[i].AMINWON.To<string>(); break;
                        case "3": SS2.ActiveSheet.Cells[0, 4].Text = lst[i].AMINWON.To<string>(); break;
                        case "4": SS2.ActiveSheet.Cells[0, 5].Text = lst[i].AMINWON.To<string>(); break;
                        case "5": SS2.ActiveSheet.Cells[0, 6].Text = lst[i].AMINWON.To<string>(); break;
                        case "6": SS2.ActiveSheet.Cells[0, 7].Text = lst[i].AMINWON.To<string>(); break;
                        case "7": SS2.ActiveSheet.Cells[0, 8].Text = lst[i].AMINWON.To<string>(); break;
                        default: break;
                    }

                    switch (lst[i].YOIL.Trim())
                    {
                        case "1": SS2.ActiveSheet.Cells[1, 2].Text = lst[i].PMINWON.To<string>(); break;
                        case "2": SS2.ActiveSheet.Cells[1, 3].Text = lst[i].PMINWON.To<string>(); break;
                        case "3": SS2.ActiveSheet.Cells[1, 4].Text = lst[i].PMINWON.To<string>(); break;
                        case "4": SS2.ActiveSheet.Cells[1, 5].Text = lst[i].PMINWON.To<string>(); break;
                        case "5": SS2.ActiveSheet.Cells[1, 6].Text = lst[i].PMINWON.To<string>(); break;
                        case "6": SS2.ActiveSheet.Cells[1, 7].Text = lst[i].PMINWON.To<string>(); break;
                        case "7": SS2.ActiveSheet.Cells[1, 8].Text = lst[i].PMINWON.To<string>(); break;
                        default: break;
                    }
                }
            }

            //검사항목 Display
            IList<HEA_CODE> lst2 = heaCodeService.GetItemByGubunGroupBy("13");

            if (lst2.Count > 0)
            {
                SS1.ActiveSheet.RowCount = lst2.Count * 2;

                for (int i = 0; i < lst2.Count; i++)
                {
                    SS1.ActiveSheet.Cells[nRow * 2, 0].Text = lst2[i].GUBUN2.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2, 1].Text = lst2[i].NAME.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2, 2].Text = "AM";

                    SS1.ActiveSheet.AddSpanCell(nRow * 2, 1, 2, 1);

                    SS1.ActiveSheet.Cells[nRow * 2 + 1, 0].Text = lst2[i].GUBUN2.Trim();
                    SS1.ActiveSheet.Cells[nRow * 2 + 1, 2].Text = "PM";

                    nRow = nRow + 1;
                }
            }

            //선택검사 인원 세팅사항
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i += 2)
            {
                strExName = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                //인원조회
                for (int j = 1; j < 7; j++)
                {
                    HEA_RESV_SET item1 = heaResvSetService.GetCountByGubunYoil("1999-01-01", "3", strExName, j.To<string>());
                    
                    if (!item1.IsNullOrEmpty())
                    {
                        switch (j)
                        {
                            case 1:
                                SS1.ActiveSheet.Cells[i, 3].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 3].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 2:
                                SS1.ActiveSheet.Cells[i, 4].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 4].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 3:
                                SS1.ActiveSheet.Cells[i, 5].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 5].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 4:
                                SS1.ActiveSheet.Cells[i, 6].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 6].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 5:
                                SS1.ActiveSheet.Cells[i, 7].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 7].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 6:
                                SS1.ActiveSheet.Cells[i, 8].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 8].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            case 7:
                                SS1.ActiveSheet.Cells[i, 9].Text     = item1.AMINWON.To<string>(); //오전
                                SS1.ActiveSheet.Cells[i + 1, 9].Text = item1.PMINWON.To<string>(); //오후
                                break;
                            default: break;
                        }
                    }
                }
            }

        }

        private void Screen_Clear()
        {

            cSpd.Spread_Clear_Simple(SS1, 25);
            cSpd.Spread_Clear_Simple(SS2, 2);
        }
    }
}
