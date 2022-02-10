using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;

namespace ComEmrBase
{
    /// <summary>
    /// 동의서 웹브라우저 관리
    /// </summary>
    public class EasManager
    {
        private DevComponents.DotNetBar.ExpandableSplitter splitter = null;
        public  bool isLeft { get; set; }
        private readonly static object Lock_Object = new object();
        private static EasManager instance;
        /// <summary>
        /// 프린터명 비워둘경우 기본 프린터로 인쇄
        /// </summary>
        //private readonly string PrinterName = "Microsoft Print to PDF";
        public string PrinterName  { get; set; }

        public string HOST_URL { get; set; }
        public string LOGIN_URL { get; set; }  
        public string FORM_MANAGER_URL { get; set; } 
        public string PRINT_URL { get; set; }
        public string PREVIEW_URL { get; set; }
        public string VIEW_URL { get; set; }
        public string HISTORY_URL { get; set; }
        public string HISTORY2_URL { get; set; }
        public string UPDATE_URL { get; set; }
        public string WRITE_URL { get; set; }
        public string PdfFolderPath { get; set; }
        
        public string JavascriptBoundName { get; set; } = "cefsharpBoundAsync";

        private frmEasDesigner easDesigner = null;
        private frmEasViewer easViewer = null;
        private frmEasTabletViewer tabletViewer = null;  

