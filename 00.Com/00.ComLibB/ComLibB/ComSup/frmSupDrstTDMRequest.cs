using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSupDrstTDMRequest : Form
    {
        #region dto 

        public class TDM_Request_Model : BaseDto
        {
            /// <summary>                          
            /// ROOMCODE
            /// </summary>                         
            private long roomcode;
            public long ROOMCODE
            {
                get { return roomcode; }
                set
                {
                    if (roomcode != value)
                    {
                        roomcode = value;
                        //OnPropertyChanged("BUN");
                    }
                }
            }


            /// <summary>                          
            /// IPDNO
            /// </summary>                         
            private long ipdno;
            public long IPDNO
            {
                get { return ipdno; }
                set
                {
                    if (ipdno != value)
                    {
                        ipdno = value;
                        //OnPropertyChanged("BUN");
                    }
                }
            }

            /// <summary>                          
            /// SEQNO
            /// </summary>                         
            private long seqno;
            public long SEQNO
            {
                get { return seqno; }
                set
                {
                    if (seqno != value)
                    {
                        seqno = value;
                        //OnPropertyChanged("BUN");
                    }
                }
            }

            /// <summary>                          
            /// BDATE
            /// </summary>                         
            private string bdate = string.Empty;
            public string BDATE
            {
                get { return bdate == null ? string.Empty : bdate; }
                set
                {
                    if (bdate != value)
                    {
                        bdate = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }


            /// <summary>                          
            /// PTNO
            /// </summary>                         
            private string ptno = string.Empty;
            public string PTNO
            {
                get { return ptno == null ? string.Empty : ptno; }
                set
                {
                    if (ptno != value)
                    {
                        ptno = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// SUBGB
            /// </summary>                         
            private string subgb = string.Empty;
            public string SUBGB
            {
                get { return subgb == null ? string.Empty : subgb; }
                set
                {
                    if (subgb != value)
                    {
                        subgb = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// SUBVAL
            /// </summary>                         
            private string subval = string.Empty;
            public string SUBVAL
            {
                get { return subval == null ? string.Empty : subval; }
                set
                {
                    if (subval != value)
                    {
                        subval = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// JEPCODE
            /// </summary>                         
            private string jepcode = string.Empty;
            public string JEPCODE
            {
                get { return jepcode == null ? string.Empty : jepcode; }
                set
                {
                    if (jepcode != value)
                    {
                        jepcode = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// WARDCODE
            /// </summary>                         
            private string wardcode = string.Empty;
            public string WARDCODE
            {
                get { return wardcode == null ? string.Empty : wardcode; }
                set
                {
                    if (wardcode != value)
                    {
                        wardcode = value;
                    }
                }
            }

            /// <summary>                          
            /// SNAME
            /// </summary>                         
            private string sname = string.Empty;
            public string SNAME
            {
                get { return sname == null ? string.Empty : sname; }
                set
                {
                    if (sname != value)
                    {
                        sname = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// DEPTCODE
            /// </summary>                         
            private string deptcode = string.Empty;
            public string DEPTCODE
            {
                get { return deptcode == null ? string.Empty : deptcode; }
                set
                {
                    if (deptcode != value)
                    {
                        deptcode = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// INDATE
            /// </summary>                         
            private string indate = string.Empty;
            public string INDATE
            {
                get { return indate == null ? string.Empty : indate; }
                set
                {
                    if (indate != value)
                    {
                        indate = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// DRNAME
            /// </summary>                         
            private string drname = string.Empty;
            public string DRNAME
            {
                get { return drname == null ? string.Empty : drname; }
                set
                {
                    if (drname != value)
                    {
                        drname = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// GBIO
            /// </summary>                         
            private string gbio = string.Empty;
            public string GBIO
            {
                get { return gbio == null ? string.Empty : gbio; }
                set
                {
                    if (gbio != value)
                    {
                        gbio = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// DRCODE
            /// </summary>                         
            private string drcode = string.Empty;
            public string DRCODE
            {
                get { return drcode == null ? string.Empty : drcode; }
                set
                {
                    if (drcode != value)
                    {
                        drcode = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// SABUN
            /// </summary>                         
            private string sabun = string.Empty;
            public string SABUN
            {
                get { return sabun == null ? string.Empty : sabun; }
                set
                {
                    if (sabun != value)
                    {
                        sabun = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }

            /// <summary>                          
            /// PROGRESS
            /// </summary>                         
            private string progress = string.Empty;
            public string PROGRESS
            {
                get { return progress == null ? string.Empty : progress; }
                set
                {
                    if (progress != value)
                    {
                        progress = value;
                        //OnPropertyChanged("EROOR");
                    }
                }
            }
        }
        #endregion

        List<TDM_Request_Model> models = null;

        /// <summary>
        /// 입원환자 IPDNO
        /// </summary>
        private long IPDNO = 0;

        /// <summary>
        /// SEQNO
        /// </summary>
        private string SEQNO = string.Empty;

        /// <summary>
        /// 선택한 항목
        /// </summary>
        TDM_Request_Model sItem = null;

        public frmSupDrstTDMRequest(long IPDNO)
        {
            this.IPDNO = IPDNO;
            InitializeComponent();
        }

        public frmSupDrstTDMRequest(string SEQNO)
        {
            this.SEQNO = SEQNO;
            InitializeComponent();
        }

        private void frmSupDrstTDMRequest_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");


            if (clsType.User.DrCode.IsNullOrEmpty())
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnNew.Enabled = false;
                btnReNew.Enabled = false;
            }
            else
            {
                btnSearch.Enabled = true;
            }

            models = new List<TDM_Request_Model>();

            //스프레드
            ssList.Initialize(new SpreadOption { RowHeaderVisible = false, SpreadSelectionUnit = SelectionUnit.Row, IsRowSelectColor = true });
            ssList.AddColumn("약품", nameof(TDM_Request_Model.JEPCODE), 120, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center });
            ssList.AddColumn("병동", nameof(TDM_Request_Model.WARDCODE), 75, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center });
            ssList.AddColumn("진료과", nameof(TDM_Request_Model.DEPTCODE), 75, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center });
            ssList.VerticalScrollBarPolicy = ScrollBarPolicy.Always;
            ssList.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;

            GetTDM();

            if (SEQNO.NotEmpty())
            {
                sItem = new TDM_Request_Model()
                {
                    SEQNO = SEQNO.To<long>(0)
                };
                GetContent();
            }

            GetIpdData(IPDNO, string.Empty, string.Empty);
            btnSearch.PerformClick();

    
        }

        private void GetTDM()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            MParameter mParameter = new MParameter();

            cboTDM.Items.Clear();

            try
            {
                #region 기초코드 쿼리 
                mParameter.AppendSql("SELECT NAME AS SNAME                      ");
                mParameter.AppendSql("  FROM KOSMOS_PMPA.BAS_BCODE              ");
                mParameter.AppendSql(" WHERE GUBUN = 'TDM_대상약물(성분명)'        ");
                mParameter.AppendSql(" ORDER BY SORT                            ");

                List<TDM_Request_Model> dt = clsDB.ExecuteReader<TDM_Request_Model>(mParameter, clsDB.DbCon);

                if (dt.Count == 0)
                    return;

                cboTDM.SetDataSource(dt, "SNAME", "SNAME", false);

                #endregion

                cboTDM.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, mParameter.NativeQuerySql, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            MParameter mParameter = new MParameter();
            models.Clear();

            try
            {
                if (IPDNO > 0)
                {
                    mParameter.AppendSql("SELECT    B.ROOMCODE                                                                                                                          ");
                    mParameter.AppendSql("	    ,   B.PANO                                                                                                                              ");
                    mParameter.AppendSql("	    ,   B.SNAME                                                                                                                             ");
                    mParameter.AppendSql("	    ,   B.DEPTCODE                                                                                                                          ");
                    mParameter.AppendSql("	    ,   (SELECT SUBVAL FROM KOSMOS_OCS.OCS_TDM_MASTERSUB WHERE SEQNO = A.SEQNO AND SUBGB = '000') AS JEPCODE                                ");
                    mParameter.AppendSql("	    ,   B.WARDCODE                                                                                                                          ");
                    mParameter.AppendSql("	    ,   A.SEQNO                                                                                                                             ");
                    mParameter.AppendSql("	    ,   TO_CHAR(A.REQDATE, 'YYYY-MM-DD') AS REQDATE                                                                                         ");
                    mParameter.AppendSql("	    ,   TO_CHAR(B.INDATE , 'YYYY-MM-DD') AS INDATE                                                                                          ");
                    mParameter.AppendSql("	    ,   :GBIO AS GBIO                                                                                                                       ");
                    mParameter.AppendSql("	    ,   (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE= B.DRCODE) AS DRNAME                                                         ");
                    mParameter.AppendSql("	    ,   (SELECT EMP_NM FROM KOSMOS_ERP.HR_EMP_BASIS WHERE EMP_ID = TRIM(A.SABUN)) AS EMP_NM                                                 ");
                    mParameter.AppendSql("	    ,   (SELECT PROGRESS FROM KOSMOS_OCS.OCS_TDM_RETURN WHERE SEQNO = A.SEQNO) AS PROGRESS                                                  ");
                    mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_MASTER A                                                                                                            ");
                    mParameter.AppendSql(" INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER B                                                                                                      ");
                    mParameter.AppendSql("    ON A.PTNO  = B.PANO                                                                                                                       ");
                    mParameter.AppendSql("   AND A.IPDNO = B.IPDNO                                                                                                                      ");
                    mParameter.AppendSql(" WHERE A.IPDNO = :IPDNO                                                                                                                       ");

                    mParameter.Add("IPDNO", IPDNO);
                }
                else
                {
                    mParameter.AppendSql("SELECT    0 AS ROOMCODE                                                                                                                       ");
                    mParameter.AppendSql("	    ,   B.PANO                                                                                                                              ");
                    mParameter.AppendSql("	    ,   B.SNAME                                                                                                                             ");
                    mParameter.AppendSql("	    ,   B.DEPTCODE                                                                                                                          ");
                    mParameter.AppendSql("	    ,   (SELECT SUBVAL FROM KOSMOS_OCS.OCS_TDM_MASTERSUB WHERE SEQNO = A.SEQNO AND SUBGB = '000') AS JEPCODE                                ");
                    mParameter.AppendSql("	    ,   '' AS WARDCODE                                                                                                                      ");
                    mParameter.AppendSql("	    ,   :GBIO AS GBIO                                                                                                                       ");
                    mParameter.AppendSql("	    ,   A.SEQNO                                                                                                                             ");
                    mParameter.AppendSql("	    ,   TO_CHAR(A.REQDATE, 'YYYY-MM-DD') AS REQDATE                                                                                         ");
                    mParameter.AppendSql("	    ,   TO_CHAR(B.BDATE , 'YYYY-MM-DD') AS INDATE                                                                                           ");
                    mParameter.AppendSql("	    ,   (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE= B.DRCODE) AS DRNAME                                                         ");
                    mParameter.AppendSql("	    ,   (SELECT EMP_NM FROM KOSMOS_ERP.HR_EMP_BASIS WHERE EMP_ID = TRIM(A.SABUN)) AS EMP_NM                                                 ");
                    mParameter.AppendSql("	    ,   (SELECT PROGRESS FROM KOSMOS_OCS.OCS_TDM_RETURN WHERE SEQNO = A.SEQNO) AS PROGRESS                                                  ");
                    mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_MASTER A                                                                                                            ");
                    mParameter.AppendSql(" INNER JOIN KOSMOS_PMPA.OPD_MASTER B                                                                                                          ");
                    mParameter.AppendSql("    ON A.PTNO    = B.PANO                                                                                                                     ");
                    mParameter.AppendSql("   AND A.REQDATE = B.BDATE                                                                                                                    ");
                    mParameter.AppendSql(" WHERE A.PTNO    = :PTNO                                                                                                                      ");
                    mParameter.AppendSql("   AND A.GBIO    = :GBIO                                                                                                                      ");

                    mParameter.Add("PTNO", clsOrdFunction.Pat.PtNo);
                }

                mParameter.Add("GBIO", IPDNO > 0 ? "I" : "O");

                models = clsDB.ExecuteReader<TDM_Request_Model>(mParameter, clsDB.DbCon);
                ssList.DataSource = models;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void GetIpdData(long IPDNO, string PTNO, string BDATE)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            MParameter mParameter = new MParameter();

            try
            {
                if (IPDNO > 0)
                {
                    mParameter.AppendSql("SELECT    B.ROOMCODE                                                                                  ");
                    mParameter.AppendSql("	    ,   B.PANO AS PTNO                                                                              ");
                    mParameter.AppendSql("	    ,   B.SNAME                                                                                     ");
                    mParameter.AppendSql("	    ,   B.DEPTCODE                                                                                  ");
                    mParameter.AppendSql("	    ,   B.WARDCODE                                                                                  ");
                    mParameter.AppendSql("	    ,   TO_CHAR(B.INDATE, 'YYYY-MM-DD') AS INDATE                                                   ");
                    mParameter.AppendSql("	    ,   (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE= B.DRCODE) AS DRNAME                 ");
                    mParameter.AppendSql("  FROM KOSMOS_PMPA.IPD_NEW_MASTER B                                                                   ");
                    mParameter.AppendSql(" WHERE B.IPDNO = :IPDNO                                                                               ");

                    mParameter.Add("IPDNO", IPDNO);
                }
                else
                {
                    mParameter.AppendSql("SELECT    0 AS ROOMCODE                                                                               ");
                    mParameter.AppendSql("	    ,   B.PANO AS PTNO                                                                              ");
                    mParameter.AppendSql("	    ,   B.SNAME                                                                                     ");
                    mParameter.AppendSql("	    ,   B.DEPTCODE                                                                                  ");
                    mParameter.AppendSql("	    ,   '' AS WARDCODE                                                                              ");
                    mParameter.AppendSql("	    ,   TO_CHAR(B.BDATE, 'YYYY-MM-DD') AS INDATE                                                    ");
                    mParameter.AppendSql("	    ,   (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE= B.DRCODE) AS DRNAME                 ");
                    mParameter.AppendSql("  FROM KOSMOS_PMPA.OPD_MASTER B                                                                       ");
                    mParameter.AppendSql(" WHERE B.PANO  = :PANO                                                                                ");
                    mParameter.AppendSql("   AND B.BDATE = TO_DATE(:BDATE, 'YYYY-MM-DD')                                                        ");

                    mParameter.Add("PANO", PTNO.NotEmpty() ? PTNO : clsOrdFunction.Pat.PtNo, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                    mParameter.Add("BDATE", PTNO.NotEmpty() ? BDATE : clsOrdFunction.GstrBDate);
                }

                TDM_Request_Model item = clsDB.ExecuteReaderSingle<TDM_Request_Model>(mParameter, clsDB.DbCon);
                if (item != null)
                {
                    ssPatient.ActiveSheet.Cells[0, 0].Text = item.ROOMCODE.ToString();
                    ssPatient.ActiveSheet.Cells[0, 1].Text = item.PTNO.Trim();
                    ssPatient.ActiveSheet.Cells[0, 2].Text = item.SNAME.Trim();
                    ssPatient.ActiveSheet.Cells[0, 3].Text = item.DEPTCODE.Trim();
                    ssPatient.ActiveSheet.Cells[0, 4].Text = item.INDATE.Trim();
                    ssPatient.ActiveSheet.Cells[0, 5].Text = item.DRNAME.Trim();
                    ssPatient.ActiveSheet.Cells[0, 6].Text = item.SABUN.Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            sItem = null;
            sItem = new TDM_Request_Model()
            {
                PTNO = ssPatient.ActiveSheet.Cells[0, 1].Text,
                SNAME = ssPatient.ActiveSheet.Cells[0, 2].Text,
                DEPTCODE = ssPatient.ActiveSheet.Cells[0, 3].Text,
                INDATE = ssPatient.ActiveSheet.Cells[0, 4].Text,
                GBIO = clsOrdFunction.Pat.IPDNO > 0 ? "I": "O",
                SEQNO = 0
            };

            txtJepReason.Clear();
            txtAucMic.Clear();
            txtInfection.Clear();
            txtPeak.Clear();
            txtReason.Clear();
            txtReturn.Clear();
            txtTrough.Clear();

            foreach(Control control in panChk.Controls)
            {
                if (control is CheckBox)
                {
                    (control as CheckBox).Checked = false;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region 필수입력 체크
            if (txtJepReason.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "해당 약품 투여 사유 필수 입력항목입니다. 입력해주세요!");
                txtJepReason.Focus();
                return;
            }

            if (txtInfection.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "감염 부위 필수 입력항목입니다. 입력해주세요!");
                txtInfection.Focus();
                return;
            }

            if (panChk.Controls.OfType<CheckBox>().Any(d=> d.Checked == true) == false)
            {
                ComFunc.MsgBoxEx(this, "의뢰사유 필수 입력항목입니다. 체크해주세요!");
                panChk.Focus();
                return;
            }

            if (txtReason.Text.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "의뢰 사유 상세 설명 필수 입력항목입니다. 입력해주세요!");
                txtReason.Focus();
                return;
            }

            if (sItem == null)
            {
                sItem = new TDM_Request_Model()
                {
                    PTNO = ssPatient.ActiveSheet.Cells[0, 1].Text,
                    SNAME = ssPatient.ActiveSheet.Cells[0, 2].Text,
                    DEPTCODE = ssPatient.ActiveSheet.Cells[0, 3].Text,
                    INDATE = ssPatient.ActiveSheet.Cells[0, 4].Text,
                    GBIO = clsOrdFunction.Pat.IPDNO > 0 ? "I" : "O",
                    SEQNO = 0
                };
            }
            #endregion

            //시퀀스
            sItem.SEQNO = sItem.SEQNO == 0 ? GetSequencesNo() : sItem.SEQNO;            
            if (SaveMstData(sItem).Result != ResultType.Success)
            {
                ComFunc.MsgBoxEx(this, "저장 실패!", "오류");
                return;
            }
            else
            {
                if (SaveSubData(sItem.SEQNO).Result != ResultType.Success)
                {
                    ComFunc.MsgBoxEx(this, "저장 실패!", "오류");
                }
                else
                {
                    ComFunc.MsgBoxEx(this, "저장하였습니다.", "오류");
                }
            }

            GetData();
        }

        private long GetSequencesNo()
        {
            MParameter mParameter = new MParameter();

            mParameter.AppendSql("SELECT KOSMOS_OCS.SEQ_OCS_TDMSEQNO.NEXTVAL AS SEQNO");
            mParameter.AppendSql("  FROM DUAL");

            return clsDB.ExecuteScalar<long>(mParameter, clsDB.DbCon);
        }


        private MTSResult SaveMstData(TDM_Request_Model item)
        {
            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {
                mParameter.AppendSql("	  MERGE INTO KOSMOS_OCS.OCS_TDM_MASTER D			 ");
                mParameter.AppendSql("	  USING DUAL			                             ");
                mParameter.AppendSql("	     ON (D.SEQNO = :SEQNO)                           ");
                mParameter.AppendSql("	   WHEN MATCHED THEN                                 ");
                mParameter.AppendSql("	     UPDATE                                          ");
                mParameter.AppendSql("	        SET CREATEDATE = SYSDATE                     ");

                mParameter.AppendSql("	   WHEN NOT MATCHED THEN                             ");
                mParameter.AppendSql("	  INSERT 				                             ");
                mParameter.AppendSql("	  (                            				         ");
                mParameter.AppendSql("	      SEQNO											 ");
                mParameter.AppendSql("	    , JEPCODE                                        ");
                mParameter.AppendSql("	    , PTNO                                           ");
                mParameter.AppendSql("	    , SABUN                                          ");
                mParameter.AppendSql("	    , IPDNO                                          ");
                mParameter.AppendSql("	    , GBIO                                           ");
                mParameter.AppendSql("	    , REQDATE                                        ");
                mParameter.AppendSql("	    , CREATEDATE                                     ");
                mParameter.AppendSql("	  )                            				         ");

                mParameter.AppendSql("	    VALUES                                           ");
                mParameter.AppendSql("	  (                            				         ");
                mParameter.AppendSql("	         :SEQNO										 ");
                mParameter.AppendSql("	       , :JEPCODE                                    ");
                mParameter.AppendSql("	       , :PTNO                                       ");
                mParameter.AppendSql("	       , :SABUN                                      ");
                mParameter.AppendSql("	       , :IPDNO                                      ");
                mParameter.AppendSql("	       , :GBIO                                       ");
                mParameter.AppendSql("	       , :REQDATE                                    ");
                mParameter.AppendSql("	       , SYSDATE                                     ");
                mParameter.AppendSql("	  )                            				         ");

                mParameter.Add("SEQNO",   item.SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                mParameter.Add("JEPCODE", item.JEPCODE, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                mParameter.Add("PTNO",    item.PTNO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                mParameter.Add("SABUN",   clsType.User.Sabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                mParameter.Add("IPDNO",   IPDNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                mParameter.Add("GBIO", IPDNO > 0 ? "I" : "O", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);
                mParameter.Add("REQDATE", clsOrdFunction.GstrBDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date);

                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));
                result.SetSuccessMessage("성공");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            return result;
        }

        private MTSResult SaveSubData(long SEQNO)
        {
            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_MASTERSUB			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                foreach (Control item in ComFunc.GetAllControls(panControl).OfType<Control>().Where(d => d.Tag.NotEmpty()))
                {
                    string VALGB = item.Tag.ToString();
                    string CONTENT = string.Empty;

                    //약제 회신내용 저장X 보여주기만함.
                    if (VALGB.Equals("V13"))
                        continue;


                    if (item is CheckBox)
                    {
                        CONTENT = (item as CheckBox).Checked.ToString();
                    }
                    else if (item is RadioButton)
                    {
                        CONTENT = (item as CheckBox).Checked.ToString();
                    }
                    else if (item is TextBox || item is ComboBox)
                    {
                        CONTENT = item.Text.Trim();
                        if (CONTENT.IsNullOrEmpty())
                            continue;

                    }

                    mParameter = new MParameter();
                    mParameter.AppendSql("	  INSERT INTO KOSMOS_OCS.OCS_TDM_MASTERSUB			 ");
                    mParameter.AppendSql("	  (                            				         ");
                    mParameter.AppendSql("	      SEQNO											 ");
                    mParameter.AppendSql("	    , SUBGB                                          ");
                    mParameter.AppendSql("	    , SUBVAL                                         ");
                    mParameter.AppendSql("	  )                            				         ");

                    mParameter.AppendSql("	  SELECT :SEQNO										 ");
                    mParameter.AppendSql("	       , :SUBGB                                      ");
                    mParameter.AppendSql("	       , :SUBVAL                                     ");
                    mParameter.AppendSql("	    FROM DUAL                                        ");

                    mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                    mParameter.Add("SUBGB", VALGB, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);
                    mParameter.Add("SUBVAL", CONTENT, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);

                    result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));
                }

                result.SetSuccessMessage("성공");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            return result;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return;

            if (sItem == null)
            {
                return;
            }

            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {
                mParameter = new MParameter();
                mParameter.AppendSql("SELECT 1                                       ");
                mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_MASTER  			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");
                mParameter.AppendSql("   AND SABUN = :SABUN						     ");

                mParameter.Add("SEQNO", sItem.SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                mParameter.Add("SABUN", clsType.User.IdNumber, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);

                if (clsDB.ExecuteScalar<int>(mParameter, clsDB.DbCon) == 0)
                {
                    result.SetErrMessage("작성자가 다릅니다 삭제 할 수 없습니다.");
                    return;
                }

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_MASTER  			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", sItem.SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_MASTERSUB			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", sItem.SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                result.SetSuccessMessage("삭제하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            result.ShowMessage();
            GetData();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssList_CellClick(object sender, CellClickEventArgs e)
        {
            if (models.Count == 0)
                return;

            sItem = null;
            if (ssList.GetRowData(e.Row) is TDM_Request_Model)
            {
                sItem = ssList.GetRowData(e.Row) as TDM_Request_Model;
                GetContent();
            }
        }

        private void GetContent()
        {
            #region 데이타
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

 
            List<Control> controls = ComFunc.GetAllControls(panControl).OfType<Control>().Where(d => d.Tag.NotEmpty()).ToList();

            foreach(Control control in controls)
            {
                if (control is CheckBox)
                {
                    (control as CheckBox).Checked = false;
                }
                else if (control is RadioButton)
                {
                    (control as RadioButton).Checked = false;
                }
                else if (control is TextBox || control is ComboBox)
                {
                    control.Text = string.Empty;
                }
            }

            try
            {
                MParameter mParameter = new MParameter();

                mParameter.AppendSql("SELECT  COALESCE((SELECT NAME FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN = 'TDM진행상황' AND CODE = SUB.PROGRESS), '미확인')                ");
                mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_RETURN SUB                                                                                          ");
                mParameter.AppendSql(" WHERE SUB.SEQNO = :SEQNO 		                                                                                            ");

                mParameter.Add("SEQNO", sItem.SEQNO);

                string PROGRESS = clsDB.ExecuteScalar<string>(mParameter, clsDB.DbCon);
                if (PROGRESS.NotEmpty())
                {
                    ssPatient_Sheet1.Cells[0, 7].Text = PROGRESS;
                }

                mParameter = new MParameter();

                mParameter.AppendSql("SELECT IPDNO, PTNO, TO_CHAR(REQDATE, 'YYYY-MM-DD') BDATE                                                                      ");
                mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_MASTER                                                                                              ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO 		                                                                                                ");

                mParameter.Add("SEQNO", sItem.SEQNO);

                TDM_Request_Model item2 = clsDB.ExecuteReaderSingle<TDM_Request_Model>(mParameter, clsDB.DbCon);
                if (item2.NotEmpty())
                {                    
                    GetIpdData(item2.IPDNO, item2.PTNO, item2.BDATE);
                }

                mParameter = new MParameter();
                mParameter.AppendSql("SELECT    SUBGB                                                                                     ");
                mParameter.AppendSql("	    ,   SUBVAL                                                                                    ");
                mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_MASTERSUB A                                                               ");
                mParameter.AppendSql(" WHERE A.SEQNO = :SEQNO                                                                             ");
                mParameter.AppendSql("   AND A.SUBGB NOT IN ('V13') -- 약제 회신내용                                                        ");

                mParameter.AppendSql(" UNION ALL                                                                                          ");
                mParameter.AppendSql("SELECT    SUBGB                                                                                     ");
                mParameter.AppendSql("	    ,   SUBVAL                                                                                    ");
                mParameter.AppendSql("  FROM KOSMOS_OCS.OCS_TDM_RETURNSUB A                                                               ");
                mParameter.AppendSql(" WHERE A.SEQNO = :SEQNO                                                                             ");
                mParameter.AppendSql("   AND A.SUBGB IN ('V13') -- 약제 회신내용                                                            ");
                mParameter.AppendSql(" ORDER BY SUBGB                                                                                     ");

                mParameter.Add("SEQNO", sItem.SEQNO);

                List<TDM_Request_Model> DataList = clsDB.ExecuteReader<TDM_Request_Model>(mParameter, clsDB.DbCon);
                if (DataList == null || DataList?.Count == 0)
                    return;

                ssPatient_Sheet1.Cells[0, 6].Text = "Y";

                foreach (TDM_Request_Model item in DataList)
                {
                    if (controls.Any(d => d.Tag.ToString().Equals(item.SUBGB)) == false)
                        continue;

                    Control control = controls.Where(d => d.Tag.ToString().Equals(item.SUBGB)).FirstOrDefault();

                    if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = item.SUBVAL.To<bool>();
                    }
                    else if (control is RadioButton)
                    {
                        (control as RadioButton).Checked = item.SUBVAL.To<bool>();
                    }
                    else if (control is TextBox || control is ComboBox)
                    {
                        control.Text = item.SUBVAL;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
            #endregion
        }

        private void btnSearchReturn_Click(object sender, EventArgs e)
        {
            if (sItem == null)
            {
                ComFunc.MsgBoxEx(this, "의뢰서를 선택해주세요!");
                return;
            }

            using (frmSupDrstTDMReturn frm = new frmSupDrstTDMReturn(sItem.SEQNO.ToString()))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }
        }

        private void btnReNew_Click(object sender, EventArgs e)
        {
            sItem = null;
            sItem = new TDM_Request_Model()
            {
                PTNO = ssPatient.ActiveSheet.Cells[0, 1].Text,
                SNAME = ssPatient.ActiveSheet.Cells[0, 2].Text,
                DEPTCODE = ssPatient.ActiveSheet.Cells[0, 3].Text,
                INDATE = ssPatient.ActiveSheet.Cells[0, 4].Text,
                GBIO = clsOrdFunction.Pat.IPDNO > 0 ? "I" : "O",
                SEQNO = 0
            };

            txtReturn.Clear();
            txtReason.Clear();

            foreach (Control control in panChk.Controls)
            {
                if (control is CheckBox)
                {
                    (control as CheckBox).Checked = false;
                }
            }
        }

        private void cboTDM_SelectedIndexChanged(object sender, EventArgs e)
        {
            PanConcentration.Visible = !cboTDM.Text.Trim().Equals("Vancomycin");

            if (PanConcentration.Visible == false)
            {
                panel18.Height = 187;
            }
            else
            {
                panel18.Height = 320;
            }
        }
    }
}
