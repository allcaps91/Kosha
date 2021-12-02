using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComBase;

namespace ComEmrBase
{
    public partial class frmNursingRecord : Form, EmrChartForm
    {

        #region // 상용구 관련 모듈
        private Control mControl = null;
        private frmEmrMacrowordProg frmMacrowordProgEvent;
        //Form mCallForm = null;
        #endregion

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "간호기록지";
        public EmrPatient p = null;
        public string mstrEmrNo = "0";
        public string mstrMode = "W";

        public string mstrUserGb = "";

        #endregion

        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region // 폼에 사용하는 변수를 코딩하는 부분
        int nImage;
        int nSelectedImage;
        //int nImageSaved;
        //int nSelectedImageSaved;

        string mstrXML = "";

        #endregion


        #region // TopMenu관련 이벤트 처리 함수

        private void usFormTopMenuEvent_SetTimeCheckShow(ComboBox mkText)
        {
            mMaskBox = mkText;
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usTimeSetEvent = new usTimeSet();
            usTimeSetEvent.rSetTime += new usTimeSet.SetTime(usTimeSetEvent_SetTime);
            usTimeSetEvent.rEventClosed += new usTimeSet.EventClosed(usTimeSetEvent_EventClosed);
            this.Controls.Add(usTimeSetEvent);
            usTimeSetEvent.Top = mMaskBox.Top - 5;
            usTimeSetEvent.Left = mMaskBox.Left;
            usTimeSetEvent.BringToFront();
        }

        private void usTimeSetEvent_SetTime(string strText)
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
            usFormTopMenuEvent.txtMedFrTime.Text = strText;
        }

        private void usTimeSetEvent_EventClosed()
        {
            if (usTimeSetEvent != null)
            {
                usTimeSetEvent.Dispose();
                usTimeSetEvent = null;
            }
        }

        private void usFormTopMenuEvent_SetSave(string strFrDate, string strFrTime)
        {
            pSaveData();
        }
        private void usFormTopMenuEvent_SetDel(string strFrDate, string strFrTime)
        {
            pDelData();
        }
        private void usFormTopMenuEvent_SetClear()
        {
            mstrEmrNo = "0";
            pClearForm();
        }
        private void usFormTopMenuEvent_SetPrint()
        {
            pPrintForm();
        }
        private void usFormTopMenuEvent_EventClosed()
        {
            //아무것도 하지 않는다.
        }

        #endregion

        #region //EmrChartForm

        public void SetChartHisMsg(string strEmrNo, string strOldGb)
        {
            return;
        }
        public double SaveDataMsg(string strFlag)
        {
            double dblEmrNo = 0;
            //dblEmrNo = pSaveData(strFlag);
            return dblEmrNo;
        }

        public bool DelDataMsg()
        {
            return pDelData();
        }

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
            if (strPRINTFLAG == "N")
            {
                frmEmrPrintOption frmEmrPrintOptionX = new frmEmrPrintOption();
                frmEmrPrintOptionX.ShowDialog();
            }

            if (clsFormPrint.mstrPRINTFLAG == "-1")
            {
                return rtnVal;
            }

            if (clsEmrQuery.SaveEmrXmlPrnYnForm(clsDB.DbCon, mstrEmrNo, "0") == false)
            {
                return rtnVal;
            }

