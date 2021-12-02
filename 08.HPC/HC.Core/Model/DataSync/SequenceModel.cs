namespace HC_Core.Model
{
    using ComBase.Mvc;
    using ComBase.Mvc.Validation;
    using System;
    using System.Drawing;


    public class SequenceModel : BaseDto
    {
        public string sequence_sql { get; set; }
        public string sequence_name { get; set; }

    }
}
