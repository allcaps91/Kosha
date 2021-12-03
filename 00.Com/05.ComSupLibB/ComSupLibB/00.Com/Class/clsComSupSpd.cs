using ComBase; //기본 클래스
using System;
using System.Drawing;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : clsComSupSpd.cs
    /// Description     : 진료지원 공통 스프레드 관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-01
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>    
    public class clsComSupSpd
    {

        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();

        public enum enmPrtType { NVC,ENDO_BAR,ENDO_PATHOL,XRAY_PRT };

        public class cComSupPRT_ENDOBar
        {
            public string strPano = "";
            public string strSName = "";
            public string strSexAge = "";            
            public string strSuCode = "";
            public string strSuName = "";
            public string strType = ""; //용법구분
            public string strRQ = "";   //실제사용량
            public string strDrName = "";   //부서코드에 사용
        }

        public class cComSupPRT_ENDO_Pathology
        {
            public string GbJob = "";
            public string Gubun = "";
            public string Ptno = "";
            public string SName = "";
            public string Sex = "";
            public string Age = "";
            public string Jumin = "";
            public string DeptCode = "";
            public string DrName = "";
            public string RDrName = "";
            public string Room = "";
            public string RDate = "";
            public string SuCode = "";
            public string Remark1 = "";
            public string Remark4 = "";
            public string Result1 = "";            
            public string Result2 = "";
            public string Result3 = "";
            public string Result4 = "";
            public string Result5 = "";
            public string Result6 = "";
            public string Result62 = "";
            public string Result63 = "";
            public string Reqeust1 = "";
            public string lbRDrName = "";

        }

        public class cXrayPrt
        {
            public string Pano = "";
            public string Gubun = "";
            public string strSName = "";
            public string strBDate = "";
            public string strSexAge = "";            
            public string DeptCode = "";
            public string DrCode = "";
            public string DrName = "";             
            public string SeekDate = "";
            public string XName = "";
            public string ROWID = "";
        }

        //미시행 시트선택시 환자정보
        public class sPatInfo
        {

            public string strPano = "";
            public string strBDate = "";
            public string strDept = "";
            public string strDrCode = "";
            public string strExCode = "";
            public string strROWID = "";

        }

        #region //미시행 메인 관련 enum, 배열변수
        /// <summary> 미시행 검사내역 메인 enum </summary>
        public enum enmNoExecuteMain
        {
            check01 ,Gubun, BDate,  Pano,  SName,  Age,  Sex, DeptCode,DrCode,DrName,ExCode,
            ExName,JepDate,RDate,DelDate,ExamGubun,Wrtno,OrderNo,OrderRemark,Table,ROWID,
            ChkName,Memo,MemoSave
        }

        /// <summary> 미시행 검사내역 메인 컬럼헤드 배열 </summary>
        public string[] sSpdNoExecutoMain = { "선택","구분","처방일자","등록번호","성명","나이","SEX","과","의사코드","의사명","검사코드","검사명","접수일","예약일","삭제일자","검사구분","wrtno","오더넘버","오더참고사항","테이블","ROWID","검수자", "메모","메모저장" };

        /// <summary> 미시행 검사내역 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdNoExecutoMain = { 18,35,70,70,60,30,30,30,40,60,60,100,70,70,70,70,20,100,100,120,50,50,130,35 };

        /// <summary> 미시행 검사 메인 스프레드 표시 </summary>
        public void sSpd_NoExecuteMain(FarPoint.Win.Spread.FpSpread spd, string[] colName,int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmNoExecuteMain)).Length;            

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);

            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.check01, clsSpread.enmSpdType.CheckBox);            
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.Memo, clsSpread.enmSpdType.Text,null,null,null,null,false);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.MemoSave, clsSpread.enmSpdType.Button,null,"저장");
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //컬럼 머지
            methodSpd.setColMerge(spd, (int)enmNoExecuteMain.SName);
            methodSpd.setColMerge(spd, (int)enmNoExecuteMain.Pano);


            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmNoExecuteMain.ExName, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmNoExecuteMain.OrderRemark, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmNoExecuteMain.Memo, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.Gubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.Wrtno, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.OrderNo, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.Table, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            methodSpd.setSpdFilter(spd, (int)enmNoExecuteMain.Gubun, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //미시행 메인 예약관련 enum, 배열변수
        /// <summary> 예약정보 메인 enum </summary>
        public enum enmNoExecuteResv
        {
            Pano, SName, DeptCode, DrCode, RDate
        }

        /// <summary> 예약정보 메인 컬럼헤드 배열 </summary>
        public string[] sSpdNoExecuteResv = { "등록번호","성명","과","의사","예약일자" };

        /// <summary> 예약정보 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdNoExecuteResv = { 80,80,30,70,80 };

        /// <summary> 예약정보 메인 스프레드 표시 </summary>
        public void sSpd_NoExecuteResv(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmNoExecuteMain)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            //spd.VerticalScrollBarWidth = 10;
            //spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteResv., clsSpread.enmSpdType.CheckBox);
            ////methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.RDate, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteResv.RDate, clsSpread.enmSpdType.ComboBox, s);

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteResv.DrCode, clsSpread.enmSpdType.Hide);


        }

        #endregion
        
        #region //처방의 판독 판독지 관련 enum, 배열변수
        /// <summary> 처방의 판독 판독지 enum </summary>
        public enum enmXrayReadDr
        {
            chk,InDate,SeekDate,ExCode,ExName,ReadDate,Bigo,ROWID
        }

        /// <summary> 처방의 판독 판독지 컬럼헤드 배열 </summary>
        public string[] sSpdXrayReadDr = { " ","입력일자", "촬영일자", "검사코드", "촬영명", "판독지일자","비고","ROWID" };

        /// <summary> 처방의 판독 판독지 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayReadDr = { 20,80,80,80,200,80,400,50 };

        /// <summary> 처방의 판독 판독지 스프레드 표시 </summary>
        public void sSpd_XrayReadDr(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayReadDr)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXrayReadDr.chk, clsSpread.enmSpdType.CheckBox);
            ////methodSpd.setColStyle(spd, -1, (int)enmNoExecuteMain.RDate, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmNoExecuteResv.RDate, clsSpread.enmSpdType.ComboBox, s);

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayReadDr.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //심사용 미시행 메인 관련 enum, 배열변수
        /// <summary> 심사용 미시행 검사내역 메인 enum </summary>
        public enum enmMirNoExecuteMain
        {
            check01, BDate, Pano, SName, Gubun, DeptCode, DrCode, DrName, ExCode, ExName,
            OrderNo, JepDate, RDate,Amt1,Amt2, Johap
        }

        /// <summary> 심사용 미시행 검사내역 메인 컬럼헤드 배열 </summary>
        public string[] sSpdMirNoExecutoMain = { "선택", "처방일자", "등록번호", "성명","구분", "과", "의사코드", "의사명", "수가코드", "수가명", "오더넘버", "접수일", "예약일","발생금액", "부도금액","구분" };

        /// <summary> 심사용 미시행 검사내역 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdMirNoExecutoMain = { 20, 80, 80, 70, 30, 30, 50, 60, 80, 200, 100, 80, 80, 80, 80, 50 };

        /// <summary> 심사용 미시행 검사 메인 스프레드 표시 </summary>
        public void sSpd_MirNoExecuteMain(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirNoExecuteMain)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmMirNoExecuteMain.check01, clsSpread.enmSpdType.CheckBox); 
            //methodSpd.setColStyle(spd, -1, (int)enmMirNoExecuteMain.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirNoExecuteMain.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            //methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmMirNoExecuteMain.Amt1, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmMirNoExecuteMain.Amt2, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmMirNoExecuteMain.Johap, clsSpread.HAlign_C, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmMirNoExecuteMain.DrCode, clsSpread.enmSpdType.Hide);            
            methodSpd.setColStyle(spd, -1, (int)enmMirNoExecuteMain.OrderNo, clsSpread.enmSpdType.Hide);            


        }

        #endregion

        #region //심사용 미시행 메인 검체현황 관련 enum, 배열변수
        /// <summary> 심사용 미시행 검사내역 메인 enum </summary>
        public enum enmMirExamOrder
        {
            JDate,Pano,Bi,DeptCode,OrderNo,SpecNo,RDate,MasterCode,ExamName
        }

        /// <summary> 심사용 미시행 검사 검체현황 컬럼헤드 배열 </summary>
        public string[] sSpdExamOrder = { "접수일자", "등록번호", "보험", "과", "오더No", "검체번호", "예약일자", "검체코드","검체명" };


        /// <summary> 심사용 미시행 검사 검체현황 컬럼사이즈 배열 </summary>
        public int[] nSpdExamOrder = { 80,70,30,30,80,100,80,80,100 };

        /// <summary> 심사용 미시행 검사 메인 스프레드 표시 </summary>
        public void sSpd_ExamOrder(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmMirExamOrder)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            //spd.VerticalScrollBarWidth = 10;
            //spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmMirExamOrder.check01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirExamOrder.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmMirExamOrder.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);

            //히든            
            methodSpd.setColStyle(spd, -1, (int)enmMirExamOrder.OrderNo, clsSpread.enmSpdType.Hide);
            
        }

        #endregion

        #region //산부인과 검사 스케쥴 등록 폼 관련 - frmComSupEXSET01.cs

        public enum enmOgSch 
        {
            Chk, Change, Pano, RDate, RTime, Remark, Jong, Part, ROWID
        };
        public string[] sSpdOgSch = { "선택", "수정", "등록번호", "예약일자", "시:분", "내용", "검사종류", "검사부위","ROWID" };

        public int[] nSpdOgSch = { 30, 30, 80, 80, 50, 300, 35, 60, 100 };

        public void sSpd_OgSch(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { " ", "1", "2" };
            string[] s2 = { " ", "pelvic", "chest" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmOgSch)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Chk, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Pano, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.RDate, clsSpread.enmSpdType.Date);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.RTime, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Remark, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Jong, clsSpread.enmSpdType.ComboBox, s);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Part, clsSpread.enmSpdType.ComboBox, s2);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmOgSch.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든            
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.Change, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmOgSch.ROWID, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmOgSch.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmOgSch.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion

        #region //종합 환자 정보

        #region //예약관련 enum, 배열변수
        /// <summary> 예약정보 메인 enum </summary>
        public enum enmTotResv
        {
            Sort,Part, Jong, Pano, SName,STS,RSTS,RDate,DeptCode, DrCode,Code,Name,Remark,M,EMGWRTNO,Uid,ROWID
        }

        /// <summary> 예약정보 메인 컬럼헤드 배열 </summary>
        public string[] sSpdTotResv = { "Sort","파트", "종류", "등록번호", "성명","검사확인","현황", "예약일자", "과", "의사","코드","검사명칭","참고사항","M","EMGWRTNO","Uid","ROWID" };

        /// <summary> 예약정보 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdTotResv = { 20,70,60,80, 50,35, 35,110 ,30, 55,60,70,90,30,50,50,80};

        /// <summary> 예약정보 메인 스프레드 표시 </summary>
        public void sSpd_TotResv(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string argGpart = "")
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmTotResv)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmTotResv.Code, clsSpread.enmSpdType.CheckBox);
            ////methodSpd.setColStyle(spd, -1, (int)enmTotResv.RDate, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmTotResv.RDate, clsSpread.enmSpdType.ComboBox, s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotResv.Code, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotResv.Name, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotResv.Jong, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotResv.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.Sort, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.Pano, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.M, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmTotResv.SName, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.Remark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.EMGWRTNO, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.Uid, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotResv.ROWID, clsSpread.enmSpdType.Hide);

            // 5. sort, filter
            methodSpd.setSpdFilter(spd, (int)enmTotResv.Part, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            

            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "부도", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary);  //부도

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "당일", false);
            unary.BackColor = Color.LightPink;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary);

            if (argGpart == "0")
            {
                FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary2;
                unary2 = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "부도", false);
                unary2.BackColor = Color.SkyBlue;
                spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary2);
            }

        }

        #endregion

        #endregion                

        #region //컨설트 명단조회 enum, 배열변수
        /// <summary> 컨설트 명단조회 enum </summary>
        public enum enmConsultList
        {
            Ptno, SName, Sex, infect,GbERSMS,Time, WardCode,Room,TeamNo,DeptCode,FrDrCode,tab1, toDept, toDrCode
            , BDate,STS,Print, NST, Kekri, Move, fall, Bi
            , InDate
            ,DrCode,IPDNO
            ,Age,Return, ToREMARK, FrREMARK, ROWID
        }

        public string[] sSpdConsultList = { "등록번호","성명","성별","감염정보","응급","소요시간","병동","호실","팀번호","의뢰과","의뢰의사"," ","회신과","회신의사"
                                            ,"처방일자","상태","인쇄","NST","격리협진여부","거동","낙상","자격"
                                            ,"입원일"
                                            ,"의사코드","IPDNO"
                                            ,"나이","회송확인","TODRCODE","FRREMARK","ROWID" };

        public int[] nSpdConsultList = { 60,60,40,40,30,100,30,40,35,35,60,2,35,60
                                         ,80,30,30,30,60,30,30,30
                                         ,80
                                         ,60,50
                                         ,30,40,100,100,60 };

        public void sSpd_ConsultList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmConsultList)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.infect, clsSpread.enmSpdType.IMAGE);
            sup.setColStyle_Text(spd, -1, (int)enmConsultList.ToREMARK, true, true, false, 4000); //txt재정의
            sup.setColStyle_Text(spd, -1, (int)enmConsultList.FrREMARK, true, true, false, 4000); //txt재정의
            ////methodSpd.setColStyle(spd, -1, (int)enmTotResv.RDate, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmTotResv.RDate, clsSpread.enmSpdType.ComboBox, s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmConsultList.Code, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든                       
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.Time, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.IPDNO, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.DrCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmConsultList.toDept, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmConsultList.toDrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.Age, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.ToREMARK, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.FrREMARK, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultList.ROWID, clsSpread.enmSpdType.Hide);

            // 5. sort, filter
            methodSpd.setSpdFilter(spd, (int)enmConsultList.Ptno, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmConsultList.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmConsultList.DeptCode, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmConsultList.FrDrCode, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmConsultList.toDept, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmConsultList.toDrCode, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);



            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "", false);
            unary.BackColor = Color.Gray;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmConsultList.tab1, unary);  //

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "당일", false);
            //unary.BackColor = Color.Yellow;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary);  //당일

        }

        #endregion              

        #region //진료지원 BAS_BCODE 항목별 등록관리 enum, 배열변수
        /// <summary> 코드관리 메인 enum </summary>
        public enum enmBCodeMst
        {
            Chk,Change,Gubun,Code,Name
            ,Gubun2,Gubun3,Gubun4,Gubun5,Sort
            ,Part,Cnt,GuNum1,GuNum2,GuNum3
            ,JDate,EntDate,EntSabun, DelDate, ROWID
        }

        /// <summary> 코드관리 메인 컬럼헤드 배열 </summary>
        public string[] sSpdBCodeMst = { "삭제","수정","구분","코드","명칭"
            ,"구분2","구분3","구분4","구분5","SORT"
            ,"PART","CNT","구분N1","구분N2","구분N3"
            ,"적용일자", "등록일자","등록자", "삭제일자","ROWID" };

        /// <summary> 예약정보 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdBCodeMst = { 30,30,50,60,250
                                      ,50,50,50,50,50
                                      ,40,50,50,50,50
                                      ,80,80,60,80,60 };

        /// <summary> 코드관리 메인 스프레드 표시 </summary>
        public void sSpd_BCodeMst(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmBCodeMst)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Chk, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Code, clsSpread.enmSpdType.Text);
            //methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Name, clsSpread.enmSpdType.Text);
            //methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.RDate, clsSpread.enmSpdType.ComboBox, s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmBCodeMst.Chk, clsSpread.HAlign_R, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmBCodeMst.Code, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmBCodeMst.Name, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Gubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Change, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Gubun2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Gubun3, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Gubun4, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.Gubun5, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.GuNum1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.GuNum2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.GuNum3, clsSpread.enmSpdType.Hide);

            methodSpd.setColStyle(spd, -1, (int)enmBCodeMst.ROWID, clsSpread.enmSpdType.Hide);

            //// 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "부도", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary);  //부도

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "당일", false);
            //unary.BackColor = Color.Yellow;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmTotResv.RSTS, unary);  //당일

        }

        #endregion

        #region //영상의학 상용구 공통 등록관리 enum, 배열변수
        
        public enum enmResultSetUse
        {
            Key, Chk, Code, Remark, ROWID
        }

        
        public string[] sSpdResultSetUse = { "KEY", "선택", "코드", "상용 단어", "ROWID" };

        
        public int[] nSpdResultSetUse = { 60, 30, 60, 600, 80 };

        
        public void sSpd_ResultSetUse(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string Job)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmResultSetUse)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 15;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Chk, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Code, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Remark, clsSpread.enmSpdType.Text);
            sup.setColStyle_Text(spd, -1, (int)enmResultSetUse.Remark, true, true, false, 2000); //txt재정의


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmResultSetUse.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.ROWID, clsSpread.enmSpdType.Hide);
            if (Job == "TO")
            {
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Key, clsSpread.enmSpdType.Hide);
            }
            else if (Job == "XRAY")
            {
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Chk, clsSpread.enmSpdType.Hide);
                methodSpd.setColStyle(spd, -1, (int)enmResultSetUse.Code, clsSpread.enmSpdType.Hide);
            }

            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayReadList.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = null;

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            unary.BackColor = Color.LightGreen;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmResultSetUse.Key, unary);

            

        }

        #endregion


        #region //공통 시트검색 enum, 배열변수

        public enum enmComSupSearch
        {
            Job,Chk, Code, Name, ROWID
        }
        
        public string[] sSpdComSupSearch = {  "JOB","선택", "코드", "명칭", "ROWID" };


        public int[] nSpdComSupSearch = { 30, 60, 50, 270, 80 };


        public void sSpd_ComSupSearch(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt, string Job)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmComSupSearch)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmComSupSearch.Chk, clsSpread.enmSpdType.CheckBox);            
            //methodSpd.setColStyle(spd, -1, (int)enmComSupSearch.Remark, clsSpread.enmSpdType.Text);
            //sup.setColStyle_Text(spd, -1, (int)enmComSupSearch.Remark, true, true, false, 2000); //txt재정의


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmComSupSearch.Name, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmComSupSearch.ROWID, clsSpread.enmSpdType.Hide);
            if (Job == "")
            {
                
            }
            else if (Job == "XRAY")
            {
                methodSpd.setColStyle(spd, -1, (int)enmComSupSearch.Job, clsSpread.enmSpdType.Hide);
                methodSpd.setColStyle(spd, -1, (int)enmComSupSearch.Chk, clsSpread.enmSpdType.Hide);                
            }

            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayReadList.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayReadList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            //// 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = null;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.NotEqualTo, "", false);
            //unary.BackColor = Color.LightGreen;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmComSupSearch.Key, unary);



        }

        #endregion


        public void SPREAD_PRINT(FarPoint.Win.Spread.SheetView Spd, FarPoint.Win.Spread.FpSpread FpSpd, string[] argHead, string[] argFont, int argLef, int argTop, int ArgPortrait = 1, bool preview = true)
        {
            FarPoint.Win.Spread.PrintInfo pi = new FarPoint.Win.Spread.PrintInfo();

            //string strHead1 = "";
            //string strHead2 = "";
            //string strFont1 = "";
            //string strFont2 = "";

            //pi.ShowColumnHeaders = true;
            //pi.ShowRowHeaders = true;

            //argFont[0] = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            //argHead[0] = "/f1" + VB.Space(35) + " 약품의 재고구분 및 보관";

            //argFont[1] = "/fn\"바탕체\" /fz\"10\" /fb1 /fi0 /fu0 /fk0 /fs2";
            //argHead[1] = "/f2" + "인쇄일자 : " + DateTime.Now.ToString("yyyy-MM-dd");

            //SS1.PrintHeader = strFont1 + strHead1 + "/n/n" + strFont2 + strHead2



            //strHead1 = "/c/f1" + "퇴원예고자 List" + "/f1/n/n";
            //strHead2 = "/n/l/f2" + "퇴원일자 : " + Strings.Format(Conversion.Val(strOrdDate), "####년##월##일") + " /n/l/f2" + "출력시간 : " + strPrintTime.Substring(0, 2) + "시" + strPrintTime.Substring(2, 2) + "분" + " /r/f2" + "출력자 : " + clsType.gUseInfo.strUseName + "     /n";

            ////Print Head 지정
            //strFont1 = "/fn\"바탕체\" /fz\"14\" /fb1 /fi0 /fu0 /fk0 /fs1";
            //strFont2 = "/fn\"바탕체\" /fz\"12\" /fb0 /fi0 /fu0 /fk0 /fs2";


            string str = "";

            if (argFont.Length > 0)
            {
                for (int i = 0; i < argFont.Length; i++)
                {
                    str = str + argFont[i] + argHead[i];
                }

                //str = argFont[0] + argHead[0] +  argFont[1] + argHead[1];

                //pi.HeaderHeight = 50;
                pi.Header = str;


            }

            pi.Margin.Top = argTop;
            pi.Margin.Left = argLef;

            pi.Margin.Bottom = 10;
            pi.Margin.Right = 10;

            pi.ShowGrid = false; ;
            pi.ShowBorder = true;
            pi.ShowShadows = true;
            pi.ShowColor = true;
            //pi.UseMax = true;
            //pi.BestFitCols = true;
            if (ArgPortrait == 1)
            {
                pi.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            }
            else
            {
                pi.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            }
            pi.UseSmartPrint = false;
            pi.Preview = preview;

            Spd.PrintInfo = pi;

            FpSpd.PrintSheet(0);

        }

        
       

    }
}
