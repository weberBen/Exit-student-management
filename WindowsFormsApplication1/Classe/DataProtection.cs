using System;
using System.Security.Cryptography;
using System.Text;
using System.Security;
using System.IO;
using System.Collections.Generic;
using ToolsClass;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

//from https://msdn.microsoft.com/en-us/library/system.security.cryptography.protecteddata(v=vs.110).aspx
//from https://weblogs.asp.net/jongalloway/encrypting-passwords-in-a-net-app-config-file



public class DataProtection
{
    // Create byte array for additional entropy when using Protect method.
    private const string entropy_key = "8JViJDT`URK'Dk-Oj<7=6f4^ct#5,.Wf*z|i7Wv6[UoNfyMlE#+vQHOLx[)V',{9pb;hASp}#Qz=#2>gSm`&[vX%*dJY]D}-#9RlQ@n0NVZhfS.,JqLF1?MB>kR<SK";
    private static byte[] s_aditionalEntropy = Encoding.Unicode.GetBytes(entropy_key);
    private static string PATH_FOR_SECURE_LOG = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar+"secure_log.txt";
    private const string XML_ATTRIBUTE_SECURE_DATA_CONNEXION_WEBSITE = "Data_connexion_website";
    private const string XML_ATTRIBUTE_VALUE_ID_WEBSITE = "Id";
    private const string XML_ATTRIBUTE_VALUE_PASSWORD_WEBSITE = "Password";
    private static Encoding encoding = Encoding.ASCII;


