using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaTongAutoSunap.cs
    /// Description     : 무인수납 세부 건수 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-11
    /// Update History  : 2017-11-04
    /// <history>       
    /// d:\psmh\OPD\olrepa\Frm무인수납통계.frm(Frm무인수납통계) => frmPmpaTongAutoSunap.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\olrepa\Frm무인수납통계.frm(Frm무인수납통계)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaTongAutoSunap : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        
        string CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

        string[] Fstrpart2 = new string[8];
        string[] FstrAge2 = new string[11];
        string[] FstrSex2 = new string[11];
        string[] FstrDept2 = new string[101];
        string[] FstrTime2 = new string[12];
        string FstrPartNo = "";
        string FstrDeptNo = "";

        public frmPmpaTongAutoSunap()
        {
            InitializeComponent();
            setEvent();
        }
       
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtPart.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.chkAll.CheckedChanged += new EventHandler(eBtnEvent);

        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {   
            if (sender == this.dtpFDate)
            {
                dtpTDate.Focus();
            }

            else if (sender == this.dtpTDate)
            {
                btnView.Focus();
            }

            else if (sender == this.txtPart)
            {
                if (e.KeyChar == 13)
                {
                    btnView.Focus();
                }
            }
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
            chkAll.Checked = true;

            Set_Init();
            txtPart.Focus();
        }

        void Set_Init()
        {
            txtCnt.Text = "";

            FstrPartNo = "5001,1;5002,2;5003,3;5004,4;5005,5;5006,6;5050,7;";

            Fstrpart2[1] = "5001 치과";
            Fstrpart2[2] = "5002 본관1층";
            Fstrpart2[3] = "5003 본관2층";
            Fstrpart2[4] = "5004 본관3층";
            Fstrpart2[5] = "5005 정형외과앞";
            Fstrpart2[6] = "5006 2층안과앞";
            Fstrpart2[7] = "5050 모바일";

            FstrAge2[1] = "0-9세";
            FstrAge2[2] = "10-19세";
            FstrAge2[3] = "20-29세";
            FstrAge2[4] = "30-39세";
            FstrAge2[5] = "40-49세";
            FstrAge2[6] = "50-59세";
            FstrAge2[7] = "60-69세";
            FstrAge2[8] = "70-79세";
            FstrAge2[9] = "80-89세";
            FstrAge2[10] = "90이상";

            FstrTime2[1] = "~08:59";
            FstrTime2[2] = "09:00~09:59";
            FstrTime2[3] = "10:00~10:59";
            FstrTime2[4] = "11:00~11:59";
            FstrTime2[5] = "12:00~12:59";
            FstrTime2[6] = "13:00~13:59";
            FstrTime2[7] = "14:00~14:59";
            FstrTime2[8] = "15:00~15:59";
            FstrTime2[9] = "16:00~16:59";
            FstrTime2[10] = "17:00~";

            FstrSex2[1] = "남";
            FstrSex2[2] = "여";
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

            else if (sender == this.chkAll)
            {
                if(chkAll.Checked == true)
                {
                    label1.Visible = false;
                    txtPart.Visible = false;
                }

                else
                {
                    label1.Visible = true;
                    txtPart.Visible = true;
                }
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nX = 0;
            int nY = 0;
            int nDeptCnt = 0;
            int nAge = 0;
            int nRow = 0;
            int nRead = 0;

            double nTot2 = 0;
            double nTot3 = 0;

            int nDayCnt = 0;

            string strJumin1 = "";
            string strJumin2 = "";
            string strDept = "";

            string strNew = "";
            string strOld = "";

            int[] nCnt1 = new int[8];               //조별건수
            int[,] nCnt2 = new int[8, 101];         //수납연령건수
            int[,] nCnt3 = new int[8, 101];         //과별건수

            double[] nCnt4 = new double[8];         //조별금액
            int[] nCnt5 = new int[1001];            //일자별건수

            string[] strCnt5 = new string[1001];    //일자저장
            int[,] nCnt6 = new int[8, 101];         //시간대별건수
            int[,] nCnt7 = new int[8, 101];         //성별

            double nCarAmt = 0;

            string strSTime;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < 1001; i++)
            {
                nCnt5[i] = 0;
                strCnt5[i] = "";
            }


            for(i = 0; i < 8; i++)
            {
                nCnt1[i] = 0;
                nCnt4[i] = 0;

                for(j = 0; j < 101; j++)
                {
                    nCnt2[i, j] = 0;
                    nCnt3[i, j] = 0;
                    nCnt6[i, j] = 0;
                    nCnt7[i, j] = 0;

                    FstrDept2[j] = "";
                }
            }

            CS.Spread_All_Clear(ssList1);

            //과 리스트 체크
            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  a.DeptCode                                                                  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP A, " + ComNum.DB_PMPA + "BAS_PATIENT B   ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                    ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')          ";
            SQL += ComNum.VBLF + "      AND a.PANO  <> '81000004'                                               ";

            if(chkAll.Checked == true)
            {
                SQL += ComNum.VBLF + "      AND a.PART IN ('5001','5002','5003','5004','5005','5006','5050' )   ";
                if(txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND PART  = '" + txtPart.Text.Trim() + "'                           ";
                }
            }
            else
            {
                if(txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "  AND PART  = '" + txtPart.Text.Trim() + "'                           ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND a.PART NOT IN ('4349','7777','7778' )                           ";
                }
            }

            SQL += ComNum.VBLF + "      AND (a.DELDATE IS NULL OR a.DELDATE ='')                                ";
            SQL += ComNum.VBLF + "GROUP BY a.DeptCode                                                           ";

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

                FstrDeptNo = "";

                if (dt.Rows.Count > 0)
                {
                    nRead = dt.Rows.Count;
                    nDeptCnt = nRead;

                    for (i = 0; i < nRead; i++)
                    {
                        strDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        FstrDeptNo += strDept + "," + (i + 1) + ";";
                        FstrDept2[i + 1] = CF.READ_DEPTNAMEK(clsDB.DbCon,strDept) + " " + strDept;
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

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                ";
            SQL += ComNum.VBLF + "  a.Part, A.PANO, B.SNAME,b.Sex, AMT, STIME,A.REMARK, A.BIGO,TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, ";
            SQL += ComNum.VBLF + "  A.AMT1, A.AMT2, A.AMT3, A.AMT4, A.AMT5, A.DEPTCODE,b.Jumin1,b.Jumin2,b.Jumin3,                      ";
            SQL += ComNum.VBLF + "  A.BI, A.SEQNO2, A.CARDGB,a.CardAmt, A.YAKAMT,a.WorkGbn,a.GbSPC,A.ROWID                              ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP A, " + ComNum.DB_PMPA + "BAS_PATIENT B                           ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                             ";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)                                                                            ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE >= TO_DATE('" + dtpFDate.Text + "','YYYY-MM-DD')                                  ";
            SQL += ComNum.VBLF + "      AND a.ACTDATE <= TO_DATE('" + dtpTDate.Text + "','YYYY-MM-DD')                                  ";
            SQL += ComNum.VBLF + "      AND a.PANO  <> '81000004'                                                                       ";
            if(chkAll.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND a.PART IN ('5001','5002','5003','5004','5005','5006','5050' )                               ";
            }
            else
            {
                if(txtPart.Text != "")
                {
                    SQL += ComNum.VBLF + "AND PART  = '" + txtPart.Text.Trim() + "'                                                     ";
                }
                else
                {
                    SQL += ComNum.VBLF + "AND a.PART NOT IN ('4349','7777','7778' )                                                     ";
                }
            }
            
            SQL += ComNum.VBLF + "      AND (a.DELDATE IS NULL OR a.DELDATE ='')                                                        ";
            SQL += ComNum.VBLF + "ORDER BY a.ActDate,a.STIme                                                                            ";

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
                ssList1_Sheet1.Rows.Count = nRead;

                txtCnt.Text = nRead + "건수";

                nTot2 = 0;
                nTot3 = 0;
                nCarAmt = 0;

                for(i = 0; i < nRead; i++)
                {
                    ssList1.ActiveSheet.Rows[i].Height = 20;

                    strNew = dt.Rows[i]["ActDate"].ToString().Trim();

                    if(strNew != strOld)
                    {
                        nDayCnt += 1;
                        strCnt5[nDayCnt] = strNew;
                    }

                    nCnt5[nDayCnt] += 1;

                    ssList1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Part"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 3].Text = String.Format("{0:###,###,##0}", VB.Val(dt.Rows[i]["AMT"].ToString().Trim()));
                    ssList1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["STIME"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 5].Text = " " + dt.Rows[i]["REMARK"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["BI"].ToString().Trim();
                    ssList1_Sheet1.Cells[i, 8].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["SEQNO2"].ToString().Trim()));

                    switch (dt.Rows[i]["CARDGB"].ToString().Trim())
                    {
                        case "1":
                            ssList1_Sheet1.Cells[i, 9].Text = "카드승인";
                            break;
                        case "2":
                            ssList1_Sheet1.Cells[i, 9].Text = "현금영수";
                            break;
                    }

                    ssList1_Sheet1.Cells[i, 10].Text = String.Format("{0:###,###,###}", VB.Val(dt.Rows[i]["CardAmt"].ToString().Trim()));
                    nCarAmt += VB.Val(dt.Rows[i]["CardAmt"].ToString().Trim());
                    ssList1_Sheet1.Cells[i, 11].Text = dt.Rows[i]["BIGO"].ToString().Trim();

                    nX = Convert.ToInt32(VB.Val(VB.Pstr(VB.Pstr(VB.Pstr(FstrPartNo, dt.Rows[i]["Part"].ToString().Trim(), 2), ";", 1), ",", 2)));

                    nCnt1[nX] += 1; //조별건수

                    nCnt4[nX] += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());   //조별금액
                    nTot2 += VB.Val(dt.Rows[i]["AMT"].ToString().Trim());

                    //나이별
                    strJumin1 = dt.Rows[i]["Jumin1"].ToString().Trim();
                    if(dt.Rows[i]["Jumin3"].ToString().Trim() != "")
                    {
                        strJumin2 = clsAES.DeAES(dt.Rows[i]["Jumin3"].ToString().Trim());
                    }
                    else
                    {
                        strJumin2 = dt.Rows[i]["Jumin2"].ToString().Trim();
                    }

                    //nAge = Convert.ToInt32(ComFunc.AgeCalcEx(strJumin1 + strJumin2, dt.Rows[i]["ActDate"].ToString().Trim().Replace("-", "")));
                    nAge = Convert.ToInt32(ComFunc.AgeCalcEx(strJumin1 + strJumin2, dt.Rows[i]["ActDate"].ToString().Trim()));

                    switch (nAge)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                            nCnt2[nX, 1] += 1;
                            break;

                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                            nCnt2[nX, 2] += 1;
                            break;

                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            nCnt2[nX, 3] += 1;
                            break;

                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                            nCnt2[nX, 4] += 1;
                            break;

                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                            nCnt2[nX, 5] += 1;
                            break;

                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            nCnt2[nX, 6] += 1;
                            break;

                        case 60:
                        case 61:
                        case 62:
                        case 63:
                        case 64:
                        case 65:
                        case 66:
                        case 67:
                        case 68:
                        case 69:
                            nCnt2[nX, 7] += 1;
                            break;

                        case 70:
                        case 71:
                        case 72:
                        case 73:
                        case 74:
                        case 75:
                        case 76:
                        case 77:
                        case 78:
                        case 79:
                            nCnt2[nX, 8] += 1;
                            break;

                        case 80:
                        case 81:
                        case 82:
                        case 83:
                        case 84:
                        case 85:
                        case 86:
                        case 87:
                        case 88:
                        case 89:
                            nCnt2[nX, 9] += 1;
                            break;

                        //case 90:
                        //case 91:
                        //case 92:
                        //case 93:
                        //case 94:
                        //case 95:
                        //case 96:
                        //case 97:
                        //case 98:
                        //case 99:
                        //case 100:
                        //case 101:
                        //case 102:
                        //case 103:
                        //case 104:
                        //case 105:
                        //case 106:
                        //case 107:
                        //case 108:
                        //case 109:
                        //case 110:
                        default:
                            nCnt2[nX, 10] += 1;
                            break;
                    }

                    //과별

                    nY = Convert.ToInt32(VB.Pstr(VB.Pstr(VB.Pstr(FstrDeptNo, dt.Rows[i]["DeptCode"].ToString().Trim(), 2), ";", 1), ",", 2));

                    nCnt3[nX, nY] += 1;

                    //시간대별
                    strSTime = dt.Rows[i]["STime"].ToString().Trim();

                    if (String.Compare(VB.Left(strSTime, 2), "08") <= 0 || String.Compare(VB.Left(strSTime, 2), "17") >= 0)
                    {
                        if (String.Compare(strSTime, "08:59") <= 0)
                        {
                            nCnt6[nX, 1] += 1;
                        }
                        else if (string.Compare(strSTime, "17:00") >= 0)
                        {
                            nCnt6[nX, 10] += 1;
                        }
                    }

                    else if(VB.Left(strSTime, 2) != "08" || VB.Left(strSTime, 2) != "17")
                    {
                        if (String.Compare(VB.Right(strSTime, 2), "59") <= 0)
                        {
                            switch (VB.Left(strSTime, 2))
                            {
                                case "09":
                                    nCnt6[nX, 2] += 1;
                                    break;
                                case "10":
                                    nCnt6[nX, 3] += 1;
                                    break;
                                case "11":
                                    nCnt6[nX, 4] += 1;
                                    break;
                                case "12":
                                    nCnt6[nX, 5] += 1;
                                    break;
                                case "13":
                                    nCnt6[nX, 6] += 1;
                                    break;
                                case "14":
                                    nCnt6[nX, 7] += 1;
                                    break;
                                case "15":
                                    nCnt6[nX, 8] += 1;
                                    break;
                                case "16":
                                    nCnt6[nX, 9] += 1;
                                    break;                            
                            }
                        }
                    }

                    //성별건수

                    switch (dt.Rows[i]["SEX"].ToString().Trim())
                    {
                        case "M":
                            nCnt7[nX, 1] += 1;
                            break;

                        default:
                            nCnt7[nX, 2] += 1;
                            break;
                    }

                    strOld = strNew;

                

                }
            }
            dt.Dispose();
            dt = null;

            ssList1_Sheet1.Rows.Count += 1;
            ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 2].Text = "합계";
            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:###,###,###,##0}", nTot2);

            ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 10].Text = String.Format("{0:###,###,###,##0}", nCarAmt);

            ssList2_Sheet1.Rows.Count = 0;
            ssList2_Sheet1.Rows.Count = 100;

            nRow = 0;
            nRow += 1;

            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "무인수납조";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = "건수";
            ssList2_Sheet1.Cells[nRow - 1, 2].Text = "수납금액";

            ssList2_Sheet1.Cells[nRow - 1, 0, nRow - 1, 2].BackColor = Color.LightPink;

            for(i = 1; i <= 7; i++)
            {
                nRow += 1;
                ssList2_Sheet1.Cells[nRow - 1, 0].Text = Fstrpart2[i];
                ssList2_Sheet1.Cells[nRow - 1, 1].Text = nCnt1[i].ToString();
                ssList2_Sheet1.Cells[nRow - 1, 2].Text = String.Format("{0:###,###,###,##0}", nCnt4[i]);
            }

            nRow += 1;

            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "전체건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = (nCnt1[1] + nCnt1[2] + nCnt1[3] + nCnt1[4] + nCnt1[5] + nCnt1[6] + nCnt1[7]).ToString();
            ssList2_Sheet1.Cells[nRow - 1, 2].Text = String.Format("{0:###,###,###,##0}", nCnt4[1] + nCnt4[2] + nCnt4[3] + nCnt4[4] + nCnt4[5] + nCnt4[6] + nCnt4[7]);

            //수납연령 견수
            nRow += 1;
            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "수납연령별";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = "건수";

            ssList2_Sheet1.Cells[nRow - 1, 0, nRow - 1, 2].BackColor = Color.LightPink;

            nTot2 = 0;
            for(i = 1; i <= 10; i++)
            {
                nRow += 1;
                ssList2_Sheet1.Cells[nRow -1, 0].Text = FstrAge2[i];
                ssList2_Sheet1.Cells[nRow -1, 1].Text = (nCnt2[1, i] + nCnt2[2, i] + nCnt2[3, i] + nCnt2[4, i] + nCnt2[5, i] + nCnt2[6, i] + nCnt2[7, i]).ToString();
                nTot2 += nCnt2[1, i] + nCnt2[2, i] + nCnt2[3, i] + nCnt2[4, i] + nCnt2[5, i] + nCnt2[6, i] + nCnt2[7, i];
            }

            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "전체건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = String.Format("{0:###,###,##0}", nTot2);

            //과별건수
            nRow += 1;
            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "과별건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = "건수";

            ssList2_Sheet1.Cells[nRow - 1, 0, nRow - 1, 2].BackColor = Color.LightPink;

            nTot2 = 0;
            for(i = 1; i <= nDeptCnt; i++)
            {
                nRow += 1;
                ssList2_Sheet1.Cells[nRow - 1, 0].Text = FstrDept2[i];
                ssList2_Sheet1.Cells[nRow - 1, 1].Text = (nCnt3[1, i] + nCnt3[2, i] + nCnt3[3, i] + nCnt3[4, i] + nCnt3[5, i] + nCnt3[6, i] + nCnt3[7, i]).ToString();
                nTot2 += nCnt3[1, i] + nCnt3[2, i] + nCnt3[3, i] + nCnt3[4, i] + nCnt3[5, i] + nCnt3[6, i] + nCnt3[7, i];


            }
            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "전체건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = String.Format("{0:###,###,##0}", nTot2);

            //시간대별건수
            nRow += 1;
            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "시간대별건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = "건수";
            ssList2_Sheet1.Cells[nRow - 1, 0, nRow - 1, 2].BackColor = Color.LightPink;

            nTot2 = 0;

            for(i = 1; i <= 10; i++)
            {
                nRow += 1;
                ssList2_Sheet1.Cells[nRow - 1, 0].Text = FstrTime2[i];
                ssList2_Sheet1.Cells[nRow - 1, 1].Text = (nCnt6[1, i] + nCnt6[2, i] + nCnt6[3, i] + nCnt6[4, i] + nCnt6[5, i] + nCnt6[6, i] + nCnt6[7, i]).ToString();
                nTot2 += nCnt6[1, i] + nCnt6[2, i] + nCnt6[3, i] + nCnt6[4, i] + nCnt6[5, i] + nCnt6[6, i] + nCnt6[7, i];
            }
            nRow += 1;

            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "전체건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = String.Format("{0:###,###,##0}", nTot2);

            //성별건수
            nRow += 1;
            nRow += 1;
            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "성별건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = "건수";

            ssList2_Sheet1.Cells[nRow - 1, 0, nRow - 1, 2].BackColor = Color.LightPink;

            nTot2 = 0;

            for (i = 1; i <= 2; i++)
            {
                nRow += 1;
                ssList2_Sheet1.Cells[nRow - 1, 0].Text = FstrSex2[i];
                ssList2_Sheet1.Cells[nRow - 1, 1].Text = (nCnt7[1, i] + nCnt7[2, i] + nCnt7[3, i] + nCnt7[4, i] + nCnt7[5, i] + nCnt7[6, i] + nCnt7[7, i]).ToString();
                nTot2 += nCnt7[1, i] + nCnt7[2, i] + nCnt7[3, i] + nCnt7[4, i] + nCnt7[5, i] + nCnt7[6, i] + nCnt7[7, i];
            }
            nRow += 1;

            ssList2_Sheet1.Cells[nRow - 1, 0].Text = "전체건수";
            ssList2_Sheet1.Cells[nRow - 1, 1].Text = String.Format("{0:###,###,##0}", nTot2);

            ssList2_Sheet1.RowCount = ssList2_Sheet1.NonEmptyRowCount;
            ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


            //일자별 건수
            ssList3_Sheet1.Rows.Count = 1;
            ssList3_Sheet1.Rows.Count = 1000;

            ssList3_Sheet1.ColumnCount = nDayCnt + 1;

            nTot3 = 0;
            for(i = 1; i <= nDayCnt; i++)
            {
                ssList3_Sheet1.Cells[0, i].Text = strCnt5[i];
                ssList3_Sheet1.Cells[1, i].Text = nCnt5[i].ToString();
                nTot3 += nCnt5[i];
            }

            ssList3_Sheet1.Cells[0, 0].Text = "전체";
            ssList3_Sheet1.Cells[1, 0].Text = String.Format("{0:###,###,##0}", nTot3);

            //Row 정리
            ssList3_Sheet1.RowCount = ssList3_Sheet1.NonEmptyRowCount;
            ssList3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssList3_Sheet1.ColumnCount = ssList3_Sheet1.NonEmptyColumnCount;

            ssList3_Sheet1.Cells[0, 0, ssList3_Sheet1.Rows.Count - 1, ssList3_Sheet1.Columns.Count - 1].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            ssList3_Sheet1.Cells[0, 0, ssList3_Sheet1.Rows.Count - 1, ssList3_Sheet1.Columns.Count - 1].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;

            Cursor.Current = Cursors.Default;
        }

    }
}
