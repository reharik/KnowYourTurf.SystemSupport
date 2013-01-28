using System;
using System.Linq;
using DecisionCritical.Core.Domain;
using DecisionCritical.Core.Enumerations;

namespace SystemSupport.Test
{
    public static class ObjectMother
    {
              public static PortfolioItem ValidPortfolioItem_AcademicCourse(string name)
        {
            return new PortfolioItem
                       {
                           FriendlyName = name,
                           Description = "A Portfolio Item",
                           TypeName = "AcademicCourse"
                       };
        }

        public static Contract ValidContract(string name)
        {
            return new Contract
                       {
                           Name = name,
                           Description = "Nursing contract",
                           Date = DateTime.Parse("1/1/1999"),
                           Amount = 1000,
                           AwardedBy = "John Ghatti",
                           AssetType = new AssetType { Name = "Contract" }
                       };
        }

        public static SoftwareDevelopment ValidSoftware(string name)
        {
            return new SoftwareDevelopment
            {
                Name = name,
                Description = "used the mouse",
                Version = "1.00",
                Platform = "Windows",
                DevelopmentDate = DateTime.Parse("1/1/1999"),
                AssetType = new AssetType { Name = "Software" }
            };
        }
        
        public static Fellowship ValidFellowship(string name)
        {
            return new Fellowship()
            {
                Name = name,
                Provider = "Robs Mom",
                Description = "Good Fellowship",
                AssetType = new AssetType { Name = "Fellowship" }
               
            };
        }

        public static ContinuingEducation ValidContinuingEducation(string name)
        {
            return new ContinuingEducation()
                       {
                           Name = name,
                           CourseSubtitle = "Some Big Course",
                           AssetType = new AssetType { Name = "ContinuingEducation" }
                       };
        }

        public static Consulting ValidConsultingActivity(string name)
        {
            return new Consulting()
                       {
                           Name = name,
                           Institution = "Bear Stearns",
                           City = "New York",
                           State = "NY",
                           Accomplishments = "Series 7",
                           From = DateTime.Parse("1/5/1999"),
                           To = DateTime.Parse("1/5/2001"),
                           IsToPresent = true,
                           Description = "Descr",
                           AssetType = new AssetType { Name = "ConsultingActivity" }

                       };
        }

        public static Membership ValidMembership(string name)
        {
            return new Membership()
                       {
                           Name = name,
                           AssetType = new AssetType { Name = "Membership" }
                       };
        }

        public static CommunityService ValidCommunityService(string name)
        {
            return new CommunityService()
                       {
                           Name = name,
                           Project = "Project",
                           AssetType = new AssetType { Name = "CommunityService" }
                       };
        }

        public static Honor ValidHonor(string name)
        {
            return new Honor()
                       {
                           Name = name,
                           AssetType = new AssetType { Name = "Honor" }
                       };
        }

        public static Training ValidTraining(string name)
        {
            return new Training()
                       {
                           Name = name,
                           AssetType = new AssetType { Name = "Training" }
                       };
        }

        public static ClinicalExperience ValidClinicalExperience(string name)
        {
            return new ClinicalExperience()
                       {
                           IsCurrent = true,
                           AssetType = new AssetType { Name = "ClinicalExperience" }
                       };
        }

        public static Presentation ValidPresentation(string name)
        {
            return new Presentation()
                       {
                           Name = "title",
                           AssetType = new AssetType { Name = "Presentation" }
                       };
        }

        public static Committee ValidCommittee(string name)
        {
            return new Committee()
                       {
                           Name = "title",
                           ThesisTitle = "Thesis Tit",
                           StudentName = "S Name",
                           InstitutionName = "school for the mentally disabled",
                           Employer = "Jamiqua",
                           Role = "Grandmaster",
                           Accomplishments= "cage fighting champ",
                           From = DateTime.Parse("1/5/1999"),
                           To = DateTime.Parse("1/5/2001"),
                           AssetType = new AssetType { Name = "Committee" }
                       };
        }

