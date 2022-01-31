using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace SpecFlowNunit.Steps
{
    [Binding, Scope(Feature = "Проверка страницы с документаций IntelliJ IDEA и страницы авторизации")]
    public sealed class JetbrainsStepDefinitions : IDisposable
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver driver = new ChromeDriver();
        private readonly IConfigurationRoot config;

        public JetbrainsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            config = new ConfigurationBuilder().AddJsonFile("aliases.json").Build();
            driver.Manage().Window.Maximize();
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        [Given(@"пользователь открыл страницу ""(.*)""")]
        public void OpenPage(string page)
        {
            string pageUrl = config[page];
            driver.Navigate().GoToUrl(pageUrl);
            Thread.Sleep(2000);
        }

        [StepDefinition(@"""(.*)"" присутствует на странице")]
        public void ElementOnPage(string elementXpath)
        {
            try
            {
                elementXpath = config[elementXpath];
                IWebElement element = driver.FindElement(By.XPath(elementXpath));
                Assert.IsTrue(element.Displayed);
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TEST ERROR: {0}", ex.Message);
                driver.Close();
                driver.Quit();
            }
        }
        [StepDefinition(@"пользователь кликнул на кнопку ""(.*)""")]
        public void ButtonClick(string element)
        {
            try
            {
                string elementXpath = config[element];
                driver.FindElement(By.XPath(elementXpath)).Click();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TEST ERROR: {0}", ex.Message);
                driver.Close();
                driver.Quit();
            }
        }

        [StepDefinition(@"пользователь увидит следующие значения")]
        public void CheckValues(Table table)
        {
            try
            {
                Dictionary<string, string> actionShortcutsDict = new Dictionary<string, string>();
                foreach (var row in table.Rows)
                {
                    actionShortcutsDict.Add(row[0], row[1]);
                }
                ReadOnlyCollection<IWebElement> tableElements = driver.FindElements(By.XPath("//table[@id='34e971b0']//tbody//tr//td"));
                for (int i = 0; i < tableElements.Count; i += 2)
                {
                    string action = tableElements[i].Text;
                    string shortcut = tableElements[i + 1].Text;
                    Assert.IsTrue(action.Equals(actionShortcutsDict.ElementAt(i / 2).Key));
                    Assert.IsTrue(shortcut.Equals(actionShortcutsDict.ElementAt(i / 2).Value));
                }
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine("TEST ERROR: {0}", ex.Message);
                driver.Close();
                driver.Quit();
            }
        }

        [Given(@"пользователь логинится, вводя неправильные зачения")]
        public void Login(Table table)
        {
            //try
            //{
            IWebElement loginInput = driver.FindElement(By.XPath(config["Поле логина"]));
            IWebElement passInput = driver.FindElement(By.XPath(config["Поле пароля"]));
            IWebElement signInButton = driver.FindElement(By.XPath(config["Кнопка логина"]));
            Assert.IsTrue(loginInput.Displayed);
            Assert.IsTrue(passInput.Displayed);
            Assert.IsTrue(signInButton.Displayed);
            foreach (var credentials in table.Rows)
            {
                loginInput.Clear();
                passInput.Clear();
                string login = credentials[0];
                string pass = credentials[1];
                loginInput.SendKeys(login);
                passInput.SendKeys(pass);
                signInButton.Click();
                Thread.Sleep(200);
                Assert.IsTrue(driver.FindElement(By.XPath(config["Текст ошибки авторизации"])).Displayed);
                Thread.Sleep(1000);
            }

            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine("TEST ERROR: {0}", ex.Message);
            //}

        }

    }
}

