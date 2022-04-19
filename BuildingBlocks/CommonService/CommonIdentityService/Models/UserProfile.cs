namespace CommonIdentityService.IdentityService.Models
{
    public class UserProfile
    {
        string UserName {get;set;}
        string UserNickName{get;set;}
        string UserEmail {get;set;}
        string Name {get;set;}
        uint Age {get;set;}
        string Gender {get;set;}
        string School {get;set;}
        string Province {get;set;}
        string City {get;set;}
        string SelfDescription {get;set;}
        string Birthday {get;set;}
        //todo  string UserAvatarUrl=
        //todo string UserBackgroundImageUrl=
        string CreateTime {get;set;}
        public UserProfile(string userName,string userNickName,string userEmail,string name,uint age,string gender,string school,string province,string city,string selfDescription,string birthday,string createTime)
        {
            UserName=userName;
            UserNickName=userNickName;
            UserEmail=userEmail;
            Name=name;
            Age=age;
            Gender=gender;
            School=school;
            Province=province;
            City=city;
            SelfDescription=selfDescription;
            Birthday=birthday;
            CreateTime=createTime;
        }
    }
}
