using HC.Core.Dto;
using HC.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Core
{
    public class SignPadBound
    {
        public SignPadForm SignPadForm { get; set; }
        public bool IsUserSign {get;set;}

        public SignPadBound(SignPadForm signPadForm, bool isUserSIgn)
        {
            this.SignPadForm = signPadForm;
            this.IsUserSign = isUserSIgn;
        }
        public void Ready()
        {
            if (SignPadForm != null)
            {
                if (IsUserSign)
                {
                    SignPadForm.ShowSaveButton();
                }
            }
        }
        public void Save(string base64)
        {
            SignPadForm.Save(base64);
        }
        public void SaveSiteUser(string base64)
        {
            SignPadForm.Save(base64);
        }
    }
}
