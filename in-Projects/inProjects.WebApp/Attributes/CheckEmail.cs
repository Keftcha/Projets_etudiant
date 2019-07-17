using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace inProjects.WebApp.Attributes
{
    public class CheckEmail : ValidationAttribute
    {
        public override bool IsValid( object value )
        {
            string email = (string)value;

            string regexPattern = @"\A[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,59}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,59}[a-zA-Z0-9])?)*\.[a-zA-Z0-9]{2,61}\z";
            Regex regex = new Regex( regexPattern, RegexOptions.Compiled );

            if( email.Length <= 320 && regex.IsMatch( email ) )
                return true;
            else
                return false;
        }
    }
}