        public static EasManager Instance
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        instance = new EasManager();
                    }
                    return instance;
                }

            }
        }
        public EasManager()
        {
            //this.HOST_URL = "http://localhost:8080/eas";
            this.HOST_URL = "http://192.168.100.50:8080/eas";
            //this.LOGIN_URL = HOST_URL;// + "/winform?userId=" + clsType.User.IdNumber;
            this.LOGIN_URL = HOST_URL + "/winform?userId=" + clsType.User.IdNumber;

            this.FORM_MANAGER_URL = HOST_URL + "/form/main?isWinform=1";
            this.PRINT_URL = HOST_URL + "/write/$formNo?formDataId={}&isPrint=yes&isWinform=1&updateNo="; //빈서식지
            this.PREVIEW_URL = HOST_URL + "/write/$formNo?formDataId={}&isPrint=yes&isWinform=0&updateNo=";
            this.VIEW_URL = HOST_URL + "/write/$formNo?formDataId={}&isPrint=view&isWinform=1&updateNo=";
            this.HISTORY_URL = HOST_URL + "/write/$formNo?easFormDataHistoryId={}&isPrint=view&isWinform=1&updateNo=";
            this.HISTORY2_URL = HOST_URL + "/write/$formNo?formDataId={0}&easFormDataHistoryId={1}&isPrint=view&isWinform=1&updateNo=";
            this.UPDATE_URL = HOST_URL + "/write/$formNo?formDataId=$formDataId&isWinform=1&updateNo=$updateNo"; // 수정
            this.WRITE_URL = HOST_URL + "/write/$formNo?formDataId=0&ptNo=$ptNo&medDrCd=$medDrCd&medDeptCd=$medDeptCd&medFrDate=$medFrDate&medFrTime=$medFrTime&inOutCls=$inOutCls&isWinform=1&updateNo=$updateNo";

            this.PdfFolderPath =  "C:\\HealthSoft\\pdfprint";
            PrinterName = string.Empty;

          //  easDesigner = new frmEasDesigner(this);
            easViewer = new frmEasViewer(this);
        //    Show();

        }

        public frmEasDesigner GetEasFormDesigner()
        {
            if (this.easDesigner.IsDisposed)
            {
         
                easDesigner = new frmEasDesigner(this);
            }
            return this.easDesigner;
        }
        public frmEasTabletViewer GetEasTablerViewer()
        {
            if (this.easViewer != null)
            {
                if (this.tabletViewer == null || this.tabletViewer.IsDisposed)
                {
                    tabletViewer = new frmEasTabletViewer(this, easViewer.GetBound());
                }

            }
            else
            {
                throw new Exception("전자동의서 뷰어가 없습니다");
            }
        
            return this.tabletViewer;
        }
        public frmEasViewer GetEasFormViewer(DevComponents.DotNetBar.ExpandableSplitter splitter = null)
        {
            if (this.easViewer.IsDisposed)
            {
                if (tabletViewer != null)
                {
                    //tabletViewer.Dispose();
                    //tabletViewer = null;
                }
           
                easViewer = new frmEasViewer(this);
                
             
            }
            this.splitter = splitter;
            return this.easViewer;
        }
        public void LeftMoveSplitter()
        {
            if (splitter != null)
            {
                isLeft = true;
                splitter.SplitPosition = 50;
            }
            
        }
        public void RightMoveSplitter()
        {
            if (splitter != null )
            {
                isLeft = false;
                splitter.SplitPosition = 361;
            }

        }
        public void ShowTabletMoniror()
        {
       
            easViewer.ShowTabletViewer(null);
        }
        public void ShowTabletMoniror(EasParam easParam, long formDataId = 0)
        {

            easViewer.ShowTabletViewer(easParam, formDataId);
        }
        private void ShowDesigner()
        {
            if (easDesigner == null || easDesigner.IsDisposed)
            {
                easDesigner = new frmEasDesigner(this);

            }
            easDesigner.Show();
            easDesigner.BringToFront();
        }
        private void ShowViewer()
        {
            if (easViewer == null || easViewer.IsDisposed)
            {
                easViewer = new frmEasViewer(this);

            }
            easViewer.Show();
            easViewer.BringToFront();

        }
        /// <summary>
        /// 동의서 pc에서 작성하기
        /// </summary>
        public void Write(EmrForm emrForm, EmrPatient acpEmr, EasParam easParam = null)
        {
            ShowViewer();
            easViewer.Write(emrForm, acpEmr, easParam);

        }
        public void Print(EmrForm emrForm, EmrPatient acpEmr, string formDataId)
        {
            easViewer.Print(emrForm, acpEmr, formDataId);
        }
        //http://localhost:8080/eas/write/3509?formDataId=0&ptNo=09116446&updateNo=1&easFormDataHistoryId=79
        public void HistoryView(EmrForm emrForm, EmrPatient acpEmr, long formNo, string easFormDataHistoryId)
        {
            easViewer.HistoryView(emrForm, acpEmr, formNo, easFormDataHistoryId);
        }

        public void HistoryView(EmrForm emrForm, EmrPatient acpEmr, long formNo, string easFormDataId, string easFormDataHistoryId)
        {
            easViewer.HistoryView(emrForm, acpEmr, formNo, easFormDataId, easFormDataHistoryId);
        }


        public void Update(EmrForm emrForm, EmrPatient acpEmr, long formDataId, EasParam easParam = null)
        {
            easViewer.Update(emrForm, acpEmr, formDataId, easParam);
        }
        /// <summary>
        /// 작성된 동의서 보기 (로그인 없이 작동)
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="formDataId"></param>
        public void View(EmrForm emrForm, EmrPatient acpEmr, long formNo, string formDataId, bool onlyView = false)
        {
            if(onlyView == false)
            {
                ShowViewer();
            }
            
            EasFormData easFormData = GetEasFormData(formDataId);
            if (onlyView)
            {
                easViewer.View(emrForm, acpEmr, formNo, long.Parse(formDataId));
            }
            else
            {
                if (easFormData.UserId == clsType.User.IdNumber || (clsType.User.DrCode.NotEmpty() && (acpEmr.medDeptCd.Equals("TO") || acpEmr.medDeptCd.Equals("HR"))))
                {
                    //수정가능
                    easViewer.Update(emrForm, acpEmr, long.Parse(formDataId), null);
                    easViewer.ShowTabletViewer(null, long.Parse(formDataId));
                }
                else
                {
                    //보기만
                    easViewer.View(emrForm, acpEmr, formNo, long.Parse(formDataId));
                }
            }
          
            
        }
        public string GetPdfFileName(long formNo, int updateNo)
        {
            Log.Debug("동의서 인쇄 시작 formNo:{}, updateNo:{}", formNo, updateNo);
            DirectoryInfo di = new DirectoryInfo(PdfFolderPath);
            if (!di.Exists)
            {
                di.Create();
            }
            string pdfFileName = @"C:\HealthSoft\pdfprint\" + formNo + "_" + "version_" + "_" + VB.Format(DateTime.Now, "yyyyMMHHhhmmss.fff") + ".pdf";
            return pdfFileName;
        }

        public void ClearPdfFoler()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(PdfFolderPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }
        /// <summary>
        /// 서식 미리보기
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        public void Preview(int formNo, int updateNo)
        {
            ShowDesigner();
            easDesigner.Preview(formNo, updateNo);
        }
        /// <summary>
        /// 서식 편집하기
        /// </summary>
        /// <param name="formNo"></param>
        /// <param name="updateNo"></param>
        public void Edit(int formNo, int updateNo)
        {
            ShowDesigner();
            easDesigner.EditUrl(formNo, updateNo);
        }
       
        /// <summary>
        /// 차트복사, 복사신청 방법에 따라 updateNo를 결정해야함 20190723
        /// 차트복사 신청시 작성된 데이타  AEASFORMDATA 키를 가지고 있으면 키를 가지고 내원내역 그리고 서식정보를 가지고 올수 있음
        /// AEASFORMDATA 키가 없을경우 내원내역 정보에 해당되는 AEASFORMDATA의 서식정보를 가져와야함(AEASFORMCONTENT 조인)
        /// </summary>
        /// <returns></returns>
        //public int Print(long formNo, int updateNo, long formDataId, EmrForm emrForm = null, EmrPatient emrPatient = null)
        //{
        //    this.printedPageCount = 0;

        //    //ShowDesigner();
        //    easDesigner.SavedChartPrint(SetPrintedPageCount, formNo, updateNo, formDataId);
        //    return this.printedPageCount;
        //}

        //public int GetPrintedPageCount()
        //{
        //    return this.printedPageCount;
        //}

        //public void SetPrintedPageCount(int printedPageCount)
        //{
        //     this.printedPageCount += printedPageCount;
        //}

        public long GetFormDataId(EmrForm emrForm , EmrPatient AcpEmr, long argEmrNo)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            try
            {
                SQL = "SELECT C.ID FROM ADMIN.AEASFORMCONTENT A ";
                SQL += ComNum.VBLF + "  INNER JOIN ADMIN.AEMRFORM B ";
                SQL += ComNum.VBLF + "  ON A.FORMNO = b.FORMNO ";
                SQL += ComNum.VBLF + "  AND A.UPDATENO = B.UPDATENO ";
                SQL += ComNum.VBLF + "  INNER JOIN ADMIN.AEASFORMDATA C ";
                SQL += ComNum.VBLF + "  ON C.EASFORMCONTENT = A.ID ";
                SQL += ComNum.VBLF + "  WHERE C.PTNO =   '" + AcpEmr.ptNo + "'";
                SQL += ComNum.VBLF + "  AND C.MEDDEPTCD = '" + AcpEmr.medDeptCd + "'";
                SQL += ComNum.VBLF + "  AND C.INOUTCLS  = '" + AcpEmr.inOutCls + "'";
                SQL += ComNum.VBLF + "  AND B.FORMNO  = " + emrForm.FmFORMNO;
                if (argEmrNo > 0) { SQL += ComNum.VBLF + "  AND C.ID  = " + argEmrNo; }
                SQL += ComNum.VBLF + "  AND C.ISDELETED ='N' ";
                //  SQL += ComNum.VBLF + "  AND B.UPDATENO  = " + fView.FmUPDATENO;

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return 0;
                }

                return dt.Rows[0]["ID"].To<long>(0);
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                return 0;
            }
        }

        public EasFormData GetEasFormData(string formDataId)
        {

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            EasFormData easFormData = new EasFormData();

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL = SQL + " SELECT ID, MODIFIEDUSER, CREATED, MODIFIED FROM " + ComNum.DB_EMR + "AEASFORMDATA";
                SQL = SQL + " WHERE ID = " + formDataId;

                
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                }

                easFormData.Id = formDataId;
                easFormData.UserId = dt.Rows[0]["MODIFIEDUSER"].ToString().Trim();
                easFormData.Created = dt.Rows[0]["CREATED"].ToString();
                easFormData.Modified = dt.Rows[0]["MODIFIED"].ToString();

                dt.Dispose();
                dt = null;

                return easFormData;

            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return null;
            }
        }

        public string GetOcrNo(EmrForm emrForm, EmrPatient emrPatient, int pageCount)
        {

            long ocrNo = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(clsDB.DbCon);

                SQL = "";
                SQL = SQL + "SELECT " + ComNum.DB_EMR + "SEQ_AEMROCRPRTHIS_OCRNO.nextVal as ocrNo FROM Dual";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                }

                ocrNo = long.Parse(dt.Rows[0]["ocrNo"].ToString());
                dt.Dispose();
                dt = null;

                StringBuilder strSql = new StringBuilder();

                strSql.AppendLine("INSERT INTO ADMIN.AEMROCRPRTHIS ");
                strSql.AppendLine(" (OCRNO, FORMCODE, ACPNO, OCRDATE, OCRTIME, PTNO, PTNAME, INOUTCLS, MEDFRDATE, MEDDEPTCD, WARDCODE, FORMNO, UPDATENO, USEID, PAGECOUNT ) ");
                strSql.AppendLine("VALUES (             ");
                strSql.AppendLine("      " + ocrNo + ",");
                strSql.AppendLine("      '113',");
                strSql.AppendLine("      " + emrPatient.acpNo + ",");
                strSql.AppendLine("      TO_CHAR(SYSDATE, 'YYYYMMDD'),");
                strSql.AppendLine("      TO_CHAR(SYSDATE, 'hh24miss'),");
                strSql.AppendLine("      '" + emrPatient.ptNo + "',");
                strSql.AppendLine("      '" + emrPatient.ptName + "',");
                strSql.AppendLine("      '" + emrPatient.inOutCls + "',");
                strSql.AppendLine("      '" + emrPatient.medFrDate + "',");
                strSql.AppendLine("      '" + emrPatient.medDeptCd + "',");
                strSql.AppendLine("      '" + emrPatient.ward + "',");
                strSql.AppendLine("      '" + emrForm.FmFORMNO + "',");
                strSql.AppendLine("      '" + emrForm.FmUPDATENO + "',");
                strSql.AppendLine("      '" + clsType.User.Sabun + "',");
                strSql.AppendLine("      " + pageCount );
                    
                strSql.AppendLine(")");

                SqlErr = clsDB.ExecuteNonQueryEx(strSql.ToString().Trim(), ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    Cursor.Current = Cursors.Default;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                return ocrNo.ToString().PadLeft(13, '0');

            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;

                return string.Empty;
            }
        }
    }

    public class EasFormData
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Created { get; set; }
        public string Modified { get; set; }
    }
}
