using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using System.Diagnostics;

namespace GumTree
{
    public partial class Form1 : Form
    {
        public string category_path = "Category.xml";
        public string productlist_path = "Ads.xml";
        public string postcode_path = "PostalCodes.txt";
        public int pg_one_post = 0;
        public int relogtime = 0;
        public bool viewflag = false;
        public bool loginflag = false;
        public log logForm = new log();
        Thread postall;
        public string gaps = "";
        public string access_token_string = "";
        public string google_account_name = "";
        List<string> cookieName = new List<string>();
        List<string> cookieValue = new List<string>();
        List<string> initCookieName = new List<string>();
        List<string> initCookieValue = new List<string>();
        List<string> postcodeArr = new List<string>();
        List<ProductList> PlistArray = new List<ProductList>();
        List<ProductList> ChangePlistArray = new List<ProductList>();
        List<ProductList> PostedlistArray = new List<ProductList>();
        ProductList selectedList = new ProductList();
        ProductList selectedPostedAd = new ProductList();

        private string client_id = "";
        private string jsh = "";
        private string locationurl = "";
        private string GALX = "";
        private string gxf = "";
        private string continu = "";
        private string state_wrapper = "";
        private string xsrfsign = "";
        private Point mousePoint;
        public Form1()
        {
           
            InitializeComponent();
            
        }
        public void AnalyseProductListXml()
        {
            try
            {
                XmlTextReader xmlTextReader = new XmlTextReader(productlist_path);
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load((XmlReader)xmlTextReader);
                foreach (XmlElement xmlElement in xmlDocument.SelectNodes("//Ad"))
                {
                    ProductList list = new ProductList();
                    list.Id = xmlElement.ChildNodes[0].InnerText;
                    list.Type = xmlElement.ChildNodes[1].InnerText;
                    list.Category = xmlElement.ChildNodes[2].InnerText;
  //                  list.postcode = xmlElement.ChildNodes[3].InnerText;
                    list.Title = xmlElement.ChildNodes[3].InnerText;
                    list.Description = xmlElement.ChildNodes[4].InnerText;
                    list.Images = xmlElement.ChildNodes[5].InnerText;
                    list.Price = xmlElement.ChildNodes[6].InnerText;
                    PlistArray.Add(list);
                    list_product.Items.Add(list.Title);
                }
                xmlTextReader.Close();
                FileInfo fi = new FileInfo(postcode_path);
                if (!fi.Exists)
                {
                    MessageBox.Show("PostCodes are in \"PostalCodes.txt\" file.\nBut this file don't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    this.Dispose();
                    logForm.Dispose();
                    return;
                }
                string[] textValue = System.IO.File.ReadAllLines(postcode_path);
                for (int i = 0; i < textValue.Length; i++)
                {
                    postcodeArr.Add(textValue[i]);
                }

            }
            catch
            {
                MessageBox.Show("Ads are in \"Ads.xml\" file.\nBut this file format is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                this.Dispose();
                logForm.Dispose();
                return;
            }
           
            
        }

        private void list_product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list_product.SelectedIndex != -1)
            {
                
                string selecttitle = list_product.SelectedItem.ToString();
                for (int i = 0; i < PlistArray.Count; i++)
                {
                    if (PlistArray[i].Title.Equals(selecttitle))
                    {
                        selectedList.Id = PlistArray[i].Id;
                        selectedList.Title = PlistArray[i].Title;
                        selectedList.Category = PlistArray[i].Category;
                        selectedList.Description = PlistArray[i].Description;
                        selectedList.Images = PlistArray[i].Images;
                        selectedList.Type = PlistArray[i].Type;
                        selectedList.Price = PlistArray[i].Price;
//                        selectedList.postcode = PlistArray[i].postcode;
                    }
                }
                string[] photoarr = selectedList.Images.Split('|');
                list_photo.Items.Clear();
                for (int i = 0; i < photoarr.Length; i++)
                {
                    if (!photoarr[i].Equals(""))
                    {
                        list_photo.Items.Add(photoarr[i]);
                    }
                }
                list_posted.ClearSelected();
                tb_title.Text = selectedList.Title;
                tb_category.Text = selectedList.Category;
                c.Text = selectedList.Price;
                tb_description.Text = selectedList.Description.Replace(@"\r\n","\r\n");
 //               tb_relog.Text = selectedList.postcode;
            }
            else
            {
                selectedList.clear();
            }
            
        }