        public static Publication ValidPublication(string name)
        {
            return new Publication()
                       {
                           Name = "title",
                           AssetType = new AssetType { Name = "Publication" }
                           
                       };
        }


        public static AcademicRoleType ValidAcademicRoleType(string name)
        {
            return new AcademicRoleType
                       {
                           Name = name,
                           Sequence = "12"
                       };
        }

        public static TeachingExperience ValidTeachingExperience(string name)
        {
            return new TeachingExperience
                       {
                           Organization = "SomeClient",
                           City = "Austin",
                           State = "TX",
                           Description = "Description of Teaching Experience",
                           From = DateTime.Parse("1/5/1999"),
                           To = DateTime.Parse("1/5/2001"),
                           IsToPresent = false,
                           AssetType = new AssetType { Name = "TeachingExperience" }
                       };
        }

        public static FundedActivity ValidFundedActivity(string name)
        {
            return new FundedActivity
                       {
                           Name = name,
                           Amount = 100,
                           AwardedBy = "Award Host",
                           Description = "Description of Teaching Experience",
                           Reviewer = "Bob",
                           AssetType = new AssetType { Name = "FundedActivity" }
                       };
        }

        public static User ValidUser(string name)
        {
            var user = new User
                           {
                               FirstName = name,
                               LastName = "Harik",

                               UserId = "Some stupid org based id"
                           };
            user.AddAddress(ValidAddress("home").WithEntityId(1));
            user.AddEmail(ValidEmail("Work").WithEntityId(1));
            user.AddPhone(ValidPhone("work").WithEntityId(1));
            return user;
        }

        public static UserLoginInfo ValidUserLoginInfo(string name)
        {
            var userLoginInfo = new UserLoginInfo
            {
                Password = name,
                UserClientSettings = new UserClientSettings(),
                UserPreferenceses = new UserPreferences()
            };
            userLoginInfo.AddNewCurrentUserSubscription(new UserSubscription
                                                  {
                                                      EntityId = 1,
                                                      CreateDate = DateTime.Parse("1/5/1972"),
                                                      Current = true,
                                                      PortfolioSubscriptionLevel = new PortfolioSubscriptionLevel(),
                                                      Promotion = new Promotion()
                                                  });
            userLoginInfo.User = ValidUser(name);
            return userLoginInfo;
        }

        public static Phone ValidPhone(string tag)
        {
            return new Phone
                       {
                           PhoneNumber = "512.123.4567",
                           PhoneType = new PhoneType{Name = tag}
                       };
        }

        public static Email ValidEmail(string tag)
        {
            return new Email
                       {
                           EmailAddress = "rharik@decisioncritical.com",
                           EmailVerified = true,
                       };
        }

        public static Address ValidAddress(string tag)
        {
            return new Address
                       {
                           AddressType = new AddressType {Name = tag},
                           Address1 = "1706 Willow St",
                           City = "Austin",
                           State = "TX",
                       };
        }

        public static License ValidCredential_License(string name)
        {
            return new License
                       {
                           LicenseType = ValidLicenseType("Raif"),
                           Number = "123",
                           State = "Tx",
                           IssueDate = DateTime.Parse("1/5/1972"),
                           ExpirationDate = DateTime.Parse("1/5/2072"),
                           AssetType = new AssetType { Name = "License" }
                       };
        }

        public static LicenseType ValidLicenseType(string name)
        {
            return new LicenseType
                       {
                           Description = "some license",
                           Name = "License",
                           Grantor = "Allah",
                           CEProfession = "some profession"
                       };
        }

        public static Document ValidDocument(string name)
        {
            return new Document
                       {
                           Name = name,
                           Description = "some important doc",
                           FileUrl = "somewhere/document1.docx",
                           DocumentFileType =DocumentFileType.Document.ToString(),
                           DocumentCategory = ValidDocumentCategory("cat").WithEntityId(),
                           Size = 1000
                       };
        }

