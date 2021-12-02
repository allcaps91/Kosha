/// <summary>
/// 환자정보
/// </summary>
namespace ComBase
{
    /// <summary>
    /// 환자정보
    /// </summary>
    public class clsPat
    {

        public struct Pat_Info
        {
            public double WRTNO;
            public double IPDNO;
            public string Pano;
            public string sName;
            public string Sex;
            public string Age;
            public string InDate;
            public string DeptCode;
            public string DrCode;
            public string WardCode;
            public string RoomCode;
            public string RDate;
            public string DIAGNOSIS;
            public string BDate;
            public string DRSABUN;
            public string NRSABUN;
            public string PMSABUN;
            public string DTSABUN;
            public string OrderNo;
            public string ROWID;
        }
        public static Pat_Info PATi;

        #region //사용안함
        //        IPDNO As Long
        //PtNo                    As String * 8
        //        sName As String* 10
        //        Age As Integer
        //Sex                     As String * 1
        //        GbSpc As String* 1
        //        DeptCode As String* 2
        //        DrCode As String* 6
        //        Staffid As String
        //Bi                      As String * 2
        //        JTime As String* 5
        //        GbChojae As String* 1
        //        RDATE As String* 8
        //        RTime As String* 5
        //        RDrCode As String* 6
        //        Remark1 As String       '골수검사 임상소견
        //        Remark2 As String       '골수검사 주요병력
        //        Remark3 As String       '내시경
        //        Remark4 As String       'Biopsy Or Cytology
        //        Tel As String
        //GbSheet                 As String
        //        VCode As String       'Y.암등록환자/N.암미등록환자
        //        Exam As String       'Y.검사결과확인예약
        //        WardCode As String* 4
        //        RoomCode As String* 6
        //        DrName As String* 10
        //        PNEUMONIA As String   '페렴
        //        Pregnant As String   '임신여부
        //        MCODE As String   '희귀난치성등록자구분
        //        Thyroid As String   '갑상선여부
        //        res As String   '내시경 예약
        //        Insulin As String   '인슐린투여환자
        //        MCODE_OPD As String   '차상위 희귀
        //        VCODE_OPD As String
        //JUMIN                   As String   '주민번호
        //        bunup As String   '분업 2012-05-29
        //        Birth As String   '생일 2013-02-06
        //        INDATE As String   '2013-08-01
        //        ResMemo As String   '2014-08-07
        //        ResSMSNot As String   '2016-06-16(예약문자 형성 안함)
        #endregion
    }
}
