using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHaLtdGaResv.cs
/// Description     : 회사별 가예약 관리
/// Author          : 김민철
/// Create Date     : 2019-10-07 
/// Update History  : 
/// <seealso cref="\Hea\HaMain\Frm회사별가예약.frm(Frm회사별가예약)"/>
/// </summary>
namespace ComHpcLibB
{
    public partial class frmHaLtdGaResv : Form
    {
        ComFunc CF = null;
        HicLtdService             hicLtdCodeService = null;
        HeaResvLtdService         heaResvLtdService = null;
        HeaResvExamPatientService heaResvExamPatientService = null;
        HeaJepsuService           heaJepsuService = null;
        HicCodeService            hicCodeService = null;
        HeaCodeService            heaCodeService = null;
        HicLtdService             hicLtdService = null;
        HeaResvLtdHisService      heaResvLtdHisService = null;
        HeaResvSetService         heaResvSetService = null;
        HeaResvExamService        heaResvExamService = null;

        public frmHaLtdGaResv()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        private void SetControl()
        {
            CF = new ComFunc();
            hicLtdCodeService           = new HicLtdService();
            heaResvLtdService           = new HeaResvLtdService();
            heaResvExamPatientService   = new HeaResvExamPatientService();
            heaJepsuService             = new HeaJepsuService();
            hicCodeService              = new HicCodeService();
            heaCodeService              = new HeaCodeService();
            hicLtdService               = new HicLtdService();
            heaResvLtdHisService        = new HeaResvLtdHisService();
            heaResvSetService           = new HeaResvSetService();
            heaResvExamService          = new HeaResvExamService();

            ssList.Initialize(new SpreadOption { ColumnHeaderHeight = 33, RowHeight = 28 });
            ssList.AddColumn("코드",        nameof(HIC_LTD.CODE),     44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            ssList.AddColumn("사업장명",    nameof(HIC_LTD.SANGHO),  180, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });

            SS1.Initialize(new SpreadOption { ColumnHeaderHeight = 33, RowHeight = 38 });
            SS1.AddColumn("예약일자",  nameof(HEA_RESV_LTD.SDATE), 84, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("종검",      "",                         44, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS1.AddColumn("선택검사",  "",                        500, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false, Aligen = CellHorizontalAlignment.Left });

            SS2.Initialize(new SpreadOption { ColumnHeaderHeight = 33, RowHeight = 30 });
            SS2.AddColumn("선택검사명", "",                         72, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left });
            SS2.AddColumn("오전",       "",                         38, FpSpreadCellType.TextCellType);
            SS2.AddColumn("오후",       "",                         38, FpSpreadCellType.TextCellType);
            SS2.AddColumn("오전예약",   "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false});
            SS2.AddColumn("오후예약",   "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsEditble = false });
            SS2.AddColumn(" ",          "",                          5, FpSpreadCellType.TextCellType);
            SS2.AddColumn("오전여유",   "",                         38, FpSpreadCellType.TextCellType);
            SS2.AddColumn("오후여유",   "",                         38, FpSpreadCellType.TextCellType);
            SS2.AddColumn("코드",       "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("ROWID",      "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("오전",       "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("오후",       "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("오전여유",   "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
            SS2.AddColumn("오후여유",   "",                         38, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { IsVisivle = false });
        }

        private void SetEvent()
        {
            this.Load                    += new EventHandler(eFormLoad);
            this.btnExit.Click           += new EventHandler(eBtnClick);
            this.btnClose.Click          += new EventHandler(eBtnClick);
            this.btnSearch.Click         += new EventHandler(eBtnClick);
            this.btnSave.Click           += new EventHandler(eBtnClick);
            this.btnDelete.Click         += new EventHandler(eBtnClick);
            this.btnSearch_LtdInfo.Click += new EventHandler(eBtnClick);
            this.btnDelete_All.Click     += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick     += new CellClickEventHandler(eSpdDblClick);
            this.ssList.CellDoubleClick  += new CellClickEventHandler(eSpdDblClick);
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnClose)
            {
                grpLtdRemark.Visible = false;
            }
            //회사 참고사항
            else if (sender == btnSearch_LtdInfo)
            {
                grpLtdRemark.Visible = true;

                txtLtdRemark.Text = "";
                txtLtdRemark.Text = hicLtdCodeService.GetHaRemarkbyLtdCode(txtLtdCode.Text.To<long>());
            }
            //일괄삭제
            else if (sender == btnDelete_All)
            {
                Delete_GaResv_Ltd(txtLtdCode.Text);
            }
            //회사별 선택검사 예약현황
            else if (sender == btnSearch)
            {
                DisPlay_Exam_GaResv_Ltd(txtLtdCode.Text);
            }
            //회사별 선택검사 저장
            else if (sender == btnSave)
            {
                Data_Save_ResvExam(txtLtdCode.Text);
            }
            //회사별 선택검사 삭제
            else if (sender == btnDelete)
            {
                Data_Delete_ResvExam(txtLtdCode.Text);
            }
        }

        private void Data_Delete_ResvExam(string argLtdCode)
        {
            int nAmJan = 0, nPmJan = 0;
            int nAmInwon = 0, nPmInwon = 0;
            int result = 0;
            string strLtdCode = argLtdCode;
            string strYoil = string.Empty;
            string strExGubun = string.Empty;
            string strRowid = string.Empty;
            bool bDel = false;

            if (strLtdCode.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            string strSDate = dtpDate.Text;

            switch (VB.Left(CF.READ_YOIL(clsDB.DbCon, strSDate), 1))
            {
                case "월": strYoil = "1"; break;
                case "화": strYoil = "2"; break;
                case "수": strYoil = "3"; break;
                case "목": strYoil = "4"; break;
                case "금": strYoil = "5"; break;
                case "토": strYoil = "6"; break;
                case "일": strYoil = "7"; break;
                default:
                    break;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {

                nAmInwon = 0;
                nPmInwon = 0;
                if (!SS2.ActiveSheet.Cells[i, 3].Text.IsNullOrEmpty()) { nAmJan = nAmInwon - Convert.ToInt32(VB.Pstr(SS2.ActiveSheet.Cells[i, 3].Text, "/", 1)); }
                if (!SS2.ActiveSheet.Cells[i, 4].Text.IsNullOrEmpty()) { nPmJan = nPmInwon - Convert.ToInt32(VB.Pstr(SS2.ActiveSheet.Cells[i, 4].Text, "/", 1)); }
                if (nAmJan < 0) { nAmJan = 0; }
                if (nPmJan < 0) { nPmJan = 0; }
                strExGubun = SS2.ActiveSheet.Cells[i, 8].Text;
                strRowid = SS2.ActiveSheet.Cells[i, 9].Text;

                if (strRowid.IsNullOrEmpty())
                {
                    if (nAmInwon > 0 || nPmInwon > 0 || nAmJan > 0 || nPmJan > 0)
                    {
                        HEA_RESV_LTD item = new HEA_RESV_LTD
                        {
                            SDATE = strSDate,
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdService.InsertData(item);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "I",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }
                }
                else
                {
                    if (nAmInwon == 0 && nPmInwon == 0 && nAmJan == 0 && nPmJan == 0)
                    {
                        result = heaResvLtdService.DeleteDataByRowid(strRowid);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        bDel = true;
                    }
                    else
                    {
                        HEA_RESV_LTD item3 = new HEA_RESV_LTD
                        {
                            SDATE = strSDate,
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJEPSU = nAmJan,
                            PMJEPSU = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdService.UpDateInwon(item3);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        bDel = false;
                    }

                    if (bDel)
                    {
                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "D",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = 0,
                            PMINWON = 0,
                            AMJEPSU = 0,
                            PMJEPSU = 0,
                            AMJAN = 0,
                            PMJAN = 0,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                    }
                    else
                    {
                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "S",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 0;

            UpDate_GaResv_Ltd(argLtdCode);      //가예약 사업장 Data 정리
            DisPlay_GaResv_Ltd(argLtdCode);     //사업장 가예약 현황 
        }

        private void Data_Save_ResvExam(string argLtdCode)
        {
            int nAmJan = 0, nPmJan = 0, nAmJepsu = 0, nPmJepsu = 0;
            int nAmJan2 = 0, nPmJan2 = 0, nAmCount = 0, nPmCount = 0;
            int nAmInwon = 0, nPmInwon = 0;
            int result = 0;
            string strLtdCode = argLtdCode;
            string strYoil = string.Empty;
            string strExName = string.Empty;
            string strMsg = string.Empty;
            string strExGubun = string.Empty;
            string strRowid = string.Empty;
            bool bDel = false;

            if (strLtdCode.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            string strSDate = dtpDate.Text;

            switch (VB.Left(CF.READ_YOIL(clsDB.DbCon, strSDate), 1))
            {
                case "월": strYoil = "1"; break;
                case "화": strYoil = "2"; break;
                case "수": strYoil = "3"; break;
                case "목": strYoil = "4"; break;
                case "금": strYoil = "5"; break;
                case "토": strYoil = "6"; break;
                case "일": strYoil = "7"; break;
                default:
                    break;
            }

            //해당일자 예약상황을 다시 읽음
            Yeyak_Inwon_ReCheck(argLtdCode);

            //인원초과 특정일자는 접수불가 처리
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strExName = SS2.ActiveSheet.Cells[i, 0].Text;

                nAmJan2 = SS2.ActiveSheet.Cells[i, 12].Text.To<int>();      //최초보관 오전여유
                nPmJan2 = SS2.ActiveSheet.Cells[i, 13].Text.To<int>();      //최초보관 오후여유
                nAmJan2 = nAmJan2 + SS2.ActiveSheet.Cells[i, 10].Text.To<int>();    //최초보관 오전인원
                nPmJan2 = nPmJan2 + SS2.ActiveSheet.Cells[i, 11].Text.To<int>();    //최초보관 오후인원
                nAmJan2 = nAmJan2 - SS2.ActiveSheet.Cells[i, 1].Text.To<int>();     //변경 오전인원
                nAmCount = SS2.ActiveSheet.Cells[i, 1].Text.To<int>();

                nPmJan2 = nPmJan2 - SS2.ActiveSheet.Cells[i, 2].Text.To<int>();     //변경 오후인원
                nPmCount = SS2.ActiveSheet.Cells[i, 2].Text.To<int>();

                SS2.ActiveSheet.Cells[i, 6].Text = nAmJan2.To<string>();
                SS2.ActiveSheet.Cells[i, 7].Text = nPmJan2.To<string>();

                //if (!strExName.Equals("심장초음파") && !strExName.Equals("경동맥초음파"))
                //{
                    if (nAmCount > 0 || nPmCount > 0)
                    {
                        if ((nAmCount > 0 && nAmJan2 < 0) || (nPmCount > 0 && nPmJan2 < 0))
                        {
                            strMsg = "해당일자에 " + strExName + " 검사가 정원을 초과함" + ComNum.VBLF;
                            if (nAmJan2 < 0) { strMsg = strMsg + "오전 정원초과: " + (nAmJan2 * -1).To<string>() + "명 " + ComNum.VBLF; }
                            if (nPmJan2 < 0) { strMsg = strMsg + "오후 정원초과: " + (nPmJan2 * -1).To<string>() + "명 " + ComNum.VBLF; }
                            strMsg = strMsg + "예약정원 초과로 접수가 불가능합니다.";
                            MessageBox.Show(strMsg, "예약정원 초과");
                            return;
                        }
                    }
                //}
            }

            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                nAmJan = SS2.ActiveSheet.Cells[i, 1].Text.To<int>();
                nPmJan = SS2.ActiveSheet.Cells[i, 2].Text.To<int>();
                nAmJepsu = SS2.ActiveSheet.Cells[i, 3].Text.To<int>();
                nPmJepsu = SS2.ActiveSheet.Cells[i, 4].Text.To<int>();

                nAmInwon = nAmJepsu + nAmJan;
                nPmInwon = nPmJepsu + nPmJan;

                strExGubun = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                strRowid = SS2.ActiveSheet.Cells[i, 9].Text;

                if (strRowid.Equals(""))
                {
                    if (nAmInwon > 0 || nPmInwon > 0 || nAmJan > 0 || nPmJan > 0 && strExGubun != "")
                    {
                        HEA_RESV_LTD item = new HEA_RESV_LTD
                        {
                            SDATE = strSDate,
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJEPSU = nAmJepsu,
                            PMJEPSU = nPmJepsu,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            //AMJEPSU = nAmJan,
                            //PMJEPSU = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdService.InsertData(item);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "I",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJEPSU = nAmJepsu,
                            PMJEPSU = nPmJepsu,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }
                }
                else
                {
                    if (nAmInwon == 0 && nPmInwon == 0 && nAmJepsu == 0 && nPmJepsu == 0 && nAmJan == 0 && nPmJan == 0 )
                    {
                        result = heaResvLtdService.DeleteDataByRowid(strRowid);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        bDel = true;
                    }
                    else
                    {
                        HEA_RESV_LTD item3 = new HEA_RESV_LTD
                        {
                            SDATE = strSDate,
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            //AMJEPSU = nAmJan,
                            //PMJEPSU = nPmJan,
                            AMJEPSU = nAmJepsu,
                            PMJEPSU = nPmJepsu,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil,
                            RID = strRowid
                        };

                        result = heaResvLtdService.UpDateInwon(item3);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                        bDel = false;
                    }

                    if (bDel)
                    {
                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "D",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = 0,
                            PMINWON = 0,
                            AMJEPSU = 0,
                            PMJEPSU = 0,
                            AMJAN = 0,
                            PMJAN = 0,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }

                    }
                    else
                    {
                        HEA_RESV_LTD_HIS item2 = new HEA_RESV_LTD_HIS
                        {
                            JOBSABUN = clsType.User.IdNumber.To<long>(),
                            GBJOB = "S",
                            SDATE = Convert.ToDateTime(strSDate),
                            GUBUN = strExGubun,
                            LTDCODE = strLtdCode.To<long>(),
                            ENTSABUN = clsType.User.IdNumber.To<long>(),
                            AMINWON = nAmInwon,
                            PMINWON = nPmInwon,
                            AMJEPSU = nAmJepsu,
                            PMJEPSU = nPmJepsu,
                            AMJAN = nAmJan,
                            PMJAN = nPmJan,
                            YOIL = strYoil
                        };

                        result = heaResvLtdHisService.InsertData(item2);
                        if (result <= 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                        }
                    }
                }
            }

            clsDB.setCommitTran(clsDB.DbCon);

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 0;

            DisPlay_GaResv_Ltd(argLtdCode);     //사업장 가예약 현황 

        }

        private void Yeyak_Inwon_ReCheck(string argLtdCode)
        {
            int nCNT1 = 0;
            int nCNT2 = 0;
            int nCNT3 = 0;
            int nCNT4 = 0;
            int nRow = 0;
            string strLtdCode = argLtdCode;
            string strGubun = string.Empty;

            if (strLtdCode.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            //회사별 설정한 인원수를 표시함
            List<HEA_RESV_LTD> list = heaResvLtdService.GetListInwonByLtdCode(argLtdCode.To<long>(), dtpDate.Text);

            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    strGubun = list[i].GUBUN;
                    nCNT1 = (int)list[i].AMINWON;
                    nCNT2 = (int)list[i].PMINWON;
                    nCNT3 = (int)list[i].AMJAN;
                    nCNT4 = (int)list[i].PMJAN;

                    nRow = 0;
                    for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                    {
                        if (SS2.ActiveSheet.Cells[j, 8].Text.Trim() == strGubun)
                        {
                            nRow = j;
                            break;
                        }
                    }

                    SS2.ActiveSheet.Cells[nRow, 3].Text = list[i].AMJEPSU.To<string>();
                    SS2.ActiveSheet.Cells[nRow, 4].Text = list[i].PMJEPSU.To<string>();
                }
            }

            //예약 가능한 인원수를 표시함
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strGubun = SS2.ActiveSheet.Cells[i, 8].Text.Trim();

                //종검,선택검사 인원수
                HEA_RESV_SET item = heaResvSetService.GetItemByGubun(dtpDate.Text, strGubun);
                if (!item.IsNullOrEmpty())
                {
                    nCNT1 = (int)item.AMINWON;
                    nCNT2 = (int)item.PMINWON;
                }

                //예약인원수를 찾음
                if (strGubun.Equals("00"))      //종검
                {
                    HEA_JEPSU item2 = heaJepsuService.GetSumAmPmCount1(dtpDate.Text, 0);
                    if (!item2.IsNullOrEmpty())
                    {
                        nCNT1 = nCNT1 - item2.AMCNT.To<int>();
                        nCNT2 = nCNT2 - item2.PMCNT.To<int>();
                    }
                }
                else
                {
                    HEA_RESV_EXAM item2 = heaResvExamService.GetCountAMPMbyRTime(dtpDate.Text, CF.DATE_ADD(clsDB.DbCon, dtpDate.Text, +1), strGubun);
                    if (!item2.IsNullOrEmpty())
                    {
                        nCNT1 = nCNT1 - item2.AMCNT.To<int>();
                        nCNT2 = nCNT2 - item2.PMCNT.To<int>();
                    }
                }

                //개인별 가예약
                HEA_RESV_SET item3 = heaResvSetService.GetSumGaInwonAMPMByGubun(dtpDate.Text, strGubun);
                if (!item3.IsNullOrEmpty())
                {
                    nCNT1 = nCNT1 - (int)item3.GAINWONAM;
                    nCNT2 = nCNT2 - (int)item3.GAINWONPM;
                }

                //회사별 가예약 인원수를 찾음
                HEA_RESV_LTD item4 = heaResvLtdService.GetSumAmPmJanByGubun(dtpDate.Text, strGubun);
                if (!item4.IsNullOrEmpty())
                {
                    nCNT1 = nCNT1 - (int)item4.AMJAN;
                    nCNT2 = nCNT2 - (int)item4.PMJAN;
                }

                SS2.ActiveSheet.Cells[i, 6].Text = nCNT1.To<string>();
                SS2.ActiveSheet.Cells[i, 7].Text = nCNT2.To<string>();

                //변동된 여유인원수 계산을 위해 보관함
                SS2.ActiveSheet.Cells[i, 12].Text = nCNT1.To<string>();
                SS2.ActiveSheet.Cells[i, 13].Text = nCNT2.To<string>();
            }
        }

        private void DisPlay_Exam_GaResv_Ltd(string argLtdCode)
        {
            int nCNT1 = 0;
            int nCNT2 = 0;
            int nCNT3 = 0;
            int nCNT4 = 0;
            int nRow = 0;

            string strLtdCode = txtLtdCode.Text.Trim();
            string strGubun = string.Empty;

            if (strLtdCode.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            SS2.ActiveSheet.ClearRange(0, 0, SS2.ActiveSheet.Rows.Count, SS2.ActiveSheet.ColumnCount, true);
            SS2.ActiveSheet.Rows.Count = 50;

            SS2.ActiveSheet.Cells[0, 0].Text = "종합검진";
            SS2.ActiveSheet.Cells[0, 8].Text = "00";

            //검사항목 Display
            List<HEA_CODE> list1 = heaCodeService.GetGroupNameByGubun("13");

            SS2.ActiveSheet.RowCount = list1.Count + 1;

            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    SS2.ActiveSheet.Cells[i + 1, 0].Text = " " + list1[i].NAME;
                    SS2.ActiveSheet.Cells[i + 1, 8].Text = " " + list1[i].GUBUN2;
                }
            }

            //회사별 설정한 인원수를 표시함
            List<HEA_RESV_LTD> list2 = heaResvLtdService.GetListInwonByLtdCode(argLtdCode.To<long>(), dtpDate.Text);

            if (list2.Count > 0)
            {
                for (int i = 0; i < list2.Count; i++)
                {
                    strGubun = list2[i].GUBUN;
                    nCNT1 = (int)list2[i].AMINWON;
                    nCNT2 = (int)list2[i].PMINWON;
                    nCNT3 = (int)list2[i].AMJAN;
                    nCNT4 = (int)list2[i].PMJAN;

                    if (nCNT3 < 0) { nCNT3 = 0; }
                    if (nCNT4 < 0) { nCNT4 = 0; }

                    nRow = 0;
                    for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                    {
                        if (SS2.ActiveSheet.Cells[j, 8].Text.Trim() == strGubun)
                        {
                            nRow = j;
                            break;
                        }
                    }

                    if(nCNT3 ==0 )
                    {
                        SS2.ActiveSheet.Cells[nRow, 1].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 1].Text = nCNT3.To<string>();
                    }

                    if (nCNT4 == 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 2].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 2].Text = nCNT4.To<string>();
                    }

                    if (list2[i].AMJEPSU == 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 3].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 3].Text = list2[i].AMJEPSU.To<string>();
                    }

                    if (list2[i].PMJEPSU == 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 4].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 4].Text = list2[i].PMJEPSU.To<string>();
                    }

                    SS2.ActiveSheet.Cells[nRow, 9].Text = list2[i].RID;

                    if (nCNT3 == 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 10].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 10].Text = nCNT3.To<string>();
                    }

                    if (nCNT4 == 0)
                    {
                        SS2.ActiveSheet.Cells[nRow, 11].Text = "";
                    }
                    else
                    {
                        SS2.ActiveSheet.Cells[nRow, 11].Text = nCNT4.To<string>();
                    }

                    //SS2.ActiveSheet.Cells[nRow, 1].Text = nCNT3.To<string>();
                    //SS2.ActiveSheet.Cells[nRow, 2].Text = nCNT4.To<string>();
                    //SS2.ActiveSheet.Cells[nRow, 3].Text = list2[i].AMJEPSU.To<string>();
                    //SS2.ActiveSheet.Cells[nRow, 4].Text = list2[i].PMJEPSU.To<string>();
                    //SS2.ActiveSheet.Cells[nRow, 9].Text = list2[i].RID;
                    //SS2.ActiveSheet.Cells[nRow, 10].Text = nCNT3.To<string>();
                    //SS2.ActiveSheet.Cells[nRow, 11].Text = nCNT4.To<string>();
                }
            }