        public static DocumentCategory ValidDocumentCategory(string name)
        {
            return new DocumentCategory
                       {
                           Name = name,
                           Description = "some doc cat"
                       };
        }

        public static Education ValidEducation(string name)
        {
            return new Education
                       {
                           Name = "school name",
                           City = "austin",
                           State = "tx",
                           Country = "USA",
                           DegreeType = new DegreeType {Name = "college"},
                           EducationInProgress = true,
                           DegreeComments = "comments",
                           DegreeName = "animal husbandry",
                           FieldOfStudy = new FieldOfStudy {Name = "chimpanzees"},
                           FieldOfStudyOther = "some other",
                           AdvancedPracticeType = new AdvancedPracticeType {Name = "some practice type"},
                           DegreeDate= DateTime.Parse("1/1/1919"),
                           DegreeMajor = "zoo animals",
                           GPA = double.Parse("1.2"),
                           ClassRank = 100000,
                           CreditsPerSemester = 2,
                           CreditsPerQuarter = 2,
                           AssetType = new AssetType { Name = "Education" }

                       };
        }




        public static MilitaryService ValidMilitaryService(string name)
        {
            return new MilitaryService
                       {
                           Name = name,
                           Rank = "sarge",
                           Status = "Active",
                           ServiceBranch = new ServiceBranch {Name = "branch"},
                           Discharge = "gooey",
                           AssetType = new AssetType{Name = "MilitaryService"}
                       };
        }

        public static WorkHistory ValidWorkHistory(string name)
        {
            return new WorkHistory
                       {
                           Employer = "Mr Happy",
                           PayFrequency = new PayFrequency {Name = "weekly"},
                           Salary = 12,
                           AssetType = new AssetType{Name = "WorkHistory"}

                       };
        }

        
        public static Research ValidResearch(string name)
        {
            return new Research
                       {
                           Name = name,
                           Description = "Research Topic",
                           FundedBy = "NSNA",
                           AssetType = new AssetType { Name = "Research" }

                       };
        }

     

        public static Portfolio ValidPortfolio(string Name)
        {
            var portfolio = new Portfolio
                                {
                                    Description = "some test portfolio",
                                    Name = Name,
                                    User = ValidUser("raif")
                                };
            return portfolio;
        }


        public static StudentActivity ValidStudentActivity(string name)
        {
            return new StudentActivity
            {
                Name = name,
               Role = "Chief",
                User = ValidUser("Kev"),
                AssetType = new AssetType { Name = "StudentActivity" }
            };
        }
        public static Goal ValidGoal(string defaultDescription)
        {
            return new Goal
                           {
                               Name = "Masters Degree",
                               Description = defaultDescription,
                               GoalStatusComments = "status comment",
                               User = ValidUser("Frank"),
                               AssetType = new AssetType { Name = "Goal" }
                           };
        }
        public static Reflection ValidReflection(string name)
        {
            return new Reflection
            {
                Name = name,
                Description = "desc",
                User = ValidUser("Kev"),
                AssetType = new AssetType { Name = "Reflection" }
            };
        }
        public static Interview ValidInterview(string name)
        {
            return new Interview
            {
                Name = name,
                Description = "Valid Interview",
                InterviewedBy = "President",
                User = ValidUser("Frank"),
                AssetType = new AssetType { Name = "Interview" }
            };
        }
        
        public static Grant ValidGrant(string defaultDescription)
        {
            return new Grant
            {
                Name = "NSNA Grant",
                Description = defaultDescription,
                Reviewer = "NSNA",
                User = ValidUser("Dr Phil"),
                AssetType = new AssetType { Name = "Grant" }
            };
        }


        public static HealthPolicy ValidHealthPolicy(string defaultDescription)
        {
            return new HealthPolicy
            {
                Name = "CDC Law 837",
                City = "New York",
                Role = "Role 123",
                User = ValidUser("Dr Phil"),
                HealthPolicyType = new HealthPolicyType { Name = "HealthPolicy A" },
                AssetType = new AssetType { Name = "HealthPolicy" }

            };
        }


