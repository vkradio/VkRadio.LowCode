using System.Net.Mail;

namespace VkRadio.Orm.Util;

public static class Validator
{
    public static bool IsEmailValid(string email)
    {
        // TODO: This method works not for all situations, and sometimes does not catch obviously invalid email values
        var coll = new MailAddressCollection();

        var isEmailValid = false;

        try
        {
            coll.Add(email);
            isEmailValid = true;
        }
        catch
        {
        }

        return isEmailValid;
    }
}