            rtnVal = clsFormPrint.PrintFormLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }

        public int ToImageFormMsg(string strPRINTFLAG)
        {
            int rtnVal = clsFormPrint.PrintToTifFileLong(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, mstrEmrNo, panChart, "C");
            return rtnVal;
        }
        #endregion //EmrChartForm

        #region //외부에서 이벤트 받아서 처리 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 환자 받아서 기록지를 초기화 한다.
        /// </summary>
        public void gPatientinfoRecive(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            //폼을 클리어하고 기록지 작성 정보등을 갱신한다.
            mstrEmrNo = "0";
            pClearForm();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;

            //기록지 정보를 세팅한다.
            pSetEmrInfo();

            if (VB.Val(mstrEmrNo) > 0)
            {
                // 내원일을 세팅한다.
                clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);
            }
            QueryChartList();
        }

        /// <summary>
        /// 폼이 로드할때 초기 세팅을 한다
        /// </summary>
        public void pInitForm()
        {
            //텍스트 박스에 상용구 이벤트를 세팅한다

            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------

            pClearForm();
            pSetEmrInfo();
        }

        private void pSetEmrInfo()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);

            //권한에 따라서 버튼을 세팅을 한다. 
            clsEmrFunc.usBtnShowReg(usFormTopMenuEvent);
            usFormTopMenuEvent.mbtnPrint.Visible = false;

            //EMRNO가 있으면 기록 정보를 세팅을 한다.
            pLoadEmrChartInfo();

        }

        /// <summary>
        /// 폼별 특수한 초기화세팅이 필요할 경우 코딩.
        /// </summary>
        public void pInitFormSpc()
        {

        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return 0;
            }
            
            if (VB.Val(mstrEmrNo) != 0)
            {
                if (ComFunc.MsgBoxQ("기존 내용을 변경하시겠습니까?", "EMR", MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    return dblEmrNo;
                }
            }

            dblEmrNo = pSaveEmrData();
            if (dblEmrNo == 0)
            {
                ComFunc.MsgBoxEx(this, "저장중 에러가 발생했습니다.");
            }
            else
            {
                //ComFunc.MsgBoxEx(this, "저장하였습니다.");
                //mstrEmrNo = Convert.ToString(dblEmrNo);
                //pSetEmrInfo();
                mstrEmrNo = "0";
                pClearForm();
                GetJinDanStsInfo("1");
                QueryChartList();
            }
            return dblEmrNo;
        }

        #endregion


        //==========================================================================================//
        //=============================== 아래부터 코딩을 하면 됨 ===================================//
        //=========================================================================================//

        #region // 기록지 클리어, 저장, 삭제, 프린터
        /// <summary>
        /// 화면 정리
        /// </summary>
        public void pClearForm()
        {
            //모든 컨트롤을 초기화 한다.
            //ComFunc.SetAllControlClearEx(this);
            //시간 세팅을 한다.
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(VB.Left(strCurDateTime, 8), "D"));
            usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(VB.Mid(strCurDateTime, 9, 4), "M");
            usFormTopMenuEvent.dtMedFrDate.Enabled = true;
            usFormTopMenuEvent.txtMedFrTime.Enabled = true;
            usFormTopMenuEvent.mbtnTime.Visible = true;
            //자격에따라서 버튼을 설정을 한다.
            pClearFormExcept();
        }

        /// <summary>
        /// 폼별로 EMR 작성 내역을 화면에 보여준다.
        /// </summary>
        private void pLoadEmrChartInfo()
        {
            //mstrEmrNo = "1455366";
            if (VB.Val(mstrEmrNo) == 0)
            {
                return;
            }
            //출력한 기록지는 수정이 안되도록 막는다.
            //bool blnPRNYN = clsEmrQuery.GetPrnYnInfo(mstrEmrNo);
            //if (blnPRNYN == true)
            //{
            //    clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            //}

            LoadNrRecord();
            //clsXML.LoadDataXML(this, mstrEmrNo, false);
        }

        private void LoadNrRecord()
        {
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            txtNarration.Text = "";
            txtNarrationEmrNo.Text = "";
            txtDAEmrNo.Text = "";
            txtCHARTDATE.Text = "";
            txtCHARTTIME.Text = "";
            txtUSEID.Text = "";
            mstrUserGb = "";

            SSTab1.SelectedIndex = 0;
            tabNr.SelectedIndex = 0;

            chkD.Checked = false;
            chkA.Checked = false;
            chkRe.Checked = false;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT A.EMRNO,C.USEID, A.JINDANNO,A.MACRONO,A.USERGB,A.DELCLS, A.JINDANTITLE, A.TYPE, A.NRRECORD AS CONTENT, B.DISPSEQ";
            SQL = SQL + ComNum.VBLF + "      , C.CHARTDATE, C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNURSRECORD A";
            SQL = SQL + ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_EMR + "EMRNRMACRO B";
            SQL = SQL + ComNum.VBLF + "       ON A.MACRONO = B.MACRONO";
            SQL = SQL + ComNum.VBLF + "       AND A.USERGB = B.USERGB";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMRXML C";
            SQL = SQL + ComNum.VBLF + "       ON C.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "  WHERE A.EMRNO =" + mstrEmrNo;
            SQL = SQL + ComNum.VBLF + "  AND A.TYPE ='D'";
            SQL = SQL + ComNum.VBLF + "  AND A.DELCLS ='1'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {

                txtJindanNo.Text = dt.Rows[0]["JINDANNO"].ToString().Trim();
                txtNrJindan.Text = dt.Rows[0]["JINDANTITLE"].ToString().Trim();
                txtDAEmrNo.Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                txtCHARTDATE.Text = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                txtCHARTTIME.Text = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                txtUSEID.Text = dt.Rows[0]["USEID"].ToString().Trim();
                mstrUserGb = dt.Rows[0]["USERGB"].ToString().Trim();

                ssData_Sheet1.RowCount = dt.Rows.Count;
                ssData_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DELCLS"].ToString().Trim() == "1")
                    {
                        ssData_Sheet1.Cells[i, 0].Value = true;
                    }
                    ssData_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    ssData_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
                    ssData_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MACRONO"].ToString().Trim();
                    ssData_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                    ssData_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                }
                tabNr.SelectedIndex = 0;
            }
            dt.Dispose();
            dt = null;

            //QueryAction
            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT A.EMRNO,C.USEID, A.JINDANNO,A.MACRONO,A.USERGB,A.DELCLS, A.JINDANTITLE, A.TYPE, A.NRRECORD AS CONTENT, B.DISPSEQ";
            SQL = SQL + ComNum.VBLF + "      , C.CHARTDATE, C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNURSRECORD A";
            SQL = SQL + ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_EMR + "EMRNRMACRO B";
            SQL = SQL + ComNum.VBLF + "       ON A.MACRONO = B.MACRONO";
            SQL = SQL + ComNum.VBLF + "       AND A.USERGB = B.USERGB";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMRXML C";
            SQL = SQL + ComNum.VBLF + "       ON C.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "  WHERE A.EMRNO =" + mstrEmrNo;
            SQL = SQL + ComNum.VBLF + "  AND A.TYPE ='A'";
            SQL = SQL + ComNum.VBLF + "  AND A.DELCLS ='1'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {

                txtJindanNo.Text = dt.Rows[0]["JINDANNO"].ToString().Trim();
                txtNrJindan.Text = dt.Rows[0]["JINDANTITLE"].ToString().Trim();
                txtDAEmrNo.Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                txtCHARTDATE.Text = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                txtCHARTTIME.Text = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                txtUSEID.Text = dt.Rows[0]["USEID"].ToString().Trim();
                mstrUserGb = dt.Rows[0]["USERGB"].ToString().Trim();

                ssAction_Sheet1.RowCount = dt.Rows.Count;
                ssAction_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DELCLS"].ToString().Trim() == "1")
                    {
                        ssAction_Sheet1.Cells[i, 0].Value = true;
                    }
                    ssAction_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    ssAction_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
                    ssAction_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MACRONO"].ToString().Trim();
                    ssAction_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                    ssAction_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

            //QueryResponse
            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT A.EMRNO,C.USEID, A.JINDANNO,A.MACRONO,A.USERGB,A.DELCLS, A.JINDANTITLE, A.TYPE, A.NRRECORD AS CONTENT, B.DISPSEQ";
            SQL = SQL + ComNum.VBLF + "      , C.CHARTDATE, C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNURSRECORD A";
            SQL = SQL + ComNum.VBLF + "   LEFT OUTER JOIN " + ComNum.DB_EMR + "EMRNRMACRO B";
            SQL = SQL + ComNum.VBLF + "       ON A.MACRONO = B.MACRONO";
            SQL = SQL + ComNum.VBLF + "       AND A.USERGB = B.USERGB";
            SQL = SQL + ComNum.VBLF + "   INNER JOIN " + ComNum.DB_EMR + "EMRXML C";
            SQL = SQL + ComNum.VBLF + "       ON C.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "  WHERE A.EMRNO =" + mstrEmrNo;
            SQL = SQL + ComNum.VBLF + "  AND A.TYPE ='R'";
            SQL = SQL + ComNum.VBLF + "  AND A.DELCLS ='1'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {

                txtJindanNo.Text = dt.Rows[0]["JINDANNO"].ToString().Trim();
                txtNrJindan.Text = dt.Rows[0]["JINDANTITLE"].ToString().Trim();
                txtDAEmrNo.Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                txtCHARTDATE.Text = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                txtCHARTTIME.Text = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                txtUSEID.Text = dt.Rows[0]["USEID"].ToString().Trim();
                mstrUserGb = dt.Rows[0]["USERGB"].ToString().Trim();

                ssResponse_Sheet1.RowCount = dt.Rows.Count;
                ssResponse_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["DELCLS"].ToString().Trim() == "1")
                    {
                        ssResponse_Sheet1.Cells[i, 0].Value = true;
                    }
                    ssResponse_Sheet1.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                    ssResponse_Sheet1.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
                    ssResponse_Sheet1.Cells[i, 3].Text = dt.Rows[i]["MACRONO"].ToString().Trim();
                    ssResponse_Sheet1.Cells[i, 4].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                    ssResponse_Sheet1.Cells[i, 5].Text = dt.Rows[i]["EMRNO"].ToString().Trim();
                }
                tabNr.SelectedIndex = 1;
            }
            dt.Dispose();
            dt = null;

            //QueryNa
            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT A.EMRNO,C.USEID, A.JINDANNO,A.MACRONO,A.DELCLS, A.JINDANTITLE, A.TYPE, A.NRRECORD AS CONTENT";
            SQL = SQL + ComNum.VBLF + "      , C.CHARTDATE, C.CHARTTIME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNURSRECORD A";
            SQL = SQL + ComNum.VBLF + " INNER JOIN " + ComNum.DB_EMR + "EMRXML C";
            SQL = SQL + ComNum.VBLF + "     ON C.EMRNO = A.EMRNO";
            SQL = SQL + ComNum.VBLF + "     WHERE A.EMRNO =" + mstrEmrNo;
            SQL = SQL + ComNum.VBLF + "  AND A.TYPE ='N'";
            SQL = SQL + ComNum.VBLF + "  AND A.DELCLS ='1'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                txtNarration.Text = "";
                txtNarrationEmrNo.Text = "";
            }
            else
            {
                txtNarration.Text = dt.Rows[0]["CONTENT"].ToString().Trim();
                txtNarrationEmrNo.Text = dt.Rows[0]["EMRNO"].ToString().Trim();
                txtJindanNo.Text = dt.Rows[0]["JINDANNO"].ToString().Trim();
                txtNrJindan.Text = dt.Rows[0]["JINDANTITLE"].ToString().Trim();
                txtCHARTDATE.Text = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                txtCHARTTIME.Text = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                txtUSEID.Text = dt.Rows[0]["USEID"].ToString().Trim();
                dt.Dispose();
                dt = null;
                tabNr.SelectedIndex = 2;
            }

            //'진행중인 간호진단이 있는지 확인
            //'진행중 = 2
            //'완료 = 1
            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT JINDANNO, STATUS ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRJINDANSTATUS ";
            SQL = SQL + ComNum.VBLF + " WHERE PTNO =  '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + p.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND MEDDEPTCD = '" + p.medDeptCd + "'";
            SQL = SQL + ComNum.VBLF + "  AND INOUTCLS ='" + p.inOutCls + "'";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                if (txtCHARTDATE.Text.Trim() != "")
                {
                    usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(txtCHARTDATE.Text.Trim(), "D"));
                    usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(txtCHARTTIME.Text.Trim(), "T");
                }
                return;
            }

            if (dt.Rows[0]["STATUS"].ToString().Trim() == "1")
            {
                optIng.Checked = true;
            }
            else if (dt.Rows[0]["STATUS"].ToString().Trim() == "2")
            {
                optEnd.Checked = true;
            }
            dt.Dispose();
            dt = null;

            SSTab1.SelectedIndex = 1;

            if (txtCHARTDATE.Text.Trim() != "")
            {
                usFormTopMenuEvent.dtMedFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(txtCHARTDATE.Text.Trim(), "D"));
                usFormTopMenuEvent.txtMedFrTime.Text = ComFunc.FormatStrToDate(txtCHARTTIME.Text.Trim(), "T");
                usFormTopMenuEvent.dtMedFrDate.Enabled = false;
                SSTab1.Enabled = false;
            }
        }
        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        private double pSaveEmrData()
        {
            double dblEmrNo = 0;
            //double dblEmrCertNo = 0;

            string strChartDate = VB.Format(usFormTopMenuEvent.dtMedFrDate.Value, "yyyyMMdd");
            string strChartTime = VB.Replace(usFormTopMenuEvent.txtMedFrTime.Text, ":", "", 1, -1);
            if (strChartTime.Length < 6)
            {
                strChartTime = strChartTime + "00";
            }

            mstrXML = "";

            dblEmrNo = pSaveNrRecord(strChartDate, strChartTime);
            if (dblEmrNo != 0)
            {
                //TODO 전자인증
                //if (clsType.gHosInfo.strEmrCertUseYn == "1")
                //{
                //    dblEmrCertNo = clsEmrCerti.SaveEmrCert(Convert.ToString(dblEmrNo), mstrEmrNo, mstrXML, strChartDate, strChartTime);
                //    if (dblEmrCertNo == 0)
                //    {
                //        ComFunc.MsgBoxEx(this, "인증중 에러가 발생했습니다." + ComNum.VBLF + "추후 인증을 실시해 주시기 바랍니다.");
                //    }
                //}
            }
            return dblEmrNo;
        }

        private double pSaveNrRecord(string strChartDate, string strChartTime)
        {
            string strNrRecore = "";
            string strXML = "";
            string strXmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + "\r\n" + "<chart>" + "\r\n";
            string strXmlTail = "\r\n" + "</chart>";

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            bool bloNarrationCheck = false;
            bool bloJindan = false;
            string strStatusNo = "";
            string strUserGb = "";
            double dblEmrNoOld = 0;
            double dblEmrNoNew = 0;

            bool blnD = false;
            bool blnA = false;
            bool blnR = false;
            string checkStatus = "0";
            string strJindanno = "";
            string strMacrono = "";
            string strJINTITLE = "";
            string strContent = "";
            bool newYnPart = true;
            string strType = "";

            if (txtUSEID.Text.Trim() != "")
            {
                if (txtUSEID.Text.Trim() != clsType.User.IdNumber)
                {
                    ComFunc.MsgBoxEx(this, "수정할 권한이 없습니다");
                    return 0;
                }
            }

            if (txtNarration.Text.Trim() != "")
            {
                if ((txtNarration.Text.Trim() != "") || (txtJindanNo.Text.Trim() == "9999999999999"))
                {
                    bloNarrationCheck = true;
                }
            }
            //
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);

                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                 
                EmrPatient pTmp = null;
                pTmp = clsEmrChart.ClearPatient();

                if (VB.Val(mstrEmrNo) == 0)
                {
                    pTmp = p;
                }
                else
                {
                    pTmp = clsEmrChart.SetEmrPatInfoXml(clsDB.DbCon, mstrEmrNo);
                }

                if (bloNarrationCheck == false)
                {
                    //진행중인 간호진단이 있는지 확인
                    dt = getPatJindan(pTmp, txtJindanNo.Text.Trim());

                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            bloJindan = true;
                            strStatusNo = dt.Rows[0]["STATUSNO"].ToString().Trim();
                        }
                        dt.Dispose();
                        dt = null;
                    }

                    if (bloJindan == false)
                    {
                        if (tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)
                        {
                            for (i = 0; i < ssData_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssData_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                            for (i = 0; i < ssAction_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssAction_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                            for (i = 0; i < ssResponse_Sheet1.RowCount; i++)
                            {
                                if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true)
                                {
                                    strUserGb = ssResponse_Sheet1.Cells[i, 4].Text.Trim();
                                }
                            }
                        }
                        SQL = "SELECT mhemr.emrnrjindanstatus_seq.nextval as STATUSNO FROM DUAL ";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return 0;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            dt.Dispose();
                            dt = null;
                            return 0;
                        }
                        strStatusNo = dt.Rows[0]["STATUSNO"].ToString().Trim();
                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + " INSERT INTO " + ComNum.DB_EMR + "EMRNRJINDANSTATUS (PTNO,STATUSNO,JINDANNO,MEDFRDATE,MEDENDDATE,MEDDEPTCD,MEDDRCD,INOUTCLS,STATUS,STARTDATE,ENDDATE,USEID,DELCLS,WRITEDATE,WRITETIME, USERGB)";
                        SQL = SQL + ComNum.VBLF + " VALUES('" + pTmp.ptNo + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + strStatusNo + "', ";
                        SQL = SQL + ComNum.VBLF + "" + VB.Val(txtJindanNo.Text.Trim()) + ", ";
                        SQL = SQL + ComNum.VBLF + "'" + pTmp.medFrDate + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + pTmp.medEndDate + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + pTmp.medDeptCd + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + pTmp.medDrCd + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + pTmp.inOutCls + "', ";
                        SQL = SQL + ComNum.VBLF + "'1', ";
                        SQL = SQL + ComNum.VBLF + "'" + VB.Left(strCurDateTime, 8) + "', ";
                        SQL = SQL + ComNum.VBLF + "'', ";
                        SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "', ";
                        SQL = SQL + ComNum.VBLF + " '0',";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Left(strCurDateTime, 8) + "',";
                        SQL = SQL + ComNum.VBLF + " '" + VB.Right(strCurDateTime, 6) + "',";
                        SQL = SQL + ComNum.VBLF + " '" + strUserGb + "')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return 0;
                        }
                    }
                }

                if (tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)
                {
                    //DATA/ACTION 리절트가  동시저장이 될수도 있고 데이타 액션만 저장될수도 있음
                    dblEmrNoOld = VB.Val(mstrEmrNo);
                    dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetEmrXmlNo");

                    if (dblEmrNoOld != 0)
                    {
                        //'기존 DAR 차팅이 있는지 확인하고 기존 내용을 DELCLS = '0'으로 한다
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT TYPE ";
                        SQL = SQL + ComNum.VBLF + "    FROM   " + ComNum.DB_EMR + "EMRNURSRECORD";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "  GROUP BY TYPE";
                        SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return 0;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Rows[i]["TYPE"].ToString().Trim() == "D")
                                {
                                    blnD = true;
                                }
                                else if (dt.Rows[i]["TYPE"].ToString().Trim() == "A")
                                {
                                    blnA = true;
                                }
                                else if (dt.Rows[i]["TYPE"].ToString().Trim() == "R")
                                {
                                    blnR = true;
                                }
                            }
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    

                    if (dblEmrNoOld != 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "      AND TYPE IN ('D','A','R')";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return 0;
                        }
                    }

                    //DATA
                    if (ssData_Sheet1.RowCount > 0)
                    {
                        strType = "D";
                        if (blnD == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssData_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssData_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssData_Sheet1.Cells[i, 0].Value) == true && ssData_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                checkStatus = "1";
                                strJindanno = txtJindanNo.Text.Trim();
                                strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssData_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssData_Sheet1.Cells[i, 1].Text.Trim();
                                //if (ssData_Sheet1.Cells[i, 5].Text.Trim() == "")
                                //{
                                //    newYnPart = true;
                                //}
                                //else
                                //{
                                //    newYnPart = false;
                                //}
                                newYnPart = true;
                                int intDspSeq = 0;
                                if (ssData_Sheet1.Cells[i, 2].Text.Trim() == "" || ssData_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssData_Sheet1.Cells[i, 2].Text.Trim()));
                                }

                                if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return 0;
                                }
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssData);

                    //ACTION
                    if (ssAction_Sheet1.RowCount > 0)
                    {
                        strType = "A";
                        if (blnA == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssAction_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssAction_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssAction_Sheet1.Cells[i, 0].Value) == true && ssAction_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                checkStatus = "1";
                                strJindanno = txtJindanNo.Text.Trim();
                                strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssAction_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssAction_Sheet1.Cells[i, 1].Text.Trim();

                                newYnPart = true;
                                int intDspSeq = 0;
                                if (ssAction_Sheet1.Cells[i, 2].Text.Trim() == "" || ssAction_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssAction_Sheet1.Cells[i, 2].Text.Trim()));
                                }
                                if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return 0;
                                }
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssAction);

                    //Response
                    if (ssResponse_Sheet1.RowCount > 0)
                    {
                        strType = "R";
                        if (blnR == true)
                        {
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                            SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return 0;
                            }
                        }
                        for (i = 0; i < ssResponse_Sheet1.RowCount; i++)
                        {
                            strUserGb = ssResponse_Sheet1.Cells[i, 4].Text.Trim();
                            if (Convert.ToBoolean(ssResponse_Sheet1.Cells[i, 0].Value) == true && ssResponse_Sheet1.Cells[i, 1].Text.Trim() != "")
                            {
                                checkStatus = "1";
                                strJindanno = txtJindanNo.Text.Trim();
                                strJINTITLE = txtNrJindan.Text.Trim();

                                strMacrono = ssResponse_Sheet1.Cells[i, 3].Text.Trim();
                                strContent = ssResponse_Sheet1.Cells[i, 1].Text.Trim();

                                newYnPart = true;
                                int intDspSeq = 0;
                                if (ssResponse_Sheet1.Cells[i, 2].Text.Trim() == "" || ssResponse_Sheet1.Cells[i, 2].Text.Trim() == "999")
                                {
                                    intDspSeq = i;
                                }
                                else
                                {
                                    intDspSeq = Convert.ToInt32(VB.Val(ssResponse_Sheet1.Cells[i, 2].Text.Trim()));
                                }
                                if (saveEmrNurseRecord(dblEmrNoNew, strType, strContent, txtJindanNo.Text.Trim(), strMacrono, newYnPart, checkStatus, strUserGb, strStatusNo, intDspSeq) == false)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBoxEx(this, SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return 0;
                                }
                            }
                        }
                    }
                    strNrRecore = strNrRecore + MakeSpdXml(ssResponse);

                    strXML = strXmlHead + strNrRecore + strXmlTail;
                    if (clsXMLOld.gSaveEmrXmlEx(clsDB.DbCon, pTmp, mstrFormNo, mstrUpdateNo, dblEmrNoNew, dblEmrNoOld, strChartDate, strChartTime, strXML) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }//(tabNr.SelectedIndex == 0 || tabNr.SelectedIndex == 1)

                if (tabNr.SelectedIndex == 2)
                {
                    strType = "N";
                    strUserGb = clsType.User.IdNumber; 
                    dblEmrNoOld = VB.Val(mstrEmrNo);
                    dblEmrNoNew = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GetEmrXmlNo");

                    if (dblEmrNoOld != 0)
                    {
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                        SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNoOld;
                        SQL = SQL + ComNum.VBLF + "      AND TYPE = '" + strType + "'";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return 0;
                        }
                    }

                    checkStatus = "1";
                    newYnPart = true;

                    if (saveEmrNurseRecord(dblEmrNoNew, strType, VB.Replace(txtNarration.Text.Trim(), "'", "`"), txtJindanNo.Text.Trim(), "0", newYnPart, checkStatus, strUserGb, strStatusNo, 1) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                    strNrRecore = strNrRecore + "<txtNarration Type=\"TextBox\" Index=\"\"><![CDATA[" + VB.Replace(txtNarration.Text.Trim(), "'", "`") + "]]></txtNarration>";

                    strXML = strXmlHead + strNrRecore + strXmlTail;
                    if (clsXMLOld.gSaveEmrXmlEx(clsDB.DbCon, pTmp, mstrFormNo, mstrUpdateNo, dblEmrNoNew, dblEmrNoOld, strChartDate, strChartTime, strXML) == false)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return 0;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, "저장하였습니다.");
                Cursor.Current = Cursors.Default;
                mstrXML = strXML;
                return dblEmrNoNew;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return 0;
            }
        }

        private string MakeSpdXml(FarPoint.Win.Spread.FpSpread ssSpd)
        {
            int i = 0;
            int j = 0;
            string strXML = "";
            string strConName = ssSpd.Name;
            string strConIndex = "";

            strXML = strXML + "<" + strConName + " Type=\"fpSpreadSheet\"" + " Index=\"" + strConIndex + "\"><![CDATA[" + Convert.ToString(ssSpd.ActiveSheet.RowCount) + "_" + Convert.ToString(ssSpd.ActiveSheet.ColumnCount) + "]]></" + strConName + ">";
            for (i = 0; i < ssSpd.ActiveSheet.RowCount; i++)
            {
                for (j = 0; j < ssSpd.ActiveSheet.ColumnCount; j++)
                {
                    strXML = strXML + clsXMLOld.SaveSpreadData(ssSpd, i, j);
                }
            }
            return strXML;
        }

        private bool saveEmrNurseRecord(double dblEmrNo, string strType, string strContent, string strJindanNo, string strMacrono, bool newYnPart, string checkStatus, string strUserGb, string strStatusNo, int intDspSeq)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                if (newYnPart == true)
                {
                    if (strJindanNo == "")
                    {
                        strJindanNo = "9999999999999";
                    }

                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "  INSERT INTO " + ComNum.DB_EMR + "EMRNURSRECORD ( ";
                    SQL = SQL + ComNum.VBLF + "    EMRNO, STATUSNO, TYPE , NRRECORD, JINDANTITLE, JINDANNO, USERGB, MACRONO, DELCLS, DSPSEQ ) ";
                    SQL = SQL + ComNum.VBLF + "  VALUES ( ";
                    SQL = SQL + ComNum.VBLF + "  " + dblEmrNo + ",";
                    SQL = SQL + ComNum.VBLF + "  " + VB.Val(strStatusNo) + ",";
                    SQL = SQL + ComNum.VBLF + "  '" + strType + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + VB.Replace(strContent.Trim(), "'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + "  '" + VB.Replace(txtNrJindan.Text.Trim(), "'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + "  " + strJindanNo + ",";
                    SQL = SQL + ComNum.VBLF + "  '" + strUserGb + "',";
                    if (strMacrono == "")
                    {
                        SQL = SQL + ComNum.VBLF + "0,";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + strMacrono + ",";
                    }
                    SQL = SQL + ComNum.VBLF + checkStatus + ",";
                    SQL = SQL + ComNum.VBLF + intDspSeq + ")";

                }
                else
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "  UPDATE " + ComNum.DB_EMR + "EMRNURSRECORD SET ";
                    SQL = SQL + ComNum.VBLF + "      NRRECORD = '" + VB.Replace(strContent.Trim(), "'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + "      JINDANTITLE = '" + VB.Replace(txtNrJindan.Text.Trim(), "'", "`") + "',";
                    SQL = SQL + ComNum.VBLF + "      JINDANNO = " + strJindanNo + ",";
                    SQL = SQL + ComNum.VBLF + "      MACRONO = " + VB.Val(strMacrono) + ",";
                    SQL = SQL + ComNum.VBLF + "      TYPE = '" + strType + "',";
                    SQL = SQL + ComNum.VBLF + "      DELCLS = '" + checkStatus + "',";
                    SQL = SQL + ComNum.VBLF + "      DSPSEQ = " + intDspSeq ;
                    SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + dblEmrNo;
                    SQL = SQL + ComNum.VBLF + "      AND   MACRONO = " + VB.Val(strMacrono);
                    if (strType != "N")
                    {
                        SQL = SQL + ComNum.VBLF + "      AND   USERGB = '" + strUserGb + "'";
                    }
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this,  ex.Message);
                return false;
            }
        }

        private DataTable getPatJindan(EmrPatient pPara, string JindanNo)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT B.STATUSNO, B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.USERGB, A.GRPFORMNAME, B.USEID, D.USENAME";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRDIAGNOSISGROUP A ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMRNRJINDANSTATUS B ";
            SQL = SQL + ComNum.VBLF + "                                ON A.GRPFORMNO = B.JINDANNO ";
            SQL = SQL + ComNum.VBLF + "                                AND A.USERGB = B.USERGB ";
            SQL = SQL + ComNum.VBLF + "                             INNER JOIN " + ComNum.DB_EMR + "VIEWEMRUSER D ";
            SQL = SQL + ComNum.VBLF + "                                ON B.USEID = D.USEID  ";
            SQL = SQL + ComNum.VBLF + "  WHERE B.PTNO = '" + pPara.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.MEDFRDATE = '" + pPara.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.INOUTCLS ='" + pPara.inOutCls + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.DELCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "  GROUP BY  B.STATUSNO, B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.USERGB, A.GRPFORMNAME, B.USEID, D.USENAME";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return null;
            }
            return dt;
        }
        /// <summary>
        /// 삭제
        /// </summary>
        /// <returns></returns>
        public bool pDelData()
        {
            if (VB.Val(mstrEmrNo) == 0)
            {
                return false;
            }

            if (VB.Val(mstrEmrNo) != 0)
            {
                if (clsEmrQuery.IsChangeAuth(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false) return false;
            }

            if (pDelNrRecord() == false)
            {
                return false;
            }

            if (clsXML.gDeleteEmrXml(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == true)
            {
                mstrEmrNo = "0";
                pClearForm();

                pSetEmrInfo();
                GetJinDanStsInfo("1");
                QueryChartList();

            }
            return false;
        }

        private bool pDelNrRecord()
        {
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "UPDATE   " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                SQL = SQL + ComNum.VBLF + "    WHERE   EMRNO = " + mstrEmrNo;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this,  ex.Message);
                return false;
            }
            
        }
        /// <summary>
        /// 기록지를 출력한다.
        /// </summary>
        public void pPrintForm()
        {

        }

        /// <summary>
        /// 클리어하고 폼별로 별요한것 기본 세팅
        /// </summary>
        private void pClearFormExcept()
        {
            clsEmrFunc.usBtnHide(usFormTopMenuEvent);
            if (clsType.User.AuAWRITE == "1")
            {
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnClear");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnSave");
                clsEmrFunc.usBtnShow(usFormTopMenuEvent, "mbtnDelete");
            }
            //else
            //{
            //    panWrite.Visible = false;
            //    panList.Top = 0;
            //    panList.Height = panChart.Height - 20;
            //}

            //mstrEmrNo = "0";

            // 내원일을 세팅한다.
            clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);

            txtJindanNo.Text = "";
            txtNrJindan.Text = "";
            txtNarration.Text = "";

            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;
            SSTab1.Enabled = true;
            tabNr.SelectedTab = tabNr_TabPage2;

            mstrUserGb = "";
        }

        #endregion


        private void frmMacrowordProgEvent_SetMacro(string strCtlName, string strMacrono, string strMacro, string strCtlNameIdx)
        {
            string strConIndex = "";
            strConIndex = clsXML.IsArryCon(mControl);

            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;

            //strMacro = VB.Replace(strMacro, "\n", "\r\n", 1, -1);

            if (optAdd.Checked == true)
            {
                mControl.Text = mControl.Text + " " + strMacro;
            }
            else
            {
                mControl.Text = strMacro;
            }
        }

        private void frmMacrowordProgEvent_EventClosed()
        {
            frmMacrowordProgEvent.Close();
            frmMacrowordProgEvent = null;
        }

        public frmNursingRecord()
        {
            InitializeComponent();
        }

        public frmNursingRecord(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
        }

        public frmNursingRecord(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            p = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
        }

        private void frmNursingRecord_Load(object sender, EventArgs e)
        {
            trvJindan.Dock = DockStyle.Fill;
            panItem.Dock = DockStyle.Fill;

            nImage = 0;
            nSelectedImage = 1;

            //nImageSaved = 2;
            //nSelectedImageSaved = 3;

            trvJindan.ImageList = this.ImageList2;
            ssItem_Sheet1.RowCount = 0;
            ssList_Sheet1.RowCount = 0;
            ssStatus_Sheet1.RowCount = 0;

            panItem.Visible = false;
            pComboSet();
            pInitForm();

            SetUserAut();
            if (VB.Val(mstrEmrNo) > 0)
            {
                // 내원일을 세팅한다.
                clsEmrFunc.SetMedFrEndDate(clsDB.DbCon, mstrEmrNo, p, dtpFrDate, dtpEndDate);
            }
            QueryChartList();

            optIng.Checked = true;
            GetJinDanStsInfo("1");

            optNanda.Checked = true;
        }

        private void pComboSet()
        {
            int i = 0;
            DataTable dt = null;

            cboWard.Items.Clear();
            //-->병동콤보 세팅
            cboWard.Items.Add("전  체" + VB.Space(50) + "0");

            dt = clsEmrQueryPohangS.READ_WARD_LIST(clsDB.DbCon);

            if (dt == null)
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count != 0)
            {
                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add((dt.Rows[i]["WARDNAME"].ToString() + "").Trim() + VB.Space(50) + (dt.Rows[i]["WARDCD"].ToString() + "").Trim());
                }
            }
            dt.Dispose();
            dt = null;
            cboWard.SelectedIndex = 0;

            for (i = 0; i < cboWard.Items.Count; i++)
            {
                if ((VB.Right(cboWard.Items[i].ToString(), 10)).Trim() == clsType.User.DeptCode)
                {
                    cboWard.SelectedIndex = i;
                    break;
                }
            }
        }

        private void SetUserAut()
        {
            //if (clsType.User.AuAWRITE == "1")
            //{
            //    panWrite.Visible = true;
            //    panChart.Top = 37;
            //}
            //else
            //{
            //    panWrite.Visible = false;
            //    panChart.Top = 0;
            //}
            //if (clsType.User.AuAPRINTIN == "1")
            //{
            //    mbtnPrint.Visible = true;
            //}
            //else
            //{
            //    mbtnPrint.Visible = true;
            //}

            panWrite.Visible = true;
            panChart.Top = 37;
        }

        private void mbtnSearchAll_Click(object sender, EventArgs e)
        {
            QueryChartList();
        }

        private void QueryChartList()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssList_Sheet1.RowCount = 0;

            return;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT /*+ INDEX(A EMRXML_IDX22) */ A.EMRNO, A.CHARTDATE, A.CHARTTIME, A.PTNO,  A.MEDDRCD, A.MEDDEPTCD, A.MEDFRDATE, A.WRITEDATE,A.WRITETIME, A.PRNYN, ";
            SQL = SQL + ComNum.VBLF + "         C.JINDANNO, C.JINDANTITLE AS JINTITLE,C.DELCLS, C.MACRONO, ";
            SQL = SQL + ComNum.VBLF + "         C.TYPE, C.USERGB, ";
            SQL = SQL + ComNum.VBLF + "         C.NRRECORD AS CONTENT, B.USENAME, A.USEID, C.DSPSEQ , D.DISPSEQ ";      //'E.PRTREQSEQ,  "
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRXML A INNER JOIN " + ComNum.DB_EMR + "VIEWEMRUSER B ";
            SQL = SQL + ComNum.VBLF + "                   ON A.USEID = B.USEID ";
            SQL = SQL + ComNum.VBLF + "                INNER JOIN " + ComNum.DB_EMR + "EMRNURSRECORD C ";
            SQL = SQL + ComNum.VBLF + "                   ON A.EMRNO = C.EMRNO   ";
            SQL = SQL + ComNum.VBLF + "                 LEFT OUTER JOIN " + ComNum.DB_EMR + "EMRNRMACRO D ";
            SQL = SQL + ComNum.VBLF + "                   ON C.MACRONO = D.MACRONO ";
            SQL = SQL + ComNum.VBLF + "                   AND C.USERGB = D.USERGB ";
            SQL = SQL + ComNum.VBLF + "                INNER JOIN " + ComNum.DB_EMR + "EMRNRMACRODISPSEQ DD ";
            SQL = SQL + ComNum.VBLF + "                   ON C.TYPE = DD.TYPE ";
            SQL = SQL + ComNum.VBLF + "WHERE A.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE >= '" + VB.Format(dtpFrDate.Value, "yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE <= '" + VB.Format(dtpEndDate.Value, "yyyyMMdd") + "'";
            
            SQL = SQL + ComNum.VBLF + "    AND C.DELCLS =  '1' ";
			SQL = SQL + ComNum.VBLF + "    AND C.NRRECORD IS NOT NULL ";

            if (chkDesc.Checked == true)
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE ASC,  A.CHARTTIME ASC, A.EMRNO, DD.DISPSEQ, C.DSPSEQ, D.DISPSEQ ";  //, SORTSEQ DESC ";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " ORDER BY A.CHARTDATE DESC, A.CHARTTIME DESC, A.EMRNO, DD.DISPSEQ, C.DSPSEQ, D.DISPSEQ "; //, SORTSEQ DESC ";
            }

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            string strChartDate = "";
            string strChartTime  = "";
            string strJindanno  = "";
            //int spanCount = 0;
            string tempStrChartTime  = "";
    
            string strChartDateOld  = "";
            string strChartTimeOld  = "";
            string strType  = "";
            string strTypeOld  = "";
            string strJINTITLE  = "";
            string strJINTITLEOld  = "";

            ssList_Sheet1.RowCount = dt.Rows.Count;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strChartDate = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTDATE"].ToString() + "").Trim(), "D");
                strChartTime = ComFunc.FormatStrToDate((dt.Rows[i]["CHARTTIME"].ToString() + "").Trim(), "M");
                strJINTITLE = (dt.Rows[i]["JINTITLE"].ToString() + "").Trim();
                strType = (dt.Rows[i]["TYPE"].ToString() + "").Trim();
                strJindanno = (dt.Rows[i]["JINDANNO"].ToString() + "").Trim();
                tempStrChartTime = (dt.Rows[i]["CHARTTIME"].ToString() + "").Trim();
                int intStrChartTime = Convert.ToInt32(VB.Val(tempStrChartTime));

                if (strChartDate != strChartDateOld)
                {
                    ssList_Sheet1.Cells[i, 1].Text = strChartDate;
                    ssList_Sheet1.Cells[i, 2].Text = strChartTime;
                    ssList_Sheet1.Cells[i, 3].Text = strJINTITLE;
                    ssList_Sheet1.Cells[i, 4].Text = strType;

                    strChartDateOld = strChartDate;
                    strChartTimeOld = strChartTime;
                    strJINTITLEOld = strJINTITLE;
                    strTypeOld = strType;
                }
                else
                {
                    if (strChartTimeOld != strChartTime)
                    {
                        ssList_Sheet1.Cells[i, 2].Text = strChartTime;
                        ssList_Sheet1.Cells[i, 3].Text = strJINTITLE;
                        ssList_Sheet1.Cells[i, 4].Text = strType;

                        strChartTimeOld = strChartTime;
                        strJINTITLEOld = strJINTITLE;
                        strTypeOld = strType;
                    }
                    else
                    {
                        if (strJINTITLEOld != strJINTITLE)
                        {
                            ssList_Sheet1.Cells[i, 3].Text = strJINTITLE;
                            ssList_Sheet1.Cells[i, 4].Text = strType;

                            strJINTITLEOld = strJINTITLE;
                            strTypeOld = strType;
                        }
                        else
                        {
                            if (strTypeOld != strType)
                            {
                                ssList_Sheet1.Cells[i, 4].Text = strType;

                                strTypeOld = strType;
                            }
                        }
                    }
                }
                ssList_Sheet1.Cells[i, 5].Text = (dt.Rows[i]["CONTENT"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 6].Text = (dt.Rows[i]["USENAME"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 7].Text = (dt.Rows[i]["JINDANNO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 8].Text = (dt.Rows[i]["MACRONO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 9].Text = (dt.Rows[i]["USEID"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 10].Text = (dt.Rows[i]["EMRNO"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 11].Text = (dt.Rows[i]["USERGB"].ToString() + "").Trim();
                ssList_Sheet1.Cells[i, 12].Value = ((dt.Rows[i]["PRNYN"].ToString() + "").Trim() == "1" ? true : false);

                if ((intStrChartTime >= 220000 && intStrChartTime <= 240000) || (intStrChartTime >= 0 && intStrChartTime < 70000))
                {
                    ssList_Sheet1.Cells[i, 0, i, ssList_Sheet1.ColumnCount - 1].ForeColor = Color.Red; 
                }

                ssList_Sheet1.SetRowHeight(i, Convert.ToInt32(ssList_Sheet1.GetPreferredRowHeight(i)) + 16);
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panItem.Visible = false;
        }

        private void mbtnNrJindan_Click(object sender, EventArgs e)
        {
            //TODO
            //frmNursingRecordJindan frmNursingRecordJindanX = new frmNursingRecordJindan();
            //frmNursingRecordJindanX.ShowDialog();
        }

        private void optNanda_CheckedChanged(object sender, EventArgs e)
        {
            if (optNanda.Checked == true)
            {
                cboWard.Enabled = false;
                setJindanTree();
            }
        }

        private void optWard_CheckedChanged(object sender, EventArgs e)
        {
            if (optWard.Checked == true)
            {
                cboWard.Enabled = true;
                if (cboWard.Text.Trim() == "")
                {
                    return;
                }
                setJindanTree();
            }
        }

        private void cboWard_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (optWard.Checked == true)
            {
                setJindanTree();
            }
        }

        private void setJindanTree()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strUserGb = "";

            return;

            if (optWard.Checked == true)
            {
                if (cboWard.Text.Trim() == "" || cboWard.Text.Trim().Length < 6)
                {
                    ComFunc.MsgBoxEx(this, "병동을 선택하세요");
                    return;
                }
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }
            else
            {
                strUserGb = "NANDAJIN";
            }
            

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT A.GRPFORMNO, A.GRPFORMNAME, A.GROUPSEQ, A.DEPTH, A.GROUPPARENT ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRNRDIAGNOSISGROUP A    ";
            SQL = SQL + ComNum.VBLF + " WHERE USERGB = '" + strUserGb + "'";
            SQL = SQL + ComNum.VBLF + "AND USECLS = '1'";
            SQL = SQL + ComNum.VBLF + "ORDER BY A.DEPTH,A.GROUPPARENT, A.GROUPSEQ, A.GRPFORMNO";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            System.Windows.Forms.TreeNode oNodex;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows[i]["DEPTH"].ToString() + "").Trim() == "0")
                {
                    oNodex = trvJindan.Nodes.Add((dt.Rows[i]["GRPFORMNO"].ToString() + "").Trim(), (dt.Rows[i]["GRPFORMNAME"].ToString() + "").Trim(), nImage, nSelectedImage);
                }
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if ((dt.Rows[i]["DEPTH"].ToString() + "").Trim() == "1")
                {
                    oNodex = trvJindan.Nodes.Find((dt.Rows[i]["GROUPPARENT"].ToString() + "").Trim(), true)[0].Nodes.Add((dt.Rows[i]["GRPFORMNO"].ToString() + "").Trim(), (dt.Rows[i]["GRPFORMNAME"].ToString() + "").Trim(), nImage, nSelectedImage);
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.WaitCursor;
        }

        private void trvJindan_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            System.Windows.Forms.TreeNode Node;

            Node = e.Node;
            if (Node.GetNodeCount(false) > 0)
            {
                return;
            }

            txtJindanNo.Text = "";
            txtNrJindan.Text = "";
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;

            chkD.Checked = false;
            chkA.Checked = false;
            chkRe.Checked = false;

            txtJindanNo.Text = Convert.ToString(Convert.ToInt32(VB.Val(e.Node.Name)));
            txtNrJindan.Text = e.Node.Text.Trim();  

            GetJindanInfo(Convert.ToInt32(VB.Val(e.Node.Name)), e.Node.Text.Trim());
        }

        private void GetJindanInfo(int intVal, string strName)
        {
            string strUserGb = "";

            if (optNanda.Checked == true)
            {
                strUserGb = "NANDAJIN";
            }
            else
            {
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }

            mstrUserGb = "";

            GetNrMacroList("D", intVal, strName, strUserGb, ssData_Sheet1, "");
            GetNrMacroList("A", intVal, strName, strUserGb, ssAction_Sheet1, "");
            GetNrMacroList("R", intVal, strName, strUserGb, ssResponse_Sheet1, "");
        }

        private void GetNrMacroList(string strOption, int intJinDanNo, string strGrpName, string strUserGb, FarPoint.Win.Spread.SheetView spd, string strCopy)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            mstrUserGb = strUserGb;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "  SELECT MACRONO, JINDANNO, TYPE, CONTENT, DISPSEQ, USERGB, USEID, USECLS";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRMACRO";
            SQL = SQL + ComNum.VBLF + "  WHERE JINDANNO = " + intJinDanNo;
            SQL = SQL + ComNum.VBLF + "  AND TYPE = '" + strOption + "'";
            SQL = SQL + ComNum.VBLF + "  AND USERGB = '" + strUserGb + "'";
            SQL = SQL + ComNum.VBLF + "  ORDER BY DISPSEQ ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            spd.RowCount = dt.Rows.Count;
            spd.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                spd.Cells[i, 1].Text = dt.Rows[i]["CONTENT"].ToString().Trim();
                spd.Cells[i, 2].Text = dt.Rows[i]["DISPSEQ"].ToString().Trim();
                //if (strCopy != "COPY")
                //{
                    spd.Cells[i, 3].Text = dt.Rows[i]["MACRONO"].ToString().Trim();
                //}
                    spd.Cells[i, 4].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                if (dt.Rows[i]["USECLS"].ToString().Trim() == "0")
                {
                    spd.Cells[i, 0, i, spd.ColumnCount - 1].BackColor = Color.LightGray;
                }
                else
                {
                    spd.Cells[i, 0].Value = false;
                }
            }
            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        private void btnIndexSearch_Click(object sender, EventArgs e)
        {
            string strUserGb = "";

            if (optNanda.Checked == true)
            {
                strUserGb = "NANDAJIN";
            }
            else
            {
                strUserGb = cboWard.Text.Trim().Substring(cboWard.Text.Trim().Length - 6, 6).Trim();
            }

            string strIdxItem = txtIndexSearch.Text.Trim();
            ssItem_Sheet1.RowCount = 0;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT GRPFORMNO, GRPFORMNAME, USERGB  ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRDIAGNOSISGROUP";
            SQL = SQL + ComNum.VBLF + "     WHERE USERGB  = '" + strUserGb + "'";
            SQL = SQL + ComNum.VBLF + "     AND USECLS ='1'";
            SQL = SQL + ComNum.VBLF + "     AND DEPTH ='1'";
            SQL = SQL + ComNum.VBLF + "     AND GRPFORMNAME LIKE '%" + strIdxItem + "%'";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssItem_Sheet1.RowCount = dt.Rows.Count;
            ssItem_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssItem_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GRPFORMNO"].ToString().Trim();
                ssItem_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GRPFORMNAME"].ToString().Trim();
                ssItem_Sheet1.Cells[i, 2].Text = dt.Rows[i]["USERGB"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            panItem.Visible = true;
            panItem.BringToFront(); 
        }

        private void ssItem_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssItem_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            int intVal = Convert.ToInt32(VB.Val(ssItem_Sheet1.Cells[e.Row, 0].Text.Trim()));
            string strGrpName = ssItem_Sheet1.Cells[e.Row, 1].Text.Trim();

            GetJindanInfo(intVal, strGrpName);
        }

        private void optAll_CheckedChanged(object sender, EventArgs e)
        {
            if (optAll.Checked == true)
            {
                GetJinDanStsInfo("0");
            }
        }

        private void optEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (optEnd.Checked == true)
            {
                GetJinDanStsInfo("2");
            }
        }

        private void optIng_CheckedChanged(object sender, EventArgs e)
        {
            if (optIng.Checked == true)
            {
                GetJinDanStsInfo("1");
            }
        }

        private void GetJinDanStsInfo(string strStatus)
        {
            ssStatus_Sheet1.RowCount = 0;

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            return;

            Cursor.Current = Cursors.WaitCursor;

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + " A.GROUPSEQ, A.GRPFORMNO, A.USERGB, ";
            SQL = SQL + ComNum.VBLF + " B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.STATUSNO, A.GRPFORMNAME, B.USEID, D.USENAME ";
            SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "EMRNRDIAGNOSISGROUP A ";
            SQL = SQL + ComNum.VBLF + "  INNER JOIN " + ComNum.DB_EMR + "EMRNRJINDANSTATUS B ";
            SQL = SQL + ComNum.VBLF + "                                ON A.GRPFORMNO = B.JINDANNO ";
            SQL = SQL + ComNum.VBLF + "                                AND A.USERGB = B.USERGB ";
            SQL = SQL + ComNum.VBLF + "                             INNER JOIN " + ComNum.DB_EMR + "VIEWEMRUSER D ";
            SQL = SQL + ComNum.VBLF + "                                ON B.USEID = D.USEID  ";
            SQL = SQL + ComNum.VBLF + "  WHERE B.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.MEDFRDATE = '" + p.medFrDate + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.MEDDEPTCD = '" + p.medDeptCd + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.MEDDRCD   = '" + p.medDrCd + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.INOUTCLS ='" + p.inOutCls + "'";
            SQL = SQL + ComNum.VBLF + "  AND B.DELCLS = '0' ";
            if (strStatus != "0")
            {
                SQL = SQL + ComNum.VBLF + "  AND B.STATUS = '" + strStatus + "' ";// '1:진행중 2:완료
            }
            SQL = SQL + ComNum.VBLF + " GROUP BY A.GROUPSEQ, A.GRPFORMNO, A.USERGB, B.STATUS, B.STARTDATE, B.ENDDATE, B.JINDANNO, B.STATUSNO, A.GRPFORMNAME, B.USEID, D.USENAME ";
            SQL = SQL + ComNum.VBLF + " ORDER BY B.STARTDATE";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssStatus_Sheet1.RowCount = dt.Rows.Count;
            ssStatus_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            for (i = 0; i < dt.Rows.Count; i++)
            {
                ssStatus_Sheet1.Cells[i, 1].Text = dt.Rows[i]["GRPFORMNAME"].ToString().Trim();
                ssStatus_Sheet1.Cells[i, 2].Text = ComFunc.FormatStrToDate(dt.Rows[i]["STARTDATE"].ToString().Trim(), "D");
                ssStatus_Sheet1.Cells[i, 3].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ENDDATE"].ToString().Trim(), "D");
                ssStatus_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JINDANNO"].ToString().Trim();
                ssStatus_Sheet1.Cells[i, 5].Text = dt.Rows[i]["STATUSNO"].ToString().Trim();
                ssStatus_Sheet1.Cells[i, 6].Text = dt.Rows[i]["USERGB"].ToString().Trim();
                ssStatus_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GROUPSEQ"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void ssStatus_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssStatus_Sheet1.RowCount == 0)
            {
                return;
            }
            if (e.ColumnHeader == true)
            {
                return;
            }

            int intVal = Convert.ToInt32(VB.Val(ssStatus_Sheet1.Cells[e.Row, 4].Text.Trim()));
            string strGrpName = ssStatus_Sheet1.Cells[e.Row, 1].Text.Trim();
            string strUserGb = ssStatus_Sheet1.Cells[e.Row, 6].Text.Trim();

            txtJindanNo.Text = "";
            txtNrJindan.Text = "";
            ssData_Sheet1.RowCount = 0;
            ssAction_Sheet1.RowCount = 0;
            ssResponse_Sheet1.RowCount = 0;

            chkD.Checked = false;
            chkA.Checked = false;
            chkRe.Checked = false;

            txtJindanNo.Text = Convert.ToString(intVal);
            txtNrJindan.Text = strGrpName;  

            GetNrMacroList("D", intVal, strGrpName, strUserGb, ssData_Sheet1, "");
            GetNrMacroList("A", intVal, strGrpName, strUserGb, ssAction_Sheet1, "");
            GetNrMacroList("R", intVal, strGrpName, strUserGb, ssResponse_Sheet1, "");
        }

        private void btnJindanDel_Click(object sender, EventArgs e)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssStatus_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말 삭제하시겠습니까?", "간호진단", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssStatus_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssStatus_Sheet1.Cells[i, 0].Value) == true)
                    {
                        string strStatusNo = ssStatus_Sheet1.Cells[i, 5].Text.Trim() ;
                        SQL = " UPDATE " + ComNum.DB_EMR + "EMRNRJINDANSTATUS SET DELCLS = '1', ";
                        SQL = SQL + ComNum.VBLF + "                             WRITEDATE = '" + strCurDateTime.Substring(0,8) + "', ";
                        SQL = SQL + ComNum.VBLF + "                             WRITETIME = '" + strCurDateTime.Substring(strCurDateTime.Length - 6, 6) + "', ";
                        SQL = SQL + ComNum.VBLF + "                             USEID = '" + clsType.User.IdNumber + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO =  '" + p.ptNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "  AND MEDDEPTCD = '" + p.medDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "  AND MEDDRCD   = '" + p.medDrCd + "'";
                        SQL = SQL + ComNum.VBLF + "  AND INOUTCLS ='" + p.inOutCls + "'";
                        SQL = SQL + ComNum.VBLF + "   AND STATUSNO = '" + strStatusNo + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "삭제 하였습니다.");

                GetJinDanStsInfo("1");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnComplete_Click(object sender, EventArgs e)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssStatus_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("완료하시겠습니까?", "간호진단", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssStatus_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssStatus_Sheet1.Cells[i, 0].Value) == true)
                    {
                        string strStatusNo = ssStatus_Sheet1.Cells[i, 5].Text.Trim();
                        SQL = " UPDATE " + ComNum.DB_EMR + "EMRNRJINDANSTATUS SET STATUS = '2', ";
                        SQL = SQL + ComNum.VBLF + "                             WRITEDATE = '" + strCurDateTime.Substring(0, 8) + "', ";
                        SQL = SQL + ComNum.VBLF + "                             WRITETIME = '" + strCurDateTime.Substring(strCurDateTime.Length - 6, 6) + "', ";
                        SQL = SQL + ComNum.VBLF + "                             USEID = '" + clsType.User.IdNumber + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE PTNO =  '" + p.ptNo + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND MEDFRDATE = '" + p.medFrDate + "'";
                        SQL = SQL + ComNum.VBLF + "  AND MEDDEPTCD = '" + p.medDeptCd + "'";
                        SQL = SQL + ComNum.VBLF + "  AND MEDDRCD   = '" + p.medDrCd + "'";
                        SQL = SQL + ComNum.VBLF + "  AND INOUTCLS ='" + p.inOutCls + "'";
                        SQL = SQL + ComNum.VBLF + "   AND STATUSNO = '" + strStatusNo + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "완료 하였습니다.");

                GetJinDanStsInfo("0");
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this,  ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void mbtnDelete_Click(object sender, EventArgs e)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            int i = 0;
            //DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("정말 삭제하시겠습니까?", "간호진단", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                for (i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                    {
                        if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 12].Value) == false)
                        {
                            string strEmrNo = ssList_Sheet1.Cells[i, 10].Text.Trim();
                            string strMacrono = ssList_Sheet1.Cells[i, 8].Text.Trim();

                            SQL = " UPDATE " + ComNum.DB_EMR + "EMRNURSRECORD SET DELCLS = '0'";
                            SQL = SQL + ComNum.VBLF + " WHERE EMRNO = " + VB.Val(strEmrNo);
                            SQL = SQL + ComNum.VBLF + "  AND MACRONO = " + VB.Val(strMacrono);
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBoxEx(this, SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                return;
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, "삭제 하였습니다.");

                QueryChartList();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void cmdInsData_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssData_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssData_Sheet1.RowCount = ssData_Sheet1.RowCount + 1;
            ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 0].Value = true;
            ssData_Sheet1.Cells[ssData_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdInsAction_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssAction_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssAction_Sheet1.RowCount = ssAction_Sheet1.RowCount + 1;
            ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 0].Value = true;
            ssAction_Sheet1.Cells[ssAction_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdInsResponse_Click(object sender, EventArgs e)
        {
            string strUserGb = "";
            if (ssResponse_Sheet1.RowCount <= 0)
            {
                strUserGb = mstrUserGb;
            }
            else
            {
                strUserGb = ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 4].Text.Trim();
            }
            ssResponse_Sheet1.RowCount = ssResponse_Sheet1.RowCount + 1;
            ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 0].Value = true;
            ssResponse_Sheet1.Cells[ssResponse_Sheet1.RowCount - 1, 4].Text = strUserGb;
        }

        private void cmdDelData_Click(object sender, EventArgs e)
        {
            if (ssData_Sheet1.RowCount == 0)
            {
                return;
            }
            ssData_Sheet1.Rows[ssData_Sheet1.ActiveRowIndex].Remove();
        }

        private void cmdDelAction_Click(object sender, EventArgs e)
        {
            if (ssAction_Sheet1.RowCount == 0)
            {
                return;
            }
            ssAction_Sheet1.Rows[ssAction_Sheet1.ActiveRowIndex].Remove();
        }

        private void cmdDelResponse_Click(object sender, EventArgs e)
        {
            if (ssResponse_Sheet1.RowCount == 0)
            {
                return;
            }
            ssResponse_Sheet1.Rows[ssResponse_Sheet1.ActiveRowIndex].Remove();
        }

        private void btnNarration_Click(object sender, EventArgs e)
        {
            string strConIndex = "";
            mControl = txtNarration;
            strConIndex = clsXML.IsArryCon(mControl);

            if (frmMacrowordProgEvent == null)
            {
                frmMacrowordProgEvent = new frmEmrMacrowordProg(mControl.Name, strConIndex, mstrFormNo, "200", mstrFormText);
                frmMacrowordProgEvent.rSetMacro += new frmEmrMacrowordProg.SetMacro(frmMacrowordProgEvent_SetMacro);
                frmMacrowordProgEvent.rEventClosed += new frmEmrMacrowordProg.EventClosed(frmMacrowordProgEvent_EventClosed);
            }
            frmMacrowordProgEvent.ShowDialog();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssList_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }
            string strEmrNo = ssList_Sheet1.Cells[e.Row, 10].Text.Trim();
            mstrEmrNo = strEmrNo;

            pLoadEmrChartInfo();
        }

        private void CheckSpd(FarPoint.Win.Spread.SheetView spd, bool chkVal)
        {
            int i = 0;
            for (i = 0; i < spd.RowCount; i++)
            {
                spd.Cells[i, 0].Value = chkVal;
            }
        }

        private void chkD_CheckedChanged(object sender, EventArgs e)
        {
            CheckSpd(ssData_Sheet1, chkD.Checked);
        }

        private void chkA_CheckedChanged(object sender, EventArgs e)
        {
            CheckSpd(ssAction_Sheet1, chkA.Checked);
        }

        private void chkRe_CheckedChanged(object sender, EventArgs e)
        {
            CheckSpd(ssResponse_Sheet1, chkRe.Checked);
        }

        private void mbtnPrint_Click(object sender, EventArgs e)
        {
            mbtnPrint.Enabled = false;
            ssList_Sheet1.Columns[0].Visible = false;
            ssList_Sheet1.Columns[1].Width = 86;
            ssList_Sheet1.Columns[12].Visible = false;
            clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, p, true, dtpFrDate.Value.ToString("yyyyMMdd"), dtpEndDate.Value.ToString("yyyyMMdd"),
                                         ssList, "P", 30, 20, 50, 50, false, FarPoint.Win.Spread.PrintOrientation.Portrait, "ROW", -1, 10, 0, "A");
            ssList_Sheet1.Columns[0].Visible = true;
            ssList_Sheet1.Columns[1].Width = 70;
            ssList_Sheet1.Columns[12].Visible = true;
            mbtnPrint.Enabled = true;
        }
    }
}
