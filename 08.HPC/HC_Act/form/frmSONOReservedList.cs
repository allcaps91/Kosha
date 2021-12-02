using ComBase;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;
using FarPoint.Win.Spread;
using System.Drawing;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmSONOReservedList.cs
/// Description     : SONO 예약자 명단
/// Author          : 이상훈
/// Create Date     : 2019-08-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "FrmSONO명단.frm(FrmSONO명단)" />

namespace HC_Act
{
    public partial class frmSONOReservedList : Form
    {
        HeaJepsuService heaJepsuService = null;
        HicResultService hicResultService = null;
        HeaResvExamService heaResvExamService = null;
        HeaResultService heaResultService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmSONOReservedList()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            heaJepsuService = new HeaJepsuService();
            hicResultService = new HicResultService();
            heaResvExamService = new HeaResvExamService();
            heaResultService = new HeaResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();
        }

        void fn_Form_Load()
        {
            clsCompuInfo.SetComputerInfo();
            ComFunc.ReadSysDate(clsDB.DbCon);
            dtpSDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate);
            sp.Spread_All_Clear(ssList);

            ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
            eBtnClick(btnSearch, new EventArgs());
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                //TX22 UGI 상부소화기조영
                //TX27 복부초음파
                //TX99 갑상선초음파
                //TX83 유방초음파
                //ZE17 전립선 초음파

                int nREAD = 0;
                int nRead2 = 0;
                int nRow = 0;
                
                List<string> lstInstr = new List<string>
                {
                    "TX22",
                    "TX27",
                    "TX99",
                    "TX83",
                    "ZE17"
                };

                nRow = 0;

                ssList.ActiveSheet.RowCount = 0;

                List<HEA_JEPSU> list = heaJepsuService.GetWrtNobySDate(dtpSDate.Text);

                if (list.Count > 0)
                {
                    nREAD = list.Count;
                    
                    for (int i = 0; i < nREAD; i++)
                    {
                        List<HEA_RESULT> list2 = heaResultService.GetExCodebyWrtNo_All(list[i].WRTNO, lstInstr);
                        
                        if (list2.Count > 0)
                        {
                            nRead2 = list2.Count;
                            nRow += 1;
                            if (ssList.ActiveSheet.RowCount < nRow)
                            {
                                ssList.ActiveSheet.RowCount = nRow;
                            }

                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].PTNO;
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME.Trim();
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].AMPM2.Trim() == "1" ? "오전" : "오후";

                            for (int j = 0; j < nRead2; j++)
                            {
                                switch (list2[j].EXCODE.Trim())
                                {
                                    case "TX22":
                                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = "◎";
                                        break;
                                    case "TX27":
                                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = "◎";
                                        break;
                                    case "TX99":
                                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = "◎";
                                        break;
                                    case "TX83":
                                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = "◎";   //유방
                                        if (list[i].AMPM2 == "1")
                                        {
                                            HEA_RESV_EXAM list3 = heaResvExamService.GetCountbyPaNo(list[i].PANO, "TX83");
                                            if (list3 != null)
                                            {
                                                if (list3.AMPM.Trim() == "P")
                                                {
                                                    ssList.ActiveSheet.Cells[nRow - 1, 6].BackColor = Color.FromArgb(255, 215, 235);
                                                }
                                            }
                                        }
                                        break;
                                    case "ZE17":
                                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = "◎";
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "SONO 예약자 명단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);

                ///TODO : 이상훈(2019.08.26) 출력 Log 여부 확인
                //SQL_LOG("", ssList.PrintHeader);
            }
        }
    }
}
