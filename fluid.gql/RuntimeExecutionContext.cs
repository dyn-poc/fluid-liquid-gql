using Newtonsoft.Json.Linq;

namespace fluid.gql;

public class RuntimeExecutionContext
{
    public AccountAccess Account;
    public GigyaSite Site;

}

public class GigyaSite
{
    public string ApiKey { get; set; }
    public string Domain { get; set; }


    public GigyaSite(string apiKey = null, string domain = "gigya.com")
    {
        ApiKey = apiKey;
        Domain = domain;
    }
    
    //schema 
}

// public record AccountAccess(JObject profile= null, JObject data = null, JObject prefernces= null, JObject subscribtions= null);


public class AccountAccess : JObject
{
    public AccountAccess(JObject profile= null, JObject data = null, JObject preferences= null, JObject subscriptions= null)
    {
       Add(nameof(profile), profile);
       Add(nameof(data), data);
       Add(nameof(preferences), preferences);
       Add(nameof(subscriptions), subscriptions);
    }
    
    
}

