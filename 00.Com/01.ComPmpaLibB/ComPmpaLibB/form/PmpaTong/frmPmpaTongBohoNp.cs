using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongBohoNp.cs
    /// Description     : 보호정신과 진료내역 통계
    /// Author          : 안정수
    /// Create Date     : 2017-08-16
    /// Update History  : 
    /// <history>       
    /// 본 소스(VB)에서도 실행시 쿼리오류가 뜸, ex) 열의 정의가 애매합니다.... 쿼리확인 필요
    /// d:\psmh\OPD\oviewa\OVIEWA31.FRM(FrmBohoNpTong) => frmPmpaTongBohoNp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA31.FRM(FrmBohoNpTong)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongBohoNp : Form
    {
        int[,] nTotAmt = new int[3, 3];
        public frmPmpaTongBohoNp()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등  

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            optGbn0.Checked = true;

            string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
            dtpFDate.Text = Convert.ToDateTime(CurrentDate).AddDays(-5).ToShortDateString();
            dtpTDate.Text = CurrentDate;            
        }
       
        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                eGetData();
            }

            else if (sender == this.btnPrint)
            {
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strTitle2 = "";

            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;


            #region //시트 히든

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = false;


            #endregion

            string strPrintName = clsPrint.gGetDefaultPrinter();//기본프린터 이름을 가져온다

            if (strPrintName != "")
            {
                strTitle = "보호 정신과 진료내역 통계";
                strTitle2 = "작업기간: " + dtpFDate.Text + " => " + dtpTDate.Text;

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strTitle2, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter, strPrintName);
            }

            #region //시트 히든 복원

            //ssList.ActiveSheet.Columns[(int)clsSupFnExSpd.enmSupFnExMain.Infect].Visible = true;
            #endregion
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;
            int nRow = 0;

            string strOldData = "";
            string strNewData = "";
            string strNewOK = "";

            string strDrCode = "";
            string strPano = "";
            string strBDate = "";
            string strSuNext = "";

            double nQty = 0;
            double nAmt = 0;

            int nNal = 0;
            int nMaxNal = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            dtpFDate.Enabled = false;
            dtpTDate.Enabled = false;
            optGbn0.Enabled = false;
            optGbn1.Enabled = false;
            btnView.Enabled = false;
            btnPrint.Enabled = false;

            //누적항목 배열을 Clear
            for(i = 0; i < 3; i++)
            {
                for(j = 0; j < 3; j++)
                {
                    nTotAmt[i, j] = 0;
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  S.DrCode, TO_CHAR(S.BDate, 'YYYY-MM-DD') BDate,                                     ";
            SQL += ComNum.VBLF + "  S.Pano, S.SuNext, S.Qty, SUM(S.Nal) Nal,                                            ";
            SQL += ComNum.VBLF + "  MAX(DECODE(B.DRNAME, NULL, 'ERROR', B.DRNAME)) DRNAME,                              ";
            SQL += ComNum.VBLF + "  MAX(P.SNAME) SNAME, MAX(BAMT) BAMT                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP S, " + ComNum.DB_PMPA + "BAS_DOCTOR B,            ";
            SQL += ComNum.VBLF +           ComNum.DB_PMPA + "BAS_PATIENT P,                                             ";
            SQL += ComNum.VBLF + "                           (SELECT SUNEXT, BAMT                                       ";
            SQL += ComNum.VBLF + "                              FROM (SELECT SUNEXT, BAMT FROM BAS_SUH                  ";
            SQL += ComNum.VBLF + "                                     UNION ALL                                        ";
            SQL += ComNum.VBLF + "                            SELECT SUNEXT, BAMT FROM BAS_SUT)                         ";
            SQL += ComNum.VBLF + "                              GROUP BY SUNEXT, BAMT) U                                ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "      AND S.ActDate >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "      AND S.BDate   >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "      AND S.BDate   <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')                  ";
            SQL += ComNum.VBLF + "      AND S.Bi IN ('21','22')                                                         ";
            SQL += ComNum.VBLF + "      AND S.DeptCode = 'NP'                                                           ";
            SQL += ComNum.VBLF + "      AND S.Bun >= '11'                                                               ";
            SQL += ComNum.VBLF + "      AND S.GbSelf = '0'                                                              ";
            SQL += ComNum.VBLF + "      AND S.DRCODE = B.DRCODE(+)                                                      ";
            SQL += ComNum.VBLF + "      AND S.PANO  = P.PANO                                                            ";
            SQL += ComNum.VBLF + "      AND S.SUNEXT = U.SUNEXT(+)                                                      ";
            SQL += ComNum.VBLF + "GROUP BY S.DrCode, S.BDate, S.Pano, S.SuNext, S.Qty                                   ";
            SQL += ComNum.VBLF + "ORDER BY S.DrCode, S.BDate, S.Pano, S.SuNext, S.Qty                                   ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    dtpFDate.Enabled = true;
                    dtpTDate.Enabled = true;
                    btnView.Enabled = true;
                    btnPrint.Enabled = true;

                    ComFunc.MsgBox("해당 조건의 데이터는 없습니다!!!");
                    return;
                }

                nREAD = dt.Rows.Count;

                strOldData = "";
                strNewOK = "";
                nMaxNal = 0;

                for(i = 0; i < nREAD; i++)
                {
                    strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                    strBDate = dt.Rows[i]["BDate"].ToString().Trim();
                    strPano = dt.Rows[i]["Pano"].ToString().Trim();
                    strSuNext = dt.Rows[i]["SuNext"].ToString().Trim();
                    nQty = Convert.ToInt32(dt.Rows[i]["Qty"].ToString().Trim());
                    nNal = Convert.ToInt32(dt.Rows[i]["Nal"].ToString().Trim());

                    if(nNal > 0 && nQty > 0)
                    {
                        strNewData = VB.Left(strDrCode + "0000", 4) + strBDate + strPano;

                        if(strOldData == "")
                        {
                            strOldData = strNewData;
                        }

                        if(VB.Left(strNewData, 4) != VB.Left(strOldData, 4))
                        {
                            #region Pano_Total_Rtn(GoSub)

                            nRow += 1;
                            if(nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow + 10;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 4].Text = "개인소계";
                            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[1, 1]);
                            nTotAmt[1, 2] = nMaxNal * 970;

                            if(optGbn0.Checked == true)
                            {
                                nTotAmt[1, 2] += 10050 - 970;
                            }
                            nTotAmt[1, 3] = nTotAmt[1, 2] - nTotAmt[1, 1];
                            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[1, 2]);
                            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[1, 3]);

                            nTotAmt[2, 2] += nTotAmt[1, 2];
                            nTotAmt[3, 2] += nTotAmt[1, 2];

                            nTotAmt[1, 1] = 0;
                            nTotAmt[1, 2] = 0;
                            nTotAmt[1, 3] = 0;

                            #endregion Pano_Total_Rtn(GoSub) End

                            #region Doctor_Total_Rtn(GoSub)

                            nRow += 1;
                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow + 10;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 4].Text = "의사소계";
                            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[2, 1]);
                            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[2, 2]);
                            nTotAmt[2, 3] = nTotAmt[2, 2] - nTotAmt[2, 1];

                            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[2, 3]);

                            nTotAmt[2, 1] = 0;
                            nTotAmt[2, 2] = 0;
                            nTotAmt[2, 3] = 0;

                            #endregion Doctor_Total_Rtn(GoSub) End

                            strOldData = strNewData;
                            strNewOK = "OK";
                        }
                        else if(strOldData != strNewData)
                        {
                            #region Pano_Total_Rtn(GoSub)

                            nRow += 1;
                            if (nRow > ssList_Sheet1.Rows.Count)
                            {
                                ssList_Sheet1.Rows.Count = nRow + 10;
                            }

                            ssList_Sheet1.Cells[nRow - 1, 4].Text = "개인소계";
                            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[1, 1]);
                            nTotAmt[1, 2] = nMaxNal * 970;

                            if (optGbn0.Checked == true)
                            {
                                nTotAmt[1, 2] += 10050 - 970;
                            }
                            nTotAmt[1, 3] = nTotAmt[1, 2] - nTotAmt[1, 1];
                            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[1, 2]);
                            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[1, 3]);

                            nTotAmt[2, 2] += nTotAmt[1, 2];
                            nTotAmt[3, 2] += nTotAmt[1, 2];

                            nTotAmt[1, 1] = 0;
                            nTotAmt[1, 2] = 0;
                            nTotAmt[1, 3] = 0;

                            #endregion Pano_Total_Rtn(GoSub) End

                            strOldData = strNewData;
                            strNewOK = "OK";
                        }

                        #region Display_Rtn(GoSub)
                        nRow += 1;
                        if(nRow > ssList_Sheet1.Rows.Count)
                        {
                            ssList_Sheet1.Rows.Count = nRow + 10;
                        }

                        if(strNewOK == "OK")
                        {
                            ssList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["DrName"].ToString().Trim();
                            ssList_Sheet1.Cells[nRow - 1, 1].Text = strBDate;
                            ssList_Sheet1.Cells[nRow - 1, 2].Text = strPano;
                            ssList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Sname"].ToString().Trim();

                            strNewOK = "";
                        }

                        ssList_Sheet1.Cells[nRow - 1, 4].Text = " " + strSuNext;
                        ssList_Sheet1.Cells[nRow - 1, 5].Text = String.Format("{0:#,###}", nQty);
                        ssList_Sheet1.Cells[nRow - 1, 6].Text = nNal.ToString();

                        nAmt = Convert.ToInt32(dt.Rows[i]["BAmt"].ToString().Trim()) * nQty * nNal;

                        ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nAmt);

                        nTotAmt[1, 1] += Convert.ToInt32(nAmt);
                        nTotAmt[2, 1] += Convert.ToInt32(nAmt);
                        nTotAmt[3, 1] += Convert.ToInt32(nAmt);

                        if(nMaxNal < nNal)
                        {
                            nMaxNal = nNal;
                        }

                        #endregion Display_Rtn(GoSub) End
                    }
                }
            }

            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            #region Pano_Total_Rtn(GoSub)

            nRow += 1;
            if (nRow > ssList_Sheet1.Rows.Count)
            {
                ssList_Sheet1.Rows.Count = nRow + 10;
            }

            ssList_Sheet1.Cells[nRow - 1, 4].Text = "개인소계";
            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[1, 1]);
            nTotAmt[1, 2] = nMaxNal * 970;

            if (optGbn0.Checked == true)
            {
                nTotAmt[1, 2] += 10050 - 970;
            }
            nTotAmt[1, 3] = nTotAmt[1, 2] - nTotAmt[1, 1];
            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[1, 2]);
            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[1, 3]);

            nTotAmt[2, 2] += nTotAmt[1, 2];
            nTotAmt[3, 2] += nTotAmt[1, 2];

            nTotAmt[1, 1] = 0;
            nTotAmt[1, 2] = 0;
            nTotAmt[1, 3] = 0;

            #endregion Pano_Total_Rtn(GoSub) End

            #region Doctor_Total_Rtn(GoSub)

            nRow += 1;
            if (nRow > ssList_Sheet1.Rows.Count)
            {
                ssList_Sheet1.Rows.Count = nRow + 10;
            }

            ssList_Sheet1.Cells[nRow - 1, 4].Text = "의사소계";
            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[2, 1]);
            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[2, 2]);
            nTotAmt[2, 3] = nTotAmt[2, 2] - nTotAmt[2, 1];

            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[2, 3]);

            nTotAmt[2, 1] = 0;
            nTotAmt[2, 2] = 0;
            nTotAmt[2, 3] = 0;

            #endregion Doctor_Total_Rtn(GoSub) End

            #region Total_Display(GoSub)

            nRow += 1;
            ssList_Sheet1.Rows.Count = nRow;

            ssList_Sheet1.Cells[nRow - 1, 4].Text = "전체합계";
            ssList_Sheet1.Cells[nRow - 1, 7].Text = String.Format("{0:#,###}", nTotAmt[3, 1]);
            ssList_Sheet1.Cells[nRow - 1, 8].Text = String.Format("{0:#,###}", nTotAmt[3, 2]);
            nTotAmt[3, 3] = nTotAmt[3, 2] - nTotAmt[3, 1];
            ssList_Sheet1.Cells[nRow - 1, 9].Text = String.Format("{0:#,###}", nTotAmt[3, 3]);

            #endregion Total_Display(GoSub) End

            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;
            btnView.Enabled = true;
            btnPrint.Enabled = true;
        }

        void SCREEN_CLEAR()
        {
            dtpFDate.Enabled = true;
            dtpTDate.Enabled = true;
            btnView.Enabled = true;
            btnPrint.Enabled = true;

            ssList_Sheet1.Rows.Count = 0;
        }

    }
}
