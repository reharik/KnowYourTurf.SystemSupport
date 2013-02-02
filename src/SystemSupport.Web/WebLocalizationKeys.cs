namespace SystemSupport.Web
{
    using CC.Core.Localization;

    public class WebLocalizationKeys : StringToken
    {
        protected WebLocalizationKeys(string key)
            : this(key, null)
        {
        }

        protected WebLocalizationKeys(string key, string default_EN_US_Text)
            : base(key, default_EN_US_Text)
        {

        }
        public static readonly StringToken INVALID_USERNAME_OR_PASSWORD = new WebLocalizationKeys("INVALID_USERNAME_OR_PASSWORD", "Invalid Username or Password");
        public static readonly StringToken SIGN_IN = new WebLocalizationKeys("SIGN_IN", "Sign In");
        public static readonly StringToken SYSTEM_SUPPORT_SIGN_IN = new WebLocalizationKeys("SYSTEM_SUPPORT_SIGN_IN ", "System Support Sign In");
        public static readonly StringToken LOGIN_SYSTEM_SUPPORT = new WebLocalizationKeys("LOGIN_SYSTEM_SUPPORT", "Login - System Support");
        public static readonly StringToken LOG_OUT = new WebLocalizationKeys("LOG_OUT", "Logout");
        public static readonly StringToken USERS = new WebLocalizationKeys("USERS", "Users");
        public static readonly StringToken DELETE_ITEM = new WebLocalizationKeys("DELETE_ITEM", "Delete this item!");
        public static readonly StringToken EDIT_ITEM = new WebLocalizationKeys("EDIT_ITEM", "Edit this item!");
        public static readonly StringToken DISPLAY_ITEM = new WebLocalizationKeys("DISPLAY_ITEM", "Display this item!");
        public static readonly StringToken ADD_NEW_USER = new WebLocalizationKeys("ADD_NEW_USER", "Add New User");
        public static readonly StringToken USER = new WebLocalizationKeys("USER", "User");
        public static readonly StringToken ADD_NEW = new WebLocalizationKeys("ADD_NEW_USER", "Add New");

        public static readonly StringToken SAVE = new WebLocalizationKeys("SAVE", "Save");
        public static readonly StringToken CANCEL = new WebLocalizationKeys("CANCEL", "Cancel");
        public static readonly StringToken PERSONAL_INFORMATION = new WebLocalizationKeys("PERSONAL_INFORMATION", "Personal Information");
        public static readonly StringToken INITIAL = new WebLocalizationKeys("INITIAL", "Initial");
        public static readonly StringToken PASSWORD = new WebLocalizationKeys("PASSWORD", "Password");
        public static readonly StringToken ADDRESSES = new WebLocalizationKeys("ADDRESSES", "Addresses");
        public static readonly StringToken EMAIL_ADDRESSES = new WebLocalizationKeys("EMAIL_ADDRESSES", "Emails");
        public static readonly StringToken PHONE_NUMBERS = new WebLocalizationKeys("PHONE_NUMBERS", "Phone Numbers");
        public static readonly StringToken ADD_THIS_ITEM = new WebLocalizationKeys("ADD_THIS_ITEM", "Add This Item");
        public static readonly StringToken CLOSE_BUTTON_TITLE = new WebLocalizationKeys("CLOSE_BUTTON_TITLE", "Close");
        public static readonly StringToken DEFAULT = new WebLocalizationKeys("DEFAULT", "Default");
        public static readonly StringToken CHOOSE_CLIENT = new WebLocalizationKeys("CHOOSE_CLIENT", "Choose Client");
        public static readonly StringToken LOGIN_INFORMATION = new WebLocalizationKeys("LOGIN_INFORMATION", "Login Information");

        public static readonly StringToken TENANT = new WebLocalizationKeys("TENANT", "Tenant");
        public static readonly StringToken TENANTS = new WebLocalizationKeys("TENANTS", "Tenants");

        public static readonly StringToken CLIENT = new WebLocalizationKeys("CLIENT", "CLIENT");
        public static readonly StringToken Clients = new WebLocalizationKeys("Clients", "Clients");
        public static readonly StringToken REQUIRED = new WebLocalizationKeys("REQUIRED", "Required");
        public static readonly StringToken SUBSCRIPTION_INFORMATION = new WebLocalizationKeys("SUBSCRIPTION_INFORMATION", "Subscription Information");
        public static readonly StringToken VIEW_SUBSCRIPTION_HISTORY = new WebLocalizationKeys("VIEW_SUBSCRIPTION_HISTORY", "View Subscription History");
        public static readonly StringToken HIDE_SUBSCRIPTION_HISTORY = new WebLocalizationKeys("HIDE_SUBSCRIPTION_HISTORY", "Hide Subscription History");
        public static readonly StringToken USER_SUBSCRIPTION_HISTORY = new WebLocalizationKeys("USER_SUBSCRIPTION_HISTORY", "User Subscription History");
        public static readonly StringToken USER_SUBSCRIPTION_INFORMATION = new WebLocalizationKeys("USER_SUBSCRIPTION_INFORMATION", "User Subscription Information");
        
        public static readonly StringToken BEGINNING_DATE = new WebLocalizationKeys("BEGINNING_DATE", "Beginning Date");
        public static readonly StringToken EXPIRATION_DATE = new WebLocalizationKeys("EXPIRATION_DATE", "Expiration Date");
        public static readonly StringToken PROMOTION = new WebLocalizationKeys("PROMOTION", "Promotion Code");
        public static readonly StringToken PROMOTIONS = new WebLocalizationKeys("PROMOTIONS", "Promotions");
        public static readonly StringToken MONTH_BASE_PROMOTION = new WebLocalizationKeys("MONTH_BASE_PROMOTION", "Month Based Promotion");
        public static readonly StringToken PERCENTAGE_BASE_PROMOTION = new WebLocalizationKeys("PERCENTAGE_BASE_PROMOTION", "Percentage Based Promotion");
        public static readonly StringToken PENDING_CHARGES = new WebLocalizationKeys("PENDING_CHARGES", "Pending Charges");
        public static readonly StringToken CHARGE = new WebLocalizationKeys("CHARGE", "Charge");
        public static readonly StringToken VOID = new WebLocalizationKeys("VOID", "Void");
        public static readonly StringToken RETURN = new WebLocalizationKeys("RETURN", "Return");
        public static readonly StringToken LOG_IN = new WebLocalizationKeys("LOG_IN", "Log In");
        public static readonly StringToken ERROR_UNEXPECTED = new WebLocalizationKeys("ERROR_UNEXPECTED", "Sorry, an unexpected error occurred while processing your request.");
        public static readonly StringToken NEW = new WebLocalizationKeys("NEW", "New ");
        public static readonly StringToken DELETE_ITEMS = new WebLocalizationKeys("DELETE_ITEM", "Delete selected items");
        public static readonly StringToken ADD_A_NEW_ = new WebLocalizationKeys("ADD_A_NEW_", "Add A New {0}");
        public static readonly StringToken PHONE = new WebLocalizationKeys("PHONE", "Phone");
        public static readonly StringToken ADDRESS = new WebLocalizationKeys("ADDRESS", "Address");
        public static readonly StringToken EMAIL = new WebLocalizationKeys("EMAIL", "Email");
        public static readonly StringToken ACCOUNT_INFORMATION = new WebLocalizationKeys("ACCOUNT_INFORMATION", "Account Information");
        public static readonly StringToken ADDRESS1 = new WebLocalizationKeys("ADDRESS1", "Address Line 1");
        public static readonly StringToken ADDRESS2 = new WebLocalizationKeys("ADDRESS2", "Address Line 2");

        public static readonly StringToken EMAIL_TEMPLATES = new WebLocalizationKeys("EMAIL_TEMPLATES", "Email Templates");
        public static readonly StringToken EMAIL_TEMPLATE = new WebLocalizationKeys("EMAIL_TEMPLATE", "Email Template");
        public static readonly StringToken DONT_CHANGE_EMAIL_TOKENS = new WebLocalizationKeys("DONT_CHANGE_EMAIL_TOKENS", "Please don't change the Email Tokens unless you have updated the code!");
        public static readonly StringToken CLIENT_NAME = new WebLocalizationKeys("CLIENT_NAME", "Client Name");
        public static readonly StringToken START_DATE = new WebLocalizationKeys("START_DATE", "Start Date");
        public static readonly StringToken END_DATE = new WebLocalizationKeys("END_DATE", "End Date");
        public static readonly StringToken FILTER = new WebLocalizationKeys("FILTER", "Filter");
        public static readonly StringToken PERMISSIONS = new WebLocalizationKeys("PERMISSIONS", "Permissions");
        public static readonly StringToken SHOW_PERMISSIONS = new WebLocalizationKeys("SHOW_PERMISSIONS", "Show Permissions");
        public static readonly StringToken PERMISSIONS_FOR = new WebLocalizationKeys("PERMISSIONS_FOR", "Permissions For {0}");
        public static readonly StringToken USER_GROUP = new WebLocalizationKeys("USER_GROUP", "User Group");
        public static readonly StringToken USER_GROUPS = new WebLocalizationKeys("USER_GROUPS", "User Groups");
        public static readonly StringToken USER_GROUP_NAME = new WebLocalizationKeys("USER_GROUP_NAME", "User Group Name");
        public static readonly StringToken PERMISSION_USER_GROUPS = new WebLocalizationKeys("PERMISSION_USER_GROUPS", "Permission User Groups");
        public static readonly StringToken SYSTEM_OFFLINE = new WebLocalizationKeys("SYSTEM_OFFLINE", "System Offline");
        public static readonly StringToken SITE_CONFIGURATION = new WebLocalizationKeys("SITE_CONFIGURATION", "Site Configuration");
        public static readonly StringToken INVALID_CONFIG_FILE = new WebLocalizationKeys("INVALID_CONFIG_FILE", "Invalid config file path");
        public static readonly StringToken TO_PRESENT = new WebLocalizationKeys("TO_PRESENT", "To Present");
        public static readonly StringToken UNKNOWN = new WebLocalizationKeys("UNKNOWN", "Unknown");


        public static readonly StringToken EMPLOYEE_PHOTO = new WebLocalizationKeys("EMPLOYEE_PHOTO", "Employee Photo");
        public static readonly StringToken STATUS = new WebLocalizationKeys("STATUS", "Status");
        public static readonly StringToken EMPLOYEE_INFORMATION = new WebLocalizationKeys("EMPLOYEE_INFORMATION", "Employee Information");
        public static readonly StringToken FORGOT_YOUR_PASSWORD = new WebLocalizationKeys("FORGOT_YOUR_PASSWORD", "Forgot your password?");
    
    
    }
}