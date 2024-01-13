// See https://aka.ms/new-console-template for more information
using KeyGenerator;

Console.WriteLine("Hello, World!");


var result = KeyService.GenerateKey("IdreesTextile", DateTime.Now.AddMonths(1));

Console.WriteLine(result.SubscriptionKeyWithExpiration);

var date = KeyService.DecodeExpirationDate("1EA8B4953A6FFE45-MjAyMzEwMDg=");
var validationErrors = new List<string>();
validationErrors.Add("error");


var check = new RefCheck
{
    Error = validationErrors,
};
check.Error.Add("2nderror");
check.Error.Add("3rderror");

Console.WriteLine($"list {validationErrors.Count} classcount {check.Error.Count}");


Console.WriteLine(date.ToString());