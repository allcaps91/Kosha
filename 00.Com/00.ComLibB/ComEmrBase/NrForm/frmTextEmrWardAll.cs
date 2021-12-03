using ComBase;
using FarPoint.Win.Spread.CellType;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ComEmrBase
{
    /// <summary>
    /// 일괄작업 폼
    /// \mtsEmrf\frmTextEmrWardAll.frm
    /// </summary>
    public partial class frmTextEmrWardAll : Form, EmrChartForm
    {
        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        //EmrForm pForm = null;

        public string mstrFormNo = "";
        public string mstrFORMNAME = "";
        public string mstrUpdateNo = "0";
        public string mstrEmrNo = "0";  //961 131641  //963 735603
        public string mstrMode = "W";

        const int mlngStartCol = 13;
        const int mlngPtStartCol = 2;

        #endregion

        #region TOP 메뉴 시간 관련
        usTimeSet usTimeSetEvent = null;
        #endregion

        #region //EmrChartForm        

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
        #endregion

        #region //Private Function 기록지 클리어, 저장, 삭제, 프린터

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            mstrEmrNo = "0";
            ClearForm();
            ClearChart();
        }

        // <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;

            if (pAcp.ptNo.Length == 0)
            {
                ComFunc.MsgBoxEx(this, "환자를 선택해 주십시오.");
                return dblEmrNo;
            }

  

            return dblEmrNo;
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        private bool pDelData()
        {
            bool rtnVal = false;

            return rtnVal;
        }


        #endregion //기록지 클리어, 저장, 삭제, 프린터

        #region //Public Function 외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }
       
        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            return pSaveEmrData();
        }


        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool DelDataMsg()
        {
            return pDelData();
        }

        /// <summary>
        /// 클리어
        /// </summary>
        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            pClearForm();
        }

        public void SetUserFormMsg(double dblMACRONO)
        {
            //TODO
            //pSetUserForm(dblMACRONO);
        }

        public bool SaveUserFormMsg(double dblMACRONO)
        {
            bool rtnVal = false;
            return rtnVal;
        }

        public int PrintFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //if (strPRINTFLAG == "N")
            //{
            //    frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
            //    frmEmrPrintOptionX.ShowDialog();
            //}

            //if (clsFormPrint.mstrPRINTFLAG == "-1")
            //{
            //    return rtnVal;
            //}

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            //{ 
            //    return rtnVal;
            //}

            //rtnVal = clsFormPrint.PrintFormLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = 0;
            //rtnVal = clsFormPrint.PrintToTifFileLong(mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion

        #region //생성자
        public frmTextEmrWardAll()
        {
            InitializeComponent();
        }
      
        public frmTextEmrWardAll(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();
        }
       #endregion //생성자

        #region 변수
        private string gUserGrade = string.Empty;
        #endregion
        
        private void frmTextEmrWardAll_Load(object sender, EventArgs e)
        {
            dtpChartDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            txtMedFrTime.Text = dtpChartDate.Value.ToString("HH:mm");

            ssUserChart_Sheet1.ColumnHeader.Visible = true;
            ssUserChart_Sheet1.ColumnCount = mlngStartCol;
            ssUserChart_Sheet1.RowCount = 0;

            ssFORM_Sheet1.ColumnCount = mlngStartCol;
            ssFORM_Sheet1.RowCount = 0;

            lblFORMNAME.Text = "";

            SetCombo();
            GetUserChart();
            GetPatList();
        }

        /// <summary>
        /// 콤보박스 설정
        /// </summary>
        void SetCombo()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;

            cboTeam.Items.Clear();
            cboTeam.Items.Add("전체");
            cboTeam.Items.Add("A");
            cboTeam.Items.Add("B");
            cboTeam.SelectedIndex = 0;

            cboJob.Items.Clear();
            cboJob.Items.Add("1.재원자명단");
            cboJob.Items.Add("2.당일입원자");
            cboJob.Items.Add("3.퇴원예고자");
            cboJob.Items.Add("4.당일퇴원자");
            cboJob.Items.Add("5.중증도미분류");
            cboJob.Items.Add("6.수술예정자");
            cboJob.Items.Add("7.진단명 누락자");
            cboJob.Items.Add("A.응급실경유입원(1-3일전)");
            cboJob.Items.Add("B.재원기간 7-14일 환자");
            cboJob.Items.Add("C.재원기간 3-7일 환자");
            cboJob.Items.Add("D.어제퇴원자");

            #region ComboWard_SET()
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            int sIndex = -1;
            int sCount = 0;

            try
            {
                SQL = " SELECT NAME WARDCODE, MATCH_CODE";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "NUR_CODE";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = '2'";
                SQL = SQL + ComNum.VBLF + "   AND SUBUSE = '1'";
                SQL = SQL + ComNum.VBLF + " ORDER BY PRINTRANKING";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cboWard.Items.Add(reader.GetValue(0).ToString().Trim());
                        if (reader.GetValue(1).ToString().Trim().Equals(clsType.User.BuseCode))
                        {
                            sIndex = sCount;
                        }
                        sCount += 1;
                    }
                }

                cboWard.Items.Add("HD");
                cboWard.Items.Add("ER");
                cboWard.Items.Add("OP");
                cboWard.Items.Add("ENDO");
                cboWard.Items.Add("외래수혈");

                cboWard.SelectedIndex = sIndex == -1 ? cboWard.SelectedIndex : sIndex + 1;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            #endregion

            #region ComboVRoom
            cboVRoom.Items.Clear();
            cboVRoom.Items.Add("전체");
            SQL = string.Empty;

            try
            {
                SQL = " SELECT ROOMCODE FROM IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " Where ACTDATE Is Null ";
                SQL = SQL + ComNum.VBLF + "  AND PANO NOT IN ('81000004') ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS <> '9' ";

                switch(cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode='234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode='233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY ROOMCODE ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ROOMCODE ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cboVRoom.Items.Add(reader.GetValue(0).ToString().Trim());
                    }
                }

                cboVRoom.SelectedIndex = 0;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion

            #region ComboVDr
            cboVDr.Items.Clear();
            cboVDr.Items.Add("전체");
            SQL = string.Empty;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT DRNAME, A.DRCODE";
                SQL = SQL + ComNum.VBLF + "FROM (";
                SQL = SQL + ComNum.VBLF + " SELECT DRCODE FROM ADMIN.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " Where ACTDATE Is Null ";
                SQL = SQL + ComNum.VBLF + "  AND PANO NOT IN ('81000004') ";
                SQL = SQL + ComNum.VBLF + "  AND GBSTS <> '9' ";

                switch (cboWard.Text.Trim())
                {
                    case "전체":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode>' ' ";
                        break;
                    case "MICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode = '234' ";
                        break;
                    case "SICU":
                        SQL = SQL + ComNum.VBLF + "  AND  RoomCode = '233' ";
                        break;
                    case "ND":
                    case "NR":
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode IN ('ND','IQ','NR') ";
                        break;
                    default:
                        SQL = SQL + ComNum.VBLF + "  AND  WardCode = '" + cboWard.Text.Trim() + "' ";
                        break;
                }

                SQL = SQL + ComNum.VBLF + " GROUP BY DRCODE ";
                SQL = SQL + ComNum.VBLF + ") A";
                SQL = SQL + ComNum.VBLF + "INNER JOIN ADMIN.OCS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "   ON A.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.DRCODE ";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cboVDr.Items.Add(reader.GetValue(0).ToString().Trim() + "." + reader.GetValue(1).ToString().Trim());
                    }
                }

                cboVDr.SelectedIndex = 0;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
            #endregion
        }

        /// <summary>
        /// 일괄작업 폼 리스트 가져오기
        /// </summary>
        void GetUserChart()
        {
            OracleDataReader reader = null;
            string SQL = string.Empty;

            try
            {
                SQL = "SELECT  A.GRPFORMNAME, B.FORMNAME1 FORMNAME, B.DISPSEQ, B.FORMNO,  A.GRPFORMNO  ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.EMRGRPFORM A ";
                SQL = SQL + ComNum.VBLF + "      INNER JOIN ADMIN.EMRFORM B";
                SQL = SQL + ComNum.VBLF + "          ON A.GRPFORMNO = B.GRPFORMNO";
                //'==================================
                //'2014-01-15
                //'2201 일괄 기록 추가. 향후 한가지 더 추가해달라고 하면. 코드화 예정
                SQL = SQL + ComNum.VBLF + "          AND B.FORMNO IN (1562, 1572, 1614, 1547, 1725, 1573, 1970, 2201)";
                //'===================================
                SQL = SQL + ComNum.VBLF + "  WHERE (B.USECHECK IS NULL ";
                SQL = SQL + ComNum.VBLF + "      OR B.USECHECK = '0')";
                SQL = SQL + ComNum.VBLF + "  ORDER BY B.GRPFORMNO, B.FORMNO";

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (reader.HasRows)
                {
                    ssFORM_Sheet1.RowCount = 0;
                    while (reader.Read())
                    {
                        ssFORM_Sheet1.RowCount += 1;
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 0].Text = reader.GetValue(0).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 1].Text = reader.GetValue(1).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 3].Text = reader.GetValue(3).ToString().Trim();
                        ssFORM_Sheet1.Cells[ssFORM_Sheet1.RowCount - 1, 4].Text = reader.GetValue(4).ToString().Trim();
                    }
                }

                cboVRoom.SelectedIndex = 0;

                reader.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// 해당환자 표시
        /// </summary>
        void GetPatList() 
        {
            DataTable dt = null;
            string SQL = string.Empty;

            try
            {
                ssUserChart_Sheet1.RowCount = 0;

                string strJob = VB.Left(cboJob.Text, 1);

                string strPriDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-1).ToShortDateString();
                string strToDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(0).ToShortDateString();
                string strNextDate = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(1).ToShortDateString();

                if (cboWard.Text.Trim() != "HD")
                {
                    SQL = "SELECT M.WardCode,M.RoomCode,M.Pano,M.SName,M.Sex,M.Age,M.Bi,M.PName,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(M.InDate,'YYYY-MM-DD') InDate,M.Ilsu,M.IpdNo,M.GbSts,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(M.OutDate,'YYYY-MM-DD') OutDate,";
                    SQL = SQL + ComNum.VBLF + " M.DeptCode,M.DrCode,D.DrName,M.AmSet1,M.AmSet4,M.AmSet6,M.AmSet7 ";
                    SQL = SQL + ComNum.VBLF + " FROM   ADMIN.IPD_NEW_MASTER  M, ";
                    SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_PATIENT P, ";
                    SQL = SQL + ComNum.VBLF + "        ADMIN.BAS_DOCTOR  D ";

                    switch (cboWard.Text.Trim())
                    {
                        case "전체": SQL = SQL + ComNum.VBLF + "WHERE M.WardCode>' ' "; break;
                        case "MICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='234' "; break;
                        case "SICU": SQL = SQL + ComNum.VBLF + "WHERE M.RoomCode='233' "; break;
                        case "ND":
                        case "NR":
                            SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('ND','IQ','NR') "; break;
                        //'Case "3B":   SQL = SQL + ComNum.VBLF + "WHERE M.WardCode IN ('3B','DR') "; //'COMBOBOX 처리
                        default: SQL = SQL + ComNum.VBLF + "WHERE M.WardCode='" + cboWard.Text.Trim() + "' "; break;
                    }



                    //if (clsType.User.IdNumber != "4349") SQL = SQL + ComNum.VBLF + "  AND M.Pano<>'81000004' ";

                    //'작업분류
                    if (strJob == "1") //'재원자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.OutDate IS NULL OR M.OutDate>=TO_DATE('" + strToDate + "','YYYY-MM-DD')) ";
                        SQL = SQL + ComNum.VBLF + " AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND M.Pano < '90000000' ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "2") //'당일입원자
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.InDate >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime >= TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.IpwonTime < TO_DATE('" + strNextDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND M.Pano < '90000000' ";
                        SQL = SQL + ComNum.VBLF + "  AND M.Pano <> '81000004' ";
                        SQL = SQL + ComNum.VBLF + "  AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "3") //'퇴원예고
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate>=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts NOT IN ('7','9') ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND (M.ROutDate IS NULL OR M.ROutDate>=TRUNC(SYSDATE) ) ";
                    }
                    else if (strJob == "4") //'당일퇴원
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS = '7' ";// '퇴원수납완료
                    }
                    else if (strJob == "6") //'수술예정자
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.ActDate IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                    }
                    else if (strJob == "A") //'응급실경유입원 1-3일전
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu >= 1 AND M.Ilsu<=3) ";
                        SQL = SQL + ComNum.VBLF + " AND M.AmSet7 IN ('3','4','5') ";
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate IS NULL ";
                        SQL = SQL + ComNum.VBLF + " AND M.ROutDate>=TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "B") //'재원기간 7-14일 환자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=7 AND M.Ilsu<=14) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }
                    else if (strJob == "C") //'재원기간 3-7일 환자
                    {
                        SQL = SQL + ComNum.VBLF + " AND (M.ActDate IS NULL OR M.ActDate=TRUNC(SYSDATE)) ";
                        SQL = SQL + ComNum.VBLF + " AND (M.Ilsu>=3 AND M.Ilsu<=7) ";
                        SQL = SQL + ComNum.VBLF + " AND M.GbSTS <> '9' ";
                    }

                    else if (strJob == "D") //'어제퇴원자
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.OutDate=TRUNC(SYSDATE-1) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND M.GbSts IN ('0','2')  ";
                    }


                    SQL = SQL + ComNum.VBLF + "  AND M.Pano=P.Pano(+) ";
                    SQL = SQL + ComNum.VBLF + "  AND M.DrCode=D.DrCode(+) ";

                    //'스탭
                    if (cboVDr.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.DRCODE = '" + VB.Right(cboVDr.Text.Trim(), 4) + "' ";
                    }
                    //'병실
                    if (cboVRoom.Text != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND M.ROOMCODE = '" + cboVRoom.Text.Trim() + "' ";
                    }

                    if (mstrFormNo.Equals("1572"))
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS ( SELECT SUB1.PTNO";
                        SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OCS_IORDER SUB1";
                        SQL = SQL + ComNum.VBLF + "  WHERE SUB1.BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                        SQL = SQL + ComNum.VBLF + "      AND SUB1.SUCODE = 'C3710'";
                        SQL = SQL + ComNum.VBLF + "      AND SUB1.PTNO = M.PANO)        ";
                    }


                    if (cboTeam.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + "  AND EXISTS ";
                        SQL = SQL + ComNum.VBLF + " (SELECT * FROM ADMIN.NUR_TEAM_ROOMCODE T";
                        SQL = SQL + ComNum.VBLF + "          WHERE M.WARDCODE = T.WARDCODE";
                        SQL = SQL + ComNum.VBLF + "             AND M.ROOMCODE = T.ROOMCODE";
                        SQL = SQL + ComNum.VBLF + "             AND T.TEAM = '" + cboTeam.Text.Trim() + "')";
                    }
                }
                else
                {
                    SQL = " SELECT '' AS WardCode, '' AS RoomCode, Pano, SName, Sex, Age, '' AS Bi,  '' AS PName,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(BDATE,'YYYY-MM-DD') AS InDate, 0 as Ilsu, 0 as IpdNo,  '' AS GBSTS,";
                    SQL = SQL + ComNum.VBLF + " '' AS OutDate, ";
                    SQL = SQL + ComNum.VBLF + "  DeptCode, DrCode,  '' AS DrName,  '' AS AmSet1, '' AS AmSet4,  '' AS AmSet6,  '' AS AmSet7 ";
                    SQL = SQL + ComNum.VBLF + "    FROM ADMIN.OPD_MASTER M";
                    SQL = SQL + ComNum.VBLF + " WHERE DEPTCODE = 'HD'";
                    SQL = SQL + ComNum.VBLF + "      AND JIN IN ('0','1','2','3','4','5','6','7','9','C','E','F','H','M','L','K', 'N','I','J','Q','R','S','A','B')  ";
                    SQL = SQL + ComNum.VBLF + "      AND BDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "UNION ALL    ";
                    SQL = SQL + ComNum.VBLF + "SELECT B.WARDCODE AS WardCode, TO_CHAR(B.ROOMCODE) AS RoomCode, A.Pano, A.SName, A.Sex, A.Age, B.BI,  B.PNAME,";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(B.INDATE,'YYYY-MM-DD') AS InDate, B.ILSU, IpdNo,  GBSTS,";
                    SQL = SQL + ComNum.VBLF + "     TO_CHAR(B.OutDate,'YYYY-MM-DD') OUTDATE,";
                    SQL = SQL + ComNum.VBLF + "     b.DeptCode , b.DrCode, c.DrName, AmSet1, AmSet4, AmSet6, AmSet7";
                    SQL = SQL + ComNum.VBLF + "   FROM ADMIN.TONG_HD_DAILY A, ADMIN.IPD_NEW_MASTER B, ADMIN.BAS_DOCTOR C";
                    SQL = SQL + ComNum.VBLF + "WHERE TDATE = TO_DATE('" + strToDate + "','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "    AND IPDOPD = 'I'";
                    SQL = SQL + ComNum.VBLF + "    AND A.PANO = B.PANO";
                    SQL = SQL + ComNum.VBLF + "    AND TRUNC(B.INDATE) <= A.TDATE";
                    SQL = SQL + ComNum.VBLF + "    AND (B.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD') OR B.OUTDATE >= A.TDATE)";
                    SQL = SQL + ComNum.VBLF + "    AND B.DRCODE = C.DRCODE";
                }

                SQL = SQL + ComNum.VBLF + "   ORDER BY RoomCode, SName, Indate DESC  ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt == null)
                    return;

                if (dt.Rows.Count > 0)
                {
                    ssUserChart_Sheet1.RowCount = dt.Rows.Count;
                    ssUserChart_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                    {
                        if (ssUserChart_Sheet1.Columns[i].Tag != null)
                        {
                            ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                        }
                    }

                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 1].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 2].Text = dt.Rows[i]["SName"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 3].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 5].Text = "0";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 6].Text = "0";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 7].Text = dt.Rows[i]["InDate"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 8].Text = "120000";
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 9].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssUserChart_Sheet1.Cells[i, mlngPtStartCol + 10].Text = dt.Rows[i]["DrCode"].ToString().Trim();

                    }
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            MakeWardChart();
        }

        /// <summary>
        /// 폼을 클리어 한다
        /// </summary>
        void ClearForm()
        {
            mstrEmrNo = "0";

            for(int i = 3; i < ssUserChart_Sheet1.RowCount; i++)
            {
                for(int j = 3; j < ssUserChart_Sheet1.ColumnCount; j++)
                {
                    ssUserChart_Sheet1.Cells[i, j].Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 서식지를 클리어 한다
        /// </summary>
        void ClearChart()
        {
            ssUserChart_Sheet1.ColumnCount = 0;
            ssUserChart_Sheet1.ColumnCount = 1;
            ssUserChart_Sheet1.Columns[0].Locked = false;

        }

        /// <summary>
        /// 서식지를 만든다
        /// </summary>
        void MakeWardChart()
        {
            ssUserChart_Sheet1.ColumnCount = mlngStartCol;
            ssFORMXML_Sheet1.ColumnCount = mlngStartCol;

            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;


            SQL = SQL + ComNum.VBLF + "  SELECT A.FORMNO, A.FORMNAME1 FORMNAME, RMKINX,  ";
            SQL = SQL + ComNum.VBLF + "      B.ITEMNO, B.ITEMNAME, B.ITEMTYPE, B.ITEMHALIGN, B.ITEMVALIGN,";
            SQL = SQL + ComNum.VBLF + "      B.ITEMHEIGHT, B.ITEMWIDTH, B.MULTILINE, B.USEMACRO, B.CONTROLID, B.ITEMRMK, B.TAGHEAD, B.TAGTAIL";
            SQL = SQL + ComNum.VBLF + "      FROM ADMIN.EMRFORM A INNER JOIN ADMIN.EMROPTFORM B";
            SQL = SQL + ComNum.VBLF + "         ON A.FORMNO = B.FORMNO";
            SQL = SQL + ComNum.VBLF + "      WHERE A.FORMNO = " + VB.Val(mstrFormNo);
            SQL = SQL + ComNum.VBLF + "      ORDER BY B.ITEMNO";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count == 0)
            {
                return;
            }

            ssUserChart_Sheet1.ColumnCount = mlngStartCol + dt.Rows.Count;
            ssFORMXML_Sheet1.ColumnCount = mlngStartCol + dt.Rows.Count;
            ssFORMXML_Sheet1.RowCount = 2;
            ssItem_Sheet1.ColumnCount = mlngStartCol + dt.Rows.Count;

            FarPoint.Win.ComplexBorder complexBorder2 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            ssUserChart_Sheet1.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.SheetCornerStyle.Border = complexBorder2;
            ssUserChart_Sheet1.ColumnHeader.DefaultStyle.Border = complexBorder2;
            ssUserChart_Sheet1.RowHeader.DefaultStyle.Border = complexBorder2;

            List<string> lstCnt = new List<string>();

            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssUserChart_Sheet1.Columns[mlngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssUserChart_Sheet1.Columns[mlngStartCol + i].Width = ssUserChart_Sheet1.Columns[mlngStartCol + i].Label.Equals("비고") == false ? (int)VB.Val(dt.Rows[i]["ITEMHEIGHT"].ToString().Trim()) + 30 : 200;
                    if (ssUserChart_Sheet1.Columns[mlngStartCol + i].Label.Equals("Insulin"))
                    {
                        ssUserChart_Sheet1.Columns[mlngStartCol + i].Width = 120;
                    }

                    //ssUserChart_Sheet1.Columns[0].Width = 200;


                    if (ssUserChart_Sheet1.ColumnCount == 0)
                        continue;

                    ssUserChart_Sheet1.Columns[mlngStartCol + i].Locked = false;
                    lstCnt.Clear();

                    switch (dt.Rows[i]["ITEMTYPE"].ToString().Trim().ToUpper())
                    {
                        case "EDIT":
                            TextCellType TypeText = new TextCellType();
                            TypeText.Multiline = dt.Rows[i]["MULTILINE"].ToString().Trim().Equals("1");
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = TypeText;
                            break;
                        case "COMBO":
                            ComboBoxCellType TypeCombo = new ComboBoxCellType();
                            ListBox listBox = new ListBox();
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = TypeCombo;
                            listBox.Items.AddRange(dt.Rows[i]["ITEMRMK"].ToString().Trim().Split('^'));
                            TypeCombo.ListControl = listBox;
                            TypeCombo.Editable = true;
                            if (dt.Rows[i]["RMKINX"].ToString().Trim().Length > 0)
                            {
                                ssUserChart_Sheet1.Columns[mlngStartCol + i].Tag = listBox.Items[ (int)VB.Val(dt.Rows[i]["RMKINX"].ToString().Trim())];
                            }
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = TypeCombo;

                            break;
                        case "CHECK":
                            CheckBoxCellType TypeCheckBox = new CheckBoxCellType();
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].CellType = TypeCheckBox;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMHALIGN"].ToString().Trim().ToUpper())
                    {
                        case "LEFT":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                            break;
                        case "RIGHT":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Right;
                            break;
                    }


                    switch (dt.Rows[i]["ITEMVALIGN"].ToString().Trim().ToUpper())
                    {
                        case "TOP":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Top;
                            break;
                        case "CENTER":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                            break;
                        case "BOTTOM":
                            ssUserChart_Sheet1.Columns[mlngStartCol + i].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Bottom;
                            break;
                    }

                    //if (dt.Rows[i]["USEMACRO"].ToString().Trim() == "1" && dt.Rows[i]["CONTROLID"].ToString().Trim().Length > 0)
                    //{
                    //    ssUserChart_Sheet1.Columns[mlngStartCol + i].Tag = dt.Rows[i]["CONTROLID"].ToString().Trim();
                    //}

                    ssFORMXML_Sheet1.Cells[0, mlngStartCol + i].Text = dt.Rows[i]["TAGHEAD"].ToString().Trim();
                    ssFORMXML_Sheet1.Cells[1, mlngStartCol + i].Text = dt.Rows[i]["TAGTAIL"].ToString().Trim();

                    ssItem_Sheet1.Columns[mlngStartCol + i].Label = dt.Rows[i]["ITEMNAME"].ToString().Trim();
                    ssItem_Sheet1.Cells[0, mlngStartCol + i].Text = dt.Rows[i]["CONTROLID"].ToString().Trim();
                }

                if (ssUserChart_Sheet1.RowCount == 0)
                    return;

                for (int i = 0; i < ssUserChart_Sheet1.ColumnCount; i++)
                {
                    if (ssUserChart_Sheet1.Columns[i].Tag != null)
                    {
                        ssUserChart_Sheet1.Cells[0, i, ssUserChart_Sheet1.RowCount - 1, i].Text = ssUserChart_Sheet1.Columns[i].Tag.ToString();
                    }
                }

                switch (mstrFormNo)
                {
                    case "1562":
                    case "2201":
                        DefaultValue();
                        break;
                    default:
                        return;
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void mbtnTime_Click(object sender, EventArgs e)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }

            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Left = 179;
            usTimeSetEvent.Top = 75;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void ssFORM_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            mstrFormNo = ssFORM_Sheet1.Cells[e.Row, 3].Text.Trim();
            mstrFORMNAME = ssFORM_Sheet1.Cells[e.Row, 1].Text.Trim();

            lblFORMNAME.Text = mstrFORMNAME;

            if (mstrFormNo.Equals("1572"))
            {
                GetPatList();
            }

            MakeWardChart();

         
        

            return;

        }

        /// <summary>
        /// 생성후 기본값 설정
        /// </summary>
        void DefaultValue()
        {
            Cursor.Current = Cursors.WaitCursor;

            string SQL = string.Empty;
            DataTable dt = null;
            OracleDataReader reader = null;

            string strVal1 = string.Empty;
            string strVal2 = string.Empty;

            SQL = " SELECT VAL1, VAL2 FROM ADMIN.EMR_SETUP_01 ";
            SQL += ComNum.VBLF + " WHERE BUSE = '" + clsType.User.BuseCode + "'";

            string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(sqlErr, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                return;
            }

            try
            {
                if (dt.Rows.Count > 0)
                {
                    strVal1 = dt.Rows[0]["VAL1"].ToString().Trim();
                    strVal2 = dt.Rows[0]["VAL2"].ToString().Trim();
                }

                dt.Dispose();

                int intCol = -1;
                List<string> strTEMP1 = new List<string>();
                

                for (int i = 0; i < ssUserChart_Sheet1.RowCount; i++)
                {
                    string strPano = ssUserChart_Sheet1.Cells[i, 3].Text.Trim();
                    int index = -1;

                    switch (mstrFormNo)
                    {
                        case "1562":
                            intCol = 17;
                            break;
                        case "2201":
                            intCol = 16;
                            break;
                    }

                    #region 쿼리

                    SQL = " SELECT RT_A, LT_A, RT_L, LT_L ";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.NUR_VITAL_REGION ";
                    SQL = SQL + ComNum.VBLF + "    WHERE PANO = '" + strPano + "'";

                    string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                    if (SqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }


                    ListBox strTEMP1_T = new ListBox();
                    strTEMP1.Clear();

                    if (reader.HasRows && reader.Read())
                    {
                        if (reader.GetValue(0).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Arm");
                        }
                        if(reader.GetValue(1).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Arm");
                        }
                        if (reader.GetValue(2).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Rt Leg");
                        }
                        if (reader.GetValue(3).ToString().Trim().Equals("0"))
                        {
                            strTEMP1.Add("Lt Leg");
                        }

                        strTEMP1_T.Items.Clear();
                        strTEMP1_T.Items.AddRange(strTEMP1.ToArray());

                        if (strVal1.Equals("Rt Arm") && strTEMP1.IndexOf("Rt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Arm") && strTEMP1.IndexOf("Lt Arm") == -1)
                        {
                        }
                        else if (strVal1.Equals("Rt Leg") && strTEMP1.IndexOf("Rt Leg") == -1)
                        {
                        }
                        else if (strVal1.Equals("Lt Leg") && strTEMP1.IndexOf("Lt Leg") == -1)
                        {
                        }
                        else
                        {
                            index = strTEMP1_T.Items.IndexOf(strVal1);
                            if(strVal1.Length > 0 && index != -1)
                            {
                                strTEMP1_T.Items.RemoveAt(index);
                                strTEMP1_T.Items.Insert(0, strVal1);
                            }
                        }

                        ssUserChart_Sheet1.Cells[i, intCol].CellType = null;
                        ComboBoxCellType TypeCombo = new ComboBoxCellType();
                        TypeCombo.ListControl = strTEMP1_T;
                        TypeCombo.Editable = true;
                        ssUserChart_Sheet1.Cells[i, intCol].CellType = TypeCombo;
                        ssUserChart_Sheet1.Cells[i, intCol].Text = strTEMP1_T.Items[0].ToString();
                    }
                    else
                    {
                        if (strVal1.Length > 0)
                        {
                            ListBox strTEMP2_T = new ListBox();
                            strTEMP2_T.Items.Add("Rt Arm");
                            strTEMP2_T.Items.Add("Lt Arm");
                            strTEMP2_T.Items.Add("Rt Leg");
                            strTEMP2_T.Items.Add("Lt Leg");

                            index = strTEMP2_T.Items.IndexOf(strVal1);
                            if (index != -1)
                            {
                                strTEMP2_T.Items.RemoveAt(index);
                                strTEMP2_T.Items.Insert(0, strVal1);
                            }

                            ComboBoxCellType TypeCombo3 = new ComboBoxCellType();
                            ssUserChart_Sheet1.Cells[i, intCol].CellType = null;
                            TypeCombo3.ListControl = strTEMP2_T;
                            TypeCombo3.Editable = true;
                            ssUserChart_Sheet1.Cells[i, intCol].CellType = TypeCombo3;
                            ssUserChart_Sheet1.Cells[i, intCol].Text = strTEMP2_T.Items[0].ToString();
                        }
                    }

                    reader.Dispose();


                    switch (mstrFormNo)
                    {
                        case "1562":
                            intCol = 21;
                            break;
                        case "2201":
                            intCol = 20;
                            break;
                    }

                    ListBox strTEMP3_T = new ListBox();
                    strTEMP3_T.Items.Clear();
                    strTEMP3_T.Items.Add("고막");
                    strTEMP3_T.Items.Add("Axilla");
                    strTEMP3_T.Items.Add("Oral");
                    strTEMP3_T.Items.Add("Rectal");

                    index = strTEMP3_T.Items.IndexOf(strVal2);
                    if (strVal2.Length > 0 && index != -1)
                    {
                        strTEMP3_T.Items.RemoveAt(index);
                        strTEMP3_T.Items.Insert(0, strVal2);
                    }

                    ComboBoxCellType TypeCombo4 = new ComboBoxCellType();
                    ssUserChart_Sheet1.Cells[i, intCol].CellType = null;
                    TypeCombo4.ListControl = strTEMP3_T;
                    TypeCombo4.Editable = true;
                    ssUserChart_Sheet1.Cells[i, intCol].CellType = TypeCombo4;
                    ssUserChart_Sheet1.Cells[i, intCol].Text = strTEMP3_T.Items[0].ToString();
                    #endregion
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetPatList();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for(int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    for (int lngCol = 13; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
                    {
                        ssUserChart_Sheet1.Cells[lngRow, lngCol].Text = "";
                    }
                }
            }
        }

        private void btnSearchPreHis_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mstrFormNo))
                return;

            if (ssUserChart_Sheet1.RowCount == 0)
                return;

            for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                GetUchartHis(ssUserChart_Sheet1.Cells[lngRow, 3].Text.Trim(), mstrFormNo, ssUserChart, lngRow);
            }
        }

        /// <summary>
        /// 조회 함수
        /// </summary>
        void GetUchartHis(string strPtno, string strFormNo, FarPoint.Win.Spread.FpSpread objSpread, int lngRow)
        {
            if (mstrFormNo == "")
                return;

            if (pAcp.ptNo == "")
                return;

            objSpread.ActiveSheet.RowCount = 0;

            //int lngStartCol = 12;

            string SQL = string.Empty;
            DataTable dt = null;

            //READ_INIT_FALL();

            try
            {
                SQL = " SELECT A.EMRNO, ";
                SQL += ComNum.VBLF + "    xmltype.getstringval(extract(CHARTXML,'/chart')) AS CHARTA,";
                SQL += ComNum.VBLF + "    0 ACPNO, A.INOUTCLS, A.CHARTDATE, A.CHARTTIME, ";
                SQL += ComNum.VBLF + "    A.MEDDEPTCD, A.MEDDRCD, A.USEID,";
                SQL += ComNum.VBLF + "    A.MEDFRDATE, A.MEDFRTIME,  B.FORMNO, B.FORMNAME1 FORMNAME,  B.USERFORMNO";
                SQL += ComNum.VBLF + "FROM ADMIN.EMRXML A INNER JOIN ADMIN.EMRFORM B ";
                SQL += ComNum.VBLF + "      ON A.FORMNO = B.FORMNO";
                SQL += ComNum.VBLF + "  WHERE A.FORMNO = " + strFormNo;
                SQL += ComNum.VBLF + "      AND A.PTNO = '" + strPtno + "' ";
                SQL += ComNum.VBLF + "      AND A.EMRNO = (SELECT MAX(X.EMRNO) FROM ADMIN.EMRXMLMST X";
                SQL += ComNum.VBLF + "                      WHERE X.FORMNO = " + strFormNo;
                SQL += ComNum.VBLF + "                          AND X.PTNO = '" + strPtno + "') ";

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    return;
                }

                objSpread.ActiveSheet.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    #region XML세팅
                    XmlDocument xml = new XmlDocument(); // XmlDocument 생성
                    xml.LoadXml(LoadXml(dt.Rows[i]["EMRNO"].ToString().Trim()));
                    #endregion

                    #region SetUserXmlValue
                    if (xml.DocumentElement != null)
                    {
                        foreach (XmlNode xn in xml.DocumentElement.ChildNodes)
                        {
                            SetUserXmlValue(xn, objSpread, i);
                        }
                    }
                    #endregion
                }

                dt.Dispose();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }
        }

        /// <summary>
        /// XML 불러오는 함수
        /// </summary>
        /// <param name="strEmrNo">EMRNO</param>
        /// <returns></returns>
        string LoadXml(string strEmrNo)
        {
            OracleDataReader reader = null;
            string strXml = string.Empty;

            try
            {
                string SQL = string.Empty;
                SQL = SQL + ComNum.VBLF + "SELECT CHARTXML";
                SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "EMRXML";
                SQL = SQL + ComNum.VBLF + "    WHERE EMRNO = " + VB.Val(strEmrNo);

                string SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                if (SqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strXml;
                }

                if (reader.HasRows && reader.Read())
                {
                    strXml = reader.GetValue(0).ToString().Trim();
                    return strXml;
                }

                return strXml;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                return strXml;
            }
        }

        /// <summary>
        /// XML 해당 아이템 찾아서 값 넣어주는 함수
        /// 
        /// </summary>
        /// <param name="a_objElement">XMLNODE </param>
        /// <param name="objSpread">스프레드 객체</param>
        /// <param name="lngRowX">로우</param>
        void SetUserXmlValue(XmlNode a_objElement, FarPoint.Win.Spread.FpSpread objSpread, int lngRowX)
        {
            string strItem = a_objElement.Name;
            string strValue = a_objElement.InnerText;
            int lngItemCol = 0;
            TextCellType textCellType = new TextCellType();

            for (int lngCol = 0; lngCol < ssItem_Sheet1.ColumnCount; lngCol++)
            {
                if (strItem == ssItem_Sheet1.Cells[0, lngCol + 12].Text.Trim())
                {
                    lngItemCol = lngCol;
                    break;
                }
            }

            if (lngItemCol == 0)
                return;

            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].HorizontalAlignment = ssUserChart.ActiveSheet.Cells[0, lngItemCol].HorizontalAlignment;
            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].VerticalAlignment = ssUserChart.ActiveSheet.Cells[0, lngItemCol].VerticalAlignment;
            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType = ssUserChart.ActiveSheet.Cells[0, lngItemCol].CellType;

            objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].Text = strValue;

            if (objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType != null &&
               objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType.ToString() == "TextCellType")
            {
                textCellType = (TextCellType)objSpread.ActiveSheet.Cells[lngRowX, lngItemCol].CellType;
                objSpread.ActiveSheet.Rows[lngRowX].Height = textCellType.Multiline == true ? objSpread.ActiveSheet.Rows[lngRowX].GetPreferredHeight() + 10 : 22;
            }

            textCellType.Dispose();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMedFrTime.Text))
            {
                ComFunc.MsgBoxEx(this, "시간을 입력해 주십시오");
                return;
            }

            string strChartDate = dtpChartDate.Value.ToString("yyyyMMdd");
            string strChartTime = txtMedFrTime.Text.Trim().Replace(":", "").Trim();
            string strInOutCls = "I";

            string strHead = @"<?xml version=""1.0"" encoding=""UTF-8""?>";
            string strChartX1 = "<chart>";
            string strChartX2 = "</chart>";

            clsDB.setBeginTran(clsDB.DbCon);
            try
            {

                for(int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
                {
                    if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True") == false)
                        continue;

                    for (int lngCol = mlngStartCol; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
                    {
                        if (ssUserChart_Sheet1.Cells[lngRow, lngCol].Text.Trim().Length > 0)
                        {
                            double dblEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "GetEmrXmlNo");
                            string strXML = strHead + strChartX1;
                            
                            string strPtNo = ssUserChart_Sheet1.Cells[lngRow, 3].Text.Trim();
                            string strAcpNo = ssUserChart_Sheet1.Cells[lngRow, 7].Text.Trim();
                            string strMedFrDate = ssUserChart_Sheet1.Cells[lngRow, 9].Text.Trim().Replace("-", "");
                            string strMedFrTime = ssUserChart_Sheet1.Cells[lngRow, 10].Text.Trim();
                            string strMedDeptCd = ssUserChart_Sheet1.Cells[lngRow, 11].Text.Trim();
                            string strMedDrCd = ssUserChart_Sheet1.Cells[lngRow, 12].Text.Trim();


                            #region SaveRutine
                            for (int lngCol2 = mlngStartCol; lngCol2 < ssUserChart_Sheet1.ColumnCount; lngCol2++)
                            {
                                if (lngCol2 == ssUserChart_Sheet1.ColumnCount)
                                    break;

                                string strTAGHEAD = ssFORMXML_Sheet1.Cells[0, lngCol2].Text.Trim();
                                string strTAGTAIL = ssFORMXML_Sheet1.Cells[1, lngCol2].Text.Trim();
                                strXML += strTAGHEAD + ssUserChart_Sheet1.Cells[lngRow, lngCol2].Text.Trim().Replace("'", "`") + strTAGTAIL + ComNum.VBLF;
                            }

                            strXML += strChartX2;

                            if( SaveEmrXmlData(dblEmrNo.ToString(), mstrFormNo, strChartDate, strChartTime,
                                               strAcpNo, strPtNo, strInOutCls, strMedFrDate, strMedFrTime,
                                               "", "", strMedDeptCd, strMedDrCd,
                                               strXML) == false)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                return;
                            }
                            break;
                            #endregion
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장이 완료되었습니다.");
                chkAll.Checked = false;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.setRollbackTran(clsDB.DbCon);
            }
        }


        /// <summary>
        /// 기록지 저장
        /// </summary>
        /// <param name="strEmrNo">EmrNo</param>
        /// <param name="strFormNo">폼번호</param>
        /// <param name="strUseId">작성자 아이디</param>
        /// <param name="strChartDate">작성날짜</param>
        /// <param name="strChartTime">작성시간</param>
        /// <param name="strAcpNo">Acpno</param>
        /// <param name="strPtNo">등록번호</param>
        /// <param name="strInOutCls">외래,입원 구분</param>
        /// <param name="strMedFrDate">입원날짜</param>
        /// <param name="strMedFrTime">입원시간</param>
        /// <param name="strMedEndDate">퇴원날짜</param>
        /// <param name="strMedEndTime">퇴원시간</param>
        /// <param name="strMedDeptCd">과</param>
        /// <param name="strMedDrCd">의사</param>
        /// <param name="strXML">XML Data</param>
        /// <param name="strUPDATENO">기록지 최종 업데이트번호</param>
        /// <returns></returns>
        bool SaveEmrXmlData(string strEmrNo, string strFormNo,
                            string strChartDate, string strChartTime, string strAcpNo,
                            string strPtNo, string strInOutCls, string strMedFrDate,
                            string strMedFrTime, string strMedEndDate, string strMedEndTime,
                            string strMedDeptCd, string strMedDrCd, string strXML,
                            string strUPDATENO = "1"
                            )
        {
            bool rtnVal = false;

            string writeDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            string writeTime = ComQuery.CurrentDateTime(clsDB.DbCon, "T");
            StringBuilder SQL = new StringBuilder();
            int RowAffected = 0;

            SQL.AppendLine(" INSERT INTO " + ComNum.DB_EMR + "EMRXMLMST");
            SQL.AppendLine("      ( ");
            SQL.AppendLine("      EMRNO, PTNO, GBEMR, ");
            SQL.AppendLine("      FORMNO, USEID, CHARTDATE, CHARTTIME, ");
            SQL.AppendLine("      INOUTCLS, MEDFRDATE, MEDFRTIME, MEDENDDATE, MEDENDTIME, ");
            SQL.AppendLine("      MEDDEPTCD, MEDDRCD, WRITEDATE, WRITETIME");
            SQL.AppendLine("      ) ");
            SQL.AppendLine("      VALUES (");
            SQL.AppendLine("      " + strEmrNo + ",");
            SQL.AppendLine("      '" + strPtNo + "',");
            SQL.AppendLine("      '1' ,");
            SQL.AppendLine("      " + strFormNo + ",");
            SQL.AppendLine("      '" + VB.Val(clsType.User.IdNumber) + "',");
            SQL.AppendLine("      '" + strChartDate + "',");
            SQL.AppendLine("      '" + strChartTime + "',");
            SQL.AppendLine("      '" + strInOutCls + "',");
            SQL.AppendLine("      '" + strMedFrDate + "',");
            SQL.AppendLine("      '" + strMedFrTime + "',");
            SQL.AppendLine("      '" + strMedEndDate + "',");
            SQL.AppendLine("      '" + strMedEndTime + "',");
            SQL.AppendLine("      '" + strMedDeptCd + "',");
            SQL.AppendLine("      '" + strMedDrCd + "',");
            SQL.AppendLine("      '" + writeDate + "',");
            SQL.AppendLine("      '" + writeTime + "'");
            SQL.AppendLine("      )");

            string sqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString().Trim(), ref RowAffected, clsDB.DbCon);
            if (sqlErr != "")
            {
                ComFunc.MsgBoxEx(this, sqlErr);
                clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }


            SQL.Clear();
            SQL.AppendLine(" INSERT INTO ADMIN.EMRXML");
            SQL.AppendLine("      (EMRNO,FORMNO,USEID,CHARTDATE,CHARTTIME,ACPNO,PTNO,");
            SQL.AppendLine("      INOUTCLS,MEDFRDATE,MEDFRTIME,MEDENDDATE,MEDENDTIME,MEDDEPTCD,MEDDRCD,");
            SQL.AppendLine("      WRITEDATE,WRITETIME,CHARTXML,UPDATENO) ");
            SQL.AppendLine("      VALUES (");
            SQL.AppendLine("      " + strEmrNo + ",");
            SQL.AppendLine("      " + strFormNo + ",");
            SQL.AppendLine("      '" + VB.Val(clsType.User.IdNumber) + "',");
            SQL.AppendLine("      '" + strChartDate + "',");
            SQL.AppendLine("      '" + strChartTime + "',");
            SQL.AppendLine("      '" + strAcpNo + "',");
            SQL.AppendLine("      '" + strPtNo + "',");
            SQL.AppendLine("      '" + strInOutCls + "',");
            SQL.AppendLine("      '" + strMedFrDate + "',");
            SQL.AppendLine("      '" + strMedFrTime + "',");
            SQL.AppendLine("      '" + strMedEndDate + "',");
            SQL.AppendLine("      '" + strMedEndTime + "',");
            SQL.AppendLine("      '" + strMedDeptCd + "',");
            SQL.AppendLine("      '" + strMedDrCd + "',");
            SQL.AppendLine("      '" + writeDate + "',");
            SQL.AppendLine("      '" + writeTime + "',");
            SQL.AppendLine("      :1,");
            SQL.AppendLine("      '" + strUPDATENO + "')");

            sqlErr = clsDB.ExecuteXmlQuery(SQL.ToString().Trim(), strXML, ref RowAffected, clsDB.DbCon);
            if (sqlErr.Length > 0 || RowAffected == 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
     
        }

        private void btnPrintPreView_Click(object sender, EventArgs e)
        {
            PreViewAndPrint("V");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PreViewAndPrint("P");
        }

        private void PreViewAndPrint(string PrintType)
        {
            btnPrint.Enabled = false;

            //'Print Head 지정
            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""12"" /fb0 /fi0 /fu0 /fk0 /fs2";
            string strHead1 = "/c/f1" + mstrFORMNAME + "/f1/n/n";
            string strHead2 = "/n/l/f2" + "차트일자 : " + dtpChartDate.Value.ToString("yyyy년 mm월 dd일") + " /r/f2" + "출력자 : " + clsType.User.UserName + "     /n";

            ssUserChart_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssUserChart_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssUserChart_Sheet1.PrintInfo.Margin.Left = 20;
            ssUserChart_Sheet1.PrintInfo.Margin.Right = 20;
            ssUserChart_Sheet1.PrintInfo.Margin.Top = 20;
            ssUserChart_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssUserChart_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssUserChart_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssUserChart_Sheet1.PrintInfo.ShowBorder = true;
            ssUserChart_Sheet1.PrintInfo.ShowColor = false;
            ssUserChart_Sheet1.PrintInfo.ShowGrid = true;
            ssUserChart_Sheet1.PrintInfo.ShowShadows = false;
            ssUserChart_Sheet1.PrintInfo.UseMax = false;
            ssUserChart_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssUserChart_Sheet1.PrintInfo.Orientation = mstrFormNo.Equals("1562") ? FarPoint.Win.Spread.PrintOrientation.Landscape : FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssUserChart_Sheet1.PrintInfo.Preview = PrintType.Equals("V");
            ssUserChart.PrintSheet(0);

            Application.DoEvents();

            btnPrint.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDuty_Click(object sender, EventArgs e)
        {
            string BtnText = (sender as Button).Text.Trim();
            int lngFcol = -1;

            for (int lngCol = 0; lngCol < ssUserChart_Sheet1.ColumnCount; lngCol++)
            {
                if (ssUserChart_Sheet1.Columns[lngCol].Label.ToUpper().Equals("DUTY"))
                {
                    lngFcol = lngCol;
                    break;
                }
            }

            if (lngFcol == -1)
                return;

            for (int lngRow = 0; lngRow < ssUserChart_Sheet1.RowCount; lngRow++)
            {
                if (ssUserChart_Sheet1.Cells[lngRow, 0].Text.Trim().Equals("True"))
                {
                    ssUserChart_Sheet1.Cells[lngRow, lngFcol].Text = BtnText;
                }
            }
        }

        private void ssUserChart_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (ssUserChart_Sheet1.RowCount == 0)
                return;

            if (e.Column < 1)
                return;

            if (ChkAutoWrite.Checked == false)
                return;

            switch (e.Column)
            {
                case 13:
                case 14:
                    if (mstrFormNo.Equals("1562") == false)
                    {
                        return;
                    }
                    break;
                default:
                    return;
            }

            int nSelectCol = e.Column;
            string strData = ssUserChart_Sheet1.Cells[e.Row, e.Column].Text.Trim();
            if (ssUserChart_Sheet1.Cells[e.Row, 0].Text.Trim().Equals("True"))
            {
                for(int i = 0; i < ssUserChart_Sheet1.RowCount; i++)
                {
                    if(ssUserChart_Sheet1.Cells[e.Row, 0].Text.Trim().Equals("True"))
                    {
                        ssUserChart_Sheet1.Cells[i, nSelectCol].Text = strData;
                    }
                }
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ssUserChart_Sheet1.Cells[0, 0, ssUserChart_Sheet1.RowCount - 1, 0].Text = chkAll.Checked ? "True" : "False";
        }

        private void ssUserChart_EditModeOff(object sender, EventArgs e)
        {
            if (ssUserChart_Sheet1.ActiveColumnIndex == ssUserChart_Sheet1.ColumnCount) return;

            //ssUserChart_Sheet1.ActiveColumnIndex += 1;
        }

        private void dtpChartDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
