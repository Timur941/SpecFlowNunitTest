using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using TechTalk.SpecFlow;

namespace SpecFlowNunit.Actions
{
    [Binding, Scope(Feature = "Отображение графика")]
    public sealed class WolframPlotStepDefinitions : IDisposable
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver driver = new ChromeDriver();
        private readonly IConfigurationRoot config;
        private const int TIMEOUTSECONDS = 10;
        WebDriverWait wait;

        public WolframPlotStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            config = new ConfigurationBuilder().AddJsonFile("aliases.json").Build();
            driver.Manage().Window.Maximize();
            TimeSpan timeout = new TimeSpan(0, 0, TIMEOUTSECONDS);
            wait = new WebDriverWait(driver, timeout);
            //wait = driver.Manage().Timeouts().ImplicitWait;
        }

        public void Dispose()
        {
            driver.Dispose();
        }

        [StepDefinition(@"пользователь открыл страницу ""(.*)""")]
        public void OpenPage(string page)
        {
            string pageUrl = config[page];
            driver.Navigate().GoToUrl(pageUrl);
            Thread.Sleep(2000);
        }

        [StepDefinition(@"""(.*)"" присутствует на странице")]
        public void ElementOnPage(string elementXpath)
        {
            elementXpath = config[elementXpath];

            //IWebElement element = driver.FindElement(By.XPath(elementXpath));
            IWebElement element = wait.Until(d => d.FindElement(By.XPath(elementXpath)));
            Assert.IsTrue(element.Displayed);
            //Thread.Sleep(1000);
        }

        [StepDefinition(@"пользователь кликнул на кнопку ""(.*)""")]
        public void ButtonClick(string elementXpath)
        {
            elementXpath = config[elementXpath];
            IWebElement button = wait.Until(d => d.FindElement(By.XPath(elementXpath)));
            button.Click();
        }

        [StepDefinition(@"пользователь ввел ""(.*)"" в ""(.*)""")]
        public void InputText(string equation, string elementXpath)
        {
            elementXpath = config[elementXpath];
            //IWebElement inputElement = driver.FindElement(By.XPath(config[elementXpath]));
            IWebElement inputElement = wait.Until(d => d.FindElement(By.XPath(elementXpath)));
            //Thread.Sleep(500);
            //Assert.IsTrue(inputElement.Displayed);
            inputElement.SendKeys(config[equation]);
            //Thread.Sleep(1000);
        }

        [Then(@"""(.*)"" присутствуют на странице в количестве (\d)")]
        public void ElementsOnPage(string elementsXpath, int elementsCount)
        {
            //Thread.Sleep(2500);
            elementsXpath = config[elementsXpath];
            //ReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath(elementsXpath));
            
            IWebElement element = wait.Until(c => c.FindElement(By.XPath(elementsXpath)));

            //IWebElement elements = driver.FindElement(By.XPath(elementsXpath));
            //if (elements.Count != elementsCount)
            //    throw new NoSuchElementException($"Указанные элементы отсутствуют на странице (Элементов найдено: {elements.Count})");
            Thread.Sleep(1000);
        }
    }
}
