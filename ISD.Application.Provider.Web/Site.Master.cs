using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ISD.DA;
using ISD.EDS;
using ISD.Util;
using Microsoft.AspNet.Identity;

namespace ISD.Provider.Web
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                SetLinkTarget();
                if (Context.User.Identity.IsAuthenticated)
                {
                    var loginName = HeadLoginView.FindControl("HeadLoginName") as LoginName;
                    String UserID = Context.User.Identity.GetUserId();

                    if (!string.IsNullOrEmpty((string)Session[SystemConstants.ses_FName]))
                    {
                        loginName.FormatString = (string)Session[SystemConstants.ses_FName];
                    }
                    else
                    {
                        var drp = new DataAccessComponent().RetrieveProviderProfiles(UserID);
                        if (drp != null)
                        {
                            string name = drp.ProviderName;

                            if (name.Length > 20)
                            {
                                name = name.Remove(20);
                                name = name + "...";
                            }
                            Session[SystemConstants.ses_FName] = name;
                            Session[SystemConstants.ses_LName] = "";
                            Session[SystemConstants.ses_Role] = SystemConstants.ProviderRole;
                            Session[SystemConstants.ses_Email] = drp.Email;
                        }
                        else
                        {
                            var dr = new DataAccessComponent().RetrieveUserProfiles(UserID);
                            if (dr != null)
                            {

                                //    Session[SystemConstants.ses_FName] = dr.FirstName;
                                //    Session[SystemConstants.ses_LName] = dr.LastName;
                                //    Session[SystemConstants.ses_Role] = SystemConstants.CustomerRole;
                                //    Session[SystemConstants.ses_Email] = dr.Email;
                            }
                        }
                    }
                    //loginName.FormatString = (string)Session[SystemConstants.ses_FName] + " " + (string)Session[SystemConstants.ses_LName];
                }

            }
        }

        private void SetLinkTarget()
        {
            DataSetComponent.v_MenuDataTable dt = new DataAccessComponent().RetrieveMenuExplorers((int)SystemConstants.MenuType.ProviderMenu);

            int x = 0;
            foreach (DataSetComponent.v_MenuRow drParent in dt)
            {


                if (drParent.LinkType == 1)
                {
                    x++;
                    if (drParent.ParentMenuID == 0 && drParent.LinkText != null)
                    {
                        MenuItem menu = new MenuItem(drParent.LinkText, x.ToString(), "", "~/Activity/Default.aspx?" + SystemConstants.CategoryID + "=" + drParent.LinkValue, "");
                        MenuNavigation.Items.Add(menu);
                        foreach (DataSetComponent.v_MenuRow drChild in dt)
                        {
                            if (drChild.ParentMenuID == drParent.ID && drChild.LinkText != null)
                            {
                                MenuItem ChildMenu = new MenuItem(drChild.LinkText, "", "", "~/Activity/Default.aspx?" + SystemConstants.CategoryID + "=" + drChild.LinkValue, "");
                                menu.ChildItems.Add(ChildMenu);
                            }
                        }
                    }
                }
                else if (drParent.LinkType == 2)
                {
                    x++;
                    if (drParent.ParentMenuID == 0 && drParent.LinkText != null)
                    {
                        MenuItem menu = new MenuItem(drParent.LinkText, x.ToString(), "", "~/Activity/Default.aspx?" + SystemConstants.ProviderID + "=" + drParent.LinkValue, "");
                        MenuNavigation.Items.Add(menu);
                        foreach (DataSetComponent.v_MenuRow drChild in dt)
                        {
                            if (drChild.ParentMenuID == drParent.ID && drChild.LinkText != null)
                            {
                                MenuItem ChildMenu = new MenuItem(drChild.LinkText, "", "", "~/Activity/Default.aspx?" + SystemConstants.ProviderID + "=" + drChild.LinkValue, "");
                                menu.ChildItems.Add(ChildMenu);
                            }
                        }
                    }
                }
                else if (drParent.LinkType == 3)
                {
                    x++;
                    if (drParent.ParentMenuID == 0)
                    {
                        MenuItem menu = new MenuItem(drParent.LinkText, x.ToString(), "", "~/Activity/Default.aspx??" + SystemConstants.ActivityID + "=" + drParent.LinkValue, "");
                        MenuNavigation.Items.Add(menu);
                        foreach (DataSetComponent.v_MenuRow drChild in dt)
                        {
                            if (drChild.ParentMenuID == drParent.ID)
                            {
                                MenuItem ChildMenu = new MenuItem(drChild.LinkText, "", "", "~/Activity/Default.aspx?" + SystemConstants.ActivityID + "=" + drChild.LinkValue, " ");
                                menu.ChildItems.Add(ChildMenu);
                            }
                        }
                    }
                }

                else if (drParent.LinkType == 4)
                {
                    x++;
                    if (drParent.ParentMenuID == 0)
                    {
                        MenuItem menu = new MenuItem(drParent.LinkText, x.ToString(), "", "~/Pages/" + drParent.LinkValue, "");
                        MenuNavigation.Items.Add(menu);
                        foreach (DataSetComponent.v_MenuRow drChild in dt)
                        {
                            if (drChild.ParentMenuID == drParent.ID)
                            {
                                MenuItem ChildMenu = new MenuItem(drChild.LinkText, "", "", "~/Pages/" + drChild.LinkValue, "");
                                menu.ChildItems.Add(ChildMenu);
                            }
                        }

                    }
                }
                else if (drParent.LinkType == 5)
                {
                    x++;
                    if (drParent.ParentMenuID == 0)
                    {
                        MenuItem menu = new MenuItem(drParent.LinkText, x.ToString(), "", "~/Pages/" + drParent.LinkValue, "");
                        MenuNavigation.Items.Add(menu);
                        MenuNavigation.Items.Add(menu);
                        foreach (DataSetComponent.v_MenuRow drChild in dt)
                        {
                            if (drChild.ParentMenuID == drParent.ID)
                            {
                                MenuItem ChildMenu = new MenuItem(drChild.LinkText, "", "", "~/Pages/" + drChild.LinkValue, "");
                                menu.ChildItems.Add(ChildMenu);
                            }
                        }

                    }
                }
            }
            MenuItem ContactUs = new MenuItem("Contact Us", x.ToString(), "", "~/ContactUs");
            MenuNavigation.Items.Add(ContactUs);
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

       
    }

}