    public static string protect(SecureString s1)
    {
        if (s1 == null)
        {
            return null;
        }

        IntPtr bstr1 = IntPtr.Zero;
        RuntimeHelpers.PrepareConstrainedRegions();
        byte[] data = new byte[] { };
        string insecure_s1 = "";

        try
        {
            bstr1 = Marshal.SecureStringToBSTR(s1);//create secure space in memory

            unsafe
            {
                for (Char* ptr1 = (Char*)bstr1.ToPointer();
                    *ptr1 != 0;
                     ++ptr1)
                {
                    insecure_s1 += *ptr1;
                }
            }

            data = Encoding.Unicode.GetBytes(insecure_s1);
            // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
            //  only by the same current user.
            byte[] encryptedData = ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            string SencryptedData = Convert.ToBase64String(encryptedData);

            Array.Clear(encryptedData, 0, encryptedData.Length);
            Array.Clear(data, 0, data.Length);

            return SencryptedData;

        }
        catch
        {
            return null;
        }
        finally
        {
            if (bstr1 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr1);//set all the secure space to a standard value
            }

            s1.Dispose();

            Array.Clear(data, 0, data.Length);
        }

    }


    public static SecureString unprotect(string Sdata)
    {
        if (string.IsNullOrEmpty(Sdata))
        {
            return null;
        }

        byte[] data = Convert.FromBase64String(Sdata);

        try
        {
            //Decrypt the data using DataProtectionScope.CurrentUser.
            byte[] decryptedData = ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            char[] decyptedChar = Encoding.Unicode.GetChars(decryptedData);

            SecureString SdecryptedData = new SecureString();
            foreach (char c in decyptedChar)
            {
                SdecryptedData.AppendChar(c);
            }
            SdecryptedData.MakeReadOnly();

            Array.Clear(decryptedData, 0, decryptedData.Length);
            Array.Clear(decyptedChar, 0, decyptedChar.Length);
            Array.Clear(data, 0, data.Length);

            return SdecryptedData;
        }
        catch (CryptographicException)
        {
            Array.Clear(data, 0, data.Length);

            return new SecureString();
        }
    }


    public static string getSaltForHashes()
    {
        string res = "";
        using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
        {
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);

            res = Convert.ToBase64String(tokenData);
        }

        return res;
    }

    public unsafe static string getSaltHashedString(SecureString secure_string, string salt)
    {

        string output;
        byte[] salt_bytes = encoding.GetBytes(salt);

        int maxLength = encoding.GetMaxByteCount(secure_string.Length);
        int saltLength = salt_bytes.Length;

        IntPtr bytes = IntPtr.Zero;
        IntPtr str = IntPtr.Zero;
        byte[] output_bytes = null;

        try
        {
            bytes = Marshal.AllocHGlobal(maxLength);
            str = Marshal.SecureStringToBSTR(secure_string);

            char* chars = (char*)str.ToPointer();
            byte* bptr = (byte*)bytes.ToPointer();
            int len = encoding.GetBytes(chars, secure_string.Length, bptr, maxLength);

            output_bytes = new byte[len + saltLength ];
            for (int i = 0; i < len; ++i)
            {
                output_bytes[i] = *bptr;
                bptr++;
            }
            //concatenate salt array
            for (int i = 0; i < saltLength; ++i)
            {
                output_bytes[i + len] = salt_bytes[i];
            }

            SHA512CryptoServiceProvider sha512 = new SHA512CryptoServiceProvider();
            output = encoding.GetString(sha512.ComputeHash(output_bytes));
        }
        finally
        {
            if (bytes != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(bytes);
            }
            if (str != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(str);
            }

        }


        return output;
        
    }


    public static string ToInsecureString(SecureString s1)
    {
        if (s1 == null)
        {
            return null;

        }


        IntPtr bstr1 = IntPtr.Zero;
        RuntimeHelpers.PrepareConstrainedRegions();
        byte[] data = { };
        string insecure_s1 = "";

        try
        {
            bstr1 = Marshal.SecureStringToBSTR(s1);//create secure space in memory

            unsafe
            {
                for (Char* ptr1 = (Char*)bstr1.ToPointer();
                    *ptr1 != 0;
                     ++ptr1)
                {
                    insecure_s1 += *ptr1;
                }
            }

            return insecure_s1;

        }
        catch
        {
            return null;
        }
        finally
        {
            /* the following insctruction set emty the given securestring (and not a copy of that securestring)
             * So in order to keep the secure string after the function we cannot make the array empty
            if (bstr1 != IntPtr.Zero)
            {
                Marshal.ZeroFreeBSTR(bstr1);//set all the secure space to a standard value
            }

            s1.Dispose();*/
        }
    }





    public static void saveSecureDataConnexionWebsite(SecureString user_id, SecureString user_pass )
    {
        /* 
         * Write into the dedicated file, by overwritting it, the user id and password for the login web page 
        */

        List<string> attribute_element_list = new List<string>(new string[] {XML_ATTRIBUTE_VALUE_ID_WEBSITE,XML_ATTRIBUTE_VALUE_PASSWORD_WEBSITE});
        List<string> element = new List<string>(new string[] { protect(user_id), protect(user_pass) });

        try
        {
            XmlHelper.changeXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                           XML_ATTRIBUTE_SECURE_DATA_CONNEXION_WEBSITE, Definition.XML_TAG_FOR_ITEM,
                                           element, Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);
        }catch(Exception e)
        {
            Error.details = "" + e;
            Error.error = "CSDCWTF";
        }
    }



    public static Tuple<SecureString,SecureString> readSecureDataConnexionWebsite()
    {
        /* 
         * Read the dedicated file to have the encrypted value of user id and password for login web page 
         * Return a tuple made by <decrypted user id, decrypted user_ password>
        */

        SecureString user_id = null;
        SecureString user_pass = null;

        List<string> attribute_element_list = new List<string>(new string[] { XML_ATTRIBUTE_VALUE_ID_WEBSITE, XML_ATTRIBUTE_VALUE_PASSWORD_WEBSITE });
        List<string> element = new List<string>(new string[] { protect(user_id), protect(user_pass) });

        List<string> res= XmlHelper.readXmlListElement(Definition.PATH_TO_TOOLS_FILE, Definition.SECTION_TAG_XML_FILE, Definition.ATTRIBUTE_ELEMENT_XML_FILE,
                                       XML_ATTRIBUTE_SECURE_DATA_CONNEXION_WEBSITE, Definition.XML_TAG_FOR_ITEM, 
                                       Definition.ATTRIBUTE_ELEMENT_XML_FILE, attribute_element_list);


        try
        {
            user_id = unprotect(res[0]);
            user_pass = unprotect(res[1]);

        }catch(Exception e)
        {
            Error.details = "" + e;
            Error.error = "CRDCTWFF";
        }



        return Tuple.Create(user_id, user_pass);
    }


}