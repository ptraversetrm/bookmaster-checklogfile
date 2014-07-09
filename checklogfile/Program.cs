using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace checklogfile
{
    class Program
    {
        static void Main(string[] args)
        {
            // Determining path based on the company name
            string[] path_array = new string[] { "TRM", "NCH" };
            foreach (string path in path_array)
            {
                string path_string = @"C:\uploads\" + path + @"\OrderProc\logs\V33_OrderProc.log";
                //Console.WriteLine("PATH"+path_string);

                // read log file
                try
                {
                    using (StreamReader sr = new StreamReader(path_string))
                    {
                        String line = sr.ReadToEnd();
                        //Console.WriteLine("LINE:"+line);

                        // looking for the word 'exception', 'Exception' ,  to determine any error
                        string[] error = new string[] { "Exception", "exception", "EXCEPTION" };

                        bool value = false;

                        foreach (string single_error in error)
                        {
                            value = line.Contains(single_error);

                            if (value == true)
                            {
                                break;
                            }
                        }

                        if (value == true)
                        {
                             //Console.WriteLine("Error in the Log File");

                            //making another curl call to get the file contents 
                            string file_url = "http://corporate.discoverbooks.com/interfaces/admin/reporting/settlements_trigger_sap_error.php?partner_id=2&signature=D1sc0vEr&log_file=" + path_string;

                            HttpWebRequest req;

                            NetworkCredential myCred = new NetworkCredential("api_user", "thedevteamlikestocurl");

                            req = (HttpWebRequest)WebRequest.Create(file_url);
                            req.PreAuthenticate = true;
                            req.Credentials = myCred;
                            req.Method = "GET";
                            req.ContentType = "application/x-www-form-urlencoded";

                            HttpWebResponse file_response = (HttpWebResponse)req.GetResponse();

                            //Console.WriteLine("Response" + file_response);
                            int status_code = (int)file_response.StatusCode;

                            // if successfull
                            if (status_code == 200)
                            {
                                // Console.WriteLine("Error Email Sent to look more into Settlements");
                            }
                            else
                            {
                                 //Console.WriteLine("No Response from Server");
                            }
                        }
                        else
                        {
                              //Console.WriteLine("No Error in Log File");
                        }

                    }
                }
                catch (Exception e)
                {
                    // Console.WriteLine("The file could not be read:");
                    // Console.WriteLine(e.Message);
                }
            }
            // Keep the console window open in debug mode.         
             //Console.ReadLine();
        }
    }
}
