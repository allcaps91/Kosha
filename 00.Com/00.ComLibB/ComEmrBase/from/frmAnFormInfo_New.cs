using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using FarPoint.Win;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;
using FarPoint.Win.Spread.Model;

namespace ComEmrBase
{
    public partial class frmAnForm_New
    {
        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        public FormEmrMessage mEmrCallForm;
        public string mstrFormNo = "1568";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "마취기록지";
        public EmrPatient AcpEmr = null;
        public string mstrEmrNo = "0";
        public string mstrEmrHisNo = "0";
        public string mstrMode = "W";

        public string mstrCHARTDATE = string.Empty;
        public string mstrCHARTTIME = string.Empty;
        public string mstrCHARTUSEID = string.Empty;
        #endregion

        //private string FORMNO = "1568";
        private List<ControlInfo> ControlInfos;
        private Timer CurrentTimer = null;
        private DateTime CreateDate;

        readonly int LabelWidth = 55;

        ComboBoxCellType Type1;
        CheckBoxCellType Type2;

        /// <summary>
        /// 초기설정
        /// </summary>
        private void SetInit()
        {
            // 버튼권한설정
            BtnShowReg();

            //if (clsType.User.Sabun == "36059")
            //{
            //    ssView.ActiveSheet.OperationMode = OperationMode.Normal;
            //}
            //else
            //{
            ssView.ActiveSheet.OperationMode = OperationMode.ReadOnly;
            //}

            panTitleSub0.Visible = false;
            PanGeneralAnes.Visible = false;
            PanSiteAnes.Visible = false;
            PanAttention.Visible = false;


            #region 컨트롤 설정

            ControlInfos = new List<ControlInfo>
            {
                new ControlInfo{ MTitle = "수술시작일자", control = dtpOpDateFr },
                new ControlInfo{ MTitle = "수술종료일자", control = dtpOpDateEnd },
                new ControlInfo{ MTitle = "혈액형", control = txtBlod },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_1 },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_2 },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_3 },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_4 },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_5 },
                new ControlInfo{ MTitle = "ASA", control = rdoASA_6 },
                new ControlInfo{ MTitle = "Emergency", control = chkEmergency },
                new ControlInfo{ MTitle = "NPO", STitle = "Y", control = rdoNPO_Y },
                new ControlInfo{ MTitle = "NPO", STitle = "N", control = rdoNPO_N },
                new ControlInfo{ MTitle = "NPO", STitle = "Text", control = txtNPO_Etc },
                new ControlInfo{ MTitle = "RoomNo", control = txtRoomNo },
                new ControlInfo{ MTitle = "Preop.Dx", control = txtPreopDx },
                new ControlInfo{ MTitle = "Postop.Dx", control = txtPostopDx },
                new ControlInfo{ MTitle = "Operation", control = txtOperation },
                new ControlInfo{ MTitle = "Remark", control = txtRemark },
                new ControlInfo{ MTitle = "WRTNO", control = txtWRTNO },

                new ControlInfo{ MTitle = "Anes. Dr", control = pnlAnesDr },
                new ControlInfo{ MTitle = "Surgeon", control = pnlSurgeon },
                new ControlInfo{ MTitle = "Assist", control = pnlAssist },
                new ControlInfo{ MTitle = "Anes. Nr", control = pnlAnesNr },
                new ControlInfo{ MTitle = "Scrub. Nr", control = pnlScrubNr },
                new ControlInfo{ MTitle = "Cir. Nr", control = pnlCirNr },

                new ControlInfo{ MTitle = "PCA IV", control = rdoPCAIV },
                new ControlInfo{ MTitle = "PCA Epidural", control = rdoPCAEP },
                
                //new ControlInfo{ MTitle = "Anes. Dr", control = txtAnesDr },
                //new ControlInfo{ MTitle = "Surgeon", control = txtSurgeon },
                //new ControlInfo{ MTitle = "Anes. Nr", control = txtAnesNr },
                //new ControlInfo{ MTitle = "Scrub. Nr", control = txtScrubNr },
                //new ControlInfo{ MTitle = "Cir. Nr", control = txtCirNr },

                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "HB", control = txtHb },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "HCT", control = txtHct },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "OT", control = txtOT },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "PT", control = txtPT },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "NA", control = txtNa },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "K", control = txtK },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "PT", control = txtPT2 },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "PTT", control = txtPTT },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "BT", control = txtPLT },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "CT", control = txtWBC },

                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "SBP", control = txtSBP },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "DBP", control = txtDBP },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "PR", control = txtPR },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "BW", control = txtBW },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "BT", control = txtBT2 },
                new ControlInfo{ MTitle = "Preanes.Findings", STitle = "EKG", control = txtEKG },

                new ControlInfo{ MTitle = "Past HX", STitle = "고혈압", control = chkHTN },
                new ControlInfo{ MTitle = "Past HX", STitle = "당뇨", control = chkDM },
                new ControlInfo{ MTitle = "Past HX", STitle = "결핵", control = chkTC },
                new ControlInfo{ MTitle = "Past HX", STitle = "암", control = chkCC },
                new ControlInfo{ MTitle = "Past HX", STitle = "간질환", control = chkLC },
                new ControlInfo{ MTitle = "Past HX", STitle = "심장질환", control = chkCD },
                new ControlInfo{ MTitle = "Past HX", STitle = "기타", control = chkOther },
                new ControlInfo{ MTitle = "Past HX", STitle = "Etc", control = txtPastHx },

                new ControlInfo{ MTitle = "Consult", control = txtConsult },
                new ControlInfo{ MTitle = "Arrive", control = txtArrive },

                new ControlInfo{ MTitle = "Mentality", STitle = "alert", control = chkMentalityAlert },
                new ControlInfo{ MTitle = "Mentality", STitle = "drowsy", control = chkMentalityDrowsy },
                new ControlInfo{ MTitle = "Mentality", STitle = "stupor", control = chkMentalityStupor },
                new ControlInfo{ MTitle = "Mentality", STitle = "semicoma", control = chkMentalitySemicoma },
                new ControlInfo{ MTitle = "Mentality", STitle = "coma", control = chkMentalityComa },
                new ControlInfo{ MTitle = "MentalityRemark", control = txtMentalityRemark },

                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "SBP", control = txtPreSBP },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "DBP", control = txtPreDBP },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "PR", control = txtPrePR },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "SPO2", control = txtPreSPO2 },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "BT", control = txtPreBT },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "NPOY", control = rdoPreNPO_Y },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "NPON", control = rdoPreNPO_N },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "AttentionY", control = rdoAttention_Y },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "AttentionN", control = rdoAttention_N },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "EKG", control = txtPreEKG },
                new ControlInfo{ MTitle = "Pre induction assessment", STitle = "비고", control = txtPreEtc },

                new ControlInfo{ MTitle = "전신마취", STitle = "GeneralAnes", control = ChkGeneralAnes },
                new ControlInfo{ MTitle = "전신마취", STitle = "General", control = chkGeneral },
                new ControlInfo{ MTitle = "전신마취", STitle = "Difficult", control = chkDifficult },
                new ControlInfo{ MTitle = "전신마취", STitle = "ETT", control = txtETT },
                new ControlInfo{ MTitle = "전신마취", STitle = "Cuff", control = chkCuff },
                new ControlInfo{ MTitle = "전신마취", STitle = "Oral", control = chkOral },
                new ControlInfo{ MTitle = "전신마취", STitle = "Nasal", control = chkNasal },
                new ControlInfo{ MTitle = "전신마취", STitle = "Trac", control = chkTrac },
                new ControlInfo{ MTitle = "전신마취", STitle = "Wire", control = chkWire },
                new ControlInfo{ MTitle = "전신마취", STitle = "Mask", control = chkMask },
                new ControlInfo{ MTitle = "전신마취", STitle = "MAC", control = chkMac },
                new ControlInfo{ MTitle = "전신마취", STitle = "LMA", control = chkLMA },
                new ControlInfo{ MTitle = "전신마취", STitle = "LMA", control = txtLMA },
                new ControlInfo{ MTitle = "전신마취", STitle = "LMARemark", control = txtLMARemark },
                new ControlInfo{ MTitle = "전신마취", STitle = "One lung", control = chkOnelung },
                new ControlInfo{ MTitle = "전신마취", STitle = "One lung R", control = chkOnelungR },
                new ControlInfo{ MTitle = "전신마취", STitle = "One lung L", control = chkOnelungL },
                new ControlInfo{ MTitle = "전신마취", STitle = "One lung ETC", control = txtOnelung },

                new ControlInfo{ MTitle = "부위마취", STitle = "SiteAnes", control = ChkSiteAnes },
                new ControlInfo{ MTitle = "부위마취", STitle = "Spinal", control = chkSpinal },
                new ControlInfo{ MTitle = "부위마취", STitle = "Spinal ETC", control = txtSpinal },
                new ControlInfo{ MTitle = "부위마취", STitle = "Spinal Product", control = txtSpinalProduct },
                new ControlInfo{ MTitle = "부위마취", STitle = "Epidural", control = chkEpidural },
                new ControlInfo{ MTitle = "부위마취", STitle = "Epidural ETC", control = txtEpidural },
                new ControlInfo{ MTitle = "부위마취", STitle = "Epidural Product", control = txtEpiduralProduct },
                new ControlInfo{ MTitle = "부위마취", STitle = "Caudal", control = chkCaudal },
                new ControlInfo{ MTitle = "부위마취", STitle = "Caudal ETC", control = txtCaudal },
                new ControlInfo{ MTitle = "부위마취", STitle = "Caudal Product", control = txtCaudalProduct },
                new ControlInfo{ MTitle = "부위마취", STitle = "NB", control = chkNB },
                new ControlInfo{ MTitle = "부위마취", STitle = "Sonoguide", control = chkSonoguide },
                new ControlInfo{ MTitle = "부위마취", STitle = "FNB", control = chkFNB },
                new ControlInfo{ MTitle = "부위마취", STitle = "SNB", control = chkSNB },
                new ControlInfo{ MTitle = "부위마취", STitle = "SNB P", control = chkSNB_P },
                new ControlInfo{ MTitle = "부위마취", STitle = "SNB G", control = chkSNB_G },
                new ControlInfo{ MTitle = "부위마취", STitle = "ACB", control = chkACB },
                new ControlInfo{ MTitle = "부위마취", STitle = "BPB", control = chkBPB },
                new ControlInfo{ MTitle = "부위마취", STitle = "BPB A", control = chkBPB_A },
                new ControlInfo{ MTitle = "부위마취", STitle = "BPB I", control = chkBPB_I },
                new ControlInfo{ MTitle = "부위마취", STitle = "기타", control = chkETC },
                new ControlInfo{ MTitle = "부위마취", STitle = "기타", control = txtEtc },

                new ControlInfo{ MTitle = "OP. Position", STitle = "Supine", control = chkSupine },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Prone", control = chkProne },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Lithotomy", control = chkLithotomy },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Jack-knife", control = chkJack },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Semi-folwer", control = chkSemi },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Lateral", control = chkLateral },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Lateral R", control = chkLateral_R },
                new ControlInfo{ MTitle = "OP. Position", STitle = "Lateral L", control = chkLateral_L },

                new ControlInfo{ MTitle = "Medication", STitle = "약속처방", control = cboYak },

                new ControlInfo{ MTitle = "Medication", control = ssMedication },
                new ControlInfo{ MTitle = "물품", control = ssJep },
                new ControlInfo{ MTitle = "PCA", control = ssPCA },
                new ControlInfo{ MTitle = "I/O IN", control = ssIn },
                new ControlInfo{ MTitle = "I/O OUT", control = ssOut },
                //new ControlInfo{ MTitle = "I/O PC", control = ssPC },
                new ControlInfo{ MTitle = "회복실", control = ssRecovery },
                new ControlInfo{ MTitle = "BST", control = ssBST },
            };

            #endregion
            if (AcpEmr != null)
            {
                btnBlood.Visible = mstrMode.Equals("W") && FormPatInfoFunc.Set_FormPatInfo_IsBlood(clsDB.DbCon, AcpEmr.ptNo, dtpOpDateFr.Value.ToString("yyyy-MM-dd"));
                if (AcpEmr.ptNo == "81000004")
                {
                    AcpEmr.acpNo = "999999";
                }
            }

            //AcpEmr = clsEmrChart.ClearPatient();
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "03468192", "I", "20170703", "MP");
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, AcpEmr.ptNo, AcpEmr.inOutCls, AcpEmr.medFrDate, AcpEmr.medDeptCd);
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "81000004", "I", DateTime.Now.ToString("yyyyMMdd"), "ME");
            //AcpEmr = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, "81000004", "I", "20191211", "ME");

            //  테스트 환자 ACPNO 없음
            //  운영에서는 빼야함
         
            //  현재시간 표시
            CurrentTimer = new Timer();
            CurrentTimer.Interval = 100;
            CurrentTimer.Tick += (s, evt) =>
            {
                DateTime dtm = DateTime.Now; //DateTime.ParseExact(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "yyyyMMddHHmmss", null);
                dtpNow.Value = dtm;
            };
            //CurrentTimer.Start();

            //  상단 환자정보
            //conPatInfo1.SetDisPlay("15344", "I", "2019-07-15", "81000004", "OSME");
            //  약속처방
            GetYakOrder();

            // 기록지 불러오기
            if (mstrEmrNo != "0")
            {
                SetData();
                chkOpStart.Checked = true;
                //chkOpStart.Enabled = false;

                if (ChkGeneralAnes.Checked == true)
                {
                    PanGeneralAnes.Visible = true;
                }
                if (ChkSiteAnes.Checked == true)
                {
                    PanSiteAnes.Visible = true;
                }
                if (rdoAttention_Y.Checked == true)
                {
                    PanAttention.Visible = true;
                }
            }

            // 스프레드 초기화
            ssMedication.ActiveSheet.RowCount = 58;
            ssJep.ActiveSheet.RowCount = 17;
            ssRecovery.ActiveSheet.RowCount = 10;
            ssPCA.ActiveSheet.RowCount = 19;

        }

        /// <summary>
        /// 버튼권한 설정
        /// </summary>
        private void BtnShowReg()
        {
            btnSave.Visible = false;
            btnDelete.Visible = false;
            btnPrint.Visible = false;

            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
            }

            if (clsType.User.AuAWRITE == "1")
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
            }

            if (System.Diagnostics.Debugger.IsAttached == true)
            {
                btnSave.Visible = true;
                btnDelete.Visible = true;
                btnPrint.Visible = true;
            }
        }


        /// <summary>
        /// 약속처방 아이템 
        /// </summary>
        private void GetYakOrder()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = SQL + ComNum.VBLF + "SELECT WRTNO, NAME";
                SQL = SQL + ComNum.VBLF + "  FROM OPR_GROUPMST";
                SQL = SQL + ComNum.VBLF + " WHERE BUCODE = '033103'";
                SQL = SQL + ComNum.VBLF + "ORDER BY NAME ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                cboYak.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    cboYak.Items.Add(string.Concat(row["WRTNO"].ToString(), ".", row["NAME"].ToString()));
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 스프레드 사안 시간설정
        /// </summary>
        /// <param name="index"></param>
        /// <param name="hhmm"></param>
        private void SetSpreadTime(int index, string hhmm = "")
        {
            DateTime dtp;
            string prevHour = string.Empty;
            if (string.IsNullOrWhiteSpace(hhmm))
            {
                dtp = CreateDate;
            }
            else
            {
                string dateTime = CreateDate.ToString("yyyyMMdd") + hhmm;
                dtp = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", null);
                prevHour = dtp.ToString("HH");

                dtp = dtp.AddMinutes(5);
            }

            ComplexBorder complexBorder = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
            for (int i = index; i < ssView.ActiveSheet.Columns.Count; i++)
            {
                //if (dtpOpDateEnd.Value.Day < dtp.Day)
                //{
                //    dtpOpDateEnd.Value = dtp;
                //}

                if (!prevHour.Equals(dtp.ToString("HH")))
                {
                    ssView.ActiveSheet.Columns.Get(i).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                    ssView.ActiveSheet.Cells[0, i].Border = complexBorder;
                    ssView.ActiveSheet.Cells[1, i].Border = complexBorder;
                }

                prevHour = dtp.ToString("HH");

                Column c = ssView.ActiveSheet.Columns[i];
                c.CellType = new TextCellType();
                c.VerticalAlignment = CellVerticalAlignment.Center;
                c.HorizontalAlignment = CellHorizontalAlignment.Center;

                //c.Width = 25;
                c.Width = 50;

                cboTime.Items.Add(dtp.ToString("HH:mm"));
                ssView.ActiveSheet.Cells[0, i].Tag = CreateDate.ToString("yyyy-MM-dd");
                ssView.ActiveSheet.Cells[0, i].Text = dtp.ToString("HH");
                ssView.ActiveSheet.Cells[1, i].Text = dtp.ToString("mm");

                dtp = dtp.AddMinutes(5);
                CreateDate = dtp;
            }
        }

        /// <summary>
        /// 그래프 마크설정
        /// </summary>
        /// <param name="index"></param>
        private void SetChartPointMarkers(int index)
        {
            //int max = ssView.ActiveSheet.Columns.Count - 2;
            ////int plus = 2;

            //if (index > 0)
            //{
            //    max = ssView.ActiveSheet.Columns.Count;
            //    plus = 0;
            //}

            //for (int i = index; i < max; i++)
            //{
            //    int grapeIndex = i + plus;
            //    string hhmm = string.Concat(ssView.ActiveSheet.Cells[0, grapeIndex].Text, ":", ssView.ActiveSheet.Cells[1, grapeIndex].Text);
            //    SBPSeries.CategoryNames.Add(hhmm);
            //    //  마크 사이즈 설정
            //    SBPSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 9F));
            //    DBPSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 9F));
            //    PulseSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 6F));
            //    BreathSeries.PointMarkers.SetMarker(i, new BuiltinMarker(MarkerShape.Square, 6F));
            //}
        }

        /// <summary>
        /// 저장
        /// </summary>
        private bool EmrMstSave(bool Cert)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);
            double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "EMRXMLHISNO");

            // 필수입력 체크
            if (CheckSaveException() == false)
            {
                return false;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            if (mstrEmrNo == "0")
            {
                mstrEmrNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "GETEMRXMLNO").ToString();
            }
            else
            {
                //  Emr 히스토리 저장
                if (!EmrChartHisSave(dblEmrHisNo, mstrEmrNo, strCurDate, strCurTime))
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    return false;
                }
            }

            //  Emr 차트 마스터 저장
            if (!EmrChartMstSave(dblEmrHisNo, mstrEmrNo, Cert ? "1" : "0"))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            #region 전자인증
            if (Cert)
            {
                if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.Sabun) == true)
                {
                    clsEmrQuery.SaveEmrCert(clsDB.DbCon, mstrEmrNo.To<double>(), false);
                }
            }
            #endregion

            //  기본 정보 - 아이템 저장 스프레드 제외
            if (!ChartItemSave(dblEmrHisNo, mstrEmrNo))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            // 스프레드 그래프 저장 
            if (!ChartGrapeSave(dblEmrHisNo, mstrEmrNo))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            // 콤보박스 아이템 저장
            if (!ChartComboItemSave(dblEmrHisNo, mstrEmrNo))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            return true;
        }

        /// <summary>
        /// 삭제
        /// </summary>
        private bool EmrMstDelete()
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            string strChartDate = string.Empty;
            string strChartTime = string.Empty;

            if (mstrEmrNo == "0") return false;

            if (VB.Val(mstrEmrNo) != 0)
            {
                //if (clsEmrQuery.IsChangeAuthOld(clsDB.DbCon, mstrEmrNo, clsType.User.IdNumber) == false)
                //{
                //    return false;
                //}

                //if (clsEmrQuery.READ_CHART_APPLY(this, mstrEmrNo) == false)
                //{
                //    return false;
                //}

                //if (clsEmrQuery.READ_PRTLOG(this, mstrEmrNo) == false)
                //{
                //    return false;
                //}

                if (ComFunc.MsgBoxQ("기존내용을 삭제하시겠습니까?") == DialogResult.No)
                {
                    return false;
                }
            }

            //double dblEmrHisNo = ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "EMRXMLHISNO");
            double dblEmrHisNo = -1;// ComQuery.GetSequencesNo(clsDB.DbCon, "" + ComNum.DB_EMR + "EMRXMLHISNO");

            clsDB.setBeginTran(clsDB.DbCon);

            //  Emr 히스토리 저장 및 삭제
            if (!EmrChartHisSave(dblEmrHisNo, mstrEmrNo, strCurDate, strCurTime))
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return false;
            }

            clsDB.setCommitTran(clsDB.DbCon);

            return true;
        }

        private bool ChartGrapeSave(double dblEmrHisNo, string emrNo)
        {
            string item = string.Empty;
            string unit = string.Empty;
            string itemCode = string.Empty;
            string hour = string.Empty;
            string minute = string.Empty;
            string value = string.Empty;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            try
            {
                int maxCount = Medcations.ToList().FindAll(r => r.IsView).Count + 2;
                var list = Medcations.ToList().FindAll(r => r.IsView);


                for (int i = 2; i < maxCount; i++)
                {

                    item = ssView.ActiveSheet.Cells[i, 0].Text;

                    MedcationInfo medcation = Medcations.ToList().Find(r => r.Name.Equals(item));

                    if (medcation != null)
                    {
                        itemCode = medcation.ItemNo;
                        unit = medcation.Uint;
                    }

                    for (int j = 2; j < ssView.ActiveSheet.ColumnCount; j++)
                    {
                        SQL = "";
                        SqlErr = ""; //에러문 받는 변수
                        intRowAffected = 0;

                        hour = ssView.ActiveSheet.Cells[0, j].Text;
                        minute = ssView.ActiveSheet.Cells[1, j].Text;
                        value = ssView.ActiveSheet.Cells[i, j].Text;
                        // 호흡종류
                        if (item == "호흡")
                        {
                            unit = ssView.ActiveSheet.Cells[i + 1, j].Text;
                        }

                        // Tourniquet
                        if (item == "Tourniquet")
                        {
                            if (ssView.ActiveSheet.Cells[i, j].Text != "")
                            {
                                unit = ssView.ActiveSheet.Cells[i, j].Tag.ToString();
                            }
                            else
                            {
                                unit = medcation.Uint;
                            }
                        }

                        SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRANCHARTGRAPE(";
                        SQL = SQL + ComNum.VBLF + "    PTNO";
                        SQL = SQL + ComNum.VBLF + "  , EMRNO";
                        SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "  , FORMNO";
                        SQL = SQL + ComNum.VBLF + "  , ITEM";
                        SQL = SQL + ComNum.VBLF + "  , ITEMCODE";
                        SQL = SQL + ComNum.VBLF + "  , HOUR";
                        SQL = SQL + ComNum.VBLF + "  , MINUTE";
                        SQL = SQL + ComNum.VBLF + "  , VALUE";
                        SQL = SQL + ComNum.VBLF + "  , UNIT";
                        SQL = SQL + ComNum.VBLF + "  , COLINDEX";
                        SQL = SQL + ComNum.VBLF + "  , ROWINDEX";
                        SQL = SQL + ComNum.VBLF + ") VALUES (";
                        SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "'    --  PTNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + emrNo + "'          --  EMRNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + dblEmrHisNo + "'    --  EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "  , '" + mstrFormNo + "'         --  FORMNO";
                        SQL = SQL + ComNum.VBLF + "  , '" + item + "'           --  ITEM";
                        SQL = SQL + ComNum.VBLF + "  , '" + itemCode + "'       --  ITEMCODE";
                        SQL = SQL + ComNum.VBLF + "  , '" + hour + "'           --  HOUR";
                        SQL = SQL + ComNum.VBLF + "  , '" + minute + "'         --  MINUTE";
                        SQL = SQL + ComNum.VBLF + "  , '" + value + "'          --  VALUE";
                        SQL = SQL + ComNum.VBLF + "  , '" + unit + "'           --  UNIT";
                        SQL = SQL + ComNum.VBLF + "  , '" + j + "'              --  COLINDEX";
                        SQL = SQL + ComNum.VBLF + "  , '" + i + "'              --  ROWINDEX";
                        SQL = SQL + ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                    }

                    //  그래프 값을 추출 하기 위해서
                    //  로우카운트 및 i 변경
                    //  그래프 값은 100로우부터 시작한다.
                    //if (i < 100 && i == (maxCount - 1))
                    //{
                    //    maxCount = 104;
                    //    i = 99;
                    //}
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        /// <summary>
        /// ! Important - formatted as bold.
        /// TODO: Task - colored light orange.
        /// </summary>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="emrNo"></param>
        /// <returns></returns>
        private bool ChartComboItemSave(double dblEmrHisNo, string emrNo)
        {
            int i = 1;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            try
            {

                foreach (MedcationInfo medcation in Medcations)
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRANCHARTITEM (";
                    SQL = SQL + ComNum.VBLF + "    PTNO";
                    SQL = SQL + ComNum.VBLF + "  , EMRNO";
                    SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "  , FORMNO";
                    SQL = SQL + ComNum.VBLF + "  , ITEMCODE";
                    SQL = SQL + ComNum.VBLF + "  , ITEM";
                    SQL = SQL + ComNum.VBLF + "  , UNIT";
                    SQL = SQL + ComNum.VBLF + "  , ISWRITE";
                    SQL = SQL + ComNum.VBLF + "  , ISVIEW";
                    SQL = SQL + ComNum.VBLF + "  , ROWINDEX";
                    SQL = SQL + ComNum.VBLF + "  , SORT";
                    SQL = SQL + ComNum.VBLF + ") VALUES (";
                    SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "'                            --  PTNO";
                    SQL = SQL + ComNum.VBLF + "  , '" + emrNo + "'                                  --  EMRNO";
                    SQL = SQL + ComNum.VBLF + "  , '" + dblEmrHisNo + "'                            --  EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "  , '" + mstrFormNo + "'                             --  FORMNO";
                    SQL = SQL + ComNum.VBLF + "  , '" + medcation.ItemNo + "'                       --  ItemNo";
                    SQL = SQL + ComNum.VBLF + "  , '" + medcation.Name + "'                         --  Name";
                    SQL = SQL + ComNum.VBLF + "  , '" + medcation.Uint + "'                         --  Uint";
                    SQL = SQL + ComNum.VBLF + "  , '" + (medcation.IsWrite == true ? 1 : 0) + "'    --  IsWrite";
                    SQL = SQL + ComNum.VBLF + "  , '" + (medcation.IsView == true ? 1 : 0) + "'     --  IsView";
                    SQL = SQL + ComNum.VBLF + "  , '" + medcation.Row + "'                          --  COLINDEX";
                    SQL = SQL + ComNum.VBLF + "  , '" + i + "'                                      --  ROWINDEX";
                    SQL = SQL + ComNum.VBLF + ")";

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return false;
                    }

                    i = i + 1;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        /// <summary>
        /// 컨트롤 저장
        /// </summary>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="emrNo"></param>
        /// <returns></returns>
        private bool ChartItemSave(double dblEmrHisNo, string emrNo)
        {
            foreach (ControlInfo item in ControlInfos)
            {
                string title = item.MTitle;
                string subTitle = item.STitle;
                string formNo = mstrFormNo;
                string controlName = item.control.Name;
                object value = null;

                if (!(item.control is FpSpread))
                {
                    value = GetControlValue(item.control);

                    if (!ControlItemSave(dblEmrHisNo, emrNo, item.MTitle, item.STitle, item.control.Name, value, "0"))
                    {
                        return false;
                    }
                }
                else
                {
                    FpSpread fpSpread = item.control as FpSpread;
                    for (int i = 0; i < fpSpread.ActiveSheet.RowCount; i++)
                    {
                        //  첫번째 컬럼이 빈값이면 DB저장을 하지 않는다.
                        if (string.IsNullOrWhiteSpace(fpSpread.ActiveSheet.Cells[i, 0].Text))
                        {
                            continue;
                        }

                        for (int j = 0; j < fpSpread.ActiveSheet.ColumnCount; j++)
                        {
                            value = fpSpread.ActiveSheet.Cells[i, j].Text;
                            if (!ControlItemSave(dblEmrHisNo, emrNo, item.MTitle, item.STitle, item.control.Name, value, "1", j, i))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        #region Emr차트 히스토리 && Emr차트 마스터 저장

        /// <summary>
        /// Emr 히스토리 저장
        /// </summary>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="emrNo"></param>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        private bool EmrChartHisSave(double dblEmrHisNo, string emrNo, string strCurDate, string strCurTime)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            //DataTable dt = null;
            int intRowAffected = 0;

            try
            {

                #region 쿼리

                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMSTHIS(";
                SQL = SQL + ComNum.VBLF + "    EMRNO";
                SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  , ACPNO";
                SQL = SQL + ComNum.VBLF + "  , FORMNO";
                SQL = SQL + ComNum.VBLF + "  , UPDATENO";
                SQL = SQL + ComNum.VBLF + "  , CHARTDATE";
                SQL = SQL + ComNum.VBLF + "  , CHARTTIME";
                SQL = SQL + ComNum.VBLF + "  , CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "  , WRITEDATE";
                SQL = SQL + ComNum.VBLF + "  , WRITETIME";
                SQL = SQL + ComNum.VBLF + "  , COMPUSEID";
                SQL = SQL + ComNum.VBLF + "  , COMPDATE";
                SQL = SQL + ComNum.VBLF + "  , COMPTIME";
                SQL = SQL + ComNum.VBLF + "  , PRNTYN";
                SQL = SQL + ComNum.VBLF + "  , SAVEGB";
                SQL = SQL + ComNum.VBLF + "  , SAVECERT";
                SQL = SQL + ComNum.VBLF + "  , FORMGB";
                SQL = SQL + ComNum.VBLF + "  , PTNO";
                SQL = SQL + ComNum.VBLF + "  , INOUTCLS";
                SQL = SQL + ComNum.VBLF + "  , MEDFRDATE";
                SQL = SQL + ComNum.VBLF + "  , MEDFRTIME";
                SQL = SQL + ComNum.VBLF + "  , MEDENDDATE";
                SQL = SQL + ComNum.VBLF + "  , MEDENDTIME";
                SQL = SQL + ComNum.VBLF + "  , MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "  , MEDDRCD";
                SQL = SQL + ComNum.VBLF + "  , OPDATE";
                SQL = SQL + ComNum.VBLF + "  , OPDEGREE";
                SQL = SQL + ComNum.VBLF + "  , OP_DEPT";
                SQL = SQL + ComNum.VBLF + "  , DEPTCDNOW";
                SQL = SQL + ComNum.VBLF + "  , DRCDNOW";
                SQL = SQL + ComNum.VBLF + "  , ROOM_NO";
                SQL = SQL + ComNum.VBLF + "  , ACPNOOUT";
                SQL = SQL + ComNum.VBLF + "  , CURDEPT";
                SQL = SQL + ComNum.VBLF + "  , CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO";
                SQL = SQL + ComNum.VBLF + "  , EMRNODC";
                SQL = SQL + ComNum.VBLF + "  , DCEMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  , DCUSEID";
                SQL = SQL + ComNum.VBLF + "  , DCDATE";
                SQL = SQL + ComNum.VBLF + "  , DCTIME";
                SQL = SQL + ComNum.VBLF + ")";
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO                                         --  차트일렵번호";
                SQL = SQL + ComNum.VBLF + "     , EMRNOHIS                                      --  차트수정일련번호";
                SQL = SQL + ComNum.VBLF + "     , ACPNO                                         --  접수번호";
                SQL = SQL + ComNum.VBLF + "     , FORMNO                                        --  서식지번호";
                SQL = SQL + ComNum.VBLF + "     , UPDATENO                                      --  업데이트번호";
                SQL = SQL + ComNum.VBLF + "     , CHARTDATE                                     --  차트일자";
                SQL = SQL + ComNum.VBLF + "     , CHARTTIME                                     --  차트시간";
                SQL = SQL + ComNum.VBLF + "     , CHARTUSEID                                    --  작성자";
                SQL = SQL + ComNum.VBLF + "     , WRITEDATE                                     --  입력일자";
                SQL = SQL + ComNum.VBLF + "     , WRITETIME                                     --  입력시간";
                SQL = SQL + ComNum.VBLF + "     , COMPUSEID                                     --  확인자";
                SQL = SQL + ComNum.VBLF + "     , COMPDATE                                      --  확인일자";
                SQL = SQL + ComNum.VBLF + "     , COMPTIME                                      --  확인시간";
                SQL = SQL + ComNum.VBLF + "     , PRNTYN                                        --  외부출력여부";
                SQL = SQL + ComNum.VBLF + "     , SAVEGB                                        --  0 : 임시저장, 1 : 확정저장";
                SQL = SQL + ComNum.VBLF + "     , SAVECERT                                      --  인증여부";
                SQL = SQL + ComNum.VBLF + "     , FORMGB                                        --  기록지종류 0 : 기본, 1 : 스캔, 2 : 이미지";
                SQL = SQL + ComNum.VBLF + "     , PTNO                                          --  환자번호";
                SQL = SQL + ComNum.VBLF + "     , INOUTCLS                                      --  외래, 입원구분";
                SQL = SQL + ComNum.VBLF + "     , MEDFRDATE                                     --  내원일자";
                SQL = SQL + ComNum.VBLF + "     , MEDFRTIME                                     --  내원시간";
                SQL = SQL + ComNum.VBLF + "     , MEDENDDATE                                    --  퇴원일자";
                SQL = SQL + ComNum.VBLF + "     , MEDENDTIME                                    --  퇴원시간";
                SQL = SQL + ComNum.VBLF + "     , MEDDEPTCD                                     --  진료과";
                SQL = SQL + ComNum.VBLF + "     , MEDDRCD                                       --  주치의";
                SQL = SQL + ComNum.VBLF + "     , OPDATE                                        --  수술일자";
                SQL = SQL + ComNum.VBLF + "     , OPDEGREE                                      --  순번";
                SQL = SQL + ComNum.VBLF + "     , OP_DEPT                                       --  수술진료과";
                SQL = SQL + ComNum.VBLF + "     , DEPTCDNOW                                     --  차트시 진료과";
                SQL = SQL + ComNum.VBLF + "     , DRCDNOW                                       --  차트시 주치의";
                SQL = SQL + ComNum.VBLF + "     , ROOM_NO                                       --  룸번호";
                SQL = SQL + ComNum.VBLF + "     , ACPNOOUT                                      ";
                SQL = SQL + ComNum.VBLF + "     , CURDEPT                                       --  현재사용자과";
                SQL = SQL + ComNum.VBLF + "     , CURGRADE, CODEUSEID, CODEDATE, CODETIME, CERTNO        --  ";
                SQL = SQL + ComNum.VBLF + "     , '" + emrNo + "'                               --  ";
                SQL = SQL + ComNum.VBLF + "     , '" + dblEmrHisNo + "' AS DCEMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     , '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "     , '" + strCurDate + "'";
                SQL = SQL + ComNum.VBLF + "     , '" + strCurTime + "'";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO = '" + emrNo + "'";

                #endregion

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                SQL = "";
                SQL = SQL + ComNum.VBLF + "DELETE FROM  " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + emrNo;

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        /// <summary>
        /// 차트 마스터 작성
        /// </summary>
        /// <param name="dblEmrHisNo"></param>
        /// <param name="emrNo"></param>
        /// <param name="saveCert">0: 임시, 1: 인증</param>
        /// <returns></returns>
        private bool EmrChartMstSave(double dblEmrHisNo, string emrNo, string saveCert)
        {
            string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
            string strCurDate = VB.Left(strCurDateTime, 8);
            string strCurTime = VB.Right(strCurDateTime, 6);

            string strChartDate = string.Empty;
            string strChartTime = string.Empty;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수


            if (string.IsNullOrEmpty(mstrCHARTDATE) || string.IsNullOrEmpty(mstrCHARTTIME))
            {
                strChartDate = strCurDate;
                strChartTime = strCurTime;
            }
            else
            {
                strChartDate = mstrCHARTDATE;
                strChartTime = mstrCHARTTIME;
            }

            if (chkChartDateTime.Checked == true)
            {
                strChartDate = dtpChartDateTime.Value.ToString("yyyyMMdd");
                strChartTime = dtpChartDateTime.Value.ToString("HHmm") + "00";
            }

            mstrCHARTDATE = strChartDate;
            mstrCHARTTIME = strChartTime;
            mstrCHARTUSEID = clsType.User.IdNumber;

            try
            {
                #region 쿼리

                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRCHARTMST(";
                SQL = SQL + ComNum.VBLF + "    EMRNO";
                SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  , ACPNO";
                SQL = SQL + ComNum.VBLF + "  , FORMNO";
                SQL = SQL + ComNum.VBLF + "  , UPDATENO";
                SQL = SQL + ComNum.VBLF + "  , CHARTDATE";
                SQL = SQL + ComNum.VBLF + "  , CHARTTIME";
                SQL = SQL + ComNum.VBLF + "  , CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "  , WRITEDATE";
                SQL = SQL + ComNum.VBLF + "  , WRITETIME";
                SQL = SQL + ComNum.VBLF + "  , COMPUSEID";
                SQL = SQL + ComNum.VBLF + "  , COMPDATE";
                SQL = SQL + ComNum.VBLF + "  , COMPTIME";
                SQL = SQL + ComNum.VBLF + "  , PRNTYN";
                SQL = SQL + ComNum.VBLF + "  , SAVEGB";
                SQL = SQL + ComNum.VBLF + "  , SAVECERT";
                SQL = SQL + ComNum.VBLF + "  , FORMGB";
                SQL = SQL + ComNum.VBLF + "  , PTNO";
                SQL = SQL + ComNum.VBLF + "  , INOUTCLS";
                SQL = SQL + ComNum.VBLF + "  , MEDFRDATE";
                SQL = SQL + ComNum.VBLF + "  , MEDFRTIME";
                SQL = SQL + ComNum.VBLF + "  , MEDENDDATE";
                SQL = SQL + ComNum.VBLF + "  , MEDENDTIME";
                SQL = SQL + ComNum.VBLF + "  , MEDDEPTCD";
                SQL = SQL + ComNum.VBLF + "  , MEDDRCD";
                SQL = SQL + ComNum.VBLF + "  , OPDATE";
                SQL = SQL + ComNum.VBLF + "  , OPDEGREE";
                SQL = SQL + ComNum.VBLF + "  , OP_DEPT";
                SQL = SQL + ComNum.VBLF + "  , DEPTCDNOW";
                SQL = SQL + ComNum.VBLF + "  , DRCDNOW";
                SQL = SQL + ComNum.VBLF + "  , ROOM_NO";
                SQL = SQL + ComNum.VBLF + "  , ACPNOOUT";
                SQL = SQL + ComNum.VBLF + "  , CURDEPT";
                SQL = SQL + ComNum.VBLF + "  , CURGRADE";
                SQL = SQL + ComNum.VBLF + ") VALUES (";
                SQL = SQL + ComNum.VBLF + "    '" + emrNo + "'                                      --  EMRNO       차트번호";
                SQL = SQL + ComNum.VBLF + "  , '" + dblEmrHisNo + "'                                --  EMRNOHIS    차트히스토리번호";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.acpNo + "'                               --  ACPNO       접수번호";
                SQL = SQL + ComNum.VBLF + "  , " + mstrFormNo + "                                   --  FORMNO      서식지번호";
                SQL = SQL + ComNum.VBLF + "  , " + mstrUpdateNo + "                                 --  UPDATENO    업데이트 번호";
                SQL = SQL + ComNum.VBLF + "  , '" + strChartDate + "'                               --  CHARTDATE   차트일자";
                SQL = SQL + ComNum.VBLF + "  , '" + strChartTime + "'                               --  CHARTTIME   차트시간";
                SQL = SQL + ComNum.VBLF + "  , '" + clsType.User.IdNumber + "'                      --  CHARTUSEID  작성자";
                SQL = SQL + ComNum.VBLF + "  , '" + strCurDate + "'                                 --  WRITEDATE   입력일자";
                SQL = SQL + ComNum.VBLF + "  , '" + strCurTime + "'                                 --  WRITETIME   입력시간";

                //  확인자
                if ("1".Equals("1"))
                {
                    SQL = SQL + ComNum.VBLF + "  , '" + clsType.User.IdNumber + "'                  --  COMPUSEID   확인자";
                    SQL = SQL + ComNum.VBLF + "  , '" + strCurDate + "'                             --  COMPDATE    확인일";
                    SQL = SQL + ComNum.VBLF + "  , '" + strCurTime + "'                             --  COMPTIME    확인시간";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  COMPUSEID   확인자";
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  COMPDATE    확인일";
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  COMPTIME    확인시간";
                }

                SQL = SQL + ComNum.VBLF + "  , ''                                                   --  PRNTYN      출력여부";
                SQL = SQL + ComNum.VBLF + "  , '0'                                                  --  SAVEGB      저장여부 0 : 임시저장, 1 : 확정저장";
                SQL = SQL + ComNum.VBLF + "  , '" + saveCert + "'                                   --  SAVECERT    인증여부 0 : 임시저장, 1 : 확정저장";
                SQL = SQL + ComNum.VBLF + "  , '0'                                                  --  FORMGB      기록지종류 0 : 기본, 1 : 스캔, 2 : 이미지";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.ptNo + "'                                --  PTNO    환자번호";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.inOutCls + "'                            --  INOUTCLS    입/외래 구분";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medFrDate + "'                           --  MEDFRDATE   내원일자";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medFrTime + "'                           --  MEDFRTIME   내원시간";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medEndDate + "'                          --  MEDENDDATE  퇴원일자";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medEndTime + "'                          --  MEDENDTIME  퇴원시간";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medDeptCd + "'                           --  MEDDEPTCD   진료과";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.medDrCd + "'                             --  MEDDRCD     진료의";

                if (!string.IsNullOrWhiteSpace(AcpEmr.opdept))
                {
                    SQL = SQL + ComNum.VBLF + "  , TO_DATE('" + AcpEmr.opdate + "', 'YYYY-MM-DD')   --  OPDATE      수술일자";
                    SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.opdegree + "'                        --  OPDEGREE    순번";
                    SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.opdept + "'                          --  OP_DEPT     수술 진료과";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  OPDATE      수술일자";
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  OPDEGREE    순번";
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  OP_DEPT     수술 진료과";
                }

                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.nowdeptcd + "'                           --  DEPTCDNOW   차트시 진료과";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.nowdrcd + "'                             --  DRCDNOW     차트시 주치의";
                SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.nowroomno + "'                           --  ROOM_NO     룹번호";
                SQL = SQL + ComNum.VBLF + "  , 0                                                    --  ACPNOOUT";

                if (string.IsNullOrWhiteSpace(AcpEmr.cur_Dept))
                {
                    SQL = SQL + ComNum.VBLF + "  , '" + AcpEmr.cur_Dept + "'                        --  CURDEPT     현재사용자과";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  , ''                                               --  CURDEPT";
                }
                SQL = SQL + ComNum.VBLF + "  , ''                                                   --  CURGRADE";
                SQL = SQL + ComNum.VBLF + ") ";

                #endregion

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                    return false;
                }
                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        #endregion

        /// <summary>
        /// 컨트롤 아이템 저장
        /// </summary>
        /// <param name="dblEmrHisNo">차트 히스토리 일련번호</param>
        /// <param name="emrNo">차트 일련번호</param>
        /// <param name="title">아이템 대분류</param>
        /// <param name="subTitle">아이템 소분류</param>
        /// <param name="controlName">컨트롤 명</param>
        /// <param name="value">값</param>
        /// <param name="controlType">컨트롤 타입 0 : 기본컨트롤, 1 : 스프레드</param>
        /// <param name="col">스프레드 columnIndex</param>
        /// <param name="row">스프레드 rowIndex</param>
        /// <returns></returns>
        private bool ControlItemSave(double dblEmrHisNo, string emrNo, string title, string subTitle, string controlName, object value, string controlType, int? col = null, int? row = null)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            try
            {
                #region 쿼리

                SQL = SQL + ComNum.VBLF + "INSERT INTO " + ComNum.DB_EMR + "AEMRANCHART(";
                SQL = SQL + ComNum.VBLF + "    PTNO";
                SQL = SQL + ComNum.VBLF + "  , EMRNO";
                SQL = SQL + ComNum.VBLF + "  , EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "  , FORMNO";
                SQL = SQL + ComNum.VBLF + "  , TITLE";
                SQL = SQL + ComNum.VBLF + "  , SUBTITLE";
                SQL = SQL + ComNum.VBLF + "  , CONTROLNAME";
                SQL = SQL + ComNum.VBLF + "  , CONTROLVALUE";
                SQL = SQL + ComNum.VBLF + "  , CONTROLTYPE";
                SQL = SQL + ComNum.VBLF + "  , COLINDEX";
                SQL = SQL + ComNum.VBLF + "  , ROWINDEX";
                SQL = SQL + ComNum.VBLF + ") VALUES (";
                SQL = SQL + ComNum.VBLF + "    '" + AcpEmr.ptNo + "'            --  PTNO                환자번호 ";
                SQL = SQL + ComNum.VBLF + "  , '" + emrNo + "'                  --  EMRNO               차트일련번호";
                SQL = SQL + ComNum.VBLF + "  , '" + dblEmrHisNo + "'            --  EMRNOHIS            차트히스토리 일련번호";
                SQL = SQL + ComNum.VBLF + "  , '" + mstrFormNo + "'                 --  FORMNO              서식지 번호";
                SQL = SQL + ComNum.VBLF + "  , '" + title + "'                  --  TITLE               컨트롤 대분류";
                SQL = SQL + ComNum.VBLF + "  , '" + subTitle + "'               --  SUBTITLE            컨트롤 소분류";
                SQL = SQL + ComNum.VBLF + "  , '" + controlName + "'            --  CONTROLNAME         컨트롤명";
                SQL = SQL + ComNum.VBLF + "  , '" + value + "'                  --  CONTROLVALUE        컨트롤 값";
                SQL = SQL + ComNum.VBLF + "  , '" + controlType + "'            --  CONTROLTYPE         0 : 기본컨트롤, 1 : 스프레드";
                SQL = SQL + ComNum.VBLF + "  , '" + col + "'                    --  COLINDEX            스프레드 컬럼";
                SQL = SQL + ComNum.VBLF + "  , '" + row + "'                    --  ROWINDEX            스프레드 로우";
                SQL = SQL + ComNum.VBLF + ")";

                #endregion

                SqlErr = clsDB.ExecuteNonQueryEx(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }

            return true;
        }

        /// <summary>
        /// 데이터 바인딩
        /// </summary>
        private void SetData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region 기본정보 불러오기
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT EMRNO, EMRNOHIS, FORMNO, UPDATENO, CHARTDATE, CHARTTIME, CHARTUSEID";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
                SQL = SQL + ComNum.VBLF + " WHERE PTNO    = '" + AcpEmr.ptNo + "'";
                SQL = SQL + ComNum.VBLF + "   AND FORMNO  = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "   AND EMRNO   = " + mstrEmrNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt == null || dt.Rows.Count == 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회 된 내용이 없습니다.");
                    return;
                }

                //mstrUpdateNo = dt.Rows[0]["UPDATENO"].ToString();
                //string emrNo = dt.Rows[0]["EMRNO"].ToString();
                mstrEmrHisNo = dt.Rows[0]["EMRNOHIS"].ToString();
                mstrCHARTDATE = dt.Rows[0]["CHARTDATE"].ToString().Trim();
                mstrCHARTTIME = dt.Rows[0]["CHARTTIME"].ToString().Trim();
                mstrCHARTUSEID = dt.Rows[0]["CHARTUSEID"].ToString().Trim();

                dtpChartDateTime.Value = DateTime.ParseExact(mstrCHARTDATE + mstrCHARTTIME, "yyyyMMddHHmmss", null);

                //emrNo = "1032";
                //emrNoHis = "25410";
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT PTNO, EMRNO, EMRNOHIS";
                SQL = SQL + ComNum.VBLF + "     , FORMNO, TITLE, SUBTITLE";
                SQL = SQL + ComNum.VBLF + "     , CONTROLNAME, CONTROLVALUE";
                SQL = SQL + ComNum.VBLF + "     , COLINDEX, ROWINDEX";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRANCHART";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO     = " + mstrEmrNo;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS  = " + mstrEmrHisNo;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssIn.ActiveSheet.RowCount = 0;
                ssOut.ActiveSheet.RowCount = 0;

                foreach (DataRow row in dt.Rows)
                {
                    ControlInfo item = ControlInfos.ToList().Find(r => r.control.Name.Equals(row["CONTROLNAME"].ToString()));
                    if (item != null)
                    {
                        if (item.control is FpSpread)
                        {
                            SetSpreadValue(item.control, row["CONTROLVALUE"].ToString(), (int)VB.Val(row["ROWINDEX"].ToString()), (int)VB.Val(row["COLINDEX"].ToString()));
                        }
                        else
                        {
                            SetControlValue(item.control, row["CONTROLVALUE"].ToString());
                        }
                    }
                }

                #endregion

                // Formula Set
                SetFormulaIntake();
                SetFormulaOutput();


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ITEM, ITEMCODE, HOUR, MINUTE";
                SQL = SQL + ComNum.VBLF + "     , VALUE, UNIT, COLINDEX, ROWINDEX";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRANCHARTGRAPE";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO    = " + mstrEmrNo;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS = " + mstrEmrHisNo;
                SQL = SQL + ComNum.VBLF + "ORDER BY ROWINDEX   ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                int maxCol = int.Parse(dt.AsEnumerable().Max(row => row["COLINDEX"]).ToString());

                var query = from row in dt.AsEnumerable()
                            group row by new
                            {
                                Item = row["ITEM"],
                                ItemNo = row["ITEMCODE"],
                                ItemUnit = row["UNIT"],
                                RowIndex = row["ROWINDEX"]
                            } into grp
                            select new
                            {
                                ItemNo = grp.Key.ItemNo.ToString(),
                                Item = grp.Key.Item.ToString(),
                                ItemUnit = grp.Key.ItemUnit.ToString(),
                                RowIndex = int.Parse(grp.Key.RowIndex.ToString())
                            };
                List<MedcationInfo> SpdMedcations = new List<MedcationInfo>();
                int i = 4;
                foreach (var item in query)
                {
                    SpdMedcations.Add(new MedcationInfo
                    {
                        Name = item.Item,
                        ItemNo = item.ItemNo,
                        Row = item.RowIndex,
                        Uint = item.ItemUnit,
                        IsView = !(item.RowIndex >= 100)
                    });
                }
                //  스프레드 컬럼/로우 설정
                ssView.ActiveSheet.Columns.Clear();
                ssView.ActiveSheet.Rows.Clear();

                //ssView.ActiveSheet.ColumnCount = maxCol - 1;
                ssView.ActiveSheet.ColumnCount = maxCol + 1;
                ssView.ActiveSheet.Rows.Count = 200;
                ssView.ActiveSheet.RowHeader.Cells[0, 0, 199, 0].Text = " ";
                ssView.ActiveSheet.SetRowHeight(-1, 18);

                ssView.ActiveSheet.Rows[0].MergePolicy = MergePolicy.Always;

                var timeList = from row in dt.AsEnumerable()
                               group row by new
                               {
                                   Hour = row["HOUR"],
                                   Minute = row["MINUTE"],
                                   ColIndex = row["COLINDEX"]
                               }
                               into grp
                               select new
                               {
                                   Hour = grp.Key.Hour.ToString(),
                                   Minute = grp.Key.Minute.ToString(),
                                   ColIndex = int.Parse(grp.Key.ColIndex.ToString())
                               };

                //timeList = timeList.OrderBy(r => r.ColIndex).ThenBy(a => a.Minute);
                timeList = timeList.OrderBy(r => r.ColIndex);

                i = 2;
                string prevHour = string.Empty;
                cboTime.Items.Clear();
                cboTime.SelectedIndex = -1;
                ComplexBorder complexBorder = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                foreach (var item in timeList)
                {
                    if (i == 2)
                    {
                        dtpNow.Value = Convert.ToDateTime(dtpOpDateFr.Value.ToString("yyyy-MM-dd") + " " + item.Hour + ":" + item.Minute);
                        CreateDate = Convert.ToDateTime(dtpOpDateFr.Value.ToString("yyyy-MM-dd") + " " + item.Hour + ":" + item.Minute);
                    }
                    else
                    {
                        if (CreateDate > Convert.ToDateTime(dtpOpDateFr.Value.ToString("yyyy-MM-dd") + " " + item.Hour + ":" + item.Minute))
                        {
                            CreateDate = Convert.ToDateTime(dtpOpDateFr.Value.AddDays(1).ToString("yyyy-MM-dd") + " " + item.Hour + ":" + item.Minute);
                        }
                        else
                        {
                            CreateDate = Convert.ToDateTime(dtpOpDateFr.Value.ToString("yyyy-MM-dd") + " " + item.Hour + ":" + item.Minute);
                        }
                    }

                    Column c = ssView.ActiveSheet.Columns[i];
                    c.CellType = new TextCellType();
                    c.VerticalAlignment = CellVerticalAlignment.Center;
                    c.HorizontalAlignment = CellHorizontalAlignment.Center;

                    if (!prevHour.Equals(item.Hour))
                    {
                        ssView.ActiveSheet.Columns.Get(i).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                        ssView.ActiveSheet.Cells[0, i].Border = complexBorder;
                        ssView.ActiveSheet.Cells[1, i].Border = complexBorder;
                    }
                    ssView.ActiveSheet.Cells[0, i].Tag = CreateDate.ToString("yyyy-MM-dd");
                    ssView.ActiveSheet.Cells[0, i].Text = item.Hour;
                    ssView.ActiveSheet.Cells[1, i].Text = item.Minute;

                    ssView.ActiveSheet.Cells[0, i].Locked = true;
                    ssView.ActiveSheet.Cells[1, i].Locked = true;

                    prevHour = item.Hour;

                    cboTime.Items.Add(item.Hour + ":" + item.Minute);

                    //c.Width = 25;
                    c.Width = 50;
                    i++;
                }


                foreach (DataRow row in dt.Rows)
                {
                    int rowIndex = int.Parse(row["ROWINDEX"].ToString());
                    int colInde = int.Parse(row["COLINDEX"].ToString());

                    ssView.ActiveSheet.RowHeader.Cells[rowIndex, 0].Text = BasicMedcationInfo.Where(d => d.ItemNo == row["ITEMCODE"].ToString()).FirstOrDefault().GroupName;
                    ssView.ActiveSheet.Cells[rowIndex, 0].Text = row["ITEM"].ToString();

                    if (row["ITEM"].ToString() != "호흡" && row["ITEM"].ToString() != "Tourniquet")
                    {
                        ssView.ActiveSheet.Cells[rowIndex, 1].Text = row["UNIT"].ToString();
                    }

                    ssView.ActiveSheet.Cells[rowIndex, colInde].Text = row["VALUE"].ToString();

                    double dVal = row["VALUE"].ToString().To<double>(0);

                    if (row["ITEM"].ToString().Equals("HR")) //맥박
                    {
                        if (dVal < 51 || dVal > 100)
                        {
                            ssView.ActiveSheet.Cells[rowIndex, colInde].ForeColor = Color.Red;
                        }
                    }

                    if (row["ITEM"].ToString().Equals("SBP"))
                    {
                        if (dVal < 101 || dVal > 199)
                        {
                            ssView.ActiveSheet.Cells[rowIndex, colInde].ForeColor = Color.Red;
                        }
                    }

                    //호흡수 RR
                    if (row["ITEM"].ToString().Equals("RR"))
                    {
                        if (dVal < 9 || dVal > 14)
                        {
                            ssView.ActiveSheet.Cells[rowIndex, colInde].ForeColor = Color.Red;
                        }
                    }

                    //체온 BT
                    if (row["ITEM"].ToString().Equals("BT"))
                    {
                        if (dVal < 36.1 || dVal > 37.4)
                        {
                            ssView.ActiveSheet.Cells[rowIndex, colInde].ForeColor = Color.Red;
                        }
                    }

                    if (row["ITEM"].ToString() == "Tourniquet")
                    {
                        ssView.ActiveSheet.Cells[rowIndex, colInde].Tag = row["UNIT"].ToString();
                    }

                }

                SetViewRowHead();

                //ssView.ActiveSheet.Columns[0].Width = 80;
                ssView.ActiveSheet.Columns[0].Width = 100;
                ssView.ActiveSheet.Columns[1].Width = 50;
                ssView.ActiveSheet.AddSpanCell(0, 0, 1, 2);
                ssView.ActiveSheet.AddSpanCell(1, 0, 1, 2);
                ssView.ActiveSheet.Cells[0, 0].Text = "시간";
                ssView.ActiveSheet.Cells[1, 0].Text = "분";

                ssView.ActiveSheet.Rows.Get(0).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);
                ssView.ActiveSheet.Rows.Get(1).Border = new ComplexBorder(new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.None), new ComplexBorderSide(ComplexBorderSideStyle.MediumLine), new ComplexBorderSide(ComplexBorderSideStyle.None), false, false);

                //  컬럼고정
                ssView.ActiveSheet.FrozenColumnCount = 2;

                List<MedcationInfo> list = SpdMedcations.ToList().FindAll(r => r.IsView);
                int viewItemCount = list.Count;


                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    ITEMCODE, ITEM, UNIT, ISWRITE, ISVIEW, ROWINDEX, SORT ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_EMR + "AEMRANCHARTITEM ";
                SQL = SQL + ComNum.VBLF + " WHERE EMRNO     = " + mstrEmrNo;
                SQL = SQL + ComNum.VBLF + "   AND EMRNOHIS  = " + mstrEmrHisNo;
                SQL = SQL + ComNum.VBLF + "ORDER BY SORT ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                Medcations = new List<MedcationInfo>();
                foreach (var row in dt.AsEnumerable())
                {
                    Medcations.Add(new MedcationInfo
                    {
                        ItemNo = row["ITEMCODE"].ToString(),
                        Name = row["ITEM"].ToString(),
                        Uint = row["UNIT"].ToString(),
                        IsWrite = (row["ISWRITE"].ToString() == "1" ? true : false),
                        IsView = (row["ISVIEW"].ToString() == "1" ? true : false),
                        Row = int.Parse(row["ROWINDEX"].ToString())
                    });
                }

                //  Write Spread 항목설정             
                List<MedcationInfo> wlist = Medcations.ToList().FindAll(r => r.IsWrite);
                ssWrite.ActiveSheet.Rows.Count = wlist.Count;
                for (int k = 0; k < wlist.Count; k++)
                {
                    ssWrite.ActiveSheet.Rows[k].Tag = wlist[k];
                    ssWrite.ActiveSheet.Cells[k, 0].Text = wlist[k].Name;
                    ssWrite.ActiveSheet.Cells[k, 2].Text = wlist[k].Uint;
                    ssWrite.ActiveSheet.Cells[k, 3].CellType = Type2;
                }

                #region 그래프 설정

                //  그래프 설정
                //XYPointSeries xyPointSeries = new XYPointSeries();
                //SpChart = new SpreadChart(xyPointSeries, "SpreadChart1");
                //ssView.ActiveSheet.DrawingContainer.ContainedObjects.Add(SpChart);

                //SpChart.IgnoreUpdateShapeLocation = false;
                //SpChart.IsGrayscale = false;

                //LegendArea legendArea1 = new LegendArea();
                //spreadChart.Model.LegendAreas.AddRange(new LegendArea[] { legendArea1 });

                //YPlotArea = new YPlotArea
                //{
                //    Location = new PointF(0.083F, 0.05F),
                //    Size = new SizeF(0.85F, 0.8F)
                //};

                //SpChart.Model.PlotAreas.AddRange(new PlotArea[] { YPlotArea });

                //string lastAlphabet = GetExcelColumnName(ssView.ActiveSheet.Columns.Count);

                //SBPSeries = new PointSeries
                //{
                //    SeriesName = "SBP",
                //    LabelVisible = true,
                //    PointBorder = new NoLine(),
                //    PointFill = Properties.Resources.Chart_SBP
                //};
                //SBPSeries.Values.DataSource = new SeriesDataField(
                //    ssView,
                //    "SBP",
                //    string.Concat("Sheet1!$C$101:$", lastAlphabet, "$101"),
                //    SegmentDataType.AutoIndex,
                //    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                //);

                //DBPSeries = new PointSeries
                //{
                //    SeriesName = "DBP",
                //    LabelVisible = true,
                //    PointBorder = new NoLine(),
                //    PointFill = Properties.Resources.Chart_DBP
                //};
                //DBPSeries.Values.DataSource = new SeriesDataField(
                //    ssView,
                //    "DBP",
                //    string.Concat("Sheet1!$C$102:$", lastAlphabet, "$102"),
                //    SegmentDataType.AutoIndex,
                //    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                //);

                //PulseSeries = new PointSeries
                //{
                //    SeriesName = "맥박",
                //    LabelVisible = true,
                //    PointBorder = new NoLine(),
                //    PointFill = Properties.Resources.Chart_Pulse
                //};
                //PulseSeries.Values.DataSource = new SeriesDataField(
                //    ssView,
                //    "맥박",
                //    string.Concat("Sheet1!$C$103:$", lastAlphabet, "$103"),
                //    SegmentDataType.AutoIndex,
                //    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                //);

                //BreathSeries = new PointSeries
                //{
                //    SeriesName = "호흡",
                //    LabelVisible = true,                    
                //    PointBorder = new NoLine(),                    
                //    PointFill = Properties.Resources.Chart_Breath
                //};
                //BreathSeries.Values.DataSource = new SeriesDataField(
                //    ssView,
                //    "호흡",
                //    string.Concat("Sheet1!$C$104:$", lastAlphabet, "$104"),
                //    SegmentDataType.AutoIndex,
                //    new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps }
                //);

                ////SBPSeries = new PointSeries
                ////{
                ////    SeriesName = "SBP",
                ////    LabelVisible = true,
                ////    PointBorder = new NoLine(),
                ////    PointFill = Properties.Resources.Chart_SBP
                ////};
                ////SBPSeries.Values.DataSource = new SeriesDataField(ssView, "SBP", "Sheet1!$C$101:$AL$101", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

                ////DBPSeries = new PointSeries
                ////{
                ////    SeriesName = "DBP",
                ////    LabelVisible = true,
                ////    PointBorder = new NoLine(),
                ////    PointFill = Properties.Resources.Chart_DBP
                ////};
                ////DBPSeries.Values.DataSource = new SeriesDataField(ssView, "DBP", "Sheet1!$C$102:$AL$102", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

                ////PulseSeries = new PointSeries
                ////{
                ////    SeriesName = "맥박",
                ////    LabelVisible = true,
                ////    PointBorder = new NoLine(),
                ////    PointFill = Properties.Resources.Chart_Pulse
                ////};
                ////PulseSeries.Values.DataSource = new SeriesDataField(ssView, "맥박", "Sheet1!$C$103:$AL$103", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

                ////BreathSeries = new PointSeries
                ////{
                ////    SeriesName = "호흡",
                ////    LabelVisible = true,
                ////    PointBorder = new NoLine(),
                ////    PointFill = Properties.Resources.Chart_Breath
                ////};
                ////BreathSeries.Values.DataSource = new SeriesDataField(ssView, "호흡", "Sheet1!$C$104:$AL$104", SegmentDataType.AutoIndex, new ChartDataSetting { EmptyValueStyle = EmptyValueStyle.Gaps });

                //YPlotArea.Series.AddRange(new Series[] {
                //    SBPSeries,
                //    DBPSeries,
                //    PulseSeries,
                //    BreathSeries,
                //});

                //IndexAxis indexAxis1 = new IndexAxis { LabelTextDirection = TextDirection.Rotate90Degree };                
                //ValueAxis valueAxis1 = new ValueAxis();
                //valueAxis1.Minimum = 0;
                //valueAxis1.Maximum = 200;

                //YPlotArea.XAxis = indexAxis1;
                //YPlotArea.YAxes.Clear();
                //YPlotArea.YAxes.AddRange(new ValueAxis[] { valueAxis1 });

                //int y = (viewItemCount * 18) + 50;

                //Rectangle rectangleOld = new Rectangle(50, y, 800, 600);
                //YPlotArea YPlotAreaOld = new YPlotArea
                //{
                //    Location = new PointF(0.083F, 0.05F),
                //    Size = new SizeF(0.85F, 0.8F)
                //};

                //int chartW = (((ssView.ActiveSheet.ColumnCount - 29) / 12) * 300) + 800;

                //Rectangle rectangleNew = new Rectangle(50, y, chartW, 600);
                //SpChart.Rectangle = rectangleNew;
                //SpChart.SheetName = "fpSpread1_Sheet1";

                ////  그래프 X축 기준점 정의
                ////  변경전 X축(변경전 넓이 * 변경전 비율) / 변경후 넓이
                //float x = (rectangleOld.Width * YPlotAreaOld.Location.X) / rectangleNew.Width;
                //YPlotArea.Location = new PointF(x, 0.05F);

                ////  ((변경전 넓이 * 변경전 비율) + (변경후 넓이 - 변경전 넓이)) / 변경후 넓이
                //float w = ((rectangleOld.Width * YPlotAreaOld.Size.Width) + (rectangleNew.Width - rectangleOld.Width)) / rectangleNew.Width;
                //YPlotArea.Size = new SizeF(w, 0.8F);


                //YPlotLocation = YPlotArea.Location;
                //YPlotSize = YPlotArea.Size;
                //ChartRect = SpChart.Rectangle;

                #endregion

                //// LabelArea
                //List<LabelArea> labelAreas1 = new List<LabelArea>();                
                //for(int col =2; col < ssView.ActiveSheet.ColumnCount; col++)
                //{
                //    if (ssView.ActiveSheet.Cells[103, col].Text == "") continue;

                //    float value = (float)VB.Val(ssView.ActiveSheet.Cells[103, col].Text);
                //    LabelArea la = new LabelArea();
                //    la.Text = VB.Left(ssView.ActiveSheet.Cells[104, col].Text, 1);
                //    la.TextFont = new Font("굴림", 8F);

                //    float width = ((float)col / (float)ssView.ActiveSheet.ColumnCount) * 0.94f;
                //    la.Location = new PointF(0.034f + width, 0.82f);
                //    labelAreas1.Add(la);
                //}
                //SpChart.Model.LabelAreas.Clear();
                //SpChart.Model.LabelAreas.AddRange(labelAreas1.ToArray());

                //SetChartPointMarkers(0);

                //SetGrapeShow();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            Cursor.Current = Cursors.Default;
        }





        /// <summary>
        /// 조회 팝업
        /// </summary>
        /// <param name="searchGbn"></param>
        private void FindUserSearch(string searchGbn, Panel panel)
        {
            frmAnUserSearch frmAnUserSearch = new frmAnUserSearch();
            frmAnUserSearch.SearchGbn = searchGbn;
            frmAnUserSearch.ShowDialog();

            //  저장
            if (!frmAnUserSearch.IsSave)
            {
                return;
            }

            for (int i = 0; i < frmAnUserSearch.ssMain.ActiveSheet.RowCount; i++)
            {
                if (!Convert.ToBoolean(frmAnUserSearch.ssMain.ActiveSheet.Cells[i, 0].Value))
                {
                    continue;
                }

                string userId = frmAnUserSearch.ssMain.ActiveSheet.Cells[i, 1].Text;
                string userName = frmAnUserSearch.ssMain.ActiveSheet.Cells[i, 2].Text;

                Label label = new Label();
                label.Text = userName;
                label.Tag = userId;
                label.Width = LabelWidth;
                label.TextAlign = ContentAlignment.MiddleLeft;
                panel.Controls.Add(label);
                label.Dock = DockStyle.Left;

                label.DoubleClick += (s, e) =>
                {
                    Label lbl = (s as Label);
                    lbl.Parent.Controls.Remove(lbl);
                };
            }
        }

        /// <summary>
        /// 컨트롤 값넣기
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private void SetControlValue(Control control, string value)
        {
            try
            {
                if (control is TextBox)
                {
                    (control as TextBox).Text = value;
                }
                else if (control is CheckBox)
                {
                    (control as CheckBox).Checked = Convert.ToBoolean(value);
                }
                else if (control is DateTimePicker)
                {
                    (control as DateTimePicker).Text = value;
                }
                else if (control is RadioButton)
                {
                    (control as RadioButton).Checked = Convert.ToBoolean(value);
                }
                else if (control is Panel)
                {
                    Panel panel = (control as Panel);
                    panel.Controls.Clear();
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        string[] split = value.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                        string idSplit = string.Empty;
                        string nameSplit = string.Empty;

                        if (split.Length > 0)
                        {
                            idSplit = split[0];
                            nameSplit = split[1];
                        }

                        string[] userIds = idSplit.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        string[] userNames = nameSplit.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < userIds.Length; i++)
                        {
                            Label label = new Label();
                            label.Text = userNames[i];
                            label.Tag = userIds[i];
                            label.Width = LabelWidth;
                            label.TextAlign = ContentAlignment.MiddleLeft;
                            panel.Controls.Add(label);
                            label.Dock = DockStyle.Left;

                            label.DoubleClick += (s, e) =>
                            {
                                Label lbl = (s as Label);
                                lbl.Parent.Controls.Remove(lbl);
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void SetSpreadValue(Control control, string value, int row, int col)
        {
            if (control is FpSpread)
            {
                if ((control as FpSpread).Name == "ssIn" || (control as FpSpread).Name == "ssOut")
                {
                    if ((control as FpSpread).ActiveSheet.RowCount <= row)
                    {
                        (control as FpSpread).ActiveSheet.RowCount = row + 1;
                    }
                }

                (control as FpSpread).ActiveSheet.Cells[row, col].Text = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable GetAutoData()
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                int rowIndex = CurrentFpSpread.ActiveSheet.ActiveRowIndex;
                int colIndex = CurrentFpSpread.ActiveSheet.ActiveColumnIndex;

                string viewData = CurrentFpSpread.ActiveSheet.Cells[rowIndex, colIndex].Text;

                SQL = "";
                if (CurrentFpSpread == ssJep)
                {

                    SQL = SQL + ComNum.VBLF + "SELECT JEPCODE AS CODE, JEPNAME AS NAME, BUSE_UNIT AS UNIT";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.ORD_JEP";
                    SQL = SQL + ComNum.VBLF + " WHERE JEPNAME LIKE '%" + viewData + "%'";
                    SQL = SQL + ComNum.VBLF + "ORDER BY JEPNAME";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "SELECT A.SUCODE AS CODE, B.SUNAMEK AS NAME, A.BUN";
                    SQL = SQL + ComNum.VBLF + "     , B.HCODE, B.UNIT AS UNIT";
                    SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_SUT A";
                    SQL = SQL + ComNum.VBLF + "  LEFT OUTER JOIN ADMIN.BAS_SUN B";
                    SQL = SQL + ComNum.VBLF + "               ON A.SUNEXT = B.SUNEXT";
                    SQL = SQL + ComNum.VBLF + " WHERE B.SUNAMEK LIKE '%" + viewData + "%'   ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY B.SUNAMEK, A.SUCODE ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    return null;
                }

                return dt;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "", clsDB.DbCon);
                return null;
            }
        }

        /// <summary>
        /// 컨트롤 값 가져오기
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        private object GetControlValue(Control control)
        {
            if (control is TextBox)
            {
                return (control as TextBox).Text.Replace("'", "`");
            }
            else if (control is CheckBox)
            {
                return (control as CheckBox).Checked;
            }
            else if (control is DateTimePicker)
            {
                return (control as DateTimePicker).Text;
            }
            else if (control is RadioButton)
            {
                return (control as RadioButton).Checked;
            }
            else if (control is Panel)
            {
                string userId = string.Empty;
                string userName = string.Empty;

                foreach (Control ctrl in control.Controls)
                {
                    if (ctrl is Label)
                    {
                        userId += (ctrl as Label).Tag.ToString() + ",";
                        userName += (ctrl as Label).Text + ",";
                    }
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    userId = userId.Substring(0, userId.Length - 1);
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    userName = userName.Substring(0, userName.Length - 1);
                }

                return string.Concat(userId, "||", userName).Replace("'", "`");
            }

            return null;
        }

        /// <summary>
        /// 숫자로 상단 컬럼 알파벳 가져오기
        /// 1 : A, 2 : B, 3 : C ...
        /// </summary>
        /// <param name="columnNumber"></param>
        /// <returns></returns>
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }



        private void SetEnterKey(Panel panel)
        {
            Control[] controls = null;
            controls = ComFunc.GetAllControls(panel);
            foreach (Control control in controls)
            {
                if (control is TextBox)
                {
                    if (!(control as TextBox).Multiline)
                    {
                        (control as TextBox).KeyDown -= PanelExt_KeyDown;
                        (control as TextBox).KeyDown += PanelExt_KeyDown;
                    }
                }
            }
        }

        private void PanelExt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }
    }

}