        private void bt_clean_Click(object sender, EventArgs e)
        {

            clean();
            //list_product.SelectedIndex = -1;
        }
        public void clean()
        {
            list_photo.Items.Clear();
            tb_title.Text = "";
            tb_category.Text = "";
            c.Text = "";
            tb_description.Text = "";
            tb_relog.Text = "";
            selectedList.clear();
            list_product.ClearSelected();
        }
        public bool login()
        {
            int relognum = 0;
            while (true)
            {
                relognum++;
                if (relognum > 3)
                {
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value = 0
                    ));
                if (!loginflag)
                {
                    string username = "";
                    string passwd = "";
                    tb_username.Invoke(new Action(
                        () => username = tb_username.Text.Split('@')[0] + "@gmail.com"
                        ));
                    tb_passwd.Invoke(new Action(
                        () => passwd = tb_passwd.Text
                        ));
                    if (username.Equals("") | passwd.Equals(""))
                    {
                        continue;
                    }
                    lb_stat.Invoke(new Action(
                        () => lb_stat.Text = "Loging to Gumtree....."
                        )
                        );
                    if (!initLogin())
                    {
                        continue;
                    }
                    pg_bar.Invoke(new Action(
                        () => pg_bar.Value += pg_bar.Maximum / 6
                        ));
                    string profileinformation = login1();
                    if (!profileinformation.Equals(""))
                    {
                        pg_bar.Invoke(new Action(
                        () => pg_bar.Value += pg_bar.Maximum / 6
                        ));
                        string content = login2(profileinformation);

                        if (!content.Equals(""))
                        {
                            pg_bar.Invoke(new Action(
                            () => pg_bar.Value += pg_bar.Maximum / 6
                            ));
                            if (!getGoogleAccount())
                            {
                                pg_bar.Invoke(new Action(
                                () => pg_bar.Value += pg_bar.Maximum / 6
                                ));
                                continue;
                            }
                            if (login3(content))
                            {
                                pg_bar.Invoke(new Action(
                                () => pg_bar.Value += pg_bar.Maximum / 6
                                ));
                                if (GumLogin())
                                {
                                    pg_bar.Invoke(new Action(
                                     () => pg_bar.Value = pg_bar.Maximum
                                     ));
                                    lb_stat.Invoke(new Action(
                                        () => lb_stat.Text = "Login Success."
                                        )
                                        );
                                    loginflag = true;
                                    return true;
                                }
                                continue;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
               
            }
        }
        
        public string Getaccesstocken(string refer, string url)
        {
            Uri uri = new Uri("https://accounts.google.com/o/oauth2/approval");
            HttpClientHandler handler = new HttpClientHandler();
            for (int i = 0; i < cookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(cookieName[i], cookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.109 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/x-www-form-urlencoded");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", refer);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://accounts.google.com");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");

            Dictionary<string, string> dicParms = new Dictionary<string, string>();
            dicParms.Add("bgresponse", "!xsWlxeRC3PgB42YbPUBE1vPGMEh3wFMCAAAAWVIAAAAImQFw0H6LJHIWJ8fwIxs7zNUyXAZFvMZPvXEE4u3LLyek5Fvf0Oqx2ngXg7POWVPX2YOAl6VJlGnjhb5FfCLJC0jgQPPv5liHNxCUxvuN_P5gT_7GFa2VWvky_pPCkoEJCkR8RJ6qtZLM_GMXr1NBO6dRpI_Sc5evmZAm9kCeF6PN8yEfJmaho655H8eqZMuSYQ40y7nY9VPZi2CLj8HxeV5SL065GiKQcYmB7wYV-dNI6VejBxQ_PlRQmuyRP3x_Avkp-3DRsjmStKn2t3cHHHhWIloIr0Grfahf6mVeHW6FFiTT3LLEP9nH0A-ysXXq3Y_5EW2_sH41QbzlFMWNmsZEYShtzzx5HnF_zpj5KS5A_cv1hwel54ZMiVVV682VM4HQ_t5Yds7AHBobkZaC8UY_jP-6nHdGHiw6qP6WBwzNT1G717PWEGVcuCTNCoaE1txqPjXJ58pr96bpKK1KFMy7H9SOltmXRZi8W7v_F9nQkS8");
            dicParms.Add("state_wrapper", state_wrapper);
            dicParms.Add("_utf8", "☃");
            dicParms.Add("submit_access", "true");

            KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
            for (int i = 0; i < dicParms.Keys.Count; i++)
            {
                keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
            }
            var result = httpClient.PostAsync(url.Replace("&amp;", "&"), new FormUrlEncodedContent(keyPairs)).Result;
            result.EnsureSuccessStatusCode();
            string strContent = result.Content.ReadAsStringAsync().Result;
            return strContent;
        }
        public string login1()
        {
            try
            {
                Uri ur1 = new Uri("https://accounts.google.com/accountLoginInfoXhr");
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                for (int i = 0; i < cookieName.Count; i++)
                {
                    handler.CookieContainer.Add(ur1, new Cookie(cookieName[i], cookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", locationurl);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Client-Data", "CI22yQEIpbbJAQjEtskBCP2VygEIwpjKAQjynMoB");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://accounts.google.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");
                Dictionary<string, string> dicParms = new Dictionary<string, string>();
                tb_username.Invoke(new Action(
                    () => dicParms.Add("Email", tb_username.Text.Split('@')[0] +"@gmail.com")
                    ));
                
                dicParms.Add("requestlocation", locationurl + "#identifier");
                dicParms.Add("bgresponse", "!gIOlg6JC3PgB42YbPUBE1vPGMEh3wFMCAAAAWVIAAAA6mQFw0H6LJHIWJ8fwIxs7zNUyXAZFvMZPvXEE4u3LLyek5Fvf0Oqx2ngXg7POWVPX2YOAl6VJlGnjhb5FfCLJC0jgQPPv5liHNxCUxvuN_P5gT_7GFa2VWvky_pPCkoEJCkR8RJ6qtZLM_GMXr1NBO6dRpI_Sc5evmZAm9kCeF6PN8yEfJmaho655H8eqZMuSYQ40y7nY9VPZi2CLj8HxeV5SL065GiKQcYmB7wYV-dNI6VejBxQ_PlRQmuyRP3x_Avkp-3DRsjmStKn2t3cHHHhWIloIr0Grfahf6mVeHW6FFiTT3LLEP9nH0A-ysXXq3Y_5EW2_sH41QbzlFMWNmsZEYShtzzx5HnF_zpj5KS5D_cn1hwegOyXWoNTK8uBb0qnQvAjuYE8gQf83C3xX5wmmIojhWEuAWASP_-3PMKod4ubp82iKEquLX-dS0hmYI5C896bcrY28w5_n6JiIjvdId32XPMT9LMo40YEgMBZ0gLo");
                dicParms.Add("Page", "PasswordSeparationSignIn");
                dicParms.Add("GALX", GALX);
                dicParms.Add("gxf", gxf);
                dicParms.Add("continue", continu); 
                dicParms.Add("scc", "1");
                dicParms.Add("sarp", "1");
                dicParms.Add("oauth", "1");
                dicParms.Add("pstMsg", "1");
                dicParms.Add("checkConnection", "youtube:2293:1");
                dicParms.Add("checkedDomains", "youtube");
                dicParms.Add("rmShown", "1");
                KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
                for (int i = 0; i < dicParms.Keys.Count; i++)
                {
                    keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
                }
                var result = httpClient.PostAsync("https://accounts.google.com/accountLoginInfoXhr", new FormUrlEncodedContent(keyPairs)).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;

                cookieName.Clear();
                cookieValue.Clear();
                gaps = Regex.Match(result.Headers.ToString(), @"GAPS=(?<VAL>[^\;]*)").Groups["VAL"].Value;
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(ur1).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    cookieName.Add(cookie.Name);
                    cookieValue.Add(cookie.Value);
                }

                string pattern = "\"encoded_profile_information\":\"(?<VAL>[^\"]*)";
                Match match = Regex.Match(strContent, pattern);
                if (match.Success)
                    return match.Groups["VAL"].Value;
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public string login2(string profileinformation)
        {
            try
            {

                Uri uri = new Uri("https://accounts.google.com/signin/challenge/sl/password");
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("GAPS", gaps));
                handler.CookieContainer.Add(uri, new Cookie("GALX", GALX));

                //                handler.CookieContainer.Add(uri, new Cookie("NID", "81=bv2egIG5Y5Z_6qQcJi3kNDsVGRyGfTIPQrhW-istiMOyTslMkdAyMLELMbl8_RgRiQhKeh_yIqDEhLLrVICcbd50rK-RDPv-XfFgFRV4EW_5zPpeUCFTax6593PfMALd; GALX=OQikheU25PM; GAPS=1:hOW33_AfUIO7EMNVgYQSMs5pbXxPWQ:E9iGI_LD9APGLp5-")); 
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", locationurl);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Client-Data", "CI22yQEIpbbJAQjEtskBCP2VygEIwpjKAQjynMoB");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://accounts.google.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");
                Dictionary<string, string> dicParms = new Dictionary<string, string>();
                tb_username.Invoke(new Action(
                    () => dicParms.Add("Email", tb_username.Text.Split('@')[0] + "@gmail.com")
                    ));
                tb_passwd.Invoke(new Action(
                    () => dicParms.Add("Passwd", tb_passwd.Text)
                    ));

                dicParms.Add("bgresponse", "!xsWlxeRC3PgB42YbPUBE1vPGMEh3wFMCAAAAWVIAAAAImQFw0H6LJHIWJ8fwIxs7zNUyXAZFvMZPvXEE4u3LLyek5Fvf0Oqx2ngXg7POWVPX2YOAl6VJlGnjhb5FfCLJC0jgQPPv5liHNxCUxvuN_P5gT_7GFa2VWvky_pPCkoEJCkR8RJ6qtZLM_GMXr1NBO6dRpI_Sc5evmZAm9kCeF6PN8yEfJmaho655H8eqZMuSYQ40y7nY9VPZi2CLj8HxeV5SL065GiKQcYmB7wYV-dNI6VejBxQ_PlRQmuyRP3x_Avkp-3DRsjmStKn2t3cHHHhWIloIr0Grfahf6mVeHW6FFiTT3LLEP9nH0A-ysXXq3Y_5EW2_sH41QbzlFMWNmsZEYShtzzx5HnF_zpj5KS5A_cv1hwel54ZMiVVV682VM4HQ_t5Yds7AHBobkZaC8UY_jP-6nHdGHiw6qP6WBwzNT1G717PWEGVcuCTNCoaE1txqPjXJ58pr96bpKK1KFMy7H9SOltmXRZi8W7v_F9nQkS8");
                dicParms.Add("Page", "PasswordSeparationSignIn");
                dicParms.Add("GALX", GALX);
                dicParms.Add("gxf", gxf);
                dicParms.Add("continue", continu);
                dicParms.Add("ProfileInformation", profileinformation);
                dicParms.Add("scc", "1");
                dicParms.Add("sarp", "1");
                dicParms.Add("oauth", "1");
                dicParms.Add("pstMsg", "1");
                dicParms.Add("checkConnection", "youtube:1499:1");
                dicParms.Add("checkedDomains", "youtube");
                dicParms.Add("rmShown", "1");
                dicParms.Add("PersistentCookie", "yes");
                KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
                for (int i = 0; i < dicParms.Keys.Count; i++)
                {
                    keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
                }
                var result = httpClient.PostAsync("https://accounts.google.com/signin/challenge/sl/password", new FormUrlEncodedContent(keyPairs)).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;

                cookieValue.Clear();
                cookieName.Clear();

                string pattern = @"Set-Cookie:(?<VAL>[^\n]*)";
                Match m = Regex.Match(result.Headers.ToString(), pattern);

                string s = ";";
                string[] coo = m.Groups["VAL"].Value.Split(s.ToCharArray());
                for (int i = 0; i < coo.Length; i++)
                {
                    if (coo[i].Contains(","))
                    {
                        string s1 = ",";
                        string[] cl1 = coo[i].Split(s1.ToCharArray());
                        foreach (string c in cl1)
                        {
                            if (c == null || c == "")
                                continue;
                            string s2 = "=";
                            string[] cl2 = c.Split(s2.ToCharArray());
                            if (cl2.Length == 1) continue;
                            string cookieKey = cl2[0].Trim();
                            string cookieVal = cl2[1].Trim();
                            if (cookieKey.StartsWith("Domain") ||
                                   cookieKey.StartsWith("Priority") ||
                                   cookieKey.StartsWith("Expires") ||
                                   cookieKey.StartsWith("Path"))
                                continue;
                            cookieName.Add(cookieKey);
                            cookieValue.Add(cookieVal);
                        }
                    }
                    else
                    {
                        string s2 = "=";
                        string[] cl2 = coo[i].Split(s2.ToCharArray());
                        if (cl2.Length == 1) continue;
                        string cookieKey = cl2[0].Trim();
                        string cookieVal = cl2[1].Trim();
                        if (cookieKey.StartsWith("Domain") ||
                               cookieKey.StartsWith("Priority") ||
                               cookieKey.StartsWith("Expires") ||
                               cookieKey.StartsWith("Path"))
                            continue;
                        cookieName.Add(cookieKey);
                        cookieValue.Add(cookieVal);
                    }
                }
                return strContent;
            }
            catch
            {
                return "";
            }
        }
        public bool login3(string content)
        {
            try
            {
                string proxypat = @"proxy=(?<VAL>[^\&]*)&";
                Match proxy = Regex.Match(content, proxypat);
                string scopepat = @"scope=(?<VAL>[^\&]*)&";
                Match scope = Regex.Match(content, scopepat);
                string originpat = @"origin=(?<VAL>[^\&]*)&";
                Match origin = Regex.Match(content, originpat);
                string response_typepat = @"response_type=(?<VAL>[^\&]*)&";
                Match response_type = Regex.Match(content, response_typepat);
                string redirect_uripat = @"redirect_uri=(?<VAL>[^\&]*)&";
                Match redirect_uri = Regex.Match(content, redirect_uripat);
                string statepat = @"state=(?<VAL>[^\&]*)&";
                Match state = Regex.Match(content, statepat);
                string client_idpat = @"client_id=(?<VAL>[^\&]*)&";
                Match client_id = Regex.Match(content, client_idpat);
                string cookie_policypat = @"cookie_policy=(?<VAL>[^\&]*)&";
                Match cookie_policy = Regex.Match(content, cookie_policypat);
                string include_granted_scopespat = @"include_granted_scopes=(?<VAL>[^\&]*)&";
                Match include_granted_scopes = Regex.Match(content, include_granted_scopespat);
                string jshpat = @"jsh=(?<VAL>[^\&]*)&";
                Match jsh1 = Regex.Match(content, jshpat);
                string from_loginpat = @"from_login=(?<VAL>[^\&]*)&";
                Match from_login = Regex.Match(content, from_loginpat);
                string aspat = @"as=(?<VAL>[^\&]*)&";
                Match asp = Regex.Match(content, aspat);
                string authuserpat = @"authuser=(?<VAL>[^\""]*)""";
                Match authuser = Regex.Match(content, authuserpat);
                if (proxy.Groups["VAL"].Value.Equals("") | scope.Groups["VAL"].Value.Equals("") | origin.Groups["VAL"].Value.Equals("") | response_type.Groups["VAL"].Value.Equals("") | redirect_uri.Groups["VAL"].Value.Equals("") | state.Groups["VAL"].Value.Equals("") | client_id.Groups["VAL"].Value.Equals("") | cookie_policy.Groups["VAL"].Value.Equals("") | include_granted_scopes.Groups["VAL"].Value.Equals("") | jsh.Equals("") | asp.Groups["VAL"].Value.Equals("") | authuser.Groups["VAL"].Value.Equals(""))
                {
                    return false;
                }
                string param = "proxy=" + proxy.Groups["VAL"].Value + "&scope=" + scope.Groups["VAL"].Value + "&origin=" + origin.Groups["VAL"].Value + "&response_type=" + response_type.Groups["VAL"].Value + "&redirect_uri=" + redirect_uri.Groups["VAL"].Value + "&state=" + state.Groups["VAL"].Value + "&client_id=" + client_id.Groups["VAL"].Value + "&cookie_policy=" + cookie_policy.Groups["VAL"].Value + "&include_granted_scopes=" + include_granted_scopes.Groups["VAL"].Value + "&jsh=" + jsh + "&from_login=" + from_login.Groups["VAL"].Value + "&as=" + asp.Groups["VAL"].Value + "&authuser=" + authuser.Groups["VAL"].Value;
                Uri uri = new Uri("https://accounts.google.com/o/oauth2/auth");
                HttpClientHandler handler = new HttpClientHandler();
              /*  handler.CookieContainer = new CookieContainer();
                handler.CookieContainer.Add(uri, new Cookie("ACCOUNT_CHOOSER","AFx_qI7stKLq0vORiZQLkkNcU_lHuLNv4GlMx9pPCErpeQ4BIXT22ID335v_c2PF-1d3PHcmJmLozRPMaATmEQfrWB5napkAlffnAPY3YnNuCwDEAx7PNmSYJ4vMXm5DM87F-ThsTK1T"));
                handler.CookieContainer.Add(uri, new Cookie("APISID", "gF_YEyFmczpkiLda/A5ZgZKmc6nD4tpEtV"));
                handler.CookieContainer.Add(uri, new Cookie("GALX", "OQikheU25PM"));
                handler.CookieContainer.Add(uri, new Cookie("GAPS", "1:oW2445Eqtpizmz3GnVQql-ywjUpnTg:ygUCqByWNHTEjUuH"));
                handler.CookieContainer.Add(uri, new Cookie("HSID", "ADBWohh-FMAdbPVoz"));
                handler.CookieContainer.Add(uri, new Cookie("LSID", "s.KR|s.youtube:egNYTCCabrONkRuQu4wc2g6q4l2ZASxFyYtMlxO3YhQ6s3qRrLOpnuRPl2KUKI2ddxmpNg."));
                handler.CookieContainer.Add(uri, new Cookie("SAPISID", "Mfwg_FfL5uV6Zq63/A6V2lsIHUoZOmJChp"));
                handler.CookieContainer.Add(uri, new Cookie("SID", "egNYTGHQBS1-gIeYtNFq-Oe9Y6HPllkwd59GlMFnHSFNVdPNUrQKMAtYRPYwlsEmrmzUPg."));
                handler.CookieContainer.Add(uri, new Cookie("SSID", "AoLAR6NA8hSWV2BAf"));*/
                for (int i = 0; i < cookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(cookieName[i], cookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://accounts.google.com/signin/challenge/sl/password");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Client-Data", "CI22yQEIpbbJAQjEtskBCP2VygEIwpjKAQjynMoB");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://accounts.google.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");
                
                var result = httpClient.GetAsync("https://accounts.google.com/o/oauth2/auth?&"+param).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                if (Regex.Match(strContent, @"<title>(?<VAL>[^\<]*)").Groups["VAL"].Value == "Request for Permission")
                {
                    state_wrapper = Regex.Match(strContent, @"state_wrapper"" value=""(?<VAL>[^\""]*)").Groups["VAL"].Value;
                    xsrfsign = Regex.Match(strContent, @"state_wrapper"" value=""(?<VAL>[^\""]*)").Groups["VAL"].Value;
                    strContent = Getaccesstocken("https://accounts.google.com/o/oauth2/auth?&" + param, Regex.Match(strContent, @"action=""(?<VAL>[^\""]*)").Groups["VAL"].Value);
                }
                string pattern = @"access_token=(?<VAL>[^\&]*)&";
                Match access_token = Regex.Match(strContent,pattern);
                access_token_string = access_token.Groups["VAL"].Value;
                if (access_token_string.Equals(""))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool GumLogin()
        {
            try
            {
                Uri uri = new Uri("https://my.gumtree.com/login");

                HttpClientHandler handler = new HttpClientHandler();

                for (int i = 0; i < initCookieName.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Cache-Control","max-age=0");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://my.gumtree.com/login");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                Dictionary<string, string> dicParms = new Dictionary<string, string>();
                string aa = "";
                tb_username.Invoke(new Action(
               () => aa = tb_username.Text.Split('@')[0] +"@gmail.com"
               )
               );
                dicParms.Add("username", aa.ToLower());
                dicParms.Add("password", access_token_string);
                dicParms.Add("loginPlatform", "googleplus");
                KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
                for (int i = 0; i < dicParms.Keys.Count; i++)
                {
                    keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
                }
                var result = httpClient.PostAsync("https://my.gumtree.com/login", new FormUrlEncodedContent(keyPairs)).Result;
                result.EnsureSuccessStatusCode();
                if (viewflag)
                {
                    string strContent = result.Content.ReadAsStringAsync().Result;
                    string id_pat = @"""adId"":""(?<VAL>[^\""]*)""";
                    string id_title = @"""title"":""(?<VAL>[^\""]*)""";

                    MatchCollection adId_match = Regex.Matches(strContent, id_pat);
                    MatchCollection title_match = Regex.Matches(strContent, id_title);
                    PostedlistArray.Clear();
                    list_posted.Invoke(new Action(
               () => list_posted.Items.Clear()
               )
               );

                    for (int i = 0; i < adId_match.Count; i++)
                    {
                        ProductList ad = new ProductList();
                        ad.Id = adId_match[i].Groups["VAL"].Value;
                        ad.Title = title_match[i].Groups["VAL"].Value;
                        PostedlistArray.Add(ad);
                        list_posted.Invoke(new Action(
               () => list_posted.Items.Add(ad.Title)
               )
               );

                    }
                }

                IEnumerable<Cookie> responseCookies = handler.CookieContainer.GetCookies(uri).Cast<Cookie>();
                initCookieValue.Clear();
                initCookieName.Clear();
                foreach (Cookie cookie in responseCookies)
                {
                    initCookieName.Add(cookie.Name);
                    initCookieValue.Add(cookie.Value);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool initLogin()
        {
            try
            {
                initCookieName.Clear();
                initCookieValue.Clear();
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var result = httpClient.GetAsync("https://my.gumtree.com/login").Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                Uri uri = new Uri("https://my.gumtree.com/login");
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    initCookieName.Add(cookie.Name);
                    initCookieValue.Add(cookie.Value);
                }
                client_id = Regex.Match(strContent, @"type:googleplus,appId:(?<Client_id>[^\""]*)""").Groups["Client_id"].Value;
                jsh = Getjsh();
                Getlocation();
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Getjsh()
        {
            try
            {
                Uri uri = new Uri("https://apis.google.com/js/client:platform.js");
                HttpClientHandler handler = new HttpClientHandler();
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
               // httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "apis.google.com");
                var result = httpClient.GetAsync(uri).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                string js = Regex.Match(strContent, @"""h"":""(?<VAL>[^\""]*)").Groups["VAL"].Value.Replace("\\u003d", "=");
                return js;
            }
            catch
            {
                return "";
            }
        }

        public void Getlocation()
        {
            try
            {
                Uri url = new Uri("https://accounts.google.com/o/oauth2/auth?client_id=" + client_id + "&response_type=code%20token%20id_token%20gsession&scope=profile%20email&cookie_policy=single_host_origin&immediate=true&include_granted_scopes=true&proxy=oauth2relay248766495&redirect_uri=postmessage&origin=https%3A%2F%2Fmy.gumtree.com&state=458934160%7C0.2086572272&authuser=0&jsh=" + jsh);
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                for (int i = 0; i < initCookieName.Count; i++)
                {
                    handler.CookieContainer.Add(url, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");
                
                var result = httpClient.GetAsync(url).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                cookieValue.Clear();
                cookieName.Clear();
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(url).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    cookieName.Add(cookie.Name);
                    cookieName.Add(cookie.Value);
                }
                handler = new HttpClientHandler();
                handler.CookieContainer = cookies;
            //    handler.AllowAutoRedirect = false;
                httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "close");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");

                url = new Uri("https://accounts.google.com/o/oauth2/auth?client_id=" + client_id + "&redirect_uri=postmessage&response_type=code%20token%20id_token%20gsession&scope=profile%20email&cookie_policy=single_host_origin&include_granted_scopes=true&proxy=oauth2relay248766495&origin=https%3A%2F%2Fmy.gumtree.com&state=458934160%7C0.292234422&&jsh=" + jsh);
                result = httpClient.GetAsync(url).Result;
                result.EnsureSuccessStatusCode();
                strContent = result.Content.ReadAsStringAsync().Result; 
                GALX = Regex.Match(strContent, @"name=""GALX"" value=""(?<VAL>[^\""]*)").Groups["VAL"].Value;
                continu = Regex.Match(strContent, @"name=""continue"" value=""(?<VAL>[^\""]*)").Groups["VAL"].Value;
                gxf = Regex.Match(strContent, @"name=""gxf"" value=""(?<VAL>[^\""]*)").Groups["VAL"].Value;
                locationurl = "https://accounts.google.com/ServiceLogin?passive=1209600&" + Regex.Match(strContent, @"https://accounts.google.com/ServiceLogin\?continue=(?<VAL>[^\""]*)").Groups["VAL"].Value;
                responseCookies = cookies.GetCookies(url).Cast<Cookie>();
                cookieValue.Clear();
                cookieName.Clear();
                foreach (Cookie cookie in responseCookies)
                {
                    cookieName.Add(cookie.Name);
                    cookieValue.Add(cookie.Value);
                }
                
            }
            catch
            {
                return ;
            }
        }
       
        public bool getGoogleAccount()
        {
            try
            {
                Uri uri = new Uri("https://accounts.google.com/ListAccounts");
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < cookieName.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(cookieName[i], cookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type","application/x-www-form-urlencoded");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://www.google.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "accounts.google.com");
                Dictionary<string, string> dicParms = new Dictionary<string, string>();
                dicParms.Add("source", "ChromiumBrowser");
                dicParms.Add("json", "standard");
                KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
                for (int i = 0; i < dicParms.Keys.Count; i++)
                {
                    keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
                }
                var result = httpClient.PostAsync("https://accounts.google.com/ListAccounts", new FormUrlEncodedContent(keyPairs)).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                string pat = "";
                tb_username.Invoke(new Action(
                    () =>  pat= @",""(?<VAL>[^\""]*)"",""" + tb_username.Text.ToLower()
                    ));
                
                Match contactName = Regex.Match(strContent, pat);
                string[] acc = contactName.Groups["VAL"].Value.Split(' ');
                google_account_name = acc[0];
                if (google_account_name.Equals(""))
                {
                    return false;
                }
//                 IEnumerable<Cookie> responseCookies = handler.CookieContainer.GetCookies(uri).Cast<Cookie>();
//                 cookieValue.Clear();
//                 cookieName.Clear();
//                 foreach (Cookie cookie in responseCookies)
//                 {
//                     cookieName.Add(cookie.Name);
//                     cookieValue.Add(cookie.Value);
//                 }
                return true;
            }
            catch
            {
                return false;
            }
        }
        private void bt_postall_Click(object sender, EventArgs e)
        {
            relogtime = int.Parse(tb_relog.Value.ToString());
            postall = new Thread(this.postThread);
            postall.Start();
        }
        
        public bool getPostUrl(ProductList ad)
        {
            Uri uri = new Uri("https://my.gumtree.com/postad");
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = 0; i < initCookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://my.gumtree.com/manage/ads");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
            var result = httpClient.GetAsync("https://my.gumtree.com/postad").Result;
            result.EnsureSuccessStatusCode();
            string pat = @"<meta property=""og:url"" content=""(?<VAL>[^\""]*)""/>";
            string strContent = result.Content.ReadAsStringAsync().Result;
            Match postad_Id = Regex.Match(strContent, pat);
            ad.postad_Id_string = postad_Id.Groups["VAL"].Value;
            if (ad.postad_Id_string.Equals(""))
            {
                return false;
            }
            pat = @"time"":(?<VAL>[^\,]*),";
            string time = Regex.Match(strContent, pat).Groups["VAL"].Value;
            string urltime = (Int64.Parse(time) + 10000).ToString();
            result = httpClient.GetAsync(ad.postad_Id_string + "?getFormData=true&_=" + urltime).Result;
            result.EnsureSuccessStatusCode();
            strContent = result.Content.ReadAsStringAsync().Result;

            result = httpClient.PostAsync(ad.postad_Id_string, new StringContent(strContent, System.Text.Encoding.UTF8, "application/json")).Result;
            result.EnsureSuccessStatusCode();



            return true;
        }
        public bool postCategory(ProductList ad)
        {
            try
            {
                Uri uri = new Uri(ad.postad_Id_string);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieName.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                string content1 = "{\"removeDraft\": true}";
                var result = httpClient.PostAsync(ad.postad_Id_string, new StringContent(content1, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();
                string content = "{\"formErrors\":{},\"categoryId\":\""+ad.Category+"\",\"locationId\":null,\"postcode\":null,\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":null,\"description\":null,\"previousContactName\":null,\"contactName\":\""+ad.contactname+"\",\"previousContactEmail\":null,\"contactEmail\":\""+ad.username+"\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":true,\"usePhone\":false,\"useUrl\":false,\"mainImageId\":null,\"imageIds\":[],\"youtubeLink\":null,\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\""+ad.username+"\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{},\"features\":{\"FEATURED\":{\"selected\":false,\"productName\":\"FEATURE_7_DAY\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                result = httpClient.PostAsync(ad.postad_Id_string, new StringContent(content, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();
                
                string strContent = result.Content.ReadAsStringAsync().Result;
                string pat = "seller-type";
                ad.sellertype = strContent.Contains(pat);
                
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool sendCategoryId(ProductList ad)
        {
            try
            {
                Uri uri = new Uri(ad.postad_Id_string);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieName.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                string content = "{\"formErrors\":{},\"categoryId\":\""+ad.Category+"\",\"locationId\":null,\"postcode\":null,\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":null,\"description\":null,\"previousContactName\":null,\"contactName\":\""+ad.contactname+"\",\"previousContactEmail\":null,\"contactEmail\":\""+ad.username+"\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":true,\"usePhone\":false,\"useUrl\":false,\"mainImageId\":null,\"imageIds\":[],\"youtubeLink\":null,\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\""+ad.username+"\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{},\"features\":{\"FEATURED\":{\"selected\":false,\"productName\":\"FEATURE_7_DAY\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var result = httpClient.PostAsync(ad.postad_Id_string, new StringContent(content, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                string pat = @"{""id"":(?<VAL>[^\,]*),";
                MatchCollection parentId = Regex.Matches(strContent,pat);
                string[] arr = new string[parentId.Count];
                int num = 0;
                foreach (Match match in parentId)
                {
                    if (match.Success){
                        arr[num] = match.Groups["VAL"].Value;
                        num++;
                    }
                }
                string parentId_string = arr[parentId.Count - 2];
                ad.ParentId = parentId_string;
                

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool getPostCode(ProductList ad)
        {
            try
            {
                Uri uri = new Uri(ad.postad_Id_string);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieName.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                string content = "{\"formErrors\":{\"global\":[]},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"\",\"postcode\":\"" + ad.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":null,\"description\":null,\"previousContactName\":null,\"contactName\":\""+ad.contactname+"\",\"previousContactEmail\":null,\"contactEmail\":\""+ad.username+"\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":true,\"usePhone\":false,\"useUrl\":false,\"mainImageId\":null,\"imageIds\":[],\"youtubeLink\":null,\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\""+ad.username+"\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":null},\"features\":{\"FEATURED\":{\"selected\":false,\"productName\":\"FEATURE_7_DAY\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var result = httpClient.PostAsync(ad.postad_Id_string, new StringContent(content, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;
                string pat = @"""locationId"":(?<VAL>[^\,]*),""postcode";
                Match locationId = Regex.Match(strContent, pat);
                ad.locationId = locationId.Groups["VAL"].Value;
                if (ad.postcode.Equals(""))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool postImage(ProductList ad)
        {
            string[] img_path = ad.Images.Split('|');
            for (int i = 0; i < img_path.Length; i++)
            {
                if (!img_path[i].Equals(""))
                {
                    if (!PostImageInit(ad))
                    {
                        continue;
                    }
                    if (!postOneImage(img_path[i],ad,i))
                    {
                        continue;
                    }
                }
            }
            if (ad.imageIds.Count == 0)
            {
                return false;
            }
            return true;
        }
        public bool PostImageInit(ProductList ad)
        {
            try
            {
                string postad_Id_string_draft = ad.postad_Id_string.Replace("postad", "postad-draft");
                Uri uri = new Uri(postad_Id_string_draft);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type","application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                string imgId_str = "";
                for (int i = 0; i < ad.imageIds.Count; i++)
                {
                    imgId_str += "\"" + ad.imageIds[i] + "\","; 
                }
                imgId_str += "\"0\"";
                string str = "{\"formErrors\":{},\"categoryId\":\""+ad.Category+"\",\"locationId\":\""+ad.locationId+"\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"\",\"description\":\"\",\"previousContactName\":null,\"contactName\":\""+ad.contactname+"\",\"previousContactEmail\":null,\"contactEmail\":\""+ad.username+"\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":["+imgId_str+"],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\""+ad.username+"\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var result = httpClient.PostAsync(postad_Id_string_draft, new StringContent(str, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();
                string strContent = result.Content.ReadAsStringAsync().Result;

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool postOneImage(string filepath,ProductList ad,int position)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            if (file.Exists)
            {
                try
                {
                    Uri uri = new Uri(ad.postad_Id_string + "/images");
                    HttpClientHandler handler = new HttpClientHandler();
                    handler.CookieContainer = new CookieContainer();
                    for (int i = 0; i < initCookieValue.Count; i++)
                    {
                        handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                    }
                    HttpClient httpClient = new HttpClient(handler);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data;");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");


                    FileInfo fInfo = new FileInfo(filepath);
                    var fileStream = File.OpenRead(filepath);
                    byte[] fileData = new byte[fInfo.Length];
                    fileStream.Read(fileData, 0, (int)fInfo.Length);
                    fileStream.Close();
                    MultipartFormDataContent content = new MultipartFormDataContent();
                    var values = new[]
                            {
                                new KeyValuePair<string, string>("position", position.ToString()),                                                                                                                                                                                              
                            };
                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value),
                            String.Format("\"{0}\"", keyValuePair.Key));
                    }

                    content.Add(new StreamContent(new MemoryStream(fileData)), "image", "blob");
                    var result = httpClient.PostAsync(ad.postad_Id_string + "/images", content).Result;
                    result.EnsureSuccessStatusCode();
                    string strContent = result.Content.ReadAsStringAsync().Result;
                    string pat = @"""id"":(?<VAL>[^\,]*),";
                    Match imageId = Regex.Match(strContent, pat);
                    string imageId_string = imageId.Groups["VAL"].Value;
                    if (imageId_string.Equals(""))
                    {
                        return false;
                    }
                    ad.imageIds.Add(imageId_string);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show("There is not "+filepath + "  in its own path.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return true;
            }
        }
        public bool postContent(ProductList ad)
        {
            try
            {
                string content_upload_url = ad.postad_Id_string.Replace("postad", "postad-draft");
                string imgId_str = "";
                for (int i = 0; i < ad.imageIds.Count; i++)
                {
                    imgId_str += "\"" + ad.imageIds[i] + "\",";
                }
                imgId_str += "\"0\"";
                string content = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                Uri uri = new Uri(content_upload_url);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");

                var resultTitle = httpClient.PostAsync(content_upload_url, new StringContent(content, System.Text.Encoding.UTF8, "application/json")).Result;
                resultTitle.EnsureSuccessStatusCode();
                string contentDes = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                //string encodeDes = HttpUtility.UrlEncode(contentDes);
                var resultDesciption = httpClient.PostAsync(content_upload_url, new StringContent(contentDes, System.Text.Encoding.UTF8, "application/json")).Result;
                resultDesciption.EnsureSuccessStatusCode();
                string contentPrice = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var resultPrice = httpClient.PostAsync(content_upload_url, new StringContent(contentPrice, System.Text.Encoding.UTF8, "application/json")).Result;
                resultPrice.EnsureSuccessStatusCode();

 /*               string contentFeatured = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var resultFeatured = httpClient.PostAsync(content_upload_url, new StringContent(contentFeatured, System.Text.Encoding.UTF8, "application/json")).Result;
                resultFeatured.EnsureSuccessStatusCode();


                string contentspotlight = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var resultspotlight = httpClient.PostAsync(content_upload_url, new StringContent(contentspotlight, System.Text.Encoding.UTF8, "application/json")).Result;
                resultspotlight.EnsureSuccessStatusCode();

                string contenturgent = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\""+ad.postcode+"\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"0\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":false}";
                var resulturgent = httpClient.PostAsync(content_upload_url, new StringContent(contenturgent, System.Text.Encoding.UTF8, "application/json")).Result;
                resulturgent.EnsureSuccessStatusCode();*/
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool postContentEnd(ProductList ad)
        {
            try
            {
                string imgId_str = "";
                for (int i = 0; i < ad.imageIds.Count; i++)
                {
                    imgId_str += "\"" + ad.imageIds[i] + "\",";
                }
                imgId_str += "\"0\"";
                Uri uri = new Uri(ad.postad_Id_string);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                string contentall = "";
                if (ad.sellertype) {
                    if (ad.phonenumber.Equals(""))
                    {
                        contentall = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\"" + ad.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description.Replace("\r\n", @"\r\n") + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"" + ad.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":false,\"attributes\":{\"seller_type\":\"private\",\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":true}";
                    }
                    else
                    {
                        contentall = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\"" + ad.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description.Replace("\r\n", @"\r\n") + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"" + ad.phonenumber + "\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"true\",\"useUrl\":false,\"mainImageId\":\"" + ad.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":\"" + ad.phonenumber + "\",\"password\":null,\"optInMarketing\":false,\"attributes\":{\"seller_type\":\"private\",\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":true}";
                    }
                }
                else
                {
                    if (ad.phonenumber.Equals(""))
                    {
                        contentall = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\"" + ad.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description.Replace("\r\n", @"\r\n") + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"" + ad.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":false,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":true}";
                    }
                    else
                    {
                        contentall = "{\"formErrors\":{},\"categoryId\":\"" + ad.Category + "\",\"locationId\":\"" + ad.locationId + "\",\"postcode\":\"" + ad.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + ad.Title + "\",\"description\":\"" + ad.Description.Replace("\r\n", @"\r\n") + "\",\"previousContactName\":null,\"contactName\":\"" + ad.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + ad.username + "\",\"contactTelephone\":\"" + ad.phonenumber + "\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"true\",\"useUrl\":false,\"mainImageId\":\"" + ad.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + ad.username + "\",\"telephoneNumber\":\"" + ad.phonenumber + "\",\"password\":null,\"optInMarketing\":false,\"attributes\":{\"price\":\"" + ad.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"false\"},\"FEATURED\":{\"selected\":\"false\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"false\"}},\"removeDraft\":\"false\",\"submitForm\":true}";
                    }

                    
                }
                var resultall = httpClient.PostAsync(ad.postad_Id_string, new StringContent(contentall, System.Text.Encoding.UTF8, "application/json")).Result;
                resultall.EnsureSuccessStatusCode();
                
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool postbumpup(ProductList ad)
        {
            try
            {
                string imgId_str = "";
                for (int i = 0; i < ad.imageIds.Count; i++)
                {
                    imgId_str += "\"" + ad.imageIds[i] + "\",";
                }
                imgId_str += "\"0\"";
                Uri uri = new Uri(ad.postad_Id_string+"/bumpup");
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.postad_Id_string);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var resultall = httpClient.GetAsync(ad.postad_Id_string+"/bumpup").Result;
                resultall.EnsureSuccessStatusCode();
/*                string content = resultall.Content.ReadAsStringAsync().Result;
                string pat = @"<form name=""postad-form"" action=""(?<VAL>[^\""]*)""";
                Match finish = Regex.Match(content, pat);
                string finish_string = finish.Groups["VAL"].Value;
                if (finish_string.Equals(""))
                    payment(httpClient,finish_string);*/
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool postadOne(ProductList ad)
        {
            try
            {
                int count = postcodeArr.Count;
                Random r = new Random();
                int num = r.Next(0, count);
                ad.Description = ad.Description.Replace("\r\n", @"\r\n");
                ad.contactname = google_account_name;
                tb_phone.Invoke(new Action(
                () => ad.phonenumber = tb_phone.Text
                )
                );
                tb_username.Invoke(new Action(
                () => ad.username = tb_username.Text.Split('@')[0]+"@gmail.com"
                )
                );
                if (!getPostUrl(ad))
                {
                    return false;
                }
                if (!postCategory(ad))
                {
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_one_post/6 
                    ));
 /*               if (!sendCategoryId(ad))
                {
                    MessageBox.Show("PostCode is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }*/
                ad.postcode = postcodeArr[num];
                if (!getPostCode(ad))
                {
                    MessageBox.Show("PostCode is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_one_post / 6
                    ));
                if (!postImage(ad))
                {
  //                  MessageBox.Show("Image upload is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_one_post / 6
                    ));
                if (!postContent(ad))
                {
                    MessageBox.Show("The information of ad is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_one_post / 6
                    ));
                if (!postContentEnd(ad))
                {
                    MessageBox.Show("The information of ad is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_one_post / 6
                    ));
                if (!postbumpup(ad))
                {
                    MessageBox.Show("The information of ad is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum
                    ));
                return true;
            }
            catch
            {
  //              MessageBox.Show("Post ad is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
        }
        public void postThread()
        {
            disableAllButton();
            int k = 0;
            if (relogtime == 0)
            {
                while (true)
                {
                    if (login())
                    {
                        pg_bar.Invoke(new Action(
                            () => pg_bar.Value = 0
                            ));

                        pg_bar.Invoke(new Action(
                            () => pg_one_post = pg_bar.Maximum / PlistArray.Count
                            ));
                        for (int i = 0; i < PlistArray.Count; i++)
                        {
                            lb_stat.Invoke(new Action(
                                () => lb_stat.Text = "Posting ad to gumtree....."
                                )
                                );
                            postadOne(PlistArray[i]);
                            pg_bar.Invoke(new Action(
                            () => pg_bar.Value = pg_one_post * (i + 1)
                            ));
                            lb_stat.Invoke(new Action(
                            () => lb_stat.Text = "Posting one ad is finished!..."
                            )
                            );
                            tb_time.Invoke(new Action(
                                () => Thread.Sleep(Convert.ToInt32(tb_time.Value.ToString()) * 1000)
                                ));

                        }
                        loginflag = false;
                        pg_bar.Invoke(new Action(
                            () => pg_bar.Value = pg_bar.Maximum
                            ));
                        lb_stat.Invoke(new Action(
                        () => lb_stat.Text = "Posting ad is finished. SUCCESS!"
                        )
                        );
                        enableAllButton();
                    }
                    else
                    {
                        lb_stat.Invoke(new Action(
                            () => lb_stat.Text = "Login is fail...."
                            )
                            );
                        enableAllButton();
                        return;
                    }
                }
            }
            else
            {
                while (true)
                {
                    if (login())
                    {
                        pg_bar.Invoke(new Action(
                            () => pg_bar.Value = 0
                            ));

                        pg_bar.Invoke(new Action(
                            () => pg_one_post = pg_bar.Maximum / relogtime
                            ));
                        for (int i = 0; i < relogtime; i++)
                        {
                            if (k == PlistArray.Count)
                            {
                                pg_bar.Invoke(new Action(
                            () => pg_bar.Value = 0
                            ));

                                k = 0;
                            }
                                
                            lb_stat.Invoke(new Action(
                                () => lb_stat.Text = "Posting ad to gumtree....."
                                )
                                );
                            postadOne(PlistArray[k]);
                            pg_bar.Invoke(new Action(
                            () => pg_bar.Value = pg_one_post * (i + 1)
                            ));
                            lb_stat.Invoke(new Action(
                            () => lb_stat.Text = "Posting one ad is finished!..."
                            )
                            );
                            string time = "";
                            tb_time.Invoke(new Action(
                                () => time = tb_time.Text
                                ));

                            tb_time.Invoke(new Action(
                            () => Thread.Sleep(Convert.ToInt32(tb_time.Value.ToString()) * 1000)
                            ));
                            k++;
                        }
                        loginflag = false;
                        pg_bar.Invoke(new Action(
                            () => pg_bar.Value = pg_bar.Maximum
                            ));
                    }
                    else
                    {
                        lb_stat.Invoke(new Action(
                            () => lb_stat.Text = "Login is fail...."
                            )
                            );
                        enableAllButton();
                        return;
                    }
                }
            }
        }
        public void postOneThread(Object obj)
        {
            disableAllButton();
            if (login())
            {
                lb_stat.Invoke(new Action(
                () => lb_stat.Text = "Posting ad to gumtree....."
                )
                );
                pg_bar.Invoke(new Action(
                   () => pg_bar.Value = 0
                   ));
                
                pg_bar.Invoke(new Action(
                    () => pg_one_post = pg_bar.Maximum
                    ));
                ProductList ad = (ProductList)obj;
                if (!postadOne(ad))
                {
                    enableAllButton();
   //                 MessageBox.Show("post ad is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                lb_stat.Invoke(new Action(
                () => lb_stat.Text = "Posting ad is finished. SUCCESS!"
                )
                );
                enableAllButton();
            }
            else
            {
                enableAllButton();
   //             int num = (int)MessageBox.Show("Login is failed. Please input your username and password again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            AnalyseProductListXml();
        }

        private void bt_edit_Click(object sender, EventArgs e)
        {

        }

        public void view_Ads()
        {
            disableAllButton();
            if (!loginflag)
            {
                lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "viewing ads...."
                    )
                    );
                viewflag = true;
                if (!login())
                {
                    lb_stat.Invoke(new Action(
                  () => lb_stat.Text = ""
                  )
                  );
                    enableAllButton();
                    int num = (int)MessageBox.Show("Login is failed. Please input your username and password again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return;
                }
                lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "view ads success..!"
                    )
                    );
                viewflag = false;
            }
            else
            {
                lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "viewing ads...."
                    )
                    );
                viewflag = true;
                GumLogin();
                enableAllButton();
                lb_stat.Invoke(new Action(
                   () => lb_stat.Text = "view ads success..!"
                   )
                   );
                viewflag = false;
            }
            enableAllButton();
        }
        private void bt_view_Click(object sender, EventArgs e)
        {
            postall = new Thread(this.view_Ads);
            postall.Start();
        }

        private void bt_edit_Click_1(object sender, EventArgs e)
        {
            if (list_posted.SelectedIndex == -1)
            {
                lb_stat.Text = "";
                MessageBox.Show("Please select a ad to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                postall = new Thread(update);
                postall.Start();
            }
        }

        public void update()
        {
            disableAllButton();
            lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "Updating ad...."
                    )
                    );
            tb_title.Invoke(new Action(
                    () => selectedPostedAd.Title = tb_title.Text
                    )
                    );
            tb_category.Invoke(new Action(
                    () => selectedPostedAd.Category = tb_category.Text
                    )
                    );
            tb_description.Invoke(new Action(
                    () => selectedPostedAd.Description = tb_description.Text.Replace("\r\n", @"\r\n")
                    )
                    );
            c.Invoke(new Action(
                    () => selectedPostedAd.Price = c.Value.ToString()
                    )
                    );
            int count = 0;
            list_posted.Invoke(new Action(
                    () => count = list_posted.Items.Count
                    )
                    );
            if (selectedPostedAd.Title.Equals("") | selectedPostedAd.Category.Equals("") | selectedPostedAd.Price.ToString().Equals("0") | count == 0 | selectedPostedAd.Description.Equals(""))
            {
                enableAllButton();
                MessageBox.Show("The information of ad is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            string des = selectedPostedAd.Description;
            des = des.Replace("\r\n", " ");
            string[] desarr = des.Split(' ');
            int cou = 0;
            for (int i = 0; i < desarr.Length; i++)
            {
                if (!desarr[i].Equals(""))
                {
                    cou++;
                }
            }
            if (cou < 12)
            {
                enableAllButton();
                MessageBox.Show("You must write at least 12 words in your description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "Updating ad's images...."
                    )
                    );
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = 0
                    ));
            updateImage();
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum / 3
                    ));
            lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "Updating ad's contents...."
                    )
                    );
            updateContent();
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum
                    ));
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum * 2 / 3
                    ));
            updatebumpup();
            list_posted.Invoke(new Action(
                    () => list_posted.SelectedValue = selectedPostedAd.Title
                    )
                    );
            lb_stat.Invoke(new Action(
                    () => lb_stat.Text = "Updating Success!"
                    )
                    );
            enableAllButton();
        }

        public void updatebumpup()
        {
            string imgId_str = "";
            for (int i = 0; i < selectedPostedAd.imageIds.Count; i++)
            {
                imgId_str += "\"" + selectedPostedAd.imageIds[i] + "\",";
            }
            imgId_str += "\"0\"";
            Uri uri = new Uri(selectedPostedAd.edit_url + "/bumpup");
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = 0; i < initCookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", selectedPostedAd.edit_url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
            var resultall = httpClient.GetAsync(selectedPostedAd.edit_url + "/bumpup").Result;
            resultall.EnsureSuccessStatusCode();
            string content = resultall.Content.ReadAsStringAsync().Result;
            string pat = @"<meta name=""twitter:url"" content=""(?<VAL>[^\""]*)""/>";
            string f = Regex.Match(content, pat).Groups["VAL"].Value;
            if (!f.Equals("https://my.gumtree.com/login"))
            {
                updateEnd();
            }
        }
        public void updateEnd()
        {
            try
            {
                string imgId_str = "";
                for (int i = 0; i < selectedPostedAd.imageIds.Count; i++)
                {
                    imgId_str += "\"" + selectedPostedAd.imageIds[i] + "\",";
                }
                imgId_str += "\"0\"";
                Uri uri = new Uri(selectedPostedAd.edit_url + "/bumpup");
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);


                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, */*");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", selectedPostedAd.edit_url);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                Dictionary<string, string> dicParms = new Dictionary<string, string>();
                dicParms.Add("bumpUp", "false");
                dicParms.Add("confirm", "Update my ad");
                KeyValuePair<string, string>[] keyPairs = new KeyValuePair<string, string>[dicParms.Keys.Count];
                for (int i = 0; i < dicParms.Keys.Count; i++)
                {
                    keyPairs[i] = new KeyValuePair<string, string>(dicParms.Keys.ElementAt(i), dicParms[dicParms.Keys.ElementAt(i)]);
                }
                var result = httpClient.PostAsync(selectedPostedAd.edit_url + "/bumpup", new FormUrlEncodedContent(keyPairs)).Result;
                result.EnsureSuccessStatusCode();
            }
            catch
            {
                return;
            }
        }
        public void updateContent()
        {
            string imgId_str = "";
            for (int i = 0; i < selectedPostedAd.imageIds.Count; i++)
            {
                imgId_str += "\"" + selectedPostedAd.imageIds[i] + "\",";
            }
            imgId_str += "\"0\"";
            Uri uri = new Uri(selectedPostedAd.edit_url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = 0; i < initCookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", selectedPostedAd.edit_url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");

            string contentall = "";
            if (!selectedPostedAd.sellertype)
            {
                if (selectedPostedAd.phonenumber.Equals(""))
                {
                    contentall = "{\"formErrors\":{},\"categoryId\":\"" + selectedPostedAd.Category + "\",\"locationId\":\"" + selectedPostedAd.locationId + "\",\"postcode\":\"" + selectedPostedAd.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + selectedPostedAd.Title + "\",\"description\":\"" + selectedPostedAd.Description + "\",\"previousContactName\":null,\"contactName\":\"" + selectedPostedAd.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + selectedPostedAd.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"" + selectedPostedAd.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + selectedPostedAd.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + selectedPostedAd.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"true\"},\"FEATURED\":{\"selected\":\"true\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"true\"}},\"removeDraft\":\"false\",\"submitForm\":false,\"featureUrgent\":\"true\",\"useAllBump\":\"true\"}";
                }
                else
                {
                    contentall = "{\"formErrors\":{},\"categoryId\":\"" + selectedPostedAd.Category + "\",\"locationId\":\"" + selectedPostedAd.locationId + "\",\"postcode\":\"" + selectedPostedAd.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + selectedPostedAd.Title + "\",\"description\":\"" + selectedPostedAd.Description + "\",\"previousContactName\":null,\"contactName\":\"" + selectedPostedAd.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + selectedPostedAd.username + "\",\"contactTelephone\":\"" + selectedPostedAd.phonenumber + "\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"true\",\"useUrl\":false,\"mainImageId\":\"" + selectedPostedAd.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + selectedPostedAd.username + "\",\"telephoneNumber\":\"" + selectedPostedAd.phonenumber + "\",\"password\":null,\"optInMarketing\":true,\"attributes\":{\"price\":\"" + selectedPostedAd.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"true\"},\"FEATURED\":{\"selected\":\"true\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"true\"}},\"removeDraft\":\"false\",\"submitForm\":false,\"featureUrgent\":\"true\",\"useAllBump\":\"true\"}";
                }
                
            }
            else
            {
                if (selectedPostedAd.phonenumber.Equals(""))
                {
                    contentall = "{\"formErrors\":{},\"categoryId\":\"" + selectedPostedAd.Category + "\",\"locationId\":\"" + selectedPostedAd.locationId + "\",\"postcode\":\"" + selectedPostedAd.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + selectedPostedAd.Title + "\",\"description\":\"" + selectedPostedAd.Description + "\",\"previousContactName\":null,\"contactName\":\"" + selectedPostedAd.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + selectedPostedAd.username + "\",\"contactTelephone\":\"\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"false\",\"useUrl\":false,\"mainImageId\":\"" + selectedPostedAd.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + selectedPostedAd.username + "\",\"telephoneNumber\":null,\"password\":null,\"optInMarketing\":true,\"attributes\":{\"seller_type\":\"private\",\"price\":\"" + selectedPostedAd.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"true\"},\"FEATURED\":{\"selected\":\"true\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"true\"}},\"removeDraft\":\"false\",\"submitForm\":false,\"featureUrgent\":\"true\",\"useAllBump\":\"true\"}";
                }
                else
                {
                    contentall = "{\"formErrors\":{},\"categoryId\":\"" + selectedPostedAd.Category + "\",\"locationId\":\"" + selectedPostedAd.locationId + "\",\"postcode\":\"" + selectedPostedAd.postcode + "\",\"visibleOnMap\":true,\"area\":null,\"termsAgreed\":\"\",\"title\":\"" + selectedPostedAd.Title + "\",\"description\":\"" + selectedPostedAd.Description + "\",\"previousContactName\":null,\"contactName\":\"" + selectedPostedAd.contactname + "\",\"previousContactEmail\":null,\"contactEmail\":\"" + selectedPostedAd.username + "\",\"contactTelephone\":\"" + selectedPostedAd.phonenumber + "\",\"contactUrl\":null,\"useEmail\":\"true\",\"usePhone\":\"true\",\"useUrl\":false,\"mainImageId\":\"" + selectedPostedAd.imageIds[0] + "\",\"imageIds\":[" + imgId_str + "],\"youtubeLink\":\"\",\"websiteUrl\":\"http://\",\"firstName\":null,\"lastName\":null,\"emailAddress\":\"" + selectedPostedAd.username + "\",\"telephoneNumber\":\"" + selectedPostedAd.phonenumber + "\",\"password\":null,\"optInMarketing\":true,\"attributes\":{\"seller_type\":\"private\",\"price\":\"" + selectedPostedAd.Price + "\"},\"features\":{\"WEBSITE_URL\":{\"productName\":\"WEBSITE_URL\",\"selected\":\"false\"},\"URGENT\":{\"productName\":\"URGENT\",\"selected\":\"true\"},\"FEATURED\":{\"selected\":\"true\",\"productName\":\"FEATURE_7_DAY\"},\"SPOTLIGHT\":{\"productName\":\"HOMEPAGE_SPOTLIGHT\",\"selected\":\"true\"}},\"removeDraft\":\"false\",\"submitForm\":false,\"featureUrgent\":\"true\",\"useAllBump\":\"true\"}";
                }
                
            }
            var resultall = httpClient.PostAsync(selectedPostedAd.edit_url, new StringContent(contentall, System.Text.Encoding.UTF8, "application/json")).Result;
            resultall.EnsureSuccessStatusCode();
        }
        public void updateImage()
        {
            int imageCount = 0;
            list_photo.Invoke(new Action(
              () => imageCount = list_photo.Items.Count
              )
              );
            List<string> postimage = new List<string>();
            for (int i = 0; i < imageCount; i++)
            {
                list_photo.Invoke(new Action(
              () => postimage.Add(list_photo.Items[i].ToString())
              )
              );
            }
            bool same = false;
            for (int i = 0; i < selectedPostedAd.imageIds.Count; i++)
            {
                for (int j = 0; j < postimage.Count; j++)
                {
                    if (selectedPostedAd.imageIds[i].Equals(postimage[j]))
                    {
                        same = true;   
                    }
                    
                }
                if(!same)
                    sendDelImage(selectedPostedAd.imageIds[i]);
                same = false;
            }
            for (int i = 0; i < postimage.Count; i++)
            {
                for (int j = 0; j < selectedPostedAd.imageIds.Count; j++)
                {
                    if (selectedPostedAd.imageIds[j].Equals(postimage[i]))
                    {
                        same = true;
                    }

                }
                if (!same)
                {
                    postimage[i] = sendUpimage(postimage[i], i);
                    list_photo.Invoke(new Action(
              () => list_photo.Items.RemoveAt(i)
              )
              );
                    list_photo.Invoke(new Action(
              () => list_photo.Items.Insert(i,postimage[i])
              )
              );
                }
                same = false;
            }
            selectedPostedAd.imageIds = postimage;
        }
        public string sendUpimage(string filepath,int position)
        {
            Uri uri = new Uri(selectedPostedAd.edit_url + "/images");
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = 0; i < initCookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "*/*");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", selectedPostedAd.edit_url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "multipart/form-data;");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");


            FileInfo fInfo = new FileInfo(filepath);
            var fileStream = File.OpenRead(filepath);
            byte[] fileData = new byte[fInfo.Length];
            fileStream.Read(fileData, 0, (int)fInfo.Length);
            fileStream.Close();
            MultipartFormDataContent content = new MultipartFormDataContent();
            var values = new[]
                            {
                                new KeyValuePair<string, string>("position", position.ToString()),                                                                                                                                                                                              
                            };
            foreach (var keyValuePair in values)
            {
                content.Add(new StringContent(keyValuePair.Value),
                    String.Format("\"{0}\"", keyValuePair.Key));
            }

            content.Add(new StreamContent(new MemoryStream(fileData)), "image", "blob");
            var result = httpClient.PostAsync(selectedPostedAd.edit_url+ "/images", content).Result;
            result.EnsureSuccessStatusCode();
            string strContent = result.Content.ReadAsStringAsync().Result;
            string pat = @"""id"":(?<VAL>[^\,]*),";
            Match imageId = Regex.Match(strContent, pat);
            string imageId_string = imageId.Groups["VAL"].Value;
            return imageId_string;
/*            if (imageId_string.Equals(""))
            {
                return false;
            }*/
//            ad.imageIds.Add(imageId_string);
//            return true;
        }
        public void sendDelImage(string id)
        {
            string url = selectedPostedAd.edit_url+"/images/delete";
            Uri uri = new Uri(url);
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            for (int i = 0; i < initCookieValue.Count; i++)
            {
                handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
            }
            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.109 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", selectedPostedAd.edit_url);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
            string str = "{\"id\":\"" + id + "\"}";
            var result = httpClient.PostAsync(url, new StringContent(str, System.Text.Encoding.UTF8, "application/json")).Result;
            result.EnsureSuccessStatusCode();
        }
        

        private void bt_delete_Click(object sender, EventArgs e)
        {
            postall = new Thread(deleteThread);
            postall.Start();
        }
        public void deleteThread()
        {
            disableAllButton();
            lb_stat.Invoke(new Action(
                () => lb_stat.Text = "Deleting ad..."
                )
                );
            if (deleteAd())
            {
                lb_stat.Invoke(new Action(
                () => lb_stat.Text = "Deleting success!"
                )
                );
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value = 0
                    ));
                deleteAd();
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum
                    ));
                enableAllButton();
            }
            else
            {
                lb_stat.Invoke(new Action(
                () => lb_stat.Text = ""
                )
                );
                enableAllButton();
                MessageBox.Show("Deleting ad is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        public bool deleteAd()
        {
            try
            {
                
                string id = "";
                list_posted.Invoke(new Action(
                () => id = PostedlistArray[list_posted.SelectedIndex].Id
                )
                );
                string url = "https://my.gumtree.com/manage/ads/delete-ad/" + id;
                Uri uri = new Uri(url);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/48.0.2564.109 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://my.gumtree.com/manage/ads");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var result = httpClient.GetAsync(url).Result;
                result.EnsureSuccessStatusCode();
                PostedlistArray.RemoveAt(list_posted.SelectedIndex);
                list_posted.Items.RemoveAt(list_posted.SelectedIndex);
                
                return true;
            }
            catch
            {

                return false;
            }
        }

        private void bt_post_Click(object sender, EventArgs e)
        {
            ProductList ad = selectedList;
            postall = new Thread(new ParameterizedThreadStart(this.postOneThread));
            postall.Start(ad);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "image file(*.jpg)|*.jpg";
            dlg.Title = "Select images";
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string[] path = dlg.FileNames;
                int count = path.Length;
                for (int i = 0; i < count; i++)
                {
                    list_photo.Items.Add(path[i]);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int selectphoto = list_photo.SelectedIndex;
            if (selectphoto > -1)
            {
                list_photo.Items.RemoveAt(selectphoto);
            }
            
        }

        private void bt_add_Click(object sender, EventArgs e)
        {
            int count = list_photo.Items.Count;
            if (tb_title.Text.Equals("") | tb_category.Text.Equals("") | c.Value.ToString().Equals("0") | count == 0 | tb_description.Text.Equals(""))
            {
                MessageBox.Show("The information of ad is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            string des = tb_description.Text;
            des = des.Replace("\r\n", " ");
            string[] desarr = des.Split(' ');
            int cou = 0;
            for (int i = 0; i < desarr.Length; i++)
            {
                if (!desarr[i].Equals(""))
                {
                    cou++;
                }
            }
            if (cou < 12)
            {
                MessageBox.Show("You must write at least 12 words in your description", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (selectedList.Title.Equals(""))
            {
                Guid guid = Guid.NewGuid();

                ProductList ad = new ProductList();
                ad.Id = guid.ToString();
                ad.Title = tb_title.Text;
                ad.Price = c.Value.ToString();
                ad.Description = tb_description.Text;
                ad.Category = tb_category.Text;
      //          ad.postcode = tb_relog.Text;
                for (int i = 0; i < list_photo.Items.Count; i++)
                {
                    ad.Images += list_photo.Items[i] + "|";
                }
                PlistArray.Add(ad);
                list_product.Items.Add(ad.Title);
                clean();
                writeXml();
            }
            else
            {
                PlistArray[list_product.SelectedIndex].Title = tb_title.Text;
                PlistArray[list_product.SelectedIndex].Price = c.Value.ToString();
                PlistArray[list_product.SelectedIndex].Description = tb_description.Text;
                PlistArray[list_product.SelectedIndex].Category = tb_category.Text;
      //          PlistArray[list_product.SelectedIndex].postcode = tb_relog.Text;
                PlistArray[list_product.SelectedIndex].Images = "";
                PlistArray[list_product.SelectedIndex].imageIds.Clear();
                for (int i = 0; i < list_photo.Items.Count; i++)
                {
                    PlistArray[list_product.SelectedIndex].Images += list_photo.Items[i] + "|";
     //               PlistArray[list_product.SelectedIndex].imageIds.Add(list_photo.Items[i].ToString());
                }
                clean();
                writeXml();
            }
        }
        public void writeXml()
        {
            XElement ads = new XElement("Ads");
            for (int i = 0; i < PlistArray.Count; i++)
            {
                
                XElement ad = new XElement("Ad",
                    new XElement("Id", PlistArray[i].Category),
                    new XElement("Type","Google"),
                    new XElement("Category",PlistArray[i].Category),
                    new XElement("Title",PlistArray[i].Title),
                    new XElement("Description",PlistArray[i].Description),
                    new XElement("Images",PlistArray[i].Images),
                    new XElement("Price",PlistArray[i].Price));
                ads.Add(ad);
            }
            string xml = ads.ToString();

            System.IO.File.WriteAllText(productlist_path, xml);

        }

        private void bt_deleteAd_Click(object sender, EventArgs e)
        {
            if (list_product.SelectedIndex != -1)
            {
//                ProductList ad = selectedList;

                for (int i = 0; i < PlistArray.Count; i++)
                {
                    if (PlistArray[i].Title.Equals(selectedList.Title))
                    {
                        PlistArray.RemoveAt(i);
                    }
                }
                list_product.Items.RemoveAt(list_product.SelectedIndex);
                clean();
                writeXml();
            }
        }

        private void list_posted_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (list_posted.SelectedIndex != -1)
            {

                ProductList ad = PostedlistArray[list_posted.SelectedIndex];
                if (ad.Description==null)
                {
                    postall = new Thread(new ParameterizedThreadStart(getPostedInfo));
                    postall.Start(ad);
                }
                else
                {
                    list_product.ClearSelected();
                    tb_title.Text = ad.Title;
                    tb_category.Text = ad.Category;
       //             tb_relog.Text = ad.postcode;
                    tb_description.Text = ad.Description.Replace(@"\r\n","\r\n");
                    c.Value = Int64.Parse(ad.Price);
                    list_photo.Items.Clear();
                    for (int i = 0; i < ad.imageIds.Count; i++)
                    {
                        list_photo.Items.Add(ad.imageIds[i]);
                    }
                    selectedPostedAd = ad;
                }

            }
        }

        public void getPostedInfo(Object obj)
        {
            disableAllButton();
            lb_stat.Invoke(new Action(
               () => lb_stat.Text = "Viewing Detail information of selected ad...."
               )
               );
            ProductList ad = (ProductList)obj;
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = 0
                    ));
            if (!getInfo1(ad))
            {
                lb_stat.Invoke(new Action(
               () => lb_stat.Text = ""
               )
               );
                enableAllButton();
                MessageBox.Show("Detail view is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_bar.Maximum/3
                    ));
            if (!getInfo2(ad))
            {
                lb_stat.Invoke(new Action(
               () => lb_stat.Text = ""
               )
               );
                enableAllButton();
                MessageBox.Show("Detail view is fail.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            pg_bar.Invoke(new Action(
                    () => pg_bar.Value = pg_bar.Maximum
                    ));
            tb_title.Invoke(new Action(
               () => tb_title.Text = ad.Title
               )
               );
            tb_category.Invoke(new Action(
               () => tb_category.Text = ad.Category
               )
               );
      
            tb_description.Invoke(new Action(
               () => tb_description.Text = ad.Description.Replace(@"\r\n","\r\n")
               )
               );
            c.Invoke(new Action(
               () => c.Text = ad.Price
               )
               );
            list_product.Invoke(new Action(
                () => list_product.ClearSelected()
                ));
            string[] arr = ad.Images.Split(',');
            int size = arr.Length;
            list_photo.Invoke(new Action(
               () => list_photo.Items.Clear()
               )
               );
            ad.imageIds.Clear();
            for (int i = 0; i < size; i++)
            {
                ad.imageIds.Add(arr[i]);
                list_photo.Invoke(new Action(
               () => list_photo.Items.Add(arr[i])
               )
               );
            }
            selectedPostedAd = ad;
            lb_stat.Invoke(new Action(
               () => lb_stat.Text = "Viewing Detail success!"
               )
               );
            enableAllButton();
        }

        public bool getInfo1(ProductList ad)
        {
            try
            {
                string id = ad.Id;
                string url = "https://my.gumtree.com/postad?advertId=" + id;
                Uri uri = new Uri(url);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "https://my.gumtree.com/manage/ads");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var result = httpClient.GetAsync(url).Result;
                result.EnsureSuccessStatusCode();
                
                string pat = @"<meta name=""twitter:url"" content=""(?<VAL>[^\""]*)""/>";
                string timepat = @"var dataLayer = \[{""time"":(?<VAL>[^\,]*)";
                string strContent = result.Content.ReadAsStringAsync().Result;
                Match postad_Id = Regex.Match(strContent, pat);
                Match time_match = Regex.Match(strContent, timepat);
                string infourl = postad_Id.Groups["VAL"].Value;
                string time_string = time_match.Groups["VAL"].Value;
                if (infourl.Equals(""))
                {
                    return false;
                }
                ad.edit_url = infourl;
                ad.edit_time = time_string;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool getInfo2(ProductList ad)
        {
            try
            {
                string time = (Int64.Parse(ad.edit_time) + 17631).ToString();
                string url = ad.edit_url + "?getFormData=true&_="+time;
                Uri uri = new Uri(url);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", ad.edit_url);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var result = httpClient.GetAsync(url).Result;
                result.EnsureSuccessStatusCode();
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_bar.Maximum / 3
                    ));
                string strContent = result.Content.ReadAsStringAsync().Result;
                string str = getDetailInfo(strContent, ad.edit_url);
                if (str.Equals(""))
                {
                    return false;
                }
                ad.sellertype = str.Contains("seller-type");

                string categorypat = @"""categoryId"":(?<VAL>[^\,]*),";
                string locationpat = @"""locationId"":(?<VAL>[^\,]*),";
                string postcodepat = @"""postcode"":""(?<VAL>[^\""]*)"",";
                string titlepat = @"""title"":""(?<VAL>[^\""]*)"",";
                string despat = @"""description"":""(?<VAL>[^\""]*)"",";
                string contactnamepat = @"""contactName"":""(?<VAL>[^\""]*)"",";
                string contactEmailpat = @"""contactEmail"":""(?<VAL>[^\""]*)"",";
                string imageIdspat = @"""imageIds"":\[(?<VAL>[^\]]*)]";
                string pricepat = @"""price"":""(?<VAL>[^\""]*)""";
                string mainpat = @"""mainImageId"":(?<VAL>[^\,]*),";
                ad.Category = Regex.Match(strContent, categorypat).Groups["VAL"].Value;
                ad.locationId = Regex.Match(strContent, locationpat).Groups["VAL"].Value;
                ad.postcode = Regex.Match(strContent, postcodepat).Groups["VAL"].Value;
                ad.Title = Regex.Match(strContent, titlepat).Groups["VAL"].Value;
                ad.Description = Regex.Match(strContent, despat).Groups["VAL"].Value;
                ad.contactname = Regex.Match(strContent, contactnamepat).Groups["VAL"].Value;
                ad.username = Regex.Match(strContent, contactEmailpat).Groups["VAL"].Value;
                ad.Images = Regex.Match(strContent, imageIdspat).Groups["VAL"].Value;
                ad.Price = Regex.Match(strContent, pricepat).Groups["VAL"].Value;
                ad.mainImage = Regex.Match(strContent, mainpat).Groups["VAL"].Value;
                pg_bar.Invoke(new Action(
                    () => pg_bar.Value += pg_bar.Maximum / 3
                    ));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public string getDetailInfo(string str,string url)
        {
            try
            {
                
                
                Uri uri = new Uri(url);
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = new CookieContainer();
                for (int i = 0; i < initCookieValue.Count; i++)
                {
                    handler.CookieContainer.Add(uri, new Cookie(initCookieName[i], initCookieValue[i]));
                }
                HttpClient httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", url);
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Origin", "https://my.gumtree.com");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "my.gumtree.com");
                var result = httpClient.PostAsync(url, new StringContent(str, System.Text.Encoding.UTF8, "application/json")).Result;
                result.EnsureSuccessStatusCode();

                string strContent = result.Content.ReadAsStringAsync().Result;

                return strContent;
            }
            catch
            {
                return "";
            }
        }

        private void disableAllButton()
        {
            list_posted.Invoke(new Action(
                () => list_posted.Enabled = false
                ));
            bt_deleteAd.Invoke(new Action(
                () => bt_deleteAd.Enabled = false
                ));
            button1.Invoke(new Action(
               () => button1.Enabled = false
               ));
            button2.Invoke(new Action(
               () => button2.Enabled = false
               ));
            bt_post.Invoke(new Action(
                () => bt_post.Enabled = false
                ));
            bt_postall.Invoke(new Action(
                () => bt_postall.Enabled = false
                ));
            bt_view.Invoke(new Action(
                () => bt_view.Enabled = false
                ));
            bt_edit.Invoke(new Action(
                () => bt_edit.Enabled = false
                ));
            bt_delete.Invoke(new Action(
                () => bt_delete.Enabled = false
                ));
            bt_add.Invoke(new Action(
                () => bt_add.Enabled = false
                ));
        }

        private void enableAllButton()
        {
            list_posted.Invoke(new Action(
                () => list_posted.Enabled = true
                ));

            bt_deleteAd.Invoke(new Action(
                () => bt_deleteAd.Enabled = true
                ));
            button1.Invoke(new Action(
               () => button1.Enabled = true
               ));
            button2.Invoke(new Action(
               () => button2.Enabled = true
               ));
            bt_post.Invoke(new Action(
                () => bt_post.Enabled = true
                ));
            bt_postall.Invoke(new Action(
                () => bt_postall.Enabled = true
                ));
            bt_view.Invoke(new Action(
                () => bt_view.Enabled = true
                ));
            bt_edit.Invoke(new Action(
                () => bt_edit.Enabled = true
                ));
            bt_delete.Invoke(new Action(
                () => bt_delete.Enabled = true
                ));
            bt_add.Invoke(new Action(
                () => bt_add.Enabled = true
                ));
            
        }

        private void bt_relog_Click(object sender, EventArgs e)
        {
            loginflag = false;
            postall.Abort();
            enableAllButton();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            logForm.Dispose();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.R)
            {
                List<int> cnt = new List<int>();
                ChangePlistArray.Clear();
                foreach (Int32 i in GetRandomNumbers(0, list_product.Items.Count - 1))
                {
                    cnt.Add(i);
                }
                for (int i = 0; i < list_product.Items.Count; i++)
                {
                    ChangePlistArray.Add(PlistArray[cnt[i]]);
                }
                PlistArray.Clear();
                list_product.Items.Clear();
                for (int i = 0; i < ChangePlistArray.Count; i++)
                {
                    PlistArray.Add(ChangePlistArray[i]);
                    list_product.Items.Add(ChangePlistArray[i].Title);
                }
                //             list_product.ClearSelected();
                clean();
                writeXml();
            }
        }
        public static Int32[] GetRandomNumbers(Int32 start, Int32 end)
        {
            Int32 startIndex = start > end ? end : start;
            Int32 endIndex = start > end ? start : end;
            Int32 nCount = endIndex - startIndex + 1;
            Int32 nLoopCount = startIndex + endIndex + 1;
            List<Int32> numberList = new List<Int32>(nCount);
            List<Int32> resultList = new List<Int32>(nCount);
            for (Int32 i = startIndex; i < nLoopCount; i++)
                numberList.Add(i);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (numberList.Count > 0)
            {
                Random rGen = new Random((Int32)sw.ElapsedTicks + resultList.Count);

                Int32 pickedIndex = rGen.Next(0, numberList.Count);

                resultList.Add(numberList[pickedIndex]);
                numberList.RemoveAt(pickedIndex);
            }

            sw.Stop();
            return resultList.ToArray();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                Location = new Point(this.Left - (mousePoint.X - e.X),
                    this.Top - (mousePoint.Y - e.Y));
            }
        }
    }
}
