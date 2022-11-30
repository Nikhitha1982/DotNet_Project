using System;

namespace validation{
    static class Validation{
        public static Boolean ValidatePhoneNumber(string phoneNumber){
            return phoneNumber.All(char.IsDigit);
        }
    } 
}