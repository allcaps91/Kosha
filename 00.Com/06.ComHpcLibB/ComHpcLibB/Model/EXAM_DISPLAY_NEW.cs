
namespace ComHpcLibB.Model
{
    using ComBase.Mvc;
    using System;

    public class EXAM_DISPLAY_NEW : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long GWRTNO { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GROUPCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string EXCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PART { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RESCODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PANJENG { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RESULT { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SANGDAM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ACTIVE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ACTPART { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long ENTSABUN { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ENTTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? READTIME { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CODE { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HNAME      { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ENAME      { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string YNAME      { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UNIT       { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public string SUCODE     { get; set; }
        public string GCODE      { get; set; }
        public string TONGBUN    { get; set; }
        public string BRESULT    { get; set; }
        public string RESULTTYPE { get; set; }
        public string GBCODEUSE  { get; set; }
        public string GBAUTOSEND { get; set; }
        public string MIN_M      { get; set; }
        public string MIN_MB     { get; set; }
        public string MIN_MR     { get; set; }
        public string MAX_M      { get; set; }
        public string MAX_MB     { get; set; }
        public string MAX_MR     { get; set; }
        public string MIN_F      { get; set; }
        public string MIN_FB     { get; set; }
        public string MIN_FR     { get; set; }
        public string MAX_F      { get; set; }
        public string MAX_FB     { get; set; }
        public string MAX_FR     { get; set; }
        public long AMT1         { get; set; }
        public long AMT2         { get; set; }
        public long AMT3         { get; set; }
        public long AMT4         { get; set; }
        public long AMT5         { get; set; }
        public DateTime? SUDATE  { get; set; }
        public long OLDAMT1      { get; set; }
        public long OLDAMT2      { get; set; }
        public long OLDAMT3      { get; set; }
        public long OLDAMT4      { get; set; }
        public long OLDAMT5      { get; set; }
        public DateTime? DELDATE { get; set; }
        public string RESBOHUM1  { get; set; }
        public string RESBOHUM2  { get; set; }
        public string RESCANCER  { get; set; }
        public string GBGAM      { get; set; }
        public string GBRESULT   { get; set; }
        public long AMT6         { get; set; }
        public long OLDAMT6      { get; set; }
        public string HEASORT    { get; set; }
        public string ENTPART    { get; set; }
        public string GBRESEMPTY { get; set; }
        public string GBPANBUN1  { get; set; }
        public string GBPANBUN2  { get; set; }
        public string XRAYCODE   { get; set; }
        public string BUCODE     { get; set; }
        public long SORTA        { get; set; }
        public string PANDISP    { get; set; }
        public long EXSORT       { get; set; }
        public string ACTTIME    { get; set; }
        public string HEAPART    { get; set; }
        public string HAROOM     { get; set; }
        public string ENDOGUBUN1 { get; set; }
        public string ENDOGUBUN2 { get; set; }
        public string ENDOGUBUN3 { get; set; }
        public string ENDOGUBUN4 { get; set; }
        public string ENDOGUBUN5 { get; set; }
        public string ENDOSCOPE  { get; set; }
        public string XNAME      { get; set; }
        public string XREMARK    { get; set; }
        public string XJONG      { get; set; }
        public string XSUBCODE   { get; set; }
        public string XORDERCODE { get; set; }
        public string GBSANGDAM  { get; set; }
        public string HEA_PRTGBN { get; set; }
        public string GOTO       { get; set; }
        public string EXAMBUN    { get; set; }
        public string EXAMFRTO   { get; set; }
        public string GBSPCEXAM  { get; set; }
        public string SENDBUSE1  { get; set; }
        public string SENDBUSE2  { get; set; }
        public string GBEKGSEND  { get; set; }
        public string GBSUGA     { get; set; }
        public string GBULINE { get; set; }

        public long GAMT1 { get; set; }
        public long SAMT1 { get; set; }
        public long JAMT1 { get; set; }
        public long IAMT1 { get; set; }
        public long GAMT2 { get; set; }
        public long SAMT2 { get; set; }
        public long JAMT2 { get; set; }
        public long IAMT2 { get; set; }
        public DateTime? SUDATE2 { get; set; }
        public long GAMT3 { get; set; }
        public long SAMT3 { get; set; }
        public long JAMT3 { get; set; }
        public long IAMT3 { get; set; }
        public DateTime? SUDATE3 { get; set; }
        public long GAMT4 { get; set; }
        public long SAMT4 { get; set; }
        public long JAMT4 { get; set; }
        public long IAMT4 { get; set; }
        public DateTime? SUDATE4 { get; set; }
        public long GAMT5 { get; set; }
        public long SAMT5 { get; set; }
        public long JAMT5 { get; set; }
        public long IAMT5 { get; set; }
        public DateTime? SUDATE5 { get; set; }

        public EXAM_DISPLAY_NEW()
        {
        }
    }
}
