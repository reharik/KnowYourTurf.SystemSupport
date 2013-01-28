using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;

namespace SystemSupport.Web.Controllers
{
    using CC.Core.CoreViewModelAndDTOs;
    using CC.Core.DomainTools;
    using CC.Core.Services;

    using KnowYourTurf.Core.Config;

    //
    public class SiteConfigurationController : DCIController
    {
        private readonly ISelectListItemService _selectListItemService;
        private readonly string _configFilePath;
        private readonly string _suiteWebsiteFullPath;

        public SiteConfigurationController(ISelectListItemService selectListItemService)
        {
            _selectListItemService = selectListItemService;
            _suiteWebsiteFullPath = SiteConfig.Settings().SuiteSiteFullPath ?? "";
            _configFilePath = getConfigFilePath();
        }

        public ActionResult AddUpdate(ViewModel input)
        {
            //TODO if file doesn't exist, send back "file doesn't exist notification
            //get current setting value

            string suiteStatus = getSuiteStatus();

            var suiteOnlineStatusTypes =
                _selectListItemService.CreateList(SuiteOnlineStatusType.GetAll<SuiteOnlineStatusType>(),
                                                  z => z.Key,
                                                  z => z.Key,
                                                  false);
            var model = new SiteConfigurationViewModel
                            {
                                Title = WebLocalizationKeys.SITE_CONFIGURATION.ToString(),
                                SuiteOnlineStatusType = suiteStatus,
                                SuiteConfigFile = _configFilePath,
                                SuiteSiteFullPath = _suiteWebsiteFullPath,

                                SuiteOnlineStatusTypeList = suiteOnlineStatusTypes

                                //TODO ...show other settings as Readonly
                            };

            return View(model);
        }

        private string getConfigFilePath()
        {
            return _suiteWebsiteFullPath + "web.config";
        }

        private string getSuiteStatus()
        {
            var suiteStatus = getAppSettingKeyValue(_configFilePath, "Suite.OnlineStatus");
            if (suiteStatus == "")
            {
                return SuiteOnlineStatusType.Online.ToString();
            }
            else
            {
                return suiteStatus;
            }
        }

        private void setAppSettingsKeyValue(string configFilePath,
                                            string keyName,
                                            string newKeyValue)
        {

            if (_suiteWebsiteFullPath == "")
            {
                throw new Exception(WebLocalizationKeys.INVALID_CONFIG_FILE.ToString());
            }

            var webConfigFile = XDocument.Load(configFilePath);

            XAttribute keyAttribute = new XAttribute("key", keyName);
            XAttribute valueAttribute = new XAttribute("value", newKeyValue);

            var appSettingsSection = webConfigFile.Descendants()
                .Where(z => z.Name.ToString().ToUpper() == "APPSETTINGS")
                .FirstOrDefault();

            if (appSettingsSection == null)
            {
                throw new Exception(WebLocalizationKeys.INVALID_CONFIG_FILE.ToString() + ":" + configFilePath);
            }

            //Test if exists
            var appOfflineSetting = getAppSetting(keyName, appSettingsSection);

            if (appOfflineSetting == null)
            {
                appSettingsSection.Add(new XElement("add",
                                                    keyAttribute,
                                                    valueAttribute));

                appOfflineSetting = getAppSetting(keyName, appSettingsSection);
            }

            var valueFromSetting = appOfflineSetting.Attributes()
                .Where(z => z.Name.ToString().ToUpper() == "VALUE")
                .FirstOrDefault();

            valueFromSetting.Value = newKeyValue;

            //OVERWRITE original web.config
            webConfigFile.Save(configFilePath);
        }

        private string  getAppSettingKeyValue(string configFilePath, string keyName)
        {
            if (_suiteWebsiteFullPath == "")
            {
                throw new Exception(WebLocalizationKeys.INVALID_CONFIG_FILE.ToString());
            }

            var webConfigFile = XDocument.Load(configFilePath);

            var appSettingsSection = webConfigFile.Descendants()
                .Where(z => z.Name.ToString().ToUpper() == "APPSETTINGS")
                .FirstOrDefault();

            if (appSettingsSection == null)
            {
                throw new Exception(WebLocalizationKeys.INVALID_CONFIG_FILE.ToString() + ":" + configFilePath);
            }

            //Test if exists
            var appOfflineSetting = getAppSetting(keyName, appSettingsSection);

            if (appOfflineSetting == null)
            {
                return "";
            }

            //check if null, else create new xElement

            var settingAttribute = appOfflineSetting.Attributes()
                .Where(z => z.Name.ToString().ToUpper() == "VALUE")
                .FirstOrDefault();

            //TODO: simplify 
            string valueFromSetting = "";
            if (settingAttribute != null)
                valueFromSetting = settingAttribute.Value.ToUpper();

            var status = "";

            switch (valueFromSetting)
            {
                case "OFFLINE":
                    status = SuiteOnlineStatusType.Offline.ToString();
                    break;

                case "TESTING":
                    status = SuiteOnlineStatusType.Testing.ToString();
                    break;

                default:
                    status = SuiteOnlineStatusType.Online.ToString();
                    break;
            }
            return status;
        }

