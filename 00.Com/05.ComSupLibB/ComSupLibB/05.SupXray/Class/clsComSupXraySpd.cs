using System;
using System.Drawing;
using ComBase; //기본 클래스
using ComSupLibB.Com;

namespace ComSupLibB.SupXray
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXraySpd.cs
    /// Description     : 진료지원 공통 영상의학과 스프레드 표시관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-19
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>    
    public class clsComSupXraySpd
    {
        Com.clsMethod method = new Com.clsMethod(); 
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();        
        
        public class cComSupPRT_XRAYCont
        {            
            public string strPano = "";
            public string strSName = "";
            public string strSexAge = "";
            public string strSuCode = "";
            public string strSuName = "";
        }        

        #region //영상의학과 촬영일자별 판독명단 VIEW 공통 enum, 배열변수
        /// <summary> 영상의학과 촬영일자별 판독명단 VIEW 공통 enum </summary>
        public enum enmXrayViewList
        {
            check01, Pano, SName, DeptCode,DrCode,
            DrName, GbIO, Ward, Room,ExCode,
            ExName, Exid,Age, Sex, XJong,
            SeekDate,  result, ROWID
        }

        /// <summary> 영상의학과 판독명단(촬영자별) 공통 명단 컬럼헤드 배열 </summary>
        public string[] sSpdXrayViewList = { "선택", "등록번호", "성명","과","의사코드",
                                             "의사명","구분","병동","호실","검사코드",
                                              "촬영명칭","촬영자", "나이","성별","XJONG",
                                              "SeekDate","판독결과","ROWID"
                                             };

        /// <summary> 미시행 검사내역 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayViewList = { 18,70, 70, 30,50,
                                          60,30,30,30,50,
                                          120,80,30,30,30,
                                          80,200 ,80                                        
                                         };

        /// <summary> 영상의학과 판독명단(촬영자별) 공통 명단 스프레드 표시 </summary>
        public void sSpd_XrayViewList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayViewList)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmXrayViewList.check01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayViewList.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayViewList.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayViewList.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayViewList.result, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayViewList.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayViewList.ROWID, clsSpread.enmSpdType.Hide);



            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayViewList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayViewList.DrName, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayViewList.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayViewList.STS00, unary);




        }

        #endregion

        #region //영상의학과 일보 enum, 배열변수
        /// <summary> 영상의학과 영상의학과 일보 공통 enum </summary>
        public enum enmXrayIlbo
        {
            DeptName,DeptCnt1,DeptCnt2,DeptCnt3,tab1,
            mCode,mName,mSomo,mRpt,mJego,
            mSum,tab2,sGisa,sCnt1,sCnt2,
            sCnt3,tab3,sGisa2,sRepert,Bigo
        }

        /// <summary> 영상의학과 영상의학과 일보 컬럼헤드 배열 </summary>
        public string[] sSpdXrayIlbo = { "진료과명","인원","촬영","부위"," ",
                                          "재료코드" ,"재료명","소모","RPT","재고",
                                          "합계"," ","촬영기사","인원","촬영",
                                          "부위"," ","촬영기사","Repert","비고"  };

        /// <summary> 미시행 검사내역 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayIlbo = { 100,40,40,40,5,
                                      80,100,40,30,40,
                                      40,5,70,40,40,
                                      40,5,70,60,80  
                                         };

        /// <summary> 영상의학과 일보 스프레드 표시 </summary>
        public void sSpd_XrayIlbo(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayIlbo)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo.check01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmXrayCodeMst.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo.DrCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo.ROWID, clsSpread.enmSpdType.Hide);



            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayIlbo.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayIlbo.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayIlbo.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayIlbo.STS00, unary);


        }

        #endregion

        #region //영상의학과 부위별 일보 enum, 배열변수
        /// <summary> 영상의학과 촬영일자별 판독명단 VIEW 공통 enum </summary>
        public enum enmXrayIlbo2
        {
            xJ1, xJ1Cnt1, xJ1Cnt2, tab1,
            xJ2, xJ2Cnt1, xJ2Cnt2, tab2,
            xJ3, xJ3Cnt1, xJ3Cnt2, tab3,
            xJ4, xJ4Cnt1, xJ4Cnt2, tab4,
            xJ5, xJ5Cnt1, xJ5Cnt2, tab5,
            xJ6, xJ6Cnt1, xJ6Cnt2, tab6,
            xJ7, xJ7Cnt1, xJ7Cnt2, tab7,
        }

        /// <summary> 영상의학과 판독명단(촬영자별) 공통 명단 컬럼헤드 배열 </summary>
        public string[] sSpdXrayIlbo2 = { "일반 촬영","부위","촬영"," ",
                                          "특수 촬영","부위","촬영"," ",
                                          "SONO 촬영","부위","촬영"," ",
                                          "CT 촬영","부위","촬영"," ",
                                          "MRI 촬영","부위","촬영"," ",
                                          "RI 촬영","부위","촬영"," ",
                                          "BMD 촬영","부위","촬영"," ",
                                        };

        /// <summary> 미시행 검사내역 메인 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayIlbo2 = {  100,30,30,5,
                                        100,30,30,5,
                                        100,30,30,5,
                                        100,30,30,5,
                                        100,30,30,5,
                                        100,30,30,5,
                                        100,30,30,5,
                                         };

        /// <summary> 영상의학과 판독명단(촬영자별) 공통 명단 스프레드 표시 </summary>
        public void sSpd_XrayIlbo2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayIlbo2)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo2.check01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo2.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo2.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmXrayCodeMst.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo2.DrCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayIlbo2.ROWID, clsSpread.enmSpdType.Hide);



            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayIlbo2.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayIlbo2.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayIlbo2.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayIlbo2.STS00, unary);




        }

        #endregion

        #region //영상의학과 기초코드마스터 enum, 배열변수
        /// <summary> 영상의학과 기초코드마스터 공통 enum </summary>
        public enum enmXrayCodeMst
        {
            XCode,XName,SubCode,SubName,SeekGbn,
            Res,Cnt1,Cnt2,Buse,ClassCode,
            ROWID 
        }

        /// <summary> 영상의학과 기초코드마스터 컬럼헤드 배열 </summary>
        public string[] sSpdXrayCodeMst = { "검사코드","검사명","소분류","소분류명","촬영방법",
                                            "예약","부위건수","촬영건수","촬영장소","ClassCode",
                                            "ROWID"  };

        /// <summary> 기초코드마스터 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayCodeMst = { 100,200,60,80,40,
                                         30,60,60,200,30,
                                         100 };

        /// <summary> 영상의학과 기초코드마스터 스프레드 표시 </summary>
        public void sSpd_XrayCodeMst(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { "", "1", "2", "3", "4", "5", "6" };


            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCodeMst)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.check01, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.ChkName, clsSpread.enmSpdType.ComboBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.ChkName, clsSpread.enmSpdType.ComboBox,s);

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayCodeMst.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayCodeMst.Buse, clsSpread.HAlign_L, clsSpread.VAlign_C);
                        

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.SeekGbn, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.ClassCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCodeMst.ROWID, clsSpread.enmSpdType.Hide);


            // 5. sort, filter
            //methodSpd.setSpdFilter(spd, (int)enmXrayCodeMst.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdSort(spd, (int)enmXrayCodeMst.DrName, true);
            //methodSpd.setSpdFilter(spd, (int)enmXrayCodeMst.DrName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "종검당일상담", false);
            //unary.BackColor = Color.BlueViolet;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayCodeMst.STS00, unary);

            //색상
            spd.ActiveSheet.Columns.Get(0).BackColor = System.Drawing.Color.LightYellow; //ROW전체


        }

        #endregion

        #region //영상의학과 기초코드마스터 help enum, 배열변수
        /// <summary> 영상의학과 기초코드마스터 공통 enum </summary>
        public enum enmXrayCodeHelp
        {
            BuCode,Buse
        }

        /// <summary> 영상의학과 기초코드마스터 help 컬럼헤드 배열 </summary>
        public string[] sSpdXrayCodeHelp = { "부서코드","촬영장소" };

        /// <summary> 기초코드마스터 help 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayCodeHelp = { 80,200};

        /// <summary> 영상의학과 기초코드마스터 help 스프레드 표시 </summary>
        public void sSpd_XrayCodeHelp(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
           
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCodeHelp)).Length;

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

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmXrayMCode.MName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCodeHelp.SeekGbn, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 기초코드 대분류 등록 enum, 배열변수
        /// <summary> 영상의학과 기초코드 대분류 등록  enum </summary>
        public enum enmXrayCode2
        {
            Code, Name, ROWID
        }

        /// <summary> 영상의학과 기초코드 대분류 등록 컬럼헤드 배열 </summary>
        public string[] sSpdXrayCode2 = { "대분류코드", "대분류명","ROWID" };

        /// <summary> 기초코드 대분류 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayCode2 = { 80, 350,100 };

        /// <summary> 영상의학과 기초코드 대분류 등록 스프레드 표시 </summary>
        public void sSpd_XrayCode2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCode2)).Length;

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

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmXrayMCode.MName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayCode2.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 기초코드 소분류 등록 enum, 배열변수
        /// <summary> 영상의학과 기초코드 소분류 등록  enum </summary>
        public enum enmXrayCode3
        {
            SubCode, SubName,ClassCode,ClassName,ROWID
        }

        /// <summary> 영상의학과 기초코드 소분류 등록 컬럼헤드 배열 </summary>
        public string[] sSpdXrayCode3 = { "소분류코드","소분류명","대분류코드","대분류명","ROWID" };

        /// <summary> 기초코드 소분류 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayCode3 = { 80, 300 ,80, 200, 100 };

        /// <summary> 영상의학과 기초코드 소분류 등록 스프레드 표시 </summary>
        public void sSpd_XrayCode3(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCode3)).Length;

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

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            //methodSpd.setColAlign(spd, (int)enmXrayMCode.MName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayCode3.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 재료코드 등록 enum, 배열변수
        /// <summary> 영상의학과 기초코드 재료코드 등록  enum </summary>
        public enum enmXrayMCode
        {
            MCode, MName, JCode, JGubun,Qty,
            Unit, ROWID
        }

        /// <summary> 영상의학과 기초코드 재료코드 등록 컬럼헤드 배열 </summary>
        public string[] sSpdXrayMCode = { "재료코드", "재료명", "자재코드", "재료구분","기본량",
                                          "단위", "ROWID" };

        /// <summary> 기초코드 재료코드 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayMCode = { 80,200,80,80,80,
                                       80,100 };

        /// <summary> 영상의학과 기초코드 재료코드 등록 스프레드 표시 </summary>
        public void sSpd_XrayMCode(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayMCode)).Length;

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

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayMCode.MName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayMCode.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 기본사용량 등록 enum, 배열변수
        /// <summary> 영상의학과 기초코드 재료코드 등록  enum </summary>
        public enum enmXrayUse
        {
            Change,chk,Gubun2,Gubun2Name,MCode,Qty,
            MName,Agree, ROWID
        }

        /// <summary> 영상의학과 기초코드 재료코드 등록 컬럼헤드 배열 </summary>
        public string[] sSpdXrayUse = { "수정","선택","성인구분","구분명칭","재료코드",
                                        "수량","재료명","조영제동의", "ROWID" };

        /// <summary> 기초코드 재료코드 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayUse = { 30,20,50,80,100,
                                     30,200,80,100 };

        /// <summary> 영상의학과 기초코드 재료코드 등록 스프레드 표시 </summary>
        public void sSpd_XrayUse(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            string[] s = { " ","0" ,"1","2","3","4"};

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayUse)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmXrayUse.chk, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmXrayUse.Qty, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmXrayUse.Gubun2, clsSpread.enmSpdType.ComboBox, s);
            methodSpd.setColStyle(spd, -1, (int)enmXrayUse.Agree, clsSpread.enmSpdType.CheckBox);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayUse.MName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든            
            methodSpd.setColStyle(spd, -1, (int)enmXrayUse.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 기본사용량  종류명단, 재료명단  enum, 배열변수
        /// <summary> 영상의학과 기초코드 종류명단, 재료명단  enum </summary>
        public enum enmXrayUseJong
        {
            Jong,JongName
        }

        /// <summary> 영상의학과 기초코드 종류명단, 재료명단 컬럼헤드 배열 </summary>
        public string[] sSpdXrayUseJong = { "검사종류","검사명칭" };

        /// <summary> 기초코드 종류명단, 재료명단 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayUseJong = { 80, 350 };

        /// <summary> 영상의학과 기초코드종류명단, 재료명단 스프레드 표시 </summary>
        public void sSpd_XrayUseJong(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayUseJong)).Length;

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
            
            

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmXrayUseJong.JongName, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmXrayUseJong.Gubun2, clsSpread.enmSpdType.Hide);


        }

        public enum enmXrayUseMCode
        {
            MCode,MName
        }

        /// <summary> 영상의학과 기초코드 종류명단, 재료명단 컬럼헤드 배열 </summary>
        public string[] sSpdXrayUseMCode = { "재료코드","재료명칭" };

        /// <summary> 기초코드 종류명단, 재료명단 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayUseMCode = { 80, 350 };

        /// <summary> 영상의학과 기초코드종류명단, 재료명단 스프레드 표시 </summary>
        public void sSpd_XrayUseMCode(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayUseMCode)).Length;

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
                        

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmXrayUseMCode.MName, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);


            //히든            
            //methodSpd.setColStyle(spd, -1, (int)enmXrayUseMCode.ROWID, clsSpread.enmSpdType.Hide);


        }

        #endregion

        #region //영상의학과 판독 상용결과 등록 enum, 배열변수 - frmComSupXraySET03.cs //frmComSupXraySET17.cs
        /// <summary> 영상의학과 판독 상용결과 등록  enum </summary>
        public enum enmXrayReadSet
        {
           XJong,XJongName,SetName,ExName,Result, Result1,
            ROWID
        }

        /// <summary> 영상의학과 판독 상용결과 등록 컬럼헤드 배열 </summary>
        public string[] sSpdXrayReadSet = { "종류","종류","상용코드명","검사명칭","상용 판독결과","상용 판독결과2",
                                            "ROWID" };

        /// <summary> 판독 상용결과 등록 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayReadSet = { 50,50,190,180,300,300,
                                        100 };

        /// <summary> 영상의학과 판독 상용결과 등록 스프레드 표시 </summary>
        public void sSpd_XrayReadSet(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayReadSet)).Length;

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
            sup.setColStyle_Text(spd, -1, (int)enmXrayReadSet.Result, true, true, false, 2000); //txt재정의
            sup.setColStyle_Text(spd, -1, (int)enmXrayReadSet.Result1, true, true, false, 2000); //txt재정의

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayReadSet.SetName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayReadSet.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayReadSet.Result, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayReadSet.XJong, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayReadSet.Result1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayReadSet.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            methodSpd.setSpdFilter(spd, (int)enmXrayReadSet.SetName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayReadSet.XJongName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayReadSet.SetName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayReadSet.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //영상의학과 오더판넬선택 enum, 배열변수
        /// <summary> 영상의학과 오더판넬선택  enum </summary>
        public enum enmXrayOrdCode
        {
            STS,OrderName,OrderCode,GbInput,GbInfo,GbBoth,
            Bun, NextCode,SuCode,GbDosage, SpecCode,
            Slipno, GbImiv, SubRate,ROWID
        }

        /// <summary> 영상의학과 오더판넬선택 컬럼헤드 배열 </summary>
        public string[] sSpdXrayOrdCode = { "구분","오더명칭","OrderCode","GbInput","GbInfo","GbBoth",
                                            "Bun","NextCode","SuCode","GbDosage", "SpecCode",
                                            "Slipno","GbImiv", "SubRate","ROWID" };

        /// <summary> 오더판넬선택 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayOrdCode = { 20,320,100,80,80,80,
                                         80,80,80,80,80,
                                         80,80,80,100 };

        /// <summary> 영상의학과 오더판넬선택 스프레드 표시 </summary>
        public void sSpd_XrayOrdCode(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayOrdCode)).Length;

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

            //정렬            
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C); ;
            methodSpd.setColAlign(spd, (int)enmXrayOrdCode.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Hide);
            spd.ActiveSheet.Columns[0].Visible = true;
            spd.ActiveSheet.Columns[1].Visible = true;

        }

        #endregion

        #region //영상의학과 오더Sub선택 enum, 배열변수
        /// <summary> 영상의학과 오더Sub선택  enum </summary>
        public enum enmXrayOrdSubCode
        {
            SuCode,SubName, ROWID
        }

        /// <summary> 영상의학과 오더판넬선택 컬럼헤드 배열 </summary>
        public string[] sSpdXrayOrdSubCode = { "오더코드","명칭","ROWID" };

        /// <summary> 오더판넬선택 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayOrdSubCode = {80,300,100 };

        /// <summary> 영상의학과 오더판넬선택 스프레드 표시 </summary>
        public void sSpd_XrayOrdSubCode(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayOrdSubCode)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayOrdSubCode.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayOrdSubCode.SubName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든   
            methodSpd.setColStyle(spd, -1, (int)enmXrayOrdSubCode.SuCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayOrdSubCode.ROWID, clsSpread.enmSpdType.Hide);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayOrdSubCode.Result, unary); //결과


        }

        #endregion

        #region //영상의학과 재료코드조회 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXrayVIEW03.cs
        /// <summary> 영상의학과 재료코드조회  enum </summary>
        public enum enmXrayView03
        {
            mCode,mName,mGubun,Qty,Unit,
            ROWID
        }

        /// <summary> 영상의학과 재료코드조회 컬럼헤드 배열 </summary>
        public string[] sSpdXrayView03 = { "재료코드","재료명","재료구분","기본량","단위",
                                            "ROWID" };

        /// <summary> 재료코드조회 컬럼사이즈 배열 </summary>
        public int[] nSpdXrayView03 = { 80,200,80,50,60,
                                        100 };

        /// <summary> 영상의학과 재료코드조회 스프레드 표시 </summary>
        public void sSpd_XrayView03(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayView03)).Length;

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

            //정렬
            methodSpd.setColAlign(spd, -1, FarPoint.Win.Spread.CellHorizontalAlignment.Center, FarPoint.Win.Spread.CellVerticalAlignment.Center);
            methodSpd.setColAlign(spd, (int)enmXrayView03.mName, FarPoint.Win.Spread.CellHorizontalAlignment.Left, FarPoint.Win.Spread.CellVerticalAlignment.Center);


            //히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayView03.ROWID, clsSpread.enmSpdType.Hide);
            

        }

        #endregion

        #region //영상의학과 조영제부작용 등록 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXraySET04.cs
        /// <summary> 영상의학과 조영제부작용  enum </summary>
        public enum enmXrayContrast
        {
            Pano,BDate,SName,Remark,EntSabun, ROWID
        }
        
        public string[] sSpdXrayContrast = { "등록번호","발생일자","성명","참고사항","등록사번","ROWID" };
        
        public int[] nSpdXrayContrast = { 80,80,80,400,80,80 };
                
        public void sSpd_XrayContrast(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayContrast)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayContrast.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayContrast.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayContrast.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터            
            //methodSpd.setSpdFilter(spd, (int)enmXrayContrast.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsRCP01A.Result, unary); //결과

            

        }

        #endregion

        #region //영상의학과 영상누락자 리스트 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXrayLIST01.cs
        /// <summary> 영상의학과 영상누락자 리스트  enum </summary>
        public enum enmXrayList01
        {
            GbIO,Pano, SName, XJong,Sex
           ,Age,DeptCode,WardCode,XName,JepDate
           ,ROWID
        }

        public string[] sSpdXrayList01 = { "입원외래", "등록번호", "성명", "종류", "성별"
                                          ,"나이","진료과","병동","검사명","접수일자"
                                          , "ROWID" };

        public int[] nSpdXrayList01 = { 60,80,80,40,40
                                       ,40,50,40,200,100
                                       ,80 };

        public void sSpd_XrayList01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayList01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayList01.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayList01.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayList01.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터            
            //methodSpd.setSpdFilter(spd, (int)enmXrayList01.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList01.Result, unary); //결과



        }

        #endregion

        #region //영상의학과 촬영대기순번 리스트 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXrayLIST02.cs
        /// <summary> 영상의학과 촬영대기순번 리스트  enum </summary>
        public enum enmXrayList02
        {
            SName,Pano, Remark, DeptCode,JepTime
            ,EndTime,Gubun, ROWID
        }

        public string[] sSpdXrayList02 = { "성명","등록번호", "비고", "과","접수시각"
                                           ,"제외시간","구분", "ROWID" };

        public int[] nSpdXrayList02 = { 50,70,180,20,100
                                       ,150,30,80 };

        public void sSpd_XrayList02(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayList02)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayList02.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayList02.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayList02.Gubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList02.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터            
            //methodSpd.setSpdFilter(spd, (int)enmXrayList02.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "응급환자", false);
            unary.BackColor = method.cSpdCellImpact_Back;
            unary.ForeColor = method.cSpdCellImpact_Fore;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList02.Remark, unary); //

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "어르신먼저", false);
            unary.BackColor = Color.FromArgb(0, 192, 192);
            unary.ForeColor = method.cSpdCellImpact_Fore;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList02.Remark, unary); //

            unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "원거리환자", false);
            unary.BackColor = Color.FromArgb(255, 192, 255);
            unary.ForeColor = method.cSpdCellImpact_Fore;
            spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList02.Remark, unary); //



        }

        #endregion

        #region //영상의학과 CT촬영 대기자 리스트 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXrayLIST03.cs
        /// <summary> 영상의학과 CT촬영 대기자 리스트  enum </summary>
        public enum enmXrayList03
        {
            SName,Pano,RTime,Remark,Seqno
           ,EndTime,ROWID
        }

        public string[] sSpdXrayList03 = { "성명","등록번호", "예약시간", "비고", "순서조정"
                                          ,"제외시간", "ROWID" };

        public int[] nSpdXrayList03 = { 50,60,80,30,50
                                       ,60,80 };

        public void sSpd_XrayList03(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayList03)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayList03.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayList03.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayList03.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터            
            //methodSpd.setSpdFilter(spd, (int)enmXrayList03.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList03.Result, unary); //결과



        }

        #endregion

        #region //영상의학과 근전도 검사 리스트 enum, 배열변수 - ComSupLibB.SupXray.frmComSupXrayLIST04.cs
        /// <summary> 영상의학과 CT촬영 대기자 리스트  enum </summary>
        public enum enmXrayList04
        {
            Pano,SName,Sex,Age,Ward
            ,Room,DeptCode,DrCode,XCode,XName
            ,Remark2,STS00, STS01,BDate,SeekDate,RDate
            ,Time1,Time2,WRTNO,EmgWRTNO,Remark
            ,ROWID
        }

        public string[] sSpdXrayList04 = { "등록번호","성명","성별","나이","병동"
                                           ,"호실","과","의사코드","검사코드","검사명"
                                           ,"참고","판독" ,"검사지","처방일자","검사일자","예약일자"
                                           ,"검사대기기간","결과보고소요기간","WRTNO","EmgWRTNO","참고사항"
                                           ,"ROWID" };

        public int[] nSpdXrayList04 = { 70,80,30,30,50
                                       ,40,40,60,60,150
                                       ,30,30,50,80,80,80
                                       ,60,70,60,60,80  
                                       ,80 };

        public void sSpd_XrayList04(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayList04)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayList03.Infect, clsSpread.enmSpdType.IMAGE);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayList04.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayList04.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.XCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.Ward, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.WRTNO, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.EmgWRTNO, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.Remark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayList04.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터            
            //methodSpd.setSpdFilter(spd, (int)enmXrayList04.ExName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayList04.Result, unary); //결과



        }

        #endregion

        #region //영상의학과 SEND enum, 배열변수 
        public enum enmSpdXSend { chk, SendTime, PacsNo, Pano, SName, SendGbn, OrderCode, OrderName };
        public string[] sSpdXSend = { " ","전송시각", "PacsNo", "등록번호", "성명", "전송구분", "오더코드", "오더명칭" };
        public int[] nSpdXSend = { 1,135, 85, 70, 80, 60, 110, 270 };

        public void sSpd_XSend(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSpdXSend)).Length;

            spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmConsultview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSpdXSend.OrderCode, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSpdXSend.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmSpdXSend.chk, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmSpdXSend.ToDrCode, clsSpread.enmSpdType.Hide);


            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmConsultview.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmConsultview.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion                

        #region //영상의학 예약변경관리 enum, 배열변수 - frmComSupXrayRESV01.cs
        /// <summary> 영상의학 예약변경관리 enm </summary>        
        public enum enmXrayResv01
        {
            chk, Pano, SName,Age,Sex
            ,XCode,XName, SeekDate, SeekTime, XRoom
            ,GbIO,DeptCode,DrCode,WardCode,RoomCode
            ,Exid,SeekDateOld,SeekTimeOld,XJong, XRoomOld,XRoomChange
            ,Change, ROWID

        }

        public string[] sSpdenmXrayResv01 = { "선택","등록번호","성명","나이","성별"
                                             ,"검사코드","검사명","촬영일자","시간","촬영실"
                                             ,"I/O","과","의사코드","병동","호실"
                                             ,"기사","촬영일old","촬영시간old","종류","촬영실old","촬영실변경"
                                             ,"변경","ROWID"
                                                };


        public int[] nSpdenmXrayResv01 = { 40,80,80,25,25
                                           ,60,190,80,50,40
                                           ,30,30,40,30,30
                                           ,70,80,60,50,40,90
                                           ,20,70    
                                              };


        public void sSpd_enmXrayResv01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayResv01)).Length;

            string[] s = { "", "L.MRI(Skyra)","M.MRI(Verio)","S.초음파본관","Q.초음파종검","D.C-T 64ch", "C.C-T 128ch","I.CT-Drive", "J.CT-Force"};

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.FrozenColumnCount = (int)enmXrayResv01.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.chk, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.SeekDate, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.SeekTime, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.XRoom, clsSpread.enmSpdType.Text);            
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.XRoomChange, clsSpread.enmSpdType.ComboBox,s);

            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayResv01.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //4.히든                                                
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.chk, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.SeekDateOld, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.SeekTimeOld, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.XJong, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.DrCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.Exid, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.XRoomOld, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.XRoomChange, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.Change, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayResv01.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            methodSpd.setSpdFilter(spd, (int)enmXrayResv01.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayResv01.Chk, unary); //점검



        }

        #endregion                

        #region //영상의학 이상검사결과 등록관리 enum, 배열변수 - frmComSupXraySET05.cs
        /// <summary> 영상의학 이상검사결과 등록관리 enm </summary>        
        public enum enmXrayCVR01
        {
            chk,Pano, SName,GbIO,Sex
            ,Age,SeekDate,XCode,XName,Exid
            ,DeptCode,DrName,CVR,CVR_Date,CVR_Name1
            ,CVR_Name2,CVR_Send,CVR_CDate, CVR_Gubun,ROWID

        }

        public string[] sSpdenmXrayCVR01 = { "선택","등록번호","성명","외래입원","성별"
                                             ,"나이","촬영일자","검사코드","검사명","촬영자"
                                             ,"과","의사","CVR","보고일시","보고자"
                                             ,"보고받는자","CVR전송","CVR확인일시","보고방법","ROWID"
                                                };


        public int[] nSpdenmXrayCVR01 = { 40,80,80,40,30
                                          ,30,80,60,200,50
                                          ,30,50,40,80,50
                                          ,80,50,120,70
                                              };


        public void sSpd_enmXrayCVR01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCVR01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.FrozenColumnCount = (int)enmXrayCVR01.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCVR01.chk, clsSpread.enmSpdType.CheckBox);            
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCVR01.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayCVR01.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                                
            methodSpd.setColStyle(spd, -1, (int)enmXrayCVR01.chk, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCVR01.CVR_Send, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCVR01.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXrayResv01.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayCVR01.Chk, unary); //점검



        }

        #endregion

        #region //영상의학 CD복사신청 관리 enum, 배열변수 - frmComSupXrayRCPN01.cs


        /// <summary> 영상의학 CD복사신청 관리 - 영상리스트 (1/3) enm </summary>        
        public enum enmXrayCDCopy01
        {
            chk,SeekDate,STS01,STS02,XJong
            ,XName,DeptCode,DrCode,DrName,XCode,PacsNo
            ,ReadNo,PacsUid,XJong2,OrderDate,JepTime
            ,XSendDate,XSubCode,Size,Copy_ROWID, ROWID

        }

        public string[] sSpdenmXrayCDCopy01 = { "선택","촬영일자","판독","영상","종류"
                                              ,"촬영명","과","의사코드","의사","코드","PACSNO"
                                              ,"판독번호","PacsUid","XJong","검사지시","접수시간"
                                              ,"PACS전송","XSubCode","Size","CdCopy_ROWID","ROWID"
                                                };


        public int[] nSpdenmXrayCDCopy01 = { 30,100,30,30,50
                                            ,200,30,40,60,70,60
                                            ,60,60,50,100,100       
                                           ,80,50,50,80,70
                                              };


        public void sSpd_enmXrayCDCopy01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCDCopy01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXrayCDCopy01.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            spd.ActiveSheet.ColumnHeader.Cells[0, 0].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
            spd.ActiveSheet.ColumnHeader.Cells[0, 0].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            spd.ActiveSheet.ColumnHeader.Cells[0, 0].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.chk, clsSpread.enmSpdType.CheckBox);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);            
            methodSpd.setColAlign(spd, (int)enmXrayCDCopy01.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                            
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.XJong2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.ReadNo, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.PacsUid, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.XSendDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.XSubCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.Size, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.Copy_ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy01.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXrayCDCopy01.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayCDCopy01.Chk, unary); //점검
            
        }


        /// <summary> 영상의학 CD복사신청 관리 - 신청리스트 (2/3) enm </summary>        
        public enum enmXrayCDCopy02
        {
            chk,BDate, Pano, SName,CdQty,Qty
            ,WardCode,EntSabun,OrderSTS,CdMake,CdMakeTime
            ,CdGubun, ROWID

        }

        public string[] sSpdenmXrayCDCopy02 = { "선택","신청일자","등록번호","성명","CD-갯수","영상갯수"
                                                ,"병동","신청인","오더여부","CD작업자","CD작업완료시간"
                                                ,"구분" ,"ROWID"
                                                };


        public int[] nSpdenmXrayCDCopy02 = { 40,80,80,100,40,40
                                            ,50,80,40,120,200   
                                            ,30,70
                                              };


        public void sSpd_enmXrayCDCopy02(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCDCopy02)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.FrozenColumnCount = (int)enmXrayCDCopy02.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy02.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmXrayCDCopy02.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //4.히든                                                
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy02.chk, clsSpread.enmSpdType.Hide);            
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy02.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXrayCDCopy02.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayCDCopy02.Chk, unary); //점검

        }



        /// <summary> 영상의학 CD복사신청 관리 - 상세내역 (3/3) enm </summary>        
        public enum enmXrayCDCopy03
        {
            chk,BDate, Pano, SName, SeekDate
            ,XJong,XCode,XName,DeptCode,DrCode
            ,EndTime, ROWID

        }

        public string[] sSpdenmXrayCDCopy03 = { "선택","신청일자","등록번호","성명","촬영일자"
                                             ,"종류","코드","촬영명","과","의사"
                                             ,"전송시간","ROWID"
                                                };


        public int[] nSpdenmXrayCDCopy03 = { 40,80,80,80,80
                                           ,40,80,180,30,50                                           
                                           ,160,70
                                              };


        public void sSpd_enmXrayCDCopy03(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayCDCopy03)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.FrozenColumnCount = (int)enmXrayCDCopy03.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy03.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayCDCopy03.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                                
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy03.chk, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayCDCopy03.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXrayCDCopy03.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayCDCopy03.Chk, unary); //점검

        }

        #endregion

        #region //영상의학 콜시간 상세 등록관리 enum, 배열변수 - frmComSupXraySET08.cs
        
        public enum enmXraySET08
        {
            BDate,SName,SDate,EDate,Remark
            ,ROWID

        }

        public string[] sSpdenmXraySET08 = { "일자","성명","시작시간","종료시간","참고사항"
                                            ,"ROWID"  };


        public int[] nSpdenmXraySET08 = { 80,70,120,120,300
                                        ,70 };


        public void sSpd_enmXraySET08(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET08)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);            
            //methodSpd.setColStyle(spd, -1, (int)enmXraySET08.Infect, clsSpread.enmSpdType.IMAGE);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET08.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //4.히든                                                            
            methodSpd.setColStyle(spd, -1, (int)enmXraySET08.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXraySET08.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXraySET08.Chk, unary); //점검



        }

        #endregion

        #region //영상의학 한글 영문변환 등록관리 enum, 배열변수 - frmComSupXraySET09.cs

        public enum enmXraySET09
        {
            Han,Eng, Remark,Eng_Old, ROWID

        }

        public string[] sSpdenmXraySET09 = { "한글","영문","비    고","영문 old","ROWID"  };


        public int[] nSpdenmXraySET09 = { 100,100,250,70,70 };


        public void sSpd_enmXraySET09(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET09)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET09.Eng, clsSpread.enmSpdType.Text);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET09.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                            
            methodSpd.setColStyle(spd, -1, (int)enmXraySET09.Eng_Old, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET09.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXraySET09.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXraySET09.Chk, unary); //점검



        }

        #endregion

        #region //영상의학 개인별 판독 내역조회 enum, 배열변수 - frmComSupXraySET10.cs

        public enum enmXraySET10
        {
            ReadDate,XJong,XCode,XName,Result
            ,Result1, ROWID

        }

        public string[] sSpdenmXraySET10 = { "판독일자","종류","검사코드","검사명","결과"
                                            ,"결과1", "ROWID" };


        public int[] nSpdenmXraySET10 = { 80,60,70,420,70
                                        ,70,70 };


        public void sSpd_enmXraySET10(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET10)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 15;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);            
            sup.setColStyle_Text(spd, -1, (int)enmXraySET10.Result, true, true, false, 2000); //txt재정의
            sup.setColStyle_Text(spd, -1, (int)enmXraySET10.Result1, true, true, false, 2000); //txt재정의
            //methodSpd.setColStyle(spd, -1, (int)enmXraySET10.XName, clsSpread.enmSpdType.Text);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET10.XName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                            
            methodSpd.setColStyle(spd, -1, (int)enmXraySET10.XCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET10.Result, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET10.Result1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET10.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            methodSpd.setSpdFilter(spd, (int)enmXraySET10.XJong, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXraySET10.Chk, unary); //점검



        }

        #endregion

        #region //영상의학 유방촬영 기본 상용구 enum, 배열변수 - frmComSupXraySET12.cs

        #region //영상의학 유방촬영 기본 상용구 - Finding (1/2)
        public enum enmXraySET12A
        {
            chk,Code,Result1,Result2, ROWID

        }

        public string[] sSpdenmXraySET12A = { "선택","코드","결과1","결과2","ROWID" };


        public int[] nSpdenmXraySET12A = {30,70,550,100, 70 };


        public void sSpd_enmXraySET12A(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET12A)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정
            spd.ActiveSheet.RowHeader.Visible = false;
            spd.ActiveSheet.ColumnHeader.Visible = false;
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12A.chk, clsSpread.enmSpdType.CheckBox);
            sup.setColStyle_Text(spd, -1, (int)enmXraySET12A.Result1, true, true, false, 2000); //txt재정의
            sup.setColStyle_Text(spd, -1, (int)enmXraySET12A.Result2, true, true, false, 2000); //txt재정의



            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET12A.Result1, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET12A.Result2, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                                        
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12A.chk, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12A.Result2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12A.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXraySET12A.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXraySET12A.Chk, unary); //점검



        }

        #endregion

        #region //영상의학 유방촬영 기본 상용구 - Conclusion (2/2)
        public enum enmXraySET12B
        {
            chk,Code, Result1,Result2, ROWID
        }

        public string[] sSpdenmXraySET12B = { "선택","코드", "결과1","결과2", "ROWID" };


        public int[] nSpdenmXraySET12B = { 30,70, 550,100, 70 };


        public void sSpd_enmXraySET12B(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET12B)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정
            spd.ActiveSheet.RowHeader.Visible = false;
            spd.ActiveSheet.ColumnHeader.Visible = false;
            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12B.chk, clsSpread.enmSpdType.CheckBox);
            sup.setColStyle_Text(spd, -1, (int)enmXraySET12B.Result1, true, true, false, 2000); //txt재정의
            sup.setColStyle_Text(spd, -1, (int)enmXraySET12B.Result2, true, true, false, 2000); //txt재정의



            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET12A.Result1, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXraySET12A.Result2, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                                        
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12B.chk, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12B.Result2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET12B.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXraySET12B.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "점검", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXraySET12B.Chk, unary); //점검



        }

        #endregion

        #endregion

        #region //영상의학 조직검사 결과조회 관리 enum, 배열변수 - frmComSupXrayRSLT01.cs

        public enum enmXrayRSLT01
        {
            chk, ResultDate,Pano, SName,GbIO
            ,DeptCode,OrderDate,ExCode,ExName,Gubun
            ,Change, ROWID

        }

        public string[] sSpdenmXrayRSLT01 = { "선택","결과일자","등록번호","성명","구분"
                                             ,"과","오더일자","검사코드","검사명","구분"
                                             ,"변경", "ROWID" };


        public int[] nSpdenmXrayRSLT01 = { 25,80,80,80,50
                                          ,35,80,80,220,40
                                          ,30,80     };


        public void sSpd_enmXrayRSLT01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayRSLT01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayRSLT01.chk, clsSpread.enmSpdType.CheckBox);            
            //sup.setColStyle_Text(spd, -1, (int)enmXrayRSLT01.Result, true, true, false, 4000); //txt재정의            


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmXrayRSLT01.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                                        
            methodSpd.setColStyle(spd, -1, (int)enmXrayRSLT01.ExCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayRSLT01.Gubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayRSLT01.Change, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayRSLT01.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            methodSpd.setSpdFilter(spd, (int)enmXrayRSLT01.Pano, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayRSLT01.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayRSLT01.DeptCode, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            methodSpd.setSpdFilter(spd, (int)enmXrayRSLT01.Gubun, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "판독", false);
            ////unary.BackColor = method.cSpdCellImpact_Back;
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayRSLT01.DeptCode, unary); //점검



        }

        #endregion

        #region //영상의학 환자성명검색 enum, 배열변수 - frmComSupXrayHELP05.cs

        public enum enmXrayHELP05
        {
            chk,Pano,SName,Jumin,DeptCode
            ,DrCode,DrName,ROWID
        }

        public string[] sSpdenmXrayHELP05 = { "선택","등록번호","성명","주민번호","과"
                                             ,"의사","의사명", "ROWID" };


        public int[] nSpdenmXrayHELP05 = { 30,100,100,170,30
                                           ,50,80,80     };


        public void sSpd_enmXrayHELP05(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXrayHELP05)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            //spd.ActiveSheet.FrozenColumnCount = (int)enmXraySET08.SName + 1;//컬럼고정

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXrayHELP05.chk, clsSpread.enmSpdType.CheckBox);            
            //sup.setColStyle_Text(spd, -1, (int)enmXrayHELP05.Result, true, true, false, 4000); //txt재정의            


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmXrayHELP05.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                                                            
            methodSpd.setColStyle(spd, -1, (int)enmXrayHELP05.chk, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXrayHELP05.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXrayHELP05.ROWID, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmXrayHELP05.XName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            //6.특정문구 색상
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "판독", false);
            ////unary.BackColor = method.cSpdCellImpact_Back;
            //unary.BackColor = Color.LightGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmXrayHELP05.DeptCode, unary); //점검



        }

        #endregion

        #region //영상의학 조영제 바코드 출력 enum, 배열변수 - frmComSupXraySET20.cs

        public enum enmXraySET20
        {
            CHK, CODE, NAME
        }

        public string[] sSpdenmSupenmXraySET20 = { "선택","코드","조영제명" };

        public int[] nSpdenmSupenmXraySET20 = {35,90,200 };

        public void sSpd_enmSupXraySET20(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET20)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 25;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20.CHK, clsSpread.enmSpdType.CheckBox);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmXraySET20.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.RDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXraySET20.OrderCode, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXraySET20.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa01.Chk, unary); //결과



        }

        #endregion

        #region //영상의학 환자정보 enum, 배열변수 - frmComSupXraySET20.cs

        public enum enmXraySET20PI
        {
            Pano, SName, Sex, Age, BDATE, XCODE, XJONG, IPDOPD, SEEKDATE, ROWID
        }

        public string[] sSpdenmSupenmXraySET20PI = { "등록번호", "성명", "성별", "나이", "처방일", "검사코드", "종류", "IPDOPD", "촬영일자", "ROWID"                
                                                };

        public int[] nSpdenmSupenmXraySET20PI = {80,60,90,80,80,80,80,80,80,150
                                                 };

        public void sSpd_enmSupXraySET20PI(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmXraySET20PI)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 25;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmXraySET20PI.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.RDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.OrderCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.BDATE, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.XCODE, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.XJONG, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.IPDOPD, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmXraySET20PI.SEEKDATE, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa01.Chk, unary); //결과



        }

        #endregion



    }
}
