using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongSeniorOPPList.cs
    /// Description     : 경로.장애 우대 및 원외처방건수 통계
    /// Author          : 안정수
    /// Create Date     : 2017-09-08 
    /// Update History  : 2017-11-03
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm경로원외처방건수.frm(Frm경로원외처방건수) => frmPmpaTongSeniorOPPList.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm경로원외처방건수.frm(Frm경로원외처방건수)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongSeniorOPPList : Form
    {
        ComFunc CF = new ComFunc();
        public frmPmpaTongSeniorOPPList()
        {
            InitializeComponent();
            SetEvent();
        }
             
        void SetEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnPrint.Click += new EventHandler(eBtnEvent);
            this.btnCancel.Click += new EventHandler(eBtnEvent);

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

            optBun0.Checked = true;
            CF.ComboMonth_Set(cboYYMM, 12);
            
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
            
            else if (sender == this.btnCancel)
            {
                ssList_Sheet1.Rows.Count = 0;
                ssList_Sheet1.Columns.Count = 30;
            }

            else if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (optBun0.Checked == true)
            {
                strTitle = "경로,장애 건수";
            }
            else
            {
                strTitle = "원외처방건수(포항시제외)";
            }

            strSubTitle = "조회일자 : " + cboYYMM.SelectedItem.ToString();

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 12, FontStyle.Bold), clsSpread.enmSpdHAlign.Right, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += SPR.setSpdPrint_String("출력일자:" + VB.Now().ToString() + VB.Space(3) + "PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 80, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, true, false, false, (float)0.8);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int Cnt = 0;
            int nCnt = 0;
            int nRead = 0;

            string strPartName = "";
            string strTDate = "";
            string strFDate = "";
            string strDD = "";

            string strMM_New = "";
            string strMM_Old = "";

            string Temp = "";

            double nGaSum = 0;
            double nSeSum = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            ssList_Sheet1.Rows.Count = 0;
            ssList_Sheet1.Columns.Count = 1;

            Cnt = 1;

            if(optBun0.Checked == true)
            {
                strFDate = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + "-" + VB.Mid(cboYYMM.SelectedItem.ToString().Trim(), 7, 2) + "-01";
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                            ";
                for(i = 1; i <= Convert.ToInt32(VB.Right(strTDate, 2)); i++)
                {
                    SQL += ComNum.VBLF + "  SUM(DECODE(TO_CHAR(ACTDATE,'DD'),'" + ComFunc.SetAutoZero(i.ToString(), 2) + "', 1)) ACT" + 
                                                                                    ComFunc.SetAutoZero(i.ToString(), 2) + ",           ";
                    nCnt += 1;
                    ssList_Sheet1.Columns.Count += 1;
                    ssList_Sheet1.ColumnHeader.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = i.ToString();
                }
                SQL += ComNum.VBLF + "PART                                                                                              ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                                                             ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                         ";
                SQL += ComNum.VBLF + "      AND RESERVED <> '1'                                                                         ";
                SQL += ComNum.VBLF + "      AND (AGE >= 65 OR BOHUN = '3')                                                              ";
                SQL += ComNum.VBLF + "      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                     ";
                SQL += ComNum.VBLF + "      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                                     ";
                SQL += ComNum.VBLF + "      AND PART NOT IN ('111','222','333','2222')                                                  ";
                SQL += ComNum.VBLF + "GROUP BY PART                                                                                     ";
            }
            else if(optBun1.Checked == true)
            {
                strFDate = VB.Left(cboYYMM.SelectedItem.ToString().Trim(), 4) + "-" + VB.Mid(cboYYMM.SelectedItem.ToString(), 7, 2) + "-01";
                strTDate = CF.READ_LASTDAY(clsDB.DbCon, strFDate);

                SQL = "";
                SQL += ComNum.VBLF + "SELECT                                                                                                                ";
                for (i = 1; i <= Convert.ToInt32(VB.Right(strTDate, 2)); i++)
                {
                    SQL += ComNum.VBLF + "  SUM(DECODE(TO_CHAR(A.ACTDATE,'DD'),'" + ComFunc.SetAutoZero(i.ToString(), 2) + "', 1)) ACT" +
                                                                                    ComFunc.SetAutoZero(i.ToString(), 2) + ",                               ";
                    nCnt += 1;
                    ssList_Sheet1.Columns.Count += 1;
                    ssList_Sheet1.ColumnHeader.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = i.ToString();
                }
                SQL += ComNum.VBLF + "X.PART                                                                                                                ";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER A, (                                                                            ";
                SQL += ComNum.VBLF + "                                          SELECT A.PANO, TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, A.DEPTCODE,A.PART   ";
                SQL += ComNum.VBLF + "                                              From OPD_SLIP A, (SELECT PANO,DEPTCODE FROM OPD_SLIP                    ";
                SQL += ComNum.VBLF + "                                                  WHERE 1=1                                                           ";
                SQL += ComNum.VBLF + "                                                      AND ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')         ";
                SQL += ComNum.VBLF + "                                                      AND ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')         ";
                SQL += ComNum.VBLF + "                                                      AND BUN >= '03' AND BUN <= '85'                                 ";
                SQL += ComNum.VBLF + "                                                      AND SUCODE NOT IN ('##14', '##15')                              ";
                SQL += ComNum.VBLF + "                                                      AND BUN NOT IN('26','15')                                       ";
                SQL += ComNum.VBLF + "                                                      AND PART <> '#'                                                 ";
                SQL += ComNum.VBLF + "                                                      AND GBBUNUP <> '$'                                              ";
                SQL += ComNum.VBLF + "                                                  GROUP BY PANO,DEPTCODE                                              ";
                SQL += ComNum.VBLF + "                                                  HAVING SUM(AMT1) = 0) Y                                             ";
                SQL += ComNum.VBLF + "                                              WHERE 1=1                                                               ";
                SQL += ComNum.VBLF + "                                                  AND A.PANO = Y.PANO(+)                                              ";
                SQL += ComNum.VBLF + "                                                  AND A.DEPTCODE = Y.DEPTCODE                                         ";
                SQL += ComNum.VBLF + "                                                  AND A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')           ";
                SQL += ComNum.VBLF + "                                                  AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')           ";
                SQL += ComNum.VBLF + "                                              GROUP BY A.PANO, TO_CHAR(A.ACTDATE,'YYYY-MM-DD'),A.DEPTCODE, A.PART) X  ";
                SQL += ComNum.VBLF + "WHERE 1=1                                                                                                             ";
                SQL += ComNum.VBLF + "      AND A.PANO = X.PANO(+)                                                                                          ";
                SQL += ComNum.VBLF + "      AND TO_CHAR(A.ACTDATE,'YYYY-MM-DD') = X.ACTDATE                                                                 ";
                SQL += ComNum.VBLF + "      AND A.DEPTCODE = X.DEPTCODE                                                                                     ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE >= TO_DATE('" + strFDate + "','YYYY-MM-DD')                                                       ";
                SQL += ComNum.VBLF + "      AND A.ACTDATE <= TO_DATE('" + strTDate + "','YYYY-MM-DD')                                                       ";
                SQL += ComNum.VBLF + "      AND A.JICODE >= '65'                                                                                            ";
                SQL += ComNum.VBLF + "GROUP BY x.PART                                                                                                       ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nRead;
                    ssList_Sheet1.Columns.Count += 1;
                    ssList_Sheet1.ColumnHeader.Cells[0, ssList_Sheet1.Columns.Count - 1].Text = "합계";

                    for(i = 0; i < nRead; i++)
                    {
                        //PART에 #이 들어가는 경우를 방지하기 위함 -> 2017-09-08 안정수 추가
                        if (dt.Rows[i]["PART"].ToString().Trim().Length <= 2)
                        {
                            ssList_Sheet1.Cells[i, 0].Text = "";
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 0].Text = CF.READ_PassName(clsDB.DbCon, dt.Rows[i]["PART"].ToString().Trim());
                        }
                        
                        for(j = 1; j < ssList_Sheet1.Columns.Count-1; j++)
                        {
                            strDD = ComFunc.SetAutoZero(ssList_Sheet1.ColumnHeader.Cells[0, j].Text, 2);
                            ssList_Sheet1.Cells[i, j].Text = (dt.Rows[i]["ACT" + strDD].ToString().Trim() == "0" ? "" : dt.Rows[i]["ACT" + strDD].ToString().Trim());
                            nGaSum += VB.Val(dt.Rows[i]["ACT" + strDD].ToString().Trim());
                        }
                        ssList_Sheet1.Cells[i, ssList_Sheet1.ColumnCount - 1].Text = String.Format("{0:###,###}", nGaSum);
                        nGaSum = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            ssList_Sheet1.Rows.Count += 1;

            ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, 0].Text = "합계";

            //세로 합계
            for(i = 1; i < ssList_Sheet1.Columns.Count; i++)
            {
                for(j = 0; j < ssList_Sheet1.Rows.Count; j++)
                {
                    nSeSum += ssList_Sheet1.Cells[j, i].Text == "" ? 0 : VB.Val(ssList_Sheet1.Cells[j, i].Text);
                }
                ssList_Sheet1.Cells[ssList_Sheet1.Rows.Count - 1, i].Text = String.Format("{0:###,###}", nSeSum);
                nSeSum = 0;
            }


        }
    }
}