            //예약 가능한 인원수를 표시함
            for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
            {
                strGubun = SS2.ActiveSheet.Cells[i, 8].Text.Trim();

                //종검,선택검사 인원수
                HEA_RESV_SET item = heaResvSetService.GetItemByGubun(dtpDate.Text, strGubun);
                if (!item.IsNullOrEmpty())
                {
                    nCNT1 = (int)item.AMINWON;
                    nCNT2 = (int)item.PMINWON;
                }

                //예약인원수를 찾음
                if (strGubun.Equals("00"))      //종검
                {
                    HEA_JEPSU item2 = heaJepsuService.GetSumAmPmCount1(dtpDate.Text, 0);
                    if (!item2.IsNullOrEmpty())
                    {
                        nCNT1 = nCNT1 - item2.AMCNT.To<int>();
                        nCNT2 = nCNT2 - item2.PMCNT.To<int>();
                    }
                }
                else
                {
                    HEA_RESV_EXAM item2 = heaResvExamService.GetCountAMPMbyRTime(dtpDate.Text, CF.DATE_ADD(clsDB.DbCon, dtpDate.Text, 1), strGubun);
                    if (!item2.IsNullOrEmpty())
                    {
                        nCNT1 = nCNT1 - item2.AMCNT.To<int>();
                        nCNT2 = nCNT2 - item2.PMCNT.To<int>();
                    }
                }

                //개인별 가예약
                HEA_RESV_SET item3 = heaResvSetService.GetSumGaInwonAMPMByGubun(dtpDate.Text, strGubun);
                if (!item3.IsNullOrEmpty())
                {
                    nCNT1 = nCNT1 - (int)item3.GAINWONAM;
                    nCNT2 = nCNT2 - (int)item3.GAINWONPM;
                }

                //회사별 가예약 인원수를 찾음
                HEA_RESV_LTD item4 = heaResvLtdService.GetSumAmPmJanByGubun(dtpDate.Text, strGubun);
                if (!item4.IsNullOrEmpty())
                {
                    nCNT1 = nCNT1 - (int)item4.AMJAN;
                    nCNT2 = nCNT2 - (int)item4.PMJAN;
                }

                SS2.ActiveSheet.Cells[i, 6].Text = nCNT1.To<string>();
                SS2.ActiveSheet.Cells[i, 7].Text = nCNT2.To<string>();

                //변동된 여유인원수 계산을 위해 보관함
                SS2.ActiveSheet.Cells[i, 12].Text = nCNT1.To<string>();
                SS2.ActiveSheet.Cells[i, 13].Text = nCNT2.To<string>();
            }
        }

        private void Delete_GaResv_Ltd(string argLtdCode)
        {
            int result = 0;

            if (argLtdCode.IsNullOrEmpty())
            {
                MessageBox.Show("회사코드가 공란입니다.", "확인");
                return;
            }

            string strMsg = "회사별 가예약 설정을" + ComNum.VBLF;
            strMsg = strMsg + "안하는 회사로 설정을 변경하고, 기존 가예약된 정보를" + ComNum.VBLF;
            strMsg = strMsg + "모두 삭제를 하시겠습니까?";

            if (ComFunc.MsgBoxQ(strMsg, "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);

            result = hicLtdCodeService.UpdateGaResv(argLtdCode.To<long>());
            if (result <= 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("종검 사업장 가예약 대상 해제시 오류발생!", "오류");
                return;
            }

            HEA_RESV_LTD_HIS item = new HEA_RESV_LTD_HIS
            {
                JOBSABUN = clsType.User.IdNumber.To<long>(),
                GBJOB = "X",
                LTDCODE = argLtdCode.To<long>(),
                ENTSABUN = clsType.User.IdNumber.To<long>()
            };

            result = heaResvLtdHisService.InsertData(item);
            if (result <= 0)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("종검 사업장 가예약 정보 변경시 오류발생!", "History 오류");
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            UpDate_GaResv_Ltd(argLtdCode);      //가예약 사업장 Data 정리

            MessageBox.Show("작업완료");

            List_Set();

            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            btnPrint.Enabled = false;
            btnDelete_All.Enabled = false;
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.RowHeader || e.ColumnHeader) { return; }

            if (sender == ssList)
            {
                txtLtdCode.Text = ssList.ActiveSheet.Cells[e.Row, 0].Text;
                lblLtdName.Text = ssList.ActiveSheet.Cells[e.Row, 1].Text;

                //가예약 사업장 작업
                Work_GaJepsu_Ltd(txtLtdCode.Text);
            }
            else if (sender == SS1)
            {
                dtpDate.Text = SS1.ActiveSheet.Cells[e.Row, 0].Text;
                DisPlay_Exam_GaResv_Ltd(txtLtdCode.Text);
            }
        }

        private void Work_GaJepsu_Ltd(string argLtdCode)
        {
            UpDate_GaResv_Ltd(argLtdCode);      //가예약 사업장 Data 정리
            DisPlay_GaResv_Ltd(argLtdCode);     //사업장 가예약 현황 
        }

        private void UpDate_GaResv_Ltd(string argLtdCode)
        {
            int result = 0;
            int nCNT1 = 0;
            int nCNT2 = 0;
            int nCNT3 = 0;
            int nCNT4 = 0;
            //string strLtdList = "";
            List<long> lstLtdList = new List<long>();
            string strLtdCode = "";
            string strSDate = "";
            string strExCode = "00";
            string strYoil = "";

            try
            {
                ComFunc.ReadSysDate(clsDB.DbCon);

                //(1) 가예약 대상 회사를 설정
                List<HIC_LTD> list1 = hicLtdCodeService.GetListGaResv();

                if (list1.Count > 0)
                {
                    for (int i = 0; i < list1.Count; i++)
                    {
                        lstLtdList.Add(list1[i].CODE);
                    }
                }

                if (lstLtdList.Count == 0)
                {
                    List_Set();
                    return;
                }

                clsDB.setBeginTran(clsDB.DbCon);

                //(2) 가예약회사가 아닌 정보를 삭제함
                result = heaResvLtdService.DelDataByNotResv(lstLtdList);
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    MessageBox.Show("가예약 대상 아닌 회사정보 삭제 시 오류발생", "오류");
                    return;
                }

                result = heaResvLtdService.DeleteByGubunIsNull();
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //(3) 접수인원 및 잔여인원을 Clear
                result = heaResvLtdService.UpDateInwonClear(argLtdCode.To<long>());
                if (result < 0)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return;
                }

                //(4) 종합건진 접수/예약인원을 업데이트
                List<HEA_JEPSU> list2 = heaJepsuService.GetJepsuCountAMPM(argLtdCode.To <long>());
                if (list2.Count > 0)
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        strLtdCode = list2[i].LTDCODE.To<string>();
                        strSDate = list2[i].SDATE.To<string>();
                        nCNT1 = list2[i].AMCNT.To<int>(0);      //오전
                        nCNT2 = list2[i].PMCNT.To<int>(0);      //오후

                        strExCode = "00";  //종검

                        switch (VB.Left(CF.READ_YOIL(clsDB.DbCon, strSDate), 1))
                        {
                            case "월": strYoil = "1"; break;
                            case "화": strYoil = "2"; break;
                            case "수": strYoil = "3"; break;
                            case "목": strYoil = "4"; break;
                            case "금": strYoil = "5"; break;
                            case "토": strYoil = "6"; break;
                            case "일": strYoil = "7"; break;
                            default:
                                break;
                        }

                        HEA_RESV_LTD item1 = heaResvLtdService.GetInwonAmPm(strSDate, argLtdCode.To<long>(), strExCode);

                        if (!item1.IsNullOrEmpty())
                        {
                            nCNT3 = (int)item1.AMINWON.To<int>(0) - nCNT1;
                            nCNT4 = (int)item1.PMINWON.To<int>(0) - nCNT2;

                            if (nCNT3 < 0) { nCNT3 = 0; }
                            if (nCNT4 < 0) { nCNT4 = 0; }

                            HEA_RESV_LTD item3 = new HEA_RESV_LTD();
                            item3.AMINWON = 999;
                            item3.PMINWON = 999;
                            if (item1.AMINWON.To<int>(0) < nCNT1) { item3.AMINWON = nCNT1; }
                            if (item1.PMINWON.To<int>(0) < nCNT2) { item3.PMINWON = nCNT2; }
                            item3.AMJEPSU = nCNT1;
                            item3.PMJEPSU = nCNT2;
                            item3.AMJAN = nCNT3;
                            item3.PMJAN = nCNT4;
                            item3.RID = item1.RID;

                            result = heaResvLtdService.UpDateInwon(item3);
                            if (result <= 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                        else
                        {
                            HEA_RESV_LTD item2 = new HEA_RESV_LTD
                            {
                                SDATE = strSDate,
                                GUBUN = strExCode,
                                LTDCODE = strLtdCode.To<long>(0),
                                AMINWON = nCNT1,
                                PMINWON = nCNT2,
                                AMJEPSU = nCNT1,
                                PMJEPSU = nCNT2,
                                AMJAN = 0,
                                PMJAN = 0,
                                YOIL = strYoil
                            };

                            result = heaResvLtdService.InsertData(item2);
                            if (result <= 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }

                    }
                }

                //(5) 선택검사 예약인원을 업데이트
                List<HEA_RESV_EXAM_PATIENT> list3 = heaResvExamPatientService.GetListByLtdCode(argLtdCode.To<long>());
                if (list3.Count > 0)
                {
                    for (int i = 0; i < list3.Count; i++)
                    {
                        strLtdCode = list3[i].LTDCODE.To<string>();
                        strSDate = VB.Left(list3[i].SDATE.To<string>(""), 10);
                        strExCode = list3[i].GBEXAM.To<string>("").Trim();
                        nCNT1 = (int)list3[i].AMCNT;
                        nCNT2 = (int)list3[i].PMCNT;

                        switch (VB.Left(CF.READ_YOIL(clsDB.DbCon, strSDate), 1))
                        {
                            case "월": strYoil = "1"; break;
                            case "화": strYoil = "2"; break;
                            case "수": strYoil = "3"; break;
                            case "목": strYoil = "4"; break;
                            case "금": strYoil = "5"; break;
                            case "토": strYoil = "6"; break;
                            case "일": strYoil = "7"; break;
                            default:
                                break;
                        }

                        HEA_RESV_LTD item4 = heaResvLtdService.GetInwonAmPm(strSDate, argLtdCode.To<long>(), strExCode);

                        if (!item4.IsNullOrEmpty())
                        {
                            nCNT3 = (int)item4.AMINWON.To<int>(0) - nCNT1;
                            nCNT4 = (int)item4.PMINWON.To<int>(0) - nCNT2;

                            if (nCNT3 < 0) { nCNT3 = 0; }
                            if (nCNT4 < 0) { nCNT4 = 0; }

                            HEA_RESV_LTD item3 = new HEA_RESV_LTD();
                            //인원수 999 세팅 -> IsNullOrEmpty 체크안되기 때문에 강제로 999세팅
                            item3.AMINWON = 999;
                            item3.PMINWON = 999;
                            if (item4.AMINWON < nCNT1) { item3.AMINWON = nCNT1; }
                            if (item4.PMINWON < nCNT2) { item3.PMINWON = nCNT2; }
                            item3.AMJEPSU = nCNT1;
                            item3.PMJEPSU = nCNT2;
                            item3.AMJAN = nCNT3;
                            item3.PMJAN = nCNT4;
                            item3.RID = item4.RID;

                            result = heaResvLtdService.UpDateInwon(item3);
                            if (result <= 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                        else
                        {
                            HEA_RESV_LTD item5 = new HEA_RESV_LTD
                            {
                                SDATE = strSDate,
                                GUBUN = strExCode,
                                LTDCODE = strLtdCode.To<long>(),
                                AMINWON = nCNT1,
                                PMINWON = nCNT2,
                                AMJEPSU = nCNT1,
                                PMJEPSU = nCNT2,
                                AMJAN = 0,
                                PMJAN = 0,
                                YOIL = strYoil
                            };

                            result = heaResvLtdService.InsertData(item5);
                            if (result <= 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void DisPlay_GaResv_Ltd(string argLtdCode)
        {
            int nRow = 0;
            string strNew = string.Empty;
            string strOLD = string.Empty;
            string strHEA = string.Empty;
            string strList = string.Empty;
            bool bJanInwon = false;

            grpLtdRemark.Visible = false;
            txtLtdRemark.Text = "";

            SS1.ActiveSheet.ClearRange(0, 0, SS1.ActiveSheet.Rows.Count, SS1.ActiveSheet.ColumnCount, true);
            SS1.ActiveSheet.Rows.Count = 0;

            Cursor.Current = Cursors.WaitCursor;

            //선택검사 목록을 읽음
            List<HEA_RESV_LTD> list1 = heaResvLtdService.GetListByLtdCode(argLtdCode.To<long>());

            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    strNew = list1[i].SDATE.To<string>("");
                    if (strOLD == "") { strOLD = strNew; }
                    if (strOLD != strNew)
                    {
                        //2015-04-09 가예약 잔여인원 표시
                        if (chkJanResv.Checked && bJanInwon == false) { strList = ""; }
                        if (strHEA != "" || strList != "")
                        {
                            nRow = nRow + 1;
                            if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }

                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = strOLD;
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = strHEA;
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = strList;

                            if (chkJanResv.Checked)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                if (bJanInwon)
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.Yellow;
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.White;
                                }   
                            }
                        }

                        strOLD = strNew;
                        strList = ""; strHEA = "";
                        bJanInwon = false;
                    }

                    if (list1[i].GUBUN.To<string>("").Trim() == "00")    //종합검진
                    {
                        if (list1[i].AMJAN > 0 || list1[i].PMJAN > 0 ) { bJanInwon = true; }
                        if (chkJanResv.Checked)
                        {
                            if (bJanInwon)
                            {
                                strHEA = list1[i].AMJAN.To<string>("0") + "/";
                                strHEA = strHEA + list1[i].PMJAN.To<string>("0");
                            }
                            else
                            {
                                strHEA = "";
                            }
                        }
                        else
                        {
                            strHEA = list1[i].AMINWON.To<string>("0") + "/";
                            strHEA = strHEA + list1[i].PMINWON.To<string>("0");
                        }
                    }
                    else
                    {
                        if (list1[i].AMINWON > 0 || list1[i].PMINWON > 0)
                        {
                            if (list1[i].AMJAN > 0 || list1[i].PMJAN > 0) { bJanInwon = true; }
                            if (chkJanResv.Checked)
                            {
                                if (list1[i].AMJAN > 0 || list1[i].PMJAN > 0)
                                {
                                    strList = strList + heaCodeService.GetNameByGubunGubun2("13", list1[i].GUBUN) + "(";
                                }
                                strList = strList + list1[i].AMJAN.To<string>("0") + "/";
                                strList = strList + list1[i].PMJAN.To<string>("0") + "),";
                            }
                            else
                            {
                                strList = strList + heaCodeService.GetNameByGubunGubun2("13", list1[i].GUBUN) + "(";
                                strList = strList + list1[i].AMINWON.To<string>("0") + "/";
                                strList = strList + list1[i].PMINWON.To<string>("0") + "),";
                            }

                        }
                    }
                }
            }

            //2015-04-09 가예약 잔여인원 표시
            if (chkJanResv.Checked && bJanInwon == false) { strList = ""; }

            if (strHEA != "" || strList != "")
            {
                nRow = nRow + 1;
                if (nRow > SS1.ActiveSheet.RowCount) { SS1.ActiveSheet.RowCount = nRow; }
                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = strOLD;
                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = strHEA;
                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = strList;

                if (chkJanResv.Checked)
                {
                    SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.White;
                }
                else
                {
                    if (bJanInwon)
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.Yellow;
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 0].BackColor = System.Drawing.Color.White;
                    }
                }
            }

            SS1.ActiveSheet.RowCount = nRow;
            btnPrint.Enabled = true;
            btnDelete_All.Enabled = true;

            Cursor.Current = Cursors.Default;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            txtLtdCode.Text = "";
            lblLtdName.Text = "";
            
            dtpDate.Text = DateTime.Now.ToShortDateString();
            btnPrint.Enabled = false;
            btnDelete_All.Enabled = false;

            grpLtdRemark.Visible = false;
            txtLtdRemark.Text = "";

            List_Set();
        }

        private void List_Set()
        {
            ssList.DataSource = hicLtdCodeService.GetListGaResv();
        }
    }
}
