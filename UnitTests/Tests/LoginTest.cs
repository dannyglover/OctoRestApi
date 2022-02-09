using System.Threading.Tasks;
using NUnit.Framework;
using OctoRestApi;
using UnitTests.Utils;

namespace UnitTests.Tests;

public class LoginTest
{
    private OctoApi OctoApiInstance { get; }

    public LoginTest()
    {
        OctoApiInstance = new OctoApi(CredentialsUtil.OctoprintUrl);
    }

    [SetUp]
    public void Setup()
    {
        
    }

    [Test]
    public async Task Test1()
    {
        // login
        await OctoApiInstance.Login(CredentialsUtil.Username, CredentialsUtil.Password);
        
        // get the response data model
        var loginResponseDataModel = OctoApiInstance.OctoDataModel.OctoLoginResponseDataModel;

        Assert.That(loginResponseDataModel != null);
        Assert.That(loginResponseDataModel?.Session != string.Empty);
    }
}