        public static ComplianceNotificationSchedule ValidComplianceNotificationSchedule()
        {
            var validUser = ValidUser("raif");
            var complianceNotificationSchedule = new ComplianceNotificationSchedule()
            {
                DayOfExpiration = true,
                DaysBeforeExpiration = 30,
                EndDaysAfterExpiration = 30,
                RepeatDaysAfterExpiration = 3,
                RepeatDaysUntilExpiration = 3,
            };
            complianceNotificationSchedule.AddEmail(validUser.Emails.FirstOrDefault());
            var complianceItem = new ComplianceItem { ComplianceItemType = ComplianceItemEnum.License.ToString() };
            complianceNotificationSchedule.AddComplianceItem(complianceItem);
            return complianceNotificationSchedule;
        }

        public static Review ValidReview(string defaultDescription)
        {
            return new Review
            {
                Name = "NSNA Grant",
                Description = defaultDescription,
               
                User = ValidUser("Dr Phil"),
                ReviewType = new ReviewType { Name = "Grant" },
                AssetType = new AssetType { Name = "Review" }

            };
        }

        public static Asset ValidAsset(string defaultName)
        {
            return new Asset
            {
                Name = defaultName,
                User = ValidUser("Dr Phil")
            };
        }


        public static CoverText ValidCoverText(string  defaultName)
        {
            return new CoverText
            {
                Name = defaultName,
                Text = "test cover text",
                CoverTextType = new CoverTextType
                {
                    Name = "Education"
                }
            };
        }

        public static License ValidLicense(string name)
        {
            return new License
                       {
                           AssetType = new AssetType {Name = "License"},
                           IssueDate = DateTime.Parse("1/5/1972"),
                           LicenseType = new LicenseType{Name = "my licesnse"}
                       };
        }

        public static NSNAMemberInfo ValidNSNAMemberInfo(string name)
        {
            return new NSNAMemberInfo
                       {
                           Address1 = "1706 Willow st",
                           City = "Austin",
                           State = "TX",
                           ZipCode = "12332",
                           EmailAddress = "rharik@decisioncritical.com",
                           FirstName = name,
                           LastName = "Harik"
                       };
        }
         public static Instructions ValidInstructions(string key)
         {
             return new Instructions
                        {
                            Key = key,
                            Text = "Some Instrucions"
                        };
         }

        public static ClinicalPracticum ValidClinicalPracticum(string name)
        {
            return new ClinicalPracticum
                       {
                           Name = name,
                           Description = "description"
                       };
        }

        public static AcademicProduct ValidAcademicProduct()
        {
            return new AcademicProduct
                       {
                           AcademicProductType = ValidAcademicProductType(),
                           AcademicProductLevel = ValidAcademicProductLevel()
                       };
        }
        public static AcademicProductType ValidAcademicProductType()
        {
            var APT = new AcademicProductType
                                               {
                                                   Name = "BSN"
                                               };
            APT.AddAcademicProductLevel(ValidAcademicProductLevel());
            return APT;
        }

        public static AcademicProductLevel ValidAcademicProductLevel()
        {
            return new AcademicProductLevel
                       {
                           Name = "Essential I",
                       };
        }


        public static Client ValidClient(string name)
        {
            return new Client
                       {
                           Name = name
                       };
        }

        public static Promotion ValidPromotion(string name)
        {
            return new Promotion
                       {
                           StartDate = DateTime.Parse("1/5/2000"),
                           EndDate = DateTime.Parse("1/5/2020"),
                           MonthsBeforeNextBilling = 12,
                           PromoCode = "bubba"
                       };
        }

        public static EmailTemplate ValidEmailTemplate(string name)
        {
            return new EmailTemplate
                       {
                           Name = name,
                           Description = "an email template",
                           Body = "<p>Hurray!</p>"
                       };
        }
    }
}