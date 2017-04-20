﻿using BudgetControl.DAL;
using BudgetControl.Models;
using BudgetControl.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Sessions
{
    public static class AuthManager
    {

        #region Login
        public static User Login(LoginViewModel logindata)
        {
            User user;

            try
            {
                // 1. Query user data.
                using (var userRepo = new UserRepository())
                {
                    user = userRepo.Get()
                        .Where(
                            u => 
                                u.UserName == logindata.Username.Trim() 
                            )
                        .FirstOrDefault();
                }

                // 2. Throw exception if user is invalid.
                if (user == null)
                {
                    throw new Exception("ไม่พบรหัสพนักงานนี้ กรุณาติดต่อผู้ดูแลระบบ");
                }

                if (user.Password != logindata.Password)
                {
                    throw new Exception("รหัสผ่านไม่ถูกต้อง");
                }
                
                // 3. Generate Token
                user.Token = Guid.NewGuid();
                user.ExpireDate = DateTime.Today.AddMonths(1);

                // 4. Update to database
                using (var userRepo = new UserRepository())
                {
                    userRepo.Update(user);
                }

                // 5. Register Cookies
                RegisterCookies(user);

                // 6. Login Complete
                AfterLoginComplete(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }

        public static void LoginByToken()
        {
            try
            {
                User user;
                // 1. Get token data from cookies
                string username = GetCookie(CK_USERNAME).Value.ToString().Trim();
                Guid token = new Guid(GetCookie(CK_TOKEN).Value.ToString());

                // 2. Query user data by token
                using (var userRepo = new UserRepository())
                {
                    user = userRepo.Get().Where(
                        u =>
                            u.UserName == username &&
                            u.Token == token)
                        .FirstOrDefault();
                }

                // 3. 
                if (user == null)
                {
                    throw new Exception();
                }

                // 4. Login Complete
                AfterLoginComplete(user);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AfterLoginComplete(User user)
        {
            RegisterSession(user);
        }

        #endregion

        #region Registered Session

        public static void RegisterSession(User user)
        {
            try
            {
                SessionItems session = new SessionItems(user);
                HttpContext.Current.Session["Authentication"] = session;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SessionItems GetAuthentication()
        {
            var authen = (SessionItems)HttpContext.Current.Session["Authentication"];
            if (authen == null)
            {
                LoginByToken();
                authen = (SessionItems)HttpContext.Current.Session["Authentication"]; 
                if(authen == null)
                {
                    throw new Exception("กรุณาเข้าสู่ระบบ");
                }
                return authen;
            }
            else
            {
                return authen;
            }
        }

        public static UserRole? GetRole()
        {
            try
            {
                return GetAuthentication().Role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static User GetCurrentUser()
        {
            try
            {
                return GetAuthentication().User;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CostCenter GetWorkingCostCenter()
        {
            try
            {
                return GetAuthentication().WorkingCostCenter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Registered Cookies

        // cookies name
        private const string CK_USERNAME = "_username";
        private const string CK_TOKEN = "_token";

        public static void RegisterCookies(User user)
        {
            CreateCookie(CK_USERNAME, user.UserName);
            CreateCookie(CK_TOKEN, user.Token.ToString());
        }

        public static void CreateCookie(string cookieName, string value)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie.Value = value;
            myCookie.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Current.Response.Cookies.Add(myCookie);
        }

        public static HttpCookie GetCookie(string cookieName)
        {
            HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
            HttpCookie myCookie = cookies.Get(cookieName);
            return myCookie;
        }

        public static void RemoveCookie(string cookieName)
        {
            HttpCookie cookie = GetCookie(cookieName);
            if (cookie != null)
            {
                cookie.Expires = DateTime.Today.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }


        #endregion

        #region Logout

        public static void Logout()
        {
            ClearCookie();
            ClearSession();
        }

        public static void ClearSession()
        {
            HttpContext.Current.Session.Clear();
        }

        public static void ClearCookie()
        {
            RemoveCookie(CK_TOKEN);
        }

        #endregion


    }
}