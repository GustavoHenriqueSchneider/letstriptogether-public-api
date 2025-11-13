using System.Globalization;
using FluentValidation;
using NUnit.Framework;

namespace Application.UnitTests;

[SetUpFixture]
public class GlobalSetup
{
    [OneTimeSetUp]
    public void SetUp()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en-US");
    }
}

