using ComBase;
using ComBase.Controls;
using FarPoint.Win.Spread;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Dr Order, Dr(Er) Order
    /// </summary>
    public partial class frmTextEmrOrder : Form, EmrChartForm
    {
        #region // TopMenu관련 선언
        private usFormTopMenu usFormTopMenuEvent;
        private usTimeSet usTimeSetEvent;
        public ComboBox mMaskBox = null;
        #endregion

        #region //폼에서 사용하는 변수
        //const string mDirection = "V";   //기록지 작성방향(H: 옆으로, V:아래로)
        //const bool mHeadVisible = true;   //해드를 보이게 할지 여부
        //const int mintHeadCol = 4;  //해드 칼럼 수(작성, 조회 공통)
        //const int mintHeadRow = 2;  //해드 줄 수 (조회시에)

        int mCallFormGb = 0;  //차트작성 0, 기록지등록에서 호출 1,

        /// <summary>
        /// 정신과 여부
        /// </summary>
        bool mViewNpChart = false;

        /// <summary>
        /// 차트작성 일자.
        /// </summary>
        string mstrOrderDate = string.Empty;
        #endregion //폼에서 사용하는 변수

        #region // 기록지 관련 변수(기록지번호, 명칭 등 수정요함)
        //Form mCallForm = null;
        FormEmrMessage mEmrCallForm = null;
        EmrPatient pAcp = null;
        //EmrForm pForm = null;

        public string mstrFormNo = "1680";
        public string mstrUpdateNo = "0";
        public string mstrFormText = "";
        public string mstrEmrNo = "42694720";  //961 131641  //963 735603
        public string mstrMode = "W";

        List<string> lstCode = null;
        #endregion

        #region // TopMenu관련 이벤트 처리 함수

        private void SetTopMenuLoad()
        {
            //----TopMenu관련 이벤트 생성 및 선언
            usFormTopMenuEvent = new usFormTopMenu();
            //usBtnShow(usFormTopMenuEvent, "mbtnSave");
            usFormTopMenuEvent.rSetTimeCheckShow += new usFormTopMenu.SetTimeCheckShow(usFormTopMenuEvent_SetTimeCheckShow);
            usFormTopMenuEvent.rSetSave += new usFormTopMenu.SetSave(usFormTopMenuEvent_SetSave);
            usFormTopMenuEvent.rSetDel += new usFormTopMenu.SetDel(usFormTopMenuEvent_SetDel);
            usFormTopMenuEvent.rSetClear += new usFormTopMenu.SetClear(usFormTopMenuEvent_SetClear);
            usFormTopMenuEvent.rSetPrint += new usFormTopMenu.SetPrint(usFormTopMenuEvent_SetPrint);
            usFormTopMenuEvent.rEventClosed += new usFormTopMenu.EventClosed(usFormTopMenuEvent_EventClosed);

            usFormTopMenuEvent.dtMedFrDate.ValueChanged += new EventHandler(dtMedFrDate_ValueChanged);

            this.Controls.Add(usFormTopMenuEvent);
            usFormTopMenuEvent.Parent = this.panTopMenu;
            usFormTopMenuEvent.Dock = DockStyle.Fill;
            //--------------------------
        }

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

        private void dtMedFrDate_ValueChanged(object sender, EventArgs e)
        {
            //필요시만 코딩함
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
            //isReciveOrderSave = true;
            //dblEmrNo = pSaveData(strFlag);
            //isReciveOrderSave = false;
            return dblEmrNo;
        }

        /// <summary>
        /// 출력
        /// </summary>
        private void pPrintForm()
        {
            if (clsType.User.AuAPRINTIN.Equals("1") == false && 
                clsType.User.AuAPRINTOUT.Equals("1") == false && 
                clsType.User.AuAPRINTSIM.Equals("1") == false)
            {
                return;
            }

            if (chkDIAG.Checked)
            {
                SET_DIAG();
            }

            if (clsType.User.BuseCode.Equals("078201") || chkRemark.Checked)
            {
                ssOrder_Sheet1.Columns[(int)Order.Remarkis].Visible = false;
            }
            else
            {
                ssOrder_Sheet1.Columns[(int)Order.Remarkis, (int)Order.Remark].Visible = false;
            }

            if (clsType.User.BuseCode.Equals("044201"))
            {
                ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].Font = new Font("굴림체", 9, FontStyle.Bold);
            }

            string strMode = "A";
            btnPrint.Enabled = false;

            FpSpread prtSpd = ssOrder;
            bool FilterPrint = false;
            IRowFilter rf = ssOrder.ActiveSheet.RowFilter;
            #region 필터적용된 항목이 있으면 그 로우들만 복사 후 출력.
            if (rf != null && rf.ColumnDefinitions != null)
            {
                foreach (FilterColumnDefinition fcd in rf.ColumnDefinitions)
                {
                    FilterItemCollection filters = fcd.Filters;
                    IFilterItem iFilterItem = fcd.Filters.Cast<IFilterItem>().Where(d => d.DisplayName.Equals("Default Filter") == false).FirstOrDefault();

                    if (iFilterItem is MultiValuesFilterItem)
                    {
                        MultiValuesFilterItem mvFilterItem = iFilterItem as MultiValuesFilterItem;
                        if (mvFilterItem != null)
                        {
                            FilterPrint = true;
                            break;
                        }
                    }
                }

            }

            SheetView sheetView = null;
            if (FilterPrint)
            {
                for (int i = 0; i < ssOrder_Sheet1.RowCount; i++)
                {
                    if (ssOrder_Sheet1.RowFilter.IsRowFilteredOut(i) == false)
                    {
                        if (prtSpd.Equals(ssOrder))
                        {
                            sheetView = CopySheet(ssOrder_Sheet1);
                            prtSpd = new FpSpread();
                            prtSpd.Sheets.Clear();
                            prtSpd.Sheets.Add(sheetView);
                            sheetView.RowCount = 0;
                        }

                        sheetView.RowCount += 1;
                        for (int j = 0; j < sheetView.ColumnCount; j++)
                        {
                            sheetView.Cells[sheetView.RowCount - 1, j].Text = ssOrder_Sheet1.Cells[i, j].Text.Trim();
                        }
                    }
                }
            }
            #endregion


            ssOrder_Sheet1.PrintInfo.ShowRowHeader = PrintHeader.Hide;
            //clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
            //                             ssOrder, "V", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "COL", -1, -1, 0, strMode);
            clsSpreadPrint.PrintEmrSpread(clsDB.DbCon, mstrFormNo, mstrUpdateNo, pAcp, true, dtpOptSDate.Value.ToString("yyyyMMdd"), dtpOptEDate.Value.ToString("yyyyMMdd"),
                                         prtSpd, clsType.User.BuseCode.Equals("078201") ? "P" : "V", 30, 20, 30, 20, false, FarPoint.Win.Spread.PrintOrientation.Landscape, "COL", -1, -1, 0, strMode);


            btnPrint.Enabled = true;
            if (clsType.User.BuseCode.Equals("044201"))
            {
                ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].Font = new Font("굴림체", 9, FontStyle.Regular);
            }
            ssOrder_Sheet1.Columns[(int)Order.Remarkis, (int)Order.Remark].Visible = true;

            if (prtSpd.Equals(ssOrder) == false)
            {
                sheetView.Dispose();
                //prtSpd.Dispose();
            }
        }


        private SheetView CopySheet(SheetView sheet)
        {
            SheetView newSheet = null;

            if (sheet != null)
            {
                newSheet = (SheetView) FarPoint.Win.Serializer.LoadObjectXml(ssOrder_Sheet1.GetType(), FarPoint.Win.Serializer.GetObjectXml(sheet, "CopySheet"), "CopySheet");
            }

            return newSheet;
        }

        /// <summary>
        /// 상병 같이 출력시 설정
        /// </summary>
        private void SET_DIAG()
        {
            if(ssOrder_Sheet1.Cells[0, (int)Order.Bdate].Text == "진 단 일")
            {
                return;
            }

            ssOrder_Sheet1.ColumnHeader.Visible = false;

            ssOrder_Sheet1.Rows.Add(0, 1);
            ssOrder_Sheet1.Rows[0].Font = new Font("굴림체", 9, FontStyle.Bold);
            ssOrder_Sheet1.Cells[0, 0].Text = " ";
            ssOrder_Sheet1.Cells[0, (int)Order.Bdate].Text = "진 단 일";
            ssOrder_Sheet1.Cells[0, (int)Order.OrderCode].Text = "코   드";
            ssOrder_Sheet1.Cells[0, (int)Order.OrderName].Text = "Diagnosis";
            ssOrder_Sheet1.AddSpanCell(0, 2, 1, 11);

            for (int i = 0; i < ssDise_Sheet1.RowCount; i++)
            {
                ssOrder_Sheet1.Rows.Add(i + 1, 1);
                ssOrder_Sheet1.AddSpanCell(i + 1, 2, 1, 11);
                ssOrder_Sheet1.Cells[i + 1, (int)Order.Bdate].Text     = ssDise_Sheet1.Cells[i, 0].Text.Trim();
                ssOrder_Sheet1.Cells[i + 1, (int)Order.OrderCode].Text = ssDise_Sheet1.Cells[i, 2].Text.Trim();
                ssOrder_Sheet1.Cells[i + 1, (int)Order.OrderName].Text = ssDise_Sheet1.Cells[i, 3].Text.Trim();
            }

            ssOrder_Sheet1.Rows.Add(ssDise_Sheet1.RowCount + 1, 1);
            ssOrder_Sheet1.AddSpanCell(ssDise_Sheet1.RowCount + 1, 0, 1, 12);

            ssOrder_Sheet1.Rows.Add(ssDise_Sheet1.RowCount + 2, 1);
            ssOrder_Sheet1.Rows[ssDise_Sheet1.RowCount + 2].Font = new Font("굴림체", 9, FontStyle.Bold);
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, 0].Text = " ";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.Bdate].Text = "처방일자";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.OrderCode].Text = "처방코드";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.OrderName].Text = "처방명";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.Content].Text = "일용량";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.RealQty].Text = "일투량";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.DosName].Text = "용법/검체";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.GbGroup].Text = "MiX";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.GbDiv].Text = "횟수";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.Nal].Text = "일수";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.DrName].Text = "Sign";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.Remarkis].Text = "";
            ssOrder_Sheet1.Cells[ssDise_Sheet1.RowCount + 2, (int)Order.Remark].Text = "Remark";

            ssOrder_Sheet1.Columns.Add(0, 1);
            ssOrder_Sheet1.Columns[0].Width = 25;

            int Cnt = 1;
            for(int i = ssDise_Sheet1.RowCount + 3; i < ssOrder_Sheet1.RowCount; i++)
            {
                ssOrder_Sheet1.Cells[i, 0].Text = Cnt.ToString();
                Cnt++;
            }

            
            ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            FarPoint.Win.ComplexBorder AcomplexBorder = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            FarPoint.Win.ComplexBorder LcomplexBorder = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, Color.Silver), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);
            
            ssOrder_Sheet1.Cells[0, 0, ssOrder_Sheet1.RowCount - 1, 0].Border = LcomplexBorder;
            ssOrder_Sheet1.Cells[0, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.Columns.Count - 1].Border = AcomplexBorder;
        }

        /// <summary>
        /// 스프래드 클리어
        /// </summary>
        private void pClearForm()
        {
            //ssOrder_Sheet1.RowCount = 0;
            //mRow = -2;
            //mCol = -2;
            GetChartHistoryCopy();
        }

        /// <summary>
        /// 저장
        /// </summary>
        /// <returns></returns>
        public double pSaveData()
        {
            double dblEmrNo = 0;

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

        public bool DelDataMsg()
        {
            bool rtnVal = false;
            //rtnVal = pDelData();
            return rtnVal;
        }

        public void ClearFormMsg()
        {
            mstrEmrNo = "0";
            //pClearForm();
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

            //if (clsEmrQuery.SaveEmrXmlPrnYnForm(mstrEmrNo, "0") == false)
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

        /// <summary>
        /// 권한별 환경을 설정을 한다(작성화면 안보이게 등)
        /// </summary>
        private void SetUserAut()
        {
            if (clsType.User.AuAWRITE == "1")
            {
                panTopMenu.Visible = true;
                //panWrite.Visible = true;
            }
            else
            {
                panTopMenu.Visible = false;
                //panWrite.Visible = false;
            }

            if (clsType.User.AuAPRINTIN == "1")
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }

        /// <summary>
        /// 사용자 템플릿 뷰어용
        /// </summary>
        public void pInitFormTemplet()
        {
            SetTopMenuLoad();
        }


        #endregion

        #region //생성자
        public frmTextEmrOrder()
        {
            InitializeComponent();
        }

        public frmTextEmrOrder(string strFormNo, string strUpdateNo, string strEmrNo, int pCallFormGb)
        {
            InitializeComponent();

            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            mstrEmrNo = strEmrNo;
            mCallFormGb = pCallFormGb;

            pInitFormTemplet();
        }


        /// <summary>
        /// 19-07-05 신규 생성자 
        /// </summary>
        /// <param name="strFormNo"></param>
        /// <param name="strUpdateNo"></param>
        /// <param name="po"></param>
        /// <param name="strEmrNo"></param>
        /// <param name="strMode"></param>
        /// <param name="strOrderDate"></param>
        /// <param name="pEmrCallForm"></param>
        public frmTextEmrOrder(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, string strOrderDate, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            mstrOrderDate = strOrderDate;
            InitializeComponent();

            pInitFormTemplet();
        }

        public frmTextEmrOrder(string strFormNo, string strUpdateNo, EmrPatient po, string strEmrNo, string strMode, FormEmrMessage pEmrCallForm) //절대 수정하시 마시요(서식생성기 에러남)
        {
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
            pAcp = po;
            mstrEmrNo = strEmrNo;
            mstrMode = strMode;
            mEmrCallForm = pEmrCallForm;
            InitializeComponent();

            pInitFormTemplet();
        }

       #endregion //생성자

        #region 스프레드 enum
        public enum Order
        {
            /// <summary>
            /// 처방일
            /// </summary>
            Bdate,
            /// <summary>
            /// 처방코드
            /// </summary>
            OrderCode,
            /// <summary>
            /// 처방명
            /// </summary>
            OrderName,
            /// <summary>
            /// 일용량
            /// </summary>
            Content,
            /// <summary>
            /// 일투량
            /// </summary>
            RealQty,
            /// <summary>
            /// 용법/검체
            /// </summary>
            DosName,
            /// <summary>
            /// Mix
            /// </summary>
            GbGroup,
            /// <summary>
            /// 횟수
            /// </summary>
            GbDiv,
            /// <summary>
            /// 일수
            /// </summary>
            Nal,
            /// <summary>
            /// 급여
            /// </summary>
            SALARY,
            /// <summary>
            /// M(방사선 Portable 촬영)
            /// </summary>
            M,
            /// <summary>
            /// DrName
            /// </summary>
            DrName,
            /// <summary>
            /// REMARK Is
            /// </summary>
            Remarkis,
            /// <summary>
            /// REMARK
            /// </summary>
            Remark,
            /// <summary>
            /// 실제 처방 시간
            /// </summary>
            ENTDATE
        }
        #endregion

        
        private void frmTextEmrOrder_Load(object sender, EventArgs e)
        {
            ssOrder_Sheet1.Columns[(int)Order.SALARY].Visible = true;
            ssOrder_Sheet1.Columns[(int)Order.M].Visible = true;

            if (mCallFormGb != 1)
            {
                SetUserAut();
            }

            //usFormTopMenuEvent.mbtnClear.Visible = true;
            //usFormTopMenuEvent.mbtnPrint.Visible = true;
            //usFormTopMenuEvent.mbtnSave.Visible = true;
            //usFormTopMenuEvent.mbtnSaveTemp.Visible = true;

            panTopMenu.Visible = false;

            //if (pAcp == null)
            //{
            //    ComFunc.MsgBoxEx(this, "환자 정보가 올바르지 않습니다.");
            //    return;
            //}

            lstCode = new List<string>();
            mViewNpChart = clsEmrQueryOld.ViewNPChart(clsType.User.Sabun);

            if (pAcp.medEndDate.Length == 0)
            {
                pAcp.medEndDate = ComQuery.CurrentDateTime(clsDB.DbCon, "D");
            }

            dtpOptSDate.Value = Convert.ToDateTime(
                ReadOptionDate(clsEmrPublic.gUserGrade, 
                pAcp.inOutCls,
                pAcp.medDeptCd,
                clsType.User.IdNumber,
                Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).AddDays(-3).ToShortDateString())
                );
           dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

           btnPrint.Visible = false;

            if(clsEmrPublic.gUserGrade == "SIMSA" || clsEmrPublic.gUserGrade == "WRITE" || clsType.User.AuAPRINTIN.Equals("1"))
            {
                btnPrint.Visible = true;
                ssOrder_Sheet1.Columns[(int) Order.Remarkis, (int)Order.Remark].Visible = true;
                ssOrder_Sheet1.Columns[(int)Order.SALARY].Width = 15;
                ssOrder_Sheet1.Columns[(int)Order.M].Visible = false;
                ssOrder_Sheet1.Columns[(int)Order.OrderName].Width = 150;
                ssOrder_Sheet1.Columns[(int)Order.Content].Width = 20;
                ssOrder_Sheet1.Columns[(int)Order.RealQty].Width = 20;
                ssOrder_Sheet1.Columns[(int)Order.DosName].Width = 120;
             
                ssOrder_Sheet1.Columns[(int)Order.GbGroup].Width = 25;
                ssOrder_Sheet1.Columns[(int)Order.GbDiv].Width = 25;
                ssOrder_Sheet1.Columns[(int)Order.Nal].Width = 25;
            }

            if (VB.Val(mstrEmrNo) > 0)
            {
                mstrOrderDate = mstrEmrNo;
            }

            if (clsType.User.BuseCode.Equals("076001"))
            {
                ssOrder_Sheet1.Columns[(int) Order.ENTDATE].Visible = true;
            }

            if (mstrOrderDate.Length > 0)
            {
                GetChartHistoryCopy();
            } 
            else
            {

                ssDise_Sheet1.RowCount = 0;
                ssOrder_Sheet1.RowCount = 0;

                dtpOptSDate.Value = Convert.ToDateTime(
                clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade,
                pAcp.inOutCls,
                pAcp.medDeptCd,
                clsType.User.IdNumber,
                ComFunc.FormatStrToDate(pAcp.medFrDate, "D"))
                );

                if(pAcp.inOutCls == "O")
                {
                    dtpOptSDate.Value = Convert.ToDateTime(
                      clsEmrQueryPohangS.ReadOptionDate(clsEmrPublic.gUserGrade,
                      pAcp.inOutCls,
                      pAcp.medDeptCd,
                      clsType.User.IdNumber,
                      ComFunc.FormatStrToDate(pAcp.medFrDate, "D"))
                      );
                    dtpOptEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(pAcp.medEndDate, "D"));
                }
                else
                {
                    if(pAcp.medEndDate == "")
                    {
                        dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));

                    }
                    else
                    {
                        dtpOptEDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(pAcp.medEndDate, "D"));

                    }
                }

                btnSearch.PerformClick();
            }

            #region //19-12-02 심사팀 필터기능 추가
            if (clsType.User.BuseCode.Equals("078201"))
            {
                ssOrder_Sheet1.RowHeader.Visible = true;
                ssOrder_Sheet1.Columns[(int)Order.OrderName].Width = 200;
                ssOrder_Sheet1.Columns[(int)Order.Content].Width = 30;
                ssOrder_Sheet1.Columns[(int)Order.GbDiv].Width = 30;
                ssOrder_Sheet1.Columns[(int)Order.GbGroup].Width = 30;
                ssOrder_Sheet1.Columns[(int)Order.Nal].Width = 30;
                ssOrder_Sheet1.Columns[(int)Order.RealQty].Width = 30;
                ssOrder_Sheet1.Columns[(int)Order.Remark].Width += 120;

                for (int i = 0; i < ssOrder.ActiveSheet.Columns.Count; i++)
                {
                    ssOrder.ActiveSheet.Columns.Get(i).AllowAutoFilter = true;
                }
                ssOrder.ActiveSheet.AutoFilterMode = AutoFilterMode.EnhancedContextMenu;
            }
            #endregion
        }

        string SET_GRADE()
        {
            string rtnVal = string.Empty;
            switch (clsType.User.BuseCode)
            {
                case "078200":
                case "078201":
                case "077502":
                case "077405":
                    rtnVal = "SIMSA";
                    break;
                case "044201":
                case "044200":
                    rtnVal = "WRITE";
                    break;

            }

            if (clsType.User.Sabun == "14472")
            {
                rtnVal= "SIMSA";
            }

            return rtnVal;

        }

        void GetChartHistoryCopy()
        {
            ssOrder_Sheet1.RowCount = 0;

            dtpOptSDate.Value = Convert.ToDateTime(
             ReadOptionDate(clsEmrPublic.gUserGrade,
             pAcp.inOutCls,
             pAcp.medDeptCd,
             clsType.User.IdNumber,
             VB.Val(pAcp.medFrDate).ToString("0000-00-00"))
             );

            if(pAcp.inOutCls == "O")
            {
                dtpOptSDate.Value = Convert.ToDateTime(
                 ReadOptionDate(clsEmrPublic.gUserGrade,
                 pAcp.inOutCls,
                 pAcp.medDeptCd,
                 clsType.User.IdNumber,
                 VB.Val(pAcp.medFrDate).ToString("0000-00-00"))
                 );
            }
            else
            {
                if(pAcp.medEndDate == "")
                {
                    dtpOptEDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
                }
                else
                {
                    dtpOptEDate.Value = Convert.ToDateTime(VB.Val(pAcp.medEndDate).ToString("0000-00-00"));
                }

                if(mstrOrderDate != "")
                {
                    dtpOptSDate.Value = Convert.ToDateTime(
                     ReadOptionDate(clsEmrPublic.gUserGrade,
                     pAcp.inOutCls,
                     pAcp.medDeptCd,
                     clsType.User.IdNumber,
                     VB.Val(mstrOrderDate).ToString("0000-00-00"))
                     );

                    dtpOptEDate.Value = Convert.ToDateTime(VB.Val(mstrOrderDate).ToString("0000-00-00"));
                }
            }

            GetCopyOrder();
        }

        void GetCopyOrder()
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;

            try
            {
                #region 쿼리
                if (pAcp.inOutCls == "I")
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup, O.POWDER,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, '' RES, O.GBPRN, O.GBACT ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_IORDER O,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ORDERCODE C,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ODOSAGE D,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_DOCTOR N,");
                    SQL.AppendLine("          KOSMOS_PMPA.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    if (mstrOrderDate != "")
                    {
                        SQL.AppendLine("          AND O.BDATE = TO_DATE('" + VB.Val(mstrOrderDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    }
                    else
                    {
                        SQL.AppendLine("          AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                        if (pAcp.medEndDate != "")
                        {
                            SQL.AppendLine("          AND O.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                        }
                    }


                    SQL.AppendLine("          AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("          AND    O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("          AND    O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("          AND    O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("          AND    O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("          AND    (O.GBIOE NOT IN ('E','EI') OR O.GBIOE IS NULL OR GBIOE = '')");
                    SQL.AppendLine("          AND    O.SUCODE = S.SUNEXT(+) ");

                    if (pAcp.medDeptCd == "RA" ||
                        (pAcp.medDeptCd == "MD" &&
                        (pAcp.medDrCd == "1107" ||
                        pAcp.medDrCd == "1125")))
                    {
                        SQL.AppendLine("                AND O.DEPTCODE = 'MD'");
                        SQL.AppendLine("                AND O.DRCODE IN ('1107','1125')");
                    }
                    else
                    {
                        SQL.AppendLine("                AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                        SQL.AppendLine("                AND O.DRCODE NOT IN ('1107','1125')");
                    }

                    SQL.AppendLine("          ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");

                }
                else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER" && pAcp.formNo != 2090)
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames,  O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, '' AS GbGroup, O.POWDER,");
                    SQL.AppendLine("     O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE, O.RES, '' GBPRN, '' GBACT  ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_OORDER O,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ORDERCODE C,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ODOSAGE D,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_DOCTOR N,");
                    SQL.AppendLine("          KOSMOS_PMPA.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("        AND O.BDATE = TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                            pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("        AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("        AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }

                    SQL.AppendLine("        AND    O.GBSUNAP ='1' AND O.Seqno    > 0   AND O.NAL      > 0");
                    SQL.AppendLine("        AND    O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND    O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND    O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND    O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("        AND    O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno");
                }
                else if ((pAcp.inOutCls == "O" && pAcp.medDeptCd == "ER"))
                {
                    SQL.AppendLine("  SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDate, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup, O.POWDER,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode,    D.DosName,   D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.DRCODE , O.GBACT ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_IORDER O,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ORDERCODE C,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_ODOSAGE D,");
                    SQL.AppendLine("          KOSMOS_OCS.OCS_DOCTOR N,");
                    SQL.AppendLine("          KOSMOS_PMPA.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("        AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("        AND O.BDATE <= TO_DATE('" + dtpOptSDate.Value.AddDays(1).ToShortDateString() + "','YYYY-MM-DD') ");
                    SQL.AppendLine("        AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE      =  N.SABUN(+)      ");
                    SQL.AppendLine("        AND O.GBIOE IN ('E','EI') ");
                    SQL.AppendLine("        AND O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("      ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");
                }
                #endregion

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOrder_Sheet1.Cells[i, (int)Order.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if (dt.Rows[i]["BUN"].ToString().Trim().Length == 0 ||
                        dt.Rows[i]["BUN"].ToString().Trim() == "10")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim().Length > 0)
                        {
                            string strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DispHeader"].ToString().Trim().Length > 0)
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text =
                                dt.Rows[i]["DispHeader"].ToString().Trim() +
                                dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                    }


                    if (dt.Rows[i]["POWDER"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text + "(가루약)";
                    }

                    if (pAcp.inOutCls != "O" && dt.Rows[i]["RES"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = "(예약)" + ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text;
                    }


                    ssOrder_Sheet1.Cells[i, (int)Order.Content].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ?
                        "" : dt.Rows[i]["CONTENTS"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.RealQty].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text =
                        dt.Rows[i]["REMARK"].ToString().Trim() == "PRN" && dt.Rows[i]["GBACT"].ToString().Trim() == "*" ?
                    "(간호사수행)" + ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text : ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text;

                    ssOrder_Sheet1.Cells[i, (int)Order.GbGroup].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbDiv].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Nal].Text = dt.Rows[i]["NAL"].ToString().Trim();


                    if (pAcp.inOutCls == "I")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd != "ER")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.DrName].Text =
                            GetDrNm(dt.Rows[i]["DRCODE"].ToString().Trim(),
                                    dt.Rows[i]["BDATE"].ToString().Trim());
                    }
                    else if (pAcp.inOutCls == "O" && pAcp.medDeptCd == "ER")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.DrName].Text = "";
                    }

                    string strRgb = dt.Rows[i]["cDispRGB"].ToString().Trim();
                    ssOrder_Sheet1.Rows[i].ForeColor = strRgb.Length == 0 || strRgb == "00000000" ? Color.Black :
                        ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString(string.Format("0x{0}", strRgb)));
                }

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        string ReadOptionDate(string argGrade, string ArgIO, string ArgDeptCode, string argUSEID, string argDATE)
        {
            string rtnVal = argDATE;

            switch (argUSEID)
            {

                case "45924":
                case "22536":
                case "36522":
                case "44892":
                    rtnVal = VB.Left(pAcp.medFrDate, 4) + "-" +
                        VB.Mid(pAcp.medFrDate, 5, 2) + "-" +
                        VB.Right(pAcp.medFrDate, 2);
                return rtnVal;
            }

            if (argGrade != "SIMSA")
            {
                return rtnVal;
            }

            OracleDataReader reader = null;

            try
            {
                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine(" SELECT NAL ");
                SQL.AppendLine(" FROM KOSMOS_EMR.EMR_OPTION_SETDATE ");
                SQL.AppendLine(" WHERE IO = '" + ArgIO + "' ");
                SQL.AppendLine("   AND USEID = " + argUSEID);
                if (ArgIO != "I")
                {
                    SQL.AppendLine("   AND DEPTCODE = '" + ArgDeptCode + "' ");
                }
                SQL.AppendLine("   AND USED = '1' ");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal; 
                }

                if(reader.HasRows && reader.Read())
                {
                    if(reader.GetValue(0).ToString().Trim().Length == 0 ||
                       VB.IsNumeric(reader.GetValue(0).ToString().Trim()) == false)
                    {
                        rtnVal = argDATE;
                        reader.Dispose();
                        return rtnVal;
                    }

                    rtnVal = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S")).
                        AddDays(VB.Val(reader.GetValue(0).ToString().Trim()) * -1).ToShortDateString();
                }

                reader.Dispose();
            }
            catch(Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (mstrFormNo == "2090")
            {
                GetDise("ER");
                GetOrderER();
            }
            else
            {
                GetDise();
                GetOrder();
            }
        }

        /// <summary>
        /// 버튼 조회 함수
        /// </summary>
        void GetDise(string DEPT = "")
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            string sqlErr = string.Empty;
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;

            lstCode.Clear();

            try
            {
                ssDise_Sheet1.RowCount = 0;

                if (DEPT == "ER")
                {
                    ssDise_Sheet1.Columns[0].Visible = false;
                    ssDise_Sheet1.Columns[1].Visible = false;
                    ssDise_Sheet1.Columns[4, 6].Visible = false;
                    ssDise_Sheet1.Columns[11].Visible = false;

                    SQL.AppendLine( "  SELECT A.IllCode, A.Boowi1, A.Boowi2, A.Boowi3, A.Boowi4, B.IllNameE, TO_CHAR(A.BDATE, 'YYYY-MM-DD') BDATE, A.RO");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_EILLS A, KOSMOS_PMPA.BAS_ILLS B ");
                    SQL.AppendLine("    WHERE A.Ptno     = '" + pAcp.ptNo + "'       ");
                    SQL.AppendLine("      AND BDate    = TO_DATE('" + VB.Val(pAcp.medFrDate).ToString("0000-00-00") + "' ,'YYYY-MM-DD') ");
                    SQL.AppendLine("      AND A.IllCode  = B.IllCode      ");
                    SQL.AppendLine("      AND B.ILLCLASS = '1'");

                    sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lstCode.IndexOf(dt.Rows[i]["IllCode"].ToString().Trim()) != -1 )
                        {                            
                            continue;
                        }

                        lstCode.Add(dt.Rows[i]["IllCode"].ToString().Trim());

                        ssDise_Sheet1.RowCount += 1;

                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["IllNameE"].ToString().Trim();

                        if (dt.Rows[i]["RO"].ToString().Trim() == "*")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 3].Text = "R/O)" + ssDise_Sheet1.Cells[i, 3].Text;
                        }

                        if (pAcp.medDeptCd == "DN" || pAcp.medDeptCd == "DT")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["Boowi1"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["Boowi2"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Boowi3"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["Boowi4"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    ssDise_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    return;
                }


                if (pAcp.inOutCls == "I")
                {
                    ssDise_Sheet1.Columns[0].Visible = true;
                    ssDise_Sheet1.Columns[1].Visible = true;
                    ssDise_Sheet1.Columns[4, 6].Visible = true;
                    ssDise_Sheet1.Columns[11].Visible = true;

                    SQL.AppendLine(" SELECT A.*, TO_CHAR(A.BDate,'YYYY-MM-DD') BDate1, IllNameE, A.RO,  ");
                    SQL.AppendLine("            TO_CHAR(A.RemoveDate,'YYYY-MM-DD') RemoveDate1, ");
                    SQL.AppendLine("            TO_CHAR(A.CfDate,'YYYY-MM-DD') CfDate1, ");
                    SQL.AppendLine("            TO_CHAR(A.EDate,'YYYY-MM-DD') EDate1, A.ROWID   ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_IILLS A, KOSMOS_PMPA.BAS_ILLS B ");
                    SQL.AppendLine("    WHERE A.Ptno     = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("      AND A.EntDate  >= TO_DATE('" + VB.Val(pAcp.medFrDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");

                    if (pAcp.medEndDate != "")
                    {
                        SQL.AppendLine("      AND A.EntDate  <= TO_DATE('" + VB.Val(pAcp.medEndDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    }

                    SQL.AppendLine("      AND A.IllCode  = B.IllCode");
                    SQL.AppendLine("ORDER BY BDate");

                    sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    ssDise_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lstCode.IndexOf(dt.Rows[i]["IllCode"].ToString().Trim()) != -1)
                        {
                            continue;
                        }

                        lstCode.Add(dt.Rows[i]["IllCode"].ToString().Trim());
                        ssDise_Sheet1.RowCount += 1;

                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDate1"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["IllNameE"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["RemoveDate1"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["EDate1"].ToString().Trim();

                        if (dt.Rows[i]["RO"].ToString().Trim() == "*")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 3].Text = "R/O)" + ssDise_Sheet1.Cells[i, 3].Text;
                        }

                        if (dt.Rows[i]["DeptCode"].ToString().Trim() == "DN" || dt.Rows[i]["DEPTCODE"].ToString().Trim() == "DT")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["Boowi1"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["Boowi2"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Boowi3"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["Boowi4"].ToString().Trim();
                        }
                        else
                        {
                            if (dt.Rows[i]["Main"].ToString().Trim() == "*")
                            {
                                ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 4].BackColor = Color.FromArgb(0, 0, 255);
                                ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 4].Text = "*";
                            }

                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["RO"].ToString().Trim();

                            if (ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 1].Text.Trim() != "")
                            {
                                ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 0, ssDise_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(255, 234, 234);
                            }
                        }


                        if (dt.Rows[i]["EDate1"].ToString().Trim() != "")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 0, ssDise_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(234, 255, 255);
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["EDate1"].ToString().Trim();

                            if (dt.Rows[i]["Main"].ToString().Trim() == "*")
                            {
                                ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 4].BackColor = Color.FromArgb(0, 0, 255);
                            }
                        }
                    }
                }
                else
                {
                    ssDise_Sheet1.Columns[0].Visible = false;
                    ssDise_Sheet1.Columns[1].Visible = false;
                    ssDise_Sheet1.Columns[4, 6].Visible = false;
                    ssDise_Sheet1.Columns[11].Visible = false;

                    
                    SQL.AppendLine(" SELECT A.IllCode, A.Boowi1, A.Boowi2, A.Boowi3, A.Boowi4, B.IllNameE, A.BDATE, A.RO ");
                    SQL.AppendLine(" FROM KOSMOS_OCS.OCS_OILLS A, KOSMOS_PMPA.BAS_ILLS B  ");
                    SQL.AppendLine("WHERE A.Ptno     = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("  AND BDate  = TO_DATE('" + VB.Val(pAcp.medFrDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    SQL.AppendLine("  AND A.DeptCode = '" + pAcp.medDeptCd + "' ");
                    SQL.AppendLine("  AND A.IllCode  = B.IllCode");
                    SQL.AppendLine("  AND B.ILLCLASS = '1'");
                    SQL.AppendLine("ORDER BY A.SEQNO");

                    sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }

                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    ssDise_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lstCode.IndexOf(dt.Rows[i]["IllCode"].ToString().Trim()) != -1)
                        {
                            continue;
                        }

                        lstCode.Add(dt.Rows[i]["IllCode"].ToString().Trim());
                        ssDise_Sheet1.RowCount += 1;

                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                        ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["IllNameE"].ToString().Trim();

                        if (dt.Rows[i]["RO"].ToString().Trim() == "*")
                        {
                            ssDise_Sheet1.Cells[i, 3].Text = "R/O)" + ssDise_Sheet1.Cells[i, 3].Text;
                        }

                        if (pAcp.medDeptCd == "DN" || pAcp.medDeptCd == "DT")
                        {
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["Boowi1"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["Boowi2"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Boowi3"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["Boowi4"].ToString().Trim();
                            ssDise_Sheet1.Cells[ssDise_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                ssDise_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }



        /// <summary>
        /// 버튼 조회 함수
        /// </summary>
        void GetOrder()
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;

            DateTime dtpSyDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
            try
            {
                #region 쿼리
                if (pAcp.inOutCls == "I")
                {
                    SQL.AppendLine("   SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDATE, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK, O.DRCODE,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup, O.POWDER,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode");
                    SQL.AppendLine("    , CASE WHEN D.DOSNAME IS NULL THEN O.GBINFO ELSE D.DOSNAME END DOSNAME ");
                    SQL.AppendLine("    , D.GBDIV,  N.DrName, C.DispRGB cDispRGB, '' RES, O.GBPRN, O.GBACT,");
                    SQL.AppendLine("    TO_CHAR(O.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1, ");
                    SQL.AppendLine("    TO_CHAR(O.PICKUPDate,'YYYY-MM-DD HH24:Mi') PICKUPDate1, ");
                    SQL.AppendLine("    TO_CHAR(O.DrOrderView,'YYYY-MM-DD HH24:Mi') DrOrderView1, O.GbVerb, o.VERBC, O.POWDER  ");
                    SQL.AppendLine("    , '' AS SABUN  ");
                    SQL.AppendLine("    , O.GBSELF ");
                    SQL.AppendLine("    , O.GBPORT ");
                    SQL.AppendLine("    , O.VER ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_IORDER O,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_ORDERCODE C,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_ODOSAGE D,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_DOCTOR N,");
                    SQL.AppendLine("         KOSMOS_PMPA.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("        AND O.BDATE >=  TO_DATE('2009-06-01','YYYY-MM-DD')  ");

                    if (mstrOrderDate != "")
                    {
                        SQL.AppendLine("        AND O.BDATE = TO_DATE('" + VB.Val(mstrOrderDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                    }
                    else
                    {
                        SQL.AppendLine("        AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                        if (pAcp.medEndDate != "")
                        {
                            SQL.AppendLine("        AND O.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                        }
                    }


                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                             pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("        AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("        AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }

                    if (!mViewNpChart)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                    }

                    if (dtpSyDate >= "2021-08-01 06:00:00".To<DateTime>())
                    {
                        SQL.AppendLine("        AND CASE WHEN O.GBSTATUS IN ('D', 'D-') AND O.BUN IN ('11', '12', '20') AND O.ACTDIV > 0 THEN 1    ");
                        SQL.AppendLine("        	     WHEN O.GBSTATUS NOT IN ('D', 'D-', 'D+') THEN 1                                           ");
                        SQL.AppendLine("        	END = 1                                                                                        ");
                    }
                    else
                    {
                        SQL.AppendLine("        AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                    }
                    SQL.AppendLine("        AND (OrderSite IS NULL OR ORDERSITE <> 'ERO')");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE     =  N.SABUN(+)      ");
                    SQL.AppendLine("        AND (O.ORDERSITE <> 'CAN' OR O.ORDERSITE IS NULL)");
                    SQL.AppendLine("        AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)");
                    SQL.AppendLine("        AND O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");

                }
                else 
                {
                    SQL.AppendLine("   SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDATE, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK, O.DRCODE,");
                    SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, '' GbGroup, O.POWDER,");
                    SQL.AppendLine("    O.RealQty,    O.DosCode");
                    SQL.AppendLine("    , CASE WHEN D.DOSNAME IS NULL THEN O.GBINFO ELSE D.DOSNAME END DOSNAME ");
                    SQL.AppendLine("    , D.GBDIV,  N.DrName, C.DispRGB cDispRGB, O.RES, '' GBPRN, '' GBACT,");
                    SQL.AppendLine("    TO_CHAR(O.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1, ");
                    SQL.AppendLine("    TO_CHAR(O.EntDate,'YYYY-MM-DD HH24:Mi') PICKUPDate1, ");
                    SQL.AppendLine("    TO_CHAR(O.EntDate,'YYYY-MM-DD HH24:Mi') DrOrderView1, '' GbVerb, '' VERBC, O.POWDER  ");
                    SQL.AppendLine("    , O.SABUN  ");
                    SQL.AppendLine("    , O.GBSELF ");
                    SQL.AppendLine("    , '' AS GBPORT ");
                    SQL.AppendLine("    , '' AS VER ");
                    SQL.AppendLine("    FROM KOSMOS_OCS.OCS_OORDER O,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_ORDERCODE C,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_ODOSAGE D,");
                    SQL.AppendLine("         KOSMOS_OCS.OCS_DOCTOR N,");
                    SQL.AppendLine("         KOSMOS_PMPA.BAS_SUN     S");
                    SQL.AppendLine("      WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                    SQL.AppendLine("        AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD')");
                    SQL.AppendLine("        AND O.BDATE = TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                    if (pAcp.inOutCls == "O")
                    {
                        if (pAcp.medDeptCd == "RA" ||
                            (pAcp.medDeptCd == "MD" &&
                            (pAcp.medDrCd == "1107" ||
                            pAcp.medDrCd == "1125")))
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = 'MD'");
                            SQL.AppendLine("        AND O.DRCODE IN ('1107','1125')");
                        }
                        else
                        {
                            SQL.AppendLine("        AND O.DEPTCODE = '" + pAcp.medDeptCd + "'");
                            SQL.AppendLine("        AND O.DRCODE NOT IN ('1107','1125')");
                        }
                    }


                    if (!mViewNpChart)
                    {
                        SQL.AppendLine("        AND O.DEPTCODE <> 'NP'");
                    }

                    if (clsEmrPublic.gUserGrade == "SIMSA")
                    {
                        SQL.AppendLine("        AND (O.GBSUNAP ='1' OR O.SUCODE IN ('$$11','$$21')) ");

                    }
                    else
                    {
                        SQL.AppendLine("        AND O.GBSUNAP ='1' ");

                    }

                    SQL.AppendLine("        AND O.Seqno    > '0'   AND O.NAL      > '0'");
                    SQL.AppendLine("        AND O.SlipNo     =  C.SlipNo(+)      ");
                    SQL.AppendLine("        AND O.OrderCode  =  C.OrderCode(+)   ");
                    SQL.AppendLine("        AND O.DosCode    =  D.DosCode(+)     ");
                    SQL.AppendLine("        AND O.DRCODE      =  N.SABUN(+)      ");
                    //SQL.AppendLine("        AND O.SABUN      =  N.DOCCODE(+)      ");
                    SQL.AppendLine("        AND O.SUCODE = S.SUNEXT(+) ");
                    SQL.AppendLine("ORDER BY O.BDATE DESC, O.GbSunap DESC, O.Slipno, O.Seqno");
                }
                #endregion

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOrder_Sheet1.Cells[i, (int)Order.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if (dt.Rows[i]["BUN"].ToString().Trim().Length == 0 ||
                        dt.Rows[i]["BUN"].ToString().Trim() == "10")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim().Length > 0)
                        {
                            string strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DispHeader"].ToString().Trim().Length > 0)
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text =
                                dt.Rows[i]["DispHeader"].ToString().Trim() + 
                                dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                    }

                    if (dt.Rows[i]["POWDER"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text + "(가루약)";
                    }

                    if (dt.Rows[i]["RES"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = "(예약)" + ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text;
                    }


                    ssOrder_Sheet1.Cells[i, (int)Order.Content].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ?
                        "" : dt.Rows[i]["CONTENTS"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.RealQty].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = dt.Rows[i]["DosName"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.SALARY].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.M].Text = dt.Rows[i]["GBPORT"].ToString().Trim();

                    if (dt.Rows[i]["REMARK"].ToString().Trim() == "PRN" && dt.Rows[i]["GBACT"].ToString().Trim() == "*")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = "간호사수행/" + dt.Rows[i]["DosName"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[i, (int)Order.GbGroup].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbDiv].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Nal].Text = dt.Rows[i]["NAL"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.Remarkis].Text = dt.Rows[i]["REMARK"].ToString().Trim() != "" ? "#" : "";
                    ssOrder_Sheet1.Cells[i, (int)Order.Remark].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DrName].Text = pAcp.inOutCls == "I" ?  dt.Rows[i]["DrName"].ToString().Trim() :
                        GetDrNm(dt.Rows[i]["DRCODE"].ToString().Trim(),
                                dt.Rows[i]["BDATE"].ToString().Trim());

                    if(dt.Rows[i]["GbVerb"].ToString().Trim() == "Y")
                    {
                        if (dt.Rows[i]["VERBC"].ToString().Trim() == "C")
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.ENTDATE].Text = dt.Rows[i]["DrOrderView1"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.ENTDATE].Text = dt.Rows[i]["EntDate1"].ToString().Trim();
                    }

                    string strRgb = dt.Rows[i]["cDispRGB"].ToString().Trim();
                    ssOrder_Sheet1.Rows[i].ForeColor = strRgb.Length == 0 || strRgb == "00000000" ? Color.Black :
                        ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString(string.Format("0x{0}", strRgb)));


                    if (dt.Rows[i]["VER"].ToString().Trim() == "CPORDER" && ssOrder.ActiveSheet.Cells[i, (int)Order.OrderName].Text.IndexOf("[CP]") == -1)
                    {
                        ssOrder.ActiveSheet.Cells[i, (int)Order.OrderName].Text = "[CP]" + ssOrder.ActiveSheet.Cells[i, (int)Order.OrderName].Text;
                    }

                    if (ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text.Length >= 70)
                    {
                        ssOrder_Sheet1.Rows[i].Height = ssOrder_Sheet1.Rows[i].GetPreferredHeight() + 15;
                    }

                }

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        /// <summary>
        /// 버튼 조회 함수
        /// </summary>
        void GetOrderER()
        {
            ssOrder_Sheet1.RowCount = 0;
            StringBuilder SQL = new StringBuilder();
            Cursor.Current = Cursors.WaitCursor;
            DataTable dt = null;

            DateTime dtpSyDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            try
            {

                SQL.AppendLine("SELECT TO_CHAR(O.BDate, 'YYYY-MM-DD') BDATE, O.BUN, O.OrderCode, O.NAL, C.OrderName,  C.OrderNames, O.REMARK, O.DRCODE,");
                SQL.AppendLine("    C.DispHeader, O.Qty, O.CONTENTS, O.GbGroup,");
                SQL.AppendLine("    O.RealQty,    O.DosCode   ");
                SQL.AppendLine("    , CASE WHEN D.DOSNAME IS NULL THEN O.GBINFO ELSE D.DOSNAME END DOSNAME ");
                SQL.AppendLine("    , D.GBDIV,  N.DrName, C.DispRGB cDispRGB, ");
                SQL.AppendLine("    TO_CHAR(O.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1,");
                SQL.AppendLine("    TO_CHAR(O.PICKUPDate,'YYYY-MM-DD HH24:Mi') PICKUPDate1, ");
                SQL.AppendLine("    TO_CHAR(O.DrOrderView,'YYYY-MM-DD HH24:Mi') DrOrderView1, O.GbVerb, O.VERBC, O.POWDER");
                SQL.AppendLine("    , O.GBSELF ");
                SQL.AppendLine("    , O.GBPORT ");
                SQL.AppendLine("  FROM KOSMOS_OCS.OCS_IORDER O,");
                SQL.AppendLine("       KOSMOS_OCS.OCS_ORDERCODE C,");
                SQL.AppendLine("       KOSMOS_OCS.OCS_ODOSAGE D,");
                SQL.AppendLine("       KOSMOS_OCS.OCS_DOCTOR N,");
                SQL.AppendLine("       KOSMOS_PMPA.BAS_SUN S");
                SQL.AppendLine(" WHERE O.PTNO = '" + pAcp.ptNo + "' ");
                SQL.AppendLine("   AND O.BDATE >= TO_DATE('2009-06-01','YYYY-MM-DD') ");
                SQL.AppendLine("   AND O.BDATE >= TO_DATE('" + dtpOptSDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");

                if (mstrOrderDate != "")
                {
                    SQL.AppendLine("   AND O.BDATE = TO_DATE('" + VB.Val(mstrOrderDate).ToString("0000-00-00") + "','YYYY-MM-DD') ");
                }
                else
                {
                    if (pAcp.medEndDate != "")
                    {
                        SQL.AppendLine("   AND O.BDATE <= TO_DATE('" + dtpOptEDate.Value.ToShortDateString() + "','YYYY-MM-DD') ");
                    }
                }

                if (!mViewNpChart)
                {
                    SQL.AppendLine("   AND O.DEPTCODE <> 'NP'");
                }

                if (dtpSyDate >= "2021-08-01 06:00:00".To<DateTime>())
                {
                    SQL.AppendLine("        AND CASE WHEN O.GBSTATUS IN ('D', 'D-') AND O.BUN IN ('11', '12', '20') AND O.ACTDIV > 0 THEN 1    ");
                    SQL.AppendLine("        	     WHEN O.GBSTATUS NOT IN ('D', 'D-', 'D+') THEN 1                                           ");
                    SQL.AppendLine("        	END = 1                                                                                        ");
                }
                else
                {
                    SQL.AppendLine("        AND O.GBSTATUS NOT IN ('D-','D','D+' )  ");
                }

                SQL.AppendLine("   AND (  O.OrderSite  In ('ERO')   OR  O.GBIOE IN ('E','EI') )  ");
                SQL.AppendLine("   AND    O.SlipNo     =  C.SlipNo(+)      ");
                SQL.AppendLine("   AND    O.OrderCode  =  C.OrderCode(+)   ");
                SQL.AppendLine("   AND    O.DosCode    =  D.DosCode(+)     ");
                SQL.AppendLine("   AND    O.DRCODE      =  N.SABUN(+)      ");
                SQL.AppendLine("   AND (NOT (O.GBVERB = 'Y' AND O.VERBC IS NULL)  OR O.GBVERB IS NULL)");
                SQL.AppendLine("   AND    O.SUCODE = S.SUNEXT(+) ");
                SQL.AppendLine("ORDER BY O.BDATE DESC, O.Slipno, O.Seqno");

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ssOrder_Sheet1.RowCount = dt.Rows.Count;
                ssOrder_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssOrder_Sheet1.Cells[i, (int)Order.Bdate].Text = dt.Rows[i]["Bdate"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.OrderCode].Text = dt.Rows[i]["OrderCode"].ToString().Trim();

                    if (dt.Rows[i]["BUN"].ToString().Trim().Length == 0 ||
                        dt.Rows[i]["BUN"].ToString().Trim() == "10")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }
                    else
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim().Length > 0)
                        {
                            string strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DispHeader"].ToString().Trim().Length > 0)
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text =
                                dt.Rows[i]["DispHeader"].ToString().Trim() +
                                dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = dt.Rows[i]["OrderName"].ToString().Trim();
                        }
                    }

                    if (dt.Rows[i]["POWDER"].ToString().Trim() == "1")
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text = ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text + "(가루약)";
                    }


                    ssOrder_Sheet1.Cells[i, (int)Order.Content].Text = VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0 ?
                        "" : dt.Rows[i]["CONTENTS"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.RealQty].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.DosName].Text = dt.Rows[i]["DosName"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbGroup].Text = dt.Rows[i]["GbGroup"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.GbDiv].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Nal].Text = dt.Rows[i]["NAL"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.SALARY].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.M].Text = dt.Rows[i]["GBPORT"].ToString().Trim();

                    ssOrder_Sheet1.Cells[i, (int)Order.DrName].Text = dt.Rows[i]["DrName"].ToString().Trim();
                    ssOrder_Sheet1.Cells[i, (int)Order.Remarkis].Text = dt.Rows[i]["REMARK"].ToString().Trim() != "" ? "#" : "";
                    ssOrder_Sheet1.Cells[i, (int)Order.Remark].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                    if (dt.Rows[i]["GbVerb"].ToString().Trim() == "Y")
                    {
                        if (dt.Rows[i]["VERBC"].ToString().Trim() == "C")
                        {
                            ssOrder_Sheet1.Cells[i, (int)Order.ENTDATE].Text = dt.Rows[i]["DrOrderView1"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[i, (int)Order.ENTDATE].Text = dt.Rows[i]["EntDate1"].ToString().Trim();
                    }

                    string strRgb = dt.Rows[i]["cDispRGB"].ToString().Trim();
                    ssOrder_Sheet1.Rows[i].ForeColor = strRgb.Length == 0 || strRgb == "00000000" ? Color.Black :
                        ColorTranslator.FromOle((int)new Int32Converter().ConvertFromString(string.Format("0x{0}", strRgb)));

                    if (ssOrder_Sheet1.Cells[i, (int)Order.OrderName].Text.Length >= 70)
                    {
                        ssOrder_Sheet1.Rows[i].Height = ssOrder_Sheet1.Rows[i].GetPreferredHeight() + 15;
                    }
                }

                dt.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
                return;
            }
        }

        string GetDrNm(string strDrCode, string strBdate, string strSabun = "")
        {
            string rtnVal = string.Empty;
            OracleDataReader reader = null;

            try
            {
                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine("SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR ");
                if (string.IsNullOrWhiteSpace(strSabun))
                {
                    SQL.AppendLine("          WHERE DRCODE = '" + strDrCode + "'");
                }
                else
                {
                    SQL.AppendLine("          WHERE TRIM(DOCCODE) = '" + strSabun + "'");
                }
                SQL.AppendLine("          AND ((GBOUT <> 'Y')");
                SQL.AppendLine("              OR (GBOUT = 'Y' AND ");
                SQL.AppendLine("                      (FDATE >= TO_DATE('" + strBdate + "','YYYY-MM-DD'))");
                SQL.AppendLine("                      AND (TDATE <= TO_DATE('" + strBdate + "','YYYY-MM-DD'))))");

                string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (sqlErr.Length > 0)
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return rtnVal;
                }

                if(reader.HasRows &&
                   reader.Read())
                {
                    rtnVal = reader.GetValue(0).ToString().Trim();
                }

                reader.Dispose();
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }

            return rtnVal;
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            pPrintForm();
        }

        void PrintCopy(string PrintType)
        {
            DataTable dt = null;

            string strPatName = string.Empty;
            string strSex = string.Empty;
            string strAge = string.Empty;
            string strJumin = string.Empty;

            try
            {
                StringBuilder SQL = new StringBuilder();
                SQL.AppendLine(" SELECT SNAME, SEX, JUMIN1, JUMIN2 ");
                SQL.AppendLine(" FROM KOSMOS_PMPA.BAS_PATIENT ");
                SQL.AppendLine(" WHERE PANO = '" + pAcp.ptNo + "' ");

                string sqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString().Trim(), clsDB.DbCon);
                if(sqlErr.Length > 0)
                {
                    ComFunc.MsgBoxEx(this, sqlErr);
                    return;
                }

                if(dt.Rows.Count > 0)
                {
                    strPatName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex     = dt.Rows[0]["SEX"].ToString().Trim();
                    strAge     = ComFunc.AgeCalc(clsDB.DbCon,
                        dt.Rows[0]["JUMIN1"].ToString().Trim() +
                        dt.Rows[0]["JUMIN2"].ToString().Trim()).ToString();
                     strJumin = string.Format("{0}-{1}******",
                        dt.Rows[0]["JUMIN1"].ToString().Trim(),
                        VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1));

                }

                dt.Dispose();



                string strFormName = "Dr Order";

                //'Print Head 지정
                string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
                string strFont2 = @"/fn""바탕체"" /fz""10"" /fb0 /fi0 /fu0 /fk0 /fs2";
                string strHead1 = "/c/f1" + strFormName + "/f1/n/n";
                string strHead2 = "/n/l/f2" + "환자번호 : " + pAcp.ptNo +
                                  VB.Space(3) + "환자성명 : " + strPatName + VB.Space(3) + "성별/나이 : " + strSex + "/" + strAge + "세" + "  주민번호 : " + strJumin + "/n/n";
                string strFooter = "/n/l/f2" + "포항성모병원" + VB.Space(40) + "출력일자 : " +  VB.Format(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "@@@@-@@-@@") + VB.Space(40) + "출력자 : " + clsType.User.UserName;
                strFooter = strFooter + "/n/l/f2" + "* 본 의무기록지는 기록작성자의 전자서명을 공인인증 받은 기록지 입니다.";
                strFooter = strFooter + "/n/l/f2" + "This is an electronically authorized offical medical record";

                ssOrder_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
                ssOrder_Sheet1.PrintInfo.Footer = strFooter;
                ssOrder_Sheet1.PrintInfo.Margin.Left = 20;
                ssOrder_Sheet1.PrintInfo.Margin.Right = 20;
                ssOrder_Sheet1.PrintInfo.Margin.Top = 20;
                ssOrder_Sheet1.PrintInfo.Margin.Bottom = 20;

                ssOrder_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssOrder_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
                ssOrder_Sheet1.PrintInfo.ShowBorder = true;
                ssOrder_Sheet1.PrintInfo.ShowColor = false;
                ssOrder_Sheet1.PrintInfo.ShowGrid = true;
                ssOrder_Sheet1.PrintInfo.ShowShadows = false;
                ssOrder_Sheet1.PrintInfo.UseMax = false;
                ssOrder_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;

                if(PrintType == "V")
                {
                    ssOrder_Sheet1.PrintInfo.Preview = true;
                }

                ssOrder.PrintSheet(0);
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBoxEx(this, ex.Message);
            }

        }

        private void SsOrder_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssOrder_Sheet1.RowCount == 0)
                return;

            if(e.Column == (int)Order.OrderCode && e.Row >= 0)
            {
                string strCode = ssOrder_Sheet1.Cells[e.Row, e.Column].Text.Trim();
                if(strCode.Length > 0)
                {
                    StringBuilder SQL = new StringBuilder();
                    SQL.AppendLine( " SELECT BUN ");
                    SQL.AppendLine(" FROM KOSMOS_PMPA.BAS_SUT ");
                    SQL.AppendLine(" WHERE SUNEXT = '" + strCode + "' ");
                    SQL.AppendLine("   AND BUN IN ('11','12','20','23')");

                    OracleDataReader reader = null;
                    string sqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                    if (sqlErr.Length > 0)
                    {
                        ComFunc.MsgBoxEx(this, sqlErr);
                        return;
                    }

                    if(reader.HasRows)
                    {
                        using(frmInfoEntrySimple frm = new frmInfoEntrySimple(strCode))
                        {
                            frm.StartPosition = FormStartPosition.CenterParent;
                            frm.ShowDialog(this);
                        }
                    }

                    reader.Dispose();
                }
            }

            if (e.Column == (int)Order.Remarkis && ssOrder_Sheet1.Cells[e.Row, e.Column].Text.Trim().Equals("#"))
            {
                ComFunc.MsgBoxEx(this, ssOrder_Sheet1.Cells[e.Row, (int)Order.Remark].Text.Trim());
            }
        }

        private void SsOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                ssOrder_Sheet1.ClipboardCopy(FarPoint.Win.Spread.ClipboardCopyOptions.AsStringSkipHidden);
            }

        }

        private void chkDIAG_CheckedChanged(object sender, EventArgs e)
        {
            pPrintForm();
        }
    }
}
