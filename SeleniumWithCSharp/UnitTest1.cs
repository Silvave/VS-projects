using System;

namespace SeleniumWithCSharp
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;

    [TestClass]
    public class UnitTest1
    {
        private IWebDriver selenium;

        [TestInitialize]
        public void SetUp()
        {
            selenium = new FirefoxDriver();
        }

        [TestMethod]
        public void TestLogin_SuccessScenario_ValidUserNamePassword()
        {
            var validUsername = "testtest";
            var validPassword = "testtest";

            selenium.Navigate()
                .GoToUrl("https://softuni.bg");

            var loginLink = selenium
                .FindElement(
                    By.Id("loginLink")
                );

            loginLink.Click();

            Assert.AreEqual(
                "https://softuni.bg/account/authenticate",
                selenium.Url
            );

            var usernameField = selenium
                .FindElement(
                    By.Id("LoginUserName")
                );

            var passwordField = selenium
                .FindElement(
                    By.Id("LoginPassword")
                );

            usernameField.Clear();
            passwordField.Clear();

            usernameField.SendKeys(validUsername);
            passwordField.SendKeys(validPassword);

            var loginButton = selenium
                .FindElement(
                    By.XPath("//input[@value='Вход']")
                );

            loginButton.Click();

            Assert.AreEqual(
                "https://softuni.bg/users/profile/show",
                selenium.Url
            );

            var usernameDropDown = selenium
                .FindElement(
                    By.XPath("/html/body/header/div/div/div/div/nav/div/div[2]/form/ul/li[2]/a")
                );

            Assert.AreEqual(
                validUsername.ToUpper(), 
                usernameDropDown.Text.Trim()
            );

            
        }

        [TestMethod]
        public void TestLogin_ShouldFail_TooShortUserName()
        {

            Dictionary<string, string> fields = new Dictionary<string, string>()
            {
                {"LoginUserName", "dd"},
                {"LoginPassword", "b"},
            };

            Dictionary<string, string> errorFields = new Dictionary<string, string>();

            foreach (var field in fields)
            {
                errorFields.Add(
                    field.Key + "-error", 
                    "Потребителското име трябва да бъде между 5 и 32 символа."
                );
            }

 
            selenium.Navigate()
                .GoToUrl("https://softuni.bg");

            var loginLink = selenium
                .FindElement(
                    By.Id("loginLink")
                );

            loginLink.Click();

            Assert.AreEqual(
                "https://softuni.bg/account/authenticate",
                selenium.Url
            );

            foreach (var field in fields)
            {
                IWebElement currentField = 
                    selenium
                        .FindElement(
                            By.Id(field.Key)
                         );

                currentField.Clear();
                currentField.SendKeys(field.Value);
            }



            var dummyField = selenium.FindElements(By.ClassName("chkBox"));

            dummyField.FirstOrDefault().Click();

            Thread.Sleep(1000);

            foreach (var error in errorFields)
            {
                IWebElement errorElement =
                    selenium.FindElement(
                        By.Id(error.Key)
                        );

                Assert.AreEqual(
                    "Потребителското име трябва да бъде между 5 и 32 символа.",
                    errorElement.Text
                );
            }

            

        }

    }
}