        private static XElement getAppSetting(string keyName, XElement appSettingsSection)
        {
            return appSettingsSection.Elements()
                .Where(z => z.Attributes()
                                .Any(a => a.Name.ToString().ToUpper() == "KEY"
                                          && a.Value.ToUpper() == keyName.ToUpper()
                                ) == true
                ).FirstOrDefault();
        }

        public JsonResult Save(SiteConfigurationViewModel input)
        {
            var suiteOnlineStatus = input.SuiteOnlineStatusType.ToUpper();

            Notification notification = new Notification();

            switch (suiteOnlineStatus)
            {
                case "OFFLINE":
                    //SuiteOnlineStatusType.Offline
                    
                    var copyResult = disableSuiteWebsite();
                    if (copyResult  != "")
                    {
                        notification.Success = false;
                        notification.Message = copyResult;
                    }
                    else
                    {
                        setAppSettingsKeyValue(_configFilePath, "Suite.OnlineStatus", suiteOnlineStatus);
                        notification.Success = true;
                        notification.Message = "Suite Website Status Changed to : " + suiteOnlineStatus;
                    }
                    
                    break;

                case "TESTING":
                    //SuiteOnlineStatusType.Testing
                    var deleteResult= removeAppOfflineFile();
                    if(deleteResult != "")
                    {
                        notification.Success = false;
                        notification.Message = deleteResult;
                    }
                    else
                    {
                        setAppSettingsKeyValue(_configFilePath, "Suite.OnlineStatus", suiteOnlineStatus);
                        notification.Success = true;
                        notification.Message = "Suite Website Status Changed to: " + suiteOnlineStatus;    
                    }
                    break;

                default:
                    //assume ONLINE
                    suiteOnlineStatus = "ONLINE";

                    var deleteResult2= removeAppOfflineFile();
                    if (deleteResult2 != "")
                    {
                        notification.Success = false;
                        notification.Message = deleteResult2;
                    }
                    else
                    {
                        setAppSettingsKeyValue(_configFilePath, "Suite.OnlineStatus", suiteOnlineStatus);
                        notification.Success = true;
                        notification.Message = "Suite Website Status Changed to: " + suiteOnlineStatus;
                    }
                    
                    //TODO: add requesting URL to "Retry" link
                    
                    break;
            }

            return Json(notification);
        }

        public bool DoesFileExist(string sourceFileFullPath)
        {
            if (!System.IO.File.Exists(sourceFileFullPath))
                return false;
            else
                return true;
        }

        private string disableSuiteWebsite()
        {
            //copy the template file so that IIS will automatically disable the site.
            if(!System.IO.Directory.Exists(_suiteWebsiteFullPath))
            {
                //suite website missing !
                return "Suite website directory not found: " + _suiteWebsiteFullPath;
            }

            string sourceFileFullPath = _suiteWebsiteFullPath + "_App_Offline_Template.htm";
            string targetFileFullPath = _suiteWebsiteFullPath + "App_Offline.htm";

            if (System.IO.File.Exists(sourceFileFullPath) == false)
	        {
                return "Template not found:" + sourceFileFullPath; //we want a NULL value in new database column
	        }
	
	        return TryCopySingleFile(sourceFileFullPath, targetFileFullPath);
        }

        private string TryCopySingleFile(string sourceFileFullPath, string targetFileFullPath)
        {
            try
            {
                //copy original
                System.IO.File.Copy(sourceFileFullPath, targetFileFullPath,true);

                if (System.IO.File.Exists(targetFileFullPath) == false)
                    return "File does not exist after COPY: "+ targetFileFullPath;

                var originalOldSize = new FileInfo(sourceFileFullPath).Length;
                var originalNewSize = new FileInfo(targetFileFullPath).Length;

                if ((originalOldSize != 0 && originalNewSize == 0) || (originalOldSize != originalNewSize)) 
                    return "Size mismatch after copy.";
            }
            catch(Exception ex)
            {
                //something failed while trying to copy file
                return "Error during copy:" + ex.Message;
            }
            return ""; //if no errors
        }

        private string removeAppOfflineFile()
        {

            string fileToDeleteFullPath = _suiteWebsiteFullPath + "App_Offline.htm";

            if (System.IO.File.Exists(fileToDeleteFullPath) == false)
            {
                return ""; 
            }

            try
            {
                //copy original
                System.IO.File.Delete(fileToDeleteFullPath);

                if (System.IO.File.Exists(fileToDeleteFullPath) == true)
                {
                    return "Could not delete file: " + fileToDeleteFullPath;
                }
                return "";
            }
            catch (Exception ex)
            {
                //something failed while trying to copy file
                return "Error during copy:" + ex.Message;
            }
        }
    }

    public class SiteConfigurationViewModel : ViewModel
    {

        //TODO: extract all other settings from the JSON string


        public string Name { get; set; }
        public string Host { get; set; }
        public string DCIUrl { get; set; }
        public string LanguageDefault { get; set; }
        public string ScriptsPath { get; set; }
        public string CssPath { get; set; }
        public string ImagesPath { get; set; }
        public string WebSiteRoot { get; set; }
        public string B2CClient { get; set; }
        public string NSNAPassword { get; set; }
        public string SuiteSiteFullPath { get; set; }

        public string SuiteOnlineStatusType { get; set; } // **************************************
        public string SuiteConfigFile { get; set; } // **************************************

        public IEnumerable<SelectListItem> SuiteOnlineStatusTypeList { get; set; }
    }
